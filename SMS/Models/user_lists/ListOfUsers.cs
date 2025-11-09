using SMS.Models.school_related;

namespace SMS.Models.user_lists
{
    public class ListOfUsers
    {
        public int Id { get; set; }

        public int SchoolID { get; set; }
        public School School { get; set; } = null!;

        public int? ClassroomID { get; set; }
        public Classroom? Classroom { get; set; }
        
        public string Email { get; set; } = string.Empty;
        public Role Role { get; set; }
    }
}
