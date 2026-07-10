using System.Collections.Generic;
using System.Threading.Tasks;
using MediBook.Models.DTOs;

namespace MediBook.Repositories.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<DoctorDto>> GetAllDoctorsAsync(string? specialty = null, string? name = null, bool onlyActive = true);
        Task<DoctorDto?> GetDoctorByIdAsync(int doctorId);
        Task<DoctorDto?> GetDoctorByUserIdAsync(int userId);
        Task<IEnumerable<DoctorAvailabilityDto>> GetDoctorAvailabilityAsync(int doctorId);
        Task SaveDoctorAvailabilityAsync(int doctorId, IEnumerable<DoctorAvailabilityDto> availabilities);
        Task<bool> ToggleDoctorStatusAsync(int doctorId);
        Task<bool> DeleteDoctorAsync(int doctorId);
        Task<bool> CreateDoctorAsync(MediBook.Models.ViewModels.AddDoctorViewModel model);
    }
}
