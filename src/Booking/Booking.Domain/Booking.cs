
using Booking.Domain.Events;

namespace Booking.Domain;

public sealed class Booking : Entity
{
    private readonly List<Ticket> _tickets = new();
    
    private Booking() { } // Required for EF Core

    public Guid PassengerId { get; private set; }
    public Guid TripId { get; private set; }
    public TravelDate TravelDate { get; private set; } = default!;
    public Money TotalPrice { get; private set; } = default!;
    public BookingStatus Status { get; private set; } = default!;
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? CancelledOnUtc { get; private set; }

    public IReadOnlyList<Ticket> Tickets => _tickets.AsReadOnly();

    public static Booking Reserve(
        Guid passengerId,
        Guid tripId,
        TravelDate travelDate,
        Money totalPrice,
        List<SeatNumber> seatNumbers)
    {
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            PassengerId = passengerId,
            TripId = tripId,
            TravelDate = travelDate,
            TotalPrice = totalPrice,
            Status = BookingStatus.Reserved,
            CreatedOnUtc = DateTime.UtcNow
        };

        foreach (var seatNumber in seatNumbers)
        {
            booking._tickets.Add(Ticket.Create(booking.Id, seatNumber));
        }

        booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));

        return booking;
    }

    public void Confirm(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            throw new InvalidOperationException("Only reserved bookings can be confirmed.");
        }

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcNow;

        RaiseDomainEvent(new BookingConfirmedDomainEvent(Id,TotalPrice.Amount));
        // NEW: Notification Event
        RaiseDomainEvent(new BookingConfirmedForNotificationEvent(Id, PassengerId));
    }

    public void Cancel(DateTime utcNow)
    {
        if (Status == BookingStatus.Cancelled || Status == BookingStatus.Completed)
        {
            throw new InvalidOperationException("Booking cannot be cancelled in current status.");
        }

        Status = BookingStatus.Cancelled;
        CancelledOnUtc = utcNow;

        RaiseDomainEvent(new BookingCancelledDomainEvent(Id));
    }

    public void Fail(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            throw new InvalidOperationException("Only reserved bookings can fail.");
        }

        Status = BookingStatus.Failed;
        
        RaiseDomainEvent(new BookingFailedDomainEvent(Id, PassengerId));
    }

    public void Complete(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException("Only confirmed bookings can be completed.");
        }

        Status = BookingStatus.Completed;
    }
}
