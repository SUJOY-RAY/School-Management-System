namespace SMS.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
        public ICollection<Lab> Labs { get; set; } = new List<Lab>();
        public ICollection<Test> Tests { get; set; } = new List<Test>();
        public ICollection<CodingTest> CodingTests { get; set; } = new List<CodingTest>();
    }
}