namespace School_Management_System.Models.school_related
{
    public class Attendance
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public int ClassroomID { get; set; }
        public Classroom Classroom { get; set; }
        public IEnumerable<User> Users { get; set; } = new List<User>();
    }
}