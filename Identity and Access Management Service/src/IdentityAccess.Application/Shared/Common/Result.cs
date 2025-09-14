using System.Diagnostics.CodeAnalysis;

namespace System.Services.IdentityAccess.Application.Shared.Common;

/// <summary>
/// Represents the result of an operation, which can either be successful or a failure.
/// </summary>
/// <typeparam name="TValue">The type of the value returned on success.</typeparam>
public class Result<TValue>
{
    private readonly TValue? _value;
    private readonly Error _error;

    /// <summary>
    /// Gets a value indicating whether the result represents a success.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failure.
    /// </summary>
    public bool IsFailure => !IsSuccess;
    
    /// <summary>
    /// Gets the value of the result if successful.
    /// </summary>
    public TValue? Value => _value;
    
    /// <summary>
    /// Gets the error of the result if it failed.
    /// </summary>
    public Error Error => IsFailure ? _error : throw new InvalidOperationException("Result is successful, cannot access Error.");

    private Result(TValue value)
    {
        IsSuccess = true;
        _value = value;
        _error = Error.None;
    }

    private Result(Error error)
    {
        IsSuccess = false;
        _value = default;
        _error = error;
    }
    
    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <param name="value">The value to be returned.</param>
    /// <returns>A new instance of a successful result.</returns>
    public static Result<TValue> Success(TValue value) => new(value);

    /// <summary>
    /// Creates a failure result.
    /// </summary>
    /// <param name="error">The error that occurred.</param>
    /// <returns>A new instance of a failed result.</returns>
    public static Result<TValue> Failure(Error error) => new(error);
}