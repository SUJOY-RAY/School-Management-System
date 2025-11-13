using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SMS.Models;
using SMS.Models.user_lists;
using SMS.Services.Interfaces;

namespace SMS.Controllers
{

    public class LOUController : Controller
    {
        private readonly IGenericService<ListOfUsers> louService;

        public LOUController(IGenericService<ListOfUsers> louService)
        {
            this.louService = louService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var lou = await louService.GetAllAsync();
            return View(lou);
        }

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

            await louService.CreateRange(users);  

            TempData["Success"] = $"{users.Count} users uploaded successfully!";
            return RedirectToAction("Index");
        }

    }
}
