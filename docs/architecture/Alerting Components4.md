# 1 System Overview

## 1.1 Analysis Date

2025-06-13

## 1.2 Technology Stack

- .NET 8
- Kubernetes (EKS)
- PostgreSQL/TimescaleDB
- gRPC
- MQTT
- Prometheus
- Alertmanager

## 1.3 Metrics Configuration

- Prometheus scrapes /metrics endpoints from all microservices and OPC Core Clients (REQ-1-090).
- Logs are aggregated in OpenSearch via Fluentd (REQ-1-090).
- Distributed tracing is enabled via OpenTelemetry (REQ-1-090).

## 1.4 Monitoring Needs

- Ensure high availability (99.9%) and performance SLAs (latency < 200ms) are met.
- Monitor the health and connectivity of up to 10,000 distributed OPC Core Clients.
- Detect failures in the data ingestion pipeline and edge-to-cloud communication.
- Ensure security mechanisms like authentication are functioning correctly.

## 1.5 Environment

production

# 2.0 Alert Condition And Threshold Design

## 2.1 Critical Metrics Alerts

### 2.1.1 Metric

#### 2.1.1.1 Metric

probe_success{job="blackbox-api-gateway"}

#### 2.1.1.2 Condition

== 0 for 1m

#### 2.1.1.3 Threshold Type

static

#### 2.1.1.4 Value

0

#### 2.1.1.5 Justification

Directly measures the availability SLO of 99.9% (REQ-1-084). If the primary external endpoint is down, the entire system is unavailable to users.

#### 2.1.1.6 Business Impact

Critical. All users and external systems are unable to access the Central Management Plane.

### 2.1.2.0 Metric

#### 2.1.2.1 Metric

api_latency_p95_seconds

#### 2.1.2.2 Condition

> 0.2 for 5m

#### 2.1.2.3 Threshold Type

static

#### 2.1.2.4 Value

0.2s

#### 2.1.2.5 Justification

Monitors the API performance SLO specified in REQ-1-074. Sustained high latency degrades user experience and can indicate downstream issues.

#### 2.1.2.6 Business Impact

High. System is usable but slow, potentially causing user frustration and impacting operations.

### 2.1.3.0 Metric

#### 2.1.3.1 Metric

up{job="opc-core-client"}

#### 2.1.3.2 Condition

absent() for 5m

#### 2.1.3.3 Threshold Type

static

#### 2.1.3.4 Value

absent

#### 2.1.3.5 Justification

Monitors the connectivity and health of distributed OPC Core Clients (REQ-1-062). Loss of heartbeat indicates a client is offline and potentially not buffering data.

#### 2.1.3.6 Business Impact

Medium. Loss of a single client impacts one site, but widespread alerts would indicate a systemic issue.

### 2.1.4.0 Metric

#### 2.1.4.1 Metric

timescaledb_ingestion_rate_error_ratio

#### 2.1.4.2 Condition

> 0.05 for 10m

#### 2.1.4.3 Threshold Type

static

#### 2.1.4.4 Value

5%

#### 2.1.4.5 Justification

Monitors the health of the core data ingestion pipeline (REQ-1-075). A high error rate indicates potential data loss.

#### 2.1.4.6 Business Impact

High. Risk of permanent data loss for real-time industrial processes.

### 2.1.5.0 Metric

#### 2.1.5.1 Metric

opc_client_buffer_full_events_total

#### 2.1.5.2 Condition

increase(opc_client_buffer_full_events_total[5m]) > 0

#### 2.1.5.3 Threshold Type

static

#### 2.1.5.4 Value

1

#### 2.1.5.5 Justification

Directly implements the alert requirement from REQ-1-079 when a client's offline buffer is full, indicating imminent data loss at the edge.

#### 2.1.5.6 Business Impact

High. The client is now overwriting its oldest data, leading to data loss.

### 2.1.6.0 Metric

#### 2.1.6.1 Metric

kube_hpa_status_condition{condition="ScalingLimited"}

#### 2.1.6.2 Condition

== 1 for 15m

#### 2.1.6.3 Threshold Type

static

#### 2.1.6.4 Value

1

#### 2.1.6.5 Justification

Indicates a service is under high load and cannot scale further (REQ-1-085), signaling an impending performance degradation or outage.

#### 2.1.6.6 Business Impact

High. The system cannot respond to increased load, likely leading to SLO breaches.

## 2.2.0.0 Threshold Strategies

*No items available*

## 2.3.0.0 Baseline Deviation Alerts

