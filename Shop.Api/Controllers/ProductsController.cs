using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Contracts;
using Shop.Infrastructure;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public partial class ProductsController : ControllerBase
{
    private readonly ShopDbContext _context;

    public ProductsController(ShopDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _context.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price
            })
            .ToListAsync();

        return Ok(products);
    }
}