using HostelAPI.Models;

namespace HostelAPI.Interfaces
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task RegisterUserAsync(User user, string password);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
    }
}