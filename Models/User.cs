using HostelAPI.Enums;

namespace HostelAPI.Models
{
    public class User
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PasswordHash { get; set; }
        public required UserRole Role { get; set; }

        // Navigation
        public required ICollection<Reservation> Reservations { get; set; }
    }

}