*No items available*

## 2.4.0.0 Predictive Alerts

*No items available*

## 2.5.0.0 Compound Conditions

- {'name': 'DatabaseConnectionPoolSaturated', 'conditions': ['pg_stat_activity_count{state="active"} / pg_settings_max_connections > 0.9', 'api_latency_p95_seconds > 0.2'], 'logic': 'AND', 'timeWindow': '5m', 'justification': 'Correlates high database connection usage with user-facing API latency to provide a more specific and actionable alert than either metric alone.'}

# 3.0.0.0 Severity Level Classification

## 3.1.0.0 Severity Definitions

### 3.1.1.0 Level

#### 3.1.1.1 Level

🚨 Critical

#### 3.1.1.2 Criteria

System-wide outage, complete data loss, or security breach is occurring or imminent. Immediate, all-hands-on-deck response required.

#### 3.1.1.3 Business Impact

Catastrophic. Stops business operations for all tenants.

#### 3.1.1.4 Customer Impact

All tenants affected, system is unusable.

#### 3.1.1.5 Response Time

< 5 minutes (automated response), < 15 minutes (human response)

#### 3.1.1.6 Escalation Required

✅ Yes

### 3.1.2.0 Level

#### 3.1.2.1 Level

🔴 High

#### 3.1.2.2 Criteria

A major component or feature is unavailable, or a core SLO/SLA is breached. High risk of escalating to Critical. Urgent response required.

#### 3.1.2.3 Business Impact

Severe. Major disruption to business operations.

#### 3.1.2.4 Customer Impact

Significant portion of tenants or a core feature is affected.

#### 3.1.2.5 Response Time

< 30 minutes

#### 3.1.2.6 Escalation Required

✅ Yes

### 3.1.3.0 Level

#### 3.1.3.1 Level

🟡 Medium

#### 3.1.3.2 Criteria

Performance degradation or partial loss of non-critical functionality. Does not pose an immediate threat to system stability.

#### 3.1.3.3 Business Impact

Moderate. Business operations are impaired but can continue.

#### 3.1.3.4 Customer Impact

Minor features affected or performance is degraded.

#### 3.1.3.5 Response Time

< 4 hours (during business hours)

#### 3.1.3.6 Escalation Required

❌ No

### 3.1.4.0 Level

#### 3.1.4.1 Level

🟢 Low

#### 3.1.4.2 Criteria

Informational or indicative of a potential future issue. No immediate impact on service.

#### 3.1.4.3 Business Impact

Low. No direct impact on business operations.

#### 3.1.4.4 Customer Impact

No direct customer impact.

#### 3.1.4.5 Response Time

Within 24-48 hours

#### 3.1.4.6 Escalation Required

❌ No

## 3.2.0.0 Business Impact Matrix

*No items available*

## 3.3.0.0 Customer Impact Criteria

*No items available*

## 3.4.0.0 Sla Violation Severity

*No items available*

## 3.5.0.0 System Health Severity

*No items available*

# 4.0.0.0 Notification Channel Strategy

## 4.1.0.0 Channel Configuration

### 4.1.1.0 Channel

#### 4.1.1.1 Channel

pagerduty

#### 4.1.1.2 Purpose

Primary notification channel for alerts requiring immediate, on-call response.

#### 4.1.1.3 Applicable Severities

- Critical
- High

#### 4.1.1.4 Time Constraints

24/7

#### 4.1.1.5 Configuration

*No data available*

### 4.1.2.0 Channel

#### 4.1.2.1 Channel

slack

#### 4.1.2.2 Purpose

General awareness, collaboration, and notifications for medium/low severity issues.

#### 4.1.2.3 Applicable Severities

- Critical
- High
- Medium
- Low

#### 4.1.2.4 Time Constraints

24/7

#### 4.1.2.5 Configuration

*No data available*

## 4.2.0.0 Routing Rules

### 4.2.1.0 Condition

#### 4.2.1.1 Condition

alert.severity == 'Critical'

#### 4.2.1.2 Severity

Critical

#### 4.2.1.3 Alert Type

Any

#### 4.2.1.4 Channels

- pagerduty
- slack

#### 4.2.1.5 Priority

🔹 1

### 4.2.2.0 Condition

#### 4.2.2.1 Condition

alert.severity == 'High'

#### 4.2.2.2 Severity

High

#### 4.2.2.3 Alert Type

Any

#### 4.2.2.4 Channels

- pagerduty
- slack

#### 4.2.2.5 Priority

🔹 2

