using HostelAPI.Enums;

namespace HostelAPI.Models
{
    public class Room
    {
        public required int Id { get; set; }
        public required string Number { get; set; }
        public required RoomType Type { get; set; }
        public required int Capacity { get; set; }
        public required decimal Price { get; set; }
        public required string Status { get; set; }

        // Navigation
        public ICollection<ReservationRoom> ReservationRooms { get; set; }
    }
}
