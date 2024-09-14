using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace time_trace.Models
{
    public class TimeSlot
    {
        public int UserScheduleId {  get; set; }
        public UserSchedule UserSchedule { get; set; } = null!;
        public DateTime DateTime { get; set; }
    }
}
