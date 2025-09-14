# 1 System Overview

## 1.1 Analysis Date

2025-06-13

## 1.2 Architecture Type

Microservices with Edge Computing

## 1.3 Technology Stack

- AWS EKS
- Docker
- .NET 8
- React
- PostgreSQL/TimescaleDB
- MQTT v5
- gRPC

## 1.4 Bounded Contexts

- Device Management & Control
- Data Ingestion & Processing
- Asset & Topology Management
- AI/ML Model Management
- Identity & Access Management

# 2.0 Project Specific Events

## 2.1 Event Id

### 2.1.1 Event Id

EVT-CMD-001

### 2.1.2 Event Name

ClientConfigurationUpdated

### 2.1.3 Event Type

command

### 2.1.4 Category

🔹 Device Management

### 2.1.5 Description

A command sent from the Central Management Plane to a specific OPC Core Client to apply a new configuration.

### 2.1.6 Trigger Condition

An Engineer or Administrator saves a configuration change for an OPC Core Client in the web UI.

### 2.1.7 Source Context

Device Management Service

### 2.1.8 Target Contexts

- Edge Application Layer (OPC Core Client)

### 2.1.9 Payload

#### 2.1.9.1 Schema

| Property | Value |
|----------|-------|
| Version | 1.0 |
| Correlation Id | Guid |
| Configuration Payload | JSON |

#### 2.1.9.2 Required Fields

- version
- correlationId
- configurationPayload

#### 2.1.9.3 Optional Fields

*No items available*

### 2.1.10.0 Frequency

low

### 2.1.11.0 Business Criticality

important

### 2.1.12.0 Data Source

| Property | Value |
|----------|-------|
| Database | PostgreSQL |
| Table | OpcServerConnection |
| Operation | update |

### 2.1.13.0 Routing

| Property | Value |
|----------|-------|
| Routing Key | tenants/{tenantId}/clients/{clientId}/commands/con... |
| Exchange | mqtt.topic |
| Queue | client-specific-subscription |

### 2.1.14.0 Consumers

- {'service': 'OPC Core Client', 'handler': 'Configuration & Update Module', 'processingType': 'async'}

### 2.1.15.0 Dependencies

- REQ-1-010
- REQ-1-062

### 2.1.16.0 Error Handling

| Property | Value |
|----------|-------|
| Retry Strategy | Client-side MQTT reconnect logic |
| Dead Letter Queue | Not applicable for commands; broker holds message ... |
| Timeout Ms | 60000 |

## 2.2.0.0 Event Id

### 2.2.1.0 Event Id

EVT-SYS-002

### 2.2.2.0 Event Name

ClientStatusReported

### 2.2.3.0 Event Type

system

### 2.2.4.0 Category

🔹 Device Management

### 2.2.5.0 Description

A status event sent from an OPC Core Client to the Central Management Plane, reporting its health and operational state.

### 2.2.6.0 Trigger Condition

On client startup, on a regular heartbeat interval, or on significant state change (e.g., connection to OPC server lost).

### 2.2.7.0 Source Context

Edge Application Layer (OPC Core Client)

### 2.2.8.0 Target Contexts

- Device Management Service

### 2.2.9.0 Payload

#### 2.2.9.1 Schema

| Property | Value |
|----------|-------|
| Version | 1.0 |
| Client Id | Guid |
| Timestamp | DateTimeOffset |
| Status | string (Online, Offline, Degraded) |
| Cpu Usage | float |
| Memory Usage | float |
| Software Version | string |

#### 2.2.9.2 Required Fields

- version
- clientId
- timestamp
- status

#### 2.2.9.3 Optional Fields

- cpuUsage
- memoryUsage
- softwareVersion

### 2.2.10.0 Frequency

medium

### 2.2.11.0 Business Criticality

normal

### 2.2.12.0 Data Source

| Property | Value |
|----------|-------|
| Database | N/A |
| Table | N/A |
| Operation | read |

### 2.2.13.0 Routing

| Property | Value |
|----------|-------|
| Routing Key | tenants/{tenantId}/clients/{clientId}/status |
| Exchange | mqtt.topic |
| Queue | central-status-queue |

### 2.2.14.0 Consumers

- {'service': 'Device Management Service', 'handler': 'ClientStatusHandler', 'processingType': 'async'}

