using System;

namespace ProductCatalogService.Domain.Exceptions
{
    public class ProductException : Exception
    {
        public ProductException(string message) : base(message)
        {
        }
    }
}