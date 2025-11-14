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
        private IGenericRepository<User> userRepository { get; set; }
        private ContextHandler contextHandler { get; set; }

        public ClassroomController(ClassroomService classroomService, IGenericRepository<User> userRepository, ContextHandler contextHandler)
        {
            this.classroomService = classroomService;
            this.userRepository = userRepository;
            this.contextHandler = contextHandler;
        }

        [Authorize]
        public async Task<IActionResult> AllClasses()
        {
            var currUserId = contextHandler.GetCurrentUserId();

            var user = await userRepository.GetByIdAsync(currUserId);

            return View(await classroomService.GetAllAsync(user));
        }

        [Authorize(Roles = "Principal")]
        public async Task<IActionResult> Create(ClassroomCreateDto classroomCreateDto)
        {
            var currUserId = contextHandler.GetCurrentUserId();

            var user = await userRepository.GetByIdAsync(currUserId);
            var school = await contextHandler.GetCurrentUserSchool() ?? throw new KeyNotFoundException("User not assigned any school.");

            Classroom classroom = new Classroom
            {
                Name = classroomCreateDto.Name,
                School = school
            };

            return View(await classroomService.CreateAsync(classroom));
        }

        [Authorize(Roles = "Principal")]
        public async Task<IActionResult> Delete(int id)
        {
            var currUserId = contextHandler.GetCurrentUserId();

            var user = await userRepository.GetByIdAsync(currUserId);
            await classroomService.DeleteAsync(id, user);

            return RedirectToAction("All");
        }
    }
}