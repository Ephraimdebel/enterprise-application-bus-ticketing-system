namespace Rating.Domain.ValueObjects;

public sealed class Score
{
    public int Value { get; }

    public Score(int value)
    {
        if (value < 1 || value > 5)
            throw new InvalidOperationException(
                $"Score must be between 1 and 5. Provided value: {value}"
            );

        Value = value;
    }

    public override bool Equals(object? obj)
        => obj is Score other && Value == other.Value;

    public override int GetHashCode()
        => Value.GetHashCode();
}
