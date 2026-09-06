using Implementation.Domain.Enums;

namespace Implementation.Domain.Entities;

public class Order
{
    private static readonly Dictionary<StatusType, StatusType[]> AllowedTransitions = new()
    {
        [StatusType.Created] = [StatusType.Accepted, StatusType.Cancelled],
        [StatusType.Accepted] = [StatusType.Preparing, StatusType.Cancelled],
        [StatusType.Preparing] = [StatusType.Ready, StatusType.Cancelled],
        [StatusType.Ready] = [StatusType.Completed],
        [StatusType.Completed] = [],
        [StatusType.Cancelled] = []
    };

    private readonly List<OrderItem> _items = [];

    public Order(Customer customer, DateTime createdAt)
    {
        Customer = customer;
        CreatedAt = createdAt;
        Status = StatusType.Created;
    }

    protected Order()
    {
    }

    public int Id { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public StatusType Status { get; private set; }

    public int CustomerId { get; private set; }

    public Customer Customer { get; private set; } = null!;

    public int? WaiterId { get; private set; }

    public Waiter? Waiter { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public string? PaymentMethod { get; private set; }

    public string? PaymentReference { get; private set; }

    public bool IsPaid { get; private set; }

    public decimal GetFinalPrice() => _items.Sum(item => item.GetTotalPrice());

    public OrderItem AddItem(MenuItem menuItem, int quantity, decimal unitPrice)
    {
        var existing = _items.FirstOrDefault(item => item.MenuItemId == menuItem.Id);
        if (existing is not null)
        {
            existing.UpdateQuantity(existing.Quantity + quantity);
            return existing;
        }

        var orderItem = new OrderItem(this, menuItem, quantity, unitPrice);
        _items.Add(orderItem);
        return orderItem;
    }

    public void RemoveItem(OrderItem orderItem)
    {
        _items.Remove(orderItem);
    }

    public void ChangeStatus(StatusType newStatus)
    {
        if (!AllowedTransitions[Status].Contains(newStatus))
        {
            throw new InvalidOperationException(
                $"Invalid status transition from {Status} to {newStatus}.");
        }

        Status = newStatus;
    }

    public void Accept()
    {
        ChangeStatus(StatusType.Accepted);
    }

    public void RecordPayment(string method, string reference)
    {
        if (IsPaid)
        {
            throw new InvalidOperationException("Order has already been paid.");
        }

        PaymentMethod = method;
        PaymentReference = reference;
        IsPaid = true;
    }

    public void Cancel()
    {
        if (Status is StatusType.Completed or StatusType.Cancelled)
        {
            throw new InvalidOperationException($"Order cannot be cancelled from status {Status}.");
        }

        Status = StatusType.Cancelled;
    }
}