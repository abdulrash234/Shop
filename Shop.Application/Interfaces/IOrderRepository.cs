using Shop.Application.Models;

namespace Shop.Application.Interfaces;

public interface IOrderRepository
{
    Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
    Task AddOrderAsync(Order order);
}