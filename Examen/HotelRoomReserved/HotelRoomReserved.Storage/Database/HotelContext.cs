using Microsoft.EntityFrameworkCore;
using HotelRoomReserved.Entities.Models;

namespace HotelRoomReserved.Storage.Database;

public class HotelContext : DbContext
{
    public HotelContext() { }

    public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HotelRoomReservedDB;Integrated Security=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.Reservations)
            .WithOne(r => r.User)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Room>()
            .HasMany(r => r.Reservations)
            .WithOne(r => r.Room)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
