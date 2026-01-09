
using Booking.Domain.Events;
using global::Booking.Domain;
namespace Booking.Application;

internal sealed class ConfirmBookingCommandHandler : ICommandHandler<ConfirmBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ConfirmBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Guid> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await (Task<Booking.Domain.Booking?>)_bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
        {
            throw new InvalidOperationException("Booking not found");
        }

        booking.Confirm(_dateTimeProvider.UtcNow);
        booking.AddDomainEvent(new BookingConfirmedForNotificationEvent(booking.Id, booking.PassengerId));
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}
