using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using MediBook.Data;
using MediBook.Models.ViewModels;
using MediBook.Services.Interfaces;

namespace MediBook.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly DbConnectionFactory _connectionFactory;

        public AdminDashboardService(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<AdminDashboardViewModel> GetDashboardDataAsync()
        {
            var vm = new AdminDashboardViewModel();
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            // Total Doctors
            using var totalDoctorsCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Role = 'Doctor' AND IsActive = TRUE", connection);
            vm.TotalDoctors = Convert.ToInt32(await totalDoctorsCmd.ExecuteScalarAsync() ?? 0);

            // Total Patients
            using var totalPatientsCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Users WHERE Role = 'Patient' AND IsActive = TRUE", connection);
            vm.TotalPatients = Convert.ToInt32(await totalPatientsCmd.ExecuteScalarAsync() ?? 0);

            // Total Appointments
            using var totalApptsCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Appointments", connection);
            vm.TotalAppointments = Convert.ToInt32(await totalApptsCmd.ExecuteScalarAsync() ?? 0);

            // Pending Appointments
            using var pendingApptsCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Appointments WHERE Status = 'Pending'", connection);
            vm.PendingAppointments = Convert.ToInt32(await pendingApptsCmd.ExecuteScalarAsync() ?? 0);

            // Completed Appointments
            using var completedApptsCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Appointments WHERE Status = 'Completed'", connection);
            vm.CompletedAppointments = Convert.ToInt32(await completedApptsCmd.ExecuteScalarAsync() ?? 0);

            // Today Appointments
            using var todayApptsCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Appointments WHERE AppointmentDate = CURRENT_DATE", connection);
            vm.TodayAppointments = Convert.ToInt32(await todayApptsCmd.ExecuteScalarAsync() ?? 0);

            // Cancelled Appointments
            using var cancelledApptsCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Appointments WHERE Status = 'Cancelled'", connection);
            vm.CancelledAppointments = Convert.ToInt32(await cancelledApptsCmd.ExecuteScalarAsync() ?? 0);

            // Total Users
            using var totalUsersCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Users", connection);
            vm.TotalUsers = Convert.ToInt32(await totalUsersCmd.ExecuteScalarAsync() ?? 0);

            // Recent Registrations
            using var recentRegCmd = new NpgsqlCommand(@"
                SELECT FullName, Email, Role, CreatedAt 
                FROM Users 
                WHERE Role IN ('Patient', 'Doctor') 
                ORDER BY CreatedAt DESC 
                LIMIT 5", connection);
            using var regReader = await recentRegCmd.ExecuteReaderAsync();
            while (await regReader.ReadAsync())
            {
                vm.RecentRegistrations.Add(new AdminRecentRegistration
                {
                    Name = regReader.GetString(0),
                    Email = regReader.GetString(1),
                    Role = regReader.GetString(2),
                    CreatedAt = regReader.GetDateTime(3)
                });
            }
            await regReader.CloseAsync();

            // Recent Appointments
            using var recentApptCmd = new NpgsqlCommand(@"
                SELECT a.AppointmentId, up.FullName AS PatientName, ud.FullName AS DoctorName, a.AppointmentDate, a.AppointmentTime, a.Status 
                FROM Appointments a
                JOIN Patients p ON a.PatientId = p.PatientId
                JOIN Users up ON p.UserId = up.UserId
                JOIN Doctors d ON a.DoctorId = d.DoctorId
                JOIN Users ud ON d.UserId = ud.UserId
                ORDER BY a.CreatedAt DESC 
                LIMIT 5", connection);
            using var apptReader = await recentApptCmd.ExecuteReaderAsync();
            while (await apptReader.ReadAsync())
            {
                vm.RecentAppointments.Add(new AdminRecentAppointment
                {
                    AppointmentId = apptReader.GetInt32(0),
                    PatientName = apptReader.GetString(1),
                    DoctorName = apptReader.GetString(2),
                    AppointmentDate = apptReader.GetDateTime(3),
                    AppointmentTime = apptReader.GetTimeSpan(4),
                    Status = apptReader.GetString(5)
                });
            }

            return vm;
        }
    }
}
