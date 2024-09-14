using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace time_trace.Models
{
    public class UserSchedule
    {
        public string UserId { get; set; } = null!;
        public int ScheduleId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Schedule Schedule { get; set; } = null!;

        public IEnumerable<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
    }
}
