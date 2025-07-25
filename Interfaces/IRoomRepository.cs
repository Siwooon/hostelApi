using HostelAPI.Models;

namespace HostelAPI.Interfaces
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room> GetByIdAsync(int id);
        Task<IEnumerable<Room>> GetDisponiblesAsync(DateTime debut, DateTime fin);
        Task<Room> AddAsync(Room Room);
        Task<Room> UpdateAsync(Room Room);
        Task DeleteAsync(int id);
    }
}