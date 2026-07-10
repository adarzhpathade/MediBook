using System.Collections.Generic;
using System.Threading.Tasks;
using MediBook.Models.DTOs;

namespace MediBook.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientProfileDto>> GetAllPatientsAsync(string? search = null);
        Task<bool> TogglePatientStatusAsync(int patientId);
        Task<bool> DeletePatientAsync(int patientId);
    }
}
