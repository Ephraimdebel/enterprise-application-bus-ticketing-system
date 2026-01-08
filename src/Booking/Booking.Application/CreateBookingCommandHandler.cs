using global::Booking.Domain;
using Booking.Application.Interfaces;

namespace Booking.Application;

public sealed class CreateBookingCommandHandler : ICommandHandler<CreateBookingCommand>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITripService _tripService;
    private readonly IPassengerService _passengerService;

    public CreateBookingCommandHandler(
        IBookingRepository bookingRepository, 
        IUnitOfWork unitOfWork,
        ITripService tripService,
        IPassengerService passengerService)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _tripService = tripService;
        _passengerService = passengerService;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        if (!await _tripService.ExistsAsync(request.TripId, cancellationToken))
        {
            throw new InvalidOperationException("Trip does not exist.");
        }

        if (!await _passengerService.ExistsAsync(request.PassengerId, cancellationToken))
        {
            throw new InvalidOperationException("Passenger does not exist.");
        }
        
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
