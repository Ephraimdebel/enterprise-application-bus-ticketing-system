namespace Trip.Domain.Entities;

public class Route
{
    public Guid RouteId { get; }
    public string Origin { get; }
    public string Destination { get; }
    public double DistanceKm { get; }
    public TimeSpan EstimatedDuration { get; }

    public Route(
        Guid routeId,
        string origin,
        string destination,
        double distanceKm,
        TimeSpan estimatedDuration)
    {
        if (string.IsNullOrWhiteSpace(origin))
            throw new ArgumentException("Origin cannot be empty.");

        if (string.IsNullOrWhiteSpace(destination))
            throw new ArgumentException("Destination cannot be empty.");

        if (distanceKm <= 0)
            throw new ArgumentException("Distance must be greater than zero.");

        RouteId = routeId;
        Origin = origin;
        Destination = destination;
        DistanceKm = distanceKm;
        EstimatedDuration = estimatedDuration;
    }
}
