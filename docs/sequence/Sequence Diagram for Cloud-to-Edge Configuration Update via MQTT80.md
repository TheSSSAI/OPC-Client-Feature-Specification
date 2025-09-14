sequenceDiagram
    participant "Frontend: Central Management Plane" as FrontendCentralManagementPlane
    participant "API Gateway" as APIGateway
    participant "Device Management Service" as DeviceManagementService
    participant "MQTT Broker" as MQTTBroker
    actor "OPC Core Client" as OPCCoreClient

    activate APIGateway
    FrontendCentralManagementPlane->>APIGateway: 1. 1. POST /api/v1/clients/{clientId}/config
    APIGateway-->>FrontendCentralManagementPlane: 11. 202 Accepted
    activate DeviceManagementService
    APIGateway->>DeviceManagementService: 2. 2. Route POST /internal/clients/{clientId}/config
    DeviceManagementService-->>APIGateway: 10. 202 Accepted
    DeviceManagementService->>DeviceManagementService: 3. 3. [Self] Validate & Persist Configuration
    activate MQTTBroker
    DeviceManagementService->>MQTTBroker: 4. 4. PUBLISH ClientConfigurationUpdated Command
    activate OPCCoreClient
    MQTTBroker->>OPCCoreClient: 5. 5. DELIVER ClientConfigurationUpdated Command
    OPCCoreClient->>OPCCoreClient: 6. 6. [Self] Validate & Apply Configuration
    OPCCoreClient->>MQTTBroker: 7. 7. PUBLISH ClientStatusReported Event (Success/Failure)
    MQTTBroker->>DeviceManagementService: 8. 8. DELIVER ClientStatusReported Event
    DeviceManagementService->>DeviceManagementService: 9. 9. [Self] Process Status and Update DB

    note over MQTTBroker: MQTT QoS 1 (At-least-once) is crucial for ensuring commands are not lost. Client-side handlers mu...
    note over OPCCoreClient: The MQTT broker's persistent session feature is critical for managing offline clients. When a cli...
    note over MQTTBroker: The topic structure tenants/{tenantId}/clients/{clientId}/... is a security enforcement point. Br...

    deactivate OPCCoreClient
    deactivate MQTTBroker
    deactivate DeviceManagementService
    deactivate APIGateway
