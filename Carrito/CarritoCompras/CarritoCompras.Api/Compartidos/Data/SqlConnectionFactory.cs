using Microsoft.Data.Sqlite;
using System.Data;

namespace CarritoCompras.Api.Compartidos.Data
{
    public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
    {
        private readonly string _connectionString = connectionString;
        public IDbConnection CreateConnection()
        {
            var connection = new SqliteConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
