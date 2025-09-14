sequenceDiagram
    participant "Central Management Plane UI" as CentralManagementPlaneUI
    participant "API Gateway" as APIGateway
    participant "IAM Service" as IAMService
    participant "Alarm & Notification Service" as AlarmNotificationService

    activate CentralManagementPlaneUI
    CentralManagementPlaneUI->>CentralManagementPlaneUI: 1. User navigates to '/profile/notifications'. Component mounts and dispatches Redux action fetchPreferences().
    CentralManagementPlaneUI->>APIGateway: 2. GET /api/v1/users/me/notification-preferences
    APIGateway-->>CentralManagementPlaneUI: 200 OK w/ UserNotificationPreferencesDto | 401 Unauthorized | 404 Not Found
    activate IAMService
    APIGateway->>IAMService: 3. Route request to IAM service: GET /users/{userId}/notification-preferences
    IAMService-->>APIGateway: Forwards response from IAM Service
    IAMService->>IAMService: 4. Retrieve user preferences from 'UserPreferences' table where userId matches.
    IAMService-->>IAMService: Returns UserPreferences entity or null.
    IAMService->>APIGateway: 5. Return 200 OK with UserNotificationPreferencesDto
    CentralManagementPlaneUI->>CentralManagementPlaneUI: 6. Receives data, updates Redux store, and renders preference form with current settings.
    CentralManagementPlaneUI->>CentralManagementPlaneUI: 7. User modifies settings and clicks 'Save'. onSubmit handler is triggered.
    CentralManagementPlaneUI->>APIGateway: 8. PUT /api/v1/users/me/notification-preferences
    APIGateway-->>CentralManagementPlaneUI: 200 OK | 400 Bad Request | 401 Unauthorized
    APIGateway->>IAMService: 9. Route request to IAM service: PUT /users/{userId}/notification-preferences
    IAMService-->>APIGateway: Forwards response from IAM Service
    IAMService->>IAMService: 10. Validate DTO and persist (UPSERT) changes into 'UserPreferences' table.
    IAMService-->>IAMService: Success or Failure
    IAMService->>APIGateway: 11. Return 200 OK
    CentralManagementPlaneUI->>CentralManagementPlaneUI: 12. Displays a success toast: 'Preferences saved successfully'.
    AlarmNotificationService->>IAMService: 13. GetUserNotificationPreferences(userId)
    IAMService-->>AlarmNotificationService: UserNotificationPreferencesResponse | NotFoundError
    AlarmNotificationService->>AlarmNotificationService: 14. Use preferences to route notification to correct channels (e.g., Email, SMS).


    deactivate IAMService
    deactivate CentralManagementPlaneUI
