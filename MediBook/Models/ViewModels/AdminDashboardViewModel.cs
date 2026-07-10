using System;
using System.Collections.Generic;

namespace MediBook.Models.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TotalAppointments { get; set; }
        public int PendingAppointments { get; set; }
        public int CompletedAppointments { get; set; }
        public int TodayAppointments { get; set; }
        public int CancelledAppointments { get; set; }
        public int TotalUsers { get; set; }

        public List<AdminRecentRegistration> RecentRegistrations { get; set; } = new List<AdminRecentRegistration>();
        public List<AdminRecentAppointment> RecentAppointments { get; set; } = new List<AdminRecentAppointment>();
    }

    public class AdminRecentRegistration
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class AdminRecentAppointment
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
