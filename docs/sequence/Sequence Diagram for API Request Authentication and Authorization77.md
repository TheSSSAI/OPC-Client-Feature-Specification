sequenceDiagram
    actor "Frontend Client" as FrontendClient
    participant "API Gateway" as APIGateway
    participant "Identity Provider" as IdentityProvider
    participant "Asset Service" as AssetService

    activate APIGateway
    FrontendClient->>APIGateway: 1. Request Protected Resource: GET /api/v1/assets
    APIGateway-->>FrontendClient: Returns 200 OK with asset data, 401 Unauthorized, or 403 Forbidden
    APIGateway->>IdentityProvider: 2. Fetch Public Keys (JWKS) if not cached
    IdentityProvider-->>APIGateway: Returns JSON Web Key Set (JWKS)
    APIGateway->>APIGateway: 3. Perform JWT Validation (Security Checkpoint 1)
    activate AssetService
    APIGateway->>AssetService: 4. Forward Validated Request: GET /assets
    AssetService-->>APIGateway: Returns HTTP response from the service (e.g., 200 OK, 403 Forbidden)
    AssetService->>AssetService: 5. Perform RBAC Authorization (Security Checkpoint 2)
    AssetService->>AssetService: 6. Execute Business Logic (e.g., query database)

    note over APIGateway: JWKS Caching is critical for performance. The gateway should only fetch the JWKS from Keycloak on...
    note over AssetService: JWT Claims used for authorization must include tenant_id to enforce data isolation (REQ-CON-001) ...

    deactivate AssetService
    deactivate APIGateway
