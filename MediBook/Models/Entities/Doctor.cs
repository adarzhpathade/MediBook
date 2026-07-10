using System;

namespace MediBook.Models.Entities
{
    public class Doctor
    {
        public int DoctorId { get; set; }
        public int UserId { get; set; }
        public string Specialization { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public int? Experience { get; set; }
        public decimal? ConsultationFee { get; set; }
        public string? Biography { get; set; }
        public string? AvailableDays { get; set; }
        public string? AvailableTime { get; set; }
        public string? ProfileImage { get; set; }
    }
}
