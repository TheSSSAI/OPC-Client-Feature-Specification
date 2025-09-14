# 1 Integration Specifications

## 1.1 Extraction Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-AST |
| Extraction Timestamp | 2024-07-31T12:00:00Z |
| Mapping Validation Score | 100% |
| Context Completeness Score | 98% |
| Implementation Readiness Level | High |

## 1.2 Relevant Requirements

### 1.2.1 Requirement Id

#### 1.2.1.1 Requirement Id

REQ-1-031

#### 1.2.1.2 Requirement Text

The Asset Hierarchy Management module shall be designed to align with the principles and models of the ISA-95 standard.

#### 1.2.1.3 Validation Criteria

- Users can create assets representing physical equipment in a multi-level hierarchy (e.g., Site > Area > Line).
- The data model supports a self-referencing parent-child relationship.

#### 1.2.1.4 Implementation Implications

- A core `Asset` domain entity with a nullable `ParentAssetId` is required.
- The service must expose REST API endpoints for full CRUD and re-parenting operations on assets.

#### 1.2.1.5 Extraction Reasoning

This is the primary functional requirement for the repository, defining its core purpose of creating a contextual asset model.

### 1.2.2.0 Requirement Id

#### 1.2.2.1 Requirement Id

REQ-1-047

#### 1.2.2.2 Requirement Text

Within the Asset Management module, users must be able to associate (map) specific OPC tags to assets and their properties within the defined hierarchy.

#### 1.2.2.3 Validation Criteria

- A user can select an asset and an unmapped OPC tag and create a persistent association between them.

#### 1.2.2.4 Implementation Implications

- A relationship (likely many-to-many or one-to-many) must be defined between the Asset and OpcTag entities.
- The service must expose API endpoints to create, view, and delete these mappings.

#### 1.2.2.5 Extraction Reasoning

This is a critical data contextualization requirement explicitly assigned to this service.

### 1.2.3.0 Requirement Id

#### 1.2.3.1 Requirement Id

REQ-1-048

#### 1.2.3.2 Requirement Text

The Asset Management module shall include a templating feature.

#### 1.2.3.3 Validation Criteria

- Users can define asset templates with a standard structure and properties.
- Users can create new asset instances from a saved template.

#### 1.2.3.4 Implementation Implications

- An `AssetTemplate` entity and corresponding database table are required.
- Business logic is needed to instantiate a new asset based on a template's definition.

#### 1.2.3.5 Extraction Reasoning

This is an explicit functional requirement for this service, aimed at improving configuration efficiency.

### 1.2.4.0 Requirement Id

#### 1.2.4.1 Requirement Id

REQ-1-069

#### 1.2.4.2 Requirement Text

The system must provide tools and documentation to facilitate data migration from legacy systems. This shall include a bulk import feature that accepts CSV and JSON files for asset hierarchies, tag configurations, and user accounts.

#### 1.2.4.3 Validation Criteria

- The service accepts a file upload (CSV/JSON) containing asset and tag data.
- The import is processed asynchronously as a background job.
- A validation report is generated upon completion.

#### 1.2.4.4 Implementation Implications

- An API endpoint for file upload and a mechanism to track job status are required.
- An asynchronous worker service must be implemented to process the file.
- Integration with a file storage service (S3) and the audit service is necessary.

#### 1.2.4.5 Extraction Reasoning

This requirement mandates the bulk import capability, a major feature of this service, detailed in its description and in user story US-061.

### 1.2.5.0 Requirement Id

#### 1.2.5.1 Requirement Id

REQ-1-074

#### 1.2.5.2 Requirement Text

The 95th percentile response time for all Central Management Plane API endpoints shall be under 200ms under nominal load.

#### 1.2.5.3 Validation Criteria

- API calls to retrieve the asset hierarchy must meet the latency target.

#### 1.2.5.4 Implementation Implications

- A high-performance caching strategy for the asset hierarchy is mandatory to meet this NFR for read operations.

