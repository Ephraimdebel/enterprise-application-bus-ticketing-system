using global::Booking.Domain;
using Booking.Application.Interfaces;

namespace Booking.Application;

public sealed class CompleteBookingCommandHandler : ICommandHandler<CompleteBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CompleteBookingCommandHandler(
        IBookingRepository bookingRepository, 
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Guid> Handle(CompleteBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await (Task<Booking.Domain.Booking?>)_bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
        {
            throw new InvalidOperationException("Booking not found");
        }

        booking.Complete(_dateTimeProvider.UtcNow);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}
