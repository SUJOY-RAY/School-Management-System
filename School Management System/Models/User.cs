using School_Management_System.Models.school_related;

namespace School_Management_System.Models
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

        public int? ClassroomID { get; set; }
        public Classroom? Classroom { get; set; }
    }
}