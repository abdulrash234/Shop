using Shop.Application.Models;

namespace Shop.Application.Interfaces;

public interface ICartService
{
    Task<Cart?> GetCartAsync(Guid userId);
    Task AddToCartAsync(Guid userId, Guid productId, int quantity);
}