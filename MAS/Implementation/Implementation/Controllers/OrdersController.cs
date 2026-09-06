using Implementation.Application.DTOs;
using Implementation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Implementation.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("{orderId:int}/status")]
    public async Task<ActionResult<OrderStatusResponse>> ChangeStatus(
        int orderId,
        [FromBody] ChangeOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        var status = await _orderService.ChangeOrderStatusAsync(orderId, request.NewStatus, cancellationToken);
        return Ok(status);
    }
}