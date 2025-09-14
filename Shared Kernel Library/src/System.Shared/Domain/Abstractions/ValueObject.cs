namespace System.Shared.Domain.Abstractions;

/// <summary>
/// Represents a base class for value objects in the domain model.
/// </summary>
/// <remarks>
/// Value Objects are objects that are defined by their properties, not by their identity.
/// Two Value Objects are considered equal if all their constituent properties are equal.
/// They should be treated as immutable.
/// </remarks>
public abstract class ValueObject : IValueObject, IEquatable<ValueObject>
{
    /// <summary>
    /// When overridden in a derived class, gets the components that define the value object's identity.
    /// </summary>
    /// <returns>An enumerable of objects that are used for equality comparison.</returns>
    protected abstract IEnumerable<object> GetEqualityComponents();

    /// <summary>
    /// Determines whether the specified object is equal to the current value object.
    /// </summary>
    /// <param name="obj">The object to compare with the current value object.</param>
    /// <returns>true if the specified object is equal to the current value object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        return Equals((ValueObject)obj);
    }

    /// <summary>
    /// Indicates whether the current value object is equal to another value object of the same type.
    /// </summary>
    /// <param name="other">A value object to compare with this object.</param>
    /// <returns>true if the current value object is equal to the other parameter; otherwise, false.</returns>
    public bool Equals(ValueObject? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <summary>
    /// Returns the hash code for this value object.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }
    
    /// <summary>
    /// Determines whether two specified instances of <see cref="ValueObject"/> are equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if the two instances are equal; otherwise, false.</returns>
    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified instances of <see cref="ValueObject"/> are not equal.
    /// </summary>
    /// <param name="left">The first object to compare.</param>
    /// <param name="right">The second object to compare.</param>
    /// <returns>true if the two instances are not equal; otherwise, false.</returns>
    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}