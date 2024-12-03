using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using time_trace.Models;

namespace time_trace.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(
            ILogger<HomeController> logger, 
            SignInManager<ApplicationUser>  signInManager)
        {
            _logger = logger;
            _signInManager = signInManager;

        }

        public IActionResult Index()
        {
            _logger.LogInformation("index page");
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction(nameof(Index), "Schedule");
            } else
            {
                return RedirectToAction("Login", "Account", new { area = "Identity" });
            }
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("privacy page");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
