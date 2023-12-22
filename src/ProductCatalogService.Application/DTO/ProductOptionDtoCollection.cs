using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ProductCatalogService.Application.DTO
{
    public class ProductOptionDtoCollection
    {
        private readonly List<ProductOptionDto> _productOptionDtos = new List<ProductOptionDto>();

        public ProductOptionDtoCollection(IEnumerable<ProductOptionDto> productDtos)
        {
            _productOptionDtos.AddRange(productDtos);
        }

        public ReadOnlyCollection<ProductOptionDto> Items => _productOptionDtos.AsReadOnly();
    }
}