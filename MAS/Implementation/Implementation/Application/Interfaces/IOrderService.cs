using Implementation.Application.DTOs;

namespace Implementation.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(int customerId);

    Task<OrderResponse> GetOrderAsync(int customerId, int orderId);

    Task<IReadOnlyCollection<OrderResponse>> GetCustomerOrdersAsync(int customerId);

    Task<OrderResponse> AddItemAsync(int customerId, int orderId, AddOrderItemRequest request);

    Task<OrderResponse> UpdateItemQuantityAsync(
        int customerId,
        int orderId,
        int orderItemId,
        UpdateOrderItemQuantityRequest request);

    Task<OrderResponse> RemoveItemAsync(int customerId, int orderId, int orderItemId);

    Task<OrderResponse> CheckoutAsync(int customerId, int orderId, CheckoutRequest request);

    Task<PaymentResponse> ProcessPaymentAsync(int customerId, int orderId, PaymentRequestDto request);

    Task<OrderStatusResponse> GetOrderStatusAsync(int customerId, int orderId);
}
