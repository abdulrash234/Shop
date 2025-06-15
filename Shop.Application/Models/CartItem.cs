namespace Shop.Application.Models;

public class CartItem
{
    public Guid CartId { get; set; }

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }
}