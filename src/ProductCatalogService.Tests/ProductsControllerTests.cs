using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductCatalogService.Application.DTO;
using ProductCatalogService.Application.Messaging.Commands;
using ProductCatalogService.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace ProductCatalogService.Tests
{
    public class ProductsControllerTests
    {
        public ProductsControllerTests()
        {
            _productsController = new ProductsController(_mediator.Object);
        }

        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly ProductsController _productsController;

        [Fact]
        public async void ShouldReturn200WhenProductCreationSucceeds()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductDto>(new ProductDto()));

            // Act
            var actionResult = await _productsController.CreateProduct(new ProductCreateDto()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn200WhenProductDeleteSucceeds()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>(true));

            // Act
            var actionResult =
                await _productsController.DeleteProduct(Guid.NewGuid()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn200WhenProductQuerySucceeds()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetProductsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new CommandResult<ProductDtoCollection>(new ProductDtoCollection(new List<ProductDto>())));

            // Act
            var actionResult = await _productsController.GetProducts(null) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn200WhenProductUpdateSucceeds()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductDto>(new ProductDto()));

            // Act
            var actionResult =
                await _productsController.UpdateProduct(Guid.NewGuid(), new ProductUpdateRequestDto()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn404WhenProductQueryFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetProductsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductDtoCollection>("Failed"));

            // Act
            var actionResult = await _productsController.GetProducts(null) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status404NotFound, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn500WhenProductCreationFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductDto>("Failed"));

            // Act
            var actionResult = await _productsController.CreateProduct(new ProductCreateDto()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn500WhenProductDeleteFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>("Failed"));

            // Act
            var actionResult =
                await _productsController.DeleteProduct(Guid.NewGuid()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn500WhenProductUpdateFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductDto>("Failed"));

            // Act
            var actionResult =
                await _productsController.UpdateProduct(Guid.NewGuid(), new ProductUpdateRequestDto()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode.Value);
        }
    }
}