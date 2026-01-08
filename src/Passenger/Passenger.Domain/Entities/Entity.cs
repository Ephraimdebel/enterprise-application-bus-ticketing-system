namespace Passenger.Domain.Entities;

public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    protected Entity(TId id) => Id = id;

    // For ORMs
    protected Entity() { }

    public TId Id { get; protected init; } = default!;

    public override bool Equals(object? obj) => obj is Entity<TId> other && Equals(other);

    public bool Equals(Entity<TId>? other)
        => other is not null &&
           GetType() == other.GetType() &&
           EqualityComparer<TId>.Default.Equals(Id, other.Id);

    public override int GetHashCode()
        => HashCode.Combine(GetType(), Id);

    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) => Equals(left, right);

    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !Equals(left, right);
}
