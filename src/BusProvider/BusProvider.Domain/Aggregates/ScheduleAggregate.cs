using BusProvider.Domain.Abstractions;
using BusProvider.Domain.Events;
using BusProvider.Domain.ValueObjects;

namespace BusProvider.Domain.Aggregates;

public sealed class ScheduleAggregate : AggregateRoot
{
    private ScheduleAggregate()
    {
    }

    private ScheduleAggregate(Guid id, Guid busId, Guid routeId, TravelDate tripDate, TravelTime departure, TravelTime arrival, SeatCapacity seatsAvailable)
    {
        Id = id;
        BusId = busId;
        RouteId = routeId;
        TripDate = tripDate;
        Departure = departure;
        Arrival = arrival;
        SeatsAvailable = seatsAvailable;
        RaiseDomainEvent(new TripPublished(id, busId, routeId, tripDate.Value, departure.Value, arrival.Value, seatsAvailable.Value, DateTime.UtcNow));
    }

    public Guid BusId { get; private set; }
    public Guid RouteId { get; private set; }
    public TravelDate TripDate { get; private set; } = TravelDate.Create(DateOnly.FromDateTime(DateTime.UtcNow.Date));
    public TravelTime Departure { get; private set; } = TravelTime.Create(new TimeOnly(0, 0));
    public TravelTime Arrival { get; private set; } = TravelTime.Create(new TimeOnly(0, 0));
    public SeatCapacity SeatsAvailable { get; private set; } = SeatCapacity.Create(1);

    public static ScheduleAggregate Create(Guid busId, Guid routeId, DateOnly tripDate, TimeOnly departure, TimeOnly arrival, int seatsAvailable)
    {
        if (busId == Guid.Empty)
        {
            throw new ArgumentException("BusId is required", nameof(busId));
        }

        if (routeId == Guid.Empty)
        {
            throw new ArgumentException("RouteId is required", nameof(routeId));
        }

        return new ScheduleAggregate(
            Guid.NewGuid(),
            busId,
            routeId,
            TravelDate.Create(tripDate),
            TravelTime.Create(departure),
            TravelTime.Create(arrival),
            SeatCapacity.Create(seatsAvailable));
    }

    public void Update(DateOnly tripDate, TimeOnly departure, TimeOnly arrival, int seatsAvailable)
    {
        TripDate = TravelDate.Create(tripDate);
        Departure = TravelTime.Create(departure);
        Arrival = TravelTime.Create(arrival);
        SeatsAvailable = SeatCapacity.Create(seatsAvailable);
    }
}
