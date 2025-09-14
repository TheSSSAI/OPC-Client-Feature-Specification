using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using AssetTopology.Application.Exceptions;

namespace AssetTopology.API.Middleware;

/// <summary>
/// Handles exceptions globally for the application, converting them into standardized RFC 7807 ProblemDetails responses.
/// </summary>
internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        _logger.LogError(
            exception,
            "Exception occurred: {Message}. TraceId: {TraceId}",
            exception.Message,
            traceId);

        var (statusCode, title, detail) = MapException(exception);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions.Add("traceId", traceId);

        if (exception is ValidationAppException validationException)
        {
            problemDetails.Extensions.Add("errors", validationException.Errors);
        }

        httpContext.Response.StatusCode = statusCode;
        
        // Use System.Text.Json for serialization to avoid conflicts and ensure consistency
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, options, cancellationToken: cancellationToken);

        return true;
    }

    private static (int StatusCode, string Title, string Detail) MapException(Exception exception)
    {
        return exception switch
        {
            ValidationAppException => (StatusCodes.Status422UnprocessableEntity, "Validation Error", "One or more validation errors occurred."),
            AssetNotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            AssetTemplateNotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            DuplicateAssetException => (StatusCodes.Status409Conflict, "Conflict", exception.Message),
            DuplicateAssetTemplateException => (StatusCodes.Status409Conflict, "Conflict", exception.Message),
            OpcTagNotFoundException => (StatusCodes.Status404NotFound, "Not Found", exception.Message),
            CircularDependencyException => (StatusCodes.Status400BadRequest, "Bad Request", exception.Message),
            ForbiddenAccessException => (StatusCodes.Status403Forbidden, "Forbidden", "You do not have permission to perform this action."),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred. Please try again later or contact support.")
        };
    }
}