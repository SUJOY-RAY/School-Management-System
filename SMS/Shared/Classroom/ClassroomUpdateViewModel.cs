using SMS.Models;
using SMS.Models.school_related;

namespace SMS.Shared.Classroom
{
    public class ClassroomUpdateViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<User> Users { get; set; } = new();
        public List<Attendance> Attendances { get; set; } = new();

        // Optional: selected IDs for form submission
        public List<int> UserIds { get; set; } = new();
        public List<int> AttendanceIdsToRemove { get; set; } = new();
    }
}
