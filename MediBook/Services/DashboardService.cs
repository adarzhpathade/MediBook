using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using MediBook.Data;
using MediBook.Models.ViewModels;
using MediBook.Services.Interfaces;

namespace MediBook.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly DbConnectionFactory _connectionFactory;

        public DashboardService(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<DashboardViewModel> GetPatientDashboardDataAsync(int userId, string fullName)
        {
            var vm = new DashboardViewModel
            {
                PatientFirstName = fullName.Split(' ')[0], // Simple extraction
                MedicalReportsCount = 0,
                ActivePrescriptionsCount = 0,
            };

            // Get PatientId for the given UserId
            int? patientId = null;
            using var connection = _connectionFactory.CreateNpgsqlConnection();
            await connection.OpenAsync();

            using var getPatientCmd = new NpgsqlCommand("SELECT PatientId FROM Patients WHERE UserId = @UserId", connection);
            getPatientCmd.Parameters.AddWithValue("@UserId", userId);
            
            var result = await getPatientCmd.ExecuteScalarAsync();
            if (result != null && result != DBNull.Value)
            {
                patientId = Convert.ToInt32(result);
            }

            if (patientId.HasValue)
            {
                // Fetch Upcoming Appointments Count
                using var upcomingCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Appointments WHERE PatientId = @PatientId AND Status IN ('Pending', 'Confirmed') AND AppointmentDate >= CURRENT_DATE", connection);
                upcomingCmd.Parameters.AddWithValue("@PatientId", patientId.Value);
                vm.UpcomingAppointmentsCount = Convert.ToInt32(await upcomingCmd.ExecuteScalarAsync() ?? 0);

                // Fetch Completed Visits Count
                using var completedCmd = new NpgsqlCommand("SELECT COUNT(*) FROM Appointments WHERE PatientId = @PatientId AND Status = 'Completed'", connection);
                completedCmd.Parameters.AddWithValue("@PatientId", patientId.Value);
                vm.CompletedVisitsCount = Convert.ToInt32(await completedCmd.ExecuteScalarAsync() ?? 0);

                // Fetch Next Appointment
                using var nextApptCmd = new NpgsqlCommand(@"
                    SELECT a.AppointmentId, a.AppointmentDate, a.AppointmentTime, d.Specialization, u.FullName as DoctorName
                    FROM Appointments a
                    JOIN Doctors d ON a.DoctorId = d.DoctorId
                    JOIN Users u ON d.UserId = u.UserId
                    WHERE a.PatientId = @PatientId AND a.Status IN ('Pending', 'Confirmed') AND (a.AppointmentDate > CURRENT_DATE OR (a.AppointmentDate = CURRENT_DATE AND a.AppointmentTime >= CURRENT_TIME))
                    ORDER BY a.AppointmentDate ASC, a.AppointmentTime ASC
                    LIMIT 1", connection);
                nextApptCmd.Parameters.AddWithValue("@PatientId", patientId.Value);

                using var reader = await nextApptCmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    vm.NextAppointment = new AppointmentSummary
                    {
                        AppointmentId = reader.GetInt32(reader.GetOrdinal("AppointmentId")),
                        DoctorName = reader.GetString(reader.GetOrdinal("DoctorName")),
                        Specialization = reader.GetString(reader.GetOrdinal("Specialization")),
                        ClinicName = "MediBook Clinic",
                        AppointmentDate = reader.GetDateTime(reader.GetOrdinal("AppointmentDate")),
                        AppointmentTime = reader.GetTimeSpan(reader.GetOrdinal("AppointmentTime")),
                        Type = "Consultation"
                    };
                }
            }



            return vm;
        }
    }
}
