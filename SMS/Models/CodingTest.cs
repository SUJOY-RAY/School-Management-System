namespace SMS.Models
{
    public class CodingTest
    {
        public int Id { get; set; }
        public int ModuleID { get; set; }
        public Module Module { get; set; }

        public ICollection<Problem> Problems { get; set; } = new List<Problem>();
    }
}