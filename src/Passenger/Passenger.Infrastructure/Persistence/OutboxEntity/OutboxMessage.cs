namespace Passenger.Infrastructure.Persistence.OutboxEntity;

/// <summary>
/// Transactional Outbox message persisted in the same DB transaction as aggregate changes.
/// </summary>
public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public DateTime OccurredOnUtc { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public string Content { get; private set; } = string.Empty;

    public int Attempts { get; private set; }
    public DateTime? LastAttemptOnUtc { get; private set; }
    public string? LastError { get; private set; }

    public DateTime? ProcessedOnUtc { get; private set; }

    private OutboxMessage() { }

    public OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        OccurredOnUtc = occurredOnUtc;
        Type = type;
        Content = content;
    }

    public void MarkAttempt(DateTime utcNow, string? error = null)
    {
        Attempts++;
        LastAttemptOnUtc = utcNow;
        LastError = error;
    }

    public void MarkProcessed(DateTime utcNow)
    {
        ProcessedOnUtc = utcNow;
        LastError = null;
    }
}
