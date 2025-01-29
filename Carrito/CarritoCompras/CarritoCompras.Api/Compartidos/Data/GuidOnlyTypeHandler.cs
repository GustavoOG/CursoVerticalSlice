using System.Data;
using static Dapper.SqlMapper;

namespace CarritoCompras.Api.Compartidos.Data
{
    internal sealed class GuidOnlyTypeHandler : TypeHandler<Guid>
    {
        public override Guid Parse(object value) => new Guid((string)value);

        public override void SetValue(IDbDataParameter parameter, Guid guid)
        {
            parameter.Value = guid.ToString();
        }
    }
}