### 2.2.15.0 Dependencies

- REQ-1-010
- REQ-1-062

### 2.2.16.0 Error Handling

| Property | Value |
|----------|-------|
| Retry Strategy | 3 retries with exponential backoff for DB persiste... |
| Dead Letter Queue | central-status-dlq |
| Timeout Ms | 5000 |

## 2.3.0.0 Event Id

### 2.3.1.0 Event Id

EVT-INT-003

### 2.3.2.0 Event Name

AnomalyDetected

### 2.3.3.0 Event Type

integration

### 2.3.4.0 Category

🔹 AI/ML

### 2.3.5.0 Description

An event indicating that an AI model running on the edge has detected an anomaly in the data stream.

### 2.3.6.0 Trigger Condition

The Edge AI Runtime Module executes a model and the resulting anomaly score exceeds a configured threshold.

### 2.3.7.0 Source Context

Edge Application Layer (OPC Core Client)

### 2.3.8.0 Target Contexts

- Alarm & Notification Service

### 2.3.9.0 Payload

#### 2.3.9.1 Schema

| Property | Value |
|----------|-------|
| Version | 1.0 |
| Model Assignment Id | Guid |
| Timestamp | DateTimeOffset |
| Anomaly Score | float |
| Triggering Value | float |

#### 2.3.9.2 Required Fields

- version
- modelAssignmentId
- timestamp
- anomalyScore

#### 2.3.9.3 Optional Fields

- triggeringValue

### 2.3.10.0 Frequency

low

### 2.3.11.0 Business Criticality

important

### 2.3.12.0 Data Source

| Property | Value |
|----------|-------|
| Database | N/A |
| Table | N/A |
| Operation | read |

### 2.3.13.0 Routing

| Property | Value |
|----------|-------|
| Routing Key | tenants/{tenantId}/events/anomaly |
| Exchange | mqtt.topic |
| Queue | central-anomaly-queue |

### 2.3.14.0 Consumers

- {'service': 'Alarm & Notification Service', 'handler': 'AnomalyEventHandler', 'processingType': 'async'}

### 2.3.15.0 Dependencies

- REQ-1-052

### 2.3.16.0 Error Handling

| Property | Value |
|----------|-------|
| Retry Strategy | 3 retries with exponential backoff |
| Dead Letter Queue | central-anomaly-dlq |
| Timeout Ms | 5000 |

# 3.0.0.0 Event Types And Schema Design

## 3.1.0.0 Essential Event Types

### 3.1.1.0 Event Name

#### 3.1.1.1 Event Name

Client Command Events

#### 3.1.1.2 Category

🔹 integration

#### 3.1.1.3 Description

Events sent from the cloud to the edge to manage the client's lifecycle and configuration.

#### 3.1.1.4 Priority

🔴 high

### 3.1.2.0 Event Name

#### 3.1.2.1 Event Name

Client Status & Heartbeat Events

#### 3.1.2.2 Category

🔹 integration

#### 3.1.2.3 Description

Events sent from the edge to the cloud to report health and operational status.

#### 3.1.2.4 Priority

🟡 medium

### 3.1.3.0 Event Name

#### 3.1.3.1 Event Name

Edge AI Insight Events

#### 3.1.3.2 Category

🔹 integration

#### 3.1.3.3 Description

Events generated by edge AI models (e.g., AnomalyDetected) sent to the cloud for further action.

#### 3.1.3.4 Priority

🟡 medium

## 3.2.0.0 Schema Design

| Property | Value |
|----------|-------|
| Format | JSON |
| Reasoning | JSON is lightweight, human-readable, and natively ... |
| Consistency Approach | A standardized event envelope will be used, contai... |

## 3.3.0.0 Schema Evolution

| Property | Value |
|----------|-------|
| Backward Compatibility | ✅ |
| Forward Compatibility | ❌ |
| Strategy | Additive changes only. New optional fields can be ... |

## 3.4.0.0 Event Structure

### 3.4.1.0 Standard Fields

- eventId
- eventName
- timestamp
- version
- correlationId

### 3.4.2.0 Metadata Requirements

- A correlation ID must be present in all command and subsequent response/status events to trace workflows end-to-end.

# 4.0.0.0 Event Routing And Processing

