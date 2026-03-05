using Microsoft.EntityFrameworkCore;
using EnterpriseApp.Domain;

namespace EnterpriseApp.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<Supplier> Suppliers { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}