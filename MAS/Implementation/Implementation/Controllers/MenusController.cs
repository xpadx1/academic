using Implementation.Application.DTOs;
using Implementation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Implementation.Controllers;

[ApiController]
[Route("api/menus")]
public class MenusController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<MenuResponse>>> GetMenus(
        CancellationToken cancellationToken)
    {
        var menus = await _menuService.GetMenusAsync(cancellationToken);
        return Ok(menus);
    }

    [HttpGet("{menuId:int}")]
    public async Task<ActionResult<MenuResponse>> GetMenu(int menuId, CancellationToken cancellationToken)
    {
        var menu = await _menuService.GetMenuAsync(menuId, cancellationToken);
        return Ok(menu);
    }

    [HttpGet("{menuId:int}/items")]
    public async Task<ActionResult<IReadOnlyCollection<MenuItemResponse>>> GetMenuItems(
        int menuId,
        CancellationToken cancellationToken)
    {
        var items = await _menuService.GetMenuItemsAsync(menuId, cancellationToken);
        return Ok(items);
    }
}