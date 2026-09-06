using Implementation.Application.DTOs;
using Implementation.Application.Interfaces;
using Implementation.Domain.Entities;
using Implementation.Domain.Enums;
using Implementation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Application.Services;

public class OrderService : IOrderService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IPaymentService _paymentService;

    public OrderService(
        RestaurantDbContext dbContext,
        IPaymentService paymentService)
    {
        _dbContext = dbContext;
        _paymentService = paymentService;
    }

    public async Task<OrderResponse> CreateOrderAsync(int customerId)
    {
        var customer = await GetCustomerWithOrdersAsync(customerId);

        var order = customer.PlaceOrder();
        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync();

        return await BuildOrderResponseAsync(order.Id, customerId);
    }

    public async Task<OrderResponse> GetOrderAsync(int customerId, int orderId)
    {
        return await BuildOrderResponseAsync(orderId, customerId);
    }

    public async Task<IReadOnlyCollection<OrderResponse>> GetCustomerOrdersAsync(int customerId)
    {
        var orders = await LoadOrdersForCustomerAsync(customerId);

        return orders.Select(ToResponse).ToList();
    }

    public async Task<OrderResponse> AddItemAsync(
        int customerId,
        int orderId,
        AddOrderItemRequest request)
    {
        var order = await GetTrackedCustomerOrderAsync(customerId, orderId);

        var menuItem = await _dbContext.MenuItems
            .FirstOrDefaultAsync(i => i.Id == request.MenuItemId);

        var unitPrice = menuItem.GetCurrentPrice();
        order.AddItem(menuItem, request.Quantity, unitPrice);

        await _dbContext.SaveChangesAsync();

        return await BuildOrderResponseAsync(orderId, customerId);
    }

    public async Task<OrderResponse> UpdateItemQuantityAsync(
        int customerId,
        int orderId,
        int orderItemId,
        UpdateOrderItemQuantityRequest request)
    {
        var order = await GetTrackedCustomerOrderAsync(customerId, orderId);

        var orderItem = order.Items.FirstOrDefault(i => i.Id == orderItemId);

        orderItem.UpdateQuantity(request.Quantity);

        await _dbContext.SaveChangesAsync();

        return await BuildOrderResponseAsync(orderId, customerId);
    }

    public async Task<OrderResponse> RemoveItemAsync(int customerId, int orderId, int orderItemId)
    {
        var order = await GetTrackedCustomerOrderAsync(customerId, orderId);

        var orderItem = order.Items.FirstOrDefault(i => i.Id == orderItemId);

        order.RemoveItem(orderItem);
        _dbContext.OrderItems.Remove(orderItem);

        await _dbContext.SaveChangesAsync();

        return await BuildOrderResponseAsync(orderId, customerId);
    }

    public async Task<OrderResponse> CheckoutAsync(int customerId, int orderId, CheckoutRequest request)
    {
        var order = await GetCustomerOrderAsync(customerId, orderId);

        return await BuildOrderResponseAsync(orderId, customerId);
    }

    public async Task<PaymentResponse> ProcessPaymentAsync(
        int customerId,
        int orderId,
        PaymentRequestDto request)
    {
        var reference = $"PAY-{Guid.NewGuid():N}".ToUpperInvariant();
        return new PaymentResponse(true, reference, null, "Accepted");
    }

    public async Task<OrderStatusResponse> GetOrderStatusAsync(int customerId, int orderId)
    {
        var order = await GetCustomerOrderAsync(customerId, orderId);

        return new OrderStatusResponse(order.Id, order.Status.ToString(), order.CreatedAt);
    }

    private async Task<OrderResponse> BuildOrderResponseAsync(int orderId, int customerId)
    {
        var orders = await LoadOrdersForCustomerAsync(customerId);
        var order = orders.FirstOrDefault(o => o.Id == orderId);

        return ToResponse(order);
    }

    private async Task<IReadOnlyList<Order>> LoadOrdersForCustomerAsync(int customerId)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .Include(o => o.Customer)
            .ToListAsync();
    }

    private async Task<Order> GetCustomerOrderAsync(int customerId, int orderId)
    {
        var orders = await LoadOrdersForCustomerAsync(customerId);
        return orders.FirstOrDefault(o => o.Id == orderId);
    }

    private async Task<Order> GetTrackedCustomerOrderAsync(int customerId, int orderId)
    {
        return await _dbContext.Orders
            .Where(o => o.CustomerId == customerId && o.Id == orderId)
            .Include(o => o.Items)
                .ThenInclude(i => i.MenuItem)
            .Include(o => o.Customer)
            .FirstOrDefaultAsync();
    }

    private async Task<Customer> GetCustomerWithOrdersAsync(int customerId)
    {
        return await _dbContext.Customers
            .Include(c => c.Orders)
            .FirstOrDefaultAsync(c => c.Id == customerId);
    }

    private OrderResponse ToResponse(Order order) =>
        OrderResponseMapper.ToOrderResponse(order);
}