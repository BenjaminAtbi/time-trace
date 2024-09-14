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
                .WithMany(e => e.Users)
                .UsingEntity<UserSchedule>();

            builder.Entity<TimeSlot>()
                .HasKey(nameof(TimeSlot.UserScheduleId), nameof(TimeSlot.DateTime));
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<UserSchedule> UserSchedules { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }


    }
}
