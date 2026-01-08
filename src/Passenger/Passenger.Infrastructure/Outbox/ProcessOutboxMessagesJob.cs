using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Passenger.Application.Abstractions;
using Passenger.Infrastructure.Persistence.DbContext;
using Quartz;

namespace Passenger.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly PassengerDbContext _db;
    private readonly IDateTimeProvider _clock;
    private readonly OutboxProcessingOptions _options;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        PassengerDbContext db,
        IDateTimeProvider clock,
        IOptions<OutboxProcessingOptions> options,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _db = db;
        _clock = clock;
        _options = options.Value;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _db.OutboxMessages
            .Where(m => m.ProcessedOnUtc == null && m.Attempts < _options.MaxAttempts)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(_options.BatchSize)
            .ToListAsync(context.CancellationToken);

        if (messages.Count == 0) return;

        foreach (var message in messages)
        {
            try
            {
                // RabbitMQ publishing removed for local development
                // message.MarkProcessed simply marks it as processed locally
                message.MarkProcessed(_clock.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process outbox message {Id} (Type={Type}).", message.Id, message.Type);
                message.MarkAttempt(_clock.UtcNow, ex.Message);
            }
        }

        await _db.SaveChangesAsync(context.CancellationToken);
    }
}
