namespace SMS.Models
{
    public class Lab
    {
        public int Id { get; set; }
        public string Name { get; set; } = Guid.NewGuid().ToString();

        public string Description { get; set; }
        public ICollection<Problem> Problems { get; set; } = new List<Problem>();


    }
}