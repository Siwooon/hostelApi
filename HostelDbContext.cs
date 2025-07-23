using HostelAPI.Enums;
using HostelAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace HostelAPI
{
    public class HostelDbContext : DbContext
    {
        public HostelDbContext(DbContextOptions<HostelDbContext> options)
    : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationRoom> ReservationRooms { get; set; }
        public DbSet<HousekeepingTask> HousekeepingTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ReservationRoom>()
                .HasKey(rr => new { rr.ReservationId, rr.RoomId });

            modelBuilder.Entity<ReservationRoom>()
                .HasOne(rr => rr.Reservation)
                .WithMany(r => r.ReservationRooms)
                .HasForeignKey(rr => rr.ReservationId);

            modelBuilder.Entity<ReservationRoom>()
                .HasOne(rr => rr.Room)
                .WithMany(r => r.ReservationRooms)
                .HasForeignKey(rr => rr.RoomId);

            modelBuilder.Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        FirstName = "Laurent",
                        LastName = "Gina",
                        PasswordHash = "hashed_password_1", // Replace with actual hashed password
                        Role = UserRole.Client
                    },
                    new User
                    {
                        Id = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "hashed_password_1", // Replace with actual hashed password
                        Role = UserRole.Client,
                        Reservations = []
                    },
                    new User
                    {
                        Id = 1,
                        FirstName = "John",
                        LastName = "Doe",
                        PasswordHash = "hashed_password_1", // Replace with actual hashed password
                        Role = UserRole.Client,
                        Reservations = []
                    });

            // Ajout des fixtures pour Rooms
            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    Number = "101",
                    Type = RoomType.Single,
                    Capacity = 1,
                    Price = 50.00m,
                    Status = RoomStatus.Available,
                    HousekeepingTasks = [],
                    ReservationRooms = []
                },
                new Room
                {
                    Id = 2,
                    Number = "102",
                    Type = RoomType.Double,
                    Capacity = 2,
                    Price = 80.00m,
                    Status = RoomStatus.Occupied,
                    HousekeepingTasks = [],
                    ReservationRooms = []
                },
                new Room
                {
                    Id = 3,
                    Number = "201",
                    Type = RoomType.Suite,
                    Capacity = 6,
                    Price = 30.00m,
                    Status = RoomStatus.Available,
                    HousekeepingTasks = [],
                    ReservationRooms = []
                }
            );
        }


    }
}
