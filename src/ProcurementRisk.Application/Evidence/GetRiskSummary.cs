using MediatR;
using ProcurementRisk.Application.Interfaces;
using ProcurementRisk.Domain.Services;

namespace ProcurementRisk.Application.Evidence;

public record GetRiskSummaryQuery(Guid SupplierId) : IRequest<RiskAssessmentResult>;

public class GetRiskSummaryQueryHandler : IRequestHandler<GetRiskSummaryQuery, RiskAssessmentResult>
{
    private readonly ISupplierRepository _suppliers;
    private readonly ISupplierEvidenceRepository _evidence;
    public GetRiskSummaryQueryHandler(ISupplierRepository suppliers, ISupplierEvidenceRepository evidence)
    {
        _suppliers = suppliers;
        _evidence = evidence;
    }

    public async Task<RiskAssessmentResult> Handle(GetRiskSummaryQuery request, CancellationToken cancellationToken)
    {
        if (await _suppliers.GetByIdAsync(request.SupplierId, cancellationToken) is null)
            throw new KeyNotFoundException($"Supplier {request.SupplierId} not found.");
        return RiskAssessmentCalculator.Calculate(
            await _evidence.GetForSupplierAsync(request.SupplierId, cancellationToken), DateTime.UtcNow);
    }
}
