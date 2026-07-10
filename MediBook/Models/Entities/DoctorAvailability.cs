using System;
using System.ComponentModel.DataAnnotations;

namespace MediBook.Models.Entities
{
    public class DoctorAvailability
    {
        [Key]
        public int AvailabilityId { get; set; }

        public int DoctorId { get; set; }
        
        public Doctor? Doctor { get; set; }

        [Required]
        public int DayOfWeek { get; set; } // 0 = Sunday, 1 = Monday, ..., 6 = Saturday

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public int SlotDuration { get; set; } // in minutes

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
