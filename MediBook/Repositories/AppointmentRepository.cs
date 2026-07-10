using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using MediBook.Data;
using MediBook.Models.DTOs;
using MediBook.Models.Entities;
using MediBook.Repositories.Interfaces;

namespace MediBook.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public AppointmentRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> CreateAppointmentAsync(Appointment appointment)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                INSERT INTO Appointments (PatientId, DoctorId, AppointmentDate, AppointmentTime, Reason, Status)
                VALUES (@PatientId, @DoctorId, @AppointmentDate, @AppointmentTime, @Reason, @Status)
                RETURNING AppointmentId;";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PatientId", appointment.PatientId);
            command.Parameters.AddWithValue("@DoctorId", appointment.DoctorId);
            command.Parameters.AddWithValue("@AppointmentDate", appointment.AppointmentDate);
            command.Parameters.AddWithValue("@AppointmentTime", appointment.AppointmentTime);
            command.Parameters.AddWithValue("@Reason", appointment.Reason ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Status", appointment.Status);

            var id = await command.ExecuteScalarAsync();
            return Convert.ToInt32(id);
        }

        public async Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string status)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                UPDATE Appointments 
                SET Status = @Status, UpdatedAt = CURRENT_TIMESTAMP
                WHERE AppointmentId = @AppointmentId;";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Status", status);
            command.Parameters.AddWithValue("@AppointmentId", appointmentId);

            var rows = await command.ExecuteNonQueryAsync();
            return rows > 0;
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT a.AppointmentId, a.PatientId, a.DoctorId, a.AppointmentDate, a.AppointmentTime, a.Reason, a.Status,
                       du.FullName AS DoctorName, d.Specialization AS DoctorSpecialization, d.ProfileImage AS DoctorProfileImage,
                       pu.FullName AS PatientName
                FROM Appointments a
                INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
                INNER JOIN Users du ON d.UserId = du.UserId
                INNER JOIN Patients p ON a.PatientId = p.PatientId
                INNER JOIN Users pu ON p.UserId = pu.UserId
                WHERE a.AppointmentId = @AppointmentId;";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@AppointmentId", appointmentId);

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapToAppointmentDto(reader);
            }

            return null;
        }

        public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsAsync(int patientId)
        {
            var appointments = new List<AppointmentDto>();
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT a.AppointmentId, a.PatientId, a.DoctorId, a.AppointmentDate, a.AppointmentTime, a.Reason, a.Status,
                       du.FullName AS DoctorName, d.Specialization AS DoctorSpecialization, d.ProfileImage AS DoctorProfileImage,
                       pu.FullName AS PatientName
                FROM Appointments a
                INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
                INNER JOIN Users du ON d.UserId = du.UserId
                INNER JOIN Patients p ON a.PatientId = p.PatientId
                INNER JOIN Users pu ON p.UserId = pu.UserId
                WHERE a.PatientId = @PatientId
                ORDER BY a.AppointmentDate DESC, a.AppointmentTime DESC;";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@PatientId", patientId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                appointments.Add(MapToAppointmentDto(reader));
            }

            return appointments;
        }

        public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId)
        {
            var appointments = new List<AppointmentDto>();
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT a.AppointmentId, a.PatientId, a.DoctorId, a.AppointmentDate, a.AppointmentTime, a.Reason, a.Status,
                       du.FullName AS DoctorName, d.Specialization AS DoctorSpecialization, d.ProfileImage AS DoctorProfileImage,
                       pu.FullName AS PatientName
                FROM Appointments a
                INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
                INNER JOIN Users du ON d.UserId = du.UserId
                INNER JOIN Patients p ON a.PatientId = p.PatientId
                INNER JOIN Users pu ON p.UserId = pu.UserId
                WHERE a.DoctorId = @DoctorId
                ORDER BY a.AppointmentDate ASC, a.AppointmentTime ASC;";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                appointments.Add(MapToAppointmentDto(reader));
            }

            return appointments;
        }

        public async Task<bool> HasOverlappingAppointmentAsync(int doctorId, DateTime date, TimeSpan time)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT COUNT(1) 
                FROM Appointments 
                WHERE DoctorId = @DoctorId 
                  AND AppointmentDate = @Date 
                  AND AppointmentTime = @Time 
                  AND Status NOT IN ('Cancelled', 'Declined');";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@DoctorId", doctorId);
            command.Parameters.AddWithValue("@Date", date);
            command.Parameters.AddWithValue("@Time", time);

            var count = Convert.ToInt32(await command.ExecuteScalarAsync());
            return count > 0;
        }

        public async Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime)
        {
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                UPDATE Appointments 
                SET AppointmentDate = @NewDate, 
                    AppointmentTime = @NewTime, 
                    Status = 'Rescheduled',
                    UpdatedAt = CURRENT_TIMESTAMP
                WHERE AppointmentId = @AppointmentId";

            using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("@NewDate", newDate);
            command.Parameters.AddWithValue("@NewTime", newTime);
            command.Parameters.AddWithValue("@AppointmentId", appointmentId);

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(string? search = null, string? status = null)
        {
            var appointments = new List<AppointmentDto>();
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            var sql = @"
                SELECT a.AppointmentId, a.PatientId, a.DoctorId, a.AppointmentDate, a.AppointmentTime, a.Reason, a.Status,
                       pu.FullName as PatientName, du.FullName as DoctorName, d.Specialization as DoctorSpecialization, d.ProfileImage as DoctorProfileImage
                FROM Appointments a
                INNER JOIN Patients p ON a.PatientId = p.PatientId
                INNER JOIN Users pu ON p.UserId = pu.UserId
                INNER JOIN Doctors d ON a.DoctorId = d.DoctorId
                INNER JOIN Users du ON d.UserId = du.UserId
                WHERE 1=1 ";

            if (!string.IsNullOrWhiteSpace(search))
            {
                sql += " AND (pu.FullName ILIKE @Search OR du.FullName ILIKE @Search) ";
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                sql += " AND a.Status = @Status ";
            }

            sql += " ORDER BY a.AppointmentDate DESC, a.AppointmentTime DESC";

            using var command = new NpgsqlCommand(sql, connection);
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                command.Parameters.AddWithValue("@Search", $"%{search}%");
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                command.Parameters.AddWithValue("@Status", status);
            }

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                appointments.Add(MapToAppointmentDto(reader));
            }

            return appointments;
        }

        private AppointmentDto MapToAppointmentDto(NpgsqlDataReader reader)
        {
            return new AppointmentDto
            {
                AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                PatientId = reader.GetInt32(reader.GetOrdinal("PatientId")),
                DoctorId = reader.GetInt32(reader.GetOrdinal("DoctorId")),
                AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                AppointmentTime = reader.GetTimeSpan(reader.GetOrdinal("AppointmentTime")),
                Reason = reader.IsDBNull(reader.GetOrdinal("Reason")) ? null : reader.GetString(reader.GetOrdinal("Reason")),
                Status = reader.GetString(reader.GetOrdinal("Status")),
                DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                DoctorSpecialization = reader.GetString(reader.GetOrdinal("DoctorSpecialization")),
                DoctorProfileImage = reader.IsDBNull(reader.GetOrdinal("DoctorProfileImage")) ? null : reader.GetString(reader.GetOrdinal("DoctorProfileImage")),
                PatientName = reader.GetString(reader.GetOrdinal("PatientName"))
            };
        }
    }
}
