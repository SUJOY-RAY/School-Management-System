namespace SMS.Services
{
    public class StaticImageService
    {
        private readonly IWebHostEnvironment _env;

        public StaticImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        /// <summary>
        /// Gets all images from /wwwroot/static/[currentController]
        /// Each image filename determines its target Controller/Action.
        /// </summary>
        public List<(string ImageUrl, string TargetController, string TargetAction)> GetStaticImageMappings(string currentController)
        {
            var staticRoot = Path.Combine(_env.WebRootPath, "static", currentController);
            var model = new List<(string ImageUrl, string TargetController, string TargetAction)>();

            if (!Directory.Exists(staticRoot))
                return model;

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };

            foreach (var file in Directory.GetFiles(staticRoot))
            {
                var ext = Path.GetExtension(file).ToLower();
                if (!validExtensions.Contains(ext))
                    continue;

                var fileNameWithoutExt = Path.GetFileNameWithoutExtension(file);
                string targetController, targetAction;

                // Split filename by '-'
                var parts = fileNameWithoutExt.Split('-', 2, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    // pattern: Controller-Action
                    targetController = parts[0];
                    targetAction = parts[1];
                }
                else
                {
                    // pattern: Controller only, defaults to Index
                    targetController = fileNameWithoutExt;
                    targetAction = "Index";
                }

                var relativePath = Path.Combine("static", currentController, Path.GetFileName(file))
                                           .Replace("\\", "/");

                model.Add((
                    ImageUrl: relativePath,
                    TargetController: targetController,
                    TargetAction: targetAction
                ));
            }

            return model;
        }
    }
}
