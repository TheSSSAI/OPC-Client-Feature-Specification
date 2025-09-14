using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Opc.Ua;
using System.Threading.Channels;
using System.Edge.OpcCoreClient.Communication;
using System.Edge.OpcCoreClient.Configuration;
using System.Edge.OpcCoreClient.Models;
using System.Edge.OpcCoreClient.Services;

namespace System.Edge.OpcCoreClient.HostedServices
{
    /// <summary>
    /// A background service responsible for connecting to an OPC UA server,
    /// subscribing to a list of tags, and producing data points into an in-memory channel.
    /// It also manages the connection state and handles failover logic.
    /// </summary>
    public class OpcPollerHostedService : BackgroundService
    {
        private readonly ILogger<OpcPollerHostedService> _logger;
        private readonly OpcUaAdapter _opcUaAdapter;
        private readonly ChannelWriter<DataPoint> _channelWriter;
        private readonly IConnectivityStateManager _connectivityStateManager;
        private readonly OpcOptions _opcOptions;
        private readonly IHostApplicationLifetime _appLifetime;

        public OpcPollerHostedService(
            ILogger<OpcPollerHostedService> logger,
            OpcUaAdapter opcUaAdapter,
            ChannelWriter<DataPoint> channelWriter,
            IConnectivityStateManager connectivityStateManager,
            IOptions<OpcOptions> opcOptions,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _opcUaAdapter = opcUaAdapter ?? throw new ArgumentNullException(nameof(opcUaAdapter));
            _channelWriter = channelWriter ?? throw new ArgumentNullException(nameof(channelWriter));
            _connectivityStateManager = connectivityStateManager ?? throw new ArgumentNullException(nameof(connectivityStateManager));
            _opcOptions = opcOptions?.Value ?? throw new ArgumentNullException(nameof(opcOptions));
            _appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("OPC Poller Hosted Service is starting.");

            // Wait until the application is fully started before trying to connect.
            await WaitForApplicationStarted(stoppingToken);
            if (stoppingToken.IsCancellationRequested) return;

            _opcUaAdapter.DataChanged += OnDataChanged;
            _opcUaAdapter.ConnectionStatusChanged += OnConnectionStatusChanged;

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (!_opcUaAdapter.IsConnected)
                    {
                        _logger.LogInformation("Attempting to connect to OPC server...");
                        await _opcUaAdapter.ConnectAndSubscribeAsync(_opcOptions.TagList, stoppingToken);
                        _logger.LogInformation("Successfully connected and subscribed to OPC server.");
                    }

                    // The work is done via the event handler (OnDataChanged).
                    // This loop's primary purpose is to maintain the connection.
                    await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    // This is expected on shutdown.
                    _logger.LogInformation("OPC Poller Hosted Service is stopping as cancellation was requested.");
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unhandled error occurred in OPC Poller main loop. Retrying in 30 seconds.");
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                }
            }

            _logger.LogInformation("OPC Poller Hosted Service is shutting down.");
            await _opcUaAdapter.DisconnectAsync();
            _opcUaAdapter.DataChanged -= OnDataChanged;
            _opcUaAdapter.ConnectionStatusChanged -= OnConnectionStatusChanged;
            _channelWriter.TryComplete();
        }
        
        private async Task WaitForApplicationStarted(CancellationToken stoppingToken)
        {
            var tcs = new TaskCompletionSource();
            using var csharp = _appLifetime.ApplicationStarted.Register(() => tcs.SetResult());
            using var tokenReg = stoppingToken.Register(() => tcs.SetCanceled());
            await tcs.Task;
        }

        private void OnConnectionStatusChanged(object? sender, OpcConnectionStatusEventArgs e)
        {
            _logger.LogInformation("OPC connection status changed to {Status}. Reason: {Reason}", e.IsConnected, e.Reason);
            // Future enhancement: We could create a more granular state for OPC connectivity
            // separate from the cloud connectivity. For now, we log it.
            // For example: _connectivityStateManager.SetOpcState(e.IsConnected);
        }

        private async void OnDataChanged(object? sender, MonitoredItemNotificationEventArgs e)
        {
            if (e.Notification == null || e.Notification.Value == null)
            {
                _logger.LogWarning("Received a null notification for item {DisplayName}.", e.MonitoredItem.DisplayName);
                return;
            }

            try
            {
                var dataValue = e.Notification.Value;

                var dataPoint = new DataPoint
                {
                    TagId = e.MonitoredItem.StartNodeId.ToString(),
                    Timestamp = dataValue.SourceTimestamp,
                    Value = dataValue.Value,
                    Quality = dataValue.StatusCode.Code
                };

                // Non-blocking write to the channel. If the channel is full, this will wait.
                await _channelWriter.WriteAsync(dataPoint, _appLifetime.ApplicationStopping);

                _logger.LogTrace("Produced data point for tag {TagId}: Value={Value}, Quality={Quality}",
                    dataPoint.TagId, dataPoint.Value, dataPoint.Quality);
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Could not write to channel as application is shutting down.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process and produce data point for tag {Tag}.", e.MonitoredItem.DisplayName);
            }
        }
    }
}