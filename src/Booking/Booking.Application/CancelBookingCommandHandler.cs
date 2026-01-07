
using global::Booking.Domain;
namespace Booking.Application;

internal sealed class CancelBookingCommandHandler : ICommandHandler<CancelBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CancelBookingCommandHandler(
        IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Guid> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await (Task<Booking.Domain.Booking?>)_bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
        {
            throw new InvalidOperationException("Booking not found");
        }

        booking.Cancel(_dateTimeProvider.UtcNow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}
