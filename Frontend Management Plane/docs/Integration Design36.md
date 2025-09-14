# 1 Integration Specifications

## 1.1 Extraction Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-LIB-SHARED |
| Extraction Timestamp | 2024-07-31T10:15:00Z |
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

- The shared library must define the Protocol Buffer messages for the gRPC stream.
- The shared library must define the DTOs for the MQTT command payloads.

#### 1.2.1.4 Implementation Implications

- A .proto file for gRPC communication must be created and maintained in this repository.
- C# record/class types for MQTT messages must be defined in this repository.

#### 1.2.1.5 Extraction Reasoning

This requirement directly mandates the creation of communication contracts (gRPC and MQTT) which are the primary responsibility of the Shared Kernel library to define and distribute.

### 1.2.2.0 Requirement Id

#### 1.2.2.1 Requirement Id

REQ-1-072

#### 1.2.2.2 Requirement Text

The system's software interfaces shall follow a defined strategy: communication between internal microservices must use gRPC for high performance and low latency. All services exposed to external clients or the frontend must use REST APIs documented with the OpenAPI specification.

#### 1.2.2.3 Validation Criteria

- The shared library provides the gRPC service and message definitions used for inter-service communication.
- The shared library provides the DTOs that define the shape of JSON payloads for the REST APIs.

#### 1.2.2.4 Implementation Implications

- This repository will contain the authoritative .proto files for all internal gRPC services.
- This repository will contain the C# record/class types for all DTOs used in public-facing REST APIs.

#### 1.2.2.5 Extraction Reasoning

This architectural requirement establishes the need for standardized gRPC and REST contracts. The Shared Kernel is the designated location for these definitions to ensure consistency across all microservices.

### 1.2.3.0 Requirement Id

#### 1.2.3.1 Requirement Id

REQ-1-029

#### 1.2.3.2 Requirement Text

All domain-specific business logic, such as the calculation of OEE and other KPIs, must be implemented directly from explicitly documented and approved formulas.

#### 1.2.3.3 Validation Criteria

- The system uses a consistent approach to domain modeling.

#### 1.2.3.4 Implementation Implications

- The Shared Kernel should provide common Domain-Driven Design (DDD) building blocks (e.g., base classes for Entity, Value Object, Aggregate Root) to ensure all services follow the same architectural patterns.

#### 1.2.3.5 Extraction Reasoning

While this requirement is about business logic implementation, the repository description for REPO-LIB-SHARED explicitly states it will provide shared DDD abstractions to enforce consistency, making this requirement relevant to its design.

## 1.3.0.0 Relevant Components

- {'component_name': 'Shared Kernel Library', 'component_specification': 'A foundational .NET 8 class library, distributed as a NuGet package, containing common code, contracts, and DTOs. It serves as the single source of truth for communication contracts (gRPC, REST, MQTT), shared enums, custom exceptions, and DDD abstractions.', 'implementation_requirements': ['Must be persistence-ignorant and have no dependencies on other repositories in the project.', 'Must be versioned using Semantic Versioning.', 'The CI/CD pipeline for this repository must automatically pack and publish a new NuGet package upon a successful merge to the main branch.'], 'architectural_context': "This is a cross-cutting component that does not belong to a single layer but provides foundational code for the 'Application Services Layer' and the 'Edge Application Layer'.", 'extraction_reasoning': 'This is the component being defined by the target repository (REPO-LIB-SHARED) itself. Its specification is derived directly from its repository definition and its role as a Shared Kernel in a microservices architecture.'}

## 1.4.0.0 Architectural Layers

- {'layer_name': 'Shared Kernel', 'layer_responsibilities': 'Centralizes cross-cutting data contracts, DTOs, enumerations, and custom exception types. It ensures that all communicating components (microservices, edge clients) share a common, unambiguous understanding of the data models being exchanged.', 'layer_constraints': ['Must have zero dependencies on other services or components within the system.', 'Must not contain business logic, I/O operations, or infrastructure-specific code.', 'Changes require strict versioning and coordinated deployment across all consuming services to avoid breaking integrations.'], 'implementation_patterns': ['Shared Kernel Pattern', 'NuGet Package Distribution'], 'extraction_reasoning': 'The repository is explicitly defined as a Shared Kernel library. Its existence is a direct implementation of this architectural pattern, which is essential for maintaining consistency and reducing coupling in a microservices architecture.'}

