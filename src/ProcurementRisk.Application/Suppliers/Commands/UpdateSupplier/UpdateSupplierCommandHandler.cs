using MediatR;
using ProcurementRisk.Application.Interfaces;

namespace ProcurementRisk.Application.Suppliers.Commands.UpdateSupplier;

public class UpdateSupplierCommandHandler : IRequestHandler<UpdateSupplierCommand>
{
    private readonly ISupplierRepository _repository;

    public UpdateSupplierCommandHandler(ISupplierRepository repository)
        => _repository = repository;

    public async Task Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier {request.Id} not found.");

        supplier.Update(request.Name, request.Country);
        await _repository.UpdateAsync(supplier, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
