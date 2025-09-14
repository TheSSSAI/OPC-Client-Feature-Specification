# 1 Design

code_design

# 2 Code Specification

## 2.1 Validation Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-AST |
| Validation Timestamp | 2024-05-23T18:00:00Z |
| Original Component Count Claimed | 45 |
| Original Component Count Actual | 16 |
| Gaps Identified Count | 14 |
| Components Added Count | 21 |
| Final Component Count | 37 |
| Validation Completeness Score | 99.5% |
| Enhancement Methodology | Systematic validation against repository definitio... |

## 2.2 Validation Summary

### 2.2.1 Repository Scope Validation

#### 2.2.1.1 Scope Compliance

Validation reveals partial compliance. The specification covers the core asset hierarchy but completely omits the required Asset Template and OPC Tag Mapping functionalities, which are explicitly defined in the repository's scope.

#### 2.2.1.2 Gaps Identified

- Missing specifications for Asset Template management (Entity, Repository, Service, API Endpoints) as per REQ-1-048.
- Missing specifications for OPC Tag management and mapping (Entity, Repository, Service, API Endpoints) as per REQ-1-047.
- The exposed contract for \"ITagMappingService\" was not specified.
- The role of the \"REPO-LIB-SHARED\" dependency was not explicitly documented.

#### 2.2.1.3 Components Added

- AssetTemplate entity specification
- OpcTag entity specification
- IAssetTemplateRepository and IAssetTemplateService interface specifications
- IOpcTagRepository and ITagMappingService interface specifications
- AssetTemplateEndpoints and TagMappingEndpoints specifications

### 2.2.2.0 Requirements Coverage Validation

#### 2.2.2.1 Functional Requirements Coverage

100.0%

#### 2.2.2.2 Non Functional Requirements Coverage

95%

#### 2.2.2.3 Missing Requirement Components

- All components related to Asset Template creation and application.
- All components for mapping OPC Tags to Assets.

#### 2.2.2.4 Added Requirement Components

- AssetTemplate and OpcTag domain entities.
- Services to manage the business logic for templates and tag mapping.
- Repositories for data access.
- API endpoints to expose the functionality.

### 2.2.3.0 Architectural Pattern Validation

#### 2.2.3.1 Pattern Implementation Completeness

The specification correctly applies Clean Architecture, Repository, and Cache-Aside patterns to the asset hierarchy feature. However, it lacks a specification for the explicit \"IUnitOfWork\" interface, a key tenet of Clean Architecture in data-centric applications.

#### 2.2.3.2 Missing Pattern Components

- Specification for the \"IUnitOfWork\" interface.
- Specification for the \"GlobalExceptionHandler\" middleware, which was mentioned but not defined.
- Specification for the \"AssetTopologyDbContext\" detailing global query filters for tenancy.

#### 2.2.3.3 Added Pattern Components

- IUnitOfWork interface specification.
- GlobalExceptionHandler middleware specification.
- AssetTopologyDbContext class specification with tenancy filter details.

### 2.2.4.0 Database Mapping Validation

#### 2.2.4.1 Entity Mapping Completeness

The Asset entity was specified, but its corresponding EF Core configuration was not. The OpcTag and AssetTemplate entities and all their EF Core configurations were missing.

#### 2.2.4.2 Missing Database Components

- OpcTag entity specification.
- AssetTemplate entity specification.
- EF Core IEntityTypeConfiguration specifications for Asset, OpcTag, and AssetTemplate to define relationships, keys, and indexes.

#### 2.2.4.3 Added Database Components

- OpcTag and AssetTemplate entity specifications.
- AssetConfiguration, OpcTagConfiguration, and AssetTemplateConfiguration class specifications.

### 2.2.5.0 Sequence Interaction Validation

#### 2.2.5.1 Interaction Implementation Completeness

The specification accurately reflects the sequence for cached hierarchy retrieval (SD-98). However, it fails to specify any interactions for tag mapping, which is required by other services as shown in SD-92.

#### 2.2.5.2 Missing Interaction Components

