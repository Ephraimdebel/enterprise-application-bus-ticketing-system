using BusProvider.Domain.Aggregates;
using BusProvider.Domain.ValueObjects;
using Xunit;

namespace BusProvider.UnitTests;

public class BusProviderTests
{
    [Fact]
    public void Create_ShouldCreateBusAggregate_WhenValid()
    {
        // Arrange
        var providerId = Guid.NewGuid();
        var busNumber = "BUS-123";
        var busType = "Luxury";
        var seatCapacity = 50;

        // Act
        var bus = BusAggregate.Create(providerId, busNumber, busType, seatCapacity);

        // Assert
        Assert.Equal(providerId, bus.ProviderId);
        Assert.Equal(busNumber, bus.BusNumber.Value);
        Assert.Equal(busType, bus.BusType.Value);
        Assert.Equal(seatCapacity, bus.SeatCapacity.Value);
    }

    [Fact]
    public void Create_ShouldThrowException_WhenProviderIdIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => BusAggregate.Create(
            Guid.Empty,
            "BUS-123",
            "Luxury",
            50));
    }
}
