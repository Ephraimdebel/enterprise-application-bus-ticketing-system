namespace Trip.Domain.ValueObjects;

public sealed class SeatNumber : IEquatable<SeatNumber>
{
    public string Number { get; }

    public SeatNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
            throw new ArgumentException("Seat number cannot be empty", nameof(number));

        Number = number;
    }

    public bool Equals(SeatNumber? other)
    {
        if (other is null) return false;
        return Number == other.Number;
    }

    public override bool Equals(object? obj)
        => Equals(obj as SeatNumber);

    public override int GetHashCode()
        => Number.GetHashCode();
}
