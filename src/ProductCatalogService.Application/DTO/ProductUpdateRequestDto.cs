﻿namespace ProductCatalogService.Application.DTO
{
    public class ProductUpdateRequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? DeliveryPrice { get; set; }
    }
}