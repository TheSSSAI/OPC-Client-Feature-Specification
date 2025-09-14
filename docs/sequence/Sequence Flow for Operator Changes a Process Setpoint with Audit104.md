# 1 Overview

## 1.1 Diagram Id

SEQ-FF-003

## 1.2 Name

Operator Changes a Process Setpoint with Audit

## 1.3 Description

An authorized Operator views a dashboard and changes a process setpoint by writing a new value to an OPC tag. The system validates the permission, sends the write command to the OPC server via the Core Client, and records the change (including old and new values) in the audit trail.

## 1.4 Type

🔹 FeatureFlow

## 1.5 Purpose

To provide the core functionality for operators to interact with and control live processes, while ensuring full traceability and compliance as per REQ-FR-001 and REQ-FR-005.

## 1.6 Complexity

Medium

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
- REPO-SVC-ADT

## 1.10 Key Interactions

- Operator enters a new setpoint value in the UI.
- Frontend sends the write request (tag, value) to the Device Management Service.
- The service sends a 'WriteTag' command via MQTT to the specific OPC Core Client.
- The client executes the OPC write operation on the target server.
- Simultaneously, the Device Management Service calls the Audit Service to log the action, including user, tag, old value (if available), and new value.

## 1.11 Triggers

- An Operator needs to adjust a live process parameter.

## 1.12 Outcomes

- The value is updated on the physical control system via the OPC server.
- A tamper-evident audit log of the change is created.

## 1.13 Business Rules

- Only authorized users can write to tags (REQ-USR-001).
- All setpoint changes must be logged in the audit trail with old and new values (REQ-FR-005).

## 1.14 Error Scenarios

- The OPC server rejects the write operation.
- The user lacks the specific permission to write to that tag.
- The audit service is unavailable.

## 1.15 Integration Points

- Third-party OPC Servers

# 2.0 Details

## 2.1 Diagram Id

SEQ-FF-003

## 2.2 Name

Implementation Sequence: Operator Process Setpoint Change with Audit

## 2.3 Description

Technical sequence for an authorized Operator changing a process setpoint. The sequence details the synchronous API request, role-based authorization check, parallel asynchronous command dispatch to the edge client via MQTT, and the synchronous, mandatory gRPC call to the Audit Service to ensure compliance with REQ-FR-001 and REQ-FR-005.

## 2.4 Participants

### 2.4.1 Frontend SPA

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Frontend

#### 2.4.1.3 Type

🔹 Frontend SPA

#### 2.4.1.4 Technology

React 18, TypeScript

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | UI |

### 2.4.2.0 API Gateway

#### 2.4.2.1 Repository Id

REPO-GW-API

#### 2.4.2.2 Display Name

API Gateway

#### 2.4.2.3 Type

🔹 API Gateway

#### 2.4.2.4 Technology

Kong v3.7.0 on K8s

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #4682B4 |
| Stereotype | Gateway |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-DVM

#### 2.4.3.2 Display Name

Device Mgmt Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8 ASP.NET Core

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #5F9EA0 |
| Stereotype | Service |

### 2.4.4.0 Microservice

#### 2.4.4.1 Repository Id

REPO-SVC-ADT

#### 2.4.4.2 Display Name

Audit Service

#### 2.4.4.3 Type

🔹 Microservice

#### 2.4.4.4 Technology

.NET 8 gRPC

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #8FBC8F |
| Stereotype | Service |

### 2.4.5.0 Message Broker

#### 2.4.5.1 Repository Id

MQTT Broker

#### 2.4.5.2 Display Name

MQTT Broker

#### 2.4.5.3 Type

🔹 Message Broker

#### 2.4.5.4 Technology

MQTT v5 (e.g., AWS IoT Core)

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #FF8C00 |
| Stereotype | Broker |

### 2.4.6.0 Edge Application

#### 2.4.6.1 Repository Id

REPO-EDGE-OPC

#### 2.4.6.2 Display Name

OPC Core Client

#### 2.4.6.3 Type

🔹 Edge Application

#### 2.4.6.4 Technology

.NET 8, Docker

#### 2.4.6.5 Order

6

#### 2.4.6.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #B8860B |
| Stereotype | Edge |

## 2.5.0.0 Interactions

