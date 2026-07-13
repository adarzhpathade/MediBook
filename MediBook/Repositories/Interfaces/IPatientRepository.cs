using System.Threading.Tasks;
using MediBook.Models.Entities;
using MediBook.Models.DTOs;

namespace MediBook.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<int> CreatePatientAsync(Patient patient, Npgsql.NpgsqlTransaction? transaction = null);
        Task<PatientProfileDto?> GetPatientProfileByUserIdAsync(int userId);
        Task<bool> UpdatePatientProfileAsync(PatientProfileDto profile);
        Task<System.Collections.Generic.IEnumerable<PatientProfileDto>> GetAllPatientsAsync(string? search = null);
        Task<bool> TogglePatientStatusAsync(int patientId);
        Task<bool> DeletePatientAsync(int patientId);
    }
}
