sequenceDiagram
    participant "Frontend: Management Plane" as FrontendManagementPlane
    participant "API Gateway" as APIGateway
    participant "AI/ML Management Service" as AIMLManagementService
    participant "Alarm & Notification Service" as AlarmNotificationService
    participant "Audit Service" as AuditService

    activate APIGateway
    FrontendManagementPlane->>APIGateway: 1. 1. [Data Scientist] POST /api/v1/models (multipart/form-data)
    APIGateway-->>FrontendManagementPlane: 201 Created
    activate AIMLManagementService
    APIGateway->>AIMLManagementService: 2. 1.1. Route validated request to service
    AIMLManagementService-->>APIGateway: 201 Created
    FrontendManagementPlane->>APIGateway: 6. 2. [Administrator] POST /api/v1/models/{modelId}/approve
    APIGateway-->>FrontendManagementPlane: 200 OK
    APIGateway->>AIMLManagementService: 7. 2.1. Route validated approval request to service
    AIMLManagementService-->>APIGateway: 200 OK

    note over AIMLManagementService: The call to the Audit Service (2.1.3) is intentionally asynchronous (fire-and-forget) to ensure t...

    deactivate AIMLManagementService
    deactivate APIGateway
