using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediBook.Models.DTOs;
using MediBook.Models.Entities;
using MediBook.Repositories.Interfaces;
using MediBook.Services.Interfaces;
using System.Linq;

namespace MediBook.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepository;
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<AppointmentDto>> GetPatientAppointmentsAsync(int patientId)
        {
            return await _appointmentRepository.GetPatientAppointmentsAsync(patientId);
        }

        public async Task<AppointmentDto?> GetAppointmentDetailsAsync(int appointmentId)
        {
            return await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
        }

        public async Task<bool> BookAppointmentAsync(int patientId, int doctorId, DateTime date, TimeSpan time, string? reason)
        {
            // Validate: No past appointments and within 5 days
            if (date.Date < DateTime.UtcNow.Date || (date.Date == DateTime.UtcNow.Date && time < DateTime.UtcNow.TimeOfDay) || date.Date > DateTime.UtcNow.AddDays(5).Date)
            {
                return false;
            }

            // Validate: No overlaps for the doctor
            var hasOverlap = await _appointmentRepository.HasOverlappingAppointmentAsync(doctorId, date, time);
            if (hasOverlap)
            {
                return false;
            }

            var appointment = new Appointment
            {
                PatientId = patientId,
                DoctorId = doctorId,
                AppointmentDate = date,
                AppointmentTime = time,
                Reason = reason,
                Status = "Pending" // Requires doctor approval before confirmation
            };

            var id = await _appointmentRepository.CreateAppointmentAsync(appointment);
            return id > 0;
        }

        public async Task<bool> CancelAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null) return false;

            // Rule: Cannot cancel within 24 hours
            var appointmentDateTime = appointment.AppointmentDate.Add(appointment.AppointmentTime);
            if ((appointmentDateTime - DateTime.UtcNow).TotalHours < 24)
            {
                return false; // Too close to appointment time
            }

            return await _appointmentRepository.UpdateAppointmentStatusAsync(appointmentId, "Cancelled");
        }

        public async Task<IEnumerable<TimeSpan>> GetAvailableSlotsAsync(int doctorId, DateTime date)
        {
            var availableSlots = new List<TimeSpan>();
            
            // Do not show past slots for today or slots beyond 5 days
            if (date.Date < DateTime.UtcNow.Date || date.Date > DateTime.UtcNow.AddDays(5).Date) return availableSlots;

            var availabilities = await _doctorRepository.GetDoctorAvailabilityAsync(doctorId);
            var dayOfWeek = (int)date.DayOfWeek;
            
            var dayAvailability = availabilities.Where(a => a.DayOfWeek == dayOfWeek);
            
            foreach (var rule in dayAvailability)
            {
                var currentTime = rule.StartTime;
                while (currentTime + TimeSpan.FromMinutes(rule.SlotDuration) <= rule.EndTime)
                {
                    if (date.Date > DateTime.UtcNow.Date || (date.Date == DateTime.UtcNow.Date && currentTime > DateTime.UtcNow.TimeOfDay))
                    {
                        var isBooked = await _appointmentRepository.HasOverlappingAppointmentAsync(doctorId, date, currentTime);
                        if (!isBooked)
                        {
                            availableSlots.Add(currentTime);
                        }
                    }
                    currentTime = currentTime + TimeSpan.FromMinutes(rule.SlotDuration);
                }
            }

            return availableSlots;
        }

        public async Task<IEnumerable<AppointmentDto>> GetDoctorAppointmentsAsync(int doctorId)
        {
            return await _appointmentRepository.GetDoctorAppointmentsAsync(doctorId);
        }

        public async Task<IEnumerable<AppointmentDto>> GetPendingRequestsAsync(int doctorId)
        {
            var appointments = await _appointmentRepository.GetDoctorAppointmentsAsync(doctorId);
            return appointments.Where(a => a.Status == "Pending");
        }

        public async Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string status)
        {
            return await _appointmentRepository.UpdateAppointmentStatusAsync(appointmentId, status);
        }

        public async Task<bool> RescheduleAppointmentAsync(int appointmentId, DateTime newDate, TimeSpan newTime)
        {
            var appt = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);
            if (appt == null) return false;

            if (newDate.Date < DateTime.UtcNow.Date || newDate.Date > DateTime.UtcNow.AddDays(5).Date) return false;

            bool isOverlapping = await _appointmentRepository.HasOverlappingAppointmentAsync(appt.DoctorId, newDate, newTime);
            if (isOverlapping) return false;

            return await _appointmentRepository.RescheduleAppointmentAsync(appointmentId, newDate, newTime);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointmentsAsync(string? search = null, string? status = null)
        {
            return await _appointmentRepository.GetAllAppointmentsAsync(search, status);
        }
    }
}
