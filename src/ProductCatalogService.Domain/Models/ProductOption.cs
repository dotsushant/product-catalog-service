using ProductCatalogService.Domain.Exceptions;
using ProductCatalogService.Helpers;
using System;

namespace ProductCatalogService.Domain.Models
{
    public class ProductOption
    {
        private ProductOption()
        {
        }

        public ProductOption(Guid productId, string name, string description)
        {
            Contract.Requires<ProductException>(productId != Guid.Empty, Resource.ProductIdIsInvalid);
            Contract.Requires<ProductException>(!string.IsNullOrWhiteSpace(name), Resource.ProductOptionNameIsInvalid);
            Contract.Requires<ProductException>(!string.IsNullOrWhiteSpace(description),
                Resource.ProductOptionDescriptionIsInvalid);

            Id = Guid.NewGuid();
            ProductId = productId;
            Name = name;
            Description = description;
        }

        public Guid Id { get; }
        public Guid ProductId { get; }
        public string Name { get; }
        public string Description { get; }
    }
}