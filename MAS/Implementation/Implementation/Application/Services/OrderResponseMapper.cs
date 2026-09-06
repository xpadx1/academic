using Implementation.Application.DTOs;
using Implementation.Domain.Entities;

namespace Implementation.Application.Services;

public static class OrderResponseMapper
{
    public static OrderResponse ToOrderResponse(Order order)
    {
        var subtotal = order.GetFinalPrice();

        return new OrderResponse(
            order.Id,
            order.CreatedAt,
            order.Status.ToString(),
            order.Items.Select(ToOrderItemResponse).ToList(),
            subtotal,
            subtotal,
            order.IsPaid,
            order.PaymentMethod);
    }

    public static OrderItemResponse ToOrderItemResponse(OrderItem item) =>
        new(
            item.Id,
            item.MenuItemId,
            item.MenuItem.Name,
            item.Quantity,
            item.UnitPrice,
            item.GetTotalPrice());
}
