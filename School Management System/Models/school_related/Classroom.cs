namespace School_Management_System.Models.school_related
{
    public class Classroom
    {
        public int Id { get; set; } 
        public int SchoolID { get; set; }
        public School School { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public IEnumerable<User> Users { get; set; } = new List<User>();

        public IEnumerable<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}