#### 1.2.5.5 Extraction Reasoning

This performance requirement directly drives the architectural decision to implement a Redis cache for the asset hierarchy.

## 1.3.0.0 Relevant Components

- {'component_name': 'Asset & Topology Service', 'component_specification': 'Manages the ISA-95 compliant asset hierarchy, asset templates, and the mapping of data tags to those assets. This service is the central authority for understanding the physical and logical structure of the industrial environment.', 'implementation_requirements': ['Implement CRUD APIs for Assets, Asset Templates, and OPC Tag mappings.', 'Provide an efficient, cached query mechanism for retrieving asset hierarchies.', 'Implement an asynchronous bulk import feature for assets and tags.', 'Ensure all data access is strictly isolated by tenant.', 'Log all CUD operations to the central audit service.'], 'architectural_context': "Resides in the 'Application Services Layer'. Acts as a foundational service, providing essential context to many other services like Query & Analytics, AI/ML Management, and Alarm & Notification.", 'extraction_reasoning': 'The REPO-SVC-AST repository is the direct and sole implementation of this architectural component.'}

## 1.4.0.0 Architectural Layers

- {'layer_name': 'Application Services Layer (Microservices)', 'layer_responsibilities': 'Implements the core business logic and domain functionality of the Central Management Plane. Each service in this layer owns a specific business capability and exposes its functionality via secure APIs.', 'layer_constraints': ['Services should be stateless to allow for horizontal scaling.', 'Services must enforce tenant data isolation at all times.', 'Communication between services should be done via well-defined APIs (REST, gRPC) or asynchronous messaging.'], 'implementation_patterns': ['Domain-Driven Design (DDD)', 'Microservices Architecture', 'API Gateway for external access'], 'extraction_reasoning': 'The Asset and Topology Service is a primary component within this layer, as defined in its repository mapping and the overall system architecture.'}

## 1.5.0.0 Dependency Interfaces

### 1.5.1.0 Interface Name

#### 1.5.1.1 Interface Name

IAssetRepository (EF Core)

#### 1.5.1.2 Source Repository

REPO-DB-POSTGRES

#### 1.5.1.3 Method Contracts

##### 1.5.1.3.1 Method Name

###### 1.5.1.3.1.1 Method Name

GetHierarchyByTenantAsync

###### 1.5.1.3.1.2 Method Signature

Task<IEnumerable<Asset>> GetHierarchyByTenantAsync(Guid tenantId)

###### 1.5.1.3.1.3 Method Purpose

Retrieves the entire asset hierarchy for a specific tenant from the PostgreSQL database using a Common Table Expression (CTE) for efficient recursive querying.

###### 1.5.1.3.1.4 Integration Context

Called by the service logic when a cache miss occurs for the asset hierarchy.

##### 1.5.1.3.2.0 Method Name

###### 1.5.1.3.2.1 Method Name

AddAssetAsync

###### 1.5.1.3.2.2 Method Signature

Task AddAssetAsync(Asset newAsset)

###### 1.5.1.3.2.3 Method Purpose

Persists a new asset entity to the database.

###### 1.5.1.3.2.4 Integration Context

Called when handling a POST /api/v1/assets request.

##### 1.5.1.3.3.0 Method Name

###### 1.5.1.3.3.1 Method Name

BulkInsertAssetsAsync

###### 1.5.1.3.3.2 Method Signature

Task BulkInsertAssetsAsync(IEnumerable<Asset> assets)

###### 1.5.1.3.3.3 Method Purpose

Performs a high-performance bulk insert of asset records for the data migration feature.

###### 1.5.1.3.3.4 Integration Context

Called by the asynchronous import job processor.

#### 1.5.1.4.0.0 Integration Pattern

Repository Pattern over Entity Framework Core

#### 1.5.1.5.0.0 Communication Protocol

SQL over TCP/IP

#### 1.5.1.6.0.0 Extraction Reasoning

