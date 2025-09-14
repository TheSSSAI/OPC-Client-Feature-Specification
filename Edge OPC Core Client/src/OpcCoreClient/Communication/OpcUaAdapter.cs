using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Opc.Ua;
using Opc.Ua.Client;
using Opc.Ua.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Edge.OpcCoreClient.Configuration;
using System.Edge.OpcCoreClient.Models;

namespace System.Edge.OpcCoreClient.Communication
{
    public interface IOpcAdapter
    {
        Task ConnectAsync(CancellationToken stoppingToken);
        IAsyncEnumerable<DataPoint> GetDataStream(CancellationToken stoppingToken);
        void Disconnect();
    }
    
    /// <summary>
    /// Adapter for managing a connection to an OPC UA server, including high-availability failover.
    /// Implements the logic for REQ-1-045 and sequence diagram #81.
    /// </summary>
    public class OpcUaAdapter : IOpcAdapter, IDisposable
    {
        private enum OpcConnectionState { Disconnected, ConnectedPrimary, ConnectedBackup, Connecting, FailingOver }
        
        private readonly ILogger<OpcUaAdapter> _logger;
        private readonly OpcOptions _options;
        private readonly Channel<DataPoint> _dataPointChannel;
        private readonly ApplicationConfiguration _appConfig;
        
        private Session _session;
        private Subscription _subscription;
        private Timer _healthCheckTimer;
        private OpcConnectionState _currentState = OpcConnectionState.Disconnected;
        private readonly object _lock = new object();

        public OpcUaAdapter(IOptions<OpcOptions> options, ILogger<OpcUaAdapter> logger)
        {
            _logger = logger;
            _options = options.Value;
            _dataPointChannel = Channel.CreateUnbounded<DataPoint>();
            
            _appConfig = CreateApplicationConfiguration();
            _appConfig.Validate(ApplicationType.Client).GetAwaiter().GetResult();
        }

        public async Task ConnectAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting OPC UA adapter connection process.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (_session == null || !_session.Connected)
                    {
                        await TryConnectAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception during OPC UA connection attempt. Retrying in 15 seconds.");
                }
                
