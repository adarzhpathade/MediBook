using System;

namespace MediBook.Models.DTOs
{
    public class DoctorDto
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public int? Experience { get; set; }
        public decimal? ConsultationFee { get; set; }
        public string? Biography { get; set; }
        public string? AvailableDays { get; set; }
        public string? AvailableTime { get; set; }
        public string? ProfileImage { get; set; }
        
        // Calculated field for UI
        public double Rating { get; set; } = 4.8; // Placeholder until Reviews table is implemented
        public bool IsActive { get; set; }
    }
}
