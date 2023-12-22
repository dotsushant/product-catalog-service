using AutoMapper;
using ProductCatalogService.Application.DTO;
using ProductCatalogService.Domain.Models;

namespace ProductCatalogService.Application.MappingProfiles
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductUpdateRequestDto, ProductDto>();
            CreateMap<ProductOption, ProductOptionDto>();
            CreateMap<ProductOptionUpdateRequestDto, ProductOptionDto>();
        }
    }
}