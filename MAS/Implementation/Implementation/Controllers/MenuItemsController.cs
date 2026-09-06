using Implementation.Application.DTOs;
using Implementation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Implementation.Controllers;

[ApiController]
[Route("api/menu-items")]
public class MenuItemsController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuItemsController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<MenuItemResponse>>> GetAvailableItems(
        CancellationToken cancellationToken)
    {
        var items = await _menuService.GetAllAvailableMenuItemsAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{menuItemId:int}")]
    public async Task<ActionResult<MenuItemDetailsResponse>> GetItemDetails(
        int menuItemId,
        CancellationToken cancellationToken)
    {
        var item = await _menuService.GetMenuItemDetailsAsync(menuItemId, cancellationToken);
        return Ok(item);
    }
}