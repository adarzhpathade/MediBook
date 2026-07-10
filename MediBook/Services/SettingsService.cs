using System.Threading.Tasks;
using MediBook.Models.DTOs;
using MediBook.Repositories.Interfaces;
using MediBook.Services.Interfaces;
using BCrypt.Net;

namespace MediBook.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IUserRepository _userRepository;

        public SettingsService(IPatientRepository patientRepository, IUserRepository userRepository)
        {
            _patientRepository = patientRepository;
            _userRepository = userRepository;
        }

        public async Task<PatientProfileDto?> GetPatientProfileAsync(int userId)
        {
            return await _patientRepository.GetPatientProfileByUserIdAsync(userId);
        }

        public async Task<bool> UpdatePatientProfileAsync(PatientProfileDto profile)
        {
            return await _patientRepository.UpdatePatientProfileAsync(profile);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
            {
                return false;
            }

            var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            return await _userRepository.UpdateUserPasswordAsync(userId, newHash);
        }
    }
}