### 4.2.3.0 Condition

#### 4.2.3.1 Condition

alert.severity == 'Medium'

#### 4.2.3.2 Severity

Medium

#### 4.2.3.3 Alert Type

Any

#### 4.2.3.4 Channels

- slack

#### 4.2.3.5 Priority

🔹 3

## 4.3.0.0 Time Based Routing

*No items available*

## 4.4.0.0 Ticketing Integration

*No items available*

## 4.5.0.0 Emergency Notifications

*No items available*

## 4.6.0.0 Chat Platform Integration

*No items available*

# 5.0.0.0 Alert Correlation Implementation

## 5.1.0.0 Grouping Requirements

- {'groupingCriteria': 'Labels: cluster, namespace, service, tenantId', 'timeWindow': '5m', 'maxGroupSize': 0, 'suppressionStrategy': "Alertmanager's default grouping. All alerts with identical labels are grouped into a single notification."}

## 5.2.0.0 Parent Child Relationships

- {'parentCondition': 'Alert: `DatabaseUnavailable` is firing', 'childConditions': ['Alert: `APILatencyHigh`', 'Alert: `DataIngestionErrorRateHigh`'], 'suppressionDuration': 'While parent is active', 'propagationRules': "Use Alertmanager's inhibition rules to suppress child alerts if the parent alert is active. This prevents a storm of alerts when the root cause is a database outage."}

## 5.3.0.0 Topology Based Correlation

*No items available*

## 5.4.0.0 Time Window Correlation

*No items available*

## 5.5.0.0 Causal Relationship Detection

*No items available*

## 5.6.0.0 Maintenance Window Suppression

- {'maintenanceType': 'Planned deployments, database maintenance', 'suppressionScope': ['Specific services', 'Entire cluster'], 'automaticDetection': False, 'manualOverride': True}

# 6.0.0.0 False Positive Mitigation

## 6.1.0.0 Noise Reduction Strategies

- {'strategy': 'Sustained Threshold Breach (`for` clause)', 'implementation': 'All Prometheus alert rules include a `for` clause (e.g., `for: 5m`) to ensure the condition is stable before firing.', 'applicableAlerts': ['All metric-based alerts'], 'effectiveness': 'High. Eliminates alerts from transient spikes.'}

## 6.2.0.0 Confirmation Counts

*No items available*

## 6.3.0.0 Dampening And Flapping

*No items available*

## 6.4.0.0 Alert Validation

*No items available*

## 6.5.0.0 Smart Filtering

*No items available*

## 6.6.0.0 Quorum Based Alerting

*No items available*

# 7.0.0.0 On Call Management Integration

## 7.1.0.0 Escalation Paths

- {'severity': 'Critical', 'escalationLevels': [{'level': 1, 'recipients': ['Primary On-Call Engineer'], 'escalationTime': '15m', 'requiresAcknowledgment': True}, {'level': 2, 'recipients': ['Secondary On-Call Engineer'], 'escalationTime': '15m', 'requiresAcknowledgment': True}], 'ultimateEscalation': 'Engineering Manager'}

## 7.2.0.0 Escalation Timeframes

*No items available*

## 7.3.0.0 On Call Rotation

*No items available*

## 7.4.0.0 Acknowledgment Requirements

### 7.4.1.0 Severity

#### 7.4.1.1 Severity

Critical

#### 7.4.1.2 Acknowledgment Timeout

15m

#### 7.4.1.3 Auto Escalation

✅ Yes

#### 7.4.1.4 Requires Comment

❌ No

### 7.4.2.0 Severity

#### 7.4.2.1 Severity

High

#### 7.4.2.2 Acknowledgment Timeout

30m

#### 7.4.2.3 Auto Escalation

✅ Yes

#### 7.4.2.4 Requires Comment

❌ No

## 7.5.0.0 Incident Ownership

*No items available*

## 7.6.0.0 Follow The Sun Support

*No items available*

# 8.0.0.0 Project Specific Alerts Config

## 8.1.0.0 Alerts

### 8.1.1.0 APIGatewayUnavailable

#### 8.1.1.1 Name

APIGatewayUnavailable

#### 8.1.1.2 Description

The API Gateway is not responding to external health checks, indicating a full system outage.

#### 8.1.1.3 Condition

probe_success{job="blackbox-api-gateway"} == 0 for 1m

#### 8.1.1.4 Threshold

0

#### 8.1.1.5 Severity

Critical

#### 8.1.1.6 Channels

- pagerduty
- slack

