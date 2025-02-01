using Microsoft.EntityFrameworkCore;
using CountryCityApi.Models;

namespace CountryCityApi.Data;

public class CountryCityContext : DbContext
{
    public CountryCityContext(DbContextOptions<CountryCityContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<City> Cities { get; set; }
}
