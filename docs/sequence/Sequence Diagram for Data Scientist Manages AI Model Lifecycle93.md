sequenceDiagram
    participant "Frontend: Management Plane" as FrontendManagementPlane
    participant "API Gateway" as APIGateway
    participant "Service: Data Query & Reporting" as ServiceDataQueryReporting
    participant "Data Sandbox Environment" as DataSandboxEnvironment
    participant "Service: AI/ML Management" as ServiceAIMLManagement

    activate APIGateway
    FrontendManagementPlane->>APIGateway: 1. 1. GET /api/v1/query/history?assetId=...&timeRange=...
    APIGateway-->>FrontendManagementPlane: Forwards request to DQR Service
    activate ServiceDataQueryReporting
    APIGateway->>ServiceDataQueryReporting: 2. 2. Forwards authorized GET request
    ServiceDataQueryReporting-->>APIGateway: Returns historical data or download link
    ServiceDataQueryReporting->>ServiceDataQueryReporting: 2.1. 2a. Validates user has 'Read' access to historical production data for the specified asset (REQ-USR-001)
    ServiceDataQueryReporting->>ServiceDataQueryReporting: 2.2. 2b. Queries TimescaleDB for historical data and generates CSV export
    FrontendManagementPlane->>DataSandboxEnvironment: 3. 3. Data Scientist uses exported data to train model externally
    FrontendManagementPlane->>FrontendManagementPlane: 4. 4. User selects ONNX file and enters metadata (name, description)
    FrontendManagementPlane->>APIGateway: 5. 5. POST /api/v1/models (multipart/form-data)
    APIGateway-->>FrontendManagementPlane: Forwards request to AML Service
    activate ServiceAIMLManagement
    APIGateway->>ServiceAIMLManagement: 6. 6. Forwards authorized POST request
    ServiceAIMLManagement-->>APIGateway: Returns 201 Created with new model details
    ServiceAIMLManagement->>ServiceAIMLManagement: 6.1. 6a. Validates ONNX file format and metadata
    ServiceAIMLManagement->>ServiceAIMLManagement: 6.2. 6b. Streams ONNX file to Amazon S3 object storage
    ServiceAIMLManagement->>ServiceAIMLManagement: 6.3. 6c. Persists model metadata to PostgreSQL DB with status 'Pending Approval'
    FrontendManagementPlane->>FrontendManagementPlane: 7. 7. UI state updates to show new model with status 'Pending Approval'
    FrontendManagementPlane->>APIGateway: 8. 8. POST /api/v1/models/{modelId}/approvals
    APIGateway-->>FrontendManagementPlane: Returns 202 Accepted
    APIGateway->>ServiceAIMLManagement: 9. 9. Forwards request to initiate approval workflow
    ServiceAIMLManagement-->>APIGateway: Returns 202 Accepted
    ServiceAIMLManagement->>ServiceAIMLManagement: 9.1. 9a. Updates model status in DB and triggers approval workflow (e.g., creates approval task)

    note over ServiceAIMLManagement: S3 Storage: The AML service is responsible for securely storing the model file in a tenant-isolat...

    deactivate ServiceAIMLManagement
    deactivate ServiceDataQueryReporting
    deactivate APIGateway
