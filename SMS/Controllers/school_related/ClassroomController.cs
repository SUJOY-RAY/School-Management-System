using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMS.Infrastructure;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Services;
using SMS.Services.Interfaces;
using SMS.Shared;
using SMS.Tools;
using System.Numerics;

namespace SMS.Controllers.school_related
{
    public class ClassroomController : Controller
    {
        private IGenericService<Classroom> classroomService { get; set; }
        private IGenericRepository<User> userRepository { get; set; }
        private ContextHandler contextHandler { get; set; }

        public ClassroomController(IGenericService<Classroom> classroomService, IGenericRepository<User> userRepository, ContextHandler contextHandler)
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
                    classrooms.AddRange(await classroomService.GetFilteredAsync(c => c.Users.Contains(user.Classrooms));

            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ClassroomCreateDto classroomCreateDto)
        {
            return View(classroomService.CreateAsync(classroomCreateDto));
        }
    }
}
