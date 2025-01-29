using Microsoft.EntityFrameworkCore;
using FoodOrderApi.Models;

namespace FoodOrderApi.Data;

public class FoodOrderContext : DbContext
{
    public FoodOrderContext(DbContextOptions<FoodOrderContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
}