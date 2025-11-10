using System.ComponentModel.DataAnnotations;

namespace SMS.Shared
{
    public class ClassroomCreateDto
    {
        [Required]
        [MinLength(3), MaxLength(15)]
        public string Name = string.Empty;
    }
}