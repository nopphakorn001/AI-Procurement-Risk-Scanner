using MediatR;

namespace ProcurementRisk.Application.Suppliers.Queries.GetAllSuppliers;

public record GetAllSuppliersQuery : IRequest<IEnumerable<SupplierDto>>;
