using Microsoft.AspNetCore.Mvc;

namespace MediBook.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return View("404");
                case 403:
                    return View("403");
                default:
                    return View("500");
            }
        }

        [Route("Error/500")]
        public IActionResult Error500()
        {
            return View("500");
        }
    }
}
