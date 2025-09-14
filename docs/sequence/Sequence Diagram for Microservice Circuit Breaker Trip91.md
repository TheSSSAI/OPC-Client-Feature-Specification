sequenceDiagram
    participant "Query Service" as QueryService
    participant "Asset Service" as AssetService

    activate AssetService
    QueryService->>AssetService: 1. gRPC Call: GetAssetDetails(request) [Loop 1..N]
    AssetService-->>QueryService: Error: gRPC StatusCode.DeadlineExceeded
    QueryService->>QueryService: 2. Internal: Failure threshold (e.g., 5) breached. Circuit Breaker transitions to OPEN state.
    activate QueryService
    QueryService->>QueryService: 3. Application attempts gRPC call: GetAssetDetails(request)
    QueryService-->>QueryService: Immediate Exception: Polly.CircuitBreaker.BrokenCircuitException
    QueryService->>QueryService: 4. Internal: Open duration (e.g., 30s) expired. Circuit Breaker transitions to HALF-OPEN state.
    QueryService->>AssetService: 5. gRPC Trial Call: GetAssetDetails(request) [Half-Open State]
    AssetService-->>QueryService: Success: AssetDetailsResponse
    QueryService->>QueryService: 6. Internal: Trial call succeeded. Circuit Breaker transitions to CLOSED state.

    note over QueryService: Circuit Breaker is configured via application settings (e.g., FailureThreshold=5, DurationOfBreak...
    note over AssetService: The Asset Service is experiencing a transient issue like a pod restart, high load, or temporary l...

    deactivate QueryService
    deactivate AssetService
