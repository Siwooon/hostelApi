namespace HostelAPI.Models
{
    public class ReservationRoom
    {
        public required int ReservationId { get; set; }
        public required Reservation Reservation { get; set; }

        public required int RoomId { get; set; }
        public required Room Room { get; set; }
    }

}
