sequenceDiagram
    participant "Frontend: Management Plane" as FrontendManagementPlane
    participant "API Gateway" as APIGateway
    participant "Device Management Service" as DeviceManagementService
    participant "MQTT Broker" as MQTTBroker
    actor "OPC Core Client" as OPCCoreClient
    participant "Docker Image Registry" as DockerImageRegistry

    activate APIGateway
    FrontendManagementPlane->>APIGateway: 1. 1. POST /api/v1/clients/{clientId}/update
    APIGateway-->>FrontendManagementPlane: 16. HTTP 202 Accepted
    activate DeviceManagementService
    APIGateway->>DeviceManagementService: 2. 2. Route Request: POST /clients/{clientId}/update
    DeviceManagementService-->>APIGateway: 15. HTTP 202 Accepted
    DeviceManagementService->>DeviceManagementService: 3. 3. Validate Request and Prepare Command
    activate MQTTBroker
    DeviceManagementService->>MQTTBroker: 4. 4. PUBLISH Update Command
    activate OPCCoreClient
    MQTTBroker->>OPCCoreClient: 5. 5. DELIVER Command Message
    OPCCoreClient->>OPCCoreClient: 6. 6. Execute Update Script
    activate DockerImageRegistry
    OPCCoreClient->>DockerImageRegistry: 7. 7. Pull Docker Image
    DockerImageRegistry-->>OPCCoreClient: 8. Image Layers
    OPCCoreClient->>OPCCoreClient: 9. 9. Stop, Remove, and Start New Container
    OPCCoreClient->>OPCCoreClient: 10. 10. [New Instance] Startup & Self Health Check
    OPCCoreClient->>MQTTBroker: 11. 11. PUBLISH Success Status
    MQTTBroker->>DeviceManagementService: 12. 12. DELIVER Status Message
    DeviceManagementService->>DeviceManagementService: 13. 13. Update Client Record in Database
    DeviceManagementService->>APIGateway: 14. 14. Return HTTP 202 Accepted

    note over OPCCoreClient: The updater script on the Edge Client is the most critical component for reliability. It must be ...
    note over FrontendManagementPlane: The entire operation is asynchronous from the user's perspective. The UI should immediately refle...

    deactivate DockerImageRegistry
    deactivate OPCCoreClient
    deactivate MQTTBroker
    deactivate DeviceManagementService
    deactivate APIGateway
