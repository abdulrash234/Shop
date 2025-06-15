using FluentAssertions;
using NSubstitute;
using Shop.Application.Interfaces;
using Shop.Application.Models;
using Shop.Application.Services;

namespace Shop.Application.Tests;

public class OrderServiceTests
{
    private readonly IOrderRepository _orderRepository = Substitute.For<IOrderRepository>();
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();

    private readonly OrderService _sut;

    public OrderServiceTests()
    {
        _sut = new OrderService(_orderRepository, _cartRepository);
    }

    [Fact]
    public async Task PlaceOrderAsyncShouldThrowWhenCartIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _cartRepository.GetCartByUserIdAsync(userId).Returns((Cart?)null);

        // Act
        Func<Task> act = async () => await _sut.PlaceOrderAsync(userId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cart is empty.");
    }

    [Fact]
    public async Task PlaceOrderAsyncShouldThrowWhenCartIsEmpty()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var emptyCart = new Cart { UserId = userId, Items = new List<CartItem>() };
        _cartRepository.GetCartByUserIdAsync(userId).Returns(emptyCart);

        // Act
        Func<Task> act = async () => await _sut.PlaceOrderAsync(userId);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Cart is empty.");
    }

    [Fact]
    public async Task PlaceOrderAsyncShouldCreateOrderAndClearCart()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        
        var cart = new Cart
        {
            UserId = userId,
            Items = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = productId,
                    Quantity = 2,
                    Product = new Product { Id = productId, Price = 10.0m }
                }
            }
        };
        _cartRepository.GetCartByUserIdAsync(userId).Returns(cart);

        // Act
        await _sut.PlaceOrderAsync(userId);

        // Assert

        await _orderRepository.Received(1).AddOrderAsync(Arg.Any<Order>());
        await _cartRepository.Received(1).ClearCartAsync(userId);
    }

    [Fact]
    public async Task GetOrdersForUserAsyncShouldReturnOrders()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedOrders = new List<Order>
        {
            new Order { Id = Guid.NewGuid(), UserId = userId }
        };

        _orderRepository.GetOrdersByUserIdAsync(userId).Returns(expectedOrders);

        // Act
        var actual = await _sut.GetOrdersForUserAsync(userId);

        // Assert
        actual.Should().BeEquivalentTo(expectedOrders);
    }
}