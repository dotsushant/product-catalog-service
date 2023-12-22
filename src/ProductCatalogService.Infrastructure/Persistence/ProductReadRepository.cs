using Dapper;
using Microsoft.Extensions.Logging;
using ProductCatalogService.Application.Interfaces.Persistence;
using ProductCatalogService.Domain.Models;
using ProductCatalogService.Infrastructure.Persistence.TypeHandlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogService.Infrastructure.Persistence
{
    public class ProductReadRepository : IProductReadRepository
    {
        private const string SelectProductsSql =
            "SELECT Id, Name, Description, Price, DeliveryPrice FROM Products";

        private const string SelectProductByIdSql =
            "SELECT Id, Name, Description, Price, DeliveryPrice FROM Products WHERE Id=@Id";

        private const string SelectProductByNameSql =
            "SELECT Id, Name, Description, Price, DeliveryPrice FROM Products WHERE Name LIKE @Name";

        private const string SelectProductOptionsSql = @"
            SELECT Id FROM Products WHERE Id=@ProductId;
            SELECT Id, ProductId, Name, Description FROM ProductOptions WHERE ProductId=@ProductId;";

        private const string SelectProductOptionByIdSql =
            "SELECT Id, ProductId, Name, Description FROM ProductOptions WHERE Id=@Id";

        private readonly IDbConnection _connection;
        private readonly ILogger<ProductReadRepository> _logger;

        public ProductReadRepository(IDbConnection connection, ILogger<ProductReadRepository> logger)
        {
            _connection = connection;
            _logger = logger;

            SqlMapper.AddTypeHandler(typeof(Guid), new GuidTypeHandler());
            SqlMapper.AddTypeHandler(typeof(decimal), new NumericTypeHandler());
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            try
            {
                return await _connection.QueryAsync<Product>(SelectProductsSql);
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductQueryHasFailed);
                throw;
            }
        }

        public async Task<Product> GetProductById(Guid id)
        {
            try
            {
                return await _connection.QuerySingleAsync<Product>(SelectProductByIdSql, new { Id = id });
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductQueryHasFailed);
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            try
            {
                return await _connection.QueryAsync<Product>(SelectProductByNameSql, new { Name = $"%{name}%" });
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductQueryHasFailed);
                throw;
            }
        }

        public async Task<IEnumerable<ProductOption>> GetProductOptions(Guid productId)
        {
            try
            {
                using (var multi = await _connection.QueryMultipleAsync(SelectProductOptionsSql, new { ProductId = productId }))
                {
                    multi.Read<Product>().Single(); // This would ensure that the product exists
                    return multi.Read<ProductOption>();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionsQueryHasFailed);
                throw;
            }
        }

        public async Task<ProductOption> GetProductOptionById(Guid productOptionId)
        {
            try
            {
                return await _connection.QuerySingleAsync<ProductOption>(SelectProductOptionByIdSql,
                    new { Id = productOptionId });
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionsQueryHasFailed);
                throw;
            }
        }
    }
}