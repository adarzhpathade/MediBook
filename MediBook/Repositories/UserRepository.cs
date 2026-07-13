using System.Threading.Tasks;
using Npgsql;
using MediBook.Data;
using MediBook.Models.Entities;
using MediBook.Repositories.Interfaces;

namespace MediBook.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public UserRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            using var command = new NpgsqlCommand("SELECT * FROM Users WHERE Email = @Email", connection);
            command.Parameters.AddWithValue("@Email", email);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Role = reader.GetString(reader.GetOrdinal("Role")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                };
            }
            return null;
        }

        public async Task<int> CreateUserAsync(User user, Npgsql.NpgsqlTransaction? transaction = null)
        {
            var connection = transaction?.Connection ?? _connectionFactory.CreateNpgsqlConnection();
            if (transaction == null) await connection.OpenAsync();

            try
            {
                using var command = new NpgsqlCommand(@"
                    INSERT INTO Users (FullName, Email, PasswordHash, Role)
                    VALUES (@FullName, @Email, @PasswordHash, @Role)
                    RETURNING UserId;", connection, transaction);
                
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                command.Parameters.AddWithValue("@Role", user.Role);

                var id = await command.ExecuteScalarAsync();
                return Convert.ToInt32(id);
            }
            finally
            {
                if (transaction == null) await connection.DisposeAsync();
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = "SELECT * FROM Users WHERE UserId = @UserId;";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                    Role = reader.GetString(reader.GetOrdinal("Role")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                };
            }

            return null;
        }

        public async Task<bool> UpdateUserPasswordAsync(int userId, string passwordHash)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserId = @UserId;";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PasswordHash", passwordHash);
            command.Parameters.AddWithValue("@UserId", userId);

            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"UPDATE Users 
                        SET FullName = @FullName, 
                            Email = @Email, 
                            Role = @Role, 
                            IsActive = @IsActive,
                            PasswordHash = @PasswordHash 
                        WHERE UserId = @UserId;";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@FullName", user.FullName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Role", user.Role);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
            command.Parameters.AddWithValue("@UserId", user.UserId);

            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }
    }
}
