using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Contracts;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetCart(Guid userId)
    {
        var cart = await _cartService.GetCartAsync(userId);
        if (cart is null) return NotFound("Cart not found.");

        var response = new CartResponseDto
        {
            UserId = cart.UserId,
            Items = cart.Items.Select(item => new CartItemResponseDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                UnitPrice = item.Product.Price,
                Quantity = item.Quantity
            }).ToList()
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        await _cartService.AddToCartAsync(dto.UserId, dto.ProductId, dto.Quantity);
        return Ok();
    }
}