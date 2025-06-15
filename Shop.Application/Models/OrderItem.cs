namespace Shop.Application.Models;

public class OrderItem
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}