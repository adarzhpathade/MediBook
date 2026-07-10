using System;
using System.Collections.Generic;

namespace MediBook.Models.ViewModels
{
    public class DoctorDashboardViewModel
    {
        public string DoctorName { get; set; } = string.Empty;

        // Statistics
        public int TodaysAppointmentsCount { get; set; }
        public int PendingRequestsCount { get; set; }
        public int CompletedAppointmentsCount { get; set; }
        public int TotalPatientsCount { get; set; }

        // Lists
        public List<DoctorAppointmentSummary> TodaysAppointments { get; set; } = new();
        public List<DoctorAppointmentSummary> PendingRequests { get; set; } = new();
        public List<DoctorAppointmentSummary> UpcomingSchedule { get; set; } = new();
    }

    public class DoctorAppointmentSummary
    {
        public int AppointmentId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
