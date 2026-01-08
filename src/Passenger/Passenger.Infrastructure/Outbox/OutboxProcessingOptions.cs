namespace Passenger.Infrastructure.Outbox;

public sealed class OutboxProcessingOptions
{
    public int BatchSize { get; init; } = 50;
    public int MaxAttempts { get; init; } = 10;
}
