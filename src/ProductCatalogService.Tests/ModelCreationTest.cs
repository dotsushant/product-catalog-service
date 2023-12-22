using ProductCatalogService.Domain.Exceptions;
using ProductCatalogService.Domain.Models;
using System;
using Xunit;

namespace ProductCatalogService.Tests
{
    public class ModelCreationTest
    {
        [Theory]
        [InlineData(null, "Product description", null, null)]
        [InlineData("", "Product description", null, null)]
        [InlineData("Product name", null, null, null)]
        [InlineData("Product name", "", null, null)]
        [InlineData("Product name", "Product description", -1.10, null)]
        [InlineData("Product name", "Product description", null, -2.0)]
        [InlineData("Product name", "Product description", -1.0, -2.0)]
        public void ShouldThrowExceptionWhenProductCreationParametersAreInvalid(string name, string description,
            double? price,
            double? deliveryPrice)
        {
            Assert.Throws<ProductException>(() =>
                new Product(name, description, (decimal?)price, (decimal?)deliveryPrice));
        }

        [Theory]
        [InlineData("00000000-0000-0000-0000-000000000000", null, null)]
        [InlineData("f02046b0-411e-4b5d-8a4f-e145e0522024", null, null)]
        [InlineData("f02046b0-411e-4b5d-8a4f-e145e0522024", "Product option name", null)]
        [InlineData("f02046b0-411e-4b5d-8a4f-e145e0522024", null, "Product option description")]
        public void ShouldThrowExceptionWhenProductOptionCreationParametersAreInvalid(string productId, string name,
            string description)
        {
            Assert.Throws<ProductException>(() =>
                new ProductOption(Guid.Parse(productId), name, description));
        }

        [Theory]
        [InlineData("Product name", "Product description", null, null)]
        [InlineData("Product name", "Product description", 1.10, null)]
        [InlineData("Product name", "Product description", null, 1.10)]
        public void ShouldCreateProductWhenValidCreationParametersAreProvided(string name, string description,
            double? price,
            double? deliveryPrice)
        {
            var product = new Product(name, description, (decimal?)price, (decimal?)deliveryPrice);
            Assert.Equal(product.Name, name);
            Assert.Equal(product.Description, description);
            Assert.Equal((double?)product.Price, price);
            Assert.Equal((double?)product.DeliveryPrice, deliveryPrice);
        }

        [Fact]
        public void ShouldCreateProductOptionWhenValidCreationParametersAreProvided()
        {
            const string ProductOptionName = "Product option";
            const string ProductOptionDescription = "Product option description";

            var product = new ProductOption(Guid.NewGuid(), ProductOptionName, ProductOptionDescription);
            Assert.Equal(product.Name, ProductOptionName);
            Assert.Equal(product.Description, ProductOptionDescription);
        }
    }
}