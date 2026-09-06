using Implementation.Application.DTOs;

namespace Implementation.Application.Interfaces;

public interface IMenuService
{
    Task<IReadOnlyCollection<MenuResponse>> GetMenusAsync(CancellationToken cancellationToken = default);

    Task<MenuResponse> GetMenuAsync(int menuId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MenuItemResponse>> GetMenuItemsAsync(
        int menuId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<MenuItemResponse>> GetAllAvailableMenuItemsAsync(
        CancellationToken cancellationToken = default);

    Task<MenuItemDetailsResponse> GetMenuItemDetailsAsync(
        int menuItemId,
        CancellationToken cancellationToken = default);
}