using Booking.Application.Interfaces;
using Booking.Domain;
using Newtonsoft.Json;

namespace Booking.Infrastructure;

public sealed class OutboxService : IOutboxService
{
    private readonly BookingDbContext _dbContext;

    public OutboxService(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var json = JsonConvert.SerializeObject(domainEvent, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        });

        var outboxMessage = new Booking.Application.OutboxMessage(
            Guid.NewGuid(),
            DateTime.UtcNow,
            domainEvent.GetType().FullName!,
            json
        );

        await _dbContext.OutboxMessages.AddAsync(outboxMessage, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    
}
