using Microsoft.EntityFrameworkCore;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MeuCubicoDbContext : DbContext
    {
        public MeuCubicoDbContext(DbContextOptions<MeuCubicoDbContext> options)
        : base(options)
        {
        }

        public DbSet<Resident> Residents { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
