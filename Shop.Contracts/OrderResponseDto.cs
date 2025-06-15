namespace Shop.Contracts;

public class OrderResponseDto
{
    public Guid OrderId { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<OrderItemResponseDto> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.TotalPrice);
}