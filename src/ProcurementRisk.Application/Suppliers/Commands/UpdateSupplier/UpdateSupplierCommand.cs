using MediatR;

namespace ProcurementRisk.Application.Suppliers.Commands.UpdateSupplier;

public record UpdateSupplierCommand(Guid Id, string Name, string Country) : IRequest;
