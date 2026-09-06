using Implementation.Domain.Interfaces;

namespace Implementation.Domain.Entities;

public abstract class MenuItem
{
    private readonly List<Ingredient> _ingredients = [];

    protected MenuItem(
        string name,
        string? description,
        decimal basePrice,
        int calories)
    {
        Name = name;
        Description = description;
        BasePrice = basePrice;
        Calories = calories;
    }

    protected MenuItem()
    {
    }

    public int Id { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public decimal BasePrice { get; private set; }

    public int Calories { get; private set; }

    public int MenuId { get; private set; }

    public Menu Menu { get; private set; } = null!;

    public IReadOnlyCollection<Ingredient> Ingredients => _ingredients;

    public abstract decimal GetCurrentPrice(IDateTimeProvider dateTimeProvider);

    public abstract bool IsCurrentlyAvailable(IDateTimeProvider dateTimeProvider);

    public void AddIngredient(Ingredient ingredient)
    {
        if (!_ingredients.Contains(ingredient))
        {
            _ingredients.Add(ingredient);
        }
    }

    public void AssignToMenu(Menu menu)
    {
        Menu = menu;
    }
}