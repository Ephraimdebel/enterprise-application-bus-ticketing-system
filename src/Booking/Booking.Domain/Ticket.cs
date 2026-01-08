
namespace Booking.Domain;

public sealed class Ticket : Entity
{
    private Ticket() { } // Required for EF Core

    public Guid BookingId { get; private set; }
    public SeatNumber SeatNumber { get; private set; } = default!;

    internal static Ticket Create(Guid bookingId, SeatNumber seatNumber)
    {
        return new Ticket
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            SeatNumber = seatNumber
        };
    }
}
