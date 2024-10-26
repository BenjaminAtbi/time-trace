using Microsoft.AspNetCore.Identity;

namespace time_trace.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Schedule> OwnedSchedules { get; set; } = new List<Schedule>();
        public List<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();
    }
}
