namespace SMS.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;


        public CourseLevel CourseLevel { get; set; }
        public ICollection<string> LearningOutcomes  = new List<string>();
        public int TimeSpent { get; set; }
        public int Grade { get; set; }
        public int completion { get; set; }
        public string Benifits { get; set; } = string.Empty;

        public ICollection<string> Designation { get; set; } = new List<string>();

        public ICollection<Module> Modules { get; set; } = new List<Module>();
        public ICollection<Topic> Topics { get; set; } = new List<Topic>();
        public ICollection<ReadingMaterial> ReadingMaterials { get; set; } = new List<ReadingMaterial>();
        public ICollection<Lab> Labs { get; set; } = new List<Lab>();
        public ICollection<Test> Tests { get; set; } = new List<Test>();

        public ICollection<string> Companies { get; set; } = new List<string>();

        public ICollection<Dictionary<string, string>> FAQ { get; set; } = new List<Dictionary<string, string>>();
    }
}