### 2.5.1.0 Request

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

1. POST /api/v1/clients/{clientId}/tags/write

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

8. HTTP 202 Accepted

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/REST |
| Method | POST |
| Parameters | Payload: { tagId: string, newValue: any } |
| Authentication | JWT Bearer Token in Authorization header. |
| Error Handling | Client handles 401, 403, 429, 5xx responses. |
| Performance | End-to-end latency for this leg should be < 200ms ... |

### 2.5.2.0 Proxy

#### 2.5.2.1 Source Id

REPO-GW-API

#### 2.5.2.2 Target Id

REPO-SVC-DVM

#### 2.5.2.3 Message

2. Route Request: POST /clients/{clientId}/tags/write

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Proxy

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

7. HTTP 202 Accepted

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (ClusterIP) |
| Method | POST |
| Parameters | Forwards original payload. Adds authenticated user... |
| Authentication | JWT validation is performed before routing. |
| Error Handling | Returns standard HTTP error codes if routing fails... |
| Performance | Adds <10ms latency. |

### 2.5.3.0 Internal Call

#### 2.5.3.1 Source Id

REPO-SVC-DVM

#### 2.5.3.2 Target Id

REPO-SVC-DVM

#### 2.5.3.3 Message

3. Validate Permissions

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Internal Call

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message



#### 2.5.3.8 Has Return

❌ No

#### 2.5.3.9 Is Activation

❌ No

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Logic |
| Method | checkWritePermission(userId, tagId) |
| Parameters | User context from token, tag ID from payload. |
| Authentication | N/A |
| Error Handling | If validation fails, aborts the sequence and retur... |
| Performance | Permission check must be highly optimized (<10ms),... |

### 2.5.4.0 gRPC Call

#### 2.5.4.1 Source Id

REPO-SVC-DVM

#### 2.5.4.2 Target Id

REPO-SVC-ADT

#### 2.5.4.3 Message

4. LogAction(LogWriteOperationRequest)

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 gRPC Call

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

5. LogActionResponse { success: true }

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

