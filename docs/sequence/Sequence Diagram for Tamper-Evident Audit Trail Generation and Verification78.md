sequenceDiagram
    actor "User via API Gateway" as UserviaAPIGateway
    participant "Asset Service" as AssetService
    participant "Audit Service" as AuditService
    participant "Audit DB" as AuditDB
    participant "Amazon QLDB Ledger" as AmazonQLDBLedger

    activate AssetService
    UserviaAPIGateway->>AssetService: 1. 1. POST /api/v1/tags/{id}/value (Request setpoint change)
    AssetService-->>UserviaAPIGateway: 8. 200 OK / 500 Internal Server Error
    AssetService->>AuditService: 2. 2. Call LogAction(LogActionRequest)
    AuditService-->>AssetService: 4. LogActionResponse
    activate AuditDB
    AuditService->>AuditDB: 3. 3. INSERT INTO audit_logs (...)
    AuditDB-->>AuditService: Success/Failure
    AssetService->>AssetService: 5. 5. Commit primary business logic (e.g., write to OPC server)
    activate AuditService
    AuditService->>AuditService: 6. 6. [Async] Scheduled background job triggers
    AuditService->>AuditDB: 7. 7. SELECT * FROM audit_logs WHERE qldb_tx_id IS NULL
    AuditDB-->>AuditService: List of un-anchored log entries
    AuditService->>AuditService: 8. 8. Calculate Merkle root hash for the batch
    activate AmazonQLDBLedger
    AuditService->>AmazonQLDBLedger: 9. 9. INSERT INTO audit_proofs ...
    AmazonQLDBLedger-->>AuditService: Transaction ID
    AuditService->>AuditDB: 10. 10. UPDATE audit_logs SET qldb_tx_id = ...
    AuditDB-->>AuditService: Success/Failure

    note over AuditDB: The 'audit_logs' table in PostgreSQL MUST be made immutable. This can be achieved via database tr...
    note over AuditService: The synchronous part of this flow (steps 2-4) is critical for compliance. The user's action canno...
    note over AuditService: The asynchronous batching process decouples the performance of the high-speed logger from the led...

    deactivate AmazonQLDBLedger
    deactivate AuditService
    deactivate AuditDB
    deactivate AssetService
