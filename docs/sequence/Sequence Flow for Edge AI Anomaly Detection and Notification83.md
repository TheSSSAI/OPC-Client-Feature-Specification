# 1 Overview

## 1.1 Diagram Id

SEQ-FF-001

## 1.2 Name

Edge AI Anomaly Detection and Notification

## 1.3 Description

The OPC Core Client reads a real-time data stream for a configured asset. It passes the data to a locally deployed ONNX model for inference. The model detects an anomalous pattern, and the client sends an 'AnomalyDetected' event to the Central Management Plane via MQTT, which then triggers a system notification.

## 1.4 Type

🔹 FeatureFlow

## 1.5 Purpose

To perform low-latency AI-driven anomaly detection at the edge, enabling rapid identification of potential issues without the delay of a cloud round-trip, as specified in REQ-FR-013.

## 1.6 Complexity

High

## 1.7 Priority

🔴 High

## 1.8 Frequency

Continuous

## 1.9 Participants

- REPO-EDGE-OPC
- MQTT Broker
- REPO-SVC-ANM

## 1.10 Key Interactions

- The client receives a new data value for a tag that has an AI model assigned.
- The Edge AI module runs the value through the assigned ONNX model using the ONNX Runtime.
- The model's output score exceeds the configured anomaly threshold.
- The client constructs an 'AnomalyDetected' event payload with details like the model ID, score, and triggering value.
- The client publishes the event to the `tenants/{id}/events/anomaly` MQTT topic.
- The Alarm & Notification Service, subscribed to this topic, receives the event and processes it to generate a system alarm and notify users.

## 1.11 Triggers

- Real-time data for a monitored asset deviates from the AI model's learned normal pattern.

## 1.12 Outcomes

- An anomaly alert is generated in the Central Management Plane.
- Operators are notified of the potential issue based on their preferences.
- Operators can provide feedback on the anomaly (true/false positive) which is logged for retraining (REQ-FR-013).

## 1.13 Business Rules

- Anomaly detection models must be deployed and executed on the Edge AI Module for real-time performance (REQ-FR-013).
- User feedback on flagged anomalies shall be logged for use in future model retraining (REQ-FR-013).

## 1.14 Error Scenarios

- The ONNX model fails to load or execute due to format or hardware issues.
- The client is offline and cannot send the event to the cloud immediately (it may be buffered).
- The Alarm Service fails to process the incoming event from the MQTT broker.

## 1.15 Integration Points

- ONNX Runtime

# 2.0 Details

## 2.1 Diagram Id

SEQ-FF-001

## 2.2 Name

Implementation: Edge AI Anomaly Detection and Notification

## 2.3 Description

Technical sequence for the feature where the OPC Core Client performs local AI/ML inference on a real-time data point. If the model's output exceeds a configured threshold, an 'AnomalyDetected' event is published via MQTT to the Central Management Plane, where the Alarm & Notification Service processes it into a system alarm and triggers user notifications. This sequence emphasizes the low-latency edge processing and the event-driven handoff to the cloud.

## 2.4 Participants

### 2.4.1 Edge Application

#### 2.4.1.1 Repository Id

REPO-EDGE-OPC

#### 2.4.1.2 Display Name

OPC Core Client

#### 2.4.1.3 Type

🔹 Edge Application

#### 2.4.1.4 Technology

.NET 8, Docker, ONNX Runtime

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E88E5 |
| Stereotype | Edge |

### 2.4.2.0 Messaging Infrastructure

#### 2.4.2.1 Repository Id

MQTT Broker

#### 2.4.2.2 Display Name

MQTT Broker

#### 2.4.2.3 Type

🔹 Messaging Infrastructure

#### 2.4.2.4 Technology

MQTT v5 (e.g., AWS IoT Core)

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #7E57C2 |
| Stereotype | Broker |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-ANM

#### 2.4.3.2 Display Name

Alarm & Notification Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8, ASP.NET Core

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #D81B60 |
| Stereotype | Service |

### 2.4.4.0 Microservice

#### 2.4.4.1 Repository Id

REPO-SVC-AST

#### 2.4.4.2 Display Name

Asset & Topology Service

#### 2.4.4.3 Type

🔹 Microservice

