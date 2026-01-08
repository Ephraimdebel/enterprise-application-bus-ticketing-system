using Microsoft.EntityFrameworkCore;
using Booking.Application;
using global::Booking.Domain;
using Newtonsoft.Json;
using Quartz;
using RabbitMQ.Client;
using System.Text;
using MediatR;

namespace Booking.Infrastructure;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
    private readonly BookingDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly MessagingService _messagingService;

    public ProcessOutboxMessagesJob(
        BookingDbContext dbContext, 
        IPublisher publisher,
        MessagingService messagingService)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _messagingService = messagingService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext
            .OutboxMessages
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
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

                if (domainEvent is null)
                {
                    continue;
                }

                await _publisher.Publish(domainEvent, context.CancellationToken);

                if (domainEvent is BookingConfirmedForPaymentEvent confirmedForPayment)
                {
                    // Special routing for Payment Module
                    await _messagingService.PublishIntegrationEventAsync(
                        confirmedForPayment, 
                        "booking.exchange", 
                        "booking.confirmed", 
                        context.CancellationToken);
                }
                else
                {
                    await _messagingService.PublishAsync(domainEvent, context.CancellationToken);
                }

                message.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                message.Error = ex.ToString();
            }
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
