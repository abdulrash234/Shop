using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Contracts;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserOrders(Guid userId)
    {
        
        var orders = await _orderService.GetOrdersForUserAsync(userId);

        var response = orders.Select(o => new OrderResponseDto
        {
            OrderId   = o.Id,
            CreatedAt = o.CreatedAt,
            Items = o.Items.Select(i => new OrderItemResponseDto
            {
                ProductId   = i.ProductId,
                ProductName = i.Product.Name,
                UnitPrice   = i.UnitPrice,
                Quantity    = i.Quantity
            }).ToList()
        }).ToList();

        return Ok(response);
    }
    
    [HttpPost("place")]
    public async Task<IActionResult> PlaceOrder([FromBody]PlaceOrderDto dto)
    {
        var orderId = await _orderService.PlaceOrderAsync(dto.UserId);
        return Ok(new { orderId });
    }
}