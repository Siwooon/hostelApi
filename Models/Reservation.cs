namespace HostelAPI.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsPaid { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsRefunded { get; set; }

        // Navigation
        public required User User { get; set; }
        public required ICollection<ReservationRoom> ReservationRooms { get; set; }
    }
}
