using Microsoft.EntityFrameworkCore;
using ProductCatalog.Entities.Models;

namespace ProductCatalog.Storage;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ProductsDB;Integrated Security=True;");
    }
}
