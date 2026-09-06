using Implementation.Application.DTOs;
using Implementation.Application.Interfaces;
using Implementation.Domain.Entities;
using Implementation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Application.Services;

public class MenuService : IMenuService
{
    private readonly RestaurantDbContext _dbContext;

    public MenuService(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<MenuResponse>> GetMenusAsync()
    {
        return await _dbContext.Menus
            .AsNoTracking()
            .OrderBy(m => m.Name)
            .Select(m => new MenuResponse(m.Id, m.Name, m.Description, m.IsAvailable))
            .ToListAsync();
    }

    public async Task<MenuResponse> GetMenuAsync(int menuId)
    {
        var menu = await _dbContext.Menus
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == menuId);

        return new MenuResponse(menu.Id, menu.Name, menu.Description, menu.IsAvailable);
    }

    public async Task<IReadOnlyCollection<MenuItemResponse>> GetMenuItemsAsync(int menuId)
    {
        var items = await _dbContext.MenuItems
            .AsNoTracking()
            .Where(i => i.MenuId == menuId)
            .ToListAsync();

        return items
            .Where(i => i.IsCurrentlyAvailable())
            .Select(ToMenuItemResponse)
            .ToList();
    }

    public async Task<IReadOnlyCollection<MenuItemResponse>> GetAllAvailableMenuItemsAsync()
    {
        var items = await _dbContext.MenuItems
            .AsNoTracking()
            .ToListAsync();

        return items
            .Where(i => i.IsCurrentlyAvailable())
            .Select(ToMenuItemResponse)
            .ToList();
    }

    public async Task<MenuItemDetailsResponse> GetMenuItemDetailsAsync(int menuItemId)
    {
        var item = await _dbContext.MenuItems
            .AsNoTracking()
            .Include(i => i.Ingredients)
            .FirstOrDefaultAsync(i => i.Id == menuItemId);

        return new MenuItemDetailsResponse(
            item.Id,
            item.Name,
            item.Description,
            item.GetCurrentPrice(),
            item.Calories,
            GetItemType(item),
            item.IsCurrentlyAvailable(),
            item.Ingredients
                .Select(i => new IngredientResponse(i.Id, i.Name, i.IsVegetarian, i.IsAllergen))
                .ToList());
    }

    private MenuItemResponse ToMenuItemResponse(MenuItem item) =>
        new(
            item.Id,
            item.Name,
            item.Description,
            item.GetCurrentPrice(),
            item.Calories,
            GetItemType(item),
            item.IsCurrentlyAvailable());

    private static string GetItemType(MenuItem item) =>
        item switch
        {
            StandardItem => "Standard",
            SeasonalItem => "Seasonal",
            _ => "Unknown"
        };
}