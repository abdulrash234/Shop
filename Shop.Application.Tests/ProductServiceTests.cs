using FluentAssertions;
using NSubstitute;
using Shop.Application.Interfaces;
using Shop.Application.Models;
using Shop.Application.Services;

namespace Shop.Application.Tests;

public class ProductServiceTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _sut = new ProductService(_productRepository);
    }

    [Fact]
    public async Task GetAllAsyncShouldReturnListOfProducts()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Product A", Price = 10.0m },
            new Product { Id = Guid.NewGuid(), Name = "Product B", Price = 20.0m }
        };

        _productRepository.GetAllAsync().Returns(expectedProducts);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Should().BeEquivalentTo(expectedProducts);
        await _productRepository.Received(1).GetAllAsync();
    }
}