#### 2.4.4.4 Technology

.NET 8, ASP.NET Core

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #00897B |
| Stereotype | Service |

### 2.4.5.0 Database

#### 2.4.5.1 Repository Id

cmp_relational_data

#### 2.4.5.2 Display Name

Relational Database

#### 2.4.5.3 Type

🔹 Database

#### 2.4.5.4 Technology

PostgreSQL on RDS

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #3949AB |
| Stereotype | Persistence |

## 2.5.0.0 Interactions

### 2.5.1.0 Internal Processing

#### 2.5.1.1 Source Id

REPO-EDGE-OPC

#### 2.5.1.2 Target Id

REPO-EDGE-OPC

#### 2.5.1.3 Message

Receives real-time data value (e.g., Temperature=45.3) for a tag associated with a configured AI model.

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Internal Processing

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Has Return

❌ No

#### 2.5.1.8 Is Activation

✅ Yes

#### 2.5.1.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | OPC UA Subscription |
| Method | DataChange Notification |
| Parameters | MonitoredItem with value, quality, and timestamp. |
| Authentication | OPC UA Security Policy (e.g., Basic256Sha256). |
| Error Handling | Handle OPC data quality flags (Bad, Uncertain). |
| Performance | Driven by OPC server update rate. |

### 2.5.2.0 AI/ML Inference

#### 2.5.2.1 Source Id

REPO-EDGE-OPC

#### 2.5.2.2 Target Id

REPO-EDGE-OPC

#### 2.5.2.3 Message

Executes local ONNX model inference using the received value.

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 AI/ML Inference

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

Anomaly Score (e.g., 0.98)

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

❌ No

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | In-Process Call |
| Method | ONNX Runtime Session.Run() |
| Parameters | Input tensor with real-time value. |
| Authentication | N/A |
| Error Handling | Log error if model file is corrupt or fails to exe... |
| Performance | Must complete in < 100ms as per REQ-NFR-001. |

### 2.5.3.0 Conditional Logic

#### 2.5.3.1 Source Id

REPO-EDGE-OPC

#### 2.5.3.2 Target Id

REPO-EDGE-OPC

#### 2.5.3.3 Message

alt: [ score > threshold (e.g., 0.98 > 0.95) ]

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Conditional Logic

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Has Return

❌ No

#### 2.5.3.8 Is Activation

❌ No

#### 2.5.3.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Logic |
| Method | Threshold Comparison |
| Parameters | Model output score, configured anomaly threshold. |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Negligible. |

#### 2.5.3.10 Nested Interactions

- {'sourceId': 'REPO-EDGE-OPC', 'targetId': 'MQTT Broker', 'message': "Publishes 'AnomalyDetected' event.", 'sequenceNumber': 4, 'type': 'Event Publishing', 'isSynchronous': False, 'hasReturn': False, 'isActivation': False, 'technicalDetails': {'protocol': 'MQTT v5 over TLS 1.3', 'method': 'PUBLISH', 'parameters': {'topic': 'tenants/{tenantId}/events/anomaly', 'QoS': 1, 'payload': {'version': '1.0', 'modelAssignmentId': 'uuid-for-model-asset-link', 'timestamp': 'ISO8601 UTC', 'anomalyScore': 0.98, 'triggeringValue': 45.3, 'correlationId': 'uuid-trace-id'}}, 'authentication': 'mTLS with Client Certificate', 'errorHandling': 'If publish fails due to network disconnect, message is queued in the on-disk buffer (REQ-NFR-002) and retried with exponential backoff upon reconnection.', 'performance': 'Low latency network operation.'}}

### 2.5.4.0 Event Delivery

#### 2.5.4.1 Source Id

MQTT Broker

#### 2.5.4.2 Target Id

REPO-SVC-ANM

#### 2.5.4.3 Message

Delivers 'AnomalyDetected' event.

#### 2.5.4.4 Sequence Number

5

#### 2.5.4.5 Type

🔹 Event Delivery

#### 2.5.4.6 Is Synchronous

❌ No

#### 2.5.4.7 Has Return

❌ No

#### 2.5.4.8 Is Activation

✅ Yes

