sequenceDiagram
    participant "Central Management Plane" as CentralManagementPlane
    participant "API Gateway" as APIGateway
    participant "Asset & Topology Service" as AssetTopologyService
    participant "Redis Cache" as RedisCache
    participant "PostgreSQL Database" as PostgreSQLDatabase

    activate APIGateway
    CentralManagementPlane->>APIGateway: 1. Requests asset hierarchy for the current user's tenant.
    APIGateway-->>CentralManagementPlane: Returns asset hierarchy in JSON format or an error response.
    APIGateway->>APIGateway: 2. Validates JWT and extracts tenantId from claims.
    activate AssetTopologyService
    APIGateway->>AssetTopologyService: 3. Forwards validated request to the Asset Service.
    AssetTopologyService-->>APIGateway: Proxies HTTP response from Asset Service.
    AssetTopologyService->>AssetTopologyService: 4. Generates tenant-specific cache key.
    activate RedisCache
    AssetTopologyService->>RedisCache: 5. Attempts to retrieve asset hierarchy from cache.
    RedisCache-->>AssetTopologyService: Returns serialized hierarchy string or nil.
    activate PostgreSQLDatabase
    AssetTopologyService->>PostgreSQLDatabase: 6. Queries all assets for the specified tenant.
    PostgreSQLDatabase-->>AssetTopologyService: Returns a flat list of asset records.
    AssetTopologyService->>AssetTopologyService: 7. Builds hierarchical tree structure from flat list.
    AssetTopologyService->>RedisCache: 8. Stores the newly built hierarchy in the cache with a TTL.
    RedisCache-->>AssetTopologyService: Returns OK.
    AssetTopologyService->>APIGateway: 9. Returns the asset hierarchy.
    APIGateway->>CentralManagementPlane: 10. Forwards the successful response.

    note over RedisCache: [ALT PATH: Cache Hit] If Redis returns a value at Step 5, the service deserializes the cached dat...
    note over RedisCache: [ALT PATH: Cache Unavailable] If the Redis request at Step 5 fails (e.g., timeout, connection err...
    note over PostgreSQLDatabase: [ERROR PATH: Database Unavailable] If the PostgreSQL query at Step 6 fails after retries, the ser...

    deactivate PostgreSQLDatabase
    deactivate RedisCache
    deactivate AssetTopologyService
    deactivate APIGateway
