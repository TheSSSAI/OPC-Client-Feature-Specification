# 1 Overview

## 1.1 Diagram Id

SEQ-OF-003

## 1.2 Name

Remote Software Update of OPC Client

## 1.3 Description

An Administrator initiates a software update for a specific OPC Core Client from the web UI. The Device Management Service commands the client (via MQTT) to download a new Docker image from a specified container registry and restart itself using the new version, with a rollback mechanism in case of failure.

## 1.4 Type

🔹 OperationalFlow

## 1.5 Purpose

To provide a centralized, secure, and automated mechanism for maintaining and updating the software of a large, distributed fleet of edge clients (REQ-BIZ-004).

## 1.6 Complexity

High

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-DVM
- MQTT Broker
- REPO-EDGE-OPC
- Docker Registry

## 1.10 Key Interactions

- Admin selects a client, a new software version, and clicks 'Update'.
- Device Management Service publishes an update command via MQTT, including the new Docker image URI.
- The OPC Core Client's update module receives the command.
- The module executes a script to `docker pull` the new image.
- The script stops and removes the old container, then starts a new container from the new image, preserving configuration and data volumes.
- The new client starts, connects to the cloud, and reports its new version via a status message.

## 1.11 Triggers

- An Administrator initiates a software update from the Central Management Plane for one or more clients.

## 1.12 Outcomes

- The remote OPC Core Client is running the new software version with minimal downtime.
- The system supports the ability to roll back to a previous version in case of a failed update (REQ-BIZ-004).

## 1.13 Business Rules

- The update mechanism must support rollback to a previous version in case of failure.
- The client must perform a health check after starting with the new version before reporting success.

## 1.14 Error Scenarios

- The client fails to download the new Docker image from the registry (e.g., auth failure, network issue).
- The new container fails to start due to a critical bug or misconfiguration.
- The update process is interrupted by a power failure at the edge, requiring a recovery process on reboot.

## 1.15 Integration Points

- Docker Image Registry (e.g., Amazon ECR, Docker Hub)

# 2.0 Details

## 2.1 Diagram Id

SEQ-OF-003

## 2.2 Name

Implementation: Remote Software Update for OPC Core Client

## 2.3 Description

A comprehensive technical sequence for an Administrator initiating a remote software update for a specific OPC Core Client. The process leverages an asynchronous command pattern via MQTT for instructing the client to pull a new Docker image, restart itself, and report back its status. The sequence includes critical error handling and an automated rollback mechanism to ensure operational resilience at the edge, as required by REQ-BIZ-004.

## 2.4 Participants

### 2.4.1 Frontend SPA

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Frontend: Management Plane

#### 2.4.1.3 Type

🔹 Frontend SPA

#### 2.4.1.4 Technology

React 18, TypeScript, Axios

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #3498DB |
| Stereotype | User Interface |

### 2.4.2.0 API Gateway

#### 2.4.2.1 Repository Id

REPO-GW-API

#### 2.4.2.2 Display Name

API Gateway

#### 2.4.2.3 Type

🔹 API Gateway

#### 2.4.2.4 Technology

Kong v3.7.0 on Kubernetes

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #F1C40F |
| Stereotype | Gateway |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-DVM

#### 2.4.3.2 Display Name

Device Management Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8, ASP.NET Core, MQTT Client

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #2ECC71 |
| Stereotype | Service |

### 2.4.4.0 Message Broker

#### 2.4.4.1 Repository Id

MQTT Broker

#### 2.4.4.2 Display Name

MQTT Broker

#### 2.4.4.3 Type

🔹 Message Broker

#### 2.4.4.4 Technology

MQTT v5 (e.g., AWS IoT Core)

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #E67E22 |
| Stereotype | Broker |

### 2.4.5.0 Edge Application

#### 2.4.5.1 Repository Id

REPO-EDGE-OPC

#### 2.4.5.2 Display Name

OPC Core Client

#### 2.4.5.3 Type

🔹 Edge Application

#### 2.4.5.4 Technology

.NET 8, Docker, MQTT Client

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #E74C3C |
| Stereotype | Edge Device |

### 2.4.6.0 Container Registry

#### 2.4.6.1 Repository Id

Docker Registry

#### 2.4.6.2 Display Name

Docker Image Registry

#### 2.4.6.3 Type

🔹 Container Registry

#### 2.4.6.4 Technology

e.g., Amazon ECR, Docker Hub

#### 2.4.6.5 Order

6

#### 2.4.6.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #9B59B6 |
| Stereotype | External System |

## 2.5.0.0 Interactions

### 2.5.1.0 HTTP Request

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

1. POST /api/v1/clients/{clientId}/update

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 HTTP Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

