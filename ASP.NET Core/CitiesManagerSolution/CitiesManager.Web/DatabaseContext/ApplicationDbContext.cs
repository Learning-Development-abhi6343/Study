using CitiesManager.WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CitiesManager.WebAPI.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }

        public ApplicationDbContext()
        {

        }

        public virtual DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>().HasData(new City() { CityID = Guid.Parse("CCEC09C0-2C05-4610-BF22-FBAAEB94D0EC"), CityName = "New York" });
            modelBuilder.Entity<City>().HasData(new City() { CityID = Guid.Parse("FE49360E-9D5C-4C0F-9B16-BDD1BD6242F5"), CityName = "London" });
        }
    }
}
