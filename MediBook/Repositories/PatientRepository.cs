using System.Threading.Tasks;
using Npgsql;
using MediBook.Data;
using MediBook.Models.Entities;
using MediBook.Models.DTOs;
using MediBook.Repositories.Interfaces;

namespace MediBook.Repositories
{
    public class PatientRepository : IPatientRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public PatientRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreatePatientAsync(Patient patient, Npgsql.NpgsqlTransaction? transaction = null)
        {
            var connection = transaction?.Connection ?? _connectionFactory.CreateNpgsqlConnection();
            if (transaction == null) await connection.OpenAsync();

            try
            {
                using var command = new NpgsqlCommand(@"
                    INSERT INTO Patients (UserId)
                    VALUES (@UserId)
                    RETURNING PatientId;", connection, transaction);
                
                command.Parameters.AddWithValue("@UserId", patient.UserId);

                var newPatientId = await command.ExecuteScalarAsync();
                return Convert.ToInt32(newPatientId);
            }
            finally
            {
                if (transaction == null) await connection.DisposeAsync();
            }
        }

        public async Task<PatientProfileDto?> GetPatientProfileByUserIdAsync(int userId)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT p.PatientId, p.UserId, u.FullName, u.Email, p.Phone, p.Gender, p.DateOfBirth, p.Address, u.IsActive, u.CreatedAt
                FROM Patients p
                INNER JOIN Users u ON p.UserId = u.UserId
                WHERE p.UserId = @UserId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new PatientProfileDto
                {
                    PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                    DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                };
            }

            return null;
        }

        public async Task<bool> UpdatePatientProfileAsync(PatientProfileDto profile)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                // Update User table
                var userSql = @"UPDATE Users SET FullName = @FullName WHERE UserId = @UserId";
                using var userCommand = new NpgsqlCommand(userSql, connection, transaction);
                userCommand.Parameters.AddWithValue("@FullName", profile.FullName);
                userCommand.Parameters.AddWithValue("@UserId", profile.UserId);
                await userCommand.ExecuteNonQueryAsync();

                // Update Patient table
                var patientSql = @"
                    UPDATE Patients 
                    SET Phone = @Phone, Gender = @Gender, DateOfBirth = @DateOfBirth, Address = @Address
                    WHERE PatientId = @PatientId";
                
                using var patientCommand = new NpgsqlCommand(patientSql, connection, transaction);
                patientCommand.Parameters.AddWithValue("@Phone", profile.Phone ?? (object)DBNull.Value);
                patientCommand.Parameters.AddWithValue("@Gender", profile.Gender ?? (object)DBNull.Value);
                patientCommand.Parameters.AddWithValue("@DateOfBirth", profile.DateOfBirth ?? (object)DBNull.Value);
                patientCommand.Parameters.AddWithValue("@Address", profile.Address ?? (object)DBNull.Value);
                patientCommand.Parameters.AddWithValue("@PatientId", profile.PatientId);
                
                await patientCommand.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<System.Collections.Generic.IEnumerable<PatientProfileDto>> GetAllPatientsAsync(string? search = null)
        {
            var patients = new System.Collections.Generic.List<PatientProfileDto>();
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT p.PatientId, p.UserId, u.FullName, u.Email, p.Phone, p.Gender, p.DateOfBirth, p.Address, u.IsActive, u.CreatedAt
                FROM Patients p
                INNER JOIN Users u ON p.UserId = u.UserId
                WHERE u.Role = 'Patient' ";

            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += " AND (u.FullName ILIKE @Search OR u.Email ILIKE @Search OR p.Phone ILIKE @Search) ";
            }

            sql += " ORDER BY u.CreatedAt DESC";

            using var command = new NpgsqlCommand(sql, connection);
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                command.Parameters.AddWithValue("@Search", $"%{search}%");
            }

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                patients.Add(new PatientProfileDto
                {
                    PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                    UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                    Gender = reader.IsDBNull(reader.GetOrdinal("Gender")) ? null : reader.GetString(reader.GetOrdinal("Gender")),
                    DateOfBirth = reader.IsDBNull(reader.GetOrdinal("DateOfBirth")) ? null : reader.GetDateTime(reader.GetOrdinal("DateOfBirth")),
                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                });
            }
            return patients;
        }

        public async Task<bool> TogglePatientStatusAsync(int patientId)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();
            var sql = @"
                UPDATE Users SET IsActive = NOT IsActive 
                WHERE UserId = (SELECT UserId FROM Patients WHERE PatientId = @PatientId)";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PatientId", patientId);
            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeletePatientAsync(int patientId)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();
            var sql = @"
                DELETE FROM Users 
                WHERE UserId = (SELECT UserId FROM Patients WHERE PatientId = @PatientId)";
            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PatientId", patientId);
            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
}
