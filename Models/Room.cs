using HostelAPI.Enums;

namespace HostelAPI.Models
{
    public class Room
    {
        public required int Id { get; set; }
        public required string Number { get; set; }  // e.g., "101"
        public required RoomType Type { get; set; }
        public required int Capacity { get; set; }
        public required decimal Price { get; set; }
        public required RoomStatus Status { get; set; }

        // Navigation
        public required ICollection<ReservationRoom> ReservationRooms { get; set; }
        public required ICollection<HousekeepingTask> HousekeepingTasks { get; set; }
    }

}
