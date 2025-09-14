sequenceDiagram
    participant "Alarm & Notification Service" as AlarmNotificationService
    participant "PostgreSQL Database" as PostgreSQLDatabase
    participant "AWS Secrets Manager" as AWSSecretsManager
    participant "PagerDuty API" as PagerDutyAPI

    activate AlarmNotificationService
    AlarmNotificationService->>AlarmNotificationService: 1. 1. processCriticalAlarm(alarmEvent)
    AlarmNotificationService->>PostgreSQLDatabase: 2. 2. SELECT * FROM NotificationRules WHERE ...
    PostgreSQLDatabase-->>AlarmNotificationService: 3. Return matching PagerDutyNotificationRule
    AlarmNotificationService->>AWSSecretsManager: 4. 4. getSecretValue(secretId: rule.pagerDutySecretId)
    AWSSecretsManager-->>AlarmNotificationService: 5. Return PagerDuty Integration Key
    AlarmNotificationService->>AlarmNotificationService: 6. 6. transformAlarmToPagerDutyPayload()
    AlarmNotificationService-->>AlarmNotificationService: 7. PagerDuty Events API v2 JSON Payload
    AlarmNotificationService->>PagerDutyAPI: 8. 8. POST /v2/enqueue (payload)
    PagerDutyAPI-->>AlarmNotificationService: 9. HTTP/1.1 202 Accepted
    AlarmNotificationService->>AlarmNotificationService: 10. 10. updateNotificationStatus(status='Success')

    note over AlarmNotificationService: Data Transformation Logic: Maps internal AlarmEvent properties to PagerDuty's schema. - AlarmEven...
    note over PagerDutyAPI: Resilience Strategy: - Retry Policy: Exponential backoff (1s, 2s, 4s) for 3 attempts on HTTP 5xx ...

    deactivate AlarmNotificationService
