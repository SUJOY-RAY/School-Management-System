using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SMS.Infrastructure.Interfaces;
using SMS.Models;
using SMS.Models.user_lists;
using SMS.Tools;

namespace SMS.Controllers
{
    public class PrincipalController : Controller
    {
        private readonly StaticImageService _imageService;

        public PrincipalController(StaticImageService imageService)
        {
            _imageService = imageService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var controllerName = ControllerContext.RouteData.Values["controller"]?.ToString();

            var images = _imageService.GetStaticImageMappings(controllerName);
            return View(images);
        }
    }
}
