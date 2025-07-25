
public interface IReservationService
{
    Task<Reservation> CreateAsync(int userId, ReservationDto dto);
    Task<List<Reservation>> GetForUserAsync(int userId);
    Task<bool> CancelAsync(int reservationId, int userId, bool isReceptionist, bool refund);
    Task<bool> AreRoomsAvailableAsync(List<int> roomIds, DateTime startDate, DateTime endDate);
    Task<bool> CheckInAsync(int id, bool paid);
    Task<bool> CheckOutAsync(int reservationId);
}
