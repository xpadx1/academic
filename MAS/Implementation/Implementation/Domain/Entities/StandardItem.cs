using Implementation.Domain.Interfaces;

namespace Implementation.Domain.Entities;

public class StandardItem : MenuItem
{
    public StandardItem(
        string name,
        string? description,
        decimal basePrice,
        int calories,
        bool isAvailable = true)
        : base(name, description, basePrice, calories)
    {
        IsAvailable = isAvailable;
    }

    protected StandardItem()
    {
    }

    public bool IsAvailable { get; private set; }

    public void SetAvailability(bool isAvailable) => IsAvailable = isAvailable;

    public override decimal GetCurrentPrice(IDateTimeProvider dateTimeProvider) => BasePrice;

    public override bool IsCurrentlyAvailable(IDateTimeProvider dateTimeProvider) => IsAvailable;
}