namespace Trip.Application.DTOs;

public sealed class TripDto
{
    public Guid TripId { get; init; }
    public string Status { get; init; } = default!;

    public DateOnly DepartureDate { get; init; }
    public TimeOnly DepartureTime { get; init; }

    public DateOnly ArrivalDate { get; init; }
    public TimeOnly ArrivalTime { get; init; }

    public string Origin { get; init; } = default!;
    public string Destination { get; init; } = default!;

    public int TotalSeats { get; init; }
    public int AvailableSeats { get; init; }
}
