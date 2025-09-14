# 1 Integration Specifications

## 1.1 Extraction Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-EDG-OPC |
| Extraction Timestamp | 2024-07-31T12:00:00Z |
| Mapping Validation Score | 100% |
| Context Completeness Score | 98% |
| Implementation Readiness Level | High |

## 1.2 Relevant Requirements

### 1.2.1 Requirement Id

#### 1.2.1.1 Requirement Id

REQ-1-010

#### 1.2.1.2 Requirement Text

Communication between the OPC Core Client and the Central Management Plane shall be secured and optimized using a dual-protocol strategy. High-volume, low-latency data streaming shall use gRPC with mutual TLS (mTLS) authentication. Command, control, and status messaging shall use MQTT v5 over a standard TLS connection to ensure robust delivery.

#### 1.2.1.3 Validation Criteria

- Time-series data is exclusively streamed to the Data Ingestion Service via a gRPC client stream.
- All commands (e.g., configuration update, software update) are received via a subscription to a client-specific MQTT topic.
- Client status updates and events (e.g., anomaly detected) are published to appropriate MQTT topics.

#### 1.2.1.4 Implementation Implications

- The application must implement both a gRPC client for the data plane and an MQTT v5 client for the control plane.
- A robust state machine is required to manage the lifecycle of both connections.
- Shared logic for loading and using the client certificate for authentication must be implemented for both protocols.

#### 1.2.1.5 Extraction Reasoning

This requirement is the cornerstone of the client's integration with the cloud, defining the fundamental communication architecture and its dual-protocol nature. It directly dictates the primary dependency interfaces.

### 1.2.2.0 Requirement Id

#### 1.2.2.1 Requirement Id

REQ-1-079

#### 1.2.2.2 Requirement Text

The OPC Core Client must implement a persistent on-disk buffer to store data during network disconnects. The buffer's maximum size shall be configurable, defaulting to the smaller of 1 GB or 24 hours of data. If the buffer becomes full, an alert shall be logged, and the client will operate as a circular buffer, overwriting the oldest data. Upon reconnection to the Central Management Plane, the client must automatically transmit all data from the buffer.

#### 1.2.2.3 Validation Criteria

- Data is successfully written to a persistent on-disk buffer when cloud connectivity is lost.
- Upon reconnection, all buffered data is transmitted to the Data Ingestion Service in the correct chronological order.
- The buffer correctly overwrites the oldest data when its capacity is exceeded.

#### 1.2.2.4 Implementation Implications

- A file-based circular buffer mechanism must be developed for efficient and reliable on-disk persistence.
- A state machine must manage the client's operational mode (Online, Offline, Recovery) to control the data flow between the OPC source, the buffer, and the gRPC stream.

#### 1.2.2.5 Extraction Reasoning

This requirement defines the client's core resilience and autonomous operation capability, which is a critical aspect of its integration pattern with the cloud services, especially under unreliable network conditions.

### 1.2.3.0 Requirement Id

#### 1.2.3.1 Requirement Id

REQ-1-056

#### 1.2.3.2 Requirement Text

The system must support Edge AI by enabling the deployment and execution of AI models on remote edge hardware (e.g., NVIDIA Jetson).

#### 1.2.3.3 Validation Criteria

- The client can receive a model deployment command via MQTT.
- The client can securely download the ONNX model file.
- The client can load and execute the model using the ONNX Runtime for real-time inference.
- Inference results (e.g., anomaly scores) are processed and can be published as events via MQTT.

#### 1.2.3.4 Implementation Implications

- The ONNX Runtime library must be integrated into the application.
- A module is required to manage the model lifecycle (download, validation, loading, execution).
- The command and control logic must be extended to handle model deployment commands.

#### 1.2.3.5 Extraction Reasoning

This requirement defines a key edge computing function of the client, which involves receiving deployment commands and models from the Central Management Plane, representing a critical integration point for MLOps.

### 1.2.4.0 Requirement Id

#### 1.2.4.1 Requirement Id

REQ-1-082

#### 1.2.4.2 Requirement Text

