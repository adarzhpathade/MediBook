using System.Threading.Tasks;
using MediBook.Models.ViewModels;

namespace MediBook.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> GetPatientDashboardDataAsync(int userId, string fullName);
    }
}
