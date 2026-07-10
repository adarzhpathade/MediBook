using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using MediBook.Data;
using MediBook.Models.DTOs;
using MediBook.Repositories.Interfaces;

namespace MediBook.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public DoctorRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync(string? specialty = null, string? name = null, bool onlyActive = true)
        {
            var doctors = new List<DoctorDto>();
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT d.DoctorId, d.UserId, u.FullName, u.Email, d.Specialization, d.Qualification, 
                       d.Experience, d.ConsultationFee, d.Biography, d.AvailableDays, d.AvailableTime, d.ProfileImage, u.IsActive
                FROM Doctors d
                INNER JOIN Users u ON d.UserId = u.UserId
                WHERE 1=1 ";

            if (onlyActive)
            {
                sql += " AND u.IsActive = true ";
            }

            if (!string.IsNullOrWhiteSpace(specialty))
            {
                sql += " AND d.Specialization ILIKE @Specialty ";
            }

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql += " AND u.FullName ILIKE @Name ";
            }

            sql += " ORDER BY u.FullName ASC";

            using var command = new NpgsqlCommand(sql, connection);
            
            if (!string.IsNullOrWhiteSpace(specialty))
                command.Parameters.AddWithValue("@Specialty", $"%{specialty}%");
            
            if (!string.IsNullOrWhiteSpace(name))
                command.Parameters.AddWithValue("@Name", $"%{name}%");

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                doctors.Add(MapToDoctorDto(reader));
            }

            return doctors;
        }

        public async Task<DoctorDto?> GetDoctorByIdAsync(int doctorId)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT d.DoctorId, d.UserId, u.FullName, u.Email, d.Specialization, d.Qualification, 
                       d.Experience, d.ConsultationFee, d.Biography, d.AvailableDays, d.AvailableTime, d.ProfileImage, u.IsActive
                FROM Doctors d
                INNER JOIN Users u ON d.UserId = u.UserId
                WHERE d.DoctorId = @DoctorId AND u.IsActive = true";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToDoctorDto(reader);
            }

            return null;
        }

        public async Task<DoctorDto?> GetDoctorByUserIdAsync(int userId)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT d.DoctorId, d.UserId, u.FullName, u.Email, d.Specialization, d.Qualification, 
                       d.Experience, d.ConsultationFee, d.Biography, d.AvailableDays, d.AvailableTime, d.ProfileImage, u.IsActive
                FROM Doctors d
                INNER JOIN Users u ON d.UserId = u.UserId
                WHERE d.UserId = @UserId AND u.IsActive = true";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToDoctorDto(reader);
            }

            return null;
        }

        public async Task<IEnumerable<DoctorAvailabilityDto>> GetDoctorAvailabilityAsync(int doctorId)
        {
            var availabilities = new List<DoctorAvailabilityDto>();
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT AvailabilityId, DoctorId, DayOfWeek, StartTime, EndTime, SlotDuration
                FROM DoctorAvailability
                WHERE DoctorId = @DoctorId
                ORDER BY DayOfWeek, StartTime";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                availabilities.Add(new DoctorAvailabilityDto
                {
                    AvailabilityId = reader.GetInt32(reader.GetOrdinal("AvailabilityId")),
                    DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                    DayOfWeek = reader.GetInt32(reader.GetOrdinal("DayOfWeek")),
                    StartTime = reader.GetTimeSpan(reader.GetOrdinal("StartTime")),
                    EndTime = reader.GetTimeSpan(reader.GetOrdinal("EndTime")),
                    SlotDuration = reader.GetInt32(reader.GetOrdinal("SlotDuration"))
                });
            }

            return availabilities;
        }

        public async Task SaveDoctorAvailabilityAsync(int doctorId, IEnumerable<DoctorAvailabilityDto> availabilities)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Delete existing availability
                var deleteSql = "DELETE FROM DoctorAvailability WHERE DoctorId = @DoctorId";
                using var deleteCommand = new NpgsqlCommand(deleteSql, connection, transaction);
                deleteCommand.Parameters.AddWithValue("@DoctorId", doctorId);
                await deleteCommand.ExecuteNonQueryAsync();

                // Insert new availability
                var insertSql = @"
                    INSERT INTO DoctorAvailability (DoctorId, DayOfWeek, StartTime, EndTime, SlotDuration)
                    VALUES (@DoctorId, @DayOfWeek, @StartTime, @EndTime, @SlotDuration)";

                foreach (var availability in availabilities)
                {
                    using var insertCommand = new NpgsqlCommand(insertSql, connection, transaction);
                    insertCommand.Parameters.AddWithValue("@DoctorId", doctorId);
                    insertCommand.Parameters.AddWithValue("@DayOfWeek", availability.DayOfWeek);
                    insertCommand.Parameters.AddWithValue("@StartTime", availability.StartTime);
                    insertCommand.Parameters.AddWithValue("@EndTime", availability.EndTime);
                    insertCommand.Parameters.AddWithValue("@SlotDuration", availability.SlotDuration);
                    
                    await insertCommand.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> ToggleDoctorStatusAsync(int doctorId)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                UPDATE Users 
                SET IsActive = NOT IsActive 
                WHERE UserId = (SELECT UserId FROM Doctors WHERE DoctorId = @DoctorId)";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                DELETE FROM Users 
                WHERE UserId = (SELECT UserId FROM Doctors WHERE DoctorId = @DoctorId)";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> CreateDoctorAsync(MediBook.Models.ViewModels.AddDoctorViewModel model)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();
            using var transaction = await connection.BeginTransactionAsync();
            try
            {
                var hash = MediBook.Helpers.PasswordHasher.HashPassword(model.Password);
                
                var insertUserSql = @"
                    INSERT INTO Users (FullName, Email, PasswordHash, Role)
                    VALUES (@FullName, @Email, @Hash, 'Doctor')
                    RETURNING UserId;";
                    
                using var userCmd = new NpgsqlCommand(insertUserSql, connection, transaction);
                userCmd.Parameters.AddWithValue("@FullName", model.FullName);
                userCmd.Parameters.AddWithValue("@Email", model.Email);
                userCmd.Parameters.AddWithValue("@Hash", hash);
                
                var userId = Convert.ToInt32(await userCmd.ExecuteScalarAsync());

                var insertDoctorSql = @"
                    INSERT INTO Doctors (UserId, Specialization, Qualification, Experience, ConsultationFee, Biography)
                    VALUES (@UserId, @Specialization, @Qualification, @Experience, @ConsultationFee, @Biography);";
                    
                using var doctorCmd = new NpgsqlCommand(insertDoctorSql, connection, transaction);
                doctorCmd.Parameters.AddWithValue("@UserId", userId);
                doctorCmd.Parameters.AddWithValue("@Specialization", model.Specialization);
                doctorCmd.Parameters.AddWithValue("@Qualification", model.Qualification);
                doctorCmd.Parameters.AddWithValue("@Experience", model.Experience.HasValue ? model.Experience.Value : (object)System.DBNull.Value);
                doctorCmd.Parameters.AddWithValue("@ConsultationFee", model.ConsultationFee.HasValue ? model.ConsultationFee.Value : (object)System.DBNull.Value);
                doctorCmd.Parameters.AddWithValue("@Biography", string.IsNullOrEmpty(model.Biography) ? (object)System.DBNull.Value : model.Biography);
                
                await doctorCmd.ExecuteNonQueryAsync();
                
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        private DoctorDto MapToDoctorDto(NpgsqlDataReader reader)
        {
            return new DoctorDto
            {
                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                FullName = reader.GetString(reader.GetOrdinal("FullName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Specialization = reader.GetString(reader.GetOrdinal("Specialization")),
                Qualification = reader.GetString(reader.GetOrdinal("Qualification")),
                Experience = reader.IsDBNull(reader.GetOrdinal("Experience")) ? null : reader.GetInt32(reader.GetOrdinal("Experience")),
                ConsultationFee = reader.IsDBNull(reader.GetOrdinal("ConsultationFee")) ? null : reader.GetDecimal(reader.GetOrdinal("ConsultationFee")),
                Biography = reader.IsDBNull(reader.GetOrdinal("Biography")) ? null : reader.GetString(reader.GetOrdinal("Biography")),
                AvailableDays = reader.IsDBNull(reader.GetOrdinal("AvailableDays")) ? null : reader.GetString(reader.GetOrdinal("AvailableDays")),
                AvailableTime = reader.IsDBNull(reader.GetOrdinal("AvailableTime")) ? null : reader.GetString(reader.GetOrdinal("AvailableTime")),
                ProfileImage = reader.IsDBNull(reader.GetOrdinal("ProfileImage")) ? null : reader.GetString(reader.GetOrdinal("ProfileImage")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }
    }
}
