sequenceDiagram
    participant "Frontend SPA" as FrontendSPA
    actor "User's Browser" as UsersBrowser
    participant "Keycloak IdP" as KeycloakIdP
    participant "API Gateway" as APIGateway
    participant "IAM Service" as IAMService

    activate FrontendSPA
    UsersBrowser->>FrontendSPA: 1. 1. User initiates login (clicks 'Login' button).
    FrontendSPA->>FrontendSPA: 2. 2. Generate PKCE code_verifier and code_challenge.
    FrontendSPA-->>FrontendSPA: code_verifier, code_challenge
    FrontendSPA->>UsersBrowser: 3. 3. Construct and trigger redirect to Keycloak authorization endpoint.
    activate KeycloakIdP
    UsersBrowser->>KeycloakIdP: 4. 4. GET /auth/realms/{realm}/protocol/openid-connect/auth?params...
    KeycloakIdP-->>UsersBrowser: HTML Login Page
    UsersBrowser->>KeycloakIdP: 5. 5. User submits credentials (and MFA if required).
    KeycloakIdP-->>UsersBrowser: HTTP 302 Redirect
    KeycloakIdP->>UsersBrowser: 6. 6. Redirect back to application's redirect_uri with authorization code.
    UsersBrowser->>FrontendSPA: 7. 7. GET /callback?code=...&state=...
    FrontendSPA-->>UsersBrowser: Application SPA loaded
    FrontendSPA->>APIGateway: 8. 8. Exchange authorization code for tokens.
    APIGateway-->>FrontendSPA: JWT Payload { accessToken, ... }
    activate IAMService
    APIGateway->>IAMService: 9. 9. Route token exchange request to IAM service.
    IAMService-->>APIGateway: JWT Payload
    IAMService->>KeycloakIdP: 10. 10. POST to Keycloak token endpoint (back-channel).
    KeycloakIdP-->>IAMService: { id_token, access_token, refresh_token }
    KeycloakIdP->>KeycloakIdP: 11. 11. Validate code, PKCE verifier, and client credentials. Issue tokens.
    KeycloakIdP-->>KeycloakIdP: JWTs
    KeycloakIdP->>IAMService: 12. 12. Return tokens to IAM Service.
    IAMService->>IAMService: 13. 13. Log successful authentication event.
    IAMService->>APIGateway: 14. 14. Return tokens.
    APIGateway->>FrontendSPA: 15. 15. Forward tokens to frontend.
    FrontendSPA->>FrontendSPA: 16. 16. Securely store tokens and redirect to dashboard.

    note over IAMService: The token exchange (step 10) is a confidential, back-channel communication. The frontend SPA must...
    note over FrontendSPA: Tokens must be stored securely in the frontend. Using browser memory (e.g., React Context/Redux) ...

    deactivate IAMService
    deactivate KeycloakIdP
    deactivate FrontendSPA
