using Trip.Domain.Entities;
using Trip.Domain.ValueObjects;

namespace Trip.Domain.Aggregates;

public class Trip
{
    private readonly List<Seat> _seats = new();

    public Guid TripId { get; }
    public TravelDateTime DepartureTime { get; private set; }
    public TravelDateTime ArrivalTime { get; private set; }
    public string Status { get; private set; }

    public Route Route { get; }
    public Bus Bus { get; }

    public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();

    public Trip(
        Guid tripId,
        TravelDateTime departureTime,
        TravelDateTime arrivalTime,
        Route route,
        Bus bus)
    {
        if (arrivalTime.Equals(departureTime))
            throw new ArgumentException("Arrival time must be after departure time.");

        TripId = tripId;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        Route = route;
        Bus = bus;
        Status = "Scheduled";

        InitializeSeats(bus.SeatCapacity);
    }

    private void InitializeSeats(int seatCapacity)
    {
        for (int i = 1; i <= seatCapacity; i++)
        {
            var seatNumber = new SeatNumber(i.ToString());
            _seats.Add(new Seat(Guid.NewGuid(), seatNumber));
        }
    }

    public void ReserveSeat(SeatNumber seatNumber)
    {
        if (Status != "Scheduled")
            throw new InvalidOperationException("Cannot reserve seats for a non-active trip.");

        var seat = _seats.SingleOrDefault(s => s.SeatNumber.Equals(seatNumber));

        if (seat is null)
            throw new InvalidOperationException("Seat does not exist.");

        seat.Reserve();
    }

    public void ReleaseSeat(SeatNumber seatNumber)
    {
        var seat = _seats.SingleOrDefault(s => s.SeatNumber.Equals(seatNumber));

        if (seat is null)
            throw new InvalidOperationException("Seat does not exist.");

        seat.Release();
    }

    public void Cancel()
    {
        Status = "Cancelled";
    }
}
