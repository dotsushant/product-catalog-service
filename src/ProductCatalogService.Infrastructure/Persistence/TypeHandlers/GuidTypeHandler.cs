using Dapper;
using System;
using System.Data;

namespace ProductCatalogService.Infrastructure.Persistence.TypeHandlers
{
    public class GuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString();
        }

        public override Guid Parse(object value)
        {
            Guid.TryParse(value?.ToString(), out var result);
            return result;
        }
    }
}