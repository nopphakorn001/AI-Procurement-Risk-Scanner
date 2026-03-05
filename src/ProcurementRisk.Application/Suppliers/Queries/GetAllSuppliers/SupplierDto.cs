namespace ProcurementRisk.Application.Suppliers.Queries.GetAllSuppliers;

public record SupplierDto(
    Guid Id,
    string Name,
    string Country,
    decimal? RiskScore,
    string? Reasoning
);
