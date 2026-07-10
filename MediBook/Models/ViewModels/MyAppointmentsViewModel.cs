using System.Collections.Generic;
using MediBook.Models.DTOs;

namespace MediBook.Models.ViewModels
{
    public class MyAppointmentsViewModel
    {
        public IEnumerable<AppointmentDto> UpcomingAppointments { get; set; } = new List<AppointmentDto>();
        public IEnumerable<AppointmentDto> PastAppointments { get; set; } = new List<AppointmentDto>();
        public string PatientName { get; set; } = string.Empty;
    }
}
