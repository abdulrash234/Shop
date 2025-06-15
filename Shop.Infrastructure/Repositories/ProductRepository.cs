using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Application.Models;

namespace Shop.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ShopDbContext _context;

    public ProductRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }
}