The system must implement a secure provisioning workflow for new OPC Core Client instances using a single-use registration token.

#### 1.2.4.3 Validation Criteria

- The client can be configured with a one-time token.
- On first run, the client makes a REST call to the provisioning endpoint with the token.
- The client generates a key pair and submits a CSR.
- The client receives and securely stores its unique, long-term X.509 certificate.

#### 1.2.4.4 Implementation Implications

- A one-time bootstrapping/provisioning module must be implemented.
- The client needs logic to generate a key pair and CSR.
- Secure local storage for the private key and certificate must be implemented.

#### 1.2.4.5 Extraction Reasoning

This requirement defines the critical, initial integration step that establishes the client's trusted identity, which is the foundation for all subsequent secure communications.

## 1.3.0.0 Relevant Components

### 1.3.1.0 Component Name

#### 1.3.1.1 Component Name

Cloud Communication Module

#### 1.3.1.2 Component Specification

Manages all communication with the Central Management Plane. Implements the dual-protocol strategy, handling the gRPC stream for data and the MQTT client for commands and status.

#### 1.3.1.3 Implementation Requirements

- Implement a resilient gRPC client for streaming data points to REPO-SVC-DIN.
- Implement a resilient MQTT client to subscribe to command topics and publish status/events to/from REPO-SVC-DVM.
- Manage mTLS/TLS credentials for secure communication.
- Coordinate with the Connectivity State Manager to determine data routing (cloud vs. local buffer).

#### 1.3.1.4 Architectural Context

Internal component within the Edge Application Layer.

#### 1.3.1.5 Extraction Reasoning

This component directly implements the core integration pattern defined in REQ-1-010.

### 1.3.2.0 Component Name

#### 1.3.2.1 Component Name

Provisioning & Update Module

#### 1.3.2.2 Component Specification

Manages the client's lifecycle, including the initial secure bootstrapping process and subsequent software/configuration updates.

#### 1.3.2.3 Implementation Requirements

- Implement the one-time REST call to REPO-SVC-DVM to exchange a token for a certificate.
- Handle MQTT commands for configuration updates and software updates.
- For software updates, execute an external script to pull a new Docker image and restart the container.

#### 1.3.2.4 Architectural Context

Internal component within the Edge Application Layer.

#### 1.3.2.5 Extraction Reasoning

This component implements the critical security and maintenance integration points for lifecycle management (REQ-1-082, REQ-1-064).

## 1.4.0.0 Architectural Layers

- {'layer_name': 'Edge Application Layer (OPC Core Client)', 'layer_responsibilities': ['Connect to industrial control systems via OPC protocols.', 'Operate autonomously and buffer data during network disconnects.', 'Stream high-volume data via gRPC and use MQTT for command/control.', 'Execute AI/ML models in ONNX format for real-time edge inference.', 'Manage its own lifecycle, including secure provisioning and software updates.'], 'layer_constraints': ['Must be packaged as a Docker container.', 'Must be capable of running on both Windows and Linux.', 'Must not implement any direct user interface.', 'Must only persist data for short-term buffering.'], 'implementation_patterns': ['Hosted Services (.NET IHostedService) for concurrent background tasks.', 'State Machine for managing connectivity and operational modes.', 'Producer-Consumer Queue for decoupling data acquisition from data transmission.', 'Store-and-Forward using a persistent on-disk buffer.'], 'extraction_reasoning': 'This is the primary architectural layer that the REPO-EDG-OPC repository is responsible for implementing, as defined in the solution architecture document.'}

## 1.5.0.0 Dependency Interfaces

### 1.5.1.0 Interface Name

#### 1.5.1.1 Interface Name

IIngestionService

#### 1.5.1.2 Source Repository

REPO-SVC-DIN

#### 1.5.1.3 Method Contracts

- {'method_name': 'StreamData', 'method_signature': 'rpc StreamData(stream DataPoint) returns (IngestAck);', 'method_purpose': 'To provide a persistent, high-performance channel for the edge client to continuously stream time-series data points to the cloud.', 'integration_context': "This RPC is established when the client is in an 'Online' or 'Recovery' state to send either real-time or buffered historical data. The contract for DataPoint and IngestAck messages is defined in REPO-LIB-SHARED."}

