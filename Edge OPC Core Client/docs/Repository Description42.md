# 1 Id

REPO-EDG-OPC

# 2 Name

Edge OPC Core Client

# 3 Description

This repository contains the source code for the distributed, cross-platform OPC Core Client application. It is the primary edge component of the system, designed to be deployed on industrial hardware within the customer's on-premise environment (REQ-1-008). Its core responsibilities include connecting to various OPC servers (DA, UA, XML-DA), performing real-time data acquisition, and securely communicating with the Central Management Plane using a dual-protocol strategy: gRPC for high-throughput data streaming and MQTT for robust command and control (REQ-1-010). It is capable of autonomous operation, featuring an on-disk buffer for store-and-forward functionality during network outages (REQ-1-079). It also hosts the Edge AI module, running ML models locally for predictive maintenance and anomaly detection (REQ-1-056). The entire application is packaged as a Docker container for consistent deployment (REQ-1-020).

# 4 Type

🔹 IoT_Edge

# 5 Namespace

System.Edge.OpcCoreClient

# 6 Output Path

edge/opc-core-client

# 7 Framework

.NET Generic Host v8.0

# 8 Language

C# 12

# 9 Technology

.NET v8.0, Docker, gRPC, MQTT, ONNX Runtime

# 10 Thirdparty Libraries

- OPCFoundation.NetStandard.Opc.Ua v1.5.375.24
- Grpc.Net.Client v2.63.0
- MQTTnet v4.3.6
- Microsoft.ML.OnnxRuntime v1.18.0

# 11 Layer Ids

- edge

# 12 Dependencies

- REPO-SVC-DIN
- REPO-SVC-DVM
- REPO-MSG-MQTT
- REPO-LIB-SHARED

# 13 Requirements

## 13.1 Requirement Id

### 13.1.1 Requirement Id

REQ-1-008

## 13.2.0 Requirement Id

### 13.2.1 Requirement Id

REQ-1-010

## 13.3.0 Requirement Id

### 13.3.1 Requirement Id

REQ-1-079

## 13.4.0 Requirement Id

### 13.4.1 Requirement Id

REQ-1-056

# 14.0.0 Generate Tests

✅ Yes

# 15.0.0 Generate Documentation

✅ Yes

# 16.0.0 Architecture Style

LayeredArchitecture

# 17.0.0 Architecture Map

- opc-core-client-011

# 18.0.0 Components Map

- opc-core-client-011

# 19.0.0 Requirements Map

- REQ-ARC-001
- REQ-ENV-001
- REQ-FR-001
- REQ-FR-006
- REQ-FR-008
- REQ-FR-011
- REQ-FR-016
- REQ-NFR-002

# 20.0.0 Dependency Contracts

## 20.1.0 Repo-Svc-Din

### 20.1.1 Required Interfaces

- {'interface': 'IIngestionService (gRPC)', 'methods': ['rpc StreamData(stream DataPoint) returns (IngestAck)'], 'events': [], 'properties': []}

### 20.1.2 Integration Pattern

gRPC Client Streaming

### 20.1.3 Communication Protocol

gRPC over HTTP/2 with mTLS

## 20.2.0 Repo-Msg-Mqtt

### 20.2.1 Required Interfaces

- {'interface': 'IMqttClient', 'methods': ['PublishAsync(MqttApplicationMessage message)', 'SubscribeAsync(MqttTopicFilter topicFilter)'], 'events': ['ApplicationMessageReceivedAsync'], 'properties': []}

### 20.2.2 Integration Pattern

Publish-Subscribe Client

### 20.2.3 Communication Protocol

MQTT v5 over TLS

# 21.0.0 Exposed Contracts

## 21.1.0 Public Interfaces

### 21.1.1 Interface

#### 21.1.1.1 Interface

IHealthCheckEndpoint

#### 21.1.1.2 Methods

- GET /healthz
- GET /readyz

#### 21.1.1.3 Events

*No items available*

#### 21.1.1.4 Properties

*No items available*

#### 21.1.1.5 Consumers

- Kubernetes/Docker Orchestrator

### 21.1.2.0 Interface

#### 21.1.2.1 Interface

IMetricsEndpoint

#### 21.1.2.2 Methods

- GET /metrics

#### 21.1.2.3 Events

*No items available*

#### 21.1.2.4 Properties

*No items available*

#### 21.1.2.5 Consumers

- Prometheus Scraper

# 22.0.0.0 Integration Patterns

| Property | Value |
|----------|-------|
| Dependency Injection | Constructor Injection via .NET's Generic Host DI c... |
| Event Communication | Listens for commands on its dedicated MQTT topic a... |
| Data Flow | OPC Data -> Internal Queue -> gRPC Stream -> Cloud... |
| Error Handling | Retry with exponential backoff for cloud connectio... |
| Async Patterns | Hosted services (IHostedService) for managing OPC ... |

# 23.0.0.0 Scope Boundaries

## 23.1.0.0 Must Implement

- Connect to OPC DA, UA, and XML-DA servers.
- Subscribe to OPC tags and receive real-time updates.
- Implement a persistent on-disk buffer for offline data storage.
- Stream buffered and real-time data to the Ingestion Service via gRPC.
- Listen for and act on commands from the Device Management Service via MQTT.
- Run ONNX AI models for local inference.
- Expose /healthz and /metrics endpoints.

## 23.2.0.0 Must Not Implement

- Any user interface.
- Persist data long-term (only buffering is allowed).
- Manage users, tenants, or any cloud-side configuration.

## 23.3.0.0 Integration Points

- Local OPC Servers.
- Central Management Plane (gRPC and MQTT endpoints).

## 23.4.0.0 Architectural Constraints

- Must be packaged as a Docker container.
- Must be capable of fully autonomous operation during network outages.
- Must use the dual-protocol strategy for cloud communication.

# 24.0.0.0 Technology Standards

## 24.1.0.0 Framework Specific

| Property | Value |
|----------|-------|
| Pattern Usage | Use .NET Generic Host for running background servi... |
| Performance Requirements | Edge AI inference latency must be under 100ms (REQ... |
| Security Requirements | Implement secure provisioning workflow (REQ-NFR-00... |

# 25.0.0.0 Cognitive Load Instructions

## 25.1.0.0 Sds Generation Guidance

### 25.1.1.0 Focus Areas

- The state machine for managing connectivity (Connected, Disconnected, Buffering).
- The design of the persistent circular buffer.
- The interaction between the gRPC streaming service and the MQTT command service.
- The abstraction layer for different OPC protocols.

### 25.1.2.0 Avoid Patterns

- Blocking calls in the data acquisition loop.

## 25.2.0.0 Code Generation Guidance

### 25.2.1.0 Implementation Patterns

- Use `System.IO.Pipelines` for high-performance I/O for the disk buffer.
- Implement separate `IHostedService` instances for each major concurrent task.

