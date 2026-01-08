namespace Trip.Api.Controllers;

public sealed class CreateTripRequest
{
    public Guid BusId { get; init; }
    public Guid RouteId { get; init; }

    public DateOnly DepartureDate { get; init; }
    public TimeOnly DepartureTime { get; init; }

    public DateOnly ArrivalDate { get; init; }
    public TimeOnly ArrivalTime { get; init; }

    public int SeatCapacity { get; init; }
    public decimal Price { get; init; }
}
