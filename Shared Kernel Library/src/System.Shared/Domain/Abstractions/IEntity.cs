namespace System.Shared.Domain.Abstractions;

/// <summary>
/// Defines the contract for a domain entity.
/// An entity is an object that is not defined by its attributes, but rather by its thread of continuity and identity.
/// </summary>
/// <typeparam name="TId">The type of the entity's unique identifier.</typeparam>
public interface IEntity<out TId> where TId : notnull
{
    /// <summary>
    /// Gets the unique identifier of the entity.
    /// </summary>
    TId Id { get; }
}