using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediBook.Services.Interfaces;
using MediBook.Helpers;

namespace MediBook.Controllers
{
    /// <summary>
    /// Admin controller. Access is enforced by AuthenticationMiddleware (session-based role check).
    /// </summary>
    public class AdminController : Controller
    {
        private readonly IAdminDashboardService _dashboardService;
        private readonly IDoctorService _doctorService;
        private readonly IAuditService _auditService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly MediBook.Repositories.Interfaces.IUserRepository _userRepository;

        public AdminController(
            IAdminDashboardService dashboardService, 
            IDoctorService doctorService, 
            IAuditService auditService, 
            IPatientService patientService, 
            IAppointmentService appointmentService,
            MediBook.Repositories.Interfaces.IUserRepository userRepository)
        {
            _dashboardService = dashboardService;
            _doctorService = doctorService;
            _auditService = auditService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Dashboard()
        {
            var vm = await _dashboardService.GetDashboardDataAsync();
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ManageDoctors(string search)
        {
            ViewBag.SearchTerm = search;
            // Fetch all doctors, including inactive ones for Admin management
            var doctors = await _doctorService.GetDoctorsAsync(null, search, onlyActive: false);
            return View(doctors);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleDoctorStatus(int id)
        {
            var success = await _doctorService.ToggleDoctorStatusAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Doctor status updated successfully.";
                await _auditService.LogActionAsync("Doctor Updated", $"Toggled status for DoctorId: {id}");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update doctor status.";
            }
            return RedirectToAction("ManageDoctors");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var success = await _doctorService.DeleteDoctorAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Doctor deleted successfully.";
                await _auditService.LogActionAsync("Doctor Deleted", $"Deleted DoctorId: {id}");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete doctor.";
            }
            return RedirectToAction("ManageDoctors");
        }

        [HttpGet]
        public IActionResult AddDoctor()
        {
            return View(new MediBook.Models.ViewModels.AddDoctorViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor(MediBook.Models.ViewModels.AddDoctorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _doctorService.CreateDoctorAsync(model);
                if (success)
                {
                    TempData["SuccessMessage"] = "Doctor created successfully.";
                    await _auditService.LogActionAsync("Doctor Created", $"Created Doctor: {model.FullName} ({model.Email})");
                    return RedirectToAction("ManageDoctors");
                }
                ModelState.AddModelError("", "Email may already be in use or an error occurred.");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ManagePatients(string search)
        {
            ViewBag.SearchTerm = search;
            var patients = await _patientService.GetAllPatientsAsync(search);
            return View(patients);
        }

        [HttpPost]
        public async Task<IActionResult> TogglePatientStatus(int id)
        {
            var success = await _patientService.TogglePatientStatusAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Patient status updated successfully.";
                await _auditService.LogActionAsync("Patient Updated", $"Toggled status for PatientId: {id}");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update patient status.";
            }
            return RedirectToAction("ManagePatients");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var success = await _patientService.DeletePatientAsync(id);
            if (success)
            {
                TempData["SuccessMessage"] = "Patient deleted successfully.";
                await _auditService.LogActionAsync("Patient Deleted", $"Deleted PatientId: {id}");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete patient.";
            }
            return RedirectToAction("ManagePatients");
        }

        [HttpGet]
        public async Task<IActionResult> ManageAppointments(string search, string status)
        {
            ViewBag.SearchTerm = search;
            ViewBag.StatusFilter = status;
            var appointments = await _appointmentService.GetAllAppointmentsAsync(search, status);
            return View(appointments);
        }

        [HttpGet]
        public async Task<IActionResult> AppointmentDetails(int id)
        {
            var appointment = await _appointmentService.GetAppointmentDetailsAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        [HttpGet]
        public async Task<IActionResult> GlobalSearch(string query)
        {
            var vm = new MediBook.Models.ViewModels.GlobalSearchViewModel
            {
                SearchTerm = query ?? string.Empty
            };

            if (!string.IsNullOrWhiteSpace(query))
            {
                vm.Doctors = await _doctorService.GetDoctorsAsync(null, query);
                vm.Patients = await _patientService.GetAllPatientsAsync(query);
                vm.Appointments = await _appointmentService.GetAllAppointmentsAsync(query, null);
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = MediBook.Helpers.SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _userRepository.GetUserByIdAsync(userId.Value);
            if (user == null) return NotFound();

            var vm = new MediBook.Models.ViewModels.AdminProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(MediBook.Models.ViewModels.AdminProfileViewModel model)
        {
            var userId = MediBook.Helpers.SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByIdAsync(userId.Value);
                if (user != null)
                {
                    user.FullName = model.FullName;
                    // Note: Update email functionality depends on repository support. Assuming simple update.
                    var success = await _userRepository.UpdateUserAsync(user);
                    if (success)
                    {
                        model.SuccessMessage = "Profile updated successfully.";
                        SessionHelper.SetUserSession(HttpContext.Session, user);
                        await _auditService.LogActionAsync("Admin Profile Updated", $"Updated name to {model.FullName}", userId.Value);
                    }
                    else
                    {
                        model.ErrorMessage = "Failed to update profile.";
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View(new MediBook.Models.ViewModels.AdminChangePasswordViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(MediBook.Models.ViewModels.AdminChangePasswordViewModel model)
        {
            var userId = MediBook.Helpers.SessionHelper.GetUserId(HttpContext.Session);
            if (userId == null) return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByIdAsync(userId.Value);
                if (user != null)
                {
                    if (MediBook.Helpers.PasswordHasher.VerifyPassword(model.CurrentPassword, user.PasswordHash))
                    {
                        user.PasswordHash = MediBook.Helpers.PasswordHasher.HashPassword(model.NewPassword);
                        var success = await _userRepository.UpdateUserAsync(user);
                        if (success)
                        {
                            model.SuccessMessage = "Password changed successfully.";
                            await _auditService.LogActionAsync("Admin Password Changed", "Admin changed their password", userId.Value);
                        }
                        else
                        {
                            model.ErrorMessage = "Failed to update password.";
                        }
                    }
                    else
                    {
                        model.ErrorMessage = "Invalid current password.";
                    }
                }
            }
            return View(model);
        }
    }
}
