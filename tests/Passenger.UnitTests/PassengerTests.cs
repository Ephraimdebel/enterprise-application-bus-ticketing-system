using Moq;
using Passenger.Domain.Aggregates;
using Passenger.Domain.Entities;
using Passenger.Domain.ValueObjects;
using Xunit;

namespace Passenger.UnitTests;

public class PassengerTests
{
    [Fact]
    public void Register_ShouldCreatePassenger_WhenValid()
    {
        // Arrange
        var id = PassengerId.New();
        var name = Name.Create("John", "Doe");
        var email = Email.Create("john.doe@example.com");
        var phoneNumber = PhoneNumber.Create("+1", "555123456");
        var utcNow = DateTime.UtcNow;

        // Act
        var passenger = global::Passenger.Domain.Aggregates.Passenger.Register(
            id, name, email, phoneNumber, true, utcNow);

        // Assert
        Assert.Equal(id, passenger.Id);
        Assert.Equal(name, passenger.Name);
        Assert.Equal(email, passenger.Email);
        Assert.Equal(phoneNumber, passenger.PhoneNumber);
    }
}
