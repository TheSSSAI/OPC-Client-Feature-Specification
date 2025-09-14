# 1 Integration Specifications

## 1.1 Extraction Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-LIB-SHARED |
| Extraction Timestamp | 2024-07-29T10:00:00Z |
| Mapping Validation Score | 100% |
| Context Completeness Score | 95% |
| Implementation Readiness Level | High |

## 1.2 Relevant Requirements

### 1.2.1 Requirement Id

#### 1.2.1.1 Requirement Id

REQ-1-010

#### 1.2.1.2 Requirement Text

Communication between the OPC Core Client and the Central Management Plane shall be secured and optimized using a dual-protocol strategy. High-volume, low-latency data streaming shall use gRPC with mutual TLS (mTLS) authentication. Command, control, and status messaging shall use MQTT v5 over a standard TLS connection to ensure robust delivery.

#### 1.2.1.3 Validation Criteria

- The Shared Kernel must define the Protocol Buffer messages for the gRPC data stream.
- The Shared Kernel must define the DTOs for MQTT command payloads.

#### 1.2.1.4 Implementation Implications

- A .proto file for the gRPC data stream must be created in this repository.
- C# record types for MQTT command and status messages must be defined in this repository.

#### 1.2.1.5 Extraction Reasoning

This requirement mandates specific communication protocols (gRPC, MQTT) whose data contracts must be defined in a shared, centralized location to ensure interoperability between the edge client (REPO-EDG-OPC) and backend services (REPO-SVC-DIN, REPO-SVC-DVM). This repository is that location.

### 1.2.2.0 Requirement Id

#### 1.2.2.1 Requirement Id

REQ-1-072

#### 1.2.2.2 Requirement Text

The system's software interfaces shall follow a defined strategy: communication between internal microservices must use gRPC for high performance and low latency. All services exposed to external clients or the frontend must use REST APIs documented with the OpenAPI specification.

#### 1.2.2.3 Validation Criteria

- The Shared Kernel provides gRPC service and message definitions in .proto files.
- The Shared Kernel provides C# record types to be used as DTOs for REST APIs.

#### 1.2.2.4 Implementation Implications

- This repository is the source of truth for all gRPC contracts used for inter-service communication.
- This repository defines the data structures for request/response bodies in the system's REST APIs, ensuring consistency.

#### 1.2.2.5 Extraction Reasoning

This architectural requirement explicitly calls for gRPC and REST communication, whose contracts are the primary responsibility of the Shared Kernel to define and distribute.

### 1.2.3.0 Requirement Id

#### 1.2.3.1 Requirement Id

REQ-1-029

#### 1.2.3.2 Requirement Text

All domain-specific business logic, such as the calculation of OEE and other KPIs, must be implemented directly from explicitly documented and approved formulas. The documentation for these formulas and the corresponding source code must be maintained in a version control system.

#### 1.2.3.3 Validation Criteria

- The Shared Kernel provides foundational abstractions for implementing Domain-Driven Design (DDD).

#### 1.2.3.4 Implementation Implications

- This repository must contain base classes and interfaces for DDD concepts like Entity, Aggregate Root, and Value Object to ensure a consistent implementation of domain logic across all services.

#### 1.2.3.5 Extraction Reasoning

While this requirement focuses on business logic implementation within services, it implies a consistent architectural approach (DDD). This library is the designated location to provide the reusable, foundational building blocks for that approach.

## 1.3.0.0 Relevant Components

- {'component_name': 'Shared Contracts and Domain Abstractions', 'component_specification': 'Provides the single source of truth for data contracts used in communication between all .NET-based components and a common set of Domain-Driven Design abstractions to enforce architectural consistency.', 'implementation_requirements': ['All data contracts must be versioned to manage changes across distributed consumers.', 'Definitions must remain free of business logic and infrastructure dependencies.', 'The project must be packaged as a versioned NuGet package and distributed via a private feed.', 'Use Grpc.Tools to auto-generate C# code from .proto files during the build process.'], 'architectural_context': "This is not a runnable component but a foundational library (Shared Kernel) that supports components across the 'Application Services Layer' and the 'Edge Application Layer'.", 'extraction_reasoning': "This repository's primary purpose is to define and provide these shared assets. Its output artifacts are critical dependencies for nearly all other service and edge components, making it a central architectural element."}

