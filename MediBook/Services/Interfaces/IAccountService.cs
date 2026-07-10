using System.Threading.Tasks;
using MediBook.Models.ViewModels;

namespace MediBook.Services.Interfaces
{
    public interface IAccountService
    {
        Task<(bool Success, string ErrorMessage)> RegisterUserAsync(RegisterViewModel model);
        Task<MediBook.Models.Entities.User?> AuthenticateUserAsync(string email, string password);
    }
}
