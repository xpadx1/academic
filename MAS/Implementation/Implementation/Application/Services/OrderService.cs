using Implementation.Application.DTOs;
using Implementation.Application.Exceptions;
using Implementation.Application.Interfaces;
using Implementation.Domain.Entities;
using Implementation.Domain.Enums;
using Implementation.Domain.Interfaces;
using Implementation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Application.Services;

public class OrderService : IOrderService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IPaymentService _paymentService;

    public OrderService(
        RestaurantDbContext dbContext,
        IDateTimeProvider dateTimeProvider,
        IPaymentService paymentService)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _paymentService = paymentService;
    }

    public async Task<OrderResponse> CreateOrderAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        var customer = await GetCustomerWithOrdersAsync(customerId, cancellationToken);

        var order = customer.PlaceOrder(_dateTimeProvider);
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await BuildOrderResponseAsync(order.Id, customerId, cancellationToken);
    }

    public async Task<OrderResponse> GetOrderAsync(
        int customerId,
        int orderId,
        CancellationToken cancellationToken = default)
    {
        return await BuildOrderResponseAsync(orderId, customerId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<OrderResponse>> GetCustomerOrdersAsync(
        int customerId,
        CancellationToken cancellationToken = default)
    {
        await EnsureCustomerExistsAsync(customerId, cancellationToken);

        var orders = await LoadOrdersForCustomerAsync(customerId, cancellationToken);

        return orders.Select(ToResponse).ToList();
    }

    public async Task<OrderResponse> AddItemAsync(
        int customerId,
        int orderId,
        AddOrderItemRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
        {
            throw new BusinessRuleException("Quantity must be greater than zero.");
        }

        var order = await GetTrackedCustomerOrderAsync(customerId, orderId, cancellationToken);

        if (order.Status != StatusType.Created)
        {
            throw new ConflictException(
                $"Items can only be added to an order in status {StatusType.Created}.");
        }

        var menuItem = await _dbContext.MenuItems
            .FirstOrDefaultAsync(i => i.Id == request.MenuItemId, cancellationToken)
            ?? throw new NotFoundException($"Menu item with id {request.MenuItemId} was not found.");

        if (!menuItem.IsCurrentlyAvailable(_dateTimeProvider))
        {
            throw new ConflictException($"Menu item '{menuItem.Name}' is currently unavailable.");
        }

        var unitPrice = menuItem.GetCurrentPrice(_dateTimeProvider);
        order.AddItem(menuItem, request.Quantity, unitPrice);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await BuildOrderResponseAsync(orderId, customerId, cancellationToken);
    }

    public async Task<OrderResponse> UpdateItemQuantityAsync(
        int customerId,
        int orderId,
        int orderItemId,
        UpdateOrderItemQuantityRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request.Quantity <= 0)
        {
            throw new BusinessRuleException("Quantity must be greater than zero.");
        }

        var order = await GetTrackedCustomerOrderAsync(customerId, orderId, cancellationToken);

        if (order.Status != StatusType.Created)
        {
            throw new ConflictException(
                $"Order items can only be modified while the order is in status {StatusType.Created}.");
        }

        var orderItem = order.Items.FirstOrDefault(i => i.Id == orderItemId)
            ?? throw new NotFoundException($"Order item with id {orderItemId} was not found in order {orderId}.");

        orderItem.UpdateQuantity(request.Quantity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await BuildOrderResponseAsync(orderId, customerId, cancellationToken);
    }

    public async Task<OrderResponse> RemoveItemAsync(
        int customerId,
        int orderId,
        int orderItemId,
        CancellationToken cancellationToken = default)
    {
        var order = await GetTrackedCustomerOrderAsync(customerId, orderId, cancellationToken);

        if (order.Status != StatusType.Created)
        {
            throw new ConflictException(
                $"Order items can only be removed while the order is in status {StatusType.Created}.");
        }

        var orderItem = order.Items.FirstOrDefault(i => i.Id == orderItemId)
            ?? throw new NotFoundException($"Order item with id {orderItemId} was not found in order {orderId}.");

        order.RemoveItem(orderItem);
        _dbContext.OrderItems.Remove(orderItem);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await BuildOrderResponseAsync(orderId, customerId, cancellationToken);
    }

    public async Task<OrderResponse> CheckoutAsync(
        int customerId,
        int orderId,
        CheckoutRequest request,
        CancellationToken cancellationToken = default)
    {
        var order = await GetCustomerOrderAsync(customerId, orderId, cancellationToken);

        ValidateCheckout(order);

        return await BuildOrderResponseAsync(orderId, customerId, cancellationToken);
    }

public async Task<PaymentResponse> ProcessPaymentAsync(
        int customerId,
        int orderId,
        PaymentRequestDto request,
        CancellationToken cancellationToken = default)
    {
        var order = await GetTrackedCustomerOrderAsync(customerId, orderId, cancellationToken);

        if (order.IsPaid)
        {
            throw new ConflictException("Order has already been paid.");
        }

        if (order.Status != StatusType.Created)
        {
            throw new ConflictException(
                $"Payment can only be processed for an order in status {StatusType.Created}.");
        }

        ValidateCheckout(order);

        var paymentAmount = order.GetFinalPrice();

        var paymentResult = await _paymentService.ProcessPaymentAsync(
            new PaymentRequest(request.Method, paymentAmount),
            cancellationToken);

        if (!paymentResult.Success)
        {
            throw new PaymentFailedException(
                paymentResult.ErrorMessage ?? "Payment processing failed.");
        }

        order.RecordPayment(request.Method.ToString(), paymentResult.Reference!);
        order.Accept();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new PaymentResponse(
            true,
            paymentResult.Reference,
            null,
            order.Status.ToString());
    }

    public async Task<OrderStatusResponse> GetOrderStatusAsync(
        int customerId,
        int orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await GetCustomerOrderAsync(customerId, orderId, cancellationToken);

        return new OrderStatusResponse(order.Id, order.Status.ToString(), order.CreatedAt);
    }

    public async Task<OrderStatusResponse> ChangeOrderStatusAsync(
        int orderId,
        string newStatus,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.TryParse<StatusType>(newStatus, ignoreCase: true, out var target))
        {
            throw new BusinessRuleException($"'{newStatus}' is not a valid order status.");
        }

        var order = await _dbContext.Orders
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken)
            ?? throw new NotFoundException($"Order with id {orderId} was not found.");

        try
        {
            order.ChangeStatus(target);
        }
        catch (InvalidOperationException ex)
        {
            throw new ConflictException(ex.Message);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new OrderStatusResponse(order.Id, order.Status.ToString(), order.CreatedAt);
    }

    private async Task<OrderResponse> BuildOrderResponseAsync(
        int orderId,
        int customerId,
        CancellationToken cancellationToken)
    {
        var orders = await LoadOrdersForCustomerAsync(customerId, cancellationToken);
        var order = orders.FirstOrDefault(o => o.Id == orderId)
            ?? throw new NotFoundException($"Order with id {orderId} was not found for customer {customerId}.");

        return ToResponse(order);
    }

private async Task<IReadOnlyList<Order>> LoadOrdersForCustomerAsync(
        int customerId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .Include(o => o.Customer)
            .ToListAsync(cancellationToken);
    }

    private async Task<Order> GetCustomerOrderAsync(
        int customerId,
        int orderId,
        CancellationToken cancellationToken)
    {
        var orders = await LoadOrdersForCustomerAsync(customerId, cancellationToken);
        return orders.FirstOrDefault(o => o.Id == orderId)
            ?? throw new NotFoundException($"Order with id {orderId} was not found for customer {customerId}.");
    }

private async Task<Order> GetTrackedCustomerOrderAsync(
        int customerId,
        int orderId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Orders
            .Where(o => o.CustomerId == customerId && o.Id == orderId)
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException($"Order with id {orderId} was not found for customer {customerId}.");
    }

    private async Task<Customer> GetCustomerWithOrdersAsync(
        int customerId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken)
            ?? throw new NotFoundException($"Customer with id {customerId} was not found.");
    }

    private async Task EnsureCustomerExistsAsync(int customerId, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Customers
            .AsNoTracking()
            .AnyAsync(c => c.Id == customerId, cancellationToken);

        if (!exists)
        {
            throw new NotFoundException($"Customer with id {customerId} was not found.");
        }
    }

    private void ValidateCheckout(Order order)
    {
        if (order.Status != StatusType.Created)
        {
            throw new ConflictException(
                $"Only orders in status {StatusType.Created} can be checked out.");
        }

        if (!order.Items.Any())
        {
            throw new BusinessRuleException("Cannot checkout an empty order.");
        }

        foreach (var orderItem in order.Items)
        {
            if (!orderItem.MenuItem.IsCurrentlyAvailable(_dateTimeProvider))
            {
                throw new ConflictException(
                    $"Menu item '{orderItem.MenuItem.Name}' is no longer available.");
            }

            if (orderItem.Quantity <= 0)
            {
                throw new BusinessRuleException(
                    $"Order item '{orderItem.MenuItem.Name}' has an invalid quantity.");
            }
        }
    }

    private OrderResponse ToResponse(Order order) =>
        OrderResponseMapper.ToOrderResponse(order);
}
