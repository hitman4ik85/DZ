using PeopleBudgetTracker.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace PeopleBudgetTracker.Storage;

public class PeopleBudgetTrackerContext : DbContext
{
    public PeopleBudgetTrackerContext() { }
    public PeopleBudgetTrackerContext(DbContextOptions<PeopleBudgetTrackerContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=PeopleBudgetDB;Integrated Security=True;");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CustomCategory> CustomCategories { get; set; }
    public DbSet<IncomeOperation> IncomeOperations { get; set; }
    public DbSet<ExpenseOperation> ExpenseOperations { get; set; }
}
