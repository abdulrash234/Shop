using Shop.Application.Models;

namespace Shop.Application.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartByUserIdAsync(Guid userId);
    Task AddToCartAsync(Guid userId, Guid productId, int quantity);
    Task ClearCartAsync(Guid userId);
}