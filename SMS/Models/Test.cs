namespace SMS.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool OnlyOnce { get; set; } = false;
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<User> AttemptedBy { get; set; } = new List<User>();

    }
}