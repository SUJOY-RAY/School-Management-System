using Microsoft.EntityFrameworkCore;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Models.user_lists;

namespace SMS.Tools
{
    public static class ModelBuilderExtensions
    {
        public static void SeedSchool(this ModelBuilder builder)
        {
            var schoolId = 1;
            var classroomId = 1;

            // ======== School ========
            builder.Entity<School>().HasData(
                new School
                {
                    Id = schoolId,
                    Name = "Greenwood High School",
                    Description = "A leading school providing quality education.",
                    Active = true
                }
            );

            // ======== Classroom ========
            builder.Entity<Classroom>().HasData(
                new Classroom
                {
                    Id = classroomId,
                    Name = "1A",
                    SchoolID = schoolId
                }
            );

            // ======== ListOfAllowedUsers ========
            builder.Entity<ListOfUsers>().HasData(
                new ListOfUsers
                {
                    Id = 1,
                    SchoolID = schoolId,
                    Email = "sujoy2k4@gmail.com",
                    Role = Role.Admin,
                },
                new ListOfUsers
                {
                    Id = 2,
                    SchoolID = schoolId,
                    Email = "kineticelement1@gmail.com",
                    Role = Role.Principal,
                },
                new ListOfUsers
                {
                    Id = 3,
                    SchoolID = schoolId,
                    Email = "teacher@school.com",
                    Role = Role.Teacher
                },
                new ListOfUsers
                {
                    Id = 4,
                    SchoolID = schoolId,
                    Email = "student@school.com",
                    Role = Role.Student
                }
            );
        }
    }
}
