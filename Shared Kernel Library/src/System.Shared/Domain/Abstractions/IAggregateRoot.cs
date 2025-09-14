namespace System.Shared.Domain.Abstractions;

/// <summary>
/// Represents a marker interface for an Aggregate Root in the domain model.
/// </summary>
/// <remarks>
/// An Aggregate Root is a specific type of Entity that acts as the entry point to a cluster of associated objects (an aggregate).
/// It is responsible for maintaining the consistency and invariants of the entire aggregate.
/// External objects are only allowed to hold references to the Aggregate Root, not to any of its internal entities.
/// This interface inherits from IEntity, ensuring all Aggregate Roots have a unique identity.
/// </remarks>
/// <typeparam name="TId">The type of the aggregate root's unique identifier.</typeparam>
public interface IAggregateRoot<TId> : IEntity<TId> where TId : notnull
{
}