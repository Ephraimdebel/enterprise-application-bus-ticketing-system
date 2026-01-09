using Trip.Domain.ValueObjects;
using Trip.Domain.Events;
using Trip.Domain.Enums;
using Trip.Domain.Entities;

namespace Trip.Domain.Aggregates;

public class Trip
{
    private readonly List<Seat> _seats = new();
    private readonly List<IDomainEvent> _domainEvents = new();

    public Guid TripId { get; private set; }
    public TripPrice Price { get; private set; }
    public TravelDateTime DepartureTime { get; private set; }
    public TravelDateTime ArrivalTime { get; private set; }

    public TripStatus Status { get; private set; }

    public Guid BusId { get; private set; }
    public Guid RouteId { get; private set; }

    public IReadOnlyCollection<Seat> Seats => _seats.AsReadOnly();
    
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();

    private Trip() { }

    public Trip(
        Guid tripId,
        TravelDateTime departureTime,
        TravelDateTime arrivalTime,
        Guid busId,
        Guid routeId,
        int seatCapacity,
        TripPrice price)
    {
        if (!arrivalTime.IsAfter(departureTime))
            throw new ArgumentException("Arrival time must be after departure time.");

        TripId = tripId;
        DepartureTime = departureTime;
        ArrivalTime = arrivalTime;
        BusId = busId;
        RouteId = routeId;
        Status = TripStatus.Scheduled;

        Price = price;
        InitializeSeats(seatCapacity);
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

        var seat = _seats.SingleOrDefault(s => s.SeatNumber.Equals(seatNumber))
            ?? throw new InvalidOperationException("Seat does not exist.");

        seat.Reserve();
        _domainEvents.Add(new TripSeatReserved(TripId, seatNumber));
    }

    public void ReleaseSeat(SeatNumber seatNumber)
    {
        if (Status == TripStatus.Cancelled)
            throw new InvalidOperationException("Cannot release seats for a cancelled trip.");

        var seat = _seats.SingleOrDefault(s => s.SeatNumber.Equals(seatNumber))
            ?? throw new InvalidOperationException("Seat does not exist.");

        seat.Release();
        _domainEvents.Add(new TripSeatReleased(TripId, seatNumber));
    }

    public void Cancel()
    {
        if (Status == TripStatus.Cancelled)
            throw new InvalidOperationException("Trip is already cancelled.");

        Status = TripStatus.Cancelled;

        foreach (var seat in _seats.Where(s => !s.IsAvailable))
        {
            seat.Release();
            _domainEvents.Add(new TripSeatReleased(TripId, seat.SeatNumber));
        }

        _domainEvents.Add(new TripCancelled(TripId));
    }

    public void UpdateSchedule(TravelDateTime newDepartureTime, TravelDateTime newArrivalTime, TripPrice newPrice)
    {
        if (!newArrivalTime.IsAfter(newDepartureTime))
            throw new ArgumentException("Arrival time must be after departure time.");

        DepartureTime = newDepartureTime;
        ArrivalTime = newArrivalTime;
        Price = newPrice;

        _domainEvents.Add(new TripScheduleUpdated(TripId, newDepartureTime, newArrivalTime));
    }

    public void Complete()
    {
        if (Status != TripStatus.Scheduled)
            throw new InvalidOperationException("Only scheduled trips can be completed.");

        Status = TripStatus.Completed;
        _domainEvents.Add(new TripCompleted(TripId));
    }
}


