using Implementation.Domain.Enums;

namespace Implementation.Domain.Entities;

public class Customer : Person
{
    private readonly List<Order> _orders = [];

    public Customer(
        string firstName,
        string lastName,
        string email,
        GenderType gender,
        DateTime memberSinceDate,
        string? phoneNumber = null)
        : base(firstName, lastName, email, gender, phoneNumber)
    {
        MemberSinceDate = memberSinceDate;
    }

    protected Customer()
    {
    }

    public DateTime MemberSinceDate { get; private set; }

    public IReadOnlyCollection<Order> Orders => _orders;

    public Order PlaceOrder()
    {
        var order = new Order(this, DateTime.UtcNow);
        _orders.Add(order);
        return order;
    }

    public int GetOrdersAmount() => _orders.Count;
}
