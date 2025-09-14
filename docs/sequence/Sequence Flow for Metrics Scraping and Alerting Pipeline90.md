# 1 Overview

## 1.1 Diagram Id

SEQ-OF-004

## 1.2 Name

Metrics Scraping and Alerting Pipeline

## 1.3 Description

Prometheus periodically scrapes the /metrics endpoint of a microservice. It evaluates configured alerting rules against the collected metrics. If a rule's condition is met for a sustained period, it fires an alert to Alertmanager, which then groups, de-duplicates, and routes a notification to the appropriate channel like PagerDuty.

## 1.4 Type

🔹 OperationalFlow

## 1.5 Purpose

To provide a complete, automated pipeline for system health monitoring, from metric collection to operator notification, enabling proactive incident response as defined in REQ-MON-001.

## 1.6 Complexity

Medium

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

Continuous

## 1.9 Participants

- REPO-SVC-DQR
- Prometheus
- Alertmanager
- PagerDuty

## 1.10 Key Interactions

- Prometheus sends an HTTP GET request to the Query Service's /metrics endpoint.
- The service responds with its current metrics in Prometheus text format.
- Prometheus stores the time-series data in its TSDB.
- Prometheus evaluates a rule, e.g., `api_latency_p95 > 0.2s for 5m`.
- The rule's condition becomes true and the alert state changes to 'Firing'.
- Prometheus sends the alert with its labels to Alertmanager.
- Alertmanager applies routing rules based on labels and sends a formatted notification to PagerDuty.

## 1.11 Triggers

- A system metric (e.g., latency, error rate, CPU usage) crosses a predefined critical threshold.

## 1.12 Outcomes

- An incident is triggered in PagerDuty.
- The on-call engineer is alerted to a potential system issue with contextual information.

## 1.13 Business Rules

- A standard set of Key Performance Indicators (KPIs) shall be defined and monitored (REQ-MON-001).
- Alertmanager shall be configured to send critical alerts to on-call personnel via PagerDuty and Slack (REQ-MON-001).

## 1.14 Error Scenarios

- The service's /metrics endpoint is down, causing a `TargetDown` alert.
- Prometheus or Alertmanager services are down, disabling the monitoring pipeline.
- Alertmanager fails to send the notification to PagerDuty due to API or network issues.

## 1.15 Integration Points

- PagerDuty
- Slack

# 2.0 Details

## 2.1 Diagram Id

SEQ-OF-004-IMPL

## 2.2 Name

Implementation: Prometheus Metrics Scraping and Alerting Pipeline

## 2.3 Description

Provides a detailed technical implementation for the automated monitoring pipeline where Prometheus scrapes metrics from a target microservice, evaluates alerting rules, and fires alerts to Alertmanager for routing to PagerDuty. This flow is critical for meeting the observability requirements outlined in REQ-MON-001.

## 2.4 Participants

### 2.4.1 MonitoringSystem

#### 2.4.1.1 Repository Id

Prometheus

#### 2.4.1.2 Display Name

Prometheus Server

#### 2.4.1.3 Type

🔹 MonitoringSystem

#### 2.4.1.4 Technology

Prometheus v2.51.x on Kubernetes

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #E6522C |
| Stereotype | Monitoring & TSDB |

### 2.4.2.0 Microservice

#### 2.4.2.1 Repository Id

REPO-SVC-DQR

#### 2.4.2.2 Display Name

Query & Analytics Service

#### 2.4.2.3 Type

🔹 Microservice

#### 2.4.2.4 Technology

.NET 8 with prometheus-net library

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #34A853 |
| Stereotype | Target Service |

### 2.4.3.0 AlertingSystem

#### 2.4.3.1 Repository Id

Alertmanager

#### 2.4.3.2 Display Name

Alertmanager

#### 2.4.3.3 Type

🔹 AlertingSystem

#### 2.4.3.4 Technology

Alertmanager v0.27.x on Kubernetes

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #000000 |
| Stereotype | Alert Routing |

### 2.4.4.0 ExternalSaaS

#### 2.4.4.1 Repository Id

PagerDuty

#### 2.4.4.2 Display Name

PagerDuty API

#### 2.4.4.3 Type

🔹 ExternalSaaS

#### 2.4.4.4 Technology

PagerDuty Events API v2

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #06AC38 |
| Stereotype | Notification Service |

## 2.5.0.0 Interactions

### 2.5.1.0 MetricsCollection

#### 2.5.1.1 Source Id

Prometheus

#### 2.5.1.2 Target Id

REPO-SVC-DQR

#### 2.5.1.3 Message

Scrape metrics endpoint based on configured scrape interval (e.g., 30s).

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 MetricsCollection

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

Returns current metric values in Prometheus text format.

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 |
| Method | GET /metrics |
| Parameters | HTTP Headers:
- `Accept: application/openmetrics-t... |
| Authentication | None (occurs over internal cluster network; mTLS c... |
| Error Handling | If the endpoint is unreachable or returns a non-2x... |
| Performance | Request timeout configured in Prometheus scrape co... |

### 2.5.2.0 MetricsResponse

#### 2.5.2.1 Source Id

REPO-SVC-DQR

#### 2.5.2.2 Target Id

Prometheus

#### 2.5.2.3 Message

200 OK with metrics payload.

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 MetricsResponse

#### 2.5.2.6 Is Synchronous

❌ No

#### 2.5.2.7 Return Message



#### 2.5.2.8 Has Return

❌ No

#### 2.5.2.9 Is Activation

❌ No

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 |
| Method | Response |
| Parameters | HTTP Headers:
- `Content-Type: text/plain; version... |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Payload size impacts scrape duration. High cardina... |

