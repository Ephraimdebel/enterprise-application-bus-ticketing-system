using BusProvider.Domain.Abstractions;
using BusProvider.Domain.Events;
using BusProvider.Domain.ValueObjects;

namespace BusProvider.Domain.Aggregates;

public sealed class BusAggregate : AggregateRoot
{
    private BusAggregate()
    {
    }

    private BusAggregate(Guid id, Guid providerId, BusNumber busNumber, BusType busType, SeatCapacity seatCapacity)
    {
        Id = id;
        ProviderId = providerId;
        BusNumber = busNumber;
        BusType = busType;
        SeatCapacity = seatCapacity;
        RaiseDomainEvent(new BusAdded(id, providerId, busNumber.Value, busType.Value, seatCapacity.Value, DateTime.UtcNow));
    }

    public Guid ProviderId { get; private set; }
    public BusNumber BusNumber { get; private set; } = BusNumber.Create("placeholder");
    public BusType BusType { get; private set; } = BusType.Create("standard");
    public SeatCapacity SeatCapacity { get; private set; } = SeatCapacity.Create(1);

    public static BusAggregate Create(Guid providerId, string busNumber, string busType, int seatCapacity)
    {
        if (providerId == Guid.Empty)
        {
            throw new ArgumentException("ProviderId is required", nameof(providerId));
        }

        return new BusAggregate(Guid.NewGuid(), providerId, BusNumber.Create(busNumber), BusType.Create(busType), SeatCapacity.Create(seatCapacity));
    }

    public void Update(string busNumber, string busType, int seatCapacity)
    {
        BusNumber = BusNumber.Create(busNumber);
        BusType = BusType.Create(busType);
        SeatCapacity = SeatCapacity.Create(seatCapacity);
    }
}
