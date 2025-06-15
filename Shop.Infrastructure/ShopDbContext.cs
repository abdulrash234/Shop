using Microsoft.EntityFrameworkCore;
using Shop.Application.Models;

namespace Shop.Infrastructure;

public class ShopDbContext : DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<User> Users { get; set; }
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var testUserId = Guid.NewGuid();
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = testUserId,
            Name = "Test User",
            Email = "test@example.com"
        });

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = Guid.NewGuid(), Name = "Wireless Mouse", Price = 25.99m },
            new Product { Id = Guid.NewGuid(), Name = "Mechanical Keyboard", Price = 89.50m },
            new Product { Id = Guid.NewGuid(), Name = "HD Monitor", Price = 199.99m }
        );

        // Composite Key for CartItem (CartId + ProductId)
        modelBuilder.Entity<CartItem>()
            .HasKey(ci => new { ci.CartId, ci.ProductId });

        // Composite Key for OrderItem (OrderId + ProductId)
        modelBuilder.Entity<OrderItem>()
            .HasKey(oi => new { oi.OrderId, oi.ProductId });
    }
}