namespace System.Edge.OpcCoreClient.Services;

/// <summary>
/// Represents the operational states of the client's cloud connection.
/// This state is central to implementing the autonomous operation and store-and-forward logic.
/// See sequence diagram #75 for state transitions.
/// </summary>
public enum ConnectivityState
{
    /// <summary>
    /// The client is connected to the cloud and is sending data in real-time.
    /// Data from OPC servers is directly dispatched to the cloud.
    /// </summary>
    Online,

    /// <summary>
    /// The client is disconnected from the cloud.
    /// Data from OPC servers is being written to the persistent local buffer.
    /// </summary>
    Offline,

    /// <summary>
    /// The client has re-established a connection to the cloud.
    /// It is actively sending data from the local buffer and is not yet processing new real-time data.
    /// Once the buffer is empty, it will transition to the Online state.
    /// </summary>
    Recovery
}