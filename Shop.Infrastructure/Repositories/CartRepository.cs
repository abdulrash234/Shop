using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Application.Models;

namespace Shop.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly ShopDbContext _context;

    public CartRepository(ShopDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetCartByUserIdAsync(Guid userId)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task AddToCartAsync(Guid userId, Guid productId, int quantity)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart { UserId = userId };
            await _context.Carts.AddAsync(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            cart.Items.Add(new CartItem
            {
                ProductId = productId,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();
    }
    
    public async Task ClearCartAsync(Guid userId)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart != null && cart.Items.Any())
        {
            cart.Items.Clear();
            await _context.SaveChangesAsync();
        }
    }
}