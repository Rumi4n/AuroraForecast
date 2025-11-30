using Microsoft.EntityFrameworkCore;
using AuroraForecast.Locations.Models;

namespace AuroraForecast.Data
{
    public class AuroraDbContext(DbContextOptions<AuroraDbContext> options) : DbContext(options)
    {
        public DbSet<User.Models.User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User.Models.User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            // Configure Location entity
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
            });
        }
    }
}