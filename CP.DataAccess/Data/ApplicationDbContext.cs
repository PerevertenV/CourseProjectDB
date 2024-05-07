using CP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<InfoAboutCurrency> InfoAboutCurrency { get; set; }
        public DbSet<Purchase> Purchase { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Payments> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<InfoAboutCurrency>().HasData(
                new InfoAboutCurrency 
                {
                    ID=1, 
                    Name="USD", 
                    AskedCoursePriceTo = 37, 
                    AvailOfAskedCourse = 5000 
                }
                );
        }
    }
}