This interface represents the contract for persisting and retrieving asset and topology data, which is a core function of the service.

### 1.5.2.0.0.0 Interface Name

#### 1.5.2.1.0.0 Interface Name

IAssetCacheRepository (Redis)

#### 1.5.2.2.0.0 Source Repository

REPO-CACHE-REDIS

#### 1.5.2.3.0.0 Method Contracts

##### 1.5.2.3.1.0 Method Name

###### 1.5.2.3.1.1 Method Name

GetHierarchyAsync

###### 1.5.2.3.1.2 Method Signature

Task<string?> GetHierarchyAsync(Guid tenantId)

###### 1.5.2.3.1.3 Method Purpose

Attempts to retrieve the serialized asset hierarchy for a tenant from the Redis cache.

###### 1.5.2.3.1.4 Integration Context

This is the first call made when a request for the asset hierarchy is received, as per the Cache-Aside pattern.

##### 1.5.2.3.2.0 Method Name

###### 1.5.2.3.2.1 Method Name

SetHierarchyAsync

###### 1.5.2.3.2.2 Method Signature

Task SetHierarchyAsync(Guid tenantId, string hierarchyJson, TimeSpan expiry)

###### 1.5.2.3.2.3 Method Purpose

Stores the serialized asset hierarchy in the Redis cache with a specific Time-To-Live (TTL).

###### 1.5.2.3.2.4 Integration Context

Called after a successful fetch from the database following a cache miss.

##### 1.5.2.3.3.0 Method Name

###### 1.5.2.3.3.1 Method Name

InvalidateHierarchyAsync

###### 1.5.2.3.3.2 Method Signature

Task InvalidateHierarchyAsync(Guid tenantId)

###### 1.5.2.3.3.3 Method Purpose

Deletes the cached asset hierarchy for a tenant, forcing a refresh from the database on the next request.

###### 1.5.2.3.3.4 Integration Context

Must be called after any CUD (Create, Update, Delete) operation on the asset structure to prevent stale data.

#### 1.5.2.4.0.0 Integration Pattern

Cache-Aside Pattern

#### 1.5.2.5.0.0 Communication Protocol

Redis Protocol (RESP)

#### 1.5.2.6.0.0 Extraction Reasoning

This interface represents the contract for the caching strategy, which is a critical performance requirement for this service.

### 1.5.3.0.0.0 Interface Name

#### 1.5.3.1.0.0 Interface Name

IAuditServiceGrpcClient

#### 1.5.3.2.0.0 Source Repository

REPO-SVC-ADT

#### 1.5.3.3.0.0 Method Contracts

- {'method_name': 'LogActionAsync', 'method_signature': 'Task<LogActionResponse> LogActionAsync(LogActionRequest request)', 'method_purpose': 'Sends a structured log of a significant CUD operation (e.g., Asset Created, Template Deleted) to the central Audit Service for immutable storage.', 'integration_context': 'Called within the same unit of work as the primary database transaction to ensure atomicity of the business operation and its audit record.'}

#### 1.5.3.4.0.0 Integration Pattern

gRPC Client Call

#### 1.5.3.5.0.0 Communication Protocol

gRPC

#### 1.5.3.6.0.0 Extraction Reasoning

Fulfills the system-wide requirement (REQ-1-040) for all significant actions to be audited. gRPC is chosen for high-performance internal communication.

### 1.5.4.0.0.0 Interface Name

#### 1.5.4.1.0.0 Interface Name

IFileStorageService (S3)

#### 1.5.4.2.0.0 Source Repository

REPO-STORE-S3

#### 1.5.4.3.0.0 Method Contracts

- {'method_name': 'UploadFileAsync', 'method_signature': 'Task<string> UploadFileAsync(string key, Stream contentStream, string contentType)', 'method_purpose': 'Uploads a bulk import file (CSV/JSON) to a secure, tenant-specific location in Amazon S3 for later processing.', 'integration_context': 'Called by the initial API request handler for a bulk import operation (from US-061).'}

