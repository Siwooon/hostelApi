using HostelAPI.Models;

public class Reservation
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCancelled { get; set; }
    public bool isRefunded { get; set; }
    public bool IsCheckedIn { get; set; }
    public bool IsCheckedOut { get; set; }
    public DateTime DateReservation { get; set; } = DateTime.UtcNow;
    public ICollection<ReservationRoom> ReservationRooms { get; set; } = new List<ReservationRoom>();
}
