namespace Implementation.Domain.Entities;

public class Ingredient
{
    public Ingredient(string name, bool isVegetarian, bool isAllergen)
    {
        Name = name;
        IsVegetarian = isVegetarian;
        IsAllergen = isAllergen;
    }

    protected Ingredient()
    {
    }

    public int Id { get; private set; }

    public string Name { get; private set; }

    public bool IsVegetarian { get; private set; }

    public bool IsAllergen { get; private set; }
}