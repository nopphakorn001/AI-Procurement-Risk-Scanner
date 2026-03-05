using MediatR;

namespace ProcurementRisk.Application.Suppliers.Commands.ScoreSupplier;

public record ScoreSupplierCommand(Guid Id, decimal RiskScore, string? Reasoning) : IRequest;
