using Trip.Domain.Aggregates;
using Trip.Domain.Enums;
using Trip.Domain.ValueObjects;
using Trip.Domain.Events;
using Xunit;

namespace Trip.UnitTests;

public class TripTests
{
    [Fact]
    public void Constructor_ShouldCreateTrip_WhenValid()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var departureTime = new TravelDateTime(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)));
        var arrivalTime = new TravelDateTime(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TimeOnly.FromTimeSpan(TimeSpan.FromHours(14)));
        var busId = Guid.NewGuid();
        var routeId = Guid.NewGuid();
        var seatCapacity = 40;
        var price = new TripPrice(150);

        // Act
        var trip = new Trip.Domain.Aggregates.Trip(
            tripId,
            departureTime,
            arrivalTime,
            busId,
            routeId,
            seatCapacity,
            price);

        // Assert
        Assert.Equal(tripId, trip.TripId);
        Assert.Equal(TripStatus.Scheduled, trip.Status);
        Assert.Equal(seatCapacity, trip.Seats.Count);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenArrivalIsBeforeDeparture()
    {
        // ... (existing code)
    }

    [Fact]
    public void Complete_ShouldSetStatusToCompleted_AndRaiseDomainEvent()
    {
        // Arrange
        var trip = new Trip.Domain.Aggregates.Trip(
            Guid.NewGuid(),
            new TravelDateTime(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TimeOnly.FromTimeSpan(TimeSpan.FromHours(10))),
            new TravelDateTime(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TimeOnly.FromTimeSpan(TimeSpan.FromHours(14))),
            Guid.NewGuid(),
            Guid.NewGuid(),
            40,
            new TripPrice(150));

        // Act
        trip.Complete();

        // Assert
        Assert.Equal(TripStatus.Completed, trip.Status);
        Assert.Contains(trip.GetDomainEvents(), e => e is TripCompleted);
    }
}
