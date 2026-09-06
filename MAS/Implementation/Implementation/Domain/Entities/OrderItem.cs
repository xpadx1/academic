namespace Implementation.Domain.Entities;

public class OrderItem
{
    public OrderItem(Order order, MenuItem menuItem, int quantity, decimal unitPrice)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        Order = order;
        MenuItem = menuItem;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    protected OrderItem()
    {
    }

    public int Id { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public int OrderId { get; private set; }

    public Order Order { get; private set; } = null!;

    public int MenuItemId { get; private set; }

    public MenuItem MenuItem { get; private set; } = null!;

    public decimal GetTotalPrice() => Quantity * UnitPrice;

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        }

        Quantity = quantity;
    }
}