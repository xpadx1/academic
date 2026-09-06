namespace Implementation.Application.DTOs;

public record MenuResponse(int Id, string Name, string? Description, bool IsAvailable);

public record IngredientResponse(int Id, string Name, bool IsVegetarian, bool IsAllergen);

public record MenuItemResponse(
    int Id,
    string Name,
    string? Description,
    decimal CurrentPrice,
    int Calories,
    string ItemType,
    bool IsAvailable);

public record MenuItemDetailsResponse(
    int Id,
    string Name,
    string? Description,
    decimal CurrentPrice,
    int Calories,
    string ItemType,
    bool IsAvailable,
    IReadOnlyCollection<IngredientResponse> Ingredients);

public record CustomerResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string Gender,
    DateTime MemberSinceDate,
    int OrdersAmount);

public record OrderItemResponse(
    int Id,
    int MenuItemId,
    string MenuItemName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);

public record OrderResponse(
    int Id,
    DateTime CreatedAt,
    string Status,
    IReadOnlyCollection<OrderItemResponse> Items,
    decimal Subtotal,
    decimal FinalPayableAmount,
    bool IsPaid,
    string? PaymentMethod);

public record OrderStatusResponse(int OrderId, string Status, DateTime CreatedAt);

public record PaymentResponse(
    bool Success,
    string? Reference,
    string? ErrorMessage,
    string OrderStatus);