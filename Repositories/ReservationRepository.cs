using HostelAPI;
using Microsoft.EntityFrameworkCore;

public class ReservationRepository : IReservationRepository
{
    private readonly HostelDbContext _context;

    public ReservationRepository(HostelDbContext context)
    {
        _context = context;
    }

    public async Task<Reservation> CreateAsync(Reservation reservation, List<int> roomIds)
    {
        var rooms = await _context.Rooms.Where(r => roomIds.Contains(r.Id)).ToListAsync();

        reservation.ReservationRooms = [.. rooms.Select(room => new ReservationRoom
        {
            RoomId = room.Id,
            Room = room,
            Reservation = reservation,
            ReservationId = reservation.Id
        })];

        _context.Reservations.Add(reservation);
        await _context.SaveChangesAsync();

        return reservation;
    }

    public async Task<List<Reservation>> GetByUserAsync(int userId)
    {
        return await _context.Reservations
            .Include(r => r.ReservationRooms)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .Include(r => r.ReservationRooms)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<bool> CancelAsync(Reservation reservation)
    {
        reservation.IsCancelled = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<decimal> CalculateTotalPriceAsync(List<int> roomIds, DateTime start, DateTime end)
    {
        if (end <= start)
            throw new ArgumentException("Start and end date exception");

        var rooms = await _context.Rooms
            .Where(r => roomIds.Contains(r.Id))
            .ToListAsync();

        var nbNuits = Math.Max(1, (end.Date - start.Date).Days);

        return rooms.Sum(r => r.Price * nbNuits);
    }

    public async Task<IEnumerable<Reservation>> GetReservationsForRoomsAsync(List<int> roomIds, DateTime startDate, DateTime endDate)
    {
        var reservations = await _context.Reservations
            .Include(r => r.ReservationRooms)
            .Where(r => r.ReservationRooms.Any(rr => roomIds.Contains(rr.RoomId)) &&
                        r.StartDate < endDate &&
                        r.EndDate > startDate)
            .ToListAsync();

        return reservations.Select(r => new Reservation
        {
            Id = r.Id,
            UserId = r.UserId,
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            IsCancelled = r.IsCancelled,
            isRefunded = r.isRefunded,
            ReservationRooms = r.ReservationRooms
            .Where(rr => roomIds.Contains(rr.RoomId))
            .Select(rr => new ReservationRoom
            {
                RoomId = rr.RoomId
            }).ToList()
        }).ToList();
    }

    public async Task UpdateAsync(Reservation reservation)
    {
        _context.Reservations.Update(reservation);
        await _context.SaveChangesAsync();
    }
}
