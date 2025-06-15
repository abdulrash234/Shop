using FluentAssertions;
using NSubstitute;
using Shop.Application.Interfaces;
using Shop.Application.Models;
using Shop.Application.Services;

namespace Shop.Application.Tests;

public class CartServiceTests
{
    private readonly ICartRepository _cartRepository = Substitute.For<ICartRepository>();
    private readonly CartService _sut;

    public CartServiceTests()
    {
        _sut = new CartService(_cartRepository);
    }

    [Fact]
    public async Task GetCartAsyncShouldReturnCartFromRepository()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedCart = new Cart { UserId = userId };
        _cartRepository.GetCartByUserIdAsync(userId).Returns(expectedCart);

        // Act
        var result = await _sut.GetCartAsync(userId);

        // Assert
        result.Should().BeEquivalentTo(expectedCart);
    }

    [Fact]
    public async Task AddToCartAsyncShouldCallRepositoryWithCorrectParameters()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();
        var quantity = 3;

        // Act
        await _sut.AddToCartAsync(userId, productId, quantity);

        // Assert
        await _cartRepository.Received(1).AddToCartAsync(userId, productId, quantity);
    }
}