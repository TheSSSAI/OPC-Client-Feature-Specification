# 1 Overview

## 1.1 Diagram Id

SEQ-RF-001

## 1.2 Name

OPC Client Offline Buffering and Data Recovery

## 1.3 Description

An OPC Core Client loses its network connection to the Central Management Plane. It enters autonomous mode, continuing to collect data from local OPC servers and storing it in a persistent on-disk buffer. Upon reconnection, it automatically forwards all buffered data to the Data Ingestion Service before resuming real-time streaming.

## 1.4 Type

🔹 RecoveryFlow

## 1.5 Purpose

To ensure zero data loss during periods of network instability or outages at the edge, guaranteeing data durability and system reliability as mandated by REQ-NFR-002.

## 1.6 Complexity

High

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-EDGE-OPC
- REPO-SVC-DVM
- REPO-SVC-DIN

## 1.10 Key Interactions

- Client's gRPC/MQTT connection to the cloud fails its health check.
- The client's Cloud Communication Module transitions to an 'offline' state.
- The client begins writing all newly collected data points to its on-disk circular buffer.
- The client periodically attempts to re-establish the cloud connection.
- Upon successful reconnection, the client reads data from the buffer in chronological order and streams it to the Ingestion Service.
- Once the buffer is empty, the client switches back to normal real-time streaming mode.

## 1.11 Triggers

- Loss of network connectivity between the edge client and the cloud platform.

## 1.12 Outcomes

- No data is lost during the network outage.
- The central data store is eventually consistent with all data collected at the edge once connectivity is restored.

## 1.13 Business Rules

- The buffer must be a persistent, on-disk circular buffer to survive client restarts (REQ-NFR-002).
- An alert must be logged in the Central Management Plane if the buffer becomes full, indicating that the oldest data is now being overwritten (REQ-NFR-002).
- The buffer size must be configurable.

## 1.14 Error Scenarios

- Disk space for the buffer is exhausted.
- A prolonged outage causes the circular buffer to overwrite data that has not yet been sent.
- Forwarded historical data is rejected by the ingestion service upon reconnection.

## 1.15 Integration Points

*No items available*

# 2.0 Details

## 2.1 Diagram Id

SEQ-RF-001

## 2.2 Name

OPC Core Client Offline Buffering and Data Recovery Flow

## 2.3 Description

This sequence details the autonomous recovery mechanism of an OPC Core Client experiencing a network outage. It illustrates the transition to an offline state, the use of a persistent on-disk circular buffer for data collection continuity, and the automated, ordered forwarding of all buffered data to the Data Ingestion Service upon network restoration, ensuring zero data loss as per REQ-NFR-002.

## 2.4 Participants

### 2.4.1 External System

#### 2.4.1.1 Repository Id

System.Boundary

#### 2.4.1.2 Display Name

Network Boundary

#### 2.4.1.3 Type

🔹 External System

#### 2.4.1.4 Technology

TCP/IP Network

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #999999 |
| Stereotype | Boundary |

### 2.4.2.0 EdgeApplication

#### 2.4.2.1 Repository Id

REPO-EDGE-OPC

#### 2.4.2.2 Display Name

OPC Core Client

#### 2.4.2.3 Type

🔹 EdgeApplication

#### 2.4.2.4 Technology

.NET 8, Docker

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #1E90FF |
| Stereotype | Edge |

### 2.4.3.0 Service

#### 2.4.3.1 Repository Id

REPO-SVC-DIN

#### 2.4.3.2 Display Name

Data Ingestion Service

#### 2.4.3.3 Type

🔹 Service

#### 2.4.3.4 Technology

.NET 8, gRPC

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #2E8B57 |
| Stereotype | Cloud Service |

### 2.4.4.0 Service

#### 2.4.4.1 Repository Id

REPO-SVC-DVM

#### 2.4.4.2 Display Name

Device Management Service

#### 2.4.4.3 Type

🔹 Service

#### 2.4.4.4 Technology

.NET 8, MQTT

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #4682B4 |
| Stereotype | Cloud Service |

## 2.5.0.0 Interactions

### 2.5.1.0 DataStreaming

#### 2.5.1.1 Source Id

REPO-EDGE-OPC

#### 2.5.1.2 Target Id

REPO-SVC-DIN

#### 2.5.1.3 Message

1. [Normal Operation] StreamRealTimeData()

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 DataStreaming

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

