using Shop.Application.Interfaces;
using Shop.Application.Models;

namespace Shop.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly ICartRepository  _cartRepo;  

    public OrderService(IOrderRepository orderRepo, ICartRepository cartRepo)
    {
        _orderRepo = orderRepo;
        _cartRepo  = cartRepo;
    }

    public async Task<Guid> PlaceOrderAsync(Guid userId)
    {
        var cart = await _cartRepo.GetCartByUserIdAsync(userId);
        if (cart is null || cart.Items.Count == 0)
            throw new InvalidOperationException("Cart is empty.");

        var order = new Order
        {
            UserId    = userId,
            CreatedAt = DateTime.UtcNow,
            Items = cart.Items.Select(ci => new OrderItem
            {
                ProductId  = ci.ProductId,
                Quantity   = ci.Quantity,
                UnitPrice  = ci.Product.Price
            }).ToList()
        };

        await _orderRepo.AddOrderAsync(order);

        await _cartRepo.ClearCartAsync(userId); 
        return order.Id;
    }

    public Task<List<Order>> GetOrdersForUserAsync(Guid userId) =>
        _orderRepo.GetOrdersByUserIdAsync(userId);
}