#### 1.5.1.4 Integration Pattern

gRPC Client Streaming

#### 1.5.1.5 Communication Protocol

gRPC over HTTP/2 with Mutual TLS (mTLS)

#### 1.5.1.6 Extraction Reasoning

This is the primary data-plane dependency for the client to send its collected data to the cloud, as defined in REQ-1-010 and detailed in its SDS.

### 1.5.2.0 Interface Name

#### 1.5.2.1 Interface Name

IProvisioningService

#### 1.5.2.2 Source Repository

REPO-SVC-DVM

#### 1.5.2.3 Method Contracts

- {'method_name': 'RegisterClient', 'method_signature': 'POST /provision/register', 'method_purpose': 'To allow a new, unprovisioned client to securely authenticate using a one-time token, submit a Certificate Signing Request (CSR), and receive a signed client certificate.', 'integration_context': 'This is a one-time bootstrapping call made by the client on its first startup to establish its identity and acquire the credentials needed for all subsequent mTLS/TLS communication.'}

#### 1.5.2.4 Integration Pattern

RESTful Request/Response

#### 1.5.2.5 Communication Protocol

HTTPS

#### 1.5.2.6 Extraction Reasoning

This is a critical bootstrap dependency required for the client to securely join the system, as detailed in REQ-1-082 and the SDS for REPO-SVC-DVM.

### 1.5.3.0 Interface Name

#### 1.5.3.1 Interface Name

IMqttCommandChannel

#### 1.5.3.2 Source Repository

REPO-SVC-DVM

#### 1.5.3.3 Method Contracts

##### 1.5.3.3.1 Method Name

###### 1.5.3.3.1.1 Method Name

SubscribeToCommands

###### 1.5.3.3.1.2 Method Signature

SUBSCRIBE tenants/{tenantId}/clients/{clientId}/commands

###### 1.5.3.3.1.3 Method Purpose

To receive asynchronous commands from the Device Management Service, such as configuration updates, software update triggers, and log retrieval requests.

###### 1.5.3.3.1.4 Integration Context

The client subscribes to this topic upon establishing a connection to the MQTT broker. It must be prepared to handle various command payloads defined in REPO-LIB-SHARED.

##### 1.5.3.3.2.0 Method Name

###### 1.5.3.3.2.1 Method Name

PublishStatus

###### 1.5.3.3.2.2 Method Signature

PUBLISH tenants/{tenantId}/clients/{clientId}/status

###### 1.5.3.3.2.3 Method Purpose

To periodically send health and status updates (heartbeats), and to acknowledge the receipt and execution of commands.

###### 1.5.3.3.2.4 Integration Context

The client publishes to this topic on a regular interval and after processing any command.

#### 1.5.3.4.0.0 Integration Pattern

Publish-Subscribe

#### 1.5.3.5.0.0 Communication Protocol

MQTT v5 over TLS with client certificate authentication

#### 1.5.3.6.0.0 Extraction Reasoning

This is the primary control-plane dependency, essential for remote management and configuration, as defined in REQ-1-010.

### 1.5.4.0.0.0 Interface Name

#### 1.5.4.1.0.0 Interface Name

Shared Contracts

#### 1.5.4.2.0.0 Source Repository

REPO-LIB-SHARED

#### 1.5.4.3.0.0 Method Contracts

*No items available*

#### 1.5.4.4.0.0 Integration Pattern

NuGet Package Reference (Compile-time)

#### 1.5.4.5.0.0 Communication Protocol

N/A

#### 1.5.4.6.0.0 Extraction Reasoning

This library is a critical compile-time dependency that provides the gRPC and MQTT message contracts, ensuring type safety and consistency between the client and the cloud services.

## 1.6.0.0.0.0 Exposed Interfaces

### 1.6.1.0.0.0 Interface Name

#### 1.6.1.1.0.0 Interface Name

IHealthCheckEndpoint