Continuous stream of data points

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | StreamRealTimeData |
| Parameters | Bi-directional stream of DataPoint messages |
| Authentication | Requires active mTLS session |
| Error Handling | Stream will be terminated on connection loss |
| Performance | Latency < 500ms end-to-end |

### 2.5.2.0 SystemFailure

#### 2.5.2.1 Source Id

System.Boundary

#### 2.5.2.2 Target Id

REPO-EDGE-OPC

#### 2.5.2.3 Message

2. Network connection is lost

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 SystemFailure

#### 2.5.2.6 Is Synchronous

❌ No

#### 2.5.2.7 Has Return

❌ No

#### 2.5.2.8 Is Activation

❌ No

#### 2.5.2.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Network |
| Method | Connection Drop |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | TCP timeouts, gRPC UNAVAILABLE status |
| Performance | N/A |

### 2.5.3.0 InternalLogic

#### 2.5.3.1 Source Id

REPO-EDGE-OPC

#### 2.5.3.2 Target Id

REPO-EDGE-OPC

#### 2.5.3.3 Message

3. gRPC stream health check fails after exhausting retry policy

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 InternalLogic

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

ConnectionFailureException

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | CloudCommunicationModule.HealthCheck() |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | Retry with exponential backoff fails; triggers sta... |
| Performance | N/A |

### 2.5.4.0 StateTransition

#### 2.5.4.1 Source Id

REPO-EDGE-OPC

#### 2.5.4.2 Target Id

REPO-EDGE-OPC

#### 2.5.4.3 Message

4. transitionToOfflineState()

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 StateTransition

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Has Return

❌ No

#### 2.5.4.8 Is Activation

✅ Yes

#### 2.5.4.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | StateManager.SetState(OFFLINE) |
| Parameters | New state: OFFLINE |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate |

#### 2.5.4.10 Nested Interactions

##### 2.5.4.10.1 Loop

###### 2.5.4.10.1.1 Source Id

REPO-EDGE-OPC

###### 2.5.4.10.1.2 Target Id

REPO-EDGE-OPC

###### 2.5.4.10.1.3 Message

4.1. [Loop] For each new data point from OPC Server:

###### 2.5.4.10.1.4 Sequence Number

4.1

###### 2.5.4.10.1.5 Type

🔹 Loop

###### 2.5.4.10.1.6 Is Synchronous

✅ Yes

###### 2.5.4.10.1.7 Has Return

❌ No

###### 2.5.4.10.1.8 Technical Details

*No data available*

###### 2.5.4.10.1.9 Nested Interactions

- {'sourceId': 'REPO-EDGE-OPC', 'targetId': 'REPO-EDGE-OPC', 'message': '4.1.1. writeDataToBuffer(dataPoint)', 'sequenceNumber': 4.11, 'type': 'DataAccess', 'isSynchronous': True, 'hasReturn': False, 'technicalDetails': {'protocol': 'File I/O', 'method': 'PersistentBuffer.Write()', 'parameters': 'Serialized DataPoint object', 'authentication': 'N/A', 'errorHandling': "If disk is full, log critical error and increment 'buffer_full_events_total' metric. Overwrite oldest data (circular buffer behavior).", 'performance': 'Write latency < 10ms'}}

##### 2.5.4.10.2.0 Loop

###### 2.5.4.10.2.1 Source Id

REPO-EDGE-OPC

###### 2.5.4.10.2.2 Target Id

REPO-EDGE-OPC

###### 2.5.4.10.2.3 Message

4.2. [Loop] Periodically attemptReconnection()

###### 2.5.4.10.2.4 Sequence Number

4.2

###### 2.5.4.10.2.5 Type

🔹 Loop

###### 2.5.4.10.2.6 Is Synchronous

✅ Yes

###### 2.5.4.10.2.7 Has Return

❌ No

###### 2.5.4.10.2.8 Technical Details

*No data available*

### 2.5.5.0.0.0 SystemEvent

#### 2.5.5.1.0.0 Source Id

System.Boundary

#### 2.5.5.2.0.0 Target Id

REPO-EDGE-OPC

#### 2.5.5.3.0.0 Message

5. Network connection is restored

#### 2.5.5.4.0.0 Sequence Number

5

#### 2.5.5.5.0.0 Type

🔹 SystemEvent

#### 2.5.5.6.0.0 Is Synchronous

❌ No

#### 2.5.5.7.0.0 Has Return

❌ No

#### 2.5.5.8.0.0 Is Activation

❌ No

#### 2.5.5.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Network |
| Method | Connection Restore |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | N/A |

