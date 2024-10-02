namespace time_trace.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<UserSchedule> UserSchedules { get; set; } = new List<UserSchedule>();
    }
}
