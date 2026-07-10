using System.Data;
using Npgsql;

namespace MediBook.Data
{
    /// <summary>
    /// Factory for creating and providing database connections using Npgsql.
    /// </summary>
    public class DbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        /// <summary>
        /// Creates a new, open Npgsql connection.
        /// </summary>
        /// <returns>An open IDbConnection.</returns>
        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
        
        /// <summary>
        /// Creates a new Npgsql connection without opening it.
        /// </summary>
        public NpgsqlConnection CreateNpgsqlConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
