sequenceDiagram
    participant "Scheduler (K8s CronJob)" as SchedulerK8sCronJob
    participant "Integration Service" as IntegrationService
    participant "Query Service" as QueryService
    participant "Azure IoT Hub" as AzureIoTHub

    activate IntegrationService
    SchedulerK8sCronJob->>IntegrationService: 1. Invoke Job: POST /jobs/iot-sync/trigger
    IntegrationService->>IntegrationService: 2. [Internal] Retrieve Last Sync Watermark
    IntegrationService-->>IntegrationService: lastSyncTimestamp
    activate QueryService
    IntegrationService->>QueryService: 3. Query for new data: GET /api/v1/query/data?since={ts}
    QueryService-->>IntegrationService: 200 OK: { data: [...] }
    IntegrationService->>IntegrationService: 4. [Loop] Apply Transformation Rules
    IntegrationService-->>IntegrationService: transformedPayload[]
    activate AzureIoTHub
    IntegrationService->>AzureIoTHub: 5. Send transformed data batch: POST /devices/{devId}/messages/events
    AzureIoTHub-->>IntegrationService: 204 No Content
    IntegrationService->>IntegrationService: 6. [Internal] Update Last Sync Watermark
    IntegrationService-->>IntegrationService: Success

    note over AzureIoTHub: The Circuit Breaker pattern is critical here to prevent the Integration Service from overwhelming...

    deactivate AzureIoTHub
    deactivate QueryService
    deactivate IntegrationService
