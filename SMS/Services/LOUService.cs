using OfficeOpenXml;
using SMS.Infrastructure.Interfaces;
using SMS.Models;
using SMS.Models.user_lists;
using SMS.Services.Templates;
using SMS.Tools;

namespace SMS.Services
{
    public class LOUService : GenericService<ListOfUsers>
    {
        private readonly ContextHandler contextHandler; 
        private readonly IGenericRepository<ListOfUsers> louRepository;
        public LOUService(
            IGenericRepository<ListOfUsers> repository,   // ✔ Correct type
            ContextHandler contextHandler
        ) : base(repository)
        {
            this.contextHandler = contextHandler;
            this.louRepository = repository;
        }


        public async Task<int> CreateRange(IFormFile excelFile)
        {
            // 1. Validate file exists
            if (excelFile == null || excelFile.Length == 0)
                throw new Exception("No file was uploaded.");

            // 2. Validate extension
            var ext = Path.GetExtension(excelFile.FileName).ToLower();
            if (ext != ".xlsx")
                throw new Exception("Invalid file format. Only .xlsx files are allowed.");

            // 3. Validate file size (limit: 5 MB)
            if (excelFile.Length > 5 * 1024 * 1024)
                throw new Exception("File too large. Maximum allowed size is 5 MB.");

            // COPY FILE INTO MEMORY
            using var stream = new MemoryStream();
            await excelFile.CopyToAsync(stream);


            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet == null)
                throw new Exception("Excel file contains no worksheets.");

            // 4. Validate required columns
            string colEmail = worksheet.Cells[1, 1].Text.Trim().ToLower();
            string colClassrooms = worksheet.Cells[1, 2].Text.Trim().ToLower();
            string colRole = worksheet.Cells[1, 3].Text.Trim().ToLower();

            if (colEmail != "email" || colClassrooms != "classroomid(s)" || colRole != "role")
            {
                throw new Exception(
                    "Invalid Excel format. Required header row: Email | ClassroomID(s) | Role"
                );
            }

            List<ListOfUsers> entities = new();
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var email = worksheet.Cells[row, 1].Text.Trim();
                var classroomIdsRaw = worksheet.Cells[row, 2].Text.Trim();
                var roleText = worksheet.Cells[row, 3].Text.Trim();
                Role parsedRole; 
                if (!Enum.TryParse(roleText, true, out parsedRole))
                {
                    throw new Exception($"Invalid role '{roleText}' at row {row}. Allowed values: {string.Join(", ", Enum.GetNames(typeof(Role)))}");
                }


                if (string.IsNullOrWhiteSpace(email))
                    continue; // skip empty rows

                var lou = new ListOfUsers
                {
                    Email = email,
                    School = await contextHandler.GetCurrentUserSchool(),
                    Role = parsedRole
                };

                entities.Add(lou);
            }
            await louRepository.AddAllAsync(entities);
            return rowCount;
        }
    }
}
