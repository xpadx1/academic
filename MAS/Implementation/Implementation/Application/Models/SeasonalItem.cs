namespace Implementation.Domain.Entities;

public class SeasonalItem : MenuItem
{
    public SeasonalItem(
        string name,
        string? description,
        decimal basePrice,
        int calories,
        DateTime seasonStart,
        DateTime seasonEnd,
        decimal seasonalPriceModifier)
        : base(name, description, basePrice, calories)
    {
        if (seasonEnd < seasonStart)
        {
            throw new ArgumentException("Season end cannot be earlier than season start.");
        }

        SeasonStart = seasonStart;
        SeasonEnd = seasonEnd;
        SeasonalPriceModifier = seasonalPriceModifier;
    }

    protected SeasonalItem()
    {
    }

    public DateTime SeasonStart { get; private set; }

    public DateTime SeasonEnd { get; private set; }

    public decimal SeasonalPriceModifier { get; private set; }

    public override bool IsCurrentlyAvailable()
    {
        var today = DateTime.UtcNow.Date;
        return today >= SeasonStart.Date && today <= SeasonEnd.Date;
    }

    public override decimal GetCurrentPrice() =>
        IsCurrentlyAvailable()
            ? decimal.Round(BasePrice * SeasonalPriceModifier, 2)
            : BasePrice;
}
