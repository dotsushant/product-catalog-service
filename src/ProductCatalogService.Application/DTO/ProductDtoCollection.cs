using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProductCatalogService.Application.DTO
{
    public class ProductDtoCollection
    {
        private readonly List<ProductDto> _productDtos = new List<ProductDto>();

        public ProductDtoCollection(IEnumerable<ProductDto> productDtos)
        {
            _productDtos.AddRange(productDtos);
        }

        public ReadOnlyCollection<ProductDto> Items => _productDtos.AsReadOnly();
    }
}