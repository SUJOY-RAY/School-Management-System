using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SMS.Models;
using SMS.Tools;

namespace SMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly ContextHandler _contextHandler;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, ContextHandler contextHandler)
        {
            _logger = logger;
            _env = env;
            _contextHandler = contextHandler;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var role = _contextHandler.GetCurrentUserRole();

                return role switch
                {
                    "Admin" => RedirectToAction("Index", "Admin"),
                    "Principal" => RedirectToAction("Index", "Principal"),
                    "Teacher" => RedirectToAction("Index", "Teacher"),
                    "Student" => RedirectToAction("Index", "Student"),
                    _ => RedirectToAction("Index", "Home")
                };
            }

            return View();
        }
    }
}