## 4.1.0.0 Routing Mechanisms

### 4.1.1.0 MQTT Topic-Based Routing

#### 4.1.1.1 Type

🔹 MQTT Topic-Based Routing

#### 4.1.1.2 Description

The primary mechanism for routing commands and status updates between the cloud and specific edge clients. Utilizes a hierarchical topic structure for granular addressing.

#### 4.1.1.3 Use Case

Cloud-to-Edge command and control messaging and Edge-to-Cloud status reporting as required by REQ-1-010.

### 4.1.2.0 gRPC Streaming

#### 4.1.2.1 Type

🔹 gRPC Streaming

#### 4.1.2.2 Description

A point-to-point, high-performance streaming mechanism for time-series data. While event-like, it is not a brokered pub/sub system.

#### 4.1.2.3 Use Case

High-volume, low-latency time-series data transfer from Edge to Cloud as required by REQ-1-010.

## 4.2.0.0 Processing Patterns

- {'pattern': 'sequential', 'applicableScenarios': ['Processing status updates from a single client to avoid race conditions when updating its state in the database.'], 'implementation': 'Ensure messages from a single client are processed by a single consumer instance at a time.'}

## 4.3.0.0 Filtering And Subscription

### 4.3.1.0 Filtering Mechanism

MQTT Topic Subscriptions

### 4.3.2.0 Subscription Model

Clients subscribe to their unique command topic. Cloud services subscribe to wildcard topics for status and events (e.g., tenants/+/clients/+/status).

### 4.3.3.0 Routing Keys

- tenants/{tenantId}/clients/{clientId}/commands/+
- tenants/{tenantId}/clients/+/status

## 4.4.0.0 Handler Isolation

| Property | Value |
|----------|-------|
| Required | ✅ |
| Approach | Microservices architecture provides natural handle... |
| Reasoning | Separation of concerns and independent scalability... |

## 4.5.0.0 Delivery Guarantees

| Property | Value |
|----------|-------|
| Level | at-least-once |
| Justification | Required for robust command and control. Losing a ... |
| Implementation | Use MQTT Quality of Service (QoS) Level 1 for all ... |

# 5.0.0.0 Event Storage And Replay

## 5.1.0.0 Persistence Requirements

| Property | Value |
|----------|-------|
| Required | ✅ |
| Duration | In-flight only |
| Reasoning | The MQTT Broker must persist messages for disconne... |

## 5.2.0.0 Event Sourcing

### 5.2.1.0 Necessary

❌ No

### 5.2.2.0 Justification

The architecture is based on a state-store model using PostgreSQL. Introducing event sourcing would add unnecessary complexity and is not required by any functional requirement. The Audit Log (REQ-1-040) provides a historical record of actions without being used for state reconstruction.

### 5.2.3.0 Scope

*No items available*

## 5.3.0.0 Technology Options

- {'technology': 'MQTT Broker (e.g., AWS IoT Core, EMQX)', 'suitability': 'high', 'reasoning': 'Directly specified in the architecture (REQ-1-010) for command and control, providing necessary features like QoS and persistent sessions for disconnected clients.'}

## 5.4.0.0 Replay Capabilities

### 5.4.1.0 Required

❌ No

### 5.4.2.0 Scenarios

*No items available*

### 5.4.3.0 Implementation

Replay is not a requirement. If a client reconnects, it will receive the latest configuration from the state store, not a replay of past events.

## 5.5.0.0 Retention Policy

| Property | Value |
|----------|-------|
| Strategy | Retain in broker until delivered |
| Duration | Dependent on MQTT persistent session expiry settin... |
| Archiving Approach | Events are archived as part of the application sta... |

# 6.0.0.0 Dead Letter Queue And Error Handling

## 6.1.0.0 Dead Letter Strategy

| Property | Value |
|----------|-------|
| Approach | Utilize MQTT v5's native Dead Letter Queue (DLQ) f... |
| Queue Configuration | A dedicated DLQ topic (e.g., `dlq/central-status`)... |
| Processing Logic | Messages in the DLQ trigger an alert via Alertmana... |

## 6.2.0.0 Retry Policies

- {'errorType': 'Transient Database Errors', 'maxRetries': 3, 'backoffStrategy': 'exponential', 'delayConfiguration': '100ms initial backoff'}

