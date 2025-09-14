namespace System.Services.IdentityAccess.Application.Shared.Common;

/// <summary>
/// Represents a specific domain error.
/// </summary>
public sealed class Error : IEquatable<Error>
{
    /// <summary>
    /// A code that uniquely identifies the type of error.
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// A description of the error.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Represents an error with no specific code or description.
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// Represents a null value error.
    /// </summary>
    public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null.");

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="description">The error description.</param>
    public Error(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Description == other.Description;
    }

    public override bool Equals(object? obj) => obj is Error other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Code, Description);

    public static implicit operator string(Error error) => error.Code;
}