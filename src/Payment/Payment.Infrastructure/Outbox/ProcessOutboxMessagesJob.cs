using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using Payment.Application;
using Payment.Infrastructure.Persistence;
using Payment.Application.Interfaces;

namespace Payment.Infrastructure.Outbox;

[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly PaymentDbContext _dbContext;
    private readonly IEventPublisher _publisher;

    public ProcessOutboxMessagesJob(PaymentDbContext dbContext, IEventPublisher publisher)
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
                var domainEvent = JsonConvert.DeserializeObject<dynamic>(
                    message.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

                if (domainEvent is null) continue;

                // Map domain events to integration events if needed, 
                // but for now we publish to RabbitMQ via the IEventPublisher
                // We simplify for the demo and publish the dynamic content
                
                _publisher.Publish(domainEvent, "payment.exchange", "payment.completed");

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