## 6.3.0.0 Poison Message Handling

| Property | Value |
|----------|-------|
| Detection Mechanism | A message that fails processing after all retries ... |
| Handling Strategy | The message is moved to the corresponding DLQ and ... |
| Alerting Required | ✅ |

## 6.4.0.0 Error Notification

### 6.4.1.0 Channels

- PagerDuty
- Slack

### 6.4.2.0 Severity

critical

### 6.4.3.0 Recipients

- On-call SRE Team

## 6.5.0.0 Recovery Procedures

- {'scenario': 'Malformed message in DLQ', 'procedure': '1. On-call engineer is alerted. 2. Engineer inspects the message payload in the DLQ. 3. If possible, manually correct and republish the message. 4. Identify the root cause (e.g., bug in client software) and create a ticket.', 'automationLevel': 'manual'}

# 7.0.0.0 Event Versioning Strategy

## 7.1.0.0 Schema Evolution Approach

| Property | Value |
|----------|-------|
| Strategy | Schema-on-read with strict backward compatibility. |
| Versioning Scheme | Semantic versioning (e.g., '1.0', '1.1') included ... |
| Migration Strategy | Consumers must be able to handle previous minor ve... |

## 7.2.0.0 Compatibility Requirements

| Property | Value |
|----------|-------|
| Backward Compatible | ✅ |
| Forward Compatible | ❌ |
| Reasoning | Ensures that deploying a new client version (produ... |

## 7.3.0.0 Version Identification

| Property | Value |
|----------|-------|
| Mechanism | A 'version' field within the event payload. |
| Location | payload |
| Format | major.minor (e.g., 1.0) |

## 7.4.0.0 Consumer Upgrade Strategy

| Property | Value |
|----------|-------|
| Approach | Consumers are updated to support the new event ver... |
| Rollout Strategy | Standard blue-green or canary deployment for micro... |
| Rollback Procedure | Standard deployment rollback procedures. |

## 7.5.0.0 Schema Registry

| Property | Value |
|----------|-------|
| Required | ❌ |
| Technology | N/A |
| Governance | Event schemas will be defined and version-controll... |

# 8.0.0.0 Event Monitoring And Observability

## 8.1.0.0 Monitoring Capabilities

### 8.1.1.0 Capability

#### 8.1.1.1 Capability

MQTT Broker Metrics

#### 8.1.1.2 Justification

To monitor the health of the core messaging infrastructure.

#### 8.1.1.3 Implementation

Scrape metrics from the MQTT broker using a Prometheus exporter (e.g., active connections, message rates, subscription count).

### 8.1.2.0 Capability

#### 8.1.2.1 Capability

Event Processing Latency

#### 8.1.2.2 Justification

To ensure commands and status updates are processed in a timely manner.

#### 8.1.2.3 Implementation

Measure the time delta between the event timestamp in the payload and the time it is processed by the consumer service.

### 8.1.3.0 Capability

#### 8.1.3.1 Capability

DLQ Size Monitoring

#### 8.1.3.2 Justification

To detect message processing failures immediately.

#### 8.1.3.3 Implementation

A Prometheus metric for the number of messages in any DLQ, with an alert firing if the count is greater than zero.

## 8.2.0.0 Tracing And Correlation

| Property | Value |
|----------|-------|
| Tracing Required | ✅ |
| Correlation Strategy | A unique `correlationId` will be generated by the ... |
| Trace Id Propagation | Use OpenTelemetry to propagate the `traceId` and `... |

## 8.3.0.0 Performance Metrics

### 8.3.1.0 Metric

#### 8.3.1.1 Metric

MQTT round-trip latency

#### 8.3.1.2 Threshold

< 200ms (P95)

#### 8.3.1.3 Alerting

✅ Yes

### 8.3.2.0 Metric

#### 8.3.2.1 Metric

Event processing throughput (msgs/sec)

#### 8.3.2.2 Threshold

N/A (monitor for anomalies)

#### 8.3.2.3 Alerting

❌ No

## 8.4.0.0 Event Flow Visualization

| Property | Value |
|----------|-------|
| Required | ✅ |
| Tooling | Grafana with data from OpenTelemetry (for traces) ... |
| Scope | Visualize the end-to-end flow of a command from th... |

## 8.5.0.0 Alerting Requirements

### 8.5.1.0 Condition

#### 8.5.1.1 Condition

Messages present in any DLQ for > 5 minutes.

#### 8.5.1.2 Severity

critical

#### 8.5.1.3 Response Time

15 minutes

#### 8.5.1.4 Escalation Path

- On-call SRE Team

### 8.5.2.0 Condition

#### 8.5.2.1 Condition

MQTT Broker is unavailable.

#### 8.5.2.2 Severity

critical

#### 8.5.2.3 Response Time

5 minutes

#### 8.5.2.4 Escalation Path

- On-call SRE Team

# 9.0.0.0 Implementation Priority

## 9.1.0.0 Component

### 9.1.1.0 Component

Core MQTT Command & Status Channel

### 9.1.2.0 Priority

🔴 high

### 9.1.3.0 Dependencies

- MQTT Broker Setup
- Device Management Service
- OPC Core Client

### 9.1.4.0 Estimated Effort

Medium

## 9.2.0.0 Component

### 9.2.1.0 Component

DLQ and Alerting Configuration

### 9.2.2.0 Priority

🟡 medium

### 9.2.3.0 Dependencies

- Core MQTT Channel
- Alertmanager setup

### 9.2.4.0 Estimated Effort

Low

## 9.3.0.0 Component

### 9.3.1.0 Component

Distributed Tracing Integration

### 9.3.2.0 Priority

🟡 medium

### 9.3.3.0 Dependencies

- Core MQTT Channel
- OpenTelemetry setup

### 9.3.4.0 Estimated Effort

Medium

# 10.0.0.0 Risk Assessment

## 10.1.0.0 Risk

### 10.1.1.0 Risk

MQTT Broker becomes a single point of failure.

### 10.1.2.0 Impact

high

### 10.1.3.0 Probability

low

### 10.1.4.0 Mitigation

Deploy the MQTT broker in a high-availability cluster configuration. Use a managed service like AWS IoT Core where possible.

## 10.2.0.0 Risk

### 10.2.1.0 Risk

Inability to process messages from the DLQ leads to data loss.

### 10.2.2.0 Impact

medium

### 10.2.3.0 Probability

medium

### 10.2.4.0 Mitigation

Implement robust alerting for the DLQ and have clear, documented procedures for manual inspection and reprocessing of failed messages.

## 10.3.0.0 Risk

### 10.3.1.0 Risk

Lack of idempotency in consumers causes data corruption from duplicate messages.

### 10.3.2.0 Impact

high

### 10.3.3.0 Probability

medium

### 10.3.4.0 Mitigation

Enforce idempotent design in all event-consuming handlers, for example by checking for existing state based on a unique identifier from the event payload.

# 11.0.0.0 Recommendations

## 11.1.0.0 Category

### 11.1.1.0 Category

🔹 Architecture

### 11.1.2.0 Recommendation

Strictly limit the use of event-driven communication to the specified asynchronous cloud-to-edge channel. Do not introduce a central message broker for inter-service communication in the backend, as the architecture explicitly calls for gRPC.

### 11.1.3.0 Justification

Adheres to the principle of using the right tool for the job (MQTT for IoT command/control, gRPC for low-latency internal APIs) and avoids unnecessary complexity and performance overhead.

### 11.1.4.0 Priority

🔴 high

## 11.2.0.0 Category

### 11.2.1.0 Category

🔹 Observability

### 11.2.2.0 Recommendation

Prioritize the implementation of distributed tracing with correlation IDs across the MQTT boundary at the beginning of the project.

### 11.2.3.0 Justification

Debugging issues that cross from the cloud to the edge and back is extremely difficult without end-to-end tracing. This capability is essential for operational readiness.

### 11.2.4.0 Priority

🔴 high

## 11.3.0.0 Category

### 11.3.1.0 Category

🔹 Governance

### 11.3.2.0 Recommendation

Maintain a version-controlled document or code artifact that serves as the single source of truth for all MQTT event schemas.

### 11.3.3.0 Justification

While a full schema registry is not required, a formal contract is needed to prevent inconsistencies between cloud and edge components, especially with a distributed team.

### 11.3.4.0 Priority

🟡 medium

