namespace System.Shared.Domain.Abstractions;

/// <summary>
/// Represents a base class for domain entities.
/// </summary>
/// <remarks>
/// Entities are objects that have a distinct identity that runs through time and different states.
/// Two entities are considered equal if they have the same identifier, regardless of their other properties.
/// This class provides a base implementation for identity-based equality.
/// </remarks>
/// <typeparam name="TId">The type of the entity's unique identifier.</typeparam>
public abstract class Entity<TId> : IEntity<TId>, IEquatable<Entity<TId>> where TId : notnull
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    public TId Id { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    protected Entity(TId id)
    {
        Id = id;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TId}"/> class.
    /// Required for ORM frameworks like Entity Framework Core.
    /// </summary>
    protected Entity()
    {
        // Required for EF Core, which can set the Id property via reflection.
        Id = default!;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != GetType())
        {
            return false;
        }

        return obj is Entity<TId> other && Id.Equals(other.Id);
    }

    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the other parameter; otherwise, false.</returns>
    public bool Equals(Entity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }
        
        return Id.Equals(other.Id);
    }
    
    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Entity{TId}"/> are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if the two instances are equal; otherwise, false.</returns>
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="Entity{TId}"/> are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if the two instances are not equal; otherwise, false.</returns>
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right)
    {
        return !(left == right);
    }
}