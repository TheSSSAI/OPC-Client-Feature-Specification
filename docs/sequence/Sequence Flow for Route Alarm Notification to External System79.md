# 1 Overview

## 1.1 Diagram Id

SEQ-IF-001

## 1.2 Name

Route Alarm Notification to External System

## 1.3 Description

The system generates a critical alarm based on an OPC A&C event. According to configured routing rules, the Alarm & Notification Service constructs a specific payload and sends it via a webhook to an external system, such as PagerDuty or Slack, to alert on-call personnel.

## 1.4 Type

🔹 IntegrationFlow

## 1.5 Purpose

To integrate with external incident management and communication platforms, enabling flexible and timely alerting for critical system events as per REQ-FR-003.

## 1.6 Complexity

Medium

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-SVC-ANM
- PagerDuty

## 1.10 Key Interactions

- The Alarm & Notification Service processes a new high-priority alarm event.
- The service looks up notification routing rules matching the alarm's priority, area, and type.
- It identifies a rule configured for a PagerDuty webhook.
- The service formats an alarm payload according to the PagerDuty Events API v2 specification.
- It makes an HTTPS POST request to the configured PagerDuty webhook URL with the payload.

## 1.11 Triggers

- An alarm is generated that matches a pre-configured notification rule pointing to an external system.

## 1.12 Outcomes

- An incident is created in PagerDuty.
- On-call personnel are notified through their defined escalation policies in PagerDuty.

## 1.13 Business Rules

- Notification routing must be configurable based on alarm priority, area, type, or on-call schedules (REQ-FR-003).
- The system must support webhooks for integration.

## 1.14 Error Scenarios

- The PagerDuty API is unavailable or returns an error (e.g., 4xx, 5xx).
- The webhook URL is misconfigured.
- Network issues prevent the outgoing HTTPS request.

## 1.15 Integration Points

- PagerDuty API
- Slack Webhooks
- Twilio API (SMS)
- SendGrid API (Email)

# 2.0 Details

## 2.1 Diagram Id

SEQ-IF-001

## 2.2 Name

Route Critical Alarm Notification to PagerDuty via Webhook

## 2.3 Description

Implementation sequence for processing a critical internal alarm, looking up a configured notification rule, securely retrieving credentials, transforming the alarm data into the PagerDuty Events API v2 format, and dispatching it via an HTTPS webhook. The sequence incorporates resilience patterns such as Retry and Circuit Breaker for robust integration.

## 2.4 Participants

### 2.4.1 Microservice

#### 2.4.1.1 Repository Id

REPO-SVC-ANM

#### 2.4.1.2 Display Name

Alarm & Notification Service

#### 2.4.1.3 Type

🔹 Microservice

#### 2.4.1.4 Technology

.NET 8, ASP.NET Core

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | «Service» |

### 2.4.2.0 Database

#### 2.4.2.1 Repository Id

db-postgres-012

#### 2.4.2.2 Display Name

PostgreSQL Database

#### 2.4.2.3 Type

🔹 Database

#### 2.4.2.4 Technology

PostgreSQL 16 on AWS RDS

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #4682B4 |
| Stereotype | «Relational Store» |

### 2.4.3.0 CloudService

#### 2.4.3.1 Repository Id

aws-secrets-manager

#### 2.4.3.2 Display Name

AWS Secrets Manager

#### 2.4.3.3 Type

🔹 CloudService

#### 2.4.3.4 Technology

AWS SDK

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | cloud |
| Color | #FF8C00 |
| Stereotype | «Secret Store» |

### 2.4.4.0 ExternalSystem

#### 2.4.4.1 Repository Id

pagerduty-api

#### 2.4.4.2 Display Name

PagerDuty API

#### 2.4.4.3 Type

🔹 ExternalSystem

#### 2.4.4.4 Technology

REST/JSON API

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #32CD32 |
| Stereotype | «External API» |

## 2.5.0.0 Interactions

### 2.5.1.0 InternalProcessing

#### 2.5.1.1 Source Id

REPO-SVC-ANM

#### 2.5.1.2 Target Id

REPO-SVC-ANM

#### 2.5.1.3 Message

1. processCriticalAlarm(alarmEvent)

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 InternalProcessing

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

❌ No

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Method Call |
| Method | processCriticalAlarm |
| Parameters | AlarmEvent object containing Priority, Area, Type,... |
| Authentication | N/A |
| Error Handling | Logs error if alarmEvent is null or malformed. |

### 2.5.2.0 DatabaseQuery

#### 2.5.2.1 Source Id

REPO-SVC-ANM

#### 2.5.2.2 Target Id

db-postgres-012

