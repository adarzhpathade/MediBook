using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediBook.Models.DTOs;

namespace MediBook.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsAsync(int patientId);
        Task<AppointmentDto?> GetAppointmentDetailsAsync(int appointmentId);
        Task<bool> BookAppointmentAsync(int patientId, int doctorId, DateTime date, TimeSpan time, string? reason);
        Task<bool> CancelAppointmentAsync(int appointmentId);
        Task<IEnumerable<TimeSpan>> GetAvailableSlotsAsync(int doctorId, DateTime date);
        Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId);
        Task<IEnumerable<AppointmentDto>> GetPendingRequestsAsync(int doctorId);
        Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string status);
        Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime);
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(string? search = null, string? status = null);
    }
}
