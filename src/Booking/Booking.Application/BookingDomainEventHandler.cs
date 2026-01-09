// using MediatR;
// using global::Booking.Domain;
// using Newtonsoft.Json;

// namespace Booking.Application;

// internal sealed class BookingDomainEventHandler : 
//     INotificationHandler<BookingReservedDomainEvent>,
//     INotificationHandler<BookingConfirmedDomainEvent>,
//     INotificationHandler<BookingCancelledDomainEvent>
// {
//     private readonly List<OutboxMessage> _outboxMessages = new();

//     // In a real scenario, we would use a service to add these to the context
//     // For now, these handlers will be triggered before SaveChangesAsync

//     public Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
//     {
//         return Task.CompletedTask;
//     }

//     public Task Handle(BookingConfirmedDomainEvent notification, CancellationToken cancellationToken)
//     {
//         return Task.CompletedTask;
//     }

//     public Task Handle(BookingCancelledDomainEvent notification, CancellationToken cancellationToken)
//     {
//         return Task.CompletedTask;
//     }
// }
using MediatR;
using Booking.Domain;
using Booking.Application.Interfaces;
using Booking.Domain.Events;

namespace Booking.Application;

internal sealed class BookingDomainEventHandler :
    INotificationHandler<BookingReservedDomainEvent>,
    INotificationHandler<BookingConfirmedDomainEvent>,
    INotificationHandler<BookingCancelledDomainEvent>
{
    private readonly IOutboxService _outboxService;

    public BookingDomainEventHandler(IOutboxService outboxService)
    {
        _outboxService = outboxService;
    }

    public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _outboxService.AddAsync(notification, cancellationToken);
    }

    public async Task Handle(BookingConfirmedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Wrap BookingConfirmedDomainEvent into a Payment-specific event
        var paymentEvent = new BookingConfirmedForPaymentEvent(notification.BookingId, notification.TotalAmount);
        await _outboxService.AddAsync(paymentEvent, cancellationToken);
    }
        public async Task Handle(BookingConfirmedForNotificationEvent notification, CancellationToken cancellationToken)
    {
        // JUST pass the domain event to OutboxService
        await _outboxService.AddAsync(notification, cancellationToken);
    }


    public async Task Handle(BookingCancelledDomainEvent notification, CancellationToken cancellationToken)
    {
        await _outboxService.AddAsync(notification, cancellationToken);
    }




}
