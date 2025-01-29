using Microsoft.EntityFrameworkCore;
using HotelRoomReserved.Entities.Models;

namespace HotelRoomReserved.Storage;

public class HotelContext : DbContext
{
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
        modelBuilder.Entity<Room>()
            .Property(r => r.Price)
            .HasColumnType("decimal(18,2)"); // Фікс проблеми з decimal
    }
}
