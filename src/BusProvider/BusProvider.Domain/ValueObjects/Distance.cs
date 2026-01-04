using BusProvider.Domain.Abstractions;

namespace BusProvider.Domain.ValueObjects;

public sealed class Distance : ValueObject
{
    private Distance(double kilometers)
    {
        Kilometers = kilometers;
    }

    public double Kilometers { get; }

    public static Distance Create(double kilometers)
    {
        if (kilometers <= 0)
        {
            throw new ArgumentException("Distance must be greater than zero", nameof(kilometers));
        }

        return new Distance(kilometers);
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Kilometers;
    }

    public override string ToString() => Kilometers.ToString("0.###");
}
