using Shop.Application.Models;

namespace Shop.Application.Interfaces;

public interface IOrderService
{
    Task<Guid> PlaceOrderAsync(Guid userId);
    Task<List<Order>> GetOrdersForUserAsync(Guid userId);
}