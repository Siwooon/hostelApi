using HostelAPI;
using HostelAPI.Enums;
using HostelAPI.Interfaces;
using HostelAPI.Models;
using Microsoft.EntityFrameworkCore;

public class RoomRepository : IRoomRepository
{
    private readonly HostelDbContext _context;

    public RoomRepository(HostelDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Room>> GetAllAsync()
        => await _context.Rooms.ToListAsync();

    public async Task<Room> GetByIdAsync(int id)
    {
        return await _context.Rooms.FindAsync(id);
    }

    public async Task<IEnumerable<Room>> GetDisponiblesAsync(DateTime debut, DateTime fin)
    {
        // Étape 1 : Récupérer les chambres qui ne sont pas réservées sur l'intervalle [debut, fin]
        var chambresReservees = await _context.ReservationRooms
            .Where(rr =>
                rr.Reservation.IsCancelled == false &&
                rr.Reservation.EndDate > debut &&
                rr.Reservation.StartDate < fin)
            .Select(rr => rr.RoomId)
            .Distinct()
            .ToListAsync();

        // Étape 2 : Retourner les chambres qui ne sont pas dans la liste des chambres réservées
        // Correction : Vérifier si la collection Status contient RoomStatus.Available
        return await _context.Rooms
            .Where(r => !chambresReservees.Contains(r.Id) && r.Status.Contains(RoomStatus.Available.ToString()))
            .ToListAsync();
    }

    public async Task<Room> AddAsync(Room Room)
    {
        _context.Rooms.Add(Room);
        await _context.SaveChangesAsync();
        return Room;
    }

    public async Task<Room> UpdateAsync(Room Room)
    {
        _context.Rooms.Update(Room);
        await _context.SaveChangesAsync();
        return Room;
    }

    public async Task DeleteAsync(int id)
    {
        var Room = await _context.Rooms.FindAsync(id);
        if (Room != null)
        {
            _context.Rooms.Remove(Room);
            await _context.SaveChangesAsync();
        }
    }
}
