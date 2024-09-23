using Microsoft.EntityFrameworkCore;

namespace FMS.API.Entities;

public class FMSDbContext : DbContext
{
    public FMSDbContext(DbContextOptions<FMSDbContext> options) : base(options) { }

    public DbSet<Flight> Flights { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Flight>()
            .Property(f => f.TypSamolotu)
            .HasConversion<string>();
    }

    public void UpdateDatabase()
    {
        if (Database.CanConnect() && Database.IsRelational())
        {
            var pendingMigrations = Database.GetPendingMigrations();
            if (pendingMigrations != null && pendingMigrations.Any())
            {
                Database.Migrate();
            }
        }

        SaveChanges();
    }
}
