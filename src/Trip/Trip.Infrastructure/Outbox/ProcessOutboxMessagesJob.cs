using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using Trip.Application;
using Trip.Domain.Events;
using Trip.Infrastructure.Persistence;

namespace Trip.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly TripDbContext _dbContext;
    private readonly IPublisher _publisher;

    public ProcessOutboxMessagesJob(TripDbContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                    message.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

                if (domainEvent is null) continue;

                await _publisher.Publish(domainEvent, context.CancellationToken);

                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                message.Error = ex.Message;
            }
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