## 1.4.0.0 Architectural Layers

- {'layer_name': 'Shared Kernel', 'layer_responsibilities': 'Centralizes cross-cutting data contracts, DTOs, enumerations, and custom exception types. It ensures that all communicating components (microservices, edge clients) share a common, unambiguous understanding of the data models being exchanged.', 'layer_constraints': ['Must have zero dependencies on other services or components within the system.', 'Must not contain business logic, I/O operations, or infrastructure-specific code.', 'Changes require strict versioning (Semantic Versioning) and coordinated deployment across all consuming services to avoid breaking integrations.'], 'implementation_patterns': ['Shared Kernel Pattern', 'NuGet Package Distribution'], 'extraction_reasoning': 'The repository is explicitly defined as a Shared Kernel library. Its existence is a direct implementation of this architectural pattern, which is essential for maintaining consistency and reducing coupling in a microservices architecture.'}

## 1.5.0.0 Dependency Interfaces

*No items available*

## 1.6.0.0 Exposed Interfaces

### 1.6.1.0 Interface Name

#### 1.6.1.1 Interface Name

Protocol Buffer Definitions (gRPC)

#### 1.6.1.2 Consumer Repositories

- REPO-SVC-DIN
- REPO-EDG-OPC

#### 1.6.1.3 Method Contracts

- {'method_name': 'IIngestionService.StreamData', 'method_signature': 'rpc StreamData (stream DataPointRequest) returns (IngestAck);', 'method_purpose': 'Defines the contract for the high-throughput streaming of time-series data from the OPC Core Client to the Data Ingestion Service.', 'implementation_requirements': 'The contract is defined in `ingestion.proto`. The C# server and client code is generated by Grpc.Tools during the build process of consuming projects.'}

#### 1.6.1.4 Service Level Requirements

- Must use Protobuf v3 syntax for efficient binary serialization.
- Definitions must be backward-compatible where possible to ease deployment.

#### 1.6.1.5 Implementation Constraints

- Any service or client that needs to communicate via gRPC must consume this shared library.

#### 1.6.1.6 Extraction Reasoning

This contract is explicitly defined as necessary by REQ-1-010 and REQ-1-072. It enables the high-performance data plane between the edge client and the data ingestion service.

### 1.6.2.0 Interface Name

#### 1.6.2.1 Interface Name

Messaging Contracts (MQTT)

#### 1.6.2.2 Consumer Repositories

- REPO-SVC-DVM
- REPO-EDG-OPC

#### 1.6.2.3 Method Contracts

##### 1.6.2.3.1 Method Name

###### 1.6.2.3.1.1 Method Name

SoftwareUpdateCommand

###### 1.6.2.3.1.2 Method Signature

C# record SoftwareUpdateCommand(string ImageUrl, string Checksum)

###### 1.6.2.3.1.3 Method Purpose

Defines the payload for an MQTT message instructing a client to perform a software update.

###### 1.6.2.3.1.4 Implementation Requirements

Defined as an immutable C# record for clear, predictable serialization to JSON for the MQTT payload.

##### 1.6.2.3.2.0 Method Name

###### 1.6.2.3.2.1 Method Name

ConfigurationUpdateCommand

###### 1.6.2.3.2.2 Method Signature

C# record ConfigurationUpdateCommand(string ConfigurationJson)

###### 1.6.2.3.2.3 Method Purpose

Defines the payload for an MQTT message pushing a new configuration to a client.

###### 1.6.2.3.2.4 Implementation Requirements

Defined as an immutable C# record for clear, predictable serialization to JSON for the MQTT payload.

