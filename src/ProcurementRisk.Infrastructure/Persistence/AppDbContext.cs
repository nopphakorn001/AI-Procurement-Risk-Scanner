using Microsoft.EntityFrameworkCore;
using ProcurementRisk.Domain.Entities;

namespace ProcurementRisk.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Supplier> Suppliers => Set<Supplier>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(s => s.Id);

            entity.Property(s => s.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(s => s.Country)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(s => s.RiskScore)
                  .HasPrecision(5, 2)
                  .IsRequired(false);

            entity.Property(s => s.Reasoning)
                  .HasMaxLength(1000)
                  .IsRequired(false);
        });
    }
}
