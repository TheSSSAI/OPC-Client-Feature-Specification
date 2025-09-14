using DataIngestionService.Application.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using System.Net.Sockets;

namespace DataIngestionService.Infrastructure.Persistence.Resilience;

/// <summary>
/// A static factory for creating Polly resilience policies for database interactions.
/// This centralizes the resilience strategy for handling transient database faults.
/// </summary>
public static class DatabaseResiliencePolicy
{
    /// <summary>
    /// Creates a composite resilience policy that combines retry and circuit breaker patterns.
    /// </summary>
    /// <param name="logger">The logger to use for policy events.</param>
    /// <param name="resilienceOptions">Configuration options for retry and circuit breaker policies.</param>
    /// <returns>An asynchronous Polly policy wrap.</returns>
    public static AsyncPolicyWrap CreatePolicy(ILogger logger, IOptions<DatabaseResilienceOptions> resilienceOptions)
    {
        var options = resilienceOptions.Value;

        // Policy for handling transient database exceptions with exponential backoff.
        AsyncRetryPolicy retryPolicy = Policy
            .Handle<NpgsqlException>(IsTransient)
            .Or<SocketException>()
            .WaitAndRetryAsync(
                retryCount: options.RetryCount,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(options.RetryBackoffPower, retryAttempt)),
                onRetry: (exception, timespan, retryAttempt, context) =>
                {
                    logger.LogWarning(exception,
                        "Database operation failed. Retrying in {Timespan}. Attempt {RetryAttempt}/{RetryCount}. CorrelationId: {CorrelationId}",
                        timespan, retryAttempt, options.RetryCount, context.CorrelationId);
                });

        // Policy for breaking the circuit after a number of consecutive failures.
        AsyncCircuitBreakerPolicy circuitBreakerPolicy = Policy
            .Handle<NpgsqlException>(IsTransient)
            .Or<SocketException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: options.CircuitBreakerExceptionsAllowed,
                durationOfBreak: TimeSpan.FromSeconds(options.CircuitBreakerDurationSeconds),
                onBreak: (exception, duration, context) =>
                {
                    logger.LogCritical(exception,
                        "Database circuit breaker opened for {Duration} seconds due to repeated failures. CorrelationId: {CorrelationId}",
                        duration.TotalSeconds, context.CorrelationId);
                },
                onReset: (context) =>
                {
                    logger.LogInformation(
                        "Database circuit breaker has been reset. CorrelationId: {CorrelationId}",
                        context.CorrelationId);
                },
                onHalfOpen: () =>
                {
                    logger.LogWarning("Database circuit breaker is now half-open. The next operation will test the connection.");
                });

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }

    /// <summary>
    /// Determines if an NpgsqlException is transient and suitable for retrying.
    /// This includes connection errors, timeouts, and other network-related issues.
    /// </summary>
    /// <param name="ex">The NpgsqlException to inspect.</param>
    /// <returns>True if the exception is considered transient; otherwise, false.</returns>
    private static bool IsTransient(NpgsqlException ex)
    {
        // Add more specific transient error codes as they are identified from production.
        // IsTransient flag usually covers most network related issues.
        return ex.IsTransient || ex.InnerException is SocketException;
    }
}