# 1 Overview

## 1.1 Diagram Id

SEQ-EP-001

## 1.2 Name

Cloud-to-Edge Configuration Update via MQTT

## 1.3 Description

An Administrator changes the configuration of an OPC Core Client in the web UI. The Device Management Service publishes a 'configuration updated' command message to a client-specific MQTT topic. The target client, subscribed to this topic, receives the message, applies the new configuration, and optionally sends a status update to confirm.

## 1.4 Type

🔹 EventProcessing

## 1.5 Purpose

To provide a robust, asynchronous, and scalable mechanism for centrally managing the configuration of a distributed fleet of edge clients (REQ-BIZ-002), leveraging MQTT for reliable delivery over potentially unstable networks.

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

## 1.10 Key Interactions

- An Admin saves configuration changes in the UI.
- The Device Management Service publishes a command message with the new JSON configuration to the MQTT topic: tenants/{tenantId}/clients/{clientId}/commands/config.
- The MQTT Broker delivers the message to the subscribed OPC Core Client (using QoS 1 for at-least-once delivery).
- The client's MQTT handler receives the message.
- The client parses the new configuration payload, validates it, and applies it to its running state.

## 1.11 Triggers

- An Administrator modifies and saves an OPC Core Client's configuration in the Central Management Plane.

## 1.12 Outcomes

- The remote OPC Core Client is running with the updated configuration without requiring a restart.
- The client may send a status update message back via MQTT to confirm the change was applied successfully.

## 1.13 Business Rules

- Communication must use MQTT v5 over TLS for robust command, control, and status message delivery (REQ-1-010).
- MQTT topics must be structured to ensure strict data isolation between tenants and clients.

## 1.14 Error Scenarios

- The client is offline and does not receive the message immediately (MQTT broker will retain it if the client has a persistent session).
- The client receives the message but fails to apply the new configuration due to validation errors.
- The MQTT broker is unavailable.

## 1.15 Integration Points

- MQTT Broker (e.g., AWS IoT Core, EMQX)

# 2.0 Details

## 2.1 Diagram Id

SEQ-EP-001

## 2.2 Name

Implementation-Ready: Cloud-to-Edge Configuration Update via MQTT

## 2.3 Description

Provides a comprehensive technical specification for updating a remote OPC Core Client's configuration asynchronously. An Administrator's action in the web UI initiates a REST API call, which triggers the Device Management Service to publish a versioned command message to a client-specific MQTT topic. The sequence leverages MQTT v5 with QoS 1 for reliable, at-least-once delivery over potentially unstable networks, and includes a closed-loop feedback mechanism where the client reports its status after applying the change. This design directly supports REQ-BIZ-002 for centralized fleet management.

## 2.4 Participants

### 2.4.1 Frontend SPA

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Frontend: Central Management Plane

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
| Color | #4287f5 |
| Stereotype | UI |

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
| Shape | boundary |
| Color | #f5a642 |
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
| Shape | component |
| Color | #f54e42 |
| Stereotype | Service |

### 2.4.4.0 Message Broker

#### 2.4.4.1 Repository Id

MQTT Broker

#### 2.4.4.2 Display Name

MQTT Broker

#### 2.4.4.3 Type

🔹 Message Broker

#### 2.4.4.4 Technology

AWS IoT Core / EMQX

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | queue |
| Color | #8d34eb |
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
| Shape | node |
| Color | #34c3eb |
| Stereotype | Edge |

## 2.5.0.0 Interactions

### 2.5.1.0 HTTP Request

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

1. POST /api/v1/clients/{clientId}/config

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 HTTP Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

