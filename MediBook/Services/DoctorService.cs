using System.Collections.Generic;
using System.Threading.Tasks;
using MediBook.Models.DTOs;
using MediBook.Repositories.Interfaces;
using MediBook.Services.Interfaces;

namespace MediBook.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsAsync(string? specialty = null, string? name = null, bool onlyActive = true)
        {
            return await _doctorRepository.GetAllDoctorsAsync(specialty, name, onlyActive);
        }

        public async Task<DoctorDto?> GetDoctorDetailsAsync(int doctorId)
        {
            return await _doctorRepository.GetDoctorByIdAsync(doctorId);
        }

        public async Task<bool> ToggleDoctorStatusAsync(int doctorId)
        {
            return await _doctorRepository.ToggleDoctorStatusAsync(doctorId);
        }

        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            return await _doctorRepository.DeleteDoctorAsync(doctorId);
        }

        public async Task<bool> CreateDoctorAsync(MediBook.Models.ViewModels.AddDoctorViewModel model)
        {
            return await _doctorRepository.CreateDoctorAsync(model);
        }
    }
}
