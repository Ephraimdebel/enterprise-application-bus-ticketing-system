using BusProvider.Application.Queries.Buses;
using BusProvider.Application.Queries.Routes;
using BusProvider.Application.Queries.Schedules;
using BusProvider.Domain.Aggregates;

namespace BusProvider.Application.Mapping;

public static class Mappers
{
    public static BusResponse ToResponse(this BusAggregate bus)
        => new(bus.Id, bus.ProviderId, bus.BusNumber.Value, bus.BusType.Value, bus.SeatCapacity.Value);

    public static RouteResponse ToResponse(this RouteAggregate route)
        => new(route.Id, route.BusId, route.Start.Value, route.End.Value, route.Distance.Kilometers);

    public static ScheduleResponse ToResponse(this ScheduleAggregate s)
        => new(s.Id, s.BusId, s.RouteId, s.TripDate.Value, s.Departure.Value, s.Arrival.Value, s.SeatsAvailable.Value);
}
