using HostelAPI.Interfaces;
using HostelAPI.Models;

public class RoomService : IRoomService
{
    private readonly IRoomRepository _repository;

    public RoomService(IRoomRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RoomDto>> GetAllAsync()
    {
        var chambres = await _repository.GetAllAsync();
        return chambres.Select(c => MapToDto(c));
    }

    public async Task<RoomDto> GetByIdAsync(int id)
    {
        var chambre = await _repository.GetByIdAsync(id);
        return chambre == null ? null : MapToDto(chambre);
    }

    public async Task<IEnumerable<RoomDto>> GetAvailablesAsync(DateTime debut, DateTime fin)
    {
        var chambres = await _repository.GetDisponiblesAsync(debut, fin);
        return chambres.Select(c => MapToDto(c));
    }

    public async Task<RoomDto> CreateAsync(RoomDto dto)
    {
        var chambre = MapToEntity(dto);
        var created = await _repository.AddAsync(chambre);
        return MapToDto(created);
    }

    public async Task<RoomDto> UpdateAsync(int id, RoomDto dto)
    {
        var chambre = await _repository.GetByIdAsync(id);
        if (chambre == null) return null;

        chambre.Type = dto.Type;
        chambre.Capacity = dto.Capacity;
        chambre.Price = dto.Price;
        chambre.Status = dto.Status;

        var updated = await _repository.UpdateAsync(chambre);
        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    // Mapping
    private RoomDto MapToDto(Room c) => new()
    {
        Id = c.Id,
        Type = c.Type,
        Price = c.Price,
        Capacity = c.Capacity,
        Status = c.Status,
        Number = c.Number
    };

    private Room MapToEntity(RoomDto dto) => new()
    {
        Id = dto.Id,
        Type = dto.Type,
        Price = dto.Price,
        Capacity = dto.Capacity,
        Status = dto.Status.ToString(),
        Number = dto.Number,
        ReservationRooms = new List<ReservationRoom>()

    };
}
