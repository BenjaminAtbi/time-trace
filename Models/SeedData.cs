using Microsoft.EntityFrameworkCore;
using time_trace.Data;

namespace time_trace.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.Schedule.Any())
                {
                    return;
                }
                context.Schedule.Add(
                    new Schedule
                    {
                        Name = "Default Schedule"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
