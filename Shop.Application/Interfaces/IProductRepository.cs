using Shop.Application.Models;

namespace Shop.Application.Interfaces;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
}