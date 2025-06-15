using Shop.Application.Interfaces;
using Shop.Application.Models;

namespace Shop.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return products;
    }
}