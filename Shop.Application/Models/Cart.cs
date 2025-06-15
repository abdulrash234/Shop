namespace Shop.Application.Models;

public class Cart
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }

    public List<CartItem> Items { get; set; } = new();
}