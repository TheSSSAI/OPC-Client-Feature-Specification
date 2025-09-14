sequenceDiagram
    participant "AR Device (HoloLens)" as ARDeviceHoloLens
    participant "API Gateway (Kong)" as APIGatewayKong
    participant "Asset & Topology Service" as AssetTopologyService
    participant "Query & Analytics Service" as QueryAnalyticsService

    ARDeviceHoloLens->>ARDeviceHoloLens: 1. 1. Decode QR Code and extract assetId
    activate APIGatewayKong
    ARDeviceHoloLens->>APIGatewayKong: 2. 2. Request associated tag identifiers for the asset
    APIGatewayKong-->>ARDeviceHoloLens: 6. HTTP 200 OK with list of tag identifiers
    activate AssetTopologyService
    APIGatewayKong->>AssetTopologyService: 3. 3. Forward request to Asset Service
    AssetTopologyService-->>APIGatewayKong: 5. Return asset tag data
    AssetTopologyService->>AssetTopologyService: 4. 4. Retrieve tag mappings from DB/Cache
    AssetTopologyService-->>AssetTopologyService: Return data
    ARDeviceHoloLens->>APIGatewayKong: 7. 7. Request real-time values for identified tags
    APIGatewayKong-->>ARDeviceHoloLens: 11. HTTP 200 OK with latest data points for each tag
    activate QueryAnalyticsService
    APIGatewayKong->>QueryAnalyticsService: 8. 8. Forward request to Query Service
    QueryAnalyticsService-->>APIGatewayKong: 10. Return latest data points
    QueryAnalyticsService->>QueryAnalyticsService: 9. 9. Query TimescaleDB for latest value of each tag
    QueryAnalyticsService-->>QueryAnalyticsService: Return latest data points
    ARDeviceHoloLens->>ARDeviceHoloLens: 12. 12. Render data as holographic overlay

    note over ARDeviceHoloLens: Trigger: Operator wearing the AR device physically looks at a piece of equipment that has a pre-c...
    note over APIGatewayKong: The API Gateway is responsible for JWT validation using the public key from the IdP (Keycloak) an...

    deactivate QueryAnalyticsService
    deactivate AssetTopologyService
    deactivate APIGatewayKong
