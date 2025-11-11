namespace SMS.Models.school_related
{
    public class Classroom
    {
        public int Id { get; set; } 
        public int SchoolID { get; set; }
        public School School { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public ICollection<User> Users { get; set; } = new List<User>();

        public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    }
}