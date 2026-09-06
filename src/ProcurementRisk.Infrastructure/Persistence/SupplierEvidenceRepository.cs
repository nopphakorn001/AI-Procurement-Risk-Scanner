using Microsoft.EntityFrameworkCore;
using ProcurementRisk.Application.Interfaces;
using ProcurementRisk.Domain.Entities;

namespace ProcurementRisk.Infrastructure.Persistence;

public class SupplierEvidenceRepository : ISupplierEvidenceRepository
{
    private readonly AppDbContext _context;
    public SupplierEvidenceRepository(AppDbContext context) => _context = context;

    public async Task<IReadOnlyList<SupplierEvidence>> GetForSupplierAsync(Guid supplierId, CancellationToken cancellationToken = default) =>
        await _context.SupplierEvidence.AsNoTracking()
            .Where(item => item.SupplierId == supplierId)
            .OrderByDescending(item => item.ObservedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(SupplierEvidence evidence, CancellationToken cancellationToken = default) =>
        await _context.SupplierEvidence.AddAsync(evidence, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
