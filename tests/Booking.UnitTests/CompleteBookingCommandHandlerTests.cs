using Moq;
using Booking.Application;
using Booking.Application.Interfaces;
using global::Booking.Domain;
using Xunit;

namespace Booking.UnitTests;

public class CompleteBookingCommandHandlerTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly CompleteBookingCommandHandler _handler;

    public CompleteBookingCommandHandlerTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(x => x.UtcNow).Returns(DateTime.UtcNow);

        _handler = new CompleteBookingCommandHandler(
            _bookingRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _dateTimeProviderMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCompleteBooking_WhenStatusIsConfirmed()
    {
        // Arrange
        var booking = CreateConfirmedBooking();
        _bookingRepositoryMock.Setup(x => x.GetByIdAsync(booking.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(booking);

        var command = new CompleteBookingCommand(booking.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(booking.Id, result);
        Assert.Equal(BookingStatus.Completed, booking.Status);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenBookingNotFound()
    {
        // Arrange
        var bookingId = Guid.NewGuid();
        _bookingRepositoryMock.Setup(x => x.GetByIdAsync(bookingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking.Domain.Booking?)null);

        var command = new CompleteBookingCommand(bookingId);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    private Booking.Domain.Booking CreateConfirmedBooking()
    {
        var booking = Booking.Domain.Booking.Reserve(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new TravelDate(DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
            new Money(100, "USD"),
            new List<SeatNumber> { new SeatNumber("A1") });
        
        booking.Confirm(DateTime.UtcNow);
        return booking;
    }
}
