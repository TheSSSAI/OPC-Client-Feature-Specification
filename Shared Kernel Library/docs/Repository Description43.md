# 1 Id

REPO-LIB-SHARED

# 2 Name

Shared Kernel Library

# 3 Description

This repository is a foundational shared library containing common code, contracts, and data transfer objects (DTOs) used across multiple backend microservices and the edge client. Its purpose is to enforce consistency, reduce code duplication, and provide a single source of truth for cross-service communication contracts. It includes definitions for gRPC Protocol Buffers, DTOs for REST APIs, shared enums (e.g., data quality flags, user roles), custom exception types, and common utility functions. By centralizing these shared assets, it ensures that different components of the distributed system can communicate reliably and evolve in a coordinated manner. It is a critical component for maintaining the integrity of the microservices architecture.

# 4 Type

🔹 SharedKernel

# 5 Namespace

System.Shared

# 6 Output Path

libs/shared-kernel

# 7 Framework

.NET v8.0

# 8 Language

C# 12

# 9 Technology

.NET v8.0

# 10 Thirdparty Libraries

- Google.Protobuf v3.27.1
- Grpc.Tools v2.64.0

# 11 Layer Ids

- shared

# 12 Dependencies

*No items available*

# 13 Requirements

*No items available*

# 14 Generate Tests

✅ Yes

# 15 Generate Documentation

✅ Yes

# 16 Architecture Style

Microservices

# 17 Architecture Map

*No items available*

# 18 Components Map

*No items available*

# 19 Requirements Map

*No items available*

# 20 Dependency Contracts

*No data available*

# 21 Exposed Contracts

## 21.1 Public Interfaces

### 21.1.1 Interface

#### 21.1.1.1 Interface

Protocol Buffer Definitions (*.proto)

#### 21.1.1.2 Methods

- Defines gRPC services like IIngestionService and IAuditService.

#### 21.1.1.3 Events

*No items available*

#### 21.1.1.4 Properties

- Message types like DataPoint, AuditLogEntry.

#### 21.1.1.5 Consumers

- REPO-SVC-DIN
- REPO-SVC-ADT
- REPO-EDG-OPC

### 21.1.2.0 Interface

#### 21.1.2.1 Interface

Data Transfer Objects (DTOs)

#### 21.1.2.2 Methods

*No items available*

#### 21.1.2.3 Events

*No items available*

#### 21.1.2.4 Properties

- UserDto
- AssetDto
- OpcClientStatusDto

#### 21.1.2.5 Consumers

- REPO-SVC-IAM
- REPO-SVC-AST
- REPO-SVC-DVM

# 22.0.0.0 Integration Patterns

| Property | Value |
|----------|-------|
| Dependency Injection | N/A |
| Event Communication | N/A |
| Data Flow | Consumed as a NuGet package by other projects. |
| Error Handling | Defines common exception types for the domain. |
| Async Patterns | N/A |

# 23.0.0.0 Scope Boundaries

## 23.1.0.0 Must Implement

- Shared communication contracts (.proto files).
- Shared DTOs for REST APIs.
- Common enumerations and constants.
- Cross-cutting utility classes (e.g., custom converters, validators).

## 23.2.0.0 Must Not Implement

- Any business logic.
- Any I/O operations or infrastructure dependencies (e.g., database access, HTTP clients).
- Any framework-specific code (should target .NET Standard).

## 23.3.0.0 Integration Points

- Packaged and distributed via a private NuGet feed.

## 23.4.0.0 Architectural Constraints

- Must have zero external dependencies on other services within the system.
- Changes to this library require careful versioning and coordinated deployment.

# 24.0.0.0 Technology Standards

## 24.1.0.0 Framework Specific

| Property | Value |
|----------|-------|
| Pattern Usage | Use Protocol Buffers v3 syntax. DTOs should be pla... |
| Performance Requirements | N/A |
| Security Requirements | Should not contain any secrets or sensitive config... |

# 25.0.0.0 Cognitive Load Instructions

## 25.1.0.0 Sds Generation Guidance

### 25.1.1.0 Focus Areas

- Defining stable and versioned contracts for gRPC and REST.
- Identifying truly common code that belongs in the kernel.
- Establishing a clear versioning strategy for the package.

### 25.1.2.0 Avoid Patterns

- Adding service-specific logic to the shared kernel.
- Creating dependencies on concrete implementations.

## 25.2.0.0 Code Generation Guidance

### 25.2.1.0 Implementation Patterns

- The gRPC build tools should be used to automatically generate C# code from `.proto` files.

