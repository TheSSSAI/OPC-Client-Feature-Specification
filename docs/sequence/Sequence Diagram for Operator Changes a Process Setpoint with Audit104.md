sequenceDiagram
    participant "Frontend" as Frontend
    participant "API Gateway" as APIGateway
    participant "Device Mgmt Service" as DeviceMgmtService
    participant "Audit Service" as AuditService
    participant "MQTT Broker" as MQTTBroker
    actor "OPC Core Client" as OPCCoreClient

    activate APIGateway
    Frontend->>APIGateway: 1. 1. POST /api/v1/clients/{clientId}/tags/write
    APIGateway-->>Frontend: 8. HTTP 202 Accepted
    activate DeviceMgmtService
    APIGateway->>DeviceMgmtService: 2. 2. Route Request: POST /clients/{clientId}/tags/write
    DeviceMgmtService-->>APIGateway: 7. HTTP 202 Accepted
    DeviceMgmtService->>DeviceMgmtService: 3. 3. Validate Permissions
    activate AuditService
    DeviceMgmtService->>AuditService: 4. 4. LogAction(LogWriteOperationRequest)
    AuditService-->>DeviceMgmtService: 5. LogActionResponse { success: true }
    DeviceMgmtService->>MQTTBroker: 6. 6. PUBLISH tenants/{tenantId}/clients/{clientId}/commands
    activate OPCCoreClient
    MQTTBroker->>OPCCoreClient: 9. 9. Deliver WriteTag Command
    OPCCoreClient->>OPCCoreClient: 10. 10. Execute OPC Write to Server
    OPCCoreClient-->>OPCCoreClient: Result { success, oldValue }
    OPCCoreClient->>MQTTBroker: 11. 11. PUBLISH tenants/{...}/status/write_result
    MQTTBroker->>DeviceMgmtService: 12. 12. Deliver Write Status

    note over DeviceMgmtService: Business Rule Enforcement: Permission validation in step 3 is critical for ensuring only authoriz...
    note over AuditService: Compliance Mandate: The gRPC call to the Audit Service (step 4) is synchronous and blocking. A fa...
    note over APIGateway: Asynchronous Acknowledgement: The API returns '202 Accepted' immediately after dispatching the co...
    note over OPCCoreClient: Feedback Loop: The status message published by the client (step 11) closes the loop, providing co...

    deactivate OPCCoreClient
    deactivate AuditService
    deactivate DeviceMgmtService
    deactivate APIGateway
