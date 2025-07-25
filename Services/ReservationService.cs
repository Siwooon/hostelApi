using HostelAPI.Enums;
using HostelAPI.Interfaces;

public class ReservationService : IReservationService
{
    private readonly IReservationRepository _ReservationRepo;
    private readonly IRoomRepository _RoomRepo;

    public ReservationService(IReservationRepository resercationrepo, IRoomRepository roomrepo)
    {
        _ReservationRepo = resercationrepo;
        _RoomRepo = roomrepo;
    }

    public async Task<Reservation> CreateAsync(int userId, ReservationDto dto)
    {
        var total = await _ReservationRepo.CalculateTotalPriceAsync(dto.RoomIds, dto.StartDate, dto.EndDate);

        var reservation = new Reservation
        {
            UserId = userId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsPaid = true,
            Price = total, // à calculer via les chambres
            ReservationRooms = dto.RoomIds.Select(id => new ReservationRoom
            {
                RoomId = id
            }).ToList()
        };

        return await _ReservationRepo.CreateAsync(reservation, dto.RoomIds);
    }

    public async Task<List<Reservation>> GetForUserAsync(int userId)
    {
        return await _ReservationRepo.GetByUserAsync(userId);
    }

    public async Task<bool> CancelAsync(int reservationId, int userId, bool isReceptionist, bool refund)
    {
        var reservation = await _ReservationRepo.GetByIdAsync(reservationId);

        if (reservation == null || reservation.isRefunded || (reservation.UserId != userId && !isReceptionist))
            return false;

        if (isReceptionist)
        {
            reservation.isRefunded = refund;
        }
        else if ((reservation.StartDate - DateTime.UtcNow).TotalHours >= 48)
        {
            reservation.isRefunded = true;
        }

        reservation.IsCancelled = true;

        await _ReservationRepo.CancelAsync(reservation);
        return true;
    }

    public async Task<bool> AreRoomsAvailableAsync(List<int> roomIds, DateTime startDate, DateTime endDate)
    {
        var reservations = await _ReservationRepo.GetReservationsForRoomsAsync(roomIds, startDate, endDate);
        if (reservations == null || !reservations.Any())
            return true;

        foreach (var reservation in reservations)
        {
            if (!reservation.IsCancelled)
            {
                foreach (var resRoom in reservation.ReservationRooms)
                {
                    if (roomIds.Contains(resRoom.RoomId))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public async Task<bool> CheckInAsync(int id, bool paid)
    {
        var reservation = await _ReservationRepo.GetByIdAsync(id);
        if (reservation == null || reservation.IsCheckedIn)
            return false;

        if (!reservation.IsPaid)
        {
            reservation.IsPaid = paid;
            if (!paid)
            {
                return false;
            }
        }

        reservation.IsCheckedIn = true;

        if (reservation.ReservationRooms != null)
        {
            foreach (var resRoom in reservation.ReservationRooms)
            {
                var room = await _RoomRepo.GetByIdAsync(resRoom.RoomId);
                if (room != null)
                {
                    var status = room.Status.Split(",").ToList();
                    status = status.Where(item => item != RoomStatus.Available.ToString()).ToList();
                    status.Add(RoomStatus.Occupied.ToString());
                    room.Status = string.Join(",", status);

                    await _RoomRepo.UpdateAsync(room);
                }
            }
        }

        await _ReservationRepo.UpdateAsync(reservation);
        return true;
    }

    public async Task<bool> CheckOutAsync(int reservationId)
    {
        var reservation = await _ReservationRepo.GetByIdAsync(reservationId);
        if (reservation == null || !reservation.IsCheckedIn)
            return false;

        reservation.IsCheckedOut = true;

        if (!reservation.IsPaid)
        {
            return false;
        }

        if (reservation.ReservationRooms != null)
        {
            foreach (var resRoom in reservation.ReservationRooms)
            {
                var room = await _RoomRepo.GetByIdAsync(resRoom.RoomId);
                if (room != null)
                {
                    var status = room.Status.Split(",").ToList();
                    status = status.Where(item => item != RoomStatus.Occupied.ToString()).ToList();
                    status.Add(RoomStatus.Available.ToString());
                    status.Add(RoomStatus.NeedsCleaning.ToString());
                    room.Status = string.Join(",", status);

                    await _RoomRepo.UpdateAsync(room);
                }
            }
        }

        await _ReservationRepo.UpdateAsync(reservation);
        return true;
    }
}