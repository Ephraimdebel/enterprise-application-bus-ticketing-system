
using global::Booking.Domain;
namespace Booking.Application;

internal sealed class CreateBookingCommandHandler : ICommandHandler<CreateBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingCommandHandler(IBookingRepository bookingRepository, IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        // In a real scenario, we would check if the passenger and trip exist via domain services or external providers
        
        var travelDate = new TravelDate(request.TravelDate);
        var totalPrice = new Money(request.TotalAmount, request.Currency);
        var seatNumbers = request.SeatNumbers.Select(s => new SeatNumber(s)).ToList();

        var booking = Booking.Domain.Booking.Reserve(
            request.PassengerId,
            request.TripId,
            travelDate,
            totalPrice,
            seatNumbers);

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return booking.Id;
    }
}
