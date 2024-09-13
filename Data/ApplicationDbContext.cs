using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;
using time_trace.Models;

namespace time_trace.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Schedules)
                .WithMany(e => e.ApplicationUsers)
                .UsingEntity<UserSchedule>();
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<UserSchedule> UserSchedules { get; set; }
        public DbSet<TimeRange> TimeRanges { get; set; }


    }
}
