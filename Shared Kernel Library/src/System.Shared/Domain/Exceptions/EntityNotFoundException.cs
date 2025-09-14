using System.Runtime.Serialization;

namespace System.Shared.Domain.Exceptions;

/// <summary>
/// The exception that is thrown when a requested domain entity is not found.
/// </summary>
[Serializable]
public sealed class EntityNotFoundException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class.
    /// </summary>
    public EntityNotFoundException()
        : base("The requested entity was not found.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public EntityNotFoundException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public EntityNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with a specific entity type and ID.
    /// </summary>
    /// <param name="entityType">The type of the entity that was not found.</param>
    /// <param name="id">The identifier of the entity that was not found.</param>
    public EntityNotFoundException(Type entityType, object id)
        : base($"Entity of type '{entityType.Name}' with ID '{id}' was not found.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityNotFoundException"/> class with serialized data.
    /// </summary>
    /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
    /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
    private EntityNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}