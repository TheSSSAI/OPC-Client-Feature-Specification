# 1 Overview

## 1.1 Diagram Id

SEQ-EH-001

## 1.2 Name

OPC Redundant Server Failover at the Edge

## 1.3 Description

An OPC Core Client, configured with a primary and backup OPC server pair, loses connection to the primary server. It detects the failure based on user-configured trigger conditions, automatically attempts to connect to the backup server, and upon success, seamlessly switches all data subscriptions to the new active server.

## 1.4 Type

🔹 ErrorHandling

## 1.5 Purpose

To provide high availability for data acquisition at the edge by automatically recovering from a single OPC server failure without manual intervention, as specified in REQ-FR-011.

## 1.6 Complexity

Medium

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-EDGE-OPC
- Primary OPC Server
- Backup OPC Server

## 1.10 Key Interactions

- The client's internal health check to the primary OPC server fails (e.g., connection loss, bad status code).
- The client's failover logic is triggered after the condition persists for a configured duration.
- The client attempts to establish a connection with the configured backup OPC server.
- Upon successful connection, it re-creates its subscription and monitored items on the new server.
- The client logs the failover event and sends an alert to the Central Management Plane via MQTT.

## 1.11 Triggers

- The primary OPC server becomes unreachable or reports a bad status, meeting the defined failover conditions.

## 1.12 Outcomes

- Data acquisition continues with minimal interruption, now sourcing from the backup server.
- A system alert is generated in the Central Management Plane to notify personnel of the failover event.

## 1.13 Business Rules

- Failover trigger conditions (e.g., connection loss, status code) and health check parameters must be user-configurable (REQ-FR-011).

## 1.14 Error Scenarios

- The backup server is also unavailable, resulting in a total loss of data source.
- The client fails to subscribe to items on the backup server due to configuration mismatches.
- A 'split-brain' scenario where the client rapidly switches between servers due to an unstable network.

## 1.15 Integration Points

- Third-party OPC Servers (DA, UA, XML-DA)

# 2.0 Details

## 2.1 Diagram Id

SEQ-EH-001

## 2.2 Name

Implementation of Edge OPC Server Redundancy Failover

## 2.3 Description

This sequence details the automated error handling process within the OPC Core Client upon detecting a failure of the primary OPC server. It covers the health check failure detection based on configurable triggers (connection loss, bad status), the state transition to a failover state, the connection attempt to the backup server, the re-establishment of data subscriptions, and the subsequent notification to the Central Management Plane via MQTT. This implements the high-availability data acquisition requirement of REQ-FR-011.

## 2.4 Participants

### 2.4.1 Edge Application

#### 2.4.1.1 Repository Id

REPO-EDGE-OPC

#### 2.4.1.2 Display Name

OPC Core Client

#### 2.4.1.3 Type

🔹 Edge Application

#### 2.4.1.4 Technology

.NET 8 in Docker Container

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | Edge Component |

### 2.4.2.0 External System

#### 2.4.2.1 Repository Id

EXT-OPC-PRIMARY

#### 2.4.2.2 Display Name

Primary OPC Server

#### 2.4.2.3 Type

🔹 External System

#### 2.4.2.4 Technology

Third-Party OPC Server (UA/DA)

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #A9A9A9 |
| Stereotype | External Dependency |

### 2.4.3.0 External System

#### 2.4.3.1 Repository Id

EXT-OPC-BACKUP

#### 2.4.3.2 Display Name

Backup OPC Server

#### 2.4.3.3 Type

🔹 External System

#### 2.4.3.4 Technology

Third-Party OPC Server (UA/DA)

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #A9A9A9 |
| Stereotype | External Dependency |

### 2.4.4.0 Messaging Infrastructure

#### 2.4.4.1 Repository Id

INFRA-MQTT-BROKER

#### 2.4.4.2 Display Name

MQTT Broker

#### 2.4.4.3 Type

🔹 Messaging Infrastructure

#### 2.4.4.4 Technology

MQTT v5 Broker (e.g., AWS IoT Core)

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #FFA500 |
| Stereotype | Cloud Service |

## 2.5.0.0 Interactions

### 2.5.1.0 HealthCheck

#### 2.5.1.1 Source Id

REPO-EDGE-OPC

#### 2.5.1.2 Target Id

EXT-OPC-PRIMARY

#### 2.5.1.3 Message

[Loop] checkServerStatus()

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 HealthCheck

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

ServerStatusDataType

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

