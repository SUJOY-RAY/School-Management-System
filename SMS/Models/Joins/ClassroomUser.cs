using SMS.Models.school_related;

namespace SMS.Models.Joins
{
    public class ClassroomUser
    {
        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
