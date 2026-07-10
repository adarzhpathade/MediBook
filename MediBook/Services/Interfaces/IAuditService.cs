using System.Threading.Tasks;

namespace MediBook.Services.Interfaces
{
    public interface IAuditService
    {
        Task LogActionAsync(string action, string details, int? userId = null);
    }
}
