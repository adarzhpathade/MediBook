using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MediBook.Helpers;

namespace MediBook.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower() ?? string.Empty;
            var session = context.Session;

            // Define protected areas
            bool isPatientArea = path.StartsWith("/patient");
            bool isDoctorArea = path.StartsWith("/doctor");
            bool isAdminArea = path.StartsWith("/admin");

            if (isPatientArea || isDoctorArea || isAdminArea)
            {
                if (!SessionHelper.IsAuthenticated(session))
                {
                    context.Response.Redirect("/Account/Login");
                    return;
                }

                var role = SessionHelper.GetUserRole(session);

                if (isPatientArea && role != "Patient")
                {
                    context.Response.Redirect("/Account/AccessDenied");
                    return;
                }
                
                if (isDoctorArea && role != "Doctor")
                {
                    context.Response.Redirect("/Account/AccessDenied");
                    return;
                }

                if (isAdminArea && role != "Admin")
                {
                    context.Response.Redirect("/Account/AccessDenied");
                    return;
                }
            }

            // Allow the request to proceed
            await _next(context);
        }
    }
}
