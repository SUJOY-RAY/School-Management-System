using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Models.user_lists;
using SMS.Services.Interfaces;
using SMS.Shared.LOU;
using SMS.Tools;

namespace SMS.Controllers
{

    public class LOUController : Controller
    {
        private readonly IGenericService<ListOfUsers> louService;
        private readonly ContextHandler contextHandler;

        public LOUController(IGenericService<ListOfUsers> louService, ContextHandler contextHandler)
        {
            this.louService = louService;
            this.contextHandler = contextHandler;
        }

        [Authorize]
        public async Task<IActionResult> LOU()
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
            var currUserSchool = await contextHandler.GetCurrentUserSchool() ?? throw new KeyNotFoundException("School not assigned to this user");

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
                        var classroomIdText = worksheet.Cells[row, 3].Text.Trim();
                        var roleText = worksheet.Cells[row, 4].Text.Trim();


                        var user = new ListOfUsers
                        {
                            Email = email,
                            School = currUserSchool,
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

        [Authorize]
        public async Task<IActionResult> UpdateLOU(int id)
        {
            var lou = await louService.GetByIdAsync(id);
            if (lou == null) return NotFound();

            var model = new UpdateLOU
            {
                Id = lou.Id,
                Email = lou.Email,
                Role = lou.Role
            };

            var roles = Enum.GetValues(typeof(Role))
                .Cast<Role>()
                .Where(r => r != Role.Admin)
                .Select(r => new SelectListItem
                {
                    Value = r.ToString(),
                    Text = r.ToString()
                }).ToList();

            ViewBag.Roles = roles;

            return View(model);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateLOU(UpdateLOU updateLOU)
        {
            if (!ModelState.IsValid)
                return View(updateLOU);

            var lou = await louService.GetByIdAsync(updateLOU.Id);
            if (lou == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction("LOU");
            }

            lou.Email = updateLOU.Email;
            lou.Role = updateLOU.Role;

            await louService.UpdateAsync(lou);

            TempData["Success"] = "User updated successfully!";
            return RedirectToAction("LOU");
        }
    }
}
