public sealed class BookingConfirmedForNotificationEvent
{
    public Guid BookingId { get; set; }
    public Guid PassengerId { get; set; }
}
