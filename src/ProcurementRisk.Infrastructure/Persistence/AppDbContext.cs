using Microsoft.EntityFrameworkCore;
using ProcurementRisk.Domain.Entities;

namespace ProcurementRisk.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<SupplierEvidence> SupplierEvidence => Set<SupplierEvidence>();

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

        modelBuilder.Entity<SupplierEvidence>(entity =>
        {
            entity.HasKey(item => item.Id);
            entity.HasIndex(item => new { item.SupplierId, item.Factor, item.ObservedAtUtc });
            entity.Property(item => item.Factor).HasConversion<string>().HasMaxLength(64);
            entity.Property(item => item.SourceType).IsRequired().HasMaxLength(80);
            entity.Property(item => item.SourceReference).IsRequired().HasMaxLength(1000);
            entity.Property(item => item.Reviewer).IsRequired().HasMaxLength(160);
            entity.Property(item => item.Confidence).HasPrecision(5, 2);
            entity.Property(item => item.RiskValue).HasPrecision(5, 2);
            entity.Property(item => item.Summary).IsRequired().HasMaxLength(2000);
            entity.HasOne<Supplier>()
                  .WithMany()
                  .HasForeignKey(item => item.SupplierId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
