using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProcurementRisk.Application.Interfaces;
using ProcurementRisk.Infrastructure.Persistence;

namespace ProcurementRisk.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var provider = configuration["DatabaseProvider"] ?? "SqlServer";
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.");

        services.AddDbContext<AppDbContext>(options =>
        {
            if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
            {
                options.UseSqlite(connectionString);
                return;
            }

            if (!provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Unsupported DatabaseProvider '{provider}'.");

            options.UseSqlServer(
                connectionString,
                sqlOptions => sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null));
        });

        services.AddScoped<ISupplierRepository, SupplierRepository>();

        return services;
    }
}
