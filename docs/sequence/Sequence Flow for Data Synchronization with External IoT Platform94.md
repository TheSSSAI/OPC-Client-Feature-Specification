# 1 Overview

## 1.1 Diagram Id

SEQ-IF-004

## 1.2 Name

Data Synchronization with External IoT Platform

## 1.3 Description

The system is configured to share data with an external cloud IoT platform like Azure IoT Hub. A dedicated integration module queries for new data, transforms it using a user-defined mapping tool to match the target platform's schema, and publishes it to the platform's endpoint (e.g., via MQTT or HTTP).

## 1.4 Type

🔹 IntegrationFlow

## 1.5 Purpose

To enable bidirectional data flow between the OPC system and major IoT platforms, facilitating broader enterprise integration and analytics as per REQ-FR-017.

## 1.6 Complexity

High

## 1.7 Priority

🟡 Medium

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-SVC-INT
- REPO-SVC-DQR
- Azure IoT Hub

## 1.10 Key Interactions

- A background job in a dedicated Integration Service (REPO-SVC-INT) runs periodically or is triggered by events.
- It queries the Query Service for new data since its last execution.
- For each data point, it applies the stored transformation rules defined in the data mapping tool.
- It authenticates with the external IoT platform (e.g., Azure IoT Hub) using stored credentials.
- It sends the transformed data payload to the platform's API endpoint.

## 1.11 Triggers

- A scheduled job or a real-time event triggers the data export process.

## 1.12 Outcomes

- OPC data is successfully transmitted to and available for use in the external IoT Platform.
- The data structure in the target platform matches the desired schema defined by the user.

## 1.13 Business Rules

- The integration must be configurable for bidirectional data flow (OPC-to-Cloud and Cloud-to-OPC) (REQ-FR-017).
- A data mapping and transformation tool must be provided to align data structures (REQ-FR-017).

## 1.14 Error Scenarios

- The external platform is unavailable or rejects the data due to schema mismatch.
- Authentication with the external platform fails due to invalid credentials.
- A transformation rule fails due to unexpected data types or values.

## 1.15 Integration Points

- AWS IoT
- Azure IoT
- Google Cloud IoT

# 2.0 Details

## 2.1 Diagram Id

SEQ-IF-004

## 2.2 Name

Implementation: Data Synchronization with External IoT Platform

## 2.3 Description

Technical sequence for a scheduled job within the Integration Service that extracts, transforms, and loads data to an external IoT platform (Azure IoT Hub). This sequence implements the Adapter and Circuit Breaker patterns for resilient, decoupled external communication.

## 2.4 Participants

### 2.4.1 Actor

#### 2.4.1.1 Repository Id

Scheduler

#### 2.4.1.2 Display Name

Scheduler (K8s CronJob)

#### 2.4.1.3 Type

🔹 Actor

#### 2.4.1.4 Technology

Kubernetes CronJob

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | Actor |
| Color | #90A4AE |
| Stereotype | System Trigger |

### 2.4.2.0 Microservice

#### 2.4.2.1 Repository Id

REPO-SVC-INT

#### 2.4.2.2 Display Name

Integration Service

#### 2.4.2.3 Type

🔹 Microservice

#### 2.4.2.4 Technology

.NET 8, ASP.NET Core

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | Component |
| Color | #4DD0E1 |
| Stereotype | Adapter |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-DQR

#### 2.4.3.2 Display Name

Query Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8, ASP.NET Core

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | Component |
| Color | #81C784 |
| Stereotype | Service |

### 2.4.4.0 External System

#### 2.4.4.1 Repository Id

Azure_IoT_Hub

#### 2.4.4.2 Display Name

Azure IoT Hub

#### 2.4.4.3 Type

🔹 External System

#### 2.4.4.4 Technology

Azure PaaS, REST API (MQTT optional)

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | Boundary |
| Color | #7986CB |
| Stereotype | External API |

## 2.5.0.0 Interactions

### 2.5.1.0 Invocation

#### 2.5.1.1 Source Id

Scheduler

#### 2.5.1.2 Target Id

REPO-SVC-INT

#### 2.5.1.3 Message

Invoke Job: POST /jobs/iot-sync/trigger

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Invocation

#### 2.5.1.6 Is Synchronous

❌ No

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

❌ No

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 (Internal Cluster IP) |
| Method | POST /jobs/iot-sync/trigger |
| Parameters | {} |
| Authentication | Kubernetes RBAC |
| Error Handling | Job scheduler manages retries on failure. |
| Performance | N/A |

