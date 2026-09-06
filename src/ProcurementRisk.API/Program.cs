using Microsoft.EntityFrameworkCore;
using ProcurementRisk.Application.Suppliers.Commands.CreateSupplier;
using ProcurementRisk.Infrastructure;
using ProcurementRisk.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateSupplierCommand).Assembly));

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins((builder.Configuration["AllowedOrigins"] ?? "http://127.0.0.1:8783")
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var provider = app.Configuration["DatabaseProvider"] ?? "SqlServer";
    if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
    {
        db.Database.EnsureCreated();
    }
    else
    {
        Exception? finalError = null;
        for (var attempt = 1; attempt <= 10; attempt++)
        {
            try
            {
                db.Database.Migrate();
                finalError = null;
                break;
            }
            catch (Exception ex)
            {
                finalError = ex;
                app.Logger.LogWarning(ex, "Database migration attempt {Attempt} failed", attempt);
                if (attempt < 10) Thread.Sleep(3000);
            }
        }

        if (finalError is not null)
            throw new InvalidOperationException("Database migration failed after 10 attempts.", finalError);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");
app.MapHealthChecks("/health");
app.MapGet("/api/status", async (AppDbContext db, IConfiguration configuration, CancellationToken ct) => new
{
    productId = "AIProcurementRiskScanner",
    contractVersion = "0.1.0",
    status = "READY",
    runtime = "LOCAL_STANDALONE",
    database = configuration["DatabaseProvider"] ?? "SqlServer",
    supplierCount = await db.Suppliers.CountAsync(ct),
    aiConnector = "NOT_CONNECTED",
    automationConnector = "NOT_CONNECTED",
    moneyMoved = false,
    externalActions = 0,
    dataQuality = "PARTIAL"
});
app.MapControllers();
app.Run();

public partial class Program { }
