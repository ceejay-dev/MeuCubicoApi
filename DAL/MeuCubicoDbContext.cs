using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MeuCubicoDbContext : IdentityDbContext<User>
    {
        public MeuCubicoDbContext(DbContextOptions<MeuCubicoDbContext> options)
        : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }
}
