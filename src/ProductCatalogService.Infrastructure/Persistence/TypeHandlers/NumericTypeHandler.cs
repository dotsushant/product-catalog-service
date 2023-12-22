using Dapper;
using System.Data;

namespace ProductCatalogService.Infrastructure.Persistence.TypeHandlers
{
    public class NumericTypeHandler : SqlMapper.TypeHandler<decimal>
    {
        public override void SetValue(IDbDataParameter parameter, decimal value)
        {
            parameter.Value = value;
        }

        public override decimal Parse(object value)
        {
            decimal.TryParse(value?.ToString(), out var result);
            return result;
        }
    }
}