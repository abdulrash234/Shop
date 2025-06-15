namespace Shop.Contracts;

public class CartResponseDto
{
    public Guid UserId { get; set; }
    public List<CartItemResponseDto> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.TotalPrice);
}