namespace Trip.Domain.Entities;

public class Bus
{
    public Guid BusId { get; }
    public string BusName { get; }
    public string PlateNumber { get; }
    public int SeatCapacity { get; }
    public string BusType { get; }

    public Bus(
        Guid busId,
        string busName,
        string plateNumber,
        int seatCapacity,
        string busType)
    {
        if (string.IsNullOrWhiteSpace(busName))
            throw new ArgumentException("Bus name cannot be empty.");

        if (string.IsNullOrWhiteSpace(plateNumber))
            throw new ArgumentException("Plate number cannot be empty.");

        if (seatCapacity <= 0)
            throw new ArgumentException("Seat capacity must be greater than zero.");

        BusId = busId;
        BusName = busName;
        PlateNumber = plateNumber;
        SeatCapacity = seatCapacity;
        BusType = busType;
    }
}
