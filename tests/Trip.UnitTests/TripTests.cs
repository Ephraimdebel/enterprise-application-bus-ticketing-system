using Trip.Domain.Aggregates;
using Trip.Domain.Enums;
using Trip.Domain.ValueObjects;
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
        // Arrange
        var tripId = Guid.NewGuid();
        var departureTime = new TravelDateTime(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TimeOnly.FromTimeSpan(TimeSpan.FromHours(14)));
        var arrivalTime = new TravelDateTime(DateOnly.FromDateTime(DateTime.Now.AddDays(1)), TimeOnly.FromTimeSpan(TimeSpan.FromHours(10)));
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Trip.Domain.Aggregates.Trip(
            tripId,
            departureTime,
            arrivalTime,
            Guid.NewGuid(),
            Guid.NewGuid(),
            40,
            new TripPrice(150)));
    }
}