#### 1.6.2.4.0.0 Service Level Requirements

- Payloads should be lightweight and easily serializable to JSON.

#### 1.6.2.5.0.0 Implementation Constraints

- Both the Device Management Service and the OPC Core Client must use these exact DTOs to ensure the command and control channel is reliable.

#### 1.6.2.6.0.0 Extraction Reasoning

This contract is required by REQ-1-010 to define the command and control communication channel. It ensures both the publisher (REPO-SVC-DVM) and subscriber (REPO-EDG-OPC) understand the message structure.

### 1.6.3.0.0.0 Interface Name

#### 1.6.3.1.0.0 Interface Name

Data Transfer Objects (REST API)

#### 1.6.3.2.0.0 Consumer Repositories

- REPO-SVC-IAM
- REPO-SVC-AST
- REPO-SVC-DVM
- REPO-FE-MPL

#### 1.6.3.3.0.0 Method Contracts

##### 1.6.3.3.1.0 Method Name

###### 1.6.3.3.1.1 Method Name

UserDto

###### 1.6.3.3.1.2 Method Signature

C# record UserDto(Guid Id, string Email, string FirstName, string LastName)

###### 1.6.3.3.1.3 Method Purpose

Defines the public data contract for a user entity, used in REST APIs for user management.

###### 1.6.3.3.1.4 Implementation Requirements

Defined as an immutable C# record for use in ASP.NET Core Minimal APIs, serialized to/from JSON.

##### 1.6.3.3.2.0 Method Name

###### 1.6.3.3.2.1 Method Name

AssetDto

###### 1.6.3.3.2.2 Method Signature

C# record AssetDto(Guid Id, string Name, Guid? ParentId)

###### 1.6.3.3.2.3 Method Purpose

Defines the public data contract for an asset entity, used in REST APIs for asset hierarchy management.

###### 1.6.3.3.2.4 Implementation Requirements

Defined as an immutable C# record for use in ASP.NET Core Minimal APIs, serialized to/from JSON.

#### 1.6.3.4.0.0 Service Level Requirements

- DTOs should be Plain Old CLR Objects (POCOs) or records to ensure easy serialization to JSON.
- Property names should follow a consistent convention (e.g., camelCase in JSON).

#### 1.6.3.5.0.0 Implementation Constraints

- These DTOs define the public REST API contracts for many services. Changes are breaking and must be managed via API versioning (REQ-1-028).
- DTOs must not contain methods with business logic.

#### 1.6.3.6.0.0 Extraction Reasoning

This contract is required by REQ-1-072, which mandates the use of REST APIs for external communication. This library provides the single source of truth for the data structures used by those APIs, ensuring consistency between the frontend and multiple backend services.

## 1.7.0.0.0.0 Technology Context

### 1.7.1.0.0.0 Framework Requirements

.NET v8.0, C# 12. Must be built as a .NET class library, suitable for packaging as a NuGet artifact.

### 1.7.2.0.0.0 Integration Technologies

- Google.Protobuf v3.27.1: For defining and serializing gRPC messages.
- Grpc.Tools v2.64.0: For build-time code generation of gRPC clients and servers from .proto files.

### 1.7.3.0.0.0 Performance Constraints

Not applicable at the library level, but the choice of Protobuf and immutable record types enables high-performance communication in consuming services.

### 1.7.4.0.0.0 Security Requirements

This library must not contain any secrets, connection strings, or sensitive configuration data. It should only contain data structure definitions and pure utility functions.

## 1.8.0.0.0.0 Extraction Validation

| Property | Value |
|----------|-------|
| Mapping Completeness Check | The repository's role as a Shared Kernel is fully ... |
| Cross Reference Validation | Exposed gRPC, MQTT, and REST DTO contracts were su... |
| Implementation Readiness Assessment | The repository is highly implementation-ready. A d... |
| Quality Assurance Confirmation | A systematic review confirms that the repository's... |

