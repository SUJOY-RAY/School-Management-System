using Microsoft.EntityFrameworkCore;
using SMS.Models;
using SMS.Tools;

namespace SMS.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public User User { get; set; }
        public Classroom Classroom { get; set; }
        public University University { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<Classroom>()
               .HasMany(c => c.Users)
               .WithOne(u => u.Classroom)
               .HasForeignKey(u => u.ClassroomID);

            builder.Entity<University>();
        }
    }
}
