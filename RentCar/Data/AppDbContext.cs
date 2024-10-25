using Microsoft.EntityFrameworkCore;
using RentCar.Models;

namespace RentCar.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }

        public DbSet<MsCar> msCar { get; set; }
        public DbSet<MsCarImages> msCarImages { get; set; }
        public DbSet<TrRental> trRental { get; set; } 
    }
}
