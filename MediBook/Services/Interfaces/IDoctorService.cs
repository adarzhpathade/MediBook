using System.Collections.Generic;
using System.Threading.Tasks;
using MediBook.Models.DTOs;

namespace MediBook.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetDoctorsAsync(string? specialty = null, string? name = null, bool onlyActive = true);
        Task<DoctorDto?> GetDoctorDetailsAsync(int doctorId);
        Task<bool> ToggleDoctorStatusAsync(int doctorId);
        Task<bool> DeleteDoctorAsync(int doctorId);
        Task<bool> CreateDoctorAsync(MediBook.Models.ViewModels.AddDoctorViewModel model);
    }
}
