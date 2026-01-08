namespace Dispute.Domain;

public interface IDisputeRepository
{
    Task<Dispute?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Dispute>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Dispute dispute, CancellationToken cancellationToken = default);
}
