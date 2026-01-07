namespace Rating.Domain.ValueObjects
{
    public readonly struct Score
    {
        public int Value { get; }

        public Score(int value)
        {
            if (value < 1 || value > 5)
                throw new ArgumentOutOfRangeException(nameof(value), "Score must be between 1 and 5");
            Value = value;
        }
    }
}
