using System;
using System.Collections.Generic;

namespace MediBook.Models.ViewModels
{
    public class DashboardViewModel
    {
        public string PatientFirstName { get; set; } = string.Empty;
        
        // Statistics
        public int UpcomingAppointmentsCount { get; set; }
        public int CompletedVisitsCount { get; set; }
        public int MedicalReportsCount { get; set; }
        public int ActivePrescriptionsCount { get; set; }

        // Next Appointment
        public AppointmentSummary? NextAppointment { get; set; }

        // Mock Lists for UI
        public List<RecommendedSpecialist> RecommendedSpecialists { get; set; } = new();
        public List<RecentRecord> RecentRecords { get; set; } = new();
        public List<HealthReminder> HealthReminders { get; set; } = new();
    }

    public class AppointmentSummary
    {
        public int AppointmentId { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string ClinicName { get; set; } = string.Empty;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string Type { get; set; } = "In-Person";
    }

    public class RecommendedSpecialist
    {
        public string Name { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public string IconName { get; set; } = string.Empty;
        public string ThemeColor { get; set; } = string.Empty;
    }

    public class RecentRecord
    {
        public string Title { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
        public string ThemeColor { get; set; } = string.Empty;
    }

    public class HealthReminder
    {
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
        public string ThemeColor { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }
}
