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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Flat>()
                .Property(f => f.Rent)
                .HasColumnType("decimal(18,2)"); 
        }
    }
}
