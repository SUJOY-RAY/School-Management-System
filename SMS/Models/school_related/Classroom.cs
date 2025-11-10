using SMS.Models.Joins;

namespace SMS.Models.school_related
{
    public class Classroom
    {
        public int Id { get; set; } 
        public int SchoolID { get; set; }
        public School School { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public ICollection<ClassroomUser> ClassroomUsers { get; set; } = new List<ClassroomUser>();

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}