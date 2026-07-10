using System;
using System.Collections.Generic;
using MediBook.Models.DTOs;

namespace MediBook.Models.ViewModels
{
    public class BookAppointmentViewModel
    {
        public DoctorDto Doctor { get; set; } = new DoctorDto();
        public string PatientName { get; set; } = string.Empty;
        
        // These will be used for the form submission
        public int DoctorId { get; set; }
        public DateTime SelectedDate { get; set; }
        public TimeSpan SelectedTime { get; set; }
        public string? Reason { get; set; }
    }
}
