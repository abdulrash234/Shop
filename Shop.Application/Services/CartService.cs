using Shop.Application.Interfaces;
using Shop.Application.Models;

namespace Shop.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;

    public CartService(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    public Task<Cart?> GetCartAsync(Guid userId)
    {
        return _cartRepository.GetCartByUserIdAsync(userId);
    }

    public Task AddToCartAsync(Guid userId, Guid productId, int quantity)
    {
        return _cartRepository.AddToCartAsync(userId, productId, quantity);
    }
}