using Microsoft.AspNetCore.Mvc;
using Preguntado.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Preguntado.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userName = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.Name)?.Value : "Invitado";
            var userId = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : "No ID";

            ViewData["UserName"] = userName;
            ViewData["UserId"] = userId;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
