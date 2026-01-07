using MediatR;
using global::Booking.Domain;
using Newtonsoft.Json;

namespace Booking.Application;

internal sealed class BookingDomainEventHandler : 
    INotificationHandler<BookingReservedDomainEvent>,
    INotificationHandler<BookingConfirmedDomainEvent>,
    INotificationHandler<BookingCancelledDomainEvent>
{
    private readonly List<OutboxMessage> _outboxMessages = new();

    // In a real scenario, we would use a service to add these to the context
    // For now, these handlers will be triggered before SaveChangesAsync
    
    public Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(BookingConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task Handle(BookingCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
