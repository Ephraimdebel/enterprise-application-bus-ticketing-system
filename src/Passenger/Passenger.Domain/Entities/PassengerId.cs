namespace Passenger.Domain.Entities;

/// <summary>
/// Strongly typed identifier for Passenger aggregate.
/// </summary>
public readonly record struct PassengerId(Guid Value)
{
    public static PassengerId New() => new(Guid.NewGuid());

    public static PassengerId FromGuid(Guid value)
        => value == Guid.Empty ? throw new ArgumentException("PassengerId cannot be empty.", nameof(value)) : new PassengerId(value);

    public static bool TryParse(string? value, out PassengerId id)
    {
        id = default;
        if (string.IsNullOrWhiteSpace(value)) return false;
        if (!Guid.TryParse(value, out var guid)) return false;
        if (guid == Guid.Empty) return false;
        id = new PassengerId(guid);
        return true;
    }

    public override string ToString() => Value.ToString();
}
