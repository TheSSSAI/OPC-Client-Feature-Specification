sequenceDiagram
    actor "API Client" as APIClient
    participant "API Gateway" as APIGateway
    participant "Rate Limit Store" as RateLimitStore

    activate APIGateway
    APIClient->>APIGateway: 1. [Loop: N times within limit] POST /api/v1/assets
    APIGateway-->>APIClient: 200 OK
    APIGateway->>RateLimitStore: 2. INCR 'ratelimit:<user_id>:<route_id>'
    RateLimitStore-->>APIGateway: Current count (<= limit)
    APIClient->>APIGateway: 3. POST /api/v1/assets (Request N+1)
    APIGateway-->>APIClient: 429 Too Many Requests
    APIGateway->>RateLimitStore: 4. INCR 'ratelimit:<user_id>:<route_id>'
    RateLimitStore-->>APIGateway: Current count (> limit)
    APIGateway->>APIGateway: 5. [Decision] Count exceeds configured limit. Block request.

    note over APIGateway: The identifier for rate limiting (user_id) is configurable. It can be derived from the JWT 'sub' ...
    note over APIGateway: The rate limit (e.g., 100 requests/minute) and policy are defined declaratively in the Kong API G...

    deactivate APIGateway
