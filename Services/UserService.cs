using HostelAPI.Interfaces;
using HostelAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HostelAPI.Services
{
    public class UserService(HostelDbContext context) : IUserService
    {
        private readonly HostelDbContext _context = context;
        private readonly PasswordHasher<object> _hasher = new();

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == username);

            if (user == null)
                return null;

            if (!CheckPassword(user.Email, password, user.Password))
            {
                return null;
            }

            return user;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task RegisterUserAsync(User user, string password)
        {
            user.Password = HashPassword(user.Email, password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
        public string HashPassword(string user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public bool CheckPassword(string user, string password, string hashedPassword)
        {
            var result = _hasher.VerifyHashedPassword(user, hashedPassword, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