#### 2.5.4.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 |
| Method | MESSAGE Delivery |
| Parameters | Subscribed topic receives the message payload. |
| Authentication | Broker authorizes service subscription. |
| Error Handling | Service maintains persistent connection to broker.... |
| Performance | Dependent on broker and network latency. |

### 2.5.5.0 API Call

#### 2.5.5.1 Source Id

REPO-SVC-ANM

#### 2.5.5.2 Target Id

REPO-SVC-AST

#### 2.5.5.3 Message

Requests asset context for the model assignment.

#### 2.5.5.4 Sequence Number

6

#### 2.5.5.5 Type

🔹 API Call

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

200 OK with { assetId, tenantId, modelName }

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | GET /api/v1/assignments/{modelAssignmentId} |
| Parameters | Path parameter: modelAssignmentId from event paylo... |
| Authentication | JWT Bearer Token |
| Error Handling | Retry on 5xx status codes. Utilize circuit breaker... |
| Performance | P95 latency < 200ms. |

### 2.5.6.0 Database Write

#### 2.5.6.1 Source Id

REPO-SVC-ANM

#### 2.5.6.2 Target Id

cmp_relational_data

#### 2.5.6.3 Message

Persists a new 'Alarm' record and logs the anomaly for user feedback.

#### 2.5.6.4 Sequence Number

7

#### 2.5.6.5 Type

🔹 Database Write

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message

Success confirmation

#### 2.5.6.8 Has Return

✅ Yes

#### 2.5.6.9 Is Activation

❌ No

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | INSERT INTO alarms; INSERT INTO anomaly_feedback_l... |
| Parameters | Alarm details: tenantId, assetId, description, sev... |
| Authentication | Database credentials from AWS Secrets Manager. |
| Error Handling | Retry on transient connection errors. If final wri... |
| Performance | Standard DB write latency. |

### 2.5.7.0 Asynchronous Task

#### 2.5.7.1 Source Id

REPO-SVC-ANM

#### 2.5.7.2 Target Id

REPO-SVC-ANM

#### 2.5.7.3 Message

[Async] Triggers notification workflow based on user preferences.

#### 2.5.7.4 Sequence Number

8

#### 2.5.7.5 Type

🔹 Asynchronous Task

#### 2.5.7.6 Is Synchronous

❌ No

#### 2.5.7.7 Has Return

❌ No

#### 2.5.7.8 Is Activation

❌ No

#### 2.5.7.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | In-Process Task Dispatch |
| Method | QueueBackgroundWorkItem |
| Parameters | Alarm ID, Tenant ID. |
| Authentication | N/A |
| Error Handling | Handled by the background job processor with its o... |
| Performance | The dispatch is immediate; the notification delive... |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Edge Processing (Steps 1-3) is latency-critical and designed for autonomous operation during network outages, as per REQ-FR-013.

#### 2.6.1.2 Position

Top

#### 2.6.1.3 Participant Id

REPO-EDGE-OPC

#### 2.6.1.4 Sequence Number

1

### 2.6.2.0 Content

#### 2.6.2.1 Content

Event Publishing (Step 4) is decoupled. The OPC Client's responsibility ends after successfully queuing the message for the MQTT broker. Reliability is handled by MQTT QoS 1 and the client's offline buffer.

#### 2.6.2.2 Position

Middle

#### 2.6.2.3 Participant Id

MQTT Broker

#### 2.6.2.4 Sequence Number

4

### 2.6.3.0 Content

#### 2.6.3.1 Content

Cloud processing is designed to be resilient. Failure to get context or persist the alarm results in the event being moved to a Dead-Letter Queue (DLQ) for manual intervention, preventing data loss.

#### 2.6.3.2 Position

Bottom

#### 2.6.3.3 Participant Id

REPO-SVC-ANM

#### 2.6.3.4 Sequence Number

6

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Communication between REPO-EDGE-OPC and the MQTT B... |
| Performance Targets | The end-to-end latency for steps 1-3 (data receipt... |
| Error Handling Strategy | The OPC Core Client must handle ONNX model executi... |
| Testing Considerations | An end-to-end test must be created that uses a moc... |
| Monitoring Requirements | The OPC Core Client must expose Prometheus metrics... |
| Deployment Considerations | The ONNX models must be packaged with the OPC Core... |

