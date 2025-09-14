using Grpc.Core;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Edge.OpcCoreClient.Models;
using System.Edge.OpcCoreClient.Services;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

// Assuming the gRPC client is generated from a .proto file in a shared library
// and available through a namespace like this.
using Ingestion; 

namespace System.Edge.OpcCoreClient.Communication
{
    /// <summary>
    /// Defines the contract for a service that sends data streams to the cloud via gRPC.
    /// </summary>
    public interface IGrpcDataSender
    {
        /// <summary>
        /// Establishes a client-streaming gRPC call and sends the provided data stream.
        /// Handles connection resilience and state management.
        /// </summary>
        /// <param name="dataStream">The asynchronous stream of data points to send.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendDataStreamAsync(IAsyncEnumerable<DataPoint> dataStream, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Encapsulates the logic for establishing and managing the client-streaming gRPC call to the Data Ingestion Service.
    /// Implements resilience policies and coordinates with the connectivity state manager.
    /// </summary>
    public class GrpcDataSender : IGrpcDataSender
    {
        private readonly IngestionService.IngestionServiceClient _grpcClient;
        private readonly IConnectivityStateManager _stateManager;
        private readonly ILogger<GrpcDataSender> _logger;
        private readonly AsyncRetryPolicy _retryPolicy;

        public GrpcDataSender(
            IngestionService.IngestionServiceClient grpcClient,
            IConnectivityStateManager stateManager,
            ILogger<GrpcDataSender> logger)
        {
            _grpcClient = grpcClient ?? throw new ArgumentNullException(nameof(grpcClient));
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _retryPolicy = Policy
                .Handle<RpcException>(ex => 
                    ex.StatusCode == StatusCode.Unavailable || 
                    ex.StatusCode == StatusCode.Internal ||
                    ex.StatusCode == StatusCode.Cancelled)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning(exception, "gRPC call failed. Retrying in {timeSpan}. Attempt {retryCount}/5.", timeSpan, retryCount);
                    });
        }

        /// <inheritdoc/>
        public async Task SendDataStreamAsync(IAsyncEnumerable<DataPoint> dataStream, CancellationToken cancellationToken)
        {
            if (_stateManager.CurrentState != ConnectivityState.Online)
            {
                _logger.LogInformation("SendDataStreamAsync called but current state is {state}. Aborting send.", _stateManager.CurrentState);
                return;
            }

            _logger.LogInformation("Attempting to establish gRPC data stream to cloud.");

            try
            {
                await _retryPolicy.ExecuteAsync(async (token) =>
                {
                    await ProcessStream(dataStream, token);
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "gRPC stream failed after all retry attempts. Transitioning to OFFLINE state.");
                _stateManager.TransitionTo(ConnectivityState.Offline);
            }
        }

        private async Task ProcessStream(IAsyncEnumerable<DataPoint> dataStream, CancellationToken cancellationToken)
        {
            using var call = _grpcClient.StreamData(cancellationToken: cancellationToken);
            int pointsSent = 0;
            
            try
            {
                await foreach (var dataPoint in dataStream.WithCancellation(cancellationToken))
                {
                    var dataPointRequest = new DataPointRequest
                    {
                        TagId = dataPoint.TagId,
                        Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(dataPoint.Timestamp),
                        Quality = dataPoint.Quality,
                        Value = MapValueToAny(dataPoint.Value)
                    };
                    
                    await call.RequestStream.WriteAsync(dataPointRequest, cancellationToken);
                    pointsSent++;
                }

                _logger.LogInformation("Data stream enumeration complete. Sent {count} data points. Completing gRPC stream.", pointsSent);
                await call.RequestStream.CompleteAsync();

                // Wait for the server's final acknowledgement
                var response = await call.ResponseAsync;
                _logger.LogInformation("gRPC stream completed successfully. Server acknowledged {count} points.", response?.PointsReceived ?? 0);
            }
            catch (RpcException ex)
            {
                _logger.LogError(ex, "An RPC error occurred during data streaming. Status: {statusCode}. The stream will be retried if possible.", ex.StatusCode);
                // Propagate the exception to be handled by the Polly policy.
                throw; 
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("gRPC data stream operation was cancelled.");
                // Don't transition to offline if it was a deliberate cancellation.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred during data streaming. The stream will be retried if possible.");
                 // Propagate the exception to be handled by the Polly policy.
                throw;
            }
        }
        
        private Google.Protobuf.WellKnownTypes.Any MapValueToAny(object value)
        {
            // In a real implementation, this would handle various types (double, int, bool, string)
            // and pack them into the appropriate protobuf message type before wrapping in Any.
            // For simplicity, we'll handle a few common types.
            switch (value)
            {
                case double d:
                    return Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.DoubleValue { Value = d });
                case float f:
                     return Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.FloatValue { Value = f });
                case int i:
                    return Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.Int32Value { Value = i });
                case long l:
                    return Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.Int64Value { Value = l });
                case bool b:
                    return Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.BoolValue { Value = b });
                case string s:
                    return Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.StringValue { Value = s });
                default:
                    // Fallback to string representation for unknown types
                    _logger.LogWarning("Unsupported data type for gRPC serialization: {type}. Converting to string.", value?.GetType().Name ?? "null");
                    return Google.Protobuf.WellKnownTypes.Any.Pack(new Google.Protobuf.WellKnownTypes.StringValue { Value = value?.ToString() ?? string.Empty });
            }
        }
    }
}