11. 202 Accepted

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | POST |
| Parameters | URL Path: clientId; Body: { version: '1.2.3', sett... |
| Authentication | JWT Bearer Token in Authorization header (as per R... |
| Error Handling | Client handles 4xx/5xx responses. Timeout: 15s. |
| Performance | Request must be sent within 100ms of user action. |

### 2.5.2.0 HTTP Proxy

#### 2.5.2.1 Source Id

REPO-GW-API

#### 2.5.2.2 Target Id

REPO-SVC-DVM

#### 2.5.2.3 Message

2. Route POST /internal/clients/{clientId}/config

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 HTTP Proxy

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

10. 202 Accepted

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (ClusterIP) |
| Method | POST |
| Parameters | Forwards request body and headers, including trace... |
| Authentication | Gateway performs JWT validation. Internal traffic ... |
| Error Handling | If DVM is unavailable, return 503 Service Unavaila... |
| Performance | Adds <10ms latency. |

### 2.5.3.0 Internal Logic

#### 2.5.3.1 Source Id

REPO-SVC-DVM

#### 2.5.3.2 Target Id

REPO-SVC-DVM

#### 2.5.3.3 Message

3. [Self] Validate & Persist Configuration

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Internal Logic

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
| Protocol | N/A |
| Method | updateClientConfiguration() |
| Parameters | Validates schema of configuration JSON. Persists n... |
| Authentication | N/A |
| Error Handling | If validation fails, immediately return 400 Bad Re... |
| Performance | DB transaction must complete <50ms. |

### 2.5.4.0 MQTT Publish

#### 2.5.4.1 Source Id

REPO-SVC-DVM

#### 2.5.4.2 Target Id

MQTT Broker

#### 2.5.4.3 Message

4. PUBLISH ClientConfigurationUpdated Command

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 MQTT Publish

#### 2.5.4.6 Is Synchronous

❌ No

#### 2.5.4.7 Return Message



#### 2.5.4.8 Has Return

❌ No

#### 2.5.4.9 Is Activation

✅ Yes

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 over TLS |
| Method | PUBLISH |
| Parameters | Topic: tenants/{tenantId}/clients/{clientId}/comma... |
| Authentication | Service authenticates to broker using client certi... |
| Error Handling | If PUBLISH fails due to broker unavailability, ret... |
| Performance | Message published within 10ms of DB commit. |

### 2.5.5.0 MQTT Delivery

#### 2.5.5.1 Source Id

MQTT Broker

#### 2.5.5.2 Target Id

REPO-EDGE-OPC

#### 2.5.5.3 Message

5. DELIVER ClientConfigurationUpdated Command

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 MQTT Delivery

#### 2.5.5.6 Is Synchronous

❌ No

#### 2.5.5.7 Return Message



#### 2.5.5.8 Has Return

❌ No

#### 2.5.5.9 Is Activation

✅ Yes

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 over TLS |
| Method | Message Delivery |
| Parameters | Delivers message to the subscribed client. |
| Authentication | Client has an established, authenticated session. |
| Error Handling | If client is offline and has a persistent session,... |
| Performance | Delivery latency is network-dependent. |

### 2.5.6.0 Internal Logic

#### 2.5.6.1 Source Id

REPO-EDGE-OPC

#### 2.5.6.2 Target Id

REPO-EDGE-OPC

#### 2.5.6.3 Message

6. [Self] Validate & Apply Configuration

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 Internal Logic

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
| Protocol | N/A |
| Method | onConfigurationReceived() |
| Parameters | Handler deserializes JSON, validates schema and va... |
| Authentication | N/A |
| Error Handling | If validation fails, logs error, discards message,... |
| Performance | Configuration apply should be non-blocking and com... |

### 2.5.7.0 MQTT Publish

#### 2.5.7.1 Source Id

REPO-EDGE-OPC

#### 2.5.7.2 Target Id

MQTT Broker

#### 2.5.7.3 Message

7. PUBLISH ClientStatusReported Event (Success/Failure)

#### 2.5.7.4 Sequence Number

7

#### 2.5.7.5 Type

🔹 MQTT Publish

#### 2.5.7.6 Is Synchronous

❌ No

#### 2.5.7.7 Return Message



#### 2.5.7.8 Has Return

❌ No

#### 2.5.7.9 Is Activation

✅ Yes

#### 2.5.7.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 over TLS |
| Method | PUBLISH |
| Parameters | Topic: tenants/{tenantId}/clients/{clientId}/statu... |
| Authentication | Established client session. |
| Error Handling | Uses standard MQTT client retry logic on publish f... |
| Performance | Status published immediately after config apply at... |

### 2.5.8.0 MQTT Delivery

#### 2.5.8.1 Source Id

MQTT Broker

#### 2.5.8.2 Target Id

REPO-SVC-DVM

#### 2.5.8.3 Message

8. DELIVER ClientStatusReported Event

#### 2.5.8.4 Sequence Number

8

#### 2.5.8.5 Type

🔹 MQTT Delivery

#### 2.5.8.6 Is Synchronous

❌ No

#### 2.5.8.7 Return Message



#### 2.5.8.8 Has Return

❌ No

#### 2.5.8.9 Is Activation

✅ Yes

#### 2.5.8.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 over TLS |
| Method | Message Delivery |
| Parameters | Delivers status message to subscribed Device Manag... |
| Authentication | Service has an established subscription. |
| Error Handling | N/A |
| Performance | Low latency delivery. |

### 2.5.9.0 Internal Logic

#### 2.5.9.1 Source Id

REPO-SVC-DVM

#### 2.5.9.2 Target Id

REPO-SVC-DVM

#### 2.5.9.3 Message

9. [Self] Process Status and Update DB

#### 2.5.9.4 Sequence Number

9

#### 2.5.9.5 Type

🔹 Internal Logic

#### 2.5.9.6 Is Synchronous

✅ Yes

#### 2.5.9.7 Return Message



#### 2.5.9.8 Has Return

❌ No

#### 2.5.9.9 Is Activation

❌ No

#### 2.5.9.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | N/A |
| Method | onClientStatusReceived() |
| Parameters | Updates the client's status record in PostgreSQL b... |
| Authentication | N/A |
| Error Handling | If status indicates failure, a system-level alert ... |
| Performance | DB update must complete <50ms. |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

MQTT QoS 1 (At-least-once) is crucial for ensuring commands are not lost. Client-side handlers must be idempotent to handle potential duplicate deliveries.

#### 2.6.1.2 Position

bottom

#### 2.6.1.3 Participant Id

MQTT Broker

#### 2.6.1.4 Sequence Number

4

### 2.6.2.0 Content

#### 2.6.2.1 Content

The MQTT broker's persistent session feature is critical for managing offline clients. When a client reconnects, it will automatically receive any queued configuration updates.

#### 2.6.2.2 Position

bottom

#### 2.6.2.3 Participant Id

REPO-EDGE-OPC

#### 2.6.2.4 Sequence Number

5

### 2.6.3.0 Content

#### 2.6.3.1 Content

The topic structure `tenants/{tenantId}/clients/{clientId}/...` is a security enforcement point. Broker ACLs MUST ensure that a client can only publish/subscribe to its own topic hierarchy.

#### 2.6.3.2 Position

top

#### 2.6.3.3 Participant Id

MQTT Broker

#### 2.6.3.4 Sequence Number

*Not specified*

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | All communication must use TLS 1.3 (HTTPS and MQTT... |
| Performance Targets | The initial API request (POST /config) must return... |
| Error Handling Strategy | If the MQTT Broker is unavailable, the Device Mana... |
| Testing Considerations | End-to-end tests must simulate a client going offl... |
| Monitoring Requirements | Monitor the health and connection status of the MQ... |
| Deployment Considerations | The MQTT Broker must be deployed in a high-availab... |

