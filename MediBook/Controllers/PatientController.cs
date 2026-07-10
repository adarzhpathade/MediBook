using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediBook.Services.Interfaces;
using MediBook.Helpers;

namespace MediBook.Controllers
{
    public class PatientController : Controller
    {
        private readonly IDashboardService _dashboardService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        private readonly ISettingsService _settingsService;

        public PatientController(IDashboardService dashboardService, IDoctorService doctorService, IAppointmentService appointmentService, ISettingsService settingsService)
        {
            _dashboardService = dashboardService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
            _settingsService = settingsService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Patient") return RedirectToAction("AccessDenied", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            var fullName = SessionHelper.GetUserName(HttpContext.Session);

            if (userId == null || string.IsNullOrEmpty(fullName)) return RedirectToAction("Login", "Account");

            var vm = await _dashboardService.GetPatientDashboardDataAsync(userId.Value, fullName);
            return View(vm);
        }

        public async Task<IActionResult> FindDoctors(string? specialty, string? name)
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Patient") return RedirectToAction("AccessDenied", "Account");
            
            var vm = new MediBook.Models.ViewModels.FindDoctorsViewModel
            {
                Doctors = await _doctorService.GetDoctorsAsync(specialty, name),
                SearchSpecialty = specialty,
                SearchName = name,
                PatientName = SessionHelper.GetUserName(HttpContext.Session) ?? ""
            };

            return View(vm);
        }

        public async Task<IActionResult> BookAppointment(int id)
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Patient") return RedirectToAction("AccessDenied", "Account");

            var doctor = await _doctorService.GetDoctorDetailsAsync(id);
            if (doctor == null) return NotFound();

            var vm = new MediBook.Models.ViewModels.BookAppointmentViewModel
            {
                Doctor = doctor,
                DoctorId = doctor.DoctorId,
                PatientName = SessionHelper.GetUserName(HttpContext.Session) ?? ""
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BookAppointment(MediBook.Models.ViewModels.BookAppointmentViewModel model)
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Patient") return RedirectToAction("AccessDenied", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");
            
            var profile = await _settingsService.GetPatientProfileAsync(userId.Value);
            if (profile == null) return RedirectToAction("Login", "Account");
            var patientId = profile.PatientId;

            if (ModelState.IsValid)
            {
                var success = await _appointmentService.BookAppointmentAsync(patientId, model.DoctorId, model.SelectedDate, model.SelectedTime, model.Reason);
                if (success)
                {
                    TempData["SuccessMessage"] = "Appointment request submitted! The doctor will review it shortly.";
                    return RedirectToAction("MyAppointments");
                }
                ModelState.AddModelError("", "The selected time slot is no longer available or invalid.");
            }

            model.Doctor = await _doctorService.GetDoctorDetailsAsync(model.DoctorId) ?? new MediBook.Models.DTOs.DoctorDto();
            model.PatientName = SessionHelper.GetUserName(HttpContext.Session) ?? "";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableSlots(int doctorId, string date)
        {
            if (DateTime.TryParse(date, out DateTime parsedDate))
            {
                var slots = await _appointmentService.GetAvailableSlotsAsync(doctorId, parsedDate);
                var formattedSlots = new System.Collections.Generic.List<string>();
                foreach(var slot in slots)
                {
                    formattedSlots.Add(new DateTime(parsedDate.Year, parsedDate.Month, parsedDate.Day, slot.Hours, slot.Minutes, 0).ToString("hh:mm tt"));
                }
                return Json(formattedSlots);
            }
            return Json(new string[] { });
        }

        public async Task<IActionResult> MyAppointments()
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Patient") return RedirectToAction("AccessDenied", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");
            
            var profile = await _settingsService.GetPatientProfileAsync(userId.Value);
            if (profile == null) return RedirectToAction("Login", "Account");
            var patientId = profile.PatientId;

            var allAppointments = await _appointmentService.GetPatientAppointmentsAsync(patientId);
            
            var upcoming = new System.Collections.Generic.List<MediBook.Models.DTOs.AppointmentDto>();
            var past = new System.Collections.Generic.List<MediBook.Models.DTOs.AppointmentDto>();

            var now = DateTime.UtcNow;
            foreach(var appt in allAppointments)
            {
                var apptDateTime = appt.AppointmentDate.Add(appt.AppointmentTime);
                if (apptDateTime >= now && appt.Status != "Cancelled" && appt.Status != "Completed" && appt.Status != "Declined")
                    upcoming.Add(appt);
                else
                    past.Add(appt);
            }

            var vm = new MediBook.Models.ViewModels.MyAppointmentsViewModel
            {
                UpcomingAppointments = upcoming,
                PastAppointments = past,
                PatientName = SessionHelper.GetUserName(HttpContext.Session) ?? ""
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Patient") return RedirectToAction("AccessDenied", "Account");

            var success = await _appointmentService.CancelAppointmentAsync(appointmentId);
            if (!success)
            {
                TempData["ErrorMessage"] = "Cannot cancel appointment within 24 hours or appointment not found.";
            }
            else
            {
                TempData["SuccessMessage"] = "Appointment cancelled successfully.";
            }

            return RedirectToAction("MyAppointments");
        }

        public async Task<IActionResult> Settings()
        {
            var userRole = SessionHelper.GetUserRole(HttpContext.Session);
            if (userRole != "Patient") return RedirectToAction("AccessDenied", "Account");

            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");

            var profile = await _settingsService.GetPatientProfileAsync(userId.Value);
            
            var vm = new MediBook.Models.ViewModels.SettingsViewModel
            {
                Profile = profile ?? new MediBook.Models.DTOs.PatientProfileDto(),
                PatientName = SessionHelper.GetUserName(HttpContext.Session) ?? ""
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(MediBook.Models.ViewModels.SettingsViewModel model)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");
            
            model.Profile.UserId = userId.Value;
            var success = await _settingsService.UpdatePatientProfileAsync(model.Profile);
            
            if(success)
            {
                var sessionUser = new MediBook.Models.Entities.User 
                { 
                    UserId = userId.Value, 
                    FullName = model.Profile.FullName, 
                    Email = model.Profile.Email, 
                    Role = "Patient" 
                };
                SessionHelper.SetUserSession(HttpContext.Session, sessionUser);
                model.PatientName = model.Profile.FullName;
                model.SuccessMessage = "Profile updated successfully.";
            }
            else
            {
                model.ErrorMessage = "Failed to update profile.";
            }

            return View("Settings", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(MediBook.Models.ViewModels.SettingsViewModel model)
        {
            var userId = SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");

            if (string.IsNullOrEmpty(model.CurrentPassword) || string.IsNullOrEmpty(model.NewPassword) || model.NewPassword != model.ConfirmNewPassword)
            {
                model.ErrorMessage = "Please check your password inputs.";
                model.Profile = await _settingsService.GetPatientProfileAsync(userId.Value) ?? new MediBook.Models.DTOs.PatientProfileDto();
                return View("Settings", model);
            }

            var success = await _settingsService.ChangePasswordAsync(userId.Value, model.CurrentPassword, model.NewPassword);
            
            if(success)
                model.SuccessMessage = "Password changed successfully.";
            else
                model.ErrorMessage = "Invalid current password.";

            model.Profile = await _settingsService.GetPatientProfileAsync(userId.Value) ?? new MediBook.Models.DTOs.PatientProfileDto();
            model.PatientName = SessionHelper.GetUserName(HttpContext.Session) ?? "";
            
            return View("Settings", model);
        }
    }
}
