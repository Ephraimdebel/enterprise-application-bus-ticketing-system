namespace Trip.Api.Controllers;

public sealed class UpdateTripRequest
{
    public DateOnly NewDepartureDate { get; init; }
    public TimeOnly NewDepartureTime { get; init; }

    public DateOnly NewArrivalDate { get; init; }
    public TimeOnly NewArrivalTime { get; init; }

    public decimal NewPrice { get; init; }
}
