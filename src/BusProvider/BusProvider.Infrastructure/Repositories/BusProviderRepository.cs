using BusProvider.Domain.Aggregates;
using BusProvider.Domain.Interfaces;
using BusProvider.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BusProvider.Infrastructure.Repositories;

public class BusProviderRepository : IBusProviderRepository
{
    private readonly BusProviderDbContext _dbContext;

    public BusProviderRepository(BusProviderDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task AddAsync(BusProviderAggregate provider, CancellationToken cancellationToken = default)
    {
        _dbContext.Providers.Add(provider);
        return Task.CompletedTask;
    }

    public Task<BusProviderAggregate?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Providers.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public Task<List<BusProviderAggregate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Providers.AsNoTracking().ToListAsync(cancellationToken);
    }

    public Task RemoveAsync(BusProviderAggregate provider, CancellationToken cancellationToken = default)
    {
        _dbContext.Providers.Remove(provider);
        return Task.CompletedTask;
    }
}
