using Trip.Domain.ValueObjects;

namespace Trip.Domain.Entities;

public class Seat
{
    public Guid SeatId { get; }
    public SeatNumber SeatNumber { get; }
    public bool IsAvailable { get; private set; }

    public Seat(Guid seatId, SeatNumber seatNumber)
    {
        SeatId = seatId;
        SeatNumber = seatNumber;
        IsAvailable = true;
    }

    public void Reserve()
    {
        if (!IsAvailable)
            throw new InvalidOperationException("Seat is already reserved.");

        IsAvailable = false;
    }

    public void Release()
    {
        if (IsAvailable)
            throw new InvalidOperationException("Seat is already available.");

        IsAvailable = true;
    }
}
