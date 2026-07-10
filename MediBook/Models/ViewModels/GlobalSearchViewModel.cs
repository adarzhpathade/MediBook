using System.Collections.Generic;
using MediBook.Models.DTOs;

namespace MediBook.Models.ViewModels
{
    public class GlobalSearchViewModel
    {
        public string SearchTerm { get; set; } = string.Empty;
        public IEnumerable<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();
        public IEnumerable<PatientProfileDto> Patients { get; set; } = new List<PatientProfileDto>();
        public IEnumerable<AppointmentDto> Appointments { get; set; } = new List<AppointmentDto>();
    }
}
