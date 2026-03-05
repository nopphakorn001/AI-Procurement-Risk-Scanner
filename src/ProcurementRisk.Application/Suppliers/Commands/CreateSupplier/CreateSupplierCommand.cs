using MediatR;

namespace ProcurementRisk.Application.Suppliers.Commands.CreateSupplier;

public record CreateSupplierCommand(string Name, string Country) : IRequest<Guid>;
