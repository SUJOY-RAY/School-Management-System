using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SMS.Models;
using SMS.Models.user_lists;
using SMS.Services;
using SMS.Services.Interfaces;
using SMS.Services.Templates;
using SMS.Shared.LOU;
using SMS.Tools;

namespace SMS.Controllers
{

    public class LOUController : Controller
    {
        private readonly GenericService<ListOfUsers> louService;
        private readonly LOUService lOUService;
        private readonly ContextHandler contextHandler;

        public LOUController(GenericService<ListOfUsers> louService, LOUService lOUService, ContextHandler contextHandler)
        {
            this.louService = louService;
            this.lOUService = lOUService;
            this.contextHandler = contextHandler;
        }

        [Authorize(Roles = "Principal")]
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
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a valid Excel file.";
                return View();
            }


            var count = await lOUService.CreateRange(file);  

            TempData["Success"] = $" {count} users uploaded successfully!";
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

        [HttpGet]
        public async Task<IActionResult> DeleteLOU(int id)
        {
            await louService.DeleteAsync(id);
            TempData["Success"] = "User deleted successfully!";

            return RedirectToAction("LOU");
        }
    }
}
