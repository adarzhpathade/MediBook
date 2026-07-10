using System;

namespace MediBook.Models.DTOs
{
    public class DoctorAvailabilityDto
    {
        public int AvailabilityId { get; set; }
        public int DoctorId { get; set; }
        public int DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int SlotDuration { get; set; }
    }
}
