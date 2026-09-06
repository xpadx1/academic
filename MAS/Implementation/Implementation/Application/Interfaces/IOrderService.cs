using Implementation.Application.DTOs;

namespace Implementation.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<OrderResponse> GetOrderAsync(
        int customerId,
        int orderId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<OrderResponse>> GetCustomerOrdersAsync(
        int customerId,
        CancellationToken cancellationToken = default);

    Task<OrderResponse> AddItemAsync(
        int customerId,
        int orderId,
        AddOrderItemRequest request,
        CancellationToken cancellationToken = default);

    Task<OrderResponse> UpdateItemQuantityAsync(
        int customerId,
        int orderId,
        int orderItemId,
        UpdateOrderItemQuantityRequest request,
        CancellationToken cancellationToken = default);

    Task<OrderResponse> RemoveItemAsync(
        int customerId,
        int orderId,
        int orderItemId,
        CancellationToken cancellationToken = default);

    Task<OrderResponse> CheckoutAsync(
        int customerId,
        int orderId,
        CheckoutRequest request,
        CancellationToken cancellationToken = default);

    Task<PaymentResponse> ProcessPaymentAsync(
        int customerId,
        int orderId,
        PaymentRequestDto request,
        CancellationToken cancellationToken = default);

    Task<OrderStatusResponse> GetOrderStatusAsync(
        int customerId,
        int orderId,
        CancellationToken cancellationToken = default);

    Task<OrderStatusResponse> ChangeOrderStatusAsync(
        int orderId,
        string newStatus,
        CancellationToken cancellationToken = default);
}