### 2.5.6.0.0.0 HealthCheck

#### 2.5.6.1.0.0 Source Id

REPO-EDGE-OPC

#### 2.5.6.2.0.0 Target Id

REPO-SVC-DIN

#### 2.5.6.3.0.0 Message

6. Reconnection attempt succeeds

#### 2.5.6.4.0.0 Sequence Number

6

#### 2.5.6.5.0.0 Type

🔹 HealthCheck

#### 2.5.6.6.0.0 Is Synchronous

✅ Yes

#### 2.5.6.7.0.0 Return Message

OK

#### 2.5.6.8.0.0 Has Return

✅ Yes

#### 2.5.6.9.0.0 Is Activation

✅ Yes

#### 2.5.6.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | grpc.health.v1.Health/Check |
| Parameters | Service name |
| Authentication | mTLS handshake is re-established |
| Error Handling | N/A |
| Performance | Response < 100ms |

### 2.5.7.0.0.0 StateTransition

#### 2.5.7.1.0.0 Source Id

REPO-EDGE-OPC

#### 2.5.7.2.0.0 Target Id

REPO-EDGE-OPC

#### 2.5.7.3.0.0 Message

7. transitionToRecoveryState()

#### 2.5.7.4.0.0 Sequence Number

7

#### 2.5.7.5.0.0 Type

🔹 StateTransition

#### 2.5.7.6.0.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0.0 Has Return

❌ No

#### 2.5.7.8.0.0 Is Activation

✅ Yes

#### 2.5.7.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | StateManager.SetState(RECOVERY) |
| Parameters | New state: RECOVERY |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate |

### 2.5.8.0.0.0 DataStreaming

#### 2.5.8.1.0.0 Source Id

REPO-EDGE-OPC

#### 2.5.8.2.0.0 Target Id

REPO-SVC-DIN

#### 2.5.8.3.0.0 Message

8. StreamBufferedData()

#### 2.5.8.4.0.0 Sequence Number

8

#### 2.5.8.5.0.0 Type

🔹 DataStreaming

#### 2.5.8.6.0.0 Is Synchronous

✅ Yes

#### 2.5.8.7.0.0 Return Message

Stream of batch acknowledgements

#### 2.5.8.8.0.0 Has Return

✅ Yes

#### 2.5.8.9.0.0 Is Activation

✅ Yes

#### 2.5.8.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | StreamBufferedData |
| Parameters | Bi-directional stream, client sends batches, serve... |
| Authentication | mTLS session established in step 6 is used |
| Error Handling | If service rejects a batch (e.g., malformed data),... |
| Performance | High throughput is prioritized. |

#### 2.5.8.11.0.0 Nested Interactions

- {'sourceId': 'REPO-EDGE-OPC', 'targetId': 'REPO-EDGE-OPC', 'message': '8.1. [Loop] While buffer is not empty:', 'sequenceNumber': 8.1, 'type': 'Loop', 'isSynchronous': True, 'hasReturn': False, 'technicalDetails': {}, 'nestedInteractions': [{'sourceId': 'REPO-EDGE-OPC', 'targetId': 'REPO-EDGE-OPC', 'message': '8.1.1. readBatchFromBuffer()', 'sequenceNumber': 8.11, 'type': 'DataAccess', 'isSynchronous': True, 'returnMessage': 'DataPoint[] batch', 'hasReturn': True, 'technicalDetails': {'protocol': 'File I/O', 'method': 'PersistentBuffer.ReadBatch()', 'parameters': 'Batch size (e.g., 1000 records)', 'authentication': 'N/A', 'errorHandling': 'Handles file corruption errors.', 'performance': 'Optimized for sequential reads.'}}, {'sourceId': 'REPO-EDGE-OPC', 'targetId': 'REPO-SVC-DIN', 'message': '8.1.2. Send data batch', 'sequenceNumber': 8.12, 'type': 'DataStreaming', 'isSynchronous': True, 'returnMessage': 'BatchAck', 'hasReturn': True, 'technicalDetails': {'protocol': 'gRPC Stream', 'method': 'WriteAsync(batch)', 'parameters': 'Array of DataPoint messages', 'authentication': 'Uses existing stream auth', 'errorHandling': 'Server returns non-OK status for invalid batches.', 'performance': 'N/A'}}, {'sourceId': 'REPO-EDGE-OPC', 'targetId': 'REPO-EDGE-OPC', 'message': '8.1.3. deleteAcknowledgedBatchFromBuffer()', 'sequenceNumber': 8.13, 'type': 'DataAccess', 'isSynchronous': True, 'hasReturn': False, 'technicalDetails': {'protocol': 'File I/O', 'method': 'PersistentBuffer.Delete()', 'parameters': 'Range of acknowledged records', 'authentication': 'N/A', 'errorHandling': 'N/A', 'performance': 'Efficiently truncates the buffer file.'}}]}

