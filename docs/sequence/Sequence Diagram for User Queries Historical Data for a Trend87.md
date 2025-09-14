sequenceDiagram
    participant "Frontend: Management Plane" as FrontendManagementPlane
    participant "API Gateway" as APIGateway
    participant "Query & Analytics Service" as QueryAnalyticsService
    participant "Time-Series Database" as TimeSeriesDatabase

    activate APIGateway
    FrontendManagementPlane->>APIGateway: 1. 1. GET /api/v1/query/historical?tagId=...&startTime=...&endTime=...&agg=avg
    APIGateway->>APIGateway: 2. 2. Validate JWT
    activate QueryAnalyticsService
    APIGateway->>QueryAnalyticsService: 3. 3. Proxy validated request
    QueryAnalyticsService->>QueryAnalyticsService: 4. 4. Validate & Authorize Request
    activate TimeSeriesDatabase
    QueryAnalyticsService->>TimeSeriesDatabase: 5. 5. Execute Optimized Time-Series Query
    TimeSeriesDatabase-->>QueryAnalyticsService: 6. Return Result Set
    QueryAnalyticsService->>QueryAnalyticsService: 7. 7. Map Data to DTO
    QueryAnalyticsService->>APIGateway: 8. 8. HTTP 200 OK with JSON Payload
    APIGateway->>FrontendManagementPlane: 9. 9. Proxy HTTP 200 OK Response
    FrontendManagementPlane->>FrontendManagementPlane: 10. 10. Render Data in Trend Chart

    note over TimeSeriesDatabase: REQ-NFR-001 Performance Target: The total time from step 5 to step 6 must be less than 1 second f...
    note over QueryAnalyticsService: REQ-FR-002 Aggregation: The SQL query in step 5 dynamically constructs the aggregation function (...

    deactivate TimeSeriesDatabase
    deactivate QueryAnalyticsService
    deactivate APIGateway
