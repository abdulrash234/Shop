using Shop.Application.Models;

namespace Shop.Application.Interfaces;

public interface IProductService
{
    Task<List<Product>> GetAllAsync();
}