namespace HostelAPI.Models
{
    public class HousekeepingTask
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool DamageReported { get; set; }
        public string? Comment { get; set; }

        // Navigation
        public required Room Room { get; set; }
    }

}