### 2.5.2.0 Internal Processing

#### 2.5.2.1 Source Id

REPO-SVC-INT

#### 2.5.2.2 Target Id

REPO-SVC-INT

#### 2.5.2.3 Message

[Internal] Retrieve Last Sync Watermark

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Internal Processing

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

lastSyncTimestamp

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

❌ No

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Database Call (Npgsql) |
| Method | SELECT value FROM service_state WHERE key = 'lastS... |
| Parameters | key: 'lastSyncTimestamp' |
| Authentication | Database credentials from Secrets Manager |
| Error Handling | Fail job if state cannot be retrieved. |
| Performance | <10ms |

### 2.5.3.0 API Call

#### 2.5.3.1 Source Id

REPO-SVC-INT

#### 2.5.3.2 Target Id

REPO-SVC-DQR

#### 2.5.3.3 Message

Query for new data: GET /api/v1/query/data?since={ts}

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 API Call

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

200 OK: { data: [...] }

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 |
| Method | GET /api/v1/query/data |
| Parameters | since={lastSyncTimestamp} |
| Authentication | Propagated JWT Bearer Token (Service Account) |
| Error Handling | Retry (3x, exponential backoff) on 5xx errors. Log... |
| Performance | SLA < 2000ms |

### 2.5.4.0 Internal Processing

#### 2.5.4.1 Source Id

REPO-SVC-INT

#### 2.5.4.2 Target Id

REPO-SVC-INT

#### 2.5.4.3 Message

[Loop] Apply Transformation Rules

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 Internal Processing

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

transformedPayload[]

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | In-Memory |
| Method | transform(data, mappingRules) |
| Parameters | Raw data array, cached mapping rules |
| Authentication | N/A |
| Error Handling | Log transformation errors for individual records a... |
| Performance | Must process >1000 records/sec in memory. |

### 2.5.5.0 API Call

#### 2.5.5.1 Source Id

REPO-SVC-INT

#### 2.5.5.2 Target Id

Azure_IoT_Hub

#### 2.5.5.3 Message

Send transformed data batch: POST /devices/{devId}/messages/events

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 API Call

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

204 No Content

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

✅ Yes

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 over TLS 1.3 |
| Method | POST /devices/{deviceId}/messages/events?api-versi... |
| Parameters | JSON payload in request body. |
| Authentication | Authorization header with generated SAS Token (fro... |
| Error Handling | Polly Circuit Breaker: break after 5 consecutive f... |
| Performance | Latency < 500ms per batch call. |

### 2.5.6.0 Internal Processing

#### 2.5.6.1 Source Id

REPO-SVC-INT

#### 2.5.6.2 Target Id

REPO-SVC-INT

#### 2.5.6.3 Message

[Internal] Update Last Sync Watermark

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 Internal Processing

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message

Success

#### 2.5.6.8 Has Return

✅ Yes

#### 2.5.6.9 Is Activation

❌ No

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Database Call (Npgsql) |
| Method | UPDATE service_state SET value = {newTimestamp} WH... |
| Parameters | newTimestamp: timestamp of last successfully proce... |
| Authentication | Database credentials from Secrets Manager |
| Error Handling | Critical failure. Log error and alert. The next jo... |
| Performance | <10ms |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

REQ-FR-017: The transformation logic in step 4 is driven by user-configurable mapping rules stored in the Integration Service's database and cached in memory for performance.

#### 2.6.1.2 Position

Top

#### 2.6.1.3 Participant Id

*Not specified*

#### 2.6.1.4 Sequence Number

4

### 2.6.2.0 Content

#### 2.6.2.1 Content

The Circuit Breaker pattern is critical here to prevent the Integration Service from overwhelming a struggling external API and to fail fast, preserving system resources.

#### 2.6.2.2 Position

Right

#### 2.6.2.3 Participant Id

Azure_IoT_Hub

#### 2.6.2.4 Sequence Number

5

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Azure IoT Hub credentials (keys or connection stri... |
| Performance Targets | The entire job should be able to process and trans... |
| Error Handling Strategy | A robust dead-letter queue (DLQ) mechanism must be... |
| Testing Considerations | Develop integration tests that use a mock HTTP ser... |
| Monitoring Requirements | The Integration Service must expose Prometheus met... |
| Deployment Considerations | The Integration Service should be deployed as a Ku... |

