using global::Booking.Domain;

namespace Booking.Application;

internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly IBookingRepository _bookingRepository;

    public GetBookingQueryHandler(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        var booking = await (Task<Booking.Domain.Booking?>)_bookingRepository.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking is null)
        {
            throw new InvalidOperationException("Booking not found");
        }

        return new BookingResponse(
            booking.Id,
            booking.PassengerId,
            booking.TripId,
            booking.Status.ToString(),
            booking.TotalPrice.Amount,
            booking.TotalPrice.Currency,
            booking.Tickets.Select(t => t.SeatNumber.Value).ToList());
    }
}
