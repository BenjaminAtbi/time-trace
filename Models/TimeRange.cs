namespace time_trace.Models
{
    public class TimeRange
    {
        public int Id { get; set; }
        public int UserScheduleId {  get; set; }
        public UserSchedule UserSchedule { get; set; } = null!;
        public byte[] Data { get; set; } = new byte[3];
    }
}
