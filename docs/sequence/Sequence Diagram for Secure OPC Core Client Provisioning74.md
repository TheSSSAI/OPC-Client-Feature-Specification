sequenceDiagram
    participant "Frontend (Management Plane)" as FrontendManagementPlane
    participant "API Gateway" as APIGateway
    participant "Device Management Service" as DeviceManagementService
    actor "OPC Core Client (Unprovisioned)" as OPCCoreClientUnprovisioned

    activate APIGateway
    FrontendManagementPlane->>APIGateway: 1. 1. Request new client registration token
    APIGateway-->>FrontendManagementPlane: 4. Return one-time token and expiry
    activate DeviceManagementService
    APIGateway->>DeviceManagementService: 2. 2. Forward token generation request
    DeviceManagementService-->>APIGateway: 3. Return generated token details
    OPCCoreClientUnprovisioned->>DeviceManagementService: 5. 5. Register with token and submit CSR
    DeviceManagementService-->>OPCCoreClientUnprovisioned: 8. Return signed client certificate
    DeviceManagementService->>DeviceManagementService: 6. 6. Validate token, generate/sign certificate
    OPCCoreClientUnprovisioned->>OPCCoreClientUnprovisioned: 9. 9. Securely store private key and certificate

    note over DeviceManagementService: The /provision/register endpoint must be publicly accessible but secured via the one-time token. ...
    note over DeviceManagementService: Database Table ClientRegistrationTokens: - token_hash (string, PK) - tenant_id (uuid, FK) - clien...
    note over OPCCoreClientUnprovisioned: Client is now provisioned. All subsequent communications (e.g., gRPC data streaming) will use the...

    deactivate DeviceManagementService
    deactivate APIGateway