## 1.5.0.0 Dependency Interfaces

*No items available*

## 1.6.0.0 Exposed Interfaces

### 1.6.1.0 Interface Name

#### 1.6.1.1 Interface Name

Protocol Buffer Definitions (gRPC)

#### 1.6.1.2 Consumer Repositories

- REPO-SVC-DIN
- REPO-EDG-OPC
- REPO-SVC-IAM

#### 1.6.1.3 Method Contracts

##### 1.6.1.3.1 Method Name

###### 1.6.1.3.1.1 Method Name

DataIngestionService.StreamData

###### 1.6.1.3.1.2 Method Signature

rpc StreamData(stream DataPointRequest) returns (IngestAck)

###### 1.6.1.3.1.3 Method Purpose

Defines the contract for the high-throughput streaming of time-series data from the OPC Core Client to the Data Ingestion Service.

###### 1.6.1.3.1.4 Integration Context

Used for the primary data plane communication from edge to cloud.

##### 1.6.1.3.2.0 Method Name

###### 1.6.1.3.2.1 Method Name

AuthorizationService.CheckPermission

###### 1.6.1.3.2.2 Method Signature

rpc CheckPermission (PermissionCheckRequest) returns (PermissionCheckResponse)

###### 1.6.1.3.2.3 Method Purpose

Defines a potential high-performance contract for internal services to check user permissions with the IAM service.

###### 1.6.1.3.2.4 Integration Context

Used for low-latency, internal, service-to-service authorization checks.

#### 1.6.1.4.0.0 Integration Pattern

gRPC Service Contracts defined in .proto files.

#### 1.6.1.5.0.0 Communication Protocol

gRPC over HTTP/2

#### 1.6.1.6.0.0 Extraction Reasoning

This is a primary contract type defined by the library, enabling high-performance communication as required by the architecture (REQ-1-072) and consumed by multiple services and the edge client.

### 1.6.2.0.0.0 Interface Name

#### 1.6.2.1.0.0 Interface Name

REST Data Transfer Objects (DTOs)

#### 1.6.2.2.0.0 Consumer Repositories

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-IAM
- REPO-SVC-AST
- REPO-SVC-DVM

#### 1.6.2.3.0.0 Method Contracts

##### 1.6.2.3.1.0 Method Name

###### 1.6.2.3.1.1 Method Name

UserDto

###### 1.6.2.3.1.2 Method Signature

record UserDto(Guid Id, string Email, string FirstName, string LastName)

###### 1.6.2.3.1.3 Method Purpose

Defines the data structure for a user, used in API responses for user management.

###### 1.6.2.3.1.4 Integration Context

Consumed by REPO-FE-MPL (via REPO-GW-API) when displaying user lists; produced by REPO-SVC-IAM.

##### 1.6.2.3.2.0 Method Name

###### 1.6.2.3.2.1 Method Name

AssetDto

###### 1.6.2.3.2.2 Method Signature

record AssetDto(Guid Id, string Name, Guid? ParentId)

###### 1.6.2.3.2.3 Method Purpose

Defines the data structure for an asset, used in API responses for asset hierarchy management.

###### 1.6.2.3.2.4 Integration Context

Consumed by REPO-FE-MPL when displaying the asset tree; produced by REPO-SVC-AST.

#### 1.6.2.4.0.0 Integration Pattern

C# record types serialized to/from JSON.

#### 1.6.2.5.0.0 Communication Protocol

HTTPS/REST

#### 1.6.2.6.0.0 Extraction Reasoning

These DTOs form the public contract for all REST APIs exposed by the microservices, ensuring consistency for the frontend application and any other external API consumers as per REPO-FE-MPL's dependency on REPO-GW-API.

### 1.6.3.0.0.0 Interface Name

#### 1.6.3.1.0.0 Interface Name

MQTT Message Contracts

#### 1.6.3.2.0.0 Consumer Repositories

- REPO-SVC-DVM
- REPO-EDG-OPC

#### 1.6.3.3.0.0 Method Contracts

