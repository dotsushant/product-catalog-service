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
    public class ProductOptionsControllerTests
    {
        public ProductOptionsControllerTests()
        {
            _productsController = new ProductOptionsController(_mediator.Object);
        }

        private readonly Mock<IMediator> _mediator = new Mock<IMediator>();
        private readonly ProductOptionsController _productsController;

        [Fact]
        public async void ShouldReturn200WhenProductOptionCreationSucceeds()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<CreateProductOptionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductOptionDto>(new ProductOptionDto()));

            // Act
            var actionResult =
                await _productsController.CreateProductOption(Guid.NewGuid(), new ProductOptionCreateDto()) as
                    ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn200WhenProductOptionDeleteSucceeds()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteProductOptionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>(true));

            // Act
            var actionResult =
                await _productsController.DeleteProductOption(Guid.NewGuid(), Guid.NewGuid()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn200WhenProductOptionQuerySucceeds()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetProductOptionsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(
                    new CommandResult<ProductOptionDtoCollection>(
                        new ProductOptionDtoCollection(new List<ProductOptionDto>())));

            // Act
            var actionResult = await _productsController.GetProductOptions(Guid.NewGuid()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn200WhenProductOptionUpdateSucceeds()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<UpdateProductOptionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductOptionDto>(new ProductOptionDto()));

            // Act
            var actionResult =
                await _productsController.UpdateProductOption(Guid.NewGuid(), Guid.NewGuid(),
                    new ProductOptionUpdateRequestDto()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status200OK, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn404WhenProductOptionQueryFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<GetProductOptionsCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductOptionDtoCollection>("Failed"));

            // Act
            var actionResult = await _productsController.GetProductOptions(Guid.NewGuid()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status404NotFound, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn500WhenProductOptionCreationFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<CreateProductOptionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductOptionDto>("Failed"));

            // Act
            var actionResult =
                await _productsController.CreateProductOption(Guid.NewGuid(), new ProductOptionCreateDto()) as
                    ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn500WhenProductOptionDeleteFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<DeleteProductOptionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<bool>("Failed"));

            // Act
            var actionResult =
                await _productsController.DeleteProductOption(Guid.NewGuid(), Guid.NewGuid()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode.Value);
        }

        [Fact]
        public async void ShouldReturn500WhenProductOptionUpdateFails()
        {
            // Arrange
            _mediator.Setup(m => m.Send(It.IsAny<UpdateProductOptionCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CommandResult<ProductOptionDto>("Failed"));

            // Act
            var actionResult =
                await _productsController.UpdateProductOption(Guid.NewGuid(), Guid.NewGuid(),
                    new ProductOptionUpdateRequestDto()) as ObjectResult;

            // Assert
            Assert.NotNull(actionResult);
            Assert.Equal(StatusCodes.Status500InternalServerError, actionResult.StatusCode.Value);
        }
    }
}