using Dapper;
using Microsoft.Extensions.Logging;
using ProductCatalogService.Application.Interfaces.Persistence;
using ProductCatalogService.Domain.Models;
using ProductCatalogService.Infrastructure.Persistence.TypeHandlers;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

namespace ProductCatalogService.Infrastructure.Persistence
{
    public class ProductWriteRepository : IProductWriteRepository
    {
        private const string CreateProductSql =
            "INSERT INTO Products(Id, Name, Description, Price, DeliveryPrice) VALUES (@Id, @Name, @Description, @Price, @DeliveryPrice)";

        private const string UpdateProductSql =
            "UPDATE Products SET Name=@Name, Description=@Description, Price=@Price, DeliveryPrice=@DeliveryPrice WHERE Id=@Id";

        private const string DeleteProductSql =
            "DELETE FROM Products WHERE Id=@Id";

        private const string CreateProductOptionSql =
            "INSERT INTO ProductOptions(Id, ProductId, Name, Description) VALUES (@Id, @ProductId, @Name, @Description)";

        private const string UpdateProductOptionSql =
            "UPDATE ProductOptions SET Name=@Name, Description=@Description WHERE Id=@Id";

        private const string DeleteProductOptionByIdSql =
            "DELETE FROM ProductOptions WHERE Id=@Id";

        private const string DeleteProductOptionsByProductIdSql =
            "DELETE FROM ProductOptions WHERE ProductId=@ProductId";

        private readonly IDbConnection _connection;
        private readonly ILogger<ProductWriteRepository> _logger;

        public ProductWriteRepository(IDbConnection connection, ILogger<ProductWriteRepository> logger)
        {
            _connection = connection;
            _logger = logger;

            SqlMapper.AddTypeHandler(typeof(Guid), new GuidTypeHandler());
            SqlMapper.AddTypeHandler(typeof(decimal), new NumericTypeHandler());
        }

        public async Task CreateProduct(Product product)
        {
            try
            {
                var affectedRows = await _connection.ExecuteAsync(CreateProductSql,
                    new
                    {
                        product.Id,
                        product.Name,
                        product.Description,
                        product.Price,
                        product.DeliveryPrice
                    });

                if (affectedRows == 0) throw new Exception();
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductCreationHasFailed);
                throw;
            }
        }

        public async Task UpdateProduct(Guid productId, string name, string description, decimal? price,
            decimal? deliveryPrice)
        {
            try
            {
                var affectedRows = await _connection.ExecuteAsync(UpdateProductSql,
                    new
                    {
                        Id = productId,
                        Name = name,
                        Description = description,
                        Price = price,
                        DeliveryPrice = deliveryPrice
                    });

                if (affectedRows == 0) throw new Exception();
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductUpdateHasFailed);
                throw;
            }
        }

        public async Task DeleteProduct(Guid productId)
        {
            try
            {
                using (var transaction = new TransactionScope())
                {
                    await _connection.ExecuteAsync(DeleteProductOptionsByProductIdSql, new { ProductId = productId });
                    var affectedRows = await _connection.ExecuteAsync(DeleteProductSql, new { Id = productId });
                    transaction.Complete();

                    if (affectedRows == 0) throw new Exception();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductDeletionHasFailed);
                throw;
            }
        }

        public async Task CreateProductOption(ProductOption productOption)
        {
            try
            {
                var affectedRows = await _connection.ExecuteAsync(CreateProductOptionSql,
                    new
                    {
                        productOption.Id,
                        productOption.ProductId,
                        productOption.Name,
                        productOption.Description
                    });

                if (affectedRows == 0) throw new Exception();
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionCreationHasFailed);
                throw;
            }
        }

        public async Task UpdateProductOption(Guid productOptionId, string name, string description)
        {
            try
            {
                var affectedRows = await _connection.ExecuteAsync(UpdateProductOptionSql,
                    new
                    {
                        Id = productOptionId,
                        Name = name,
                        Description = description
                    });

                if (affectedRows == 0) throw new Exception();
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionUpdateHasFailed);
                throw;
            }
        }

        public async Task DeleteProductOption(Guid productOptionId)
        {
            try
            {
                var affectedRows = await _connection.ExecuteAsync(DeleteProductOptionByIdSql,
                    new { Id = productOptionId });

                if (affectedRows == 0) throw new Exception();
            }
            catch (Exception e)
            {
                _logger.LogError(e, Resource.ProductOptionDeletionHasFailed);
                throw;
            }
        }
    }
}