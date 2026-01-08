using Trip.Domain.ValueObjects;

namespace Trip.Domain.Events
{
    public sealed class TripScheduleUpdated : IDomainEvent
    {
        public Guid TripId { get; }
        public TravelDateTime NewDepartureTime { get; }
        public TravelDateTime NewArrivalTime { get; }
        public DateTime OccurredOn { get; }

        public TripScheduleUpdated(Guid tripId, TravelDateTime newDepartureTime, TravelDateTime newArrivalTime)
        {
            TripId = tripId;
            NewDepartureTime = newDepartureTime;
            NewArrivalTime = newArrivalTime;
            OccurredOn = DateTime.UtcNow;
        }
    }
}
