using System.Collections.Generic;
using MediBook.Models.DTOs;

namespace MediBook.Models.ViewModels
{
    public class FindDoctorsViewModel
    {
        public IEnumerable<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();
        public string? SearchSpecialty { get; set; }
        public string? SearchName { get; set; }
        public string PatientName { get; set; } = string.Empty;
    }
}
