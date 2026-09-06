using Implementation.Application.DTOs;
using Implementation.Application.Exceptions;
using Implementation.Application.Interfaces;
using Implementation.Domain.Entities;
using Implementation.Domain.Interfaces;
using Implementation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Implementation.Application.Services;

public class MenuService : IMenuService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public MenuService(RestaurantDbContext dbContext, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<IReadOnlyCollection<MenuResponse>> GetMenusAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Menus
            .AsNoTracking()
            .OrderBy(m => m.Name)
            .Select(m => new MenuResponse(m.Id, m.Name, m.Description, m.IsAvailable))
            .ToListAsync(cancellationToken);
    }

    public async Task<MenuResponse> GetMenuAsync(int menuId, CancellationToken cancellationToken = default)
    {
        var menu = await _dbContext.Menus
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == menuId, cancellationToken)
            ?? throw new NotFoundException($"Menu with id {menuId} was not found.");

        return new MenuResponse(menu.Id, menu.Name, menu.Description, menu.IsAvailable);
    }

    public async Task<IReadOnlyCollection<MenuItemResponse>> GetMenuItemsAsync(
        int menuId,
        CancellationToken cancellationToken = default)
    {
        var menuExists = await _dbContext.Menus
            .AsNoTracking()
            .AnyAsync(m => m.Id == menuId, cancellationToken);

        if (!menuExists)
        {
            throw new NotFoundException($"Menu with id {menuId} was not found.");
        }

        var items = await _dbContext.MenuItems
            .AsNoTracking()
            .Where(i => i.MenuId == menuId)
            .ToListAsync(cancellationToken);

        return items
            .Where(i => i.IsCurrentlyAvailable(_dateTimeProvider))
            .Select(ToMenuItemResponse)
            .ToList();
    }

    public async Task<IReadOnlyCollection<MenuItemResponse>> GetAllAvailableMenuItemsAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await _dbContext.MenuItems
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return items
            .Where(i => i.IsCurrentlyAvailable(_dateTimeProvider))
            .Select(ToMenuItemResponse)
            .ToList();
    }

    public async Task<MenuItemDetailsResponse> GetMenuItemDetailsAsync(
        int menuItemId,
        CancellationToken cancellationToken = default)
    {
        var item = await _dbContext.MenuItems
            .AsNoTracking()
            .Include(i => i.Ingredients)
            .FirstOrDefaultAsync(i => i.Id == menuItemId, cancellationToken)
            ?? throw new NotFoundException($"Menu item with id {menuItemId} was not found.");

        return new MenuItemDetailsResponse(
            item.Id,
            item.Name,
            item.Description,
            item.GetCurrentPrice(_dateTimeProvider),
            item.Calories,
            GetItemType(item),
            item.IsCurrentlyAvailable(_dateTimeProvider),
            item.Ingredients
                .Select(i => new IngredientResponse(i.Id, i.Name, i.IsVegetarian, i.IsAllergen))
                .ToList());
    }

    private MenuItemResponse ToMenuItemResponse(MenuItem item) =>
        new(
            item.Id,
            item.Name,
            item.Description,
            item.GetCurrentPrice(_dateTimeProvider),
            item.Calories,
            GetItemType(item),
            item.IsCurrentlyAvailable(_dateTimeProvider));

    private static string GetItemType(MenuItem item) =>
        item switch
        {
            StandardItem => "Standard",
            SeasonalItem => "Seasonal",
            _ => "Unknown"
        };
}