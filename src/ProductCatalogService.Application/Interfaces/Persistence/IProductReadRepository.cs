using ProductCatalogService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalogService.Application.Interfaces.Persistence
{
    public interface IProductReadRepository
    {
        Task<IEnumerable<Product>> GetProducts();

        Task<Product> GetProductById(Guid productId);

        Task<IEnumerable<Product>> GetProductsByName(string productName);

        Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId);

        Task<ProductOption> GetProductOptionById(Guid productOptionId);
    }
}