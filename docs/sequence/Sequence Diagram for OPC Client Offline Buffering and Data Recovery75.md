sequenceDiagram
    participant "Network Boundary" as NetworkBoundary
    actor "OPC Core Client" as OPCCoreClient
    participant "Data Ingestion Service" as DataIngestionService
    participant "Device Management Service" as DeviceManagementService

    activate DataIngestionService
    OPCCoreClient->>DataIngestionService: 1. 1. [Normal Operation] StreamRealTimeData()
    DataIngestionService-->>OPCCoreClient: Continuous stream of data points
    NetworkBoundary->>OPCCoreClient: 2. 2. Network connection is lost
    activate OPCCoreClient
    OPCCoreClient->>OPCCoreClient: 3. 3. gRPC stream health check fails after exhausting retry policy
    OPCCoreClient-->>OPCCoreClient: ConnectionFailureException
    OPCCoreClient->>OPCCoreClient: 4. 4. transitionToOfflineState()
    OPCCoreClient->>OPCCoreClient: 4.1. 4.1. [Loop] For each new data point from OPC Server:
    OPCCoreClient->>OPCCoreClient: 4.2. 4.2. [Loop] Periodically attemptReconnection()
    NetworkBoundary->>OPCCoreClient: 5. 5. Network connection is restored
    OPCCoreClient->>DataIngestionService: 6. 6. Reconnection attempt succeeds
    DataIngestionService-->>OPCCoreClient: OK
    OPCCoreClient->>OPCCoreClient: 7. 7. transitionToRecoveryState()
    OPCCoreClient->>DataIngestionService: 8. 8. StreamBufferedData()
    DataIngestionService-->>OPCCoreClient: Stream of batch acknowledgements
    OPCCoreClient->>OPCCoreClient: 8.1. 8.1. [Loop] While buffer is not empty:
    OPCCoreClient->>OPCCoreClient: 9. 9. transitionToOnlineState()
    activate DeviceManagementService
    OPCCoreClient->>DeviceManagementService: 10. 10. PublishStatus('Online')
    OPCCoreClient->>DataIngestionService: 11. 11. Resume StreamRealTimeData()
    DataIngestionService-->>OPCCoreClient: Continuous stream of data points

    note over OPCCoreClient: The on-disk buffer is a circular buffer. If the outage is longer than the buffer's capacity, the ...
    note over OPCCoreClient: The client state machine (ONLINE -> OFFLINE -> RECOVERY -> ONLINE) is central to this flow and mu...

    deactivate DeviceManagementService
    deactivate OPCCoreClient
    deactivate DataIngestionService