#### 1.5.4.4.0.0 Integration Pattern

AWS SDK Client

#### 1.5.4.5.0.0 Communication Protocol

HTTPS

#### 1.5.4.6.0.0 Extraction Reasoning

Required to handle file uploads for the asynchronous bulk import feature specified in REQ-1-069 and user story US-061.

## 1.6.0.0.0.0 Exposed Interfaces

### 1.6.1.0.0.0 Interface Name

#### 1.6.1.1.0.0 Interface Name

Asset Management API

#### 1.6.1.2.0.0 Consumer Repositories

- REPO-GW-API

#### 1.6.1.3.0.0 Method Contracts

##### 1.6.1.3.1.0 Method Name

###### 1.6.1.3.1.1 Method Name

GetAssetHierarchy

###### 1.6.1.3.1.2 Method Signature

GET /api/v1/assets

###### 1.6.1.3.1.3 Method Purpose

Retrieves the complete, nested asset hierarchy for the authenticated user's tenant.

###### 1.6.1.3.1.4 Implementation Requirements

This endpoint must be highly performant, leveraging the Redis cache-aside pattern. P95 latency must be under 200ms.

##### 1.6.1.3.2.0 Method Name

###### 1.6.1.3.2.1 Method Name

CreateAsset

###### 1.6.1.3.2.2 Method Signature

POST /api/v1/assets

###### 1.6.1.3.2.3 Method Purpose

Creates a new asset in the hierarchy.

###### 1.6.1.3.2.4 Implementation Requirements

Upon successful creation, the service must trigger an invalidation of the cached asset hierarchy for the tenant and log the action to the audit service.

##### 1.6.1.3.3.0 Method Name

###### 1.6.1.3.3.1 Method Name

UpdateAsset

###### 1.6.1.3.3.2 Method Signature

PUT /api/v1/assets/{id}

###### 1.6.1.3.3.3 Method Purpose

Updates an existing asset's properties, such as its name or parent.

###### 1.6.1.3.3.4 Implementation Requirements

Must trigger cache invalidation and create an audit log entry.

##### 1.6.1.3.4.0 Method Name

###### 1.6.1.3.4.1 Method Name

DeleteAsset

###### 1.6.1.3.4.2 Method Signature

```sql
DELETE /api/v1/assets/{id}
```

###### 1.6.1.3.4.3 Method Purpose

Deletes an asset and its entire sub-tree.

###### 1.6.1.3.4.4 Implementation Requirements

Must trigger cache invalidation and create audit log entries for all deleted assets.

#### 1.6.1.4.0.0 Service Level Requirements

- P95 latency for read operations must be < 200ms.
- Availability must be >= 99.95%.

#### 1.6.1.5.0.0 Implementation Constraints

- All endpoints must enforce tenant isolation based on the JWT claims.
- The API must conform to the system's RESTful API standards, including URI versioning.

#### 1.6.1.6.0.0 Extraction Reasoning

This is the primary REST interface for managing the core Asset entities, consumed by the frontend via the API Gateway.

### 1.6.2.0.0.0 Interface Name

#### 1.6.2.1.0.0 Interface Name

Asset Template & Tag Mapping API

#### 1.6.2.2.0.0 Consumer Repositories

- REPO-GW-API

#### 1.6.2.3.0.0 Method Contracts

##### 1.6.2.3.1.0 Method Name

###### 1.6.2.3.1.1 Method Name

CreateTagMapping

###### 1.6.2.3.1.2 Method Signature

POST /api/v1/assets/{assetId}/tags

###### 1.6.2.3.1.3 Method Purpose

Creates a mapping between an OPC tag and an asset.

###### 1.6.2.3.1.4 Implementation Requirements

Must validate that the target asset exists and belongs to the user's tenant before creating the tag mapping.

##### 1.6.2.3.2.0 Method Name

###### 1.6.2.3.2.1 Method Name

