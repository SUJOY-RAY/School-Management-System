namespace School_Management_System.Models.school_related
{
    public class School
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        public IEnumerable<Classroom> Classrooms { get; set; } = new List<Classroom>();

        public IEnumerable<User> Users { get; set; } = new List<User>();

        public bool Active { get; set; } = true;
    }
}
