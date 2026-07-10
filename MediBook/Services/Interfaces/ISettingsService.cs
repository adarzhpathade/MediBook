using System.Threading.Tasks;
using MediBook.Models.DTOs;

namespace MediBook.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<PatientProfileDto?> GetPatientProfileAsync(int userId);
        Task<bool> UpdatePatientProfileAsync(PatientProfileDto profile);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}
