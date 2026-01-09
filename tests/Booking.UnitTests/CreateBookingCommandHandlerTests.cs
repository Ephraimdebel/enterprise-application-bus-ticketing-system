using Moq;
using Booking.Application;
using Booking.Application.Interfaces;
using global::Booking.Domain;
using Xunit;

namespace Booking.UnitTests;

public class CreateBookingCommandHandlerTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ITripService> _tripServiceMock;
    private readonly Mock<IPassengerService> _passengerServiceMock;
    private readonly CreateBookingCommandHandler _handler;

    public CreateBookingCommandHandlerTests()
    {
        _bookingRepositoryMock = new Mock<IBookingRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _tripServiceMock = new Mock<ITripService>();
        _passengerServiceMock = new Mock<IPassengerService>();

        _handler = new CreateBookingCommandHandler(
            _bookingRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _tripServiceMock.Object,
            _passengerServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenTripDoesNotExist()
    {
        // Arrange
        var command = CreateCommand();
        _tripServiceMock.Setup(x => x.ExistsAsync(command.TripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPassengerDoesNotExist()
    {
        // Arrange
        var command = CreateCommand();
        _tripServiceMock.Setup(x => x.ExistsAsync(command.TripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _passengerServiceMock.Setup(x => x.ExistsAsync(command.PassengerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldSucceed_WhenValid()
    {
        // Arrange
        var command = CreateCommand();
        _tripServiceMock.Setup(x => x.ExistsAsync(command.TripId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        _passengerServiceMock.Setup(x => x.ExistsAsync(command.PassengerId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _bookingRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Booking.Domain.Booking>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    private CreateBookingCommand CreateCommand()
    {
        return new CreateBookingCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            100,
            "USD",
            new List<string> { "A1" });
    }
}