#### 1.6.1.2.0.0 Consumer Repositories

- Container Orchestrator (e.g., Kubernetes, Docker)

#### 1.6.1.3.0.0 Method Contracts

##### 1.6.1.3.1.0 Method Name

###### 1.6.1.3.1.1 Method Name

GET /healthz

###### 1.6.1.3.1.2 Method Signature

HTTP GET to /healthz

###### 1.6.1.3.1.3 Method Purpose

Provides a liveness probe to indicate that the application process is running and has not crashed.

###### 1.6.1.3.1.4 Implementation Requirements

Should return HTTP 200 OK as long as the main .NET Generic Host process is active. Must not depend on external connections.

##### 1.6.1.3.2.0 Method Name

###### 1.6.1.3.2.1 Method Name

GET /readyz

###### 1.6.1.3.2.2 Method Signature

HTTP GET to /readyz

###### 1.6.1.3.2.3 Method Purpose

Provides a readiness probe to indicate that the application is fully initialized and ready to perform its core functions.

###### 1.6.1.3.2.4 Implementation Requirements

Should return HTTP 200 OK only when all startup tasks are complete and the client is in a healthy operational state (e.g., connected to local OPC server).

#### 1.6.1.4.0.0 Service Level Requirements

- Response time must be < 50ms.

#### 1.6.1.5.0.0 Implementation Constraints

- The endpoint must be exposed on a local HTTP port within the container.

#### 1.6.1.6.0.0 Extraction Reasoning

This is a standard cloud-native pattern for operational health monitoring, specified in REQ-1-091, and is essential for robust operation in a containerized environment.

### 1.6.2.0.0.0 Interface Name

#### 1.6.2.1.0.0 Interface Name

IMetricsEndpoint

#### 1.6.2.2.0.0 Consumer Repositories

- Prometheus Scraper

#### 1.6.2.3.0.0 Method Contracts

- {'method_name': 'GET /metrics', 'method_signature': 'HTTP GET to /metrics', 'method_purpose': 'Exposes internal application metrics in the Prometheus text-based format for monitoring and alerting.', 'implementation_requirements': "Metrics must include buffer size, data points processed per second, connection status flags (OPC and cloud), and AI inference latency. A library like 'prometheus-net' must be used."}

#### 1.6.2.4.0.0 Service Level Requirements

- Response time must be < 200ms.

#### 1.6.2.5.0.0 Implementation Constraints

- The endpoint must be exposed on a local HTTP port within the container.

#### 1.6.2.6.0.0 Extraction Reasoning

This interface is required to fulfill the system's comprehensive observability strategy (REQ-1-090) and is a standard requirement for any service in the architecture.

## 1.7.0.0.0.0 Technology Context

### 1.7.1.0.0.0 Framework Requirements

.NET 8 using the Generic Host (IHostedService pattern) for managing long-running, concurrent background tasks.

### 1.7.2.0.0.0 Integration Technologies

- Docker: The application must be fully containerized.
- gRPC (Grpc.Net.Client): For high-performance data streaming to the cloud.
- MQTT (MQTTnet): For command, control, and event messaging.
- ONNX Runtime (Microsoft.ML.OnnxRuntime): For executing local AI/ML models.

### 1.7.3.0.0.0 Performance Constraints

Edge AI model inference latency must be under 100ms. The data pipeline must be non-blocking to handle high-frequency data from OPC servers without data loss.

### 1.7.4.0.0.0 Security Requirements

All communication with the cloud (gRPC, MQTT, REST) must be encrypted using TLS 1.3. Client identity must be established via a unique X.509 certificate obtained through the secure provisioning workflow. gRPC must use mTLS.

## 1.8.0.0.0.0 Extraction Validation

| Property | Value |
|----------|-------|
| Mapping Completeness Check | The client's responsibilities are fully mapped to ... |
| Cross Reference Validation | Information is consistent across all sources. The ... |
| Implementation Readiness Assessment | High. The context specifies the framework (.NET Ge... |
| Quality Assurance Confirmation | A systematic review confirms that all defined requ... |

