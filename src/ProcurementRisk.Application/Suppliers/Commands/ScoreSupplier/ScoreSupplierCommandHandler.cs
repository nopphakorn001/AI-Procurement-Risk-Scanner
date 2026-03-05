using MediatR;
using ProcurementRisk.Application.Interfaces;

namespace ProcurementRisk.Application.Suppliers.Commands.ScoreSupplier;

public class ScoreSupplierCommandHandler : IRequestHandler<ScoreSupplierCommand>
{
    private readonly ISupplierRepository _repository;

    public ScoreSupplierCommandHandler(ISupplierRepository repository)
        => _repository = repository;

    public async Task Handle(ScoreSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Supplier {request.Id} not found.");

        supplier.ApplyScore(request.RiskScore, request.Reasoning);
        await _repository.UpdateAsync(supplier, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