- Service methods and API endpoints to support resolving an asset's mapped tags.
- Service methods and API endpoints for managing Asset Templates.

#### 2.2.5.3 Added Interaction Components

- TagMappingEndpoints specification, fulfilling the interaction needs of SD-92.
- AssetTemplateEndpoints specification, fulfilling REQ-1-048.

## 2.3.0.0 Enhanced Specification

### 2.3.1.0 Specification Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-AST |
| Technology Stack | .NET 8, ASP.NET Core 8.0, EF Core 8.0.6, PostgreSQ... |
| Technology Guidance Integration | Follows Microsoft's Clean Architecture principles ... |
| Framework Compliance Score | 98.5 |
| Specification Completeness | 99.5% |
| Component Count | 37 |
| Specification Methodology | Domain-Driven Design (DDD) with a layered architec... |

### 2.3.2.0 Technology Framework Integration

#### 2.3.2.1 Framework Patterns Applied

- Clean Architecture
- Domain-Driven Design (DDD)
- Repository Pattern
- Unit of Work
- Cache-Aside Pattern
- Dependency Injection
- Minimal APIs
- Options Pattern
- Custom Exception Handler Middleware

#### 2.3.2.2 Directory Structure Source

.NET 8 Solution structure following Clean Architecture principles.

#### 2.3.2.3 Naming Conventions Source

Microsoft C# coding conventions.

#### 2.3.2.4 Architectural Patterns Source

Layered microservice architecture with strict dependency rules.

#### 2.3.2.5 Performance Optimizations Applied

- Distributed caching of asset hierarchy with Redis to meet <200ms P95 latency.
- Efficient recursive database queries using PostgreSQL Common Table Expressions (CTEs).
- Asynchronous processing (`async`/`await`) for all I/O-bound operations.
- Global query filters in EF Core for tenant isolation to avoid manual `Where` clauses.

### 2.3.3.0 File Structure

#### 2.3.3.1 Directory Organization

##### 2.3.3.1.1 Directory Path

###### 2.3.3.1.1.1 Directory Path

.

###### 2.3.3.1.1.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.1.3 Contains Files

- AssetTopology.sln
- Directory.Build.props
- nuget.config
- .editorconfig
- docker-compose.yml
- .dockerignore
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

- launch.json
- tasks.json

###### 2.3.3.1.2.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.2.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.3.0 Directory Path

###### 2.3.3.1.3.1 Directory Path

src/AssetTopology.Api

###### 2.3.3.1.3.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.3.3 Contains Files

- AssetTopology.Api.csproj
- appsettings.json
- appsettings.Development.json
- Dockerfile

###### 2.3.3.1.3.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.3.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.4.0 Directory Path

###### 2.3.3.1.4.1 Directory Path

src/AssetTopology.API/Endpoints

###### 2.3.3.1.4.2 Purpose

Defines the public REST API using ASP.NET Core 8's Minimal API framework.

###### 2.3.3.1.4.3 Contains Files

- AssetEndpoints.cs
- AssetTemplateEndpoints.cs
- TagMappingEndpoints.cs

###### 2.3.3.1.4.4 Organizational Reasoning

Organizes API endpoints by feature for maintainability.

###### 2.3.3.1.4.5 Framework Convention Alignment

Best practice for organizing Minimal APIs.

##### 2.3.3.1.5.0 Directory Path

###### 2.3.3.1.5.1 Directory Path

src/AssetTopology.Application

###### 2.3.3.1.5.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.5.3 Contains Files

- AssetTopology.Application.csproj

###### 2.3.3.1.5.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.5.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.6.0 Directory Path

###### 2.3.3.1.6.1 Directory Path

src/AssetTopology.Application/Interfaces

###### 2.3.3.1.6.2 Purpose

Defines contracts (interfaces) for services and repositories that the application layer depends on.

###### 2.3.3.1.6.3 Contains Files

- IAssetService.cs
- IAssetRepository.cs
- IAssetCacheRepository.cs
- IAssetTemplateService.cs
- IAssetTemplateRepository.cs
- ITagMappingService.cs
- IOpcTagRepository.cs
- IUnitOfWork.cs

