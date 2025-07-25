
public interface IReservationRepository
{
    Task<Reservation> CreateAsync(Reservation reservation, List<int> roomIds);
    Task<List<Reservation>> GetByUserAsync(int userId);
    Task<Reservation?> GetByIdAsync(int id);
    Task<bool> CancelAsync(Reservation reservation);
    Task<decimal> CalculateTotalPriceAsync(List<int> roomIds, DateTime start, DateTime end);
    Task<IEnumerable<Reservation>> GetReservationsForRoomsAsync(List<int> roomIds, DateTime startDate, DateTime endDate);
    Task UpdateAsync(Reservation reservation);
}
