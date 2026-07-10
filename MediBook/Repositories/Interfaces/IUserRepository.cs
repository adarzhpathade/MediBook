using System.Threading.Tasks;
using MediBook.Models.Entities;

namespace MediBook.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<int> CreateUserAsync(User user);
        Task<User?> GetUserByIdAsync(int userId);
        Task<bool> UpdateUserPasswordAsync(int userId, string passwordHash);
        Task<bool> UpdateUserAsync(User user);
    }
}
