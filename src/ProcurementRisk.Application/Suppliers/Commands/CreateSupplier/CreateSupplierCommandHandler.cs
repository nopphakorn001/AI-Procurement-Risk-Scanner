using MediatR;
using ProcurementRisk.Application.Interfaces;
using ProcurementRisk.Domain.Entities;

namespace ProcurementRisk.Application.Suppliers.Commands.CreateSupplier;

public class CreateSupplierCommandHandler : IRequestHandler<CreateSupplierCommand, Guid>
{
    private readonly ISupplierRepository _repository;

    public CreateSupplierCommandHandler(ISupplierRepository repository)
        => _repository = repository;

    public async Task<Guid> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = Supplier.Create(request.Name, request.Country);
        await _repository.AddAsync(supplier, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return supplier.Id;
    }
}
