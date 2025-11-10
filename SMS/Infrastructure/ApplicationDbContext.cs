using Microsoft.EntityFrameworkCore;
using SMS.Tools;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Models.user_lists;
using SMS.Tools;
using System.Reflection.Emit;
using SMS.Models.Joins;

namespace SMS.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<ListOfUsers> ListOfUsers { get; set; }

        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<ClassroomUser> ClassroomUsers { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(u =>
            {
                u.HasKey(u => u.Id);
            });

            builder.Entity<School>(s =>
            {
                s.HasKey(s => s.Id);

                s.HasMany(s => s.Users)
                 .WithOne(u => u.School)
                 .HasForeignKey(u => u.SchoolID)
                 .OnDelete(DeleteBehavior.Cascade);

                s.HasMany(s => s.Classrooms)
                 .WithOne(c => c.School)
                 .HasForeignKey(c => c.SchoolID)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Classroom>(c =>
            {
                c.HasKey(c => c.Id);

                c.HasMany(c => c.Attendances)
                 .WithOne(a => a.Classroom)
                 .HasForeignKey(u => u.ClassroomID)
                 .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<ListOfUsers>(lou =>
            {
                lou.HasOne(u => u.Classroom)
                   .WithMany()
                   .HasForeignKey(u => u.ClassroomID);
                lou.HasOne(u => u.School)
                   .WithMany()
                   .HasForeignKey(u => u.SchoolID);

            });

            builder.Entity<ClassroomUser>(usr_cls =>
            {
                builder.Entity<ClassroomUser>()
                    .HasOne(cu => cu.Classroom)
                    .WithMany(c => c.ClassroomUsers)
                    .HasForeignKey(cu => cu.ClassroomId);

                builder.Entity<ClassroomUser>()
                    .HasOne(cu => cu.User)
                    .WithMany(u => u.ClassroomUsers)
                    .HasForeignKey(cu => cu.UserId);
            });

            builder.SeedSchool();
        }
    }
}
