using Microsoft.EntityFrameworkCore;
using CountryCityApi.Models;

namespace CountryCityApi.Data;

public class CountryCityContext : DbContext
{
    public CountryCityContext(DbContextOptions<CountryCityContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=AllCountryCityDB;Integrated Security=True;");
    }
    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
}