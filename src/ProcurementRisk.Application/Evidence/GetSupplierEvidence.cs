using MediatR;
using ProcurementRisk.Application.Interfaces;

namespace ProcurementRisk.Application.Evidence;

public record SupplierEvidenceDto(
    Guid Id,
    Guid SupplierId,
    string Factor,
    string SourceType,
    string SourceReference,
    DateTime ObservedAtUtc,
    string Reviewer,
    decimal Confidence,
    decimal RiskValue,
    string Summary,
    DateTime CreatedAtUtc,
    string Freshness);

public record GetSupplierEvidenceQuery(Guid SupplierId) : IRequest<IReadOnlyList<SupplierEvidenceDto>>;

public class GetSupplierEvidenceQueryHandler : IRequestHandler<GetSupplierEvidenceQuery, IReadOnlyList<SupplierEvidenceDto>>
{
    private readonly ISupplierEvidenceRepository _repository;
    private readonly ISupplierRepository _suppliers;
    public GetSupplierEvidenceQueryHandler(ISupplierEvidenceRepository repository, ISupplierRepository suppliers)
    {
        _repository = repository;
        _suppliers = suppliers;
    }

    public async Task<IReadOnlyList<SupplierEvidenceDto>> Handle(GetSupplierEvidenceQuery request, CancellationToken cancellationToken)
    {
        if (await _suppliers.GetByIdAsync(request.SupplierId, cancellationToken) is null)
            throw new KeyNotFoundException($"Supplier {request.SupplierId} not found.");
        var now = DateTime.UtcNow;
        return (await _repository.GetForSupplierAsync(request.SupplierId, cancellationToken))
            .Select(item => new SupplierEvidenceDto(
                item.Id, item.SupplierId, item.Factor.ToString(), item.SourceType, item.SourceReference,
                item.ObservedAtUtc, item.Reviewer, item.Confidence, item.RiskValue, item.Summary,
                item.CreatedAtUtc, item.IsStale(now) ? "STALE" : "FRESH"))
            .ToList();
    }
}