##### 1.6.3.3.1.0 Method Name

###### 1.6.3.3.1.1 Method Name

ConfigurationUpdateRequest

###### 1.6.3.3.1.2 Method Signature

record ConfigurationUpdateRequest(string ConfigurationJson, string Version)

###### 1.6.3.3.1.3 Method Purpose

Defines the payload for a command sent from the Device Management Service to an OPC Core Client to update its configuration.

###### 1.6.3.3.1.4 Integration Context

Published by REPO-SVC-DVM and consumed by REPO-EDG-OPC over MQTT.

##### 1.6.3.3.2.0 Method Name

###### 1.6.3.3.2.1 Method Name

SoftwareUpdateRequest

###### 1.6.3.3.2.2 Method Signature

record SoftwareUpdateRequest(string ImageUrl, string Checksum)

###### 1.6.3.3.2.3 Method Purpose

Defines the payload for a command sent to an OPC Core Client to trigger a software update.

###### 1.6.3.3.2.4 Integration Context

Published by REPO-SVC-DVM and consumed by REPO-EDG-OPC over MQTT.

#### 1.6.3.4.0.0 Integration Pattern

C# record types serialized to/from JSON for MQTT message payloads.

#### 1.6.3.5.0.0 Communication Protocol

MQTT v5

#### 1.6.3.6.0.0 Extraction Reasoning

Defines the command and control message contracts used over MQTT, as specified by REQ-1-010. This ensures the cloud services and edge clients have a common, versioned understanding of command structures.

### 1.6.4.0.0.0 Interface Name

#### 1.6.4.1.0.0 Interface Name

Domain-Driven Design Abstractions

#### 1.6.4.2.0.0 Consumer Repositories

- REPO-SVC-IAM
- REPO-SVC-AST
- REPO-SVC-DVM

#### 1.6.4.3.0.0 Method Contracts

##### 1.6.4.3.1.0 Method Name

###### 1.6.4.3.1.1 Method Name

IEntity

###### 1.6.4.3.1.2 Method Signature

interface IEntity<TId>

###### 1.6.4.3.1.3 Method Purpose

Provides a base interface for all domain entities, establishing the concept of a unique identifier.

###### 1.6.4.3.1.4 Integration Context

Used as a base for domain entities in all backend microservices.

##### 1.6.4.3.2.0 Method Name

###### 1.6.4.3.2.1 Method Name

ValueObject

###### 1.6.4.3.2.2 Method Signature

abstract record ValueObject

###### 1.6.4.3.2.3 Method Purpose

Provides a base class for implementing the Value Object pattern, ensuring immutability and structural equality.

###### 1.6.4.3.2.4 Integration Context

Used for creating domain-specific value types in backend microservices.

#### 1.6.4.4.0.0 Integration Pattern

NuGet Package Reference providing shared base classes and interfaces.

#### 1.6.4.5.0.0 Communication Protocol

In-Process

#### 1.6.4.6.0.0 Extraction Reasoning

This library provides the foundational building blocks for the DDD architectural style used by all microservices, enforcing consistency and reducing boilerplate code as stated in its repository description.

## 1.7.0.0.0.0 Technology Context

### 1.7.1.0.0.0 Framework Requirements

.NET v8.0. The repository must be a standard .NET class library project (`<Project Sdk="Microsoft.NET.Sdk">`).

### 1.7.2.0.0.0 Integration Technologies

- Google.Protobuf v3.27.1
- Grpc.Tools v2.64.0

### 1.7.3.0.0.0 Performance Constraints

DTOs and gRPC messages should be designed to be lean and efficient to serialize, avoiding complex object graphs where possible to support high-performance communication.

### 1.7.4.0.0.0 Security Requirements

The library must not contain any secrets, business logic, or infrastructure code. Its scope is strictly limited to shared contracts and abstractions.

## 1.8.0.0.0.0 Extraction Validation

| Property | Value |
|----------|-------|
| Mapping Completeness Check | The library is correctly identified as a foundatio... |
| Cross Reference Validation | The exposed interfaces directly align with the com... |
| Implementation Readiness Assessment | High. The context provides a clear boundary of res... |
| Quality Assurance Confirmation | The analysis confirms a high degree of consistency... |

