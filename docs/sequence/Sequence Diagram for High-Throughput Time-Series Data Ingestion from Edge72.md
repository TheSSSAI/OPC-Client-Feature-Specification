sequenceDiagram
    actor "OPC Core Client" as OPCCoreClient
    participant "Data Ingestion Service" as DataIngestionService
    participant "TimescaleDB" as TimescaleDB

    activate DataIngestionService
    OPCCoreClient->>DataIngestionService: 1. EstablishStream(stream DataPointRequest)
    DataIngestionService-->>OPCCoreClient: StreamHandle
    OPCCoreClient->>DataIngestionService: 2. stream.WriteAsync(DataPointRequest)
    DataIngestionService->>DataIngestionService: 3. Process incoming data points in a concurrent pipeline
    DataIngestionService->>DataIngestionService: 3.1. Deserialize Protobuf message and validate schema
    DataIngestionService->>DataIngestionService: 3.2. Enrich with TenantID and AssetID from metadata cache
    DataIngestionService->>DataIngestionService: 3.3. Add enriched data point to in-memory batch buffer
    DataIngestionService->>TimescaleDB: 4. COPY tag_data (timestamp, tag_id, value, quality, tenant_id) FROM STDIN (BINARY)
    TimescaleDB-->>DataIngestionService: COPY N (number of rows)

    note over DataIngestionService: The enrichment step (3.2) should utilize a Redis cache for tag metadata (OpcTagId -> AssetId, Ten...
    note over DataIngestionService: The batch persistence in step 4 is a transactional 'Unit of Work'. The entire batch succeeds or f...

    deactivate DataIngestionService
