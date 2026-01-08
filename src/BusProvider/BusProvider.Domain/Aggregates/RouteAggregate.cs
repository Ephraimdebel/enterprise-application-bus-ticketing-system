using BusProvider.Domain.Abstractions;
using BusProvider.Domain.Events;
using BusProvider.Domain.ValueObjects;

namespace BusProvider.Domain.Aggregates;

public sealed class RouteAggregate : AggregateRoot
{
    private RouteAggregate()
    {
    }

    private RouteAggregate(Guid id, Guid busId, Location start, Location end, Distance distance)
    {
        Id = id;
        BusId = busId;
        Start = start;
        End = end;
        Distance = distance;
        RaiseDomainEvent(new RouteCreated(id, busId, start.Value, end.Value, distance.Kilometers, DateTime.UtcNow));
    }

    public Guid BusId { get; private set; }
    public Location Start { get; private set; } = Location.Create("start");
    public Location End { get; private set; } = Location.Create("end");
    public Distance Distance { get; private set; } = Distance.Create(1);

    public static RouteAggregate Create(Guid busId, string start, string end, double distanceKm)
    {
        if (busId == Guid.Empty)
        {
            throw new ArgumentException("BusId is required", nameof(busId));
        }

        return new RouteAggregate(Guid.NewGuid(), busId, Location.Create(start), Location.Create(end), Distance.Create(distanceKm));
    }

    public void Update(string start, string end, double distanceKm)
    {
        Start = Location.Create(start);
        End = Location.Create(end);
        Distance = Distance.Create(distanceKm);
    }
}