✅ Yes

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | AuditService.LogAction |
| Parameters | Payload: { userId, tenantId, action: 'Setpoint Cha... |
| Authentication | mTLS between services. |
| Error Handling | Handled via Circuit Breaker pattern. If Audit Serv... |
| Performance | Latency should be < 50ms. |

### 2.5.5.0 Publish

#### 2.5.5.1 Source Id

REPO-SVC-DVM

#### 2.5.5.2 Target Id

MQTT Broker

#### 2.5.5.3 Message

6. PUBLISH tenants/{tenantId}/clients/{clientId}/commands

#### 2.5.5.4 Sequence Number

6

#### 2.5.5.5 Type

🔹 Publish

#### 2.5.5.6 Is Synchronous

❌ No

#### 2.5.5.7 Return Message



#### 2.5.5.8 Has Return

❌ No

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 over TLS |
| Method | PUBLISH |
| Parameters | Payload: { command: 'WriteTag', correlationId, tag... |
| Authentication | Service uses client certificate to connect to brok... |
| Error Handling | If publish fails due to broker unavailability, the... |
| Performance | Publish latency should be < 20ms. |

### 2.5.6.0 Message Delivery

#### 2.5.6.1 Source Id

MQTT Broker

#### 2.5.6.2 Target Id

REPO-EDGE-OPC

#### 2.5.6.3 Message

9. Deliver WriteTag Command

#### 2.5.6.4 Sequence Number

9

#### 2.5.6.5 Type

🔹 Message Delivery

#### 2.5.6.6 Is Synchronous

❌ No

#### 2.5.6.7 Return Message



#### 2.5.6.8 Has Return

❌ No

#### 2.5.6.9 Is Activation

✅ Yes

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 |
| Method | RECEIVE |
| Parameters | Receives payload published in step 6. |
| Authentication | Client is subscribed to its unique command topic u... |
| Error Handling | If client is offline, broker holds the message as ... |
| Performance | Delivery latency depends on network conditions. |

### 2.5.7.0 External Call

#### 2.5.7.1 Source Id

REPO-EDGE-OPC

#### 2.5.7.2 Target Id

REPO-EDGE-OPC

#### 2.5.7.3 Message

10. Execute OPC Write to Server

#### 2.5.7.4 Sequence Number

10

#### 2.5.7.5 Type

🔹 External Call

#### 2.5.7.6 Is Synchronous

✅ Yes

#### 2.5.7.7 Return Message

Result { success, oldValue }

#### 2.5.7.8 Has Return

✅ Yes

#### 2.5.7.9 Is Activation

❌ No

#### 2.5.7.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | OPC UA Binary TCP |
| Method | WriteValue(nodeId, value) |
| Parameters | Node ID corresponding to tagId, value from MQTT me... |
| Authentication | OPC UA certificate-based security. |
| Error Handling | If OPC write fails, the client logs the error loca... |
| Performance | Should be low-latency, as it's on the local indust... |

### 2.5.8.0 Publish

#### 2.5.8.1 Source Id

REPO-EDGE-OPC

#### 2.5.8.2 Target Id

MQTT Broker

#### 2.5.8.3 Message

11. PUBLISH tenants/{...}/status/write_result

#### 2.5.8.4 Sequence Number

11

#### 2.5.8.5 Type

🔹 Publish

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
| Parameters | Payload: { correlationId, success, oldValue, newVa... |
| Authentication | Client certificate. |
| Error Handling | Client will buffer status messages if disconnected... |
| Performance | N/A |

### 2.5.9.0 Message Delivery

#### 2.5.9.1 Source Id

MQTT Broker

#### 2.5.9.2 Target Id

REPO-SVC-DVM

#### 2.5.9.3 Message

12. Deliver Write Status

#### 2.5.9.4 Sequence Number

12

#### 2.5.9.5 Type

🔹 Message Delivery

#### 2.5.9.6 Is Synchronous

❌ No

#### 2.5.9.7 Return Message



#### 2.5.9.8 Has Return

❌ No

#### 2.5.9.9 Is Activation

❌ No

#### 2.5.9.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 |
| Method | RECEIVE |
| Parameters | DVM subscribes to `tenants/+/clients/+/status/+` t... |
| Authentication | N/A |
| Error Handling | If DVM fails to process the message, it will be re... |
| Performance | DVM updates internal state based on the result. Ma... |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Business Rule Enforcement: Permission validation in step 3 is critical for ensuring only authorized Operators can change setpoints, as per REQ-USR-001.

#### 2.6.1.2 Position

top-left

#### 2.6.1.3 Participant Id

REPO-SVC-DVM

#### 2.6.1.4 Sequence Number

3

### 2.6.2.0 Content

#### 2.6.2.1 Content

Compliance Mandate: The gRPC call to the Audit Service (step 4) is synchronous and blocking. A failure here MUST fail the entire operation to comply with REQ-FR-005.

#### 2.6.2.2 Position

top-right

#### 2.6.2.3 Participant Id

REPO-SVC-ADT

#### 2.6.2.4 Sequence Number

4

### 2.6.3.0 Content

#### 2.6.3.1 Content

Asynchronous Acknowledgement: The API returns '202 Accepted' immediately after dispatching the command and logging the audit event. This provides a responsive UI experience while the command executes asynchronously at the edge.

#### 2.6.3.2 Position

bottom-left

#### 2.6.3.3 Participant Id

REPO-GW-API

#### 2.6.3.4 Sequence Number

7

### 2.6.4.0 Content

#### 2.6.4.1 Content

Feedback Loop: The status message published by the client (step 11) closes the loop, providing confirmation or failure details for the operation. This can be used to update the audit log with the `oldValue` or flag a failed action.

#### 2.6.4.2 Position

bottom-right

#### 2.6.4.3 Participant Id

REPO-EDGE-OPC

#### 2.6.4.4 Sequence Number

11

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | 1. API Gateway MUST enforce JWT validation on the ... |
| Performance Targets | The synchronous API call (steps 1-8) must have a P... |
| Error Handling Strategy | - **Permission Denied:** DVM returns HTTP 403. - *... |
| Testing Considerations | 1. **Unit Tests:** DVM logic for permission valida... |
| Monitoring Requirements | 1. **DVM:** Monitor latency of the `/tags/write` e... |
| Deployment Considerations | The contract for the MQTT `WriteTag` command is a ... |

