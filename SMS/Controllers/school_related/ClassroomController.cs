using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SMS.Infrastructure.Interfaces;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Services;
using SMS.Services.Interfaces;
using SMS.Shared;
using SMS.Tools;

namespace SMS.Controllers.school_related
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
        public async Task<IActionResult> All()
        {
            var currUserId = int.Parse(contextHandler.GetCurrentUserId());

            var user = await userRepository.GetByIdAsync(currUserId);
            var classrooms = new List<Classroom>();
            switch (user.Role)
            {
                case Role.Principal:
                    classrooms.AddRange(await classroomService.GetFilteredAsync(c => c.SchoolID == user.SchoolID));
                    break;
                case Role.Teacher:
                case Role.Student:
                    classrooms.AddRange(await classroomService.GetFilteredAsync(
                            c => c.Users.Any(ct => ct.Id == currUserId
                        )));
                    break;
            }

            return View(classrooms);
        }

        [Authorize(Roles = "Principal")]
        public async Task<IActionResult> Create(ClassroomCreateDto classroomCreateDto)
        {
            var currUserId = int.Parse(contextHandler.GetCurrentUserId());

            var user = await userRepository.GetByIdAsync(currUserId);

            Classroom classroom = new Classroom
            {
                Name = classroomCreateDto.Name,
                School = user.School
            };

            return View(classroomService.CreateAsync(classroom));
        }

        [Authorize(Roles = "Principal")]
        public async Task<IActionResult> Delete(int id) {
            var currUserId = int.Parse(contextHandler.GetCurrentUserId());

            var user = await userRepository.GetByIdAsync(currUserId);
            await classroomService.DeleteAsync(id, user);

            return RedirectToAction("All");
        }
    }
}
