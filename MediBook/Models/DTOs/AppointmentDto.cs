using System;

namespace MediBook.Models.DTOs
{
    public class AppointmentDto
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public TimeSpan AppointmentTime { get; set; }
        public string? Reason { get; set; }
        public string Status { get; set; } = string.Empty;
        
        // Joined fields
        public string DoctorName { get; set; } = string.Empty;
        public string DoctorSpecialization { get; set; } = string.Empty;
        public string? DoctorProfileImage { get; set; }
        public string PatientName { get; set; } = string.Empty;
        
        // Calculated fields for UI
        public string ClinicName { get; set; } = "MediBook Center"; // Placeholder
    }
}
