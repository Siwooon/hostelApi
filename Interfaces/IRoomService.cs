public interface IRoomService
{
    Task<IEnumerable<RoomDto>> GetAllAsync();
    Task<RoomDto> GetByIdAsync(int id);
    Task<IEnumerable<RoomDto>> GetAvailablesAsync(DateTime debut, DateTime fin);
    Task<RoomDto> CreateAsync(RoomDto dto);
    Task<RoomDto> UpdateAsync(int id, RoomDto dto);
    Task DeleteAsync(int id);
}
