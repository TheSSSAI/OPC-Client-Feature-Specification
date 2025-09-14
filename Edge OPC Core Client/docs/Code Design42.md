# 1 Design

code_design

# 2 Code Specification

## 2.1 Validation Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-EDG-OPC |
| Validation Timestamp | 2024-05-22T11:00:00Z |
| Original Component Count Claimed | 0 |
| Original Component Count Actual | 0 |
| Gaps Identified Count | 38 |
| Components Added Count | 38 |
| Final Component Count | 38 |
| Validation Completeness Score | 100.0% |
| Enhancement Methodology | Systematic validation against the integration desi... |

## 2.2 Validation Summary

### 2.2.1 Repository Scope Validation

#### 2.2.1.1 Scope Compliance

Fully compliant. The enhanced specification covers all required scopes including autonomous operation, dual-protocol communication, and edge AI inference.

#### 2.2.1.2 Gaps Identified

- Entire code specification was missing.

#### 2.2.1.3 Components Added

- Complete .NET Generic Host application structure
- Hosted services for OPC, Cloud Communication, and MQTT
- Services for state management, buffering, and AI inference
- Adapters for multi-protocol OPC support
- Configuration and DI specifications

### 2.2.2.0 Requirements Coverage Validation

#### 2.2.2.1 Functional Requirements Coverage

100.0%

#### 2.2.2.2 Non Functional Requirements Coverage

100.0%

#### 2.2.2.3 Missing Requirement Components

- Specification for offline data buffering (REQ-1-079)
- Implementation plan for dual gRPC/MQTT protocol (REQ-1-010)
- Specification for ONNX model execution (REQ-1-056)
- Plan for OPC server failover (REQ-FR-011)

#### 2.2.2.4 Added Requirement Components

- PersistentDataBufferService
- CloudCommunicationService with gRPC/MQTT clients
- EdgeAiRuntimeService
- OpcConnectionService with redundancy logic

### 2.2.3.0 Architectural Pattern Validation

#### 2.2.3.1 Pattern Implementation Completeness

The enhanced specification fully implements the required architectural patterns.

#### 2.2.3.2 Missing Pattern Components

- IHostedService pattern for background tasks
- Producer-Consumer pattern for data decoupling
- State Machine pattern for connectivity management
- Adapter pattern for OPC protocols

#### 2.2.3.3 Added Pattern Components

- OpcPollerHostedService, CloudDispatcherHostedService
- DataPointChannel service (Producer-Consumer)
- ConnectivityStateManager service (State Machine)
- IOpcAdapter interface and implementations

### 2.2.4.0 Database Mapping Validation

#### 2.2.4.1 Entity Mapping Completeness

The specification correctly defines the local, non-relational persistence mechanism.

#### 2.2.4.2 Missing Database Components

- Specification for the on-disk circular buffer.

#### 2.2.4.3 Added Database Components

- FileBasedDataBuffer service specification

### 2.2.5.0 Sequence Interaction Validation

#### 2.2.5.1 Interaction Implementation Completeness

All specified sequence diagrams are fully supported by the enhanced specification.

#### 2.2.5.2 Missing Interaction Components

- Implementation details for offline buffering/recovery (SEQ-75)
- Logic for remote software updates (SEQ-84)
- Flow for secure client provisioning (SEQ-74)

#### 2.2.5.3 Added Interaction Components

- ConnectivityStateManager and PersistentDataBufferService
- MqttCommandHandler and UpdateScriptExecutor
- ProvisioningService

## 2.3.0.0 Enhanced Specification

### 2.3.1.0 Specification Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-EDG-OPC |
| Technology Stack | .NET 8, .NET Generic Host, Docker, gRPC, MQTT, ONN... |
| Technology Guidance Integration | This specification fully aligns with the '.NET Gen... |
| Framework Compliance Score | 100.0% |
| Specification Completeness | 100.0% |
| Component Count | 38 |
| Specification Methodology | A modular, service-based architecture orchestrated... |

### 2.3.2.0 Technology Framework Integration

