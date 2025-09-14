sequenceDiagram
    actor "Engineer's Web UI / CLI" as EngineersWebUICLI
    participant "API Gateway" as APIGateway
    participant "Asset & Topology Service" as AssetTopologyService
    participant "Asset Service (Background Worker)" as AssetServiceBackgroundWorker
    participant "PostgreSQL Database" as PostgreSQLDatabase
    participant "Audit Service" as AuditService
    participant "S3 Object Storage" as S3ObjectStorage

    activate APIGateway
    EngineersWebUICLI->>APIGateway: 1. 1. Uploads asset/tag configuration file
    APIGateway-->>EngineersWebUICLI: 4. Returns Job ID for status polling
    activate AssetTopologyService
    APIGateway->>AssetTopologyService: 2. 2. Forwards authenticated file upload request
    AssetTopologyService-->>APIGateway: 3. Acknowledges request and returns Job ID
    AssetTopologyService->>S3ObjectStorage: 2.1. 2.1. Temporarily stores uploaded file for processing
    AssetTopologyService->>PostgreSQLDatabase: 2.2. 2.2. Creates import job record
    AssetTopologyService->>AuditService: 2.3. 2.3. Logs import initiation event
    AssetTopologyService->>AssetServiceBackgroundWorker: 2.4. 2.4. Enqueues background job for processing
    activate AssetServiceBackgroundWorker
    AssetServiceBackgroundWorker->>AssetServiceBackgroundWorker: 5. 5. [Async] Begins processing job
    AssetServiceBackgroundWorker->>PostgreSQLDatabase: 5.1. 5.1. Updates job status to PROCESSING
    activate PostgreSQLDatabase
    AssetServiceBackgroundWorker->>PostgreSQLDatabase: 7. 7. BEGIN TRANSACTION
    PostgreSQLDatabase-->>AssetServiceBackgroundWorker: 9. COMMIT TRANSACTION (or ROLLBACK)
    AssetServiceBackgroundWorker->>PostgreSQLDatabase: 8. 8. [Loop] Executes bulk insert/update operations for valid rows
    AssetServiceBackgroundWorker->>S3ObjectStorage: 11. 11. Uploads summary report
    AssetServiceBackgroundWorker->>PostgreSQLDatabase: 12. 12. Updates job record with final status and report URL
    EngineersWebUICLI->>APIGateway: 14. 14. [Polls] Requests job status
    APIGateway-->>EngineersWebUICLI: 18. Returns job status and report URL to Engineer
    APIGateway->>AssetTopologyService: 15. 15. Forwards status request
    AssetTopologyService-->>APIGateway: 17. Returns current job status
    AssetTopologyService->>PostgreSQLDatabase: 16. 16. Fetches job status from database

    note over AssetServiceBackgroundWorker: Business Rule Enforcement (REQ-DM-003): File parsing includes structural checks (headers), data t...
    note over PostgreSQLDatabase: Transactionality & Rollback: All database modifications are performed within a single transaction...
    note over EngineersWebUICLI: Validation & Sign-off (REQ-TRN-002): The final summary report provides the necessary details for ...

    deactivate PostgreSQLDatabase
    deactivate AssetServiceBackgroundWorker
    deactivate AssetTopologyService
    deactivate APIGateway
