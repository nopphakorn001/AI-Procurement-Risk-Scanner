using MediatR;
using ProcurementRisk.Application.Interfaces;

namespace ProcurementRisk.Application.Suppliers.Commands.DeleteSupplier;

public class DeleteSupplierCommandHandler : IRequestHandler<DeleteSupplierCommand>
{
    private readonly ISupplierRepository _repository;

    public DeleteSupplierCommandHandler(ISupplierRepository repository)
        => _repository = repository;

    public async Task Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier {request.Id} not found.");

        await _repository.DeleteAsync(supplier, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
