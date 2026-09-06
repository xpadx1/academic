using Implementation.Application.DTOs;

namespace Implementation.Application.Interfaces;

public interface IMenuService
{
    Task<IReadOnlyCollection<MenuResponse>> GetMenusAsync();

    Task<MenuResponse> GetMenuAsync(int menuId);

    Task<IReadOnlyCollection<MenuItemResponse>> GetMenuItemsAsync(int menuId);

    Task<IReadOnlyCollection<MenuItemResponse>> GetAllAvailableMenuItemsAsync();

    Task<MenuItemDetailsResponse> GetMenuItemDetailsAsync(int menuItemId);
}