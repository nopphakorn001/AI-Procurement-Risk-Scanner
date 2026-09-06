using MediatR;
using ProcurementRisk.Application.Interfaces;
using ProcurementRisk.Domain.Entities;

namespace ProcurementRisk.Application.Evidence;

public record AddSupplierEvidenceCommand(
    Guid SupplierId,
    RiskFactor Factor,
    string SourceType,
    string SourceReference,
    DateTime ObservedAtUtc,
    string Reviewer,
    decimal Confidence,
    decimal RiskValue,
    string Summary) : IRequest<Guid>;

public class AddSupplierEvidenceCommandHandler : IRequestHandler<AddSupplierEvidenceCommand, Guid>
{
    private readonly ISupplierRepository _suppliers;
    private readonly ISupplierEvidenceRepository _evidence;

    public AddSupplierEvidenceCommandHandler(ISupplierRepository suppliers, ISupplierEvidenceRepository evidence)
    {
        _suppliers = suppliers;
        _evidence = evidence;
    }

    public async Task<Guid> Handle(AddSupplierEvidenceCommand request, CancellationToken cancellationToken)
    {
        if (await _suppliers.GetByIdAsync(request.SupplierId, cancellationToken) is null)
            throw new KeyNotFoundException($"Supplier {request.SupplierId} not found.");

        var evidence = SupplierEvidence.Record(
            request.SupplierId, request.Factor, request.SourceType, request.SourceReference,
            request.ObservedAtUtc, request.Reviewer, request.Confidence, request.RiskValue, request.Summary);
        await _evidence.AddAsync(evidence, cancellationToken);
        await _evidence.SaveChangesAsync(cancellationToken);
        return evidence.Id;
    }
}