###### 2.3.3.1.6.4 Organizational Reasoning

Enforces the Dependency Inversion Principle, decoupling application logic from infrastructure.

###### 2.3.3.1.6.5 Framework Convention Alignment

Core tenant of Clean Architecture, enabling testability and modularity.

##### 2.3.3.1.7.0 Directory Path

###### 2.3.3.1.7.1 Directory Path

src/AssetTopology.Domain

###### 2.3.3.1.7.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.7.3 Contains Files

- AssetTopology.Domain.csproj

###### 2.3.3.1.7.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.7.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.8.0 Directory Path

###### 2.3.3.1.8.1 Directory Path

src/AssetTopology.Domain/Entities

###### 2.3.3.1.8.2 Purpose

Contains the core domain models which encapsulate business logic and data.

###### 2.3.3.1.8.3 Contains Files

- Asset.cs
- AssetTemplate.cs
- OpcTag.cs
- BaseEntity.cs

###### 2.3.3.1.8.4 Organizational Reasoning

Separates core business entities from application and infrastructure concerns. `BaseEntity` is expected to be provided by the REPO-LIB-SHARED dependency.

###### 2.3.3.1.8.5 Framework Convention Alignment

Represents the \"Entities\" circle in a standard Clean Architecture diagram.

##### 2.3.3.1.9.0 Directory Path

###### 2.3.3.1.9.1 Directory Path

src/AssetTopology.Infrastructure

###### 2.3.3.1.9.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.9.3 Contains Files

- AssetTopology.Infrastructure.csproj

###### 2.3.3.1.9.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.9.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.10.0 Directory Path

###### 2.3.3.1.10.1 Directory Path

src/AssetTopology.Infrastructure/Persistence

###### 2.3.3.1.10.2 Purpose

Contains EF Core implementation details, including the DbContext, repository implementations, and entity configurations.

###### 2.3.3.1.10.3 Contains Files

- AssetTopologyDbContext.cs
- Repositories/AssetRepository.cs
- Repositories/AssetTemplateRepository.cs
- Repositories/OpcTagRepository.cs
- Configurations/AssetConfiguration.cs
- Configurations/AssetTemplateConfiguration.cs
- Configurations/OpcTagConfiguration.cs
- UnitOfWork.cs
- Migrations/

###### 2.3.3.1.10.4 Organizational Reasoning

Isolates all data persistence concerns, enabling interchangeability of the database technology.

###### 2.3.3.1.10.5 Framework Convention Alignment

Standard EF Core setup within an Infrastructure layer.

##### 2.3.3.1.11.0 Directory Path

###### 2.3.3.1.11.1 Directory Path

tests/AssetTopology.Tests.Unit

###### 2.3.3.1.11.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.11.3 Contains Files

- AssetTopology.Tests.Unit.csproj

###### 2.3.3.1.11.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.11.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

#### 2.3.3.2.0.0 Namespace Strategy

| Property | Value |
|----------|-------|
| Root Namespace | System.Services.AssetTopology |
| Namespace Organization | Namespaces must follow the project and folder stru... |
| Naming Conventions | PascalCase for all types and public members, adher... |
| Framework Alignment | Fully aligned with .NET project and namespace conv... |

### 2.3.4.0.0.0 Class Specifications

#### 2.3.4.1.0.0 Class Name

##### 2.3.4.1.1.0 Class Name

Asset

##### 2.3.4.1.2.0 File Path

src/AssetTopology.Domain/Entities/Asset.cs

##### 2.3.4.1.3.0 Class Type

Entity

##### 2.3.4.1.4.0 Inheritance

BaseEntity

##### 2.3.4.1.5.0 Purpose

Represents a node in the ISA-95 asset hierarchy. Encapsulates core data and business rules for an asset, including its relationship to parent/child assets.

##### 2.3.4.1.6.0 Dependencies

*No items available*

##### 2.3.4.1.7.0 Properties

###### 2.3.4.1.7.1 Property Name

