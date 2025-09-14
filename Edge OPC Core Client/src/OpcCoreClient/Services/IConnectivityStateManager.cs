namespace System.Edge.OpcCoreClient.Services;

/// <summary>
/// Defines the contract for the service that manages and tracks the client's network connectivity state.
/// This service acts as the central source of truth for whether the client should be sending data to the cloud
/// or buffering it locally. It implements the State Machine pattern as described in the SDS.
/// </summary>
public interface IConnectivityStateManager
{
    /// <summary>
    /// Gets the current connectivity state of the client.
    /// </summary>
    ConnectivityState CurrentState { get; }

    /// <summary>
    /// Gets an observable sequence that notifies subscribers of state changes.
    /// This allows other services to react to connectivity changes without polling.
    /// </summary>
    IObservable<ConnectivityState> StateChanges { get; }

    /// <summary>
    /// Triggers a transition to a new connectivity state.
    /// Implementations should ensure this is a thread-safe operation and that valid state transitions are enforced.
    /// </summary>
    /// <param name="newState">The new state to transition to.</param>
    void TransitionTo(ConnectivityState newState);

    /// <summary>
    /// Triggers a transition to the Offline state, typically due to a detected connection failure.
    /// </summary>
    /// <param name="reason">An optional reason for the disconnection.</param>
    void GoOffline(string? reason = null);

    /// <summary>
    /// Triggers a transition to the Online state, typically after a successful connection and buffer recovery.
    /// </summary>
    void GoOnline();
}