using MediatR;

namespace Dispute.Domain;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
}
