using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediBook.Models.DTOs;
using MediBook.Models.Entities;

namespace MediBook.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<int> CreateAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string status);
        Task<AppointmentDto?> GetAppointmentByIdAsync(int appointmentId);
        Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsAsync(int patientId);
        Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId);
        Task<bool> HasOverlappingAppointmentAsync(int doctorId, DateTime date, TimeSpan time);
        Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime);
        Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(string? search = null, string? status = null);
    }
}
