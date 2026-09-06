using Implementation.Application.DTOs;
using Implementation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Implementation.Controllers;

[ApiController]
[Route("api/customers/{customerId:int}")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IOrderService _orderService;

    public CustomersController(ICustomerService customerService, IOrderService orderService)
    {
        _customerService = customerService;
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<CustomerResponse>> GetCustomer(
        int customerId,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerAsync(customerId, cancellationToken);
        return Ok(customer);
    }

    [HttpGet("orders")]
    public async Task<ActionResult<IReadOnlyCollection<OrderResponse>>> GetCustomerOrders(
        int customerId,
        CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetCustomerOrdersAsync(customerId, cancellationToken);
        return Ok(orders);
    }

    [HttpPost("orders")]
    public async Task<ActionResult<OrderResponse>> CreateOrder(
        int customerId,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.CreateOrderAsync(customerId, cancellationToken);
        return CreatedAtAction(
            nameof(GetCustomerOrder),
            new { customerId, orderId = order.Id },
            order);
    }

    [HttpGet("orders/{orderId:int}")]
    public async Task<ActionResult<OrderResponse>> GetCustomerOrder(
        int customerId,
        int orderId,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.GetOrderAsync(customerId, orderId, cancellationToken);
        return Ok(order);
    }

    [HttpPost("orders/{orderId:int}/items")]
    public async Task<ActionResult<OrderResponse>> AddOrderItem(
        int customerId,
        int orderId,
        [FromBody] AddOrderItemRequest request,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.AddItemAsync(customerId, orderId, request, cancellationToken);
        return Ok(order);
    }

    [HttpPatch("orders/{orderId:int}/items/{orderItemId:int}")]
    public async Task<ActionResult<OrderResponse>> UpdateOrderItemQuantity(
        int customerId,
        int orderId,
        int orderItemId,
        [FromBody] UpdateOrderItemQuantityRequest request,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.UpdateItemQuantityAsync(
            customerId, orderId, orderItemId, request, cancellationToken);
        return Ok(order);
    }

    [HttpDelete("orders/{orderId:int}/items/{orderItemId:int}")]
    public async Task<ActionResult<OrderResponse>> RemoveOrderItem(
        int customerId,
        int orderId,
        int orderItemId,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.RemoveItemAsync(customerId, orderId, orderItemId, cancellationToken);
        return Ok(order);
    }

    [HttpPost("orders/{orderId:int}/checkout")]
    public async Task<ActionResult<OrderResponse>> Checkout(
        int customerId,
        int orderId,
        [FromBody] CheckoutRequest request,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.CheckoutAsync(customerId, orderId, request, cancellationToken);
        return Ok(order);
    }

    [HttpPost("orders/{orderId:int}/payment")]
    public async Task<ActionResult<PaymentResponse>> ProcessPayment(
        int customerId,
        int orderId,
        [FromBody] PaymentRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _orderService.ProcessPaymentAsync(customerId, orderId, request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("orders/{orderId:int}/status")]
    public async Task<ActionResult<OrderStatusResponse>> GetOrderStatus(
        int customerId,
        int orderId,
        CancellationToken cancellationToken)
    {
        var status = await _orderService.GetOrderStatusAsync(customerId, orderId, cancellationToken);
        return Ok(status);
    }
}