GetAssetTemplates

###### 1.6.2.3.2.2 Method Signature

GET /api/v1/asset-templates

###### 1.6.2.3.2.3 Method Purpose

Retrieves a list of all available asset templates for the tenant.

###### 1.6.2.3.2.4 Implementation Requirements

Endpoint for managing asset templates as per REQ-1-048.

##### 1.6.2.3.3.0 Method Name

###### 1.6.2.3.3.1 Method Name

CreateAssetTemplate

###### 1.6.2.3.3.2 Method Signature

POST /api/v1/asset-templates

###### 1.6.2.3.3.3 Method Purpose

Creates a new asset template.

###### 1.6.2.3.3.4 Implementation Requirements

Endpoint for managing asset templates as per REQ-1-048.

#### 1.6.2.4.0.0 Service Level Requirements

- P95 latency for all operations must be < 300ms.

#### 1.6.2.5.0.0 Implementation Constraints

- All endpoints must enforce tenant isolation.

#### 1.6.2.6.0.0 Extraction Reasoning

This interface provides the functionality for managing asset templates and tag mappings, which are core responsibilities of the service.

### 1.6.3.0.0.0 Interface Name

#### 1.6.3.1.0.0 Interface Name

IAssetContextInternalService (gRPC)

#### 1.6.3.2.0.0 Consumer Repositories

- REPO-SVC-QRY
- REPO-SVC-ALM
- REPO-SVC-AI

#### 1.6.3.3.0.0 Method Contracts

- {'method_name': 'GetAssetContextForTags', 'method_signature': 'rpc GetAssetContextForTags(GetAssetContextRequest) returns (GetAssetContextResponse)', 'method_purpose': 'Provides a high-performance, low-latency mechanism for other microservices to enrich raw data points with their corresponding asset context (e.g., asset name, path in hierarchy).', 'implementation_requirements': 'This gRPC service should query a denormalized or cached representation of the asset/tag mapping for maximum performance, avoiding complex joins on the critical path.'}

#### 1.6.3.4.0.0 Service Level Requirements

- P95 latency must be < 20ms.

#### 1.6.3.5.0.0 Implementation Constraints

- This interface is for internal service-to-service communication only and must not be exposed via the API Gateway.

#### 1.6.3.6.0.0 Extraction Reasoning

This internal gRPC interface is added to provide a high-performance data enrichment path for other services, aligning with the architectural principle of using gRPC for inter-service communication.

## 1.7.0.0.0.0 Technology Context

### 1.7.1.0.0.0 Framework Requirements

The service must be built using .NET 8 and ASP.NET Core 8. Data persistence must be implemented with Entity Framework Core 8.0.6 using the Npgsql provider v8.0.4.

### 1.7.2.0.0.0 Integration Technologies

- PostgreSQL: Primary data store for assets, templates, and mappings.
- Redis: High-performance caching layer for the asset hierarchy, using the StackExchange.Redis v2.7.33 library.
- Amazon S3: Object storage for bulk import files, accessed via the AWS SDK for .NET.
- gRPC: For high-performance internal service communication.
- REST/JSON: Protocol for the exposed external API.

### 1.7.3.0.0.0 Performance Constraints

Read operations for the asset hierarchy are performance-critical and must have a P95 latency under 200ms, achieved through an aggressive caching strategy.

### 1.7.4.0.0.0 Security Requirements

Strict tenant data isolation is mandatory on all database queries and cache interactions. The service relies on a validated JWT, provided by the API Gateway, for user identity and tenant information.

## 1.8.0.0.0.0 Extraction Validation

| Property | Value |
|----------|-------|
| Mapping Completeness Check | The repository is fully mapped to its architectura... |
| Cross Reference Validation | All mappings were successfully cross-referenced an... |
| Implementation Readiness Assessment | The context is highly implementation-ready. It spe... |
| Quality Assurance Confirmation | The extracted context is of high quality, demonstr... |