#### 2.5.2.3 Message

2. SELECT * FROM NotificationRules WHERE ...

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 DatabaseQuery

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

3. Return matching PagerDutyNotificationRule

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

❌ No

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | SELECT |
| Parameters | tenant_id, alarm_priority, alarm_area, alarm_type.... |
| Authentication | Database connection string with credentials. |
| Error Handling | Handles DB connection errors or timeouts. Logs err... |

### 2.5.3.0 API Call

#### 2.5.3.1 Source Id

REPO-SVC-ANM

#### 2.5.3.2 Target Id

aws-secrets-manager

#### 2.5.3.3 Message

4. getSecretValue(secretId: rule.pagerDutySecretId)

#### 2.5.3.4 Sequence Number

4

#### 2.5.3.5 Type

🔹 API Call

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

5. Return PagerDuty Integration Key

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

❌ No

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | GetSecretValue |
| Parameters | The ARN of the secret containing the PagerDuty int... |
| Authentication | AWS IAM Role associated with the service's Kuberne... |
| Error Handling | Handles AWS SDK exceptions (e.g., ResourceNotFound... |

### 2.5.4.0 DataTransformation

#### 2.5.4.1 Source Id

REPO-SVC-ANM

#### 2.5.4.2 Target Id

REPO-SVC-ANM

#### 2.5.4.3 Message

6. transformAlarmToPagerDutyPayload()

#### 2.5.4.4 Sequence Number

6

#### 2.5.4.5 Type

🔹 DataTransformation

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

7. PagerDuty Events API v2 JSON Payload

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Method Call |
| Method | transformAlarmToPagerDutyPayload |
| Parameters | Internal AlarmEvent object, PagerDuty routing key. |
| Authentication | N/A |
| Error Handling | Validates that all required fields for the PagerDu... |

### 2.5.5.0 API Call

#### 2.5.5.1 Source Id

REPO-SVC-ANM

#### 2.5.5.2 Target Id

pagerduty-api

#### 2.5.5.3 Message

8. POST /v2/enqueue (payload)

#### 2.5.5.4 Sequence Number

8

#### 2.5.5.5 Type

🔹 API Call

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

9. HTTP/1.1 202 Accepted

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | POST |
| Parameters | JSON body conforming to PagerDuty Events API v2. H... |
| Authentication | The `routing_key` inside the JSON payload acts as ... |
| Error Handling | Wrapped in a Polly Resilience Pipeline (Retry + Ci... |

### 2.5.6.0 InternalProcessing

#### 2.5.6.1 Source Id

REPO-SVC-ANM

#### 2.5.6.2 Target Id

REPO-SVC-ANM

#### 2.5.6.3 Message

10. updateNotificationStatus(status='Success')

#### 2.5.6.4 Sequence Number

10

#### 2.5.6.5 Type

🔹 InternalProcessing

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message



#### 2.5.6.8 Has Return

❌ No

#### 2.5.6.9 Is Activation

❌ No

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Method Call |
| Method | updateNotificationStatus |
| Parameters | alarmId, ruleId, status, responseDetails. |
| Authentication | N/A |
| Error Handling | Logs the successful delivery for auditing purposes... |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content



```
Data Transformation Logic:
Maps internal AlarmEvent properties to PagerDuty's schema.
- AlarmEvent.Message -> payload.summary
- AlarmEvent.Severity -> payload.severity ('critical', 'error')
- AlarmEvent.Source -> payload.source
- AlarmEvent.AssetId -> custom_details.asset_id
```

#### 2.6.1.2 Position

bottom

#### 2.6.1.3 Participant Id

REPO-SVC-ANM

#### 2.6.1.4 Sequence Number

6

### 2.6.2.0 Content

#### 2.6.2.1 Content



```
Resilience Strategy:
- Retry Policy: Exponential backoff (1s, 2s, 4s) for 3 attempts on HTTP 5xx or transient network errors.
- Circuit Breaker: Opens for 30s after 5 consecutive failures. Prevents cascading failure.
```

#### 2.6.2.2 Position

right

#### 2.6.2.3 Participant Id

pagerduty-api

#### 2.6.2.4 Sequence Number

8

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | All communication with the PagerDuty API must use ... |
| Performance Targets | End-to-end latency from alarm generation to PagerD... |
| Error Handling Strategy | If the PagerDuty API returns a 4xx status code (e.... |
| Testing Considerations | Integration tests must use a mock HTTP server (e.g... |
| Monitoring Requirements | The following metrics must be exposed to Prometheu... |
| Deployment Considerations | The PagerDuty webhook URL and the AWS Secrets Mana... |

