using Dispute.Domain;
using Microsoft.EntityFrameworkCore;

namespace Dispute.Infrastructure;

internal sealed class DisputeRepository : IDisputeRepository
{
    private readonly DisputeDbContext _dbContext;

    public DisputeRepository(DisputeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Domain.Dispute?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Disputes
            .Include(d => d.Messages)
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Domain.Dispute>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Disputes
            .Where(d => d.PassengerId == userId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Domain.Dispute dispute, CancellationToken cancellationToken = default)
    {
        await _dbContext.Disputes.AddAsync(dispute, cancellationToken);
    }
}
