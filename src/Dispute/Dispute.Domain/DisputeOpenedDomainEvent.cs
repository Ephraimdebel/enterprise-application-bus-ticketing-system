namespace Dispute.Domain;

public record DisputeOpenedDomainEvent(Guid DisputeId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