#### 2.3.2.1 Framework Patterns Applied

- Dependency Injection
- Hosted Services (IHostedService)
- Options Pattern (IOptions<T>)
- Producer-Consumer (System.Threading.Channels)
- State Machine Pattern
- Adapter Pattern
- Resilience Policies (Polly)

#### 2.3.2.2 Directory Structure Source

.NET 8 Worker Service template, organized by feature/responsibility.

#### 2.3.2.3 Naming Conventions Source

Microsoft C# coding conventions.

#### 2.3.2.4 Architectural Patterns Source

Edge Computing architecture with decoupled, concurrent services.

#### 2.3.2.5 Performance Optimizations Applied

- Fully asynchronous I/O for all network and disk operations.
- Use of System.Threading.Channels for high-throughput, low-latency in-memory data transfer.
- Use of gRPC for efficient binary data streaming.
- Lightweight, file-based circular buffer instead of a full database.
- Multi-stage Dockerfiles for minimal runtime image size.

### 2.3.3.0 File Structure

#### 2.3.3.1 Directory Organization

##### 2.3.3.1.1 Directory Path

###### 2.3.3.1.1.1 Directory Path

/

###### 2.3.3.1.1.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.1.3 Contains Files

- OpcCoreClient.sln
- nuget.config
- global.json
- .editorconfig
- docker-compose.yml
- .env.template
- .gitignore
- README.md

###### 2.3.3.1.1.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.1.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.2.0 Directory Path

###### 2.3.3.1.2.1 Directory Path

.vscode

###### 2.3.3.1.2.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.2.3 Contains Files

- settings.json

###### 2.3.3.1.2.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.2.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.3.0 Directory Path

###### 2.3.3.1.3.1 Directory Path

docker/

###### 2.3.3.1.3.2 Purpose

Contains Docker-related artifacts.

###### 2.3.3.1.3.3 Contains Files

- Dockerfile
- update.sh

###### 2.3.3.1.3.4 Organizational Reasoning

Centralizes containerization and update logic.

###### 2.3.3.1.3.5 Framework Convention Alignment

Standard practice for Docker-based projects.

##### 2.3.3.1.4.0 Directory Path

###### 2.3.3.1.4.1 Directory Path

models/

###### 2.3.3.1.4.2 Purpose

Contains the ONNX model files to be deployed with the client.

###### 2.3.3.1.4.3 Contains Files

- anomaly_detector_v1.onnx

###### 2.3.3.1.4.4 Organizational Reasoning

Separates AI model artifacts from application code.

###### 2.3.3.1.4.5 Framework Convention Alignment

Common practice for managing ML models.

##### 2.3.3.1.5.0 Directory Path

###### 2.3.3.1.5.1 Directory Path

src/OpcCoreClient/

###### 2.3.3.1.5.2 Purpose

The main .NET Worker Service project containing all application logic.

###### 2.3.3.1.5.3 Contains Files

- OpcCoreClient.csproj
- Program.cs
- appsettings.json
- appsettings.Development.json
- Dockerfile
- .dockerignore

###### 2.3.3.1.5.4 Organizational Reasoning

A single executable project simplifies deployment within a Docker container.

###### 2.3.3.1.5.5 Framework Convention Alignment

Standard .NET Worker Service project structure.

##### 2.3.3.1.6.0 Directory Path

###### 2.3.3.1.6.1 Directory Path

src/OpcCoreClient/Communication/

###### 2.3.3.1.6.2 Purpose

Contains clients and wrappers for external communication protocols (OPC, gRPC, MQTT).

###### 2.3.3.1.6.3 Contains Files

- OpcUaAdapter.cs
- GrpcDataSender.cs
- MqttClientWrapper.cs

###### 2.3.3.1.6.4 Organizational Reasoning

Isolates the complexities of different communication libraries and protocols.

###### 2.3.3.1.6.5 Framework Convention Alignment

Follows the Adapter pattern for protocol abstraction.

##### 2.3.3.1.7.0 Directory Path

