namespace System.Services.AssetTopology.Domain.Entities;

/// <summary>
/// Represents the base class for all domain entities in the system.
/// It provides a common identifier and encapsulates the core principles of a Domain-Driven Design (DDD) entity.
/// An entity is an object that is not defined by its attributes, but rather by a thread of continuity and its identity.
/// </summary>
/// <remarks>
/// This class is abstract and cannot be instantiated directly. It is intended to be inherited by specific domain entities
/// such as Asset, AssetTemplate, and OpcTag. The identity is managed via a globally unique identifier (Guid),
/// which is ideal for distributed systems and microservice architectures to prevent key collisions.
/// </remarks>
public abstract class BaseEntity
{
    /// <summary>
    /// Gets the unique identifier for this entity.
    /// </summary>
    /// <remarks>
    /// The setter is protected to ensure that the identity of an entity is immutable from outside the aggregate boundary
    /// once it has been created. This is a core principle of DDD to maintain consistency and integrity.
    /// Entity Framework Core is capable of setting this property during materialization from the database.
    /// </remarks>
    public Guid Id { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class.
    /// This constructor is used when creating a new entity that does not yet have an identity.
    /// A new Guid is generated automatically.
    /// </summary>
    protected BaseEntity()
    {
        Id = Guid.NewGuid();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseEntity"/> class with a specific identifier.
    /// This constructor is used by persistence mechanisms (like Entity Framework Core) to reconstitute an entity
    /// from the data store with its existing identity.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    protected BaseEntity(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// Two entities are considered equal if they have the same type and the same identifier.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (GetType() != other.GetType())
        {
            return false;
        }

        // Transient entities (Id is default) are not considered equal to any other entity.
        if (Id == Guid.Empty || other.Id == Guid.Empty)
        {
            return false;
        }

        return Id == other.Id;
    }

    /// <summary>
    /// Serves as the default hash function.
    /// Uses the entity's identifier for hashing.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        // The default implementation is sufficient if we use the Id property.
        // The XOR with the type's hash code ensures that entities of different types
        // but with the same Guid value do not produce the same hash code.
        return (GetType().ToString() + Id).GetHashCode();
    }

    public static bool operator ==(BaseEntity? a, BaseEntity? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(BaseEntity? a, BaseEntity? b)
    {
        return !(a == b);
    }
}