using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;
using time_trace.Models;

namespace time_trace.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, 
        IDataProtectionKeyContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserSchedule>()
                .HasKey(us => new { us.UserId, us.ScheduleId });

            builder.Entity<UserSchedule>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSchedules)
                .HasForeignKey(us => us.UserId);

            builder.Entity<UserSchedule>()
                .HasOne(us => us.Schedule)
                .WithMany(s => s.UserSchedules)
                .HasForeignKey(us => us.ScheduleId);

            builder.Entity<Schedule>()
                .HasOne(s => s.Owner)
                .WithMany(u => u.OwnedSchedules)
                .HasForeignKey(s => s.OwnerId);

            builder.Entity<TimeSlot>()
                .HasKey(ts => new { ts.DateTime, ts.UserId, ts.ScheduleId });

            builder.Entity<TimeSlot>()
                .HasOne(ts => ts.UserSchedule)
                .WithMany(us => us.TimeSlots)
                .HasForeignKey(ts => new { ts.UserId, ts.ScheduleId });
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<UserSchedule> UserSchedules { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    }
}
