using Booking.Domain;

namespace Booking.UnitTests;

public class BookingTests
{
    [Fact]
    public void Reserve_ShouldSetStatusToReserved_AndRaiseDomainEvent()
    {
        // Arrange
        var passengerId = Guid.NewGuid();
        var tripId = Guid.NewGuid();
        var travelDate = new TravelDate(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        var totalPrice = new Money(100, "USD");
        var seatNumbers = new List<SeatNumber> { new SeatNumber("A1") };

        // Act
        var booking = Booking.Domain.Booking.Reserve(passengerId, tripId, travelDate, totalPrice, seatNumbers);

        // Assert
        Assert.Equal(BookingStatus.Reserved, booking.Status);
        Assert.Single(booking.Tickets);
        Assert.Contains(booking.GetDomainEvents(), e => e is BookingReservedDomainEvent);
    }

    [Fact]
    public void Confirm_ShouldSetStatusToConfirmed_WhenStatusIsReserved()
    {
        // Arrange
        var booking = CreateReservedBooking();

        // Act
        booking.Confirm(DateTime.UtcNow);

        // Assert
        Assert.Equal(BookingStatus.Confirmed, booking.Status);
        Assert.Contains(booking.GetDomainEvents(), e => e is BookingConfirmedDomainEvent);
    }

    [Fact]
    public void Confirm_ShouldThrowException_WhenStatusIsNotReserved()
    {
        // Arrange
        var booking = CreateReservedBooking();
        booking.Cancel(DateTime.UtcNow);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => booking.Confirm(DateTime.UtcNow));
    }

    [Fact]
    public void Cancel_ShouldSetStatusToCancelled_WhenStatusIsReserved()
    {
        // Arrange
        var booking = CreateReservedBooking();

        // Act
        booking.Cancel(DateTime.UtcNow);

        // Assert
        Assert.Equal(BookingStatus.Cancelled, booking.Status);
        Assert.Contains(booking.GetDomainEvents(), e => e is BookingCancelledDomainEvent);
    }

    [Fact]
    public void Fail_ShouldSetStatusToFailed_AndRaiseDomainEvent()
    {
        // Arrange
        var booking = CreateReservedBooking();

        // Act
        booking.Fail(DateTime.UtcNow);

        // Assert
        Assert.Equal(BookingStatus.Failed, booking.Status);
        Assert.Contains(booking.GetDomainEvents(), e => e is BookingFailedDomainEvent);
    }

    [Fact]
    public void Complete_ShouldSetStatusToCompleted_WhenStatusIsConfirmed()
    {
        // Arrange
        var booking = CreateReservedBooking();
        booking.Confirm(DateTime.UtcNow);

        // Act
        booking.Complete(DateTime.UtcNow);

        // Assert
        Assert.Equal(BookingStatus.Completed, booking.Status);
    }

    private Booking.Domain.Booking CreateReservedBooking()
    {
        return Booking.Domain.Booking.Reserve(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new TravelDate(DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
            new Money(100, "USD"),
            new List<SeatNumber> { new SeatNumber("A1") });
    }
}
