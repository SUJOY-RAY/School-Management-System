using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SMS.Models;

namespace SMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            // Get user role
            var role = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

            // Default folder (fallback if no role)
            string roleFolder = "default";

            if (!string.IsNullOrEmpty(role))
            {
                // Match role to folder names you created inside Static/
                roleFolder = role switch
                {
                    "Admin" => "admin-index-page-grid",
                    "Principal" => "principal-index-page-grid",
                    "Teacher" => "teacher-index-page-grid",
                    "Student" => "student-index-page-grid",
                    _ => "default"
                };
            }

            var staticFolder = Path.Combine(_env.WebRootPath, "Static", roleFolder);

            var images = Directory.Exists(staticFolder)
                ? Directory.GetFiles(staticFolder).Select(Path.GetFileName).ToList()
                : new List<string>(); // empty if folder missing

            return View(images);
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
