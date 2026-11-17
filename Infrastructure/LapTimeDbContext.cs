using Microsoft.EntityFrameworkCore;

namespace Senac.ProvaSpec.Infrastructure;

public sealed class LapTimeDbContext(DbContextOptions<LapTimeDbContext> options) : DbContext(options)
{
    public DbSet<LapEntity> Laps => Set<LapEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LapEntity>(entity =>
        {
            entity.ToTable("Laps");
            entity.HasKey(lap => lap.Id);
            entity.Property(lap => lap.TotalMilliseconds).IsRequired();
        });
    }
}
