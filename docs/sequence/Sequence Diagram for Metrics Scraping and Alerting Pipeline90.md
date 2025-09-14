sequenceDiagram
    participant "Prometheus Server" as PrometheusServer
    participant "Query & Analytics Service" as QueryAnalyticsService
    participant "Alertmanager" as Alertmanager
    participant "PagerDuty API" as PagerDutyAPI

    activate QueryAnalyticsService
    PrometheusServer->>QueryAnalyticsService: 1. Scrape metrics endpoint based on configured scrape interval (e.g., 30s).
    QueryAnalyticsService-->>PrometheusServer: Returns current metric values in Prometheus text format.
    QueryAnalyticsService->>PrometheusServer: 2. 200 OK with metrics payload.
    PrometheusServer->>PrometheusServer: 3. Ingests time-series data into TSDB.
    PrometheusServer->>PrometheusServer: 4. Evaluates alerting rules against TSDB data based on evaluation interval (e.g., 30s).
    activate Alertmanager
    PrometheusServer->>Alertmanager: 5. Sends alert to Alertmanager after the for duration is met and state becomes 'Firing'.
    Alertmanager-->>PrometheusServer: Acknowledges receipt of the alert.
    Alertmanager->>PrometheusServer: 6. 200 OK
    activate PagerDutyAPI
    Alertmanager->>PagerDutyAPI: 7. Routes and sends formatted notification to PagerDuty API.
    PagerDutyAPI-->>Alertmanager: Acknowledges the event has been successfully queued.
    PagerDutyAPI->>Alertmanager: 8. 202 Accepted with dedup_key.

    note over Alertmanager: Alertmanager applies internal logic before notifying: 1. **Grouping:** Groups alerts by labels (e...

    deactivate PagerDutyAPI
    deactivate Alertmanager
    deactivate QueryAnalyticsService