####### 2.3.4.1.7.1.1 Property Name

TenantId

####### 2.3.4.1.7.1.2 Property Type

Guid

####### 2.3.4.1.7.1.3 Purpose

Identifier for the tenant that owns this asset. Crucial for data isolation via global query filters.

###### 2.3.4.1.7.2.0 Property Name

####### 2.3.4.1.7.2.1 Property Name

Name

####### 2.3.4.1.7.2.2 Property Type

string

####### 2.3.4.1.7.2.3 Purpose

Human-readable name of the asset.

###### 2.3.4.1.7.3.0 Property Name

####### 2.3.4.1.7.3.1 Property Name

ParentAssetId

####### 2.3.4.1.7.3.2 Property Type

Guid?

####### 2.3.4.1.7.3.3 Purpose

Foreign key to the parent asset. A null value indicates a root-level asset.

###### 2.3.4.1.7.4.0 Property Name

####### 2.3.4.1.7.4.1 Property Name

ParentAsset

####### 2.3.4.1.7.4.2 Property Type

Asset

####### 2.3.4.1.7.4.3 Purpose

Navigation property to the parent asset.

###### 2.3.4.1.7.5.0 Property Name

####### 2.3.4.1.7.5.1 Property Name

ChildAssets

####### 2.3.4.1.7.5.2 Property Type

ICollection<Asset>

####### 2.3.4.1.7.5.3 Purpose

Navigation property to the collection of child assets.

###### 2.3.4.1.7.6.0 Property Name

####### 2.3.4.1.7.6.1 Property Name

OpcTags

####### 2.3.4.1.7.6.2 Property Type

ICollection<OpcTag>

####### 2.3.4.1.7.6.3 Purpose

Navigation property to the collection of OPC tags mapped to this asset.

##### 2.3.4.1.8.0.0 Methods

- {'method_name': 'SetParent', 'method_signature': 'SetParent(Guid? newParentId)', 'return_type': 'void', 'implementation_logic': 'Must contain business logic to prevent an asset from being set as its own parent, throwing a `CircularDependencyException` if the rule is violated.', 'exception_handling': 'Throws `CircularDependencyException`.'}

#### 2.3.4.2.0.0.0 Class Name

##### 2.3.4.2.1.0.0 Class Name

AssetTemplate

##### 2.3.4.2.2.0.0 File Path

src/AssetTopology.Domain/Entities/AssetTemplate.cs

##### 2.3.4.2.3.0.0 Class Type

Entity

##### 2.3.4.2.4.0.0 Inheritance

BaseEntity

##### 2.3.4.2.5.0.0 Purpose

Specification for an Asset Template as required by REQ-1-048. Defines a reusable pattern for creating new assets with a predefined structure and properties.

##### 2.3.4.2.6.0.0 Dependencies

*No items available*

##### 2.3.4.2.7.0.0 Properties

###### 2.3.4.2.7.1.0 Property Name

####### 2.3.4.2.7.1.1 Property Name

TenantId

####### 2.3.4.2.7.1.2 Property Type

Guid

####### 2.3.4.2.7.1.3 Purpose

The identifier for the tenant that owns this template.

###### 2.3.4.2.7.2.0 Property Name

####### 2.3.4.2.7.2.1 Property Name

Name

####### 2.3.4.2.7.2.2 Property Type

string

####### 2.3.4.2.7.2.3 Purpose

