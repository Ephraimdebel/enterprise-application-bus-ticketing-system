namespace Trip.Domain.ValueObjects;

public sealed class TravelDateTime : IEquatable<TravelDateTime>
{
    public DateOnly Date { get; }
    public TimeOnly Time { get; }

    public TravelDateTime(DateOnly date, TimeOnly time)
    {
        Date = date;
        Time = time;
    }

    public bool Equals(TravelDateTime? other)
    {
        if (other is null) return false;
        return Date == other.Date && Time == other.Time;
    }

    public override bool Equals(object? obj)
        => Equals(obj as TravelDateTime);

    public bool IsAfter(TravelDateTime other)
    {
        if (other is null)
            throw new ArgumentNullException(nameof(other));

        if (Date > other.Date) return true;
        if (Date < other.Date) return false;

        return Time > other.Time;
    }


    public override int GetHashCode()
        => HashCode.Combine(Date, Time);

}
