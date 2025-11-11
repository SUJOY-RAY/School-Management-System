using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SMS.Infrastructure;
using SMS.Models;
using SMS.Models.user_lists;
using SMS.Tools;

namespace SMS.Controllers.Principal
{
    public class PrincipalController : Controller
    {
        private readonly ContextHandler _contextHandler;
        private readonly IWebHostEnvironment _env;
        private readonly IGenericRepository<ListOfUsers> _louRepo;

        public PrincipalController(ContextHandler contextHandler, IWebHostEnvironment env, IGenericRepository<ListOfUsers> louRepo)
        {
            _contextHandler = contextHandler;
            _env = env;
            _louRepo = louRepo;
        }

        [Authorize]
        public IActionResult Index()
        {
            var folderPath = Path.Combine(_env.WebRootPath, "static", "Principal");
            var files = Directory.Exists(folderPath)
                ? Directory.GetFiles(folderPath)
                    .Select(Path.GetFileName)
                    .ToList()
                : new List<string>();
            var images = files.Select(f => (ImageName: f, TargetController: "Student", TargetAction: "Index"))
                  .ToList();

            return View(images);
        }

        [HttpGet]
        [Authorize]
        public IActionResult UploadLOU()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UploadLOU(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a valid Excel file.";
                return View();
            }
            var users = new List<ListOfUsers>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.First();
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++) 
                    {
                        var email = worksheet.Cells[row, 1].Text.Trim();
                        var schoolIdText = worksheet.Cells[row, 2].Text.Trim();
                        var classroomIdText = worksheet.Cells[row, 3].Text.Trim();
                        var roleText = worksheet.Cells[row, 4].Text.Trim();

                        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(schoolIdText))
                            continue; // skip invalid rows

                        var user = new ListOfUsers
                        {
                            Email = email,
                            SchoolID = int.Parse(schoolIdText),
                            Role = Enum.TryParse<Role>(roleText, true, out var role) ? role : Role.Student
                        };

                        users.Add(user);
                    }
                }
            }
            await _louRepo.AddAllAsync(users);

            TempData["Success"] = $"{users.Count} users uploaded successfully!";
            return RedirectToAction("Index");
        }
    }
}
