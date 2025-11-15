using System.ComponentModel.DataAnnotations;

namespace SMS.Shared.Classroom
{
    public class ClassroomUpdateDto
    {
        [Required]
        [MinLength(3), MaxLength(15)]
        public string Name = string.Empty;
        public List<int>? UserIds { get; set; }  
        public List<int>? AttendanceIdsToRemove { get; set; } 
    }
}