#### 8.1.1.7 Correlation

##### 8.1.1.7.1 Group Id

system-availability

##### 8.1.1.7.2 Suppression Rules

*No items available*

#### 8.1.1.8.0 Escalation

##### 8.1.1.8.1 Enabled

✅ Yes

##### 8.1.1.8.2 Escalation Time

15m

##### 8.1.1.8.3 Escalation Path

- PrimaryOnCall
- SecondaryOnCall

#### 8.1.1.9.0 Suppression

| Property | Value |
|----------|-------|
| Maintenance Window | ✅ |
| Dependency Failure | ❌ |
| Manual Override | ✅ |

#### 8.1.1.10.0 Validation

##### 8.1.1.10.1 Confirmation Count

0

##### 8.1.1.10.2 Confirmation Window

1m

#### 8.1.1.11.0 Remediation

##### 8.1.1.11.1 Automated Actions

*No items available*

##### 8.1.1.11.2 Runbook Url

🔗 [http://runbooks.example.com/api-gateway-outage](http://runbooks.example.com/api-gateway-outage)

##### 8.1.1.11.3 Troubleshooting Steps

- Check status of the Kong Ingress Controller pods in Kubernetes.
- Verify the AWS Network Load Balancer health.
- Check for critical errors in the API Gateway logs in OpenSearch.

### 8.1.2.0.0 OPCClientOffline

#### 8.1.2.1.0 Name

OPCClientOffline

#### 8.1.2.2.0 Description

An OPC Core Client has stopped sending heartbeats and is considered offline.

#### 8.1.2.3.0 Condition

up{job="opc-core-client"} == 0 for 10m

#### 8.1.2.4.0 Threshold

0

#### 8.1.2.5.0 Severity

Medium

#### 8.1.2.6.0 Channels

- slack

#### 8.1.2.7.0 Correlation

##### 8.1.2.7.1 Group Id

opc-client-{{ $labels.tenantId }}-{{ $labels.instance }}

##### 8.1.2.7.2 Suppression Rules

*No items available*

#### 8.1.2.8.0 Escalation

##### 8.1.2.8.1 Enabled

❌ No

##### 8.1.2.8.2 Escalation Time



##### 8.1.2.8.3 Escalation Path

*No items available*

#### 8.1.2.9.0 Suppression

| Property | Value |
|----------|-------|
| Maintenance Window | ✅ |
| Dependency Failure | ❌ |
| Manual Override | ✅ |

#### 8.1.2.10.0 Validation

##### 8.1.2.10.1 Confirmation Count

0

##### 8.1.2.10.2 Confirmation Window

10m

#### 8.1.2.11.0 Remediation

##### 8.1.2.11.1 Automated Actions

*No items available*

##### 8.1.2.11.2 Runbook Url

🔗 [http://runbooks.example.com/opc-client-troubleshooting](http://runbooks.example.com/opc-client-troubleshooting)

##### 8.1.2.11.3 Troubleshooting Steps

- Check the client's status in the Central Management Plane.
- Advise tenant to check network connectivity at the edge location.
- Retrieve logs from the client via the Central Management Plane for analysis.

### 8.1.3.0.0 HighAPILatency

#### 8.1.3.1.0 Name

HighAPILatency

#### 8.1.3.2.0 Description

The 95th percentile of API request latency is above the 200ms SLO.

#### 8.1.3.3.0 Condition

api_latency_p95_seconds > 0.2 for 5m

#### 8.1.3.4.0 Threshold

0.2s

#### 8.1.3.5.0 Severity

High

#### 8.1.3.6.0 Channels

- pagerduty
- slack

#### 8.1.3.7.0 Correlation

##### 8.1.3.7.1 Group Id

api-performance

##### 8.1.3.7.2 Suppression Rules

- DatabaseUnavailable

#### 8.1.3.8.0 Escalation

##### 8.1.3.8.1 Enabled

✅ Yes

##### 8.1.3.8.2 Escalation Time

30m

##### 8.1.3.8.3 Escalation Path

- PrimaryOnCall

#### 8.1.3.9.0 Suppression

| Property | Value |
|----------|-------|
| Maintenance Window | ✅ |
| Dependency Failure | ✅ |
| Manual Override | ✅ |

#### 8.1.3.10.0 Validation

##### 8.1.3.10.1 Confirmation Count

0

##### 8.1.3.10.2 Confirmation Window

5m

#### 8.1.3.11.0 Remediation

##### 8.1.3.11.1 Automated Actions

*No items available*

##### 8.1.3.11.2 Runbook Url

🔗 [http://runbooks.example.com/api-latency-investigation](http://runbooks.example.com/api-latency-investigation)

##### 8.1.3.11.3 Troubleshooting Steps

- Check Grafana dashboards for database and Redis latency.
- Analyze distributed traces for slow endpoints.
- Check if any service is hitting CPU limits or HPA is maxed out.

## 8.2.0.0.0 Alert Groups

*No items available*

## 8.3.0.0.0 Notification Templates

*No items available*

# 9.0.0.0.0 Implementation Priority

## 9.1.0.0.0 Component

### 9.1.1.0.0 Component

System Availability & Performance Alerts (API Gateway, Latency)

### 9.1.2.0.0 Priority

🔴 high

### 9.1.3.0.0 Dependencies

- Prometheus Blackbox Exporter
- Kong Prometheus Plugin

### 9.1.4.0.0 Estimated Effort

Low

### 9.1.5.0.0 Risk Level

low

## 9.2.0.0.0 Component

### 9.2.1.0.0 Component

Edge Component Health Alerts (OPC Client Offline, Buffer Full)

### 9.2.2.0.0 Priority

🔴 high

### 9.2.3.0.0 Dependencies

- OPC Core Client /metrics endpoint implementation

### 9.2.4.0.0 Estimated Effort

Medium

### 9.2.5.0.0 Risk Level

medium

## 9.3.0.0.0 Component

### 9.3.1.0.0 Component

Data Pipeline Health Alerts (DB, Ingestion)

### 9.3.2.0.0 Priority

🟡 medium

### 9.3.3.0.0 Dependencies

- Postgres Exporter
- Application metrics for ingestion service

### 9.3.4.0.0 Estimated Effort

Medium

### 9.3.5.0.0 Risk Level

medium

## 9.4.0.0.0 Component

### 9.4.1.0.0 Component

Alert Correlation & Suppression Rules

### 9.4.2.0.0 Priority

🟡 medium

### 9.4.3.0.0 Dependencies

- Base alerts implementation

### 9.4.4.0.0 Estimated Effort

Low

### 9.4.5.0.0 Risk Level

low

# 10.0.0.0.0 Risk Assessment

## 10.1.0.0.0 Risk

### 10.1.1.0.0 Risk

Alert Fatigue

### 10.1.2.0.0 Impact

high

### 10.1.3.0.0 Probability

high

### 10.1.4.0.0 Mitigation

Start with a minimal, high-signal set of alerts. Aggressively use `for` clauses, correlation, and suppression. Regularly review and tune noisy alerts.

### 10.1.5.0.0 Contingency Plan

Establish a bi-weekly alert review meeting to prune or adjust noisy alerts.

## 10.2.0.0.0 Risk

### 10.2.1.0.0 Risk

Non-Actionable Alerts

### 10.2.2.0.0 Impact

medium

### 10.2.3.0.0 Probability

high

### 10.2.4.0.0 Mitigation

Every alert rule must be accompanied by a runbook URL. If an alert has no clear remediation path, it should be re-evaluated or converted to a dashboard metric.

### 10.2.5.0.0 Contingency Plan

On-call engineers are empowered to silence non-actionable alerts and create tickets for the development team to fix them.

# 11.0.0.0.0 Recommendations

## 11.1.0.0.0 Category

### 11.1.1.0.0 Category

🔹 Process

### 11.1.2.0.0 Recommendation

Implement a mandatory 'Runbook Driven Development' policy for all alerts.

### 11.1.3.0.0 Justification

Ensures that every alert is actionable and reduces the cognitive load on the on-call engineer, leading to faster incident resolution.

### 11.1.4.0.0 Priority

🔴 high

### 11.1.5.0.0 Implementation Notes

Add a required `runbook_url` annotation to all Prometheus alert rules in the CI/CD pipeline.

## 11.2.0.0.0 Category

### 11.2.1.0.0 Category

🔹 Implementation

### 11.2.2.0.0 Recommendation

Start with a very small set of critical alerts and expand only when a clear operational need is identified.

### 11.2.3.0.0 Justification

The most effective way to combat alert fatigue is to be extremely selective about what constitutes an alert. It is better to miss a minor issue than to drown in noise and miss a critical one.

### 11.2.4.0.0 Priority

🔴 high

### 11.2.5.0.0 Implementation Notes

Initially implement only the 'Critical' severity alerts defined, then gradually add 'High' severity alerts as the system stabilizes in production.