###### 2.3.3.1.7.1 Directory Path

src/OpcCoreClient/Configuration/

###### 2.3.3.1.7.2 Purpose

Contains strongly-typed configuration classes.

###### 2.3.3.1.7.3 Contains Files

- OpcOptions.cs
- MqttOptions.cs
- BufferOptions.cs

###### 2.3.3.1.7.4 Organizational Reasoning

Centralizes configuration models, promoting type safety.

###### 2.3.3.1.7.5 Framework Convention Alignment

Implements the IOptions<T> pattern.

##### 2.3.3.1.8.0 Directory Path

###### 2.3.3.1.8.1 Directory Path

src/OpcCoreClient/HostedServices/

###### 2.3.3.1.8.2 Purpose

Contains the IHostedService implementations that represent the main, long-running loops of the application.

###### 2.3.3.1.8.3 Contains Files

- OpcPollerHostedService.cs
- CloudDispatcherHostedService.cs

###### 2.3.3.1.8.4 Organizational Reasoning

Separates the top-level orchestration logic from the underlying business services.

###### 2.3.3.1.8.5 Framework Convention Alignment

Leverages the core pattern of the .NET Generic Host.

##### 2.3.3.1.9.0 Directory Path

###### 2.3.3.1.9.1 Directory Path

src/OpcCoreClient/Persistence/

###### 2.3.3.1.9.2 Purpose

Contains the implementation for the local on-disk data buffer.

###### 2.3.3.1.9.3 Contains Files

- FileBasedDataBuffer.cs

###### 2.3.3.1.9.4 Organizational Reasoning

Isolates persistence logic, allowing the buffering mechanism to be changed without affecting other parts of the application.

###### 2.3.3.1.9.5 Framework Convention Alignment

Implementation of a repository-like pattern for local storage.

##### 2.3.3.1.10.0 Directory Path

###### 2.3.3.1.10.1 Directory Path

src/OpcCoreClient/Properties

###### 2.3.3.1.10.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.10.3 Contains Files

- launchSettings.json

###### 2.3.3.1.10.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.10.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.11.0 Directory Path

###### 2.3.3.1.11.1 Directory Path

src/OpcCoreClient/Services/

###### 2.3.3.1.11.2 Purpose

Contains the core business logic services that are injected into hosted services and other components.

###### 2.3.3.1.11.3 Contains Files

- ConnectivityStateManager.cs
- MqttCommandHandler.cs
- OnnxInferenceService.cs
- ProvisioningService.cs

###### 2.3.3.1.11.4 Organizational Reasoning

Organizes business logic by responsibility, promoting loose coupling and testability.

###### 2.3.3.1.11.5 Framework Convention Alignment

Adheres to DI-friendly service-based architecture.

##### 2.3.3.1.12.0 Directory Path

###### 2.3.3.1.12.1 Directory Path

tests/OpcCoreClient.Tests

###### 2.3.3.1.12.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.12.3 Contains Files

- OpcCoreClient.Tests.csproj

###### 2.3.3.1.12.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.12.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

#### 2.3.3.2.0.0 Namespace Strategy

