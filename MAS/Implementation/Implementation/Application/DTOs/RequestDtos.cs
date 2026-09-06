using Implementation.Application.Interfaces;

namespace Implementation.Application.DTOs;

public record AddOrderItemRequest(int MenuItemId, int Quantity);

public record UpdateOrderItemQuantityRequest(int Quantity);

public record CheckoutRequest;

public record PaymentRequestDto(PaymentMethodType Method);

public record ChangeOrderStatusRequest(string NewStatus);
