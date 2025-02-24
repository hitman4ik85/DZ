using Microsoft.EntityFrameworkCore;
using WorkerManagement.Entities.Models;

namespace WorkerManagement.Storage.Data;

public class WorkerManagementContext : DbContext
{
    public WorkerManagementContext() { }

    public WorkerManagementContext(DbContextOptions<WorkerManagementContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=WorkerManagementDB;Integrated Security=True;");
        }
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<WorkLog> WorkLogs { get; set; }
    public DbSet<Bonus> Bonuses { get; set; }
}
