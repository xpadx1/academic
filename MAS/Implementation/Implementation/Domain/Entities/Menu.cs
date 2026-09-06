namespace Implementation.Domain.Entities;

public class Menu
{
    private readonly List<MenuItem> _items = [];

    public Menu(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }

    protected Menu()
    {
    }

    public int Id { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public bool IsAvailable { get; private set; } = true;

    public IReadOnlyCollection<MenuItem> Items => _items;

    public void AddItem(MenuItem item)
    {
        if (_items.Contains(item))
        {
            return;
        }

        _items.Add(item);
        item.AssignToMenu(this);
    }

    public void SetAvailability(bool isAvailable) => IsAvailable = isAvailable;
}