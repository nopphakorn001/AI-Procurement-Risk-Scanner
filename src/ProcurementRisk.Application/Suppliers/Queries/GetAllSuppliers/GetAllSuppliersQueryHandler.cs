using MediatR;
using ProcurementRisk.Application.Interfaces;

namespace ProcurementRisk.Application.Suppliers.Queries.GetAllSuppliers;

public class GetAllSuppliersQueryHandler
    : IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDto>>
{
    private readonly ISupplierRepository _repository;

    public GetAllSuppliersQueryHandler(ISupplierRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<SupplierDto>> Handle(
        GetAllSuppliersQuery request,
        CancellationToken cancellationToken)
    {
        var suppliers = await _repository.GetAllAsync(cancellationToken);
        return suppliers.Select(s => new SupplierDto(s.Id, s.Name, s.Country, s.RiskScore, s.Reasoning));
    }
}