16. HTTP 202 Accepted

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | POST |
| Parameters | URL Path: {clientId}. Body: {"targetVersion": "2.1... |
| Authentication | JWT Bearer Token in Authorization header |
| Error Handling | Handle 401/403 for auth errors, 404 for unknown cl... |
| Performance | P95 < 200ms |

### 2.5.2.0 HTTP Proxy

#### 2.5.2.1 Source Id

REPO-GW-API

#### 2.5.2.2 Target Id

REPO-SVC-DVM

#### 2.5.2.3 Message

2. Route Request: POST /clients/{clientId}/update

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 HTTP Proxy

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

15. HTTP 202 Accepted

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (ClusterIP) |
| Method | POST |
| Parameters | Forwards request body and headers. |
| Authentication | JWT is validated by gateway; request is trusted in... |
| Error Handling | Routes backend 5xx errors as standard gateway erro... |
| Performance | Adds < 10ms latency. |

### 2.5.3.0 Internal Logic

#### 2.5.3.1 Source Id

REPO-SVC-DVM

#### 2.5.3.2 Target Id

REPO-SVC-DVM

#### 2.5.3.3 Message

3. Validate Request and Prepare Command

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Internal Logic

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Has Return

❌ No

#### 2.5.3.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | In-process |
| Method | validateUpdatePermissions() |
| Parameters | User claims from JWT, {clientId} |
| Authentication | N/A |
| Error Handling | Throws AuthorizationException if user is not Admin... |
| Performance | Database lookups for client and version metadata. |

### 2.5.4.0 MQTT Publish

#### 2.5.4.1 Source Id

REPO-SVC-DVM

#### 2.5.4.2 Target Id

MQTT Broker

#### 2.5.4.3 Message

4. PUBLISH Update Command

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 MQTT Publish

#### 2.5.4.6 Is Synchronous

❌ No

#### 2.5.4.7 Has Return

❌ No

#### 2.5.4.8 Is Activation

✅ Yes

#### 2.5.4.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT over TLS |
| Method | PUBLISH |
| Parameters | Topic: tenants/{tenantId}/clients/{clientId}/comma... |
| Authentication | Service uses client certificate to connect to brok... |
| Error Handling | Retry publish on connection failure. Log error if ... |
| Performance | Low latency publish operation. |

### 2.5.5.0 MQTT Deliver

#### 2.5.5.1 Source Id

MQTT Broker

#### 2.5.5.2 Target Id

REPO-EDGE-OPC

#### 2.5.5.3 Message

5. DELIVER Command Message

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 MQTT Deliver

#### 2.5.5.6 Is Synchronous

❌ No

#### 2.5.5.7 Has Return

❌ No

#### 2.5.5.8 Is Activation

✅ Yes

#### 2.5.5.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT over TLS |
| Method | DELIVER |
| Parameters | Delivers payload from step 4. |
| Authentication | N/A |
| Error Handling | Broker persists message if client is offline (base... |
| Performance | Dependent on network conditions to the edge. |

### 2.5.6.0 Process Execution

#### 2.5.6.1 Source Id

REPO-EDGE-OPC

#### 2.5.6.2 Target Id

REPO-EDGE-OPC

#### 2.5.6.3 Message

6. Execute Update Script

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 Process Execution

#### 2.5.6.6 Is Synchronous

❌ No

#### 2.5.6.7 Has Return

❌ No

#### 2.5.6.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Local Shell |
| Method | /opt/updater/update.sh |
| Parameters | Command payload from MQTT message passed as argume... |
| Authentication | Script runs with sufficient permissions to interac... |
| Error Handling | Script has comprehensive error handling for each s... |
| Performance | N/A |

### 2.5.7.0 Docker Pull

#### 2.5.7.1 Source Id

REPO-EDGE-OPC

#### 2.5.7.2 Target Id

Docker Registry

#### 2.5.7.3 Message

7. Pull Docker Image

#### 2.5.7.4 Sequence Number

7

#### 2.5.7.5 Type

🔹 Docker Pull

#### 2.5.7.6 Is Synchronous

✅ Yes

#### 2.5.7.7 Return Message

8. Image Layers

#### 2.5.7.8 Has Return

✅ Yes

#### 2.5.7.9 Is Activation

✅ Yes

#### 2.5.7.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | docker pull <imageUri> |
| Parameters | Image URI from command payload. |
| Authentication | Docker client uses stored credentials (e.g., from ... |
| Error Handling | If pull fails, updater script publishes failure st... |
| Performance | Highly dependent on edge network bandwidth and ima... |

### 2.5.8.0 Docker Lifecycle

#### 2.5.8.1 Source Id

REPO-EDGE-OPC

#### 2.5.8.2 Target Id

REPO-EDGE-OPC

#### 2.5.8.3 Message

9. Stop, Remove, and Start New Container

#### 2.5.8.4 Sequence Number

9

#### 2.5.8.5 Type

🔹 Docker Lifecycle

#### 2.5.8.6 Is Synchronous

✅ Yes

#### 2.5.8.7 Has Return

❌ No

#### 2.5.8.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Docker Engine API |
| Method | docker stop <old>; docker rm <old>; docker run --r... |
| Parameters | Container names/IDs, volume mounts must be preserv... |
| Authentication | Docker socket permissions. |
| Error Handling | CRITICAL: If `docker run` fails, script immediatel... |
| Performance | Causes client downtime, should be as fast as possi... |

### 2.5.9.0 Application Initialization

#### 2.5.9.1 Source Id

REPO-EDGE-OPC

#### 2.5.9.2 Target Id

REPO-EDGE-OPC

#### 2.5.9.3 Message

10. [New Instance] Startup & Self Health Check

#### 2.5.9.4 Sequence Number

10

#### 2.5.9.5 Type

🔹 Application Initialization

#### 2.5.9.6 Is Synchronous

✅ Yes

#### 2.5.9.7 Has Return

❌ No

#### 2.5.9.8 Is Activation

✅ Yes

#### 2.5.9.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | In-process |
| Method | main() |
| Parameters | Configuration from mounted volumes. |
| Authentication | N/A |
| Error Handling | If health check fails (e.g., cannot read config, c... |
| Performance | Startup time should be optimized. |

### 2.5.10.0 MQTT Publish

#### 2.5.10.1 Source Id

REPO-EDGE-OPC

#### 2.5.10.2 Target Id

MQTT Broker

#### 2.5.10.3 Message

11. PUBLISH Success Status

#### 2.5.10.4 Sequence Number

11

#### 2.5.10.5 Type

🔹 MQTT Publish

#### 2.5.10.6 Is Synchronous

❌ No

#### 2.5.10.7 Has Return

❌ No

#### 2.5.10.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT over TLS |
| Method | PUBLISH |
| Parameters | Topic: tenants/{tenantId}/clients/{clientId}/statu... |
| Authentication | Client certificate. |
| Error Handling | Standard MQTT client retry logic. |
| Performance | Low latency. |

### 2.5.11.0 MQTT Deliver

#### 2.5.11.1 Source Id

MQTT Broker

#### 2.5.11.2 Target Id

REPO-SVC-DVM

#### 2.5.11.3 Message

12. DELIVER Status Message

#### 2.5.11.4 Sequence Number

12

#### 2.5.11.5 Type

🔹 MQTT Deliver

#### 2.5.11.6 Is Synchronous

❌ No

#### 2.5.11.7 Has Return

❌ No

#### 2.5.11.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT over TLS |
| Method | DELIVER |
| Parameters | Delivers payload from step 11. |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Low latency. |

### 2.5.12.0 Database Operation

#### 2.5.12.1 Source Id

REPO-SVC-DVM

#### 2.5.12.2 Target Id

REPO-SVC-DVM

#### 2.5.12.3 Message

13. Update Client Record in Database

#### 2.5.12.4 Sequence Number

13

#### 2.5.12.5 Type

🔹 Database Operation

#### 2.5.12.6 Is Synchronous

✅ Yes

#### 2.5.12.7 Has Return

❌ No

#### 2.5.12.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | UPDATE clients SET version = @version, status = @s... |
| Parameters | Version and status from MQTT message. |
| Authentication | Database connection string credentials. |
| Error Handling | Retry on transient DB errors. Log critical error i... |
| Performance | Should be a fast, indexed update. |

### 2.5.13.0 HTTP Response

#### 2.5.13.1 Source Id

REPO-SVC-DVM

#### 2.5.13.2 Target Id

REPO-GW-API

#### 2.5.13.3 Message

14. Return HTTP 202 Accepted

#### 2.5.13.4 Sequence Number

14

#### 2.5.13.5 Type

🔹 HTTP Response

#### 2.5.13.6 Is Synchronous

✅ Yes

#### 2.5.13.7 Has Return

❌ No

#### 2.5.13.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (ClusterIP) |
| Method | 202 Accepted |
| Parameters | Body: {"jobId": "uuid-v4"} |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | N/A |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

The updater script on the Edge Client is the most critical component for reliability. It must be idempotent and contain robust logic to handle failures at any step and trigger a rollback to the previous working version. The old Docker image must not be deleted until the update is fully confirmed.

#### 2.6.1.2 Position

bottom

#### 2.6.1.3 Participant Id

REPO-EDGE-OPC

#### 2.6.1.4 Sequence Number

6

### 2.6.2.0 Content

#### 2.6.2.1 Content

The entire operation is asynchronous from the user's perspective. The UI should immediately reflect an 'Updating...' status after step 1 and then poll for the job status or use a WebSocket/SignalR subscription to get the final success/failure notification.

#### 2.6.2.2 Position

top

#### 2.6.2.3 Participant Id

REPO-FE-MPL

#### 2.6.2.4 Sequence Number

1

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | The API endpoint to trigger an update must be rest... |
| Performance Targets | Client downtime during the stop/start phase (step ... |
| Error Handling Strategy | The primary strategy is automated rollback at the ... |
| Testing Considerations | Chaos testing is essential. Scenarios to test incl... |
| Monitoring Requirements | The OPC Core Client must expose metrics for update... |
| Deployment Considerations | A phased rollout strategy should be implemented in... |

