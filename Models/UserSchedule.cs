using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace time_trace.Models
{
    public class UserSchedule
    {
        public ApplicationUser User { get; set; } = null!;
        public Schedule Schedule { get; set; } = null!;

        public List<TimeRange> TimeRanges { get; set; } = new List<TimeRange>();
    }
}
