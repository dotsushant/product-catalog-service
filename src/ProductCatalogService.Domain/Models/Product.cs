using ProductCatalogService.Domain.Exceptions;
using ProductCatalogService.Helpers;
using System;

namespace ProductCatalogService.Domain.Models
{
    public class Product
    {
        private Product()
        {
        }

        public Product(string name, string description, decimal? price, decimal? deliveryPrice)
        {
            Contract.Requires<ProductException>(!string.IsNullOrWhiteSpace(name), Resource.ProductNameIsInvalid);
            Contract.Requires<ProductException>(!string.IsNullOrWhiteSpace(description),
                Resource.ProductDescriptionIsInvalid);
            Contract.Requires<ProductException>(price == null || price >= 0, Resource.ProductPriceIsInvalid);
            Contract.Requires<ProductException>(deliveryPrice == null || deliveryPrice >= 0,
                Resource.ProductDeliveryPriceIsInvalid);

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            DeliveryPrice = deliveryPrice;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public decimal? Price { get; }
        public decimal? DeliveryPrice { get; }
    }
}