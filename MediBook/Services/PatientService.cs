using System.Collections.Generic;
using System.Threading.Tasks;
using MediBook.Models.DTOs;
using MediBook.Repositories.Interfaces;
using MediBook.Services.Interfaces;

namespace MediBook.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IEnumerable<PatientProfileDto>> GetAllPatientsAsync(string? search = null)
        {
            return await _patientRepository.GetAllPatientsAsync(search);
        }

        public async Task<bool> TogglePatientStatusAsync(int patientId)
        {
            return await _patientRepository.TogglePatientStatusAsync(patientId);
        }

        public async Task<bool> DeletePatientAsync(int patientId)
        {
            return await _patientRepository.DeletePatientAsync(patientId);
        }
    }
}
