using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace time_trace.Models
{
    public class TimeSlot
    {
        [Key]
        public string UserId { get; set; } = null!;

        [Key]
        public int ScheduleId { get; set; }

        [Key]
        public DateTime DateTime { get; set; }
        
        public UserSchedule UserSchedule { get; set; } = null!;

    }
}
