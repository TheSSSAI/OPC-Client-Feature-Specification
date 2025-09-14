namespace System.Shared.Domain.Abstractions;

/// <summary>
/// Represents a base class for aggregate roots in a Domain-Driven Design (DDD) context.
/// An aggregate root is a specific type of entity that acts as the entry point and consistency boundary
/// for a cluster of related domain objects (an aggregate). It is responsible for maintaining the
/// aggregate's invariants and for publishing domain events that result from state changes.
/// </summary>
/// <typeparam name="TId">The type of the unique identifier for the aggregate root.</typeparam>
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId> where TId : notnull
{
    /// <summary>
    /// A collection of domain events that have occurred within the aggregate.
    /// These events are typically dispatched by the infrastructure layer after a transaction is successfully committed.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{TId}"/> class.
    /// </summary>
    /// <param name="id">The unique identifier of the aggregate root.</param>
    protected AggregateRoot(TId id) : base(id)
    {
    }
    
    /// <summary>
    /// A protected constructor for ORMs or serializers.
    /// </summary>
    protected AggregateRoot()
    {
    }

    /// <summary>
    /// Gets a read-only collection of the domain events that have occurred within this aggregate.
    /// This property allows external components (like an event dispatcher) to access the events
    /// without being able to modify the collection.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to the aggregate's collection of events.
    /// This method should be called by derived classes whenever a business rule is successfully enforced
    /// and results in a state change that is meaningful to other parts of the domain.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the aggregate's collection.
    /// This method is typically called by the infrastructure layer (e.g., a Unit of Work or event dispatcher)
    /// after the events have been successfully dispatched, ensuring they are not processed more than once.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}