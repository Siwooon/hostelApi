using HostelAPI.Enums;

namespace HostelAPI.Models
{
    public class User
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required UserRole Role { get; set; }

        public required ICollection<Reservation> Reservations { get; set; }
    }
}
