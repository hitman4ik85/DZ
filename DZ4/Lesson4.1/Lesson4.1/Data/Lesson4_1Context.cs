using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lesson4._1.Models;

namespace Lesson4._1.Data
{
    public class Lesson4_1Context : DbContext
    {
        public Lesson4_1Context (DbContextOptions<Lesson4_1Context> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Lesson4._1.Models.Computer> Computer { get; set; } = default!;
    }
}
