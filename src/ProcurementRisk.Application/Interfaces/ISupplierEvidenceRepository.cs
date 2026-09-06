using ProcurementRisk.Domain.Entities;

namespace ProcurementRisk.Application.Interfaces;

public interface ISupplierEvidenceRepository
{
    Task<IReadOnlyList<SupplierEvidence>> GetForSupplierAsync(Guid supplierId, CancellationToken cancellationToken = default);
    Task AddAsync(SupplierEvidence evidence, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
