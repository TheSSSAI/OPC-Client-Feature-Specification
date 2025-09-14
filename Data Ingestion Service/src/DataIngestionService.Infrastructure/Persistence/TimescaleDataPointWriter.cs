using DataIngestionService.Application.Interfaces;
using DataIngestionService.Application.Models;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using System.Diagnostics;

namespace DataIngestionService.Infrastructure.Persistence;

/// <summary>
/// Implements IDataPointWriter for TimescaleDB using a high-performance bulk-write strategy.
/// This class uses the PostgreSQL COPY protocol via Npgsql's Binary Importer for maximum throughput.
/// </summary>
public sealed class TimescaleDataPointWriter : IDataPointWriter
{
    private const string CopyCommand = 
        "COPY tag_data_points (timestamp, opc_tag_id, value, quality, tenant_id) FROM STDIN (FORMAT BINARY)";

    private readonly NpgsqlDataSource _dataSource;
    private readonly IAsyncPolicy _resiliencePolicy;
    private readonly ILogger<TimescaleDataPointWriter> _logger;

    public TimescaleDataPointWriter(
        NpgsqlDataSource dataSource,
        IAsyncPolicy resiliencePolicy, 
        ILogger<TimescaleDataPointWriter> logger)
    {
        _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        _resiliencePolicy = resiliencePolicy ?? throw new ArgumentNullException(nameof(resiliencePolicy));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task WriteBatchAsync(IReadOnlyCollection<EnrichedDataPoint> dataPoints, CancellationToken cancellationToken)
    {
        if (dataPoints == null || dataPoints.Count == 0)
        {
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        _logger.LogInformation("Starting to write a batch of {DataPointCount} data points to TimescaleDB.", dataPoints.Count);

        try
        {
            // The entire database operation is wrapped in a resilience policy to handle transient errors.
            await _resiliencePolicy.ExecuteAsync(async ct =>
            {
                await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(ct);
                await using NpgsqlBinaryImporter importer = await connection.BeginBinaryImportAsync(CopyCommand, ct);
                
                foreach (var dataPoint in dataPoints)
                {
                    await importer.StartRowAsync(ct);
                    
                    // The order of writing columns must exactly match the COPY command.
                    await importer.WriteAsync(dataPoint.Timestamp, NpgsqlTypes.NpgsqlDbType.TimestampTz, ct);
                    await importer.WriteAsync(dataPoint.OpcTagId, NpgsqlTypes.NpgsqlDbType.Uuid, ct);
                    await importer.WriteAsync(dataPoint.Value, NpgsqlTypes.NpgsqlDbType.Double, ct);
                    await importer.WriteAsync(dataPoint.Quality, NpgsqlTypes.NpgsqlDbType.Integer, ct);
                    await importer.WriteAsync(dataPoint.TenantId, NpgsqlTypes.NpgsqlDbType.Uuid, ct);
                }

                await importer.CompleteAsync(ct);
                await importer.CloseAsync(ct);

            }, cancellationToken);
            
            stopwatch.Stop();
            _logger.LogInformation(
                "Successfully wrote a batch of {DataPointCount} data points to TimescaleDB in {ElapsedMilliseconds} ms.",
                dataPoints.Count, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            // If the resilience policy fails after all retries/circuit breaker trips, this exception will be caught.
            // The exception is re-thrown to be handled by the calling service (BatchFlushingService), which will log it
            // as a critical error but will not crash the background service.
            stopwatch.Stop();
            _logger.LogError(ex,
                "Failed to write a batch of {DataPointCount} data points to TimescaleDB after {ElapsedMilliseconds} ms. The batch will be discarded.",
                dataPoints.Count, stopwatch.ElapsedMilliseconds);
            throw; // Re-throw to allow the caller to know the operation ultimately failed.
        }
    }
}