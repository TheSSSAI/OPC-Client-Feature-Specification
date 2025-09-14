namespace System.Shared.Domain.Abstractions;

/// <summary>
/// A marker interface for a domain event.
/// Domain events are objects that represent something that has happened in the domain
/// that other parts of the system might be interested in.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Gets the UTC date and time when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredOnUtc { get; }
}