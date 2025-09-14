using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Edge.OpcCoreClient.Services;

namespace System.Edge.OpcCoreClient.Services
{
    /// <summary>
    /// Implements the state machine for managing the client's network connectivity to the cloud.
    /// This class provides a thread-safe way to track and transition between states.
    /// It is a critical component for enabling the autonomous operation and store-and-forward features.
    /// </summary>
    public sealed class ConnectivityStateManager : IConnectivityStateManager, IDisposable
    {
        private readonly ILogger<ConnectivityStateManager> _logger;
        private readonly object _stateLock = new object();
        private volatile ConnectivityState _currentState;
        private Timer _logStateTimer;

        public event Func<ConnectivityState, ConnectivityState, Task> StateChangedAsync;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectivityStateManager"/> class.
        /// </summary>
        /// <param name="logger">The logger for capturing state transition information.</param>
        public ConnectivityStateManager(ILogger<ConnectivityStateManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _currentState = ConnectivityState.Offline; // Start in a safe, disconnected state.
            _logStateTimer = new Timer(LogCurrentState, null, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
        }

        /// <inheritdoc />
        public ConnectivityState CurrentState
        {
            get
            {
                lock (_stateLock)
                {
                    return _currentState;
                }
            }
        }

        /// <inheritdoc />
        public async Task TransitionToAsync(ConnectivityState newState)
        {
            ConnectivityState oldState;
            bool stateChanged = false;

            lock (_stateLock)
            {
                oldState = _currentState;
                if (oldState != newState)
                {
                    _logger.LogInformation("Attempting state transition from {OldState} to {NewState}.", oldState, newState);
                    _currentState = newState;
                    stateChanged = true;
                }
            }

            if (stateChanged)
            {
                _logger.LogInformation("State transition successful from {OldState} to {NewState}.", oldState, newState);
                try
                {
                    var handler = StateChangedAsync;
                    if (handler != null)
                    {
                        await handler.Invoke(oldState, newState);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while invoking the StateChangedAsync event for transition from {OldState} to {NewState}.", oldState, newState);
                    // Revert state on handler failure to maintain consistency?
                    // For now, we log and proceed, assuming handlers are resilient.
                }
            }
            else
            {
                _logger.LogDebug("State transition to {NewState} ignored; already in that state.", newState);
            }
        }
        
        /// <summary>
        /// Periodically logs the current state to aid in diagnostics of long-running clients.
        /// </summary>
        private void LogCurrentState(object state)
        {
            _logger.LogDebug("Periodic Health Check: Current connectivity state is {CurrentState}.", CurrentState);
        }

        public void Dispose()
        {
            _logStateTimer?.Dispose();
        }
    }
}