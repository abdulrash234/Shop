using System.ComponentModel.DataAnnotations;

namespace Shop.Contracts;

public class AddToCartDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must have value 1 more more")]
    public int Quantity { get; set; }
}