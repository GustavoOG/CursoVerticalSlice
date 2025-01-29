using System.Data;

namespace CarritoCompras.Api.Compartidos.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
