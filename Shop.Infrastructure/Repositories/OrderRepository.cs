using Microsoft.EntityFrameworkCore;
using Shop.Application.Interfaces;
using Shop.Application.Models;

namespace Shop.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ShopDbContext _context;
    public OrderRepository(ShopDbContext context) => _context = context;

    public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .ToListAsync();
    }

    public async Task AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();          // persistence hidden in repo
    }
}