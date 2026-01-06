using Trip.Domain.ValueObjects;

namespace Trip.Domain.Events;

public sealed class TripSeatReserved : IDomainEvent
{
    public Guid TripId { get; }
    public SeatNumber SeatNumber { get; }
    public DateTime OccurredOn { get; }

    public TripSeatReserved(Guid tripId, SeatNumber seatNumber)
    {
        TripId = tripId;
        SeatNumber = seatNumber;
        OccurredOn = DateTime.UtcNow;
    }
}
