using System;
using System.Threading.Tasks;
using System.Transactions;
using MediBook.Helpers;
using MediBook.Models.Entities;
using MediBook.Models.ViewModels;
using MediBook.Repositories.Interfaces;
using MediBook.Services.Interfaces;

namespace MediBook.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;

        public AccountService(IUserRepository userRepository, IPatientRepository patientRepository)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
        }

        public async Task<(bool Success, string ErrorMessage)> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _userRepository.GetUserByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    return (false, "An account with this email already exists.");
                }

                // Hash password
                var hashedPassword = PasswordHasher.HashPassword(model.Password);

                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = hashedPassword,
                    Role = "Patient",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                // Use TransactionScope to ensure both User and Patient are created together
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var userId = await _userRepository.CreateUserAsync(user);

                    var patient = new Patient
                    {
                        UserId = userId
                    };
                    await _patientRepository.CreatePatientAsync(patient);

                    scope.Complete();
                }

                return (true, string.Empty);
            }
            catch
            {
                // Log exception (not implemented yet)
                return (false, "An error occurred while creating your account. Please try again later.");
            }
        }

        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null || !user.IsActive)
            {
                return null;
            }

            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }
    }
}