### 2.5.9.0.0.0 StateTransition

#### 2.5.9.1.0.0 Source Id

REPO-EDGE-OPC

#### 2.5.9.2.0.0 Target Id

REPO-EDGE-OPC

#### 2.5.9.3.0.0 Message

9. transitionToOnlineState()

#### 2.5.9.4.0.0 Sequence Number

9

#### 2.5.9.5.0.0 Type

🔹 StateTransition

#### 2.5.9.6.0.0 Is Synchronous

✅ Yes

#### 2.5.9.7.0.0 Has Return

❌ No

#### 2.5.9.8.0.0 Is Activation

✅ Yes

#### 2.5.9.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | StateManager.SetState(ONLINE) |
| Parameters | New state: ONLINE |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate |

### 2.5.10.0.0.0 AsynchronousMessaging

#### 2.5.10.1.0.0 Source Id

REPO-EDGE-OPC

#### 2.5.10.2.0.0 Target Id

REPO-SVC-DVM

#### 2.5.10.3.0.0 Message

10. PublishStatus('Online')

#### 2.5.10.4.0.0 Sequence Number

10

#### 2.5.10.5.0.0 Type

🔹 AsynchronousMessaging

#### 2.5.10.6.0.0 Is Synchronous

❌ No

#### 2.5.10.7.0.0 Has Return

❌ No

#### 2.5.10.8.0.0 Is Activation

✅ Yes

#### 2.5.10.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 |
| Method | client.Publish |
| Parameters | Topic: tenants/{id}/clients/{id}/status, Payload: ... |
| Authentication | TLS with Client Certificate |
| Error Handling | MQTT client handles retries based on QoS level 1. |
| Performance | N/A |

### 2.5.11.0.0.0 DataStreaming

#### 2.5.11.1.0.0 Source Id

REPO-EDGE-OPC

#### 2.5.11.2.0.0 Target Id

REPO-SVC-DIN

#### 2.5.11.3.0.0 Message

11. Resume StreamRealTimeData()

#### 2.5.11.4.0.0 Sequence Number

11

#### 2.5.11.5.0.0 Type

🔹 DataStreaming

#### 2.5.11.6.0.0 Is Synchronous

✅ Yes

#### 2.5.11.7.0.0 Return Message

Continuous stream of data points

#### 2.5.11.8.0.0 Has Return

✅ Yes

#### 2.5.11.9.0.0 Is Activation

✅ Yes

#### 2.5.11.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | StreamRealTimeData |
| Parameters | Bi-directional stream of DataPoint messages |
| Authentication | Requires active mTLS session |
| Error Handling | N/A |
| Performance | Latency < 500ms end-to-end |

## 2.6.0.0.0.0 Notes

### 2.6.1.0.0.0 Content

#### 2.6.1.1.0.0 Content

The on-disk buffer is a circular buffer. If the outage is longer than the buffer's capacity, the oldest data will be overwritten. This constitutes an RPO > 0 for that specific scenario, and an alert is generated.

#### 2.6.1.2.0.0 Position

Top

#### 2.6.1.3.0.0 Participant Id

REPO-EDGE-OPC

#### 2.6.1.4.0.0 Sequence Number

4

### 2.6.2.0.0.0 Content

#### 2.6.2.1.0.0 Content

The client state machine (ONLINE -> OFFLINE -> RECOVERY -> ONLINE) is central to this flow and must be implemented to be thread-safe.

#### 2.6.2.2.0.0 Position

Right

#### 2.6.2.3.0.0 Participant Id

REPO-EDGE-OPC

#### 2.6.2.4.0.0 Sequence Number

4

## 2.7.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Upon reconnection, the gRPC mTLS handshake must be... |
| Performance Targets | The Recovery Point Objective (RPO) is 0 seconds, a... |
| Error Handling Strategy | 1. **Disk Full**: The buffer write operation fails... |
| Testing Considerations | This flow must be tested using network partition s... |
| Monitoring Requirements | The OPC Core Client must expose Prometheus metrics... |
| Deployment Considerations | The OPC Core Client Docker container must be deplo... |