❌ No

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | OPC UA Binary TCP |
| Method | ReadRequest(Server.ServerStatus) |
| Parameters | NodeId: ns=0;i=2256 |
| Authentication | Uses configured OPC UA session credentials. |
| Error Handling | Catches ServiceResultException (e.g., Bad_Connecti... |
| Performance | Timeout configured as per REQ-FR-011 (e.g., 5 seco... |

### 2.5.2.0 StateTransition

#### 2.5.2.1 Source Id

REPO-EDGE-OPC

#### 2.5.2.2 Target Id

REPO-EDGE-OPC

#### 2.5.2.3 Message

[Alt: failureCounter > config.failureThreshold] triggerFailover()

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 StateTransition

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message



#### 2.5.2.8 Has Return

❌ No

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Method Call |
| Method | initiateFailoverProcess |
| Parameters | null |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate execution. |

### 2.5.3.0 Request

#### 2.5.3.1 Source Id

REPO-EDGE-OPC

#### 2.5.3.2 Target Id

EXT-OPC-PRIMARY

#### 2.5.3.3 Message

disconnectSession()

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Request

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

Status

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

❌ No

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | OPC UA Binary TCP |
| Method | CloseSessionRequest |
| Parameters | Session context |
| Authentication | N/A |
| Error Handling | Errors are ignored as the connection is already co... |
| Performance | Best-effort with short timeout. |

### 2.5.4.0 Request

#### 2.5.4.1 Source Id

REPO-EDGE-OPC

#### 2.5.4.2 Target Id

EXT-OPC-BACKUP

#### 2.5.4.3 Message

createSession(endpointUrl, securityPolicy)

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 Request

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

Session Object

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | OPC UA Binary TCP |
| Method | CreateSessionRequest |
| Parameters | From backup server configuration. |
| Authentication | Uses configured OPC UA session credentials. |
| Error Handling | Implements Retry pattern. If connection fails, ret... |
| Performance | Connection timeout per attempt: 10 seconds. |

### 2.5.5.0 Request

#### 2.5.5.1 Source Id

REPO-EDGE-OPC

#### 2.5.5.2 Target Id

EXT-OPC-BACKUP

#### 2.5.5.3 Message

recreateSubscriptions()

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 Request

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

Subscription Statuses

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | OPC UA Binary TCP |
| Method | CreateSubscriptionRequest, CreateMonitoredItemsReq... |
| Parameters | Cached list of monitored items and subscription se... |
| Authentication | N/A (uses established session). |
| Error Handling | If any subscription fails, log a critical 'Subscri... |
| Performance | Depends on the number of tags, but should complete... |

### 2.5.6.0 StateTransition

#### 2.5.6.1 Source Id

REPO-EDGE-OPC

#### 2.5.6.2 Target Id

REPO-EDGE-OPC

#### 2.5.6.3 Message

setActiveServer(backupServer)

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 StateTransition

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
| Method | updateActiveConnectionState |
| Parameters | Backup server's connection object |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate execution. |

### 2.5.7.0 Logging

#### 2.5.7.1 Source Id

REPO-EDGE-OPC

#### 2.5.7.2 Target Id

REPO-EDGE-OPC

#### 2.5.7.3 Message

logFailoverEventToAuditTrail()

#### 2.5.7.4 Sequence Number

7

#### 2.5.7.5 Type

🔹 Logging

#### 2.5.7.6 Is Synchronous

✅ Yes

#### 2.5.7.7 Return Message



#### 2.5.7.8 Has Return

❌ No

#### 2.5.7.9 Is Activation

❌ No

#### 2.5.7.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal I/O |
| Method | auditLogger.log() |
| Parameters | Structured log entry containing timestamp, eventTy... |
| Authentication | N/A |
| Error Handling | Log to secondary file/console if primary audit log... |
| Performance | Should not block data acquisition thread. |

### 2.5.8.0 Asynchronous Notification

#### 2.5.8.1 Source Id

REPO-EDGE-OPC

#### 2.5.8.2 Target Id

INFRA-MQTT-BROKER

#### 2.5.8.3 Message

publish(topic, payload)

#### 2.5.8.4 Sequence Number

8

#### 2.5.8.5 Type

🔹 Asynchronous Notification

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
| Protocol | MQTT v5 over TLS |
| Method | PUBLISH |
| Parameters | Topic: tenants/{tenantId}/clients/{clientId}/event... |
| Authentication | Client certificate (mTLS with broker is an option)... |
| Error Handling | MQTT client library handles reconnection and messa... |
| Performance | Asynchronous fire-and-forget from the client's per... |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Failover Trigger Conditions (e.g., connection loss, specific status codes, consecutive failure count) are user-configurable in the Central Management Plane as per REQ-FR-011.

#### 2.6.1.2 Position

Top

#### 2.6.1.3 Participant Id

*Not specified*

#### 2.6.1.4 Sequence Number

2

### 2.6.2.0 Content

#### 2.6.2.1 Content

Backup Failure Scenario: If connection to the backup server fails after all retries (step 4), the client enters a DISCONNECTED state, continues buffering data (REQ-1-079), and periodically attempts to reconnect to BOTH primary and backup servers until one is available.

#### 2.6.2.2 Position

Right

#### 2.6.2.3 Participant Id

EXT-OPC-BACKUP

#### 2.6.2.4 Sequence Number

4

### 2.6.3.0 Content

#### 2.6.3.1 Content

Split-Brain Mitigation: To prevent rapid switching (flapping) in an unstable network, a configurable 'failback delay' is implemented. Once failed over to the backup, the client will not attempt to switch back to the primary until it has been consistently healthy for a defined period (e.g., 5 minutes).

#### 2.6.3.2 Position

Bottom

#### 2.6.3.3 Participant Id

*Not specified*

#### 2.6.3.4 Sequence Number

*Not specified*

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Communication with OPC UA servers must use the con... |
| Performance Targets | Recovery Time Objective (RTO) for data acquisition... |
| Error Handling Strategy | The primary error condition (primary server failur... |
| Testing Considerations | A fault injection test suite is required. Scenario... |
| Monitoring Requirements | The OPC Core Client must expose Prometheus metrics... |
| Deployment Considerations | The Docker container for the OPC Core Client requi... |

