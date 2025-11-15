using System.ComponentModel.DataAnnotations;

namespace SMS.Shared.Classroom
{
    public class ClassroomCreateDto
    {
        [Required]
        [MinLength(3), MaxLength(15)]
        public string Name { get; set; } = string.Empty;
    }
}