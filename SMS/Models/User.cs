namespace SMS.Models
{
    public class User
    {
        public int Id { get; set; }
        public int UnivID { get; set; }

        public int ClassroomID { get; set; }
        public Classroom Classroom { get; set; } = null!;

        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;

        public int UnivRank { get; set; }
        public int ClassroomRank { get; set; }
        public int OverallScore { get; set; }

        public int TestsDone { get; set; }
        public int LongestStreak { get; set; }

        public Role Role { get; set; }
    }
}