using DZ6.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZ6.Storage;

public class DZ6Context : DbContext //наслідування від класу DbContext (конфігурація нашої Бази Даних)
{
    public DZ6Context(DbContextOptions<DZ6Context> options) : base(options) //конструктор для підключення до Бази Даних
    {
    }

    //--- це так потрібно для створення міграції:
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DZ6DB;Integrated Security=True;");
    }
    //---

    //передаємо класи, щоб при запуску створилися такі таблиці
    public DbSet<Client> Clients { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<ProductStorageInfo> ProductStorageInfos { get; set; }
}
