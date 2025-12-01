namespace SMS.Models
{
    public class Problem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Background { get; set; } = string.Empty;
        public string Objective { get; set; } = string.Empty;

        public ICollection<Dictionary<string, string>> TestCases { get; set; } = new List<Dictionary<string, string>>();
    }
}