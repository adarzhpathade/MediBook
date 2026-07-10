using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MediBook.Models.ViewModels;
using MediBook.Services.Interfaces;
using MediBook.Helpers;

namespace MediBook.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterUserAsync(model);
                
                if (result.Success)
                {
                    // For now, redirect to Home until Login is implemented
                    TempData["SuccessMessage"] = "Registration successful! You can now log in.";
                    return RedirectToAction("Index", "Home"); 
                }
                
                ModelState.AddModelError(string.Empty, result.ErrorMessage);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (SessionHelper.IsAuthenticated(HttpContext.Session))
            {
                return RedirectToDashboard();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountService.AuthenticateUserAsync(model.Email, model.Password);
                
                if (user != null)
                {
                    SessionHelper.SetUserSession(HttpContext.Session, user);
                    return RedirectToDashboard();
                }
                
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            SessionHelper.ClearSession(HttpContext.Session);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        private IActionResult RedirectToDashboard()
        {
            var role = SessionHelper.GetUserRole(HttpContext.Session);
            if (role == "Patient") return RedirectToAction("Dashboard", "Patient");
            if (role == "Doctor") return RedirectToAction("Dashboard", "Doctor");
            if (role == "Admin") return RedirectToAction("Dashboard", "Admin");
            return RedirectToAction("Index", "Home");
        }
    }
}
