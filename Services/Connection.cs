using Microsoft.Data.SqlClient;
using System.Data;


namespace AdventureWorksLT2022.Services
{
    public class Connection
    {
        private readonly string _connectionString;

        public Connection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
