using System;
using System.Linq;
using System.Threading.Tasks;
using MediBook.Models.ViewModels;
using MediBook.Repositories.Interfaces;

namespace MediBook.Services
{
    public class DoctorDashboardService : Interfaces.IDoctorDashboardService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorDashboardService(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<DoctorDashboardViewModel> GetDashboardDataAsync(int userId)
        {
            var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId);
            if (doctor == null) return new DoctorDashboardViewModel();

            var allAppointments = await _appointmentRepository.GetDoctorAppointmentsAsync(doctor.DoctorId);

            var today = DateTime.Today;
            
            // Statistics
            var todaysAppointmentsCount = allAppointments.Count(a => 
                a.AppointmentDate.Date == today && 
                (a.Status == "Confirmed" || a.Status == "Completed"));
                
            var pendingRequestsCount = allAppointments.Count(a => a.Status == "Pending");
            var completedCount = allAppointments.Count(a => a.Status == "Completed");
            var totalPatientsCount = allAppointments
                .Where(a => a.Status == "Confirmed" || a.Status == "Completed")
                .Select(a => a.PatientId)
                .Distinct()
                .Count();

            // Lists
            var todaysAppointments = allAppointments
                .Where(a => a.AppointmentDate.Date == today && (a.Status == "Confirmed" || a.Status == "Completed"))
                .OrderBy(a => a.AppointmentTime)
                .Select(a => new DoctorAppointmentSummary
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.PatientName ?? "Unknown Patient",
                    Reason = a.Reason ?? string.Empty,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status
                }).ToList();

            var pendingRequests = allAppointments
                .Where(a => a.Status == "Pending")
                .OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime)
                .Select(a => new DoctorAppointmentSummary
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.PatientName ?? "Unknown Patient",
                    Reason = a.Reason ?? string.Empty,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status
                }).ToList();

            var upcomingSchedule = allAppointments
                .Where(a => a.AppointmentDate > today && a.Status == "Confirmed")
                .OrderBy(a => a.AppointmentDate).ThenBy(a => a.AppointmentTime)
                .Take(5)
                .Select(a => new DoctorAppointmentSummary
                {
                    AppointmentId = a.AppointmentId,
                    PatientName = a.PatientName ?? "Unknown Patient",
                    Reason = a.Reason ?? string.Empty,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    Status = a.Status
                }).ToList();

            return new DoctorDashboardViewModel
            {
                DoctorName = doctor.FullName ?? "Doctor",
                TodaysAppointmentsCount = todaysAppointmentsCount,
                PendingRequestsCount = pendingRequestsCount,
                CompletedAppointmentsCount = completedCount,
                TotalPatientsCount = totalPatientsCount,
                TodaysAppointments = todaysAppointments,
                PendingRequests = pendingRequests,
                UpcomingSchedule = upcomingSchedule
            };
        }
    }
}