### 2.5.3.0 InternalProcessing

#### 2.5.3.1 Source Id

Prometheus

#### 2.5.3.2 Target Id

Prometheus

#### 2.5.3.3 Message

Ingests time-series data into TSDB.

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 InternalProcessing

#### 2.5.3.6 Is Synchronous

❌ No

#### 2.5.3.7 Return Message



#### 2.5.3.8 Has Return

❌ No

#### 2.5.3.9 Is Activation

❌ No

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | TSDB.Append |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | Handles disk I/O errors, logs failures. Data loss ... |
| Performance | Highly optimized for high-volume writes. |

### 2.5.4.0 RuleEvaluation

#### 2.5.4.1 Source Id

Prometheus

#### 2.5.4.2 Target Id

Prometheus

#### 2.5.4.3 Message

Evaluates alerting rules against TSDB data based on evaluation interval (e.g., 30s).

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 RuleEvaluation

#### 2.5.4.6 Is Synchronous

❌ No

#### 2.5.4.7 Return Message



#### 2.5.4.8 Has Return

❌ No

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | RuleManager.Evaluate |
| Parameters | Rule file (`rules.yml`) defines the alert conditio... |
| Authentication | N/A |
| Error Handling | Syntax errors in rule files are logged at startup.... |
| Performance | Complex queries or high cardinality data can incre... |

### 2.5.5.0 AlertNotification

#### 2.5.5.1 Source Id

Prometheus

#### 2.5.5.2 Target Id

Alertmanager

#### 2.5.5.3 Message

Sends alert to Alertmanager after the `for` duration is met and state becomes 'Firing'.

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 AlertNotification

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

Acknowledges receipt of the alert.

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

✅ Yes

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 |
| Method | POST /api/v2/alerts |
| Parameters | Body: JSON array of alert objects, including label... |
| Authentication | None (internal cluster network). |
| Error Handling | Prometheus will retry sending alerts if Alertmanag... |
| Performance | Low latency expected. |

### 2.5.6.0 Acknowledgement

#### 2.5.6.1 Source Id

Alertmanager

#### 2.5.6.2 Target Id

Prometheus

#### 2.5.6.3 Message

200 OK

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 Acknowledgement

#### 2.5.6.6 Is Synchronous

❌ No

#### 2.5.6.7 Return Message



#### 2.5.6.8 Has Return

❌ No

#### 2.5.6.9 Is Activation

❌ No

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 |
| Method | Response |
| Parameters | Empty body. |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | N/A |

### 2.5.7.0 IncidentCreation

#### 2.5.7.1 Source Id

Alertmanager

#### 2.5.7.2 Target Id

PagerDuty

#### 2.5.7.3 Message

Routes and sends formatted notification to PagerDuty API.

#### 2.5.7.4 Sequence Number

7

#### 2.5.7.5 Type

🔹 IncidentCreation

#### 2.5.7.6 Is Synchronous

✅ Yes

#### 2.5.7.7 Return Message

Acknowledges the event has been successfully queued.

#### 2.5.7.8 Has Return

✅ Yes

#### 2.5.7.9 Is Activation

✅ Yes

#### 2.5.7.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | POST /v2/enqueue |
| Parameters | HTTP Headers:
- `Content-Type: application/json`

... |
| Authentication | The PagerDuty routing key (integration key) must b... |
| Error Handling | Alertmanager will retry on 4xx/5xx responses from ... |
| Performance | Depends on external PagerDuty API latency. Timeout... |

### 2.5.8.0 Acknowledgement

#### 2.5.8.1 Source Id

PagerDuty

#### 2.5.8.2 Target Id

Alertmanager

#### 2.5.8.3 Message

202 Accepted with dedup_key.

#### 2.5.8.4 Sequence Number

8

#### 2.5.8.5 Type

🔹 Acknowledgement

#### 2.5.8.6 Is Synchronous

❌ No

#### 2.5.8.7 Return Message



#### 2.5.8.8 Has Return

❌ No

#### 2.5.8.9 Is Activation

❌ No

#### 2.5.8.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | Response |
| Parameters | Body: JSON object confirming event acceptance. |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | N/A |

## 2.6.0.0 Notes

- {'content': "Alertmanager applies internal logic before notifying:\n1. **Grouping:** Groups alerts by labels (e.g., cluster, service).\n2. **Inhibition:** Suppresses certain alerts if others are already firing (e.g., suppress 'LowDisk' if 'InstanceDown' is active).\n3. **Silencing:** Mutes alerts based on user-defined temporary rules for maintenance.", 'position': 'top', 'participantId': 'Alertmanager', 'sequenceNumber': 6}

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | The PagerDuty Integration Key (`routing_key`) is h... |
| Performance Targets | End-to-end alert propagation time (from metric thr... |
| Error Handling Strategy | 1. **Target Down:** Prometheus will fire a synthet... |
| Testing Considerations | 1. **Rule Validation:** Use `promtool check rules ... |
| Monitoring Requirements | The monitoring system itself must be monitored ('m... |
| Deployment Considerations | All Prometheus and Alertmanager configurations (sc... |

