using SMS.Models.Joins;
using SMS.Models.school_related;

namespace SMS.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;

        public Role Role { get; set; }
        
        public int? SchoolID { get; set; }
        public School? School { get; set; } = null!;

        public ICollection<ClassroomUser> ClassroomUsers { get; set; } = new List<ClassroomUser>();
    }
}