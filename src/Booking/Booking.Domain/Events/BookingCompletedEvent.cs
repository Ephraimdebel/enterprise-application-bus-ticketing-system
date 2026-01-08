using Booking.Domain;
using System;

namespace Booking.Domain.Events
{
    public class BookingCompletedEvent : IDomainEvent
    {
        public Guid BookingId { get; }
        public Guid PassengerId { get; }
        public Guid TripId { get; }
        public decimal TotalAmount { get; }
        public DateTime OccurredOn { get; }

        public BookingCompletedEvent(Guid bookingId, Guid passengerId, Guid tripId, decimal totalAmount)
        {
            BookingId = bookingId;
            PassengerId = passengerId;
            TripId = tripId;
            TotalAmount = totalAmount;
            OccurredOn = DateTime.UtcNow;
        }
    }
}