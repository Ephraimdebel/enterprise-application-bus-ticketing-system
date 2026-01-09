namespace Trip.Infrastructure.Outbox;

public sealed class OutboxProcessingOptions
{
    public int BatchSize { get; set; } = 20;
    public int IntervalInSeconds { get; set; } = 5;
}
