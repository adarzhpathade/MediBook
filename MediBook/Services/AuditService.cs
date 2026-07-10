using System.Threading.Tasks;
using Npgsql;
using MediBook.Data;
using MediBook.Services.Interfaces;

namespace MediBook.Services
{
    public class AuditService : IAuditService
    {
        private readonly DbConnectionFactory _connectionFactory;

        public AuditService(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task LogActionAsync(string action, string details, int? userId = null)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                INSERT INTO AuditLogs (Action, Details, UserId) 
                VALUES (@Action, @Details, @UserId)";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Action", action);
            command.Parameters.AddWithValue("@Details", details ?? (object)System.DBNull.Value);
            command.Parameters.AddWithValue("@UserId", userId.HasValue ? userId.Value : (object)System.DBNull.Value);

            await command.ExecuteNonQueryAsync();
        }
    }
}
