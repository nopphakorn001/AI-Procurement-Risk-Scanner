using Microsoft.EntityFrameworkCore;
using ProcurementRisk.Application.Interfaces;
using ProcurementRisk.Domain.Entities;

namespace ProcurementRisk.Infrastructure.Persistence;

public class SupplierRepository : ISupplierRepository
{
    private readonly AppDbContext _context;

    public SupplierRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Supplier>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Suppliers
                         .AsNoTracking()
                         .OrderBy(s => s.Name)
                         .ToListAsync(cancellationToken);

    public async Task<Supplier?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _context.Suppliers.FindAsync([id], cancellationToken);

    public async Task AddAsync(Supplier supplier, CancellationToken cancellationToken = default)
        => await _context.Suppliers.AddAsync(supplier, cancellationToken);

    public Task UpdateAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        _context.Suppliers.Update(supplier);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Supplier supplier, CancellationToken cancellationToken = default)
    {
        _context.Suppliers.Remove(supplier);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
