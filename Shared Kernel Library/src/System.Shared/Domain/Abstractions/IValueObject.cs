namespace System.Shared.Domain.Abstractions;

/// <summary>
/// A marker interface for a value object.
/// Value objects are objects that we care about for what they are, not who they are.
/// They are defined by their attributes and are typically immutable.
/// Equality is based on the values of their properties.
/// </summary>
public interface IValueObject
{
}