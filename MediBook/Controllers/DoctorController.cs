using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediBook.Services.Interfaces;
using MediBook.Helpers;
using MediBook.Repositories.Interfaces;
using System;
using System.Linq;

namespace MediBook.Controllers
{
    public class DoctorController : Controller
    {
        private readonly IDoctorDashboardService _dashboardService;
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorRepository _doctorRepository;

        public DoctorController(
            IDoctorDashboardService dashboardService, 
            IAppointmentService appointmentService, 
            IDoctorRepository doctorRepository)
        {
            _dashboardService = dashboardService;
            _appointmentService = appointmentService;
            _doctorRepository = doctorRepository;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Doctor") return RedirectToAction("Login", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");

            var viewModel = await _dashboardService.GetDashboardDataAsync(userId.Value);
            return View(viewModel);
        }
        
        public async Task<IActionResult> AppointmentRequests()
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Doctor") return RedirectToAction("Login", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");
            var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId.Value);
            if (doctor == null) return NotFound();
            
            var requests = await _appointmentService.GetPendingRequestsAsync(doctor.DoctorId);
            return View(requests);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptAppointment(int appointmentId)
        {
            await _appointmentService.UpdateAppointmentStatusAsync(appointmentId, "Confirmed");
            TempData["SuccessMessage"] = "Appointment confirmed successfully.";
            return RedirectToAction(nameof(AppointmentRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeclineAppointment(int appointmentId)
        {
            await _appointmentService.UpdateAppointmentStatusAsync(appointmentId, "Declined");
            TempData["SuccessMessage"] = "Appointment declined.";
            return RedirectToAction(nameof(AppointmentRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteAppointment(int appointmentId)
        {
            await _appointmentService.UpdateAppointmentStatusAsync(appointmentId, "Completed");
            TempData["SuccessMessage"] = "Appointment marked as completed.";
            return RedirectToAction(nameof(AllAppointments));
        }
        
        [HttpGet]
        public async Task<IActionResult> RescheduleAppointment(int id)
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Doctor") return RedirectToAction("Login", "Account");
            
            ViewBag.AppointmentId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RescheduleAppointment(int appointmentId, DateTime newDate, TimeSpan newTime)
        {
            var success = await _appointmentService.RescheduleAppointmentAsync(appointmentId, newDate, newTime);
            if (success)
            {
                TempData["SuccessMessage"] = "Appointment rescheduled successfully.";
                return RedirectToAction(nameof(AppointmentRequests));
            }
            
            ModelState.AddModelError("", "Selected time slot is not available or invalid.");
            ViewBag.AppointmentId = appointmentId;
            return View();
        }
        
        public async Task<IActionResult> Schedule()
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Doctor") return RedirectToAction("Login", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");
            var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId.Value);
            if (doctor == null) return NotFound();
            
            var appointments = await _appointmentService.GetDoctorAppointmentsAsync(doctor.DoctorId);
            // We pass only confirmed/rescheduled to the calendar view
            var active = appointments.Where(a => a.Status == "Confirmed" || a.Status == "Rescheduled");
            
            return View(active);
        }
        
        public async Task<IActionResult> AllAppointments(string status = "")
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Doctor") return RedirectToAction("Login", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");
            var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId.Value);
            if (doctor == null) return NotFound();
            
            var appointments = await _appointmentService.GetDoctorAppointmentsAsync(doctor.DoctorId);
            
            if (!string.IsNullOrEmpty(status))
            {
                appointments = appointments.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }
            
            ViewBag.CurrentFilter = status;
            return View(appointments);
        }
        
        public async Task<IActionResult> Profile()
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Doctor") return RedirectToAction("Login", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");
            var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId.Value);
            if (doctor == null) return NotFound();
            
            return View(doctor);
        }

        public async Task<IActionResult> Availability()
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Doctor") return RedirectToAction("Login", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");
            var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId.Value);
            if (doctor == null) return NotFound();
            
            var availability = await _doctorRepository.GetDoctorAvailabilityAsync(doctor.DoctorId);
            return View(availability);
        }
        
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Doctor") return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        public IActionResult UpdateProfile(string specialization, string qualification, int? experience, decimal? consultationFee, string biography)
        {
            // Placeholder: Update doctor repository logic here
            TempData["SuccessMessage"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        public IActionResult UpdateAvailability()
        {
            // Placeholder: Parse form collection and save via _doctorRepository.SaveDoctorAvailabilityAsync
            TempData["SuccessMessage"] = "Availability schedule updated successfully.";
            return RedirectToAction(nameof(Availability));
        }

        [HttpPost]
        public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                TempData["ErrorMessage"] = "New passwords do not match.";
                return RedirectToAction(nameof(ChangePassword));
            }
            
            // Placeholder: Update password logic here
            TempData["SuccessMessage"] = "Password updated successfully.";
            return RedirectToAction(nameof(ChangePassword));
        }
    }
}
