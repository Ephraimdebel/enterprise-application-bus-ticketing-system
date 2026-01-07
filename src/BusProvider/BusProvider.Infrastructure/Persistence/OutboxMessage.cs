using System.Text.Json;
using BusProvider.Domain.Abstractions;

namespace BusProvider.Infrastructure.Persistence;

public class OutboxMessage
{
    public Guid Id { get; private set; }
    public DateTime OccurredOnUtc { get; private set; }
    public DateTime? ProcessedOnUtc { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Payload { get; private set; } = string.Empty;

    public static OutboxMessage FromDomainEvent(DomainEvent domainEvent)
    {
        return new OutboxMessage
        {
            Id = domainEvent.Id,
            OccurredOnUtc = domainEvent.OccurredOnUtc,
            Type = domainEvent.GetType().Name,
            Payload = JsonSerializer.Serialize(domainEvent)
        };
    }
}
