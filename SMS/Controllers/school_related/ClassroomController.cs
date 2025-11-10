using Microsoft.AspNetCore.Mvc;
using SMS.Infrastructure;
using SMS.Models.school_related;
using SMS.Tools;

namespace SMS.Controllers.school_related
{
    public class ClassroomController : Controller
    {
        private readonly ICrudRepository<Classroom> _clsRepo;
        private readonly ContextHandler handler;

        public ClassroomController(ICrudRepository<Classroom> clsRepo, ContextHandler handler)
        {
            _clsRepo = clsRepo;
            this.handler = handler;
        }


        public async Task<IActionResult> Index() {
            var currUserRole = handler.GetCurrentUserRole();
            switch (currUserRole):

            return View();
        }
    }
}
