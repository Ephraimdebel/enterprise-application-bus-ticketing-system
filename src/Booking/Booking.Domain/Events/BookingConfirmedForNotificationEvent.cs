using System;
using Booking.Domain;

namespace Booking.Domain.Events
{
    public class BookingConfirmedForNotificationEvent : IDomainEvent
    {
        public Guid BookingId { get; }
        public Guid PassengerId { get; }
        public DateTime OccurredOn { get; }  // ✅ Match interface

        public BookingConfirmedForNotificationEvent(Guid bookingId, Guid passengerId)
        {
            BookingId = bookingId;
            PassengerId = passengerId;
            OccurredOn = DateTime.UtcNow;   // ✅ Use OccurredOn
        }
    }
}
