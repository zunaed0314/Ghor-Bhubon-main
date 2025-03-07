using Microsoft.EntityFrameworkCore;
using Ghor_Bhubon.Models;

namespace Ghor_Bhubon.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<PropertyPending> PropertyPending { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<City> Cities { get; set; }  // ✅ Add this line

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flat>()
                .Property(f => f.Rent)
                .HasColumnType("decimal(18,2)");

            // ✅ Define One-to-Many relationship between City and Area
            modelBuilder.Entity<Area>()
                .HasOne(a => a.City)
                .WithMany(c => c.Areas)
                .HasForeignKey(a => a.CityId);
        }
    }
}
