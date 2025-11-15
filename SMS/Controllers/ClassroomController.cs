using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Infrastructure.Interfaces;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Services;
using SMS.Services.Interfaces;
using SMS.Shared.Classroom;
using SMS.Tools;

namespace SMS.Controllers
{
    public class ClassroomController : Controller
    {
        private ClassroomService classroomService { get; set; }
        private IGenericService<User> userService { get; set; }
        private ContextHandler contextHandler { get; set; }

        public ClassroomController(ClassroomService classroomService, IGenericService<User> userService, ContextHandler contextHandler)
        {
            this.classroomService = classroomService;
            this.userService = userService;
            this.contextHandler = contextHandler;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Classes()
        {
            var currUserId = contextHandler.GetCurrentUserId();

            var user = await userService.GetByIdAsync(currUserId);

            return View(await classroomService.GetAllAsync(user));
        }

        [HttpGet]
        [Authorize]
        public IActionResult CreateClassroom()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateClassroom(ClassroomCreateDto classroomCreateDto)
        {
            try
            {
                var currUserSchool = await contextHandler.GetCurrentUserSchool();
                Classroom classroom = new Classroom
                {
                    Name = classroomCreateDto.Name,
                    School = currUserSchool
                };

                var res = await classroomService.CreateAsync(classroom);

                if (res != null)
                    TempData["Success"] = "Classroom created successfully.";
                else
                    TempData["Error"] = "Error in creating classroom.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An unexpected error occurred: {ex.Message}";
            }

            return RedirectToAction("Classes");
        }

        [Authorize]
        public async Task<IActionResult> UpdateClassroom(int id)
        {
            var classroom = await classroomService.GetByIdAsync(id, c => c.Users, c => c.Attendances);

            if (classroom == null)
                return NotFound();

            // Prepare a DTO or ViewModel for editing
            var model = new ClassroomUpdateViewModel
            {
                Id = classroom.Id,
                Name = classroom.Name,
                Users = classroom.Users.ToList(),
                Attendances = classroom.Attendances.ToList()
            };

            return View(model);
        }



        [Authorize(Roles = "Principal")]
        public async Task<IActionResult> DeleteClassroom(int id)
        {
            var currUserId = contextHandler.GetCurrentUserId();
            try
            {
                var user = await userService.GetByIdAsync(currUserId);
                await classroomService.DeleteAsync(id, user);
                TempData["Success"] = "Classroom deleted successfully.";
            }
            catch (Exception e)
            {
                TempData["Error"] = "Error in deleting classroom.";
            }


            return RedirectToAction("Classes");
        }

        [Authorize]
        public async Task<IActionResult> Users(int id)
        {
            IEnumerable<User> users = new List<User>();

            try
            {

                var classroom = await classroomService.GetByIdAsync(id);
                users = await classroomService.Users(classroom);

                ViewBag.ClassroomName = classroom.Name;
            }
            catch (Exception)
            {
                TempData["Error"] = "Error fetching users in classroom.";
            }

            return View(users);
        }
    }
}