using System.Threading.Tasks;
using MediBook.Models.ViewModels;

namespace MediBook.Services.Interfaces
{
    public interface IDoctorDashboardService
    {
        Task<DoctorDashboardViewModel> GetDashboardDataAsync(int userId);
    }
}
