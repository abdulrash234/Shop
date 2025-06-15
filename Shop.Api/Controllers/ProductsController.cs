using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces;
using Shop.Contracts;

namespace Shop.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public  class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllAsync();
        
        var response = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
        }).ToList();

        return Ok(response);
    }
}