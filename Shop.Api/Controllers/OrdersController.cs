using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Application.Models;
using Shop.Contracts;
using Shop.Infrastructure;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ShopDbContext _context;

    public OrdersController(ShopDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserOrders(Guid userId)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(u => u.User)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();

        return Ok(orders);
    }
    
    [HttpPost("place")]
    public async Task<IActionResult> PlaceOrder([FromBody]PlaceOrderDto dto)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == dto.UserId);

        if (cart == null || !cart.Items.Any()) return BadRequest("Cart is empty.");

        var order = new Order
        {
            UserId = dto.UserId,
            CreatedAt = DateTime.UtcNow,
            Items = cart.Items.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Product.Price
            }).ToList()
        };

        _context.Orders.Add(order);
        cart.Items.Clear(); // Clear cart after placing order

        await _context.SaveChangesAsync();
        return Ok(new { order.Id });
    }
}