using Trip.Domain.Entities;
using Trip.Domain.ValueObjects;
using Trip.Domain.Events;
using Trip.Domain.Enums;

namespace Trip.Domain.Aggregates;

public class Trip
{
    private readonly List<Seat> _seats = new();
    private readonly List<IDomainEvent> _domainEvents = new();

    public Guid TripId { get; }
    public TravelDateTime DepartureTime { get; private set; }
    public TravelDateTime ArrivalTime { get; private set; }
    public TripStatus Status { get; private set; }

    public Route Route { get; }
    public Bus Bus { get; }

    public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


    public Trip(
        Guid tripId,
        TravelDateTime departureTime,
        TravelDateTime arrivalTime,
        Route route,
        Bus bus)
    {
        if (arrivalTime <= departureTime)
            throw new ArgumentException("Arrival time must be after departure time.");

        TripId = tripId;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        Route = route;
        Bus = bus;
        Status = TripStatus.Scheduled;

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
        if (Status != TripStatus.Scheduled)
            throw new InvalidOperationException("Cannot reserve seats for a non-active trip.");

        var seat = _seats.SingleOrDefault(s => s.SeatNumber.Equals(seatNumber));

        if (seat is null)
            throw new InvalidOperationException("Seat does not exist.");

        seat.Reserve();

        _domainEvents.Add(new TripSeatReserved(TripId, seatNumber));
    }

    public void ReleaseSeat(SeatNumber seatNumber)
    {
        if (Status == TripStatus.Cancelled)
            throw new InvalidOperationException("Cannot release seats for a cancelled trip.");

        var seat = _seats.SingleOrDefault(s => s.SeatNumber.Equals(seatNumber));

        if (seat is null)
            throw new InvalidOperationException("Seat does not exist.");

        seat.Release();

        _domainEvents.Add(new TripSeatReleased(TripId, seatNumber));
    }

    public void Cancel()
    {
        if (Status == TripStatus.Cancelled)
            throw new InvalidOperationException("Trip is already cancelled.");

        Status = TripStatus.Cancelled;

        // release all reserved seats automatically when trip is cancelled
        foreach (var seat in _seats.Where(s => !s.IsAvailable))
        {
            seat.Release();
            _domainEvents.Add(new TripSeatReleased(TripId, seat.SeatNumber));
        }

        _domainEvents.Add(new TripCancelled(TripId));
    }
}

