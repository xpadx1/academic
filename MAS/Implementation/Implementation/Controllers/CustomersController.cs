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
    public async Task<ActionResult<CustomerResponse>> GetCustomer(int customerId)
    {
        var customer = await _customerService.GetCustomerAsync(customerId);
        return Ok(customer);
    }

    [HttpGet("orders")]
    public async Task<ActionResult<IReadOnlyCollection<OrderResponse>>> GetCustomerOrders(int customerId)
    {
        var orders = await _orderService.GetCustomerOrdersAsync(customerId);
        return Ok(orders);
    }

    [HttpPost("orders")]
    public async Task<ActionResult<OrderResponse>> CreateOrder(int customerId)
    {
        var order = await _orderService.CreateOrderAsync(customerId);
        return CreatedAtAction(
            nameof(GetCustomerOrder),
            new { customerId, orderId = order.Id },
            order);
    }

    [HttpGet("orders/{orderId:int}")]
    public async Task<ActionResult<OrderResponse>> GetCustomerOrder(int customerId, int orderId)
    {
        var order = await _orderService.GetOrderAsync(customerId, orderId);
        return Ok(order);
    }

    [HttpPost("orders/{orderId:int}/items")]
    public async Task<ActionResult<OrderResponse>> AddOrderItem(
        int customerId,
        int orderId,
        [FromBody] AddOrderItemRequest request)
    {
        var order = await _orderService.AddItemAsync(customerId, orderId, request);
        return Ok(order);
    }

    [HttpPatch("orders/{orderId:int}/items/{orderItemId:int}")]
    public async Task<ActionResult<OrderResponse>> UpdateOrderItemQuantity(
        int customerId,
        int orderId,
        int orderItemId,
        [FromBody] UpdateOrderItemQuantityRequest request)
    {
        var order = await _orderService.UpdateItemQuantityAsync(
            customerId, orderId, orderItemId, request);
        return Ok(order);
    }

    [HttpDelete("orders/{orderId:int}/items/{orderItemId:int}")]
    public async Task<ActionResult<OrderResponse>> RemoveOrderItem(
        int customerId,
        int orderId,
        int orderItemId)
    {
        var order = await _orderService.RemoveItemAsync(customerId, orderId, orderItemId);
        return Ok(order);
    }

    [HttpPost("orders/{orderId:int}/checkout")]
    public async Task<ActionResult<OrderResponse>> Checkout(
        int customerId,
        int orderId,
        [FromBody] CheckoutRequest request)
    {
        var order = await _orderService.CheckoutAsync(customerId, orderId, request);
        return Ok(order);
    }

    [HttpPost("orders/{orderId:int}/payment")]
    public async Task<ActionResult<PaymentResponse>> ProcessPayment(
        int customerId,
        int orderId,
        [FromBody] PaymentRequestDto request)
    {
        var result = await _orderService.ProcessPaymentAsync(customerId, orderId, request);
        return Ok(result);
    }

    [HttpGet("orders/{orderId:int}/status")]
    public async Task<ActionResult<OrderStatusResponse>> GetOrderStatus(int customerId, int orderId)
    {
        var status = await _orderService.GetOrderStatusAsync(customerId, orderId);
        return Ok(status);
    }
}
