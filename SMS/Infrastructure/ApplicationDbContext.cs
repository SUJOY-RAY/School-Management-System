using Microsoft.EntityFrameworkCore;
using SMS.Tools;
using SMS.Models;
using SMS.Models.school_related;
using SMS.Models.user_lists;

namespace SMS.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<ListOfUsers> ListOfUsers { get; set; }

        public DbSet<Classroom> Classrooms { get; set; }

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

                c.HasIndex(c => c.Name)
                 .IsUnique();

                c.HasMany(c => c.Attendances)
                 .WithOne(a => a.Classroom)
                 .HasForeignKey(u => u.ClassroomID)
                 .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(c => c.Users)
                 .WithMany(u => u.Classrooms)
                 .UsingEntity(j => j.ToTable("ClassroomUsers"));
            });

            builder.Entity<ListOfUsers>(lou =>
            {
                lou.HasOne(u => u.School)
                   .WithMany()
                   .HasForeignKey(u => u.SchoolID);
                //lou.HasMany(l => Classrooms)
                //   .WithOne();
            });

            builder.SeedSchool();
        }
    }
}