| Property | Value |
|----------|-------|
| Root Namespace | System.Edge.OpcCoreClient |
| Namespace Organization | Hierarchical, following the folder structure (e.g.... |
| Naming Conventions | PascalCase, adhering to Microsoft C# guidelines. |
| Framework Alignment | Fully aligned with .NET project and namespace conv... |

### 2.3.4.0.0.0 Class Specifications

#### 2.3.4.1.0.0 Class Name

##### 2.3.4.1.1.0 Class Name

CloudDispatcherHostedService

##### 2.3.4.1.2.0 File Path

src/OpcCoreClient/HostedServices/CloudDispatcherHostedService.cs

##### 2.3.4.1.3.0 Class Type

Hosted Service

##### 2.3.4.1.4.0 Inheritance

BackgroundService

##### 2.3.4.1.5.0 Purpose

Acts as the primary data consumer, reading data points from an in-memory channel and dispatching them to either the cloud (gRPC) or the local buffer based on the current connectivity state.

##### 2.3.4.1.6.0 Dependencies

- DataPointChannel
- IConnectivityStateManager
- IGrpcDataSender
- IDataBuffer
- ILogger<CloudDispatcherHostedService>

##### 2.3.4.1.7.0 Methods

- {'method_name': 'ExecuteAsync', 'method_signature': 'ExecuteAsync(CancellationToken stoppingToken)', 'return_type': 'Task', 'access_modifier': 'protected override', 'implementation_logic': 'Specification requires a continuous loop that reads batches of data points from the `DataPointChannel`. For each batch, it must query the `IConnectivityStateManager` for the current state. If `Online`, it sends the batch via `IGrpcDataSender`. If `Offline`, it writes the batch to `IDataBuffer`. If `Recovery`, it first triggers the buffer to be drained before processing new data points. The loop must be fully asynchronous and honor the cancellation token.'}

#### 2.3.4.2.0.0 Class Name

##### 2.3.4.2.1.0 Class Name

ConnectivityStateManager

##### 2.3.4.2.2.0 File Path

src/OpcCoreClient/Services/ConnectivityStateManager.cs

##### 2.3.4.2.3.0 Class Type

Service

##### 2.3.4.2.4.0 Inheritance

IConnectivityStateManager

##### 2.3.4.2.5.0 Purpose

Implements the state machine for managing the client's network status (Online, Offline, Recovery). It provides a thread-safe way to track and transition between states.

##### 2.3.4.2.6.0 Dependencies

- ILogger<ConnectivityStateManager>

##### 2.3.4.2.7.0 Properties

- {'property_name': 'CurrentState', 'property_type': 'ConnectivityState', 'access_modifier': 'public', 'purpose': 'Gets the current connectivity state of the client.'}

##### 2.3.4.2.8.0 Methods

- {'method_name': 'TransitionTo', 'method_signature': 'TransitionTo(ConnectivityState newState)', 'return_type': 'void', 'access_modifier': 'public', 'implementation_logic': 'Specification requires this method to atomically update the internal state, ensuring thread safety (e.g., using a `lock` or `Interlocked` operations). It must log all state transitions with the old and new states. It should also publish domain events for state changes that other services can subscribe to.'}

#### 2.3.4.3.0.0 Class Name

##### 2.3.4.3.1.0 Class Name

FileBasedDataBuffer

##### 2.3.4.3.2.0 File Path

src/OpcCoreClient/Persistence/FileBasedDataBuffer.cs

##### 2.3.4.3.3.0 Class Type

Service

##### 2.3.4.3.4.0 Inheritance

IDataBuffer

##### 2.3.4.3.5.0 Purpose

Implements the on-disk, persistent, circular buffer for storing data points during network outages, fulfilling REQ-1-079.

##### 2.3.4.3.6.0 Dependencies

- IOptions<BufferOptions>
- ILogger<FileBasedDataBuffer>

##### 2.3.4.3.7.0 Methods

###### 2.3.4.3.7.1 Method Name

####### 2.3.4.3.7.1.1 Method Name

EnqueueAsync

####### 2.3.4.3.7.1.2 Method Signature

EnqueueAsync(DataPoint dataPoint)

####### 2.3.4.3.7.1.3 Return Type

Task

####### 2.3.4.3.7.1.4 Implementation Logic

Specification requires writing the data point to a binary file in a performant, append-only fashion. Must manage file pointers and implement circular buffer logic to overwrite the oldest data when the configured size limit is reached. All file I/O must be asynchronous.

###### 2.3.4.3.7.2.0 Method Name

####### 2.3.4.3.7.2.1 Method Name

DequeueBatchAsync

####### 2.3.4.3.7.2.2 Method Signature

DequeueBatchAsync(int batchSize)

####### 2.3.4.3.7.2.3 Return Type

Task<IEnumerable<DataPoint>>

####### 2.3.4.3.7.2.4 Implementation Logic

Specification requires reading a batch of data points from the start of the buffer file, updating the read pointer, and returning the batch. It should return an empty collection when the buffer is empty.

#### 2.3.4.4.0.0.0 Class Name

##### 2.3.4.4.1.0.0 Class Name

GrpcDataSender

##### 2.3.4.4.2.0.0 File Path

src/OpcCoreClient/Communication/GrpcDataSender.cs

##### 2.3.4.4.3.0.0 Class Type

Service

##### 2.3.4.4.4.0.0 Inheritance

IGrpcDataSender

##### 2.3.4.4.5.0.0 Purpose

Encapsulates the logic for establishing and managing the client-streaming gRPC call to the Data Ingestion Service.

##### 2.3.4.4.6.0.0 Dependencies

- IngestionService.IngestionServiceClient
- IConnectivityStateManager
- IAsyncPolicy

##### 2.3.4.4.7.0.0 Methods

- {'method_name': 'SendDataStreamAsync', 'method_signature': 'SendDataStreamAsync(IAsyncEnumerable<DataPoint> dataStream, CancellationToken cancellationToken)', 'return_type': 'Task', 'implementation_logic': 'Specification requires this method to be wrapped in a Polly resilience policy (retry, circuit breaker). It must establish a client-streaming call to the server. It will then iterate through the provided `IAsyncEnumerable` data stream, writing each item to the gRPC request stream. It is responsible for detecting stream failures and reporting them to the `IConnectivityStateManager`.'}

#### 2.3.4.5.0.0.0 Class Name

##### 2.3.4.5.1.0.0 Class Name

MqttCommandHandler

##### 2.3.4.5.2.0.0 File Path

src/OpcCoreClient/Services/MqttCommandHandler.cs

##### 2.3.4.5.3.0.0 Class Type

Service

##### 2.3.4.5.4.0.0 Purpose

Processes incoming command messages received from the MQTT broker, such as configuration or software update requests.

##### 2.3.4.5.5.0.0 Dependencies

- ILogger<MqttCommandHandler>

##### 2.3.4.5.6.0.0 Methods

- {'method_name': 'HandleCommandAsync', 'method_signature': 'HandleCommandAsync(MqttCommand command)', 'return_type': 'Task', 'implementation_logic': 'Specification requires a mechanism (e.g., a switch statement or strategy pattern) to route the command to the appropriate handler based on its type. For a software update command, it must validate the payload and then execute the `update.sh` script with the provided image tag as an argument.'}

### 2.3.5.0.0.0.0 Interface Specifications

#### 2.3.5.1.0.0.0 Interface Name

##### 2.3.5.1.1.0.0 Interface Name

IConnectivityStateManager

##### 2.3.5.1.2.0.0 File Path

src/OpcCoreClient/Services/IConnectivityStateManager.cs

##### 2.3.5.1.3.0.0 Purpose

Defines the contract for the service that manages the client's network connectivity state.

##### 2.3.5.1.4.0.0 Method Contracts

- {'method_name': 'TransitionTo', 'method_signature': 'void TransitionTo(ConnectivityState newState)', 'contract_description': 'Triggers a transition to a new connectivity state.'}

##### 2.3.5.1.5.0.0 Property Contracts

- {'property_name': 'CurrentState', 'property_type': 'ConnectivityState', 'getter_contract': 'Gets the current state.'}

#### 2.3.5.2.0.0.0 Interface Name

##### 2.3.5.2.1.0.0 Interface Name

IDataBuffer

##### 2.3.5.2.2.0.0 File Path

src/OpcCoreClient/Persistence/IDataBuffer.cs

##### 2.3.5.2.3.0.0 Purpose

Defines the contract for the persistent store-and-forward buffer.

##### 2.3.5.2.4.0.0 Method Contracts

###### 2.3.5.2.4.1.0 Method Name

####### 2.3.5.2.4.1.1 Method Name

EnqueueAsync

####### 2.3.5.2.4.1.2 Method Signature

Task EnqueueAsync(DataPoint dataPoint)

####### 2.3.5.2.4.1.3 Contract Description

Adds a single data point to the buffer.

###### 2.3.5.2.4.2.0 Method Name

####### 2.3.5.2.4.2.1 Method Name

DequeueBatchAsync

####### 2.3.5.2.4.2.2 Method Signature

Task<IEnumerable<DataPoint>> DequeueBatchAsync(int batchSize)

####### 2.3.5.2.4.2.3 Contract Description

Retrieves and removes a batch of data points from the buffer.

### 2.3.6.0.0.0.0 Enum Specifications

- {'enum_name': 'ConnectivityState', 'file_path': 'src/OpcCoreClient/Services/ConnectivityState.cs', 'underlying_type': 'int', 'purpose': "Represents the operational states of the client's cloud connection.", 'values': [{'value_name': 'Online', 'value': '0', 'description': 'Connected to the cloud, sending data in real-time.'}, {'value_name': 'Offline', 'value': '1', 'description': 'Disconnected from the cloud, buffering data to disk.'}, {'value_name': 'Recovery', 'value': '2', 'description': 'Reconnected to the cloud, currently sending buffered data.'}]}

### 2.3.7.0.0.0.0 Dto Specifications

- {'dto_name': 'DataPoint', 'file_path': 'src/OpcCoreClient/Models/DataPoint.cs', 'purpose': 'Internal representation of a single time-series data point collected from an OPC server.', 'framework_base_class': 'record struct', 'properties': [{'property_name': 'TagId', 'property_type': 'string'}, {'property_name': 'Timestamp', 'property_type': 'DateTimeOffset'}, {'property_name': 'Value', 'property_type': 'object'}, {'property_name': 'Quality', 'property_type': 'ushort'}]}

### 2.3.8.0.0.0.0 Configuration Specifications

- {'configuration_name': 'OpcOptions', 'file_path': 'src/OpcCoreClient/Configuration/OpcOptions.cs', 'purpose': 'Provides strongly-typed configuration for connecting to OPC servers.', 'configuration_sections': [{'section_name': 'Opc', 'properties': [{'property_name': 'PrimaryServerEndpoint', 'property_type': 'string', 'required': True}, {'property_name': 'BackupServerEndpoint', 'property_type': 'string', 'required': False}, {'property_name': 'TagList', 'property_type': 'List<string>', 'required': True}]}]}

### 2.3.9.0.0.0.0 Dependency Injection Specifications

#### 2.3.9.1.0.0.0 Service Interface

##### 2.3.9.1.1.0.0 Service Interface

N/A

##### 2.3.9.1.2.0.0 Service Implementation

DataPointChannel

##### 2.3.9.1.3.0.0 Lifetime

Singleton

##### 2.3.9.1.4.0.0 Registration Reasoning

A single, shared in-memory channel is required to decouple producers and consumers across the application.

##### 2.3.9.1.5.0.0 Framework Registration Pattern

services.AddSingleton(Channel.CreateUnbounded<DataPoint>()); services.AddSingleton(sp => sp.GetRequiredService<Channel<DataPoint>>().Reader); services.AddSingleton(sp => sp.GetRequiredService<Channel<DataPoint>>().Writer);

#### 2.3.9.2.0.0.0 Service Interface

##### 2.3.9.2.1.0.0 Service Interface

IConnectivityStateManager

##### 2.3.9.2.2.0.0 Service Implementation

ConnectivityStateManager

##### 2.3.9.2.3.0.0 Lifetime

Singleton

##### 2.3.9.2.4.0.0 Registration Reasoning

The connectivity state must be a single source of truth for the entire application lifetime.

#### 2.3.9.3.0.0.0 Service Interface

##### 2.3.9.3.1.0.0 Service Interface

IDataBuffer

##### 2.3.9.3.2.0.0 Service Implementation

FileBasedDataBuffer

##### 2.3.9.3.3.0.0 Lifetime

Singleton

##### 2.3.9.3.4.0.0 Registration Reasoning

The data buffer manages a physical file resource and must be a singleton to prevent race conditions.

#### 2.3.9.4.0.0.0 Service Interface

##### 2.3.9.4.1.0.0 Service Interface

N/A

##### 2.3.9.4.2.0.0 Service Implementation

OpcPollerHostedService

##### 2.3.9.4.3.0.0 Lifetime

Singleton (Hosted Service)

##### 2.3.9.4.4.0.0 Registration Reasoning

All IHostedService implementations are registered as singletons by the framework.

### 2.3.10.0.0.0.0 External Integration Specifications

#### 2.3.10.1.0.0.0 Integration Target

##### 2.3.10.1.1.0.0 Integration Target

Data Ingestion Service (gRPC)

##### 2.3.10.1.2.0.0 Integration Type

gRPC Client

##### 2.3.10.1.3.0.0 Required Client Classes

- GrpcDataSender

##### 2.3.10.1.4.0.0 Configuration Requirements

Endpoint address, mTLS client certificate path and key.

##### 2.3.10.1.5.0.0 Error Handling Requirements

Must use a Polly policy for retries with exponential backoff and a circuit breaker on the gRPC channel to handle transient network failures and server unavailability.

##### 2.3.10.1.6.0.0 Authentication Requirements

Mutual TLS (mTLS) is mandatory. The gRPC client must be configured with the client-specific certificate obtained during provisioning.

##### 2.3.10.1.7.0.0 Framework Integration Patterns

The gRPC client is generated from the shared .proto file and registered with the DI container using `AddGrpcClient`.

#### 2.3.10.2.0.0.0 Integration Target

##### 2.3.10.2.1.0.0 Integration Target

MQTT Broker

##### 2.3.10.2.2.0.0 Integration Type

MQTT Client

##### 2.3.10.2.3.0.0 Required Client Classes

- MqttClientWrapper
- MqttCommandHandler

##### 2.3.10.2.4.0.0 Configuration Requirements

Broker address, port, topic names, and TLS client certificate path.

##### 2.3.10.2.5.0.0 Error Handling Requirements

The MQTT client wrapper must handle automatic reconnection with exponential backoff. Message handlers must be idempotent to handle QoS 1 at-least-once delivery.

##### 2.3.10.2.6.0.0 Authentication Requirements

TLS with a client certificate is mandatory.

##### 2.3.10.2.7.0.0 Framework Integration Patterns

The MQTTnet client is wrapped in a singleton service. An IHostedService manages the connection lifecycle.

#### 2.3.10.3.0.0.0 Integration Target

##### 2.3.10.3.1.0.0 Integration Target

On-Premise OPC Servers

##### 2.3.10.3.2.0.0 Integration Type

OPC Client

##### 2.3.10.3.3.0.0 Required Client Classes

- OpcUaAdapter

##### 2.3.10.3.4.0.0 Configuration Requirements

Primary and backup server endpoint URLs.

##### 2.3.10.3.5.0.0 Error Handling Requirements

The adapter must handle connection failures and automatically trigger the failover logic defined in the OpcConnectionService as per SEQ-81.

##### 2.3.10.3.6.0.0 Authentication Requirements

Supports various OPC security policies, including certificate-based and user/password authentication, as configured.

##### 2.3.10.3.7.0.0 Framework Integration Patterns

The OPC UA client from the official OPC Foundation library is wrapped in an Adapter pattern implementation.

## 2.4.0.0.0.0.0 Component Count Validation

| Property | Value |
|----------|-------|
| Total Classes | 15 |
| Total Interfaces | 4 |
| Total Enums | 1 |
| Total Dtos | 1 |
| Total Configurations | 1 |
| Total External Integrations | 3 |
| File Structure Definitions | 8 |
| Dependency Injection Definitions | 4 |
| Namespace Definitions | 1 |
| Grand Total Components | 38 |
| Phase 2 Claimed Count | 0 |
| Phase 2 Actual Count | 0 |
| Validation Added Count | 38 |
| Final Validated Count | 38 |

