using System.Threading.Channels;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Edge.OpcCoreClient.Models;
using System.Edge.OpcCoreClient.Persistence;
using System.Edge.OpcCoreClient.Services;
using System.Edge.OpcCoreClient.Communication;

namespace System.Edge.OpcCoreClient.HostedServices
{
    /// <summary>
    /// A background service that acts as the primary data consumer. It reads data points from an in-memory channel
    /// and dispatches them to either the cloud via gRPC or a local persistent buffer based on the current connectivity state.
    /// This service is central to the client's autonomous operation and data resilience strategy.
    /// </summary>
    public sealed class CloudDispatcherHostedService : BackgroundService
    {
        private const int MaxBatchSize = 1000;
        private static readonly TimeSpan BatchReadTimeout = TimeSpan.FromSeconds(5);

        private readonly ILogger<CloudDispatcherHostedService> _logger;
        private readonly IConnectivityStateManager _stateManager;
        private readonly ChannelReader<DataPoint> _channelReader;
        private readonly IGrpcDataSender _grpcDataSender;
        private readonly IDataBuffer _dataBuffer;

        public CloudDispatcherHostedService(
            ILogger<CloudDispatcherHostedService> logger,
            IConnectivityStateManager stateManager,
            ChannelReader<DataPoint> channelReader,
            IGrpcDataSender grpcDataSender,
            IDataBuffer dataBuffer)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
            _channelReader = channelReader ?? throw new ArgumentNullException(nameof(channelReader));
            _grpcDataSender = grpcDataSender ?? throw new ArgumentNullException(nameof(grpcDataSender));
            _dataBuffer = dataBuffer ?? throw new ArgumentNullException(nameof(dataBuffer));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Cloud Dispatcher Hosted Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var currentState = _stateManager.CurrentState;
                    if (currentState == ConnectivityState.Recovery)
                    {
                        await DrainBufferAsync(stoppingToken);
                    }
                    else
                    {
                        await ProcessChannelDataAsync(stoppingToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    // This is expected during shutdown, no need to log an error.
                    _logger.LogInformation("Cloud Dispatcher Hosted Service is stopping due to cancellation request.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "An unhandled exception occurred in the Cloud Dispatcher main loop. The service will restart in 10 seconds.");
                    await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                }
            }

            _logger.LogInformation("Cloud Dispatcher Hosted Service has stopped.");
        }

        /// <summary>
        /// Processes data from the in-memory channel, dispatching it based on the current connectivity state (Online or Offline).
        /// This method is responsible for transitioning the state from Online to Offline upon a communication failure.
        /// </summary>
        private async Task ProcessChannelDataAsync(CancellationToken stoppingToken)
        {
            // Wait for data to become available or until a timeout, which allows us to periodically check the state.
            using var timeoutCts = new CancellationTokenSource(BatchReadTimeout);
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, timeoutCts.Token);

            try
            {
                await _channelReader.WaitToReadAsync(linkedCts.Token);
            }
            catch (OperationCanceledException)
            {
                // This is expected if the timeout is hit or the service is stopping.
                // It's a signal to re-evaluate the state in the main loop.
                return;
            }

            var batch = new List<DataPoint>(MaxBatchSize);
            while (batch.Count < MaxBatchSize && _channelReader.TryRead(out var dataPoint))
            {
                batch.Add(dataPoint);
            }

            if (batch.Count == 0)
            {
                return;
            }

            var currentState = _stateManager.CurrentState;
            _logger.LogDebug("Processing a batch of {BatchSize} data points in {State} state.", batch.Count, currentState);

            if (currentState == ConnectivityState.Online)
            {
                await TrySendDataToCloudAsync(batch, stoppingToken);
            }
            else // Offline state
            {
                await BufferDataAsync(batch, stoppingToken);
            }
        }

        /// <summary>
        /// Drains the persistent on-disk buffer by sending its contents to the cloud.
        /// If sending fails, it transitions the state back to Offline to preserve the remaining buffer.
        /// If successful, it transitions the state to Online.
        /// </summary>
        private async Task DrainBufferAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Entering Recovery state. Starting to drain persistent data buffer.");

            while (!stoppingToken.IsCancellationRequested && _stateManager.CurrentState == ConnectivityState.Recovery)
            {
                var batch = await _dataBuffer.DequeueBatchAsync(MaxBatchSize);
                if (batch is null || !batch.Any())
                {
                    _logger.LogInformation("Data buffer is empty. Transitioning to Online state.");
                    _stateManager.TransitionTo(ConnectivityState.Online);
                    return; // Buffer is drained
                }

                _logger.LogInformation("Draining a batch of {BatchSize} data points from buffer.", batch.Count());
                try
                {
                    await _grpcDataSender.SendBatchAsync(batch, stoppingToken);
                }
                catch (RpcException ex)
                {
                    _logger.LogError(ex, "Failed to send buffered data to the cloud during recovery. Transitioning back to Offline state.");
                    _stateManager.TransitionTo(ConnectivityState.Offline);
                    // Re-enqueue the failed batch to preserve it.
                    await _dataBuffer.EnqueueBatchAsync(batch, stoppingToken); 
                    return; // Exit recovery
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Unhandled exception while draining buffer. Transitioning back to Offline to prevent data loss.");
                    _stateManager.TransitionTo(ConnectivityState.Offline);
                    await _dataBuffer.EnqueueBatchAsync(batch, stoppingToken);
                    return; // Exit recovery
                }
            }

            _logger.LogInformation("Exited buffer drain loop. Current state: {State}", _stateManager.CurrentState);
        }

        /// <summary>
        /// Attempts to send a batch of data via gRPC. If it fails, it transitions the system state
        /// to Offline and then buffers the same batch to prevent data loss.
        /// </summary>
        private async Task TrySendDataToCloudAsync(List<DataPoint> batch, CancellationToken stoppingToken)
        {
            try
            {
                await _grpcDataSender.SendBatchAsync(batch, stoppingToken);
                _logger.LogDebug("Successfully sent a batch of {BatchSize} data points to the cloud.", batch.Count);
            }
            catch (RpcException ex)
            {
                _logger.LogWarning(ex, "Failed to send data to the cloud. Transitioning to Offline state. Data will be buffered.");
                _stateManager.TransitionTo(ConnectivityState.Offline);
                // Immediately buffer the batch that failed to send to prevent data loss.
                await BufferDataAsync(batch, stoppingToken);
            }
            catch (Exception ex)
            {
                 _logger.LogCritical(ex, "An unexpected error occurred while sending data to the cloud. Transitioning to Offline to prevent data loss.");
                _stateManager.TransitionTo(ConnectivityState.Offline);
                await BufferDataAsync(batch, stoppingToken);
            }
        }
        
        /// <summary>
        /// Enqueues a batch of data points to the persistent on-disk buffer.
        /// </summary>
        private async Task BufferDataAsync(IEnumerable<DataPoint> batch, CancellationToken stoppingToken)
        {
            try
            {
                await _dataBuffer.EnqueueBatchAsync(batch, stoppingToken);
                _logger.LogDebug("Successfully buffered a batch of {BatchSize} data points.", batch.Count());
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex, "Failed to write to persistent buffer. Potential data loss. Check disk space and permissions.");
                // In a real scenario, we might raise a critical system alert via MQTT here.
            }
        }
    }
}