using System.Security.Claims;
using DataIngestion.V1;
using DataIngestionService.Application.Models;
using DataIngestionService.Application.Services;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DataIngestionService.Api.Services;

/// <summary>
/// Implements the gRPC service for ingesting time-series data streams from OPC Core Clients.
/// This service acts as the primary, high-throughput entry point for all time-series data.
/// It is secured by a mutual TLS (mTLS) policy.
/// </summary>
[Authorize(Policy = "mTLS")]
public class IngestionGrpcService : Ingestion.IngestionBase
{
    private readonly DataPointBatchingService _batchingService;
    private readonly ILogger<IngestionGrpcService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="IngestionGrpcService"/> class.
    /// </summary>
    /// <param name="batchingService">The singleton service for batching data points in memory.</param>
    /// <param name="logger">The logger for capturing service-specific events.</param>
    public IngestionGrpcService(DataPointBatchingService batchingService, ILogger<IngestionGrpcService> logger)
    {
        _batchingService = batchingService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the client-side streaming RPC for ingesting data points.
    /// It reads from the client's stream, enriches each data point with a tenant ID derived
    /// from the client certificate, and adds it to the in-memory batching service.
    /// </summary>
    /// <param name="requestStream">The asynchronous stream of data points from the client.</param>
    /// <param name="context">The server call context, providing access to request metadata.</param>
    /// <returns>An acknowledgement message upon successful completion of the stream.</returns>
    public override async Task<IngestAck> StreamData(
        IAsyncStreamReader<DataPoint> requestStream,
        ServerCallContext context)
    {
        var clientPeer = context.Peer;
        var httpContext = context.GetHttpContext();
        
        // Critical Step: Extract TenantId from the authenticated client certificate's claims.
        // The "mTLS" authorization policy ensures the claim exists and is valid.
        var tenantIdClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (tenantIdClaim == null || !Guid.TryParse(tenantIdClaim.Value, out var tenantId))
        {
            _logger.LogCritical("mTLS authentication succeeded but TenantId claim is missing or invalid for client {ClientPeer}. Terminating stream.", clientPeer);
            context.Status = new Status(StatusCode.PermissionDenied, "Invalid or missing TenantId claim in client certificate.");
            return new IngestAck { Success = false, Message = "Permission Denied: Invalid client identity." };
        }
        
        _logger.LogInformation("Starting data stream for TenantId: {TenantId} from client {ClientPeer}", tenantId, clientPeer);

        long pointsReceived = 0;
        try
        {
            await foreach (var dataPoint in requestStream.ReadAllAsync(context.CancellationToken))
            {
                if (dataPoint == null)
                {
                    _logger.LogWarning("Received a null DataPoint message from client {ClientPeer} for TenantId: {TenantId}. Skipping.", clientPeer, tenantId);
                    continue;
                }

                // Enrich the incoming data with the validated TenantId.
                var enrichedDataPoint = new EnrichedDataPoint
                {
                    OpcTagId = Guid.Parse(dataPoint.OpcTagId), // Assuming OpcTagId is a valid GUID string
                    Timestamp = dataPoint.Timestamp.ToDateTime(),
                    Value = dataPoint.Value,
                    Quality = dataPoint.Quality,
                    TenantId = tenantId
                };

                // Add the enriched data point to the high-performance, in-memory buffer.
                _batchingService.AddDataPoint(enrichedDataPoint);
                pointsReceived++;
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
            // This is a normal and expected exception when the client gracefully closes the stream.
            _logger.LogInformation("Client {ClientPeer} for TenantId: {TenantId} cancelled the stream. Total points received: {PointsReceived}", clientPeer, tenantId, pointsReceived);
        }
        catch (IOException ioEx)
        {
            // This can happen if the underlying TCP connection is abruptly closed.
            _logger.LogWarning(ioEx, "I/O exception during streaming from client {ClientPeer} for TenantId: {TenantId}. Connection may have been lost. Total points received: {PointsReceived}", clientPeer, tenantId, pointsReceived);
            context.Status = new Status(StatusCode.Unavailable, "Network connection lost during stream.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled error occurred while processing stream from client {ClientPeer} for TenantId: {TenantId}. Terminating stream. Total points received: {PointsReceived}", clientPeer, tenantId, pointsReceived);
            context.Status = new Status(StatusCode.Internal, "An internal server error occurred.");
            return new IngestAck { Success = false, Message = "Internal Server Error." };
        }
        
        _logger.LogInformation("Data stream ended for TenantId: {TenantId} from client {ClientPeer}. Total points received: {PointsReceived}", tenantId, clientPeer, pointsReceived);

        return new IngestAck { Success = true, Message = $"Stream completed. {pointsReceived} points received." };
    }
}