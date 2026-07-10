using System;

namespace MediBook.Models.Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public int UserId { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        
        // Navigation property (not strictly needed for ADO.NET but good for logical representation)
        public User? User { get; set; }
    }
}
