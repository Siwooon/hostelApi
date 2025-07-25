using HostelAPI.Enums;
using HostelAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace HostelAPI
{
    public class HostelDbContext(DbContextOptions<HostelDbContext> options) : DbContext(options)
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationRoom> ReservationRooms { get; set; }

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
                        Email = "LaurentGina@hotmail.com",
                        Password = "AQAAAAIAAYagAAAAENq6q7SJ7IiKxWxeKnHuwwU084Spq37pZtDgcdN/ywsYaoZ1Zsu0X09tBq5A4WIOxw==",
                        Role = UserRole.Client,
                        Reservations = []
                    },
                    new User
                    {
                        Id = 2,
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "JauneDo@gmail.com",
                        Password = "AQAAAAIAAYagAAAAEC+Gmfb3aG5Olm+Gagk2f1KZjPb/8sUlOucu8Kknu8fHLoqK+PeyPu42REKvXE6wTg==",
                        Role = UserRole.Receptionist,
                        Reservations = []
                    },
                    new User
                    {
                        Id = 3,
                        FirstName = "Gérard",
                        LastName = "Menvussa",
                        Email = "Gégééé@gmail.com",
                        Password = "AQAAAAIAAYagAAAAEPGdXgvrBx3/TC3XHCCzDXPa/DRbIOIMtTbrAdjHH2abpxUpkyi+FfanrkiiwyPPZg==",
                        Role = UserRole.Housekeeping,
                        Reservations = []
                    });

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    Number = "101",
                    Type = RoomType.Single,
                    Capacity = 1,
                    Price = 50.00m,
                    Status = $"{RoomStatus.Available.ToString()},{RoomStatus.NoIssue.ToString()}",
                    ReservationRooms = []
                },
                new Room
                {
                    Id = 2,
                    Number = "102",
                    Type = RoomType.Double,
                    Capacity = 2,
                    Price = 80.00m,
                    Status = $"{RoomStatus.Available.ToString()},{RoomStatus.NeedsCleaning.ToString()}",
                    ReservationRooms = []
                },
                new Room
                {
                    Id = 3,
                    Number = "201",
                    Type = RoomType.Suite,
                    Capacity = 6,
                    Price = 30.00m,
                    Status = $"{RoomStatus.Available.ToString()},{RoomStatus.MajorDamage.ToString()}",
                    ReservationRooms = []
                }
            );
        }


    }
}
