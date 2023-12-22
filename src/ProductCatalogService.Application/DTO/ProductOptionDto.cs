using System;

namespace ProductCatalogService.Application.DTO
{
    public class ProductOptionDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}