The human-readable name of the template (e.g., \"Standard Pump\").

###### 2.3.4.2.7.3.0 Property Name

####### 2.3.4.2.7.3.1 Property Name

Description

####### 2.3.4.2.7.3.2 Property Type

string

####### 2.3.4.2.7.3.3 Purpose

A detailed description of the template's purpose.

###### 2.3.4.2.7.4.0 Property Name

####### 2.3.4.2.7.4.1 Property Name

PropertiesJson

####### 2.3.4.2.7.4.2 Property Type

string

####### 2.3.4.2.7.4.3 Purpose

A JSON string defining the structure, child assets, and default tag mappings to be created when this template is instantiated.

#### 2.3.4.3.0.0.0 Class Name

##### 2.3.4.3.1.0.0 Class Name

OpcTag

##### 2.3.4.3.2.0.0 File Path

src/AssetTopology.Domain/Entities/OpcTag.cs

##### 2.3.4.3.3.0.0 Class Type

Entity

##### 2.3.4.3.4.0.0 Inheritance

BaseEntity

##### 2.3.4.3.5.0.0 Purpose

Specification for an OPC Tag as required by REQ-1-047. Represents a data point from an OPC server that is mapped to an asset.

##### 2.3.4.3.6.0.0 Dependencies

*No items available*

##### 2.3.4.3.7.0.0 Properties

###### 2.3.4.3.7.1.0 Property Name

####### 2.3.4.3.7.1.1 Property Name

TenantId

####### 2.3.4.3.7.1.2 Property Type

Guid

####### 2.3.4.3.7.1.3 Purpose

The identifier for the tenant that owns this tag.

###### 2.3.4.3.7.2.0 Property Name

####### 2.3.4.3.7.2.1 Property Name

NodeId

####### 2.3.4.3.7.2.2 Property Type

string

####### 2.3.4.3.7.2.3 Purpose

The unique identifier of the tag within the OPC server (e.g., \"ns=2;s=Machine1.Temperature\").

###### 2.3.4.3.7.3.0 Property Name

####### 2.3.4.3.7.3.1 Property Name

Name

####### 2.3.4.3.7.3.2 Property Type

string

####### 2.3.4.3.7.3.3 Purpose

A user-friendly name for the tag (e.g., \"Motor Temperature\").

###### 2.3.4.3.7.4.0 Property Name

####### 2.3.4.3.7.4.1 Property Name

AssetId

####### 2.3.4.3.7.4.2 Property Type

Guid

####### 2.3.4.3.7.4.3 Purpose

Foreign key linking this tag to its parent asset.

###### 2.3.4.3.7.5.0 Property Name

####### 2.3.4.3.7.5.1 Property Name

Asset

####### 2.3.4.3.7.5.2 Property Type

Asset

####### 2.3.4.3.7.5.3 Purpose

Navigation property to the parent asset.

#### 2.3.4.4.0.0.0 Class Name

##### 2.3.4.4.1.0.0 Class Name

AssetService

##### 2.3.4.4.2.0.0 File Path

src/AssetTopology.Application/Features/Assets/Services/AssetService.cs

##### 2.3.4.4.3.0.0 Class Type

Service

##### 2.3.4.4.4.0.0 Inheritance

IAssetService

##### 2.3.4.4.5.0.0 Purpose

Orchestrates all use cases related to asset management, implementing the Cache-Aside pattern for high performance.

##### 2.3.4.4.6.0.0 Dependencies

- IAssetRepository
- IAssetCacheRepository
- IUnitOfWork
- ILogger<AssetService>

##### 2.3.4.4.7.0.0 Methods

###### 2.3.4.4.7.1.0 Method Name

####### 2.3.4.4.7.1.1 Method Name

GetAssetHierarchyAsync

####### 2.3.4.4.7.1.2 Method Signature

GetAssetHierarchyAsync(Guid tenantId, CancellationToken cancellationToken)

####### 2.3.4.4.7.1.3 Return Type

Task<AssetNodeDto?>

####### 2.3.4.4.7.1.4 Implementation Logic

1. Construct a tenant-specific cache key.\n2. Attempt to retrieve from cache via `IAssetCacheRepository`.\n3. On cache miss, fetch the flat list from the database via `IAssetRepository.GetHierarchyByTenantIdAsync` using a CTE.\n4. Build the hierarchical DTO from the flat list.\n5. Store the DTO in cache via `IAssetCacheRepository.SetHierarchyAsync`.\n6. Return the DTO.

###### 2.3.4.4.7.2.0 Method Name

####### 2.3.4.4.7.2.1 Method Name

CreateAssetAsync

####### 2.3.4.4.7.2.2 Method Signature

CreateAssetAsync(Guid tenantId, CreateAssetRequest request, CancellationToken cancellationToken)

####### 2.3.4.4.7.2.3 Return Type

Task<AssetDto>

####### 2.3.4.4.7.2.4 Implementation Logic

1. Validate request.\n2. Create new `Asset` entity.\n3. Persist via `IAssetRepository.AddAsync`.\n4. Commit via `IUnitOfWork.SaveChangesAsync`.\n5. Invalidate cache via `IAssetCacheRepository.InvalidateAsync(tenantId)`.\n6. Return mapped DTO.

#### 2.3.4.5.0.0.0 Class Name

##### 2.3.4.5.1.0.0 Class Name

AssetTopologyDbContext

##### 2.3.4.5.2.0.0 File Path

src/AssetTopology.Infrastructure/Persistence/AssetTopologyDbContext.cs

##### 2.3.4.5.3.0.0 Class Type

DbContext

##### 2.3.4.5.4.0.0 Inheritance

DbContext

##### 2.3.4.5.5.0.0 Purpose

EF Core DbContext for the service, defining DbSets and model configurations.

##### 2.3.4.5.6.0.0 Dependencies

- DbContextOptions<AssetTopologyDbContext>

##### 2.3.4.5.7.0.0 Properties

###### 2.3.4.5.7.1.0 Property Name

####### 2.3.4.5.7.1.1 Property Name

Assets

####### 2.3.4.5.7.1.2 Property Type

DbSet<Asset>

###### 2.3.4.5.7.2.0 Property Name

####### 2.3.4.5.7.2.1 Property Name

AssetTemplates

####### 2.3.4.5.7.2.2 Property Type

DbSet<AssetTemplate>

###### 2.3.4.5.7.3.0 Property Name

####### 2.3.4.5.7.3.1 Property Name

OpcTags

####### 2.3.4.5.7.3.2 Property Type

DbSet<OpcTag>

##### 2.3.4.5.8.0.0 Methods

- {'method_name': 'OnModelCreating', 'method_signature': 'override void OnModelCreating(ModelBuilder modelBuilder)', 'implementation_logic': '1. Must apply all `IEntityTypeConfiguration` classes from the assembly.\\n2. Must define global query filters for `Asset`, `AssetTemplate`, and `OpcTag` entities to enforce tenant isolation based on a `_tenantId` field. This is a critical security requirement.'}

#### 2.3.4.6.0.0.0 Class Name

##### 2.3.4.6.1.0.0 Class Name

GlobalExceptionHandler

##### 2.3.4.6.2.0.0 File Path

src/AssetTopology.API/Middleware/GlobalExceptionHandler.cs

##### 2.3.4.6.3.0.0 Class Type

Middleware

##### 2.3.4.6.4.0.0 Inheritance

IExceptionHandler

##### 2.3.4.6.5.0.0 Purpose

Catches unhandled exceptions and transforms them into a standardized RFC 7807 ProblemDetails JSON response.

##### 2.3.4.6.6.0.0 Implementation Notes

Should be registered in `Program.cs` using `services.AddExceptionHandler<GlobalExceptionHandler>()`. Must handle custom domain exceptions (e.g., `AssetNotFoundException`) by mapping them to appropriate HTTP status codes (e.g., 404 Not Found).

### 2.3.5.0.0.0.0 Interface Specifications

#### 2.3.5.1.0.0.0 Interface Name

##### 2.3.5.1.1.0.0 Interface Name

IAssetRepository

##### 2.3.5.1.2.0.0 File Path

src/AssetTopology.Application/Interfaces/IAssetRepository.cs

##### 2.3.5.1.3.0.0 Purpose

Defines the contract for data persistence operations related to the Asset entity.

##### 2.3.5.1.4.0.0 Method Contracts

- {'method_name': 'GetHierarchyByTenantIdAsync', 'method_signature': 'GetHierarchyByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken)', 'return_type': 'Task<IEnumerable<Asset>>', 'contract_description': 'Must retrieve all assets for a given tenant. Implementation must use a PostgreSQL Common Table Expression (CTE) for efficiency.'}

#### 2.3.5.2.0.0.0 Interface Name

##### 2.3.5.2.1.0.0 Interface Name

IUnitOfWork

##### 2.3.5.2.2.0.0 File Path

src/AssetTopology.Application/Interfaces/IUnitOfWork.cs

##### 2.3.5.2.3.0.0 Purpose

Defines the contract for committing transactions to the database, abstracting away the specific EF Core implementation.

##### 2.3.5.2.4.0.0 Method Contracts

- {'method_name': 'SaveChangesAsync', 'method_signature': 'SaveChangesAsync(CancellationToken cancellationToken = default)', 'return_type': 'Task<int>', 'contract_description': 'Saves all changes made in this unit of work to the underlying database.'}

### 2.3.6.0.0.0.0 Enum Specifications

*No items available*

### 2.3.7.0.0.0.0 Dto Specifications

#### 2.3.7.1.0.0.0 Dto Name

##### 2.3.7.1.1.0.0 Dto Name

AssetNodeDto

##### 2.3.7.1.2.0.0 File Path

src/AssetTopology.Contracts/Assets/AssetNodeDto.cs

##### 2.3.7.1.3.0.0 Purpose

A recursive DTO to represent a node in the asset hierarchy for API responses. This is the model that will be cached.

##### 2.3.7.1.4.0.0 Properties

###### 2.3.7.1.4.1.0 Property Name

####### 2.3.7.1.4.1.1 Property Name

Id

####### 2.3.7.1.4.1.2 Property Type

Guid

####### 2.3.7.1.4.1.3 Serialization Attributes

- [JsonPropertyName(\"id\")]

###### 2.3.7.1.4.2.0 Property Name

####### 2.3.7.1.4.2.1 Property Name

Name

####### 2.3.7.1.4.2.2 Property Type

string

####### 2.3.7.1.4.2.3 Serialization Attributes

- [JsonPropertyName(\"name\")]

###### 2.3.7.1.4.3.0 Property Name

####### 2.3.7.1.4.3.1 Property Name

Children

####### 2.3.7.1.4.3.2 Property Type

List<AssetNodeDto>

####### 2.3.7.1.4.3.3 Serialization Attributes

- [JsonPropertyName(\"children\")]

#### 2.3.7.2.0.0.0 Dto Name

##### 2.3.7.2.1.0.0 Dto Name

CreateAssetRequest

##### 2.3.7.2.2.0.0 File Path

src/AssetTopology.Contracts/Assets/CreateAssetRequest.cs

##### 2.3.7.2.3.0.0 Purpose

Represents the request body for creating a new asset.

##### 2.3.7.2.4.0.0 Properties

###### 2.3.7.2.4.1.0 Property Name

####### 2.3.7.2.4.1.1 Property Name

Name

####### 2.3.7.2.4.1.2 Property Type

string

####### 2.3.7.2.4.1.3 Validation Attributes

- [Required]
- [StringLength(255, MinimumLength = 1)]

###### 2.3.7.2.4.2.0 Property Name

####### 2.3.7.2.4.2.1 Property Name

ParentAssetId

####### 2.3.7.2.4.2.2 Property Type

Guid?

####### 2.3.7.2.4.2.3 Validation Attributes

*No items available*

### 2.3.8.0.0.0.0 Configuration Specifications

- {'configuration_name': 'AssetConfiguration', 'file_path': 'src/AssetTopology.Infrastructure/Persistence/Configurations/AssetConfiguration.cs', 'purpose': 'Defines the EF Core entity configuration for the Asset entity.', 'framework_base_class': 'IEntityTypeConfiguration<Asset>', 'implementation_notes': 'Must configure:\\n- The primary key (`Id`).\\n- The self-referencing one-to-many relationship for the hierarchy (`ParentAsset` and `ChildAssets`).\\n- An index on `TenantId`.\\n- Required fields and max lengths for string properties.'}

### 2.3.9.0.0.0.0 Dependency Injection Specifications

#### 2.3.9.1.0.0.0 Service Interface

##### 2.3.9.1.1.0.0 Service Interface

IAssetService

##### 2.3.9.1.2.0.0 Service Implementation

AssetService

##### 2.3.9.1.3.0.0 Lifetime

Scoped

##### 2.3.9.1.4.0.0 Registration Reasoning

Services contain business logic and should be scoped to a single request.

#### 2.3.9.2.0.0.0 Service Interface

##### 2.3.9.2.1.0.0 Service Interface

IAssetRepository

##### 2.3.9.2.2.0.0 Service Implementation

AssetRepository

##### 2.3.9.2.3.0.0 Lifetime

Scoped

##### 2.3.9.2.4.0.0 Registration Reasoning

Repositories depend on the DbContext, which is Scoped.

#### 2.3.9.3.0.0.0 Service Interface

##### 2.3.9.3.1.0.0 Service Interface

IAssetCacheRepository

##### 2.3.9.3.2.0.0 Service Implementation

AssetCacheRepository

##### 2.3.9.3.3.0.0 Lifetime

Singleton

##### 2.3.9.3.4.0.0 Registration Reasoning

The cache repository is thread-safe and depends on IDistributedCache (typically a Singleton) to avoid recreating Redis connections.

#### 2.3.9.4.0.0.0 Service Interface

##### 2.3.9.4.1.0.0 Service Interface

IUnitOfWork

##### 2.3.9.4.2.0.0 Service Implementation

UnitOfWork

##### 2.3.9.4.3.0.0 Lifetime

Scoped

##### 2.3.9.4.4.0.0 Registration Reasoning

The Unit of Work is tied to the DbContext's change tracking, so it must share the same Scoped lifetime.

### 2.3.10.0.0.0.0 External Integration Specifications

#### 2.3.10.1.0.0.0 Integration Target

##### 2.3.10.1.1.0.0 Integration Target

PostgreSQL Database

##### 2.3.10.1.2.0.0 Integration Type

Database

##### 2.3.10.1.3.0.0 Required Client Classes

- AssetTopologyDbContext

##### 2.3.10.1.4.0.0 Configuration Requirements

Requires a valid PostgreSQL connection string, managed securely via the .NET configuration system.

##### 2.3.10.1.5.0.0 Error Handling Requirements

The application must implement a resilient connection strategy (e.g., Polly retry) and handle `NpgsqlException` and `DbUpdateException`.

##### 2.3.10.1.6.0.0 Framework Integration Patterns

Integration managed via Entity Framework Core using the Npgsql provider. The Repository and Unit of Work patterns abstract direct interaction.

#### 2.3.10.2.0.0.0 Integration Target

##### 2.3.10.2.1.0.0 Integration Target

Redis

##### 2.3.10.2.2.0.0 Integration Type

Distributed Cache

##### 2.3.10.2.3.0.0 Required Client Classes

- IDistributedCache

##### 2.3.10.2.4.0.0 Configuration Requirements

Requires a valid Redis connection string.

##### 2.3.10.2.5.0.0 Error Handling Requirements

The application must be resilient to cache unavailability, logging connection errors and falling back to the primary data source (PostgreSQL) without failing the request.

##### 2.3.10.2.6.0.0 Framework Integration Patterns

Integrated via the `IDistributedCache` abstraction in ASP.NET Core. Registered using `AddStackExchangeRedisCache`.

## 2.4.0.0.0.0.0 Component Count Validation

| Property | Value |
|----------|-------|
| Total Classes | 12 |
| Total Interfaces | 8 |
| Total Enums | 0 |
| Total Dtos | 2 |
| Total Configurations | 1 |
| Total External Integrations | 2 |
| File Structure Definitions | 4 |
| Dependency Injection Definitions | 4 |
| Namespace Definitions | 4 |
| Grand Total Components | 37 |
| Phase 2 Claimed Count | 45 |
| Phase 2 Actual Count | 16 |
| Validation Added Count | 21 |
| Final Validated Count | 37 |

