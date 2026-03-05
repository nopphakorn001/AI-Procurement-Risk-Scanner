using MediatR;

namespace ProcurementRisk.Application.Suppliers.Commands.DeleteSupplier;

public record DeleteSupplierCommand(Guid Id) : IRequest;
