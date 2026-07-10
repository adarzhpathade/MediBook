using System.Threading.Tasks;
using MediBook.Models.ViewModels;

namespace MediBook.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardViewModel> GetDashboardDataAsync();
    }
}