                await Task.Delay(TimeSpan.FromSeconds(15), stoppingToken);
            }
        }

        private async Task TryConnectAsync(CancellationToken stoppingToken)
        {
            lock (_lock)
            {
                if (_currentState == OpcConnectionState.Connecting || _currentState == OpcConnectionState.FailingOver)
                {
                    _logger.LogDebug("Connection attempt is already in progress.");
                    return;
                }
                _currentState = OpcConnectionState.Connecting;
            }

            // Attempt to connect to Primary
            _logger.LogInformation("Attempting to connect to primary OPC UA server: {Endpoint}", _options.PrimaryServerEndpoint);
            var primarySession = await CreateSessionAsync(_options.PrimaryServerEndpoint, stoppingToken);

            if (primarySession != null && primarySession.Connected)
            {
                await OnConnectionSuccessAsync(primarySession, OpcConnectionState.ConnectedPrimary);
                return;
            }
            
            // If primary fails and backup is configured, attempt to connect to Backup
            if (!string.IsNullOrEmpty(_options.BackupServerEndpoint))
            {
                _logger.LogWarning("Failed to connect to primary server. Attempting to connect to backup OPC UA server: {Endpoint}", _options.BackupServerEndpoint);
                 var backupSession = await CreateSessionAsync(_options.BackupServerEndpoint, stoppingToken);
                if (backupSession != null && backupSession.Connected)
                {
                    await OnConnectionSuccessAsync(backupSession, OpcConnectionState.ConnectedBackup);
                    return;
                }
            }
            
            _logger.LogError("Failed to connect to both primary and backup OPC UA servers.");
            
            lock (_lock)
            {
                _currentState = OpcConnectionState.Disconnected;
            }
        }

        private async Task<Session> CreateSessionAsync(string endpointUrl, CancellationToken stoppingToken)
        {
            try
            {
                var endpointDescription = CoreClientUtils.SelectEndpoint(endpointUrl, useSecurity: true);
                var endpointConfiguration = EndpointConfiguration.Create(_appConfig);
                var configuredEndpoint = new ConfiguredEndpoint(null, endpointDescription, endpointConfiguration);
                
                var session = await Session.Create(
                    _appConfig,
                    configuredEndpoint,
                    updateBeforeConnect: false,
                    checkDomain: false,
                    sessionName: _appConfig.ApplicationName,
                    sessionTimeout: 60000,
                    identity: GetUserIdentity(),
                    preferredLocales: null
                );
                
                session.KeepAlive += Session_KeepAlive;
                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create session with endpoint {EndpointUrl}", endpointUrl);
                return null;
            }
        }
        
        private async Task OnConnectionSuccessAsync(Session session, OpcConnectionState newState)
        {
            _logger.LogInformation("Successfully connected to OPC UA server. New state: {NewState}", newState);
            _session = session;
            
            // Create subscription
            _subscription = new Subscription(_session.DefaultSubscription)
            {
                PublishingInterval = 1000,
                KeepAliveCount = 10,
                LifetimeCount = 100,
                MaxNotificationsPerPublish = 1000,
                Priority = 100,
                TimestampsToReturn = TimestampsToReturn.Both
            };
            
            _session.AddSubscription(_subscription);
            await _subscription.CreateAsync();
            
            // Create monitored items
            var itemsToMonitor = _options.TagList.Select(tag => new MonitoredItem(_subscription.DefaultItem)
            {
                DisplayName = tag,
                StartNodeId = new NodeId(tag)
            }).ToList();
            
            itemsToMonitor.ForEach(item => item.Notification += MonitoredItem_Notification);
            _subscription.AddItems(itemsToMonitor);
            await _subscription.ApplyChangesAsync();
            
            lock(_lock)
            {
                _currentState = newState;
            }

            // If connected to backup, start health checking the primary
            if (newState == OpcConnectionState.ConnectedBackup)
            {
                _healthCheckTimer = new Timer(async _ => await CheckPrimaryServerHealth(), null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
            }
        }

        private async Task CheckPrimaryServerHealth()
        {
             _logger.LogDebug("Performing health check on primary OPC UA server.");
             var primarySession = await CreateSessionAsync(_options.PrimaryServerEndpoint, CancellationToken.None);
             if (primarySession != null && primarySession.Connected)
             {
                 _logger.LogInformation("Primary OPC UA server has recovered. Initiating failback.");
                 _healthCheckTimer?.Dispose();
                 Disconnect(); // This will trigger the reconnect logic which prioritizes the primary
                 primarySession.Close();
             }
        }
        
        private void MonitoredItem_Notification(MonitoredItem monitoredItem, MonitoredItemNotificationEventArgs e)
        {
            try
            {
                if (e.NotificationValue is MonitoredItemNotification notification)
                {
                    var dataValue = notification.Value;
                    var dataPoint = new DataPoint(
                        monitoredItem.StartNodeId.ToString(),
                        dataValue.SourceTimestamp.ToUniversalTime(),
                        dataValue.Value,
                        dataValue.StatusCode.Code
                    );
                    
                    if (!_dataPointChannel.Writer.TryWrite(dataPoint))
                    {
                        _logger.LogWarning("Failed to write DataPoint to channel for TagId {TagId}. Channel may be full.", dataPoint.TagId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing OPC UA notification for item {DisplayName}", monitoredItem.DisplayName);
            }
        }
        
        public IAsyncEnumerable<DataPoint> GetDataStream(CancellationToken stoppingToken)
        {
            return _dataPointChannel.Reader.ReadAllAsync(stoppingToken);
        }

        public void Disconnect()
        {
            _logger.LogInformation("Disconnecting from OPC UA server.");
            _healthCheckTimer?.Dispose();
            _healthCheckTimer = null;
            
            _session?.Close();
            _session = null;
            
            lock(_lock)
            {
                _currentState = OpcConnectionState.Disconnected;
            }
        }
        
        private void Session_KeepAlive(ISession session, KeepAliveEventArgs e)
        {
            if (ServiceResult.IsBad(e.Status))
            {
                _logger.LogWarning("Session KeepAlive failed with status {Status}. Server state: {ServerState}. Initiating reconnect.", e.Status, e.CurrentState);
                Disconnect();
            }
        }
        
        private ApplicationConfiguration CreateApplicationConfiguration()
        {
            var config = new ApplicationConfiguration()
            {
                ApplicationName = "Edge OPC Core Client",
                ApplicationUri = Utils.Format(Guid.NewGuid().ToString()),
                ApplicationType = ApplicationType.Client,
                SecurityConfiguration = new SecurityConfiguration
                {
                    ApplicationCertificate = new CertificateIdentifier { StoreType = "Directory", StorePath = "pki/own", SubjectName = "Edge OPC Core Client" },
                    TrustedIssuerCertificates = new CertificateTrustList { StoreType = "Directory", StorePath = "pki/issuer" },
                    TrustedPeerCertificates = new CertificateTrustList { StoreType = "Directory", StorePath = "pki/trusted" },
                    RejectedCertificateStore = new CertificateTrustList { StoreType = "Directory", StorePath = "pki/rejected" },
                    AutoAcceptUntrustedCertificates = false,
                    AddAppCertToTrustedStore = true
                },
                TransportConfigurations = new TransportConfigurationCollection(),
                TransportQuotas = new TransportQuotas { OperationTimeout = 15000 },
                ClientConfiguration = new ClientConfiguration { DefaultSessionTimeout = 60000 },
                TraceConfiguration = new TraceConfiguration()
            };
            config.CertificateValidator.CertificateValidation += (s, e) => {
                if(e.Error.StatusCode == StatusCodes.BadCertificateUntrusted)
                {
                     e.Accept = true; // In a real app, this would involve a prompt or configuration check
                     _logger.LogWarning("Accepting untrusted server certificate: {SubjectName}", e.Certificate.SubjectName.Name);
                }
            };
            return config;
        }

        private IUserIdentity GetUserIdentity()
        {
            // This would be populated from options for username/password auth
            return new UserIdentity(); 
        }

        public void Dispose()
        {
            Disconnect();
            _dataPointChannel.Writer.TryComplete();
        }
    }
}