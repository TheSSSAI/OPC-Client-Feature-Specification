# 1 Integration Specifications

## 1.1 Extraction Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-IAM |
| Extraction Timestamp | 2024-07-31T12:00:00Z |
| Mapping Validation Score | 100% |
| Context Completeness Score | 100% |
| Implementation Readiness Level | High |

## 1.2 Relevant Requirements

### 1.2.1 Requirement Id

#### 1.2.1.1 Requirement Id

REQ-1-011

#### 1.2.1.2 Requirement Text

The system shall implement a Role-Based Access Control (RBAC) model with five predefined, configurable user roles: Administrator, Data Scientist, Engineer, Operator, and Viewer, each with a distinct set of permissions.

#### 1.2.1.3 Validation Criteria

- Administrators can create, read, update, and delete roles.
- Administrators can assign users to one or more roles.
- Access to system functions is denied by default and must be explicitly granted via a role.

#### 1.2.1.4 Implementation Implications

- The service must implement CRUD operations for Role entities.
- A many-to-many relationship between Users and Roles must be persisted.
- Authorization policies in the service must be based on these roles.

#### 1.2.1.5 Extraction Reasoning

This is a core requirement for the IAM service, defining its primary responsibility for Role-Based Access Control. The service's description explicitly states it manages the full lifecycle of roles and permissions.

### 1.2.2.0 Requirement Id

#### 1.2.2.1 Requirement Id

REQ-1-027

#### 1.2.2.2 Requirement Text

All Personally Identifiable Information (PII) must be classified as sensitive and be encrypted both at rest (e.g., in the database) and in transit (using TLS 1.3). The system must provide administrative tools to fulfill data subject rights under GDPR.

#### 1.2.2.3 Validation Criteria

- All API queries and database operations must be filtered by the user's tenant ID.
- PII (e.g., user email) must be encrypted at rest in the database.
- Mechanisms must exist to support data access and deletion requests.

#### 1.2.2.4 Implementation Implications

- A non-optional 'tenantId' filter must be applied to all database queries via EF Core global query filters.
- EF Core value converters or similar database features must be used to encrypt PII fields.
- The service must expose secure endpoints for GDPR-related actions.

#### 1.2.2.5 Extraction Reasoning

This requirement dictates critical security and compliance functionality owned by the IAM service, as it is the primary manager of user data containing PII.

### 1.2.3.0 Requirement Id

#### 1.2.3.1 Requirement Id

REQ-1-024

#### 1.2.3.2 Requirement Text

The system shall be designed with a multi-tenant architecture to logically isolate the data and configurations of different customers (tenants) within a single, shared infrastructure, enabling a Software-as-a-Service (SaaS) business model.

#### 1.2.3.3 Validation Criteria

- The system can onboard multiple, logically separated tenants.
- A single deployed instance of the software serves all tenants.

#### 1.2.3.4 Implementation Implications

- The service must manage the lifecycle of Tenant entities.
- The data model must associate all relevant data (users, roles, etc.) with a specific tenant.

#### 1.2.3.5 Extraction Reasoning

This requirement establishes the multi-tenant SaaS model, for which this service is the authoritative source for tenant management and data isolation enforcement.

### 1.2.4.0 Requirement Id

#### 1.2.4.1 Requirement Id

REQ-1-063

#### 1.2.4.2 Requirement Text

The system's licensing mechanism must be flexible to support various business models, including per-user, per-site, and recurring subscription licenses. The system must also support feature flagging based on license tiers.

#### 1.2.4.3 Validation Criteria

- License details (e.g., maximum user count, feature flags) can be associated with a tenant.
- The system prevents actions that would violate the license terms (e.g., adding more users than allowed).

#### 1.2.4.4 Implementation Implications

- The service must implement the data model for a License entity linked to a Tenant.
- Business logic must be implemented to check license constraints before performing actions like creating a new user.

#### 1.2.4.5 Extraction Reasoning

This requirement is explicitly assigned to the IAM service in its description, making it the central point for managing and enforcing tenant-specific software licenses.

## 1.3.0.0 Relevant Components

- {'component_name': 'Identity & Access Management Service', 'component_specification': 'A microservice that manages users, roles, permissions, tenants, and licenses. It integrates with an external Identity Provider (Keycloak) for authentication, while handling application-specific authorization and identity data.', 'implementation_requirements': ['Must be a stateless .NET 8 service.', 'Must expose secure REST and gRPC APIs for its functionality.', 'Must ensure strict data isolation between tenants.', 'Must synchronize user identity state with the Keycloak IdP.'], 'architectural_context': "Belongs to the Application Services Layer. It is a core backend microservice that underpins the system's security and multi-tenancy model.", 'extraction_reasoning': "The repository REPO-SVC-IAM is the direct and sole implementation of this architectural component, as defined in the 'Application Services Layer (Microservices)' of the architecture."}

## 1.4.0.0 Architectural Layers

- {'layer_name': 'Application Services Layer (Microservices)', 'layer_responsibilities': 'Implements the core business logic and domain functionality of the Central Management Plane. Each service owns a specific business capability and exposes its functionality via APIs.', 'layer_constraints': ['Services should be stateless to allow for horizontal scaling.', 'Services must implement tenant data isolation.', 'Communication should be via well-defined APIs (REST, gRPC) or asynchronous messaging.'], 'implementation_patterns': ['Domain-Driven Design (DDD)', 'Microservices Architecture', 'Containerization (Docker)'], 'extraction_reasoning': "The repository is defined as a 'Microservice' and is a key component within this layer, responsible for the identity and access management domain."}

## 1.5.0.0 Dependency Interfaces

### 1.5.1.0 Interface Name

#### 1.5.1.1 Interface Name

EF Core DbContext (IAM)

#### 1.5.1.2 Source Repository

REPO-DB-POSTGRES

#### 1.5.1.3 Method Contracts

##### 1.5.1.3.1 Method Name

###### 1.5.1.3.1.1 Method Name

DbSet<T>.FindAsync()

###### 1.5.1.3.1.2 Method Signature

ValueTask<TEntity> FindAsync(params object[] keyValues)

###### 1.5.1.3.1.3 Method Purpose

Asynchronously finds an entity with the given primary key values.

###### 1.5.1.3.1.4 Integration Context

Used within repository implementations to retrieve single entities (e.g., User, Tenant) by their ID.

##### 1.5.1.3.2.0 Method Name

###### 1.5.1.3.2.1 Method Name

DbContext.SaveChangesAsync()

###### 1.5.1.3.2.2 Method Signature

Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)

###### 1.5.1.3.2.3 Method Purpose

Asynchronously saves all changes made in this context to the database.

###### 1.5.1.3.2.4 Integration Context

Called at the end of a unit of work to commit transactions.

#### 1.5.1.4.0.0 Integration Pattern

Repository Pattern over Entity Framework Core

#### 1.5.1.5.0.0 Communication Protocol

SQL over TCP/IP

#### 1.5.1.6.0.0 Extraction Reasoning

This is the primary dependency for data persistence, as the IAM service is the owner of the tenant, user, and role data models.

### 1.5.2.0.0.0 Interface Name

#### 1.5.2.1.0.0 Interface Name

Keycloak Admin API Client

#### 1.5.2.2.0.0 Source Repository

REPO-IDP-KEYCLOAK

#### 1.5.2.3.0.0 Method Contracts

##### 1.5.2.3.1.0 Method Name

###### 1.5.2.3.1.1 Method Name

CreateUser()

###### 1.5.2.3.1.2 Method Signature

Task CreateUserAsync(UserRepresentation user)

###### 1.5.2.3.1.3 Method Purpose

Creates a new user record within the Keycloak identity store.

###### 1.5.2.3.1.4 Integration Context

Called when a new user is created in the IAM service to synchronize the identity to Keycloak, ensuring a user principal exists for authentication.

##### 1.5.2.3.2.0 Method Name

###### 1.5.2.3.2.1 Method Name

AssignClientRole()

###### 1.5.2.3.2.2 Method Signature

Task AssignClientRoleAsync(string userId, string clientId, RoleRepresentation role)

###### 1.5.2.3.2.3 Method Purpose

Assigns an application-specific role to a user in Keycloak.

###### 1.5.2.3.2.4 Integration Context

Called when a user's roles are changed in the IAM service to ensure JWT claims are correct upon the user's next login.

#### 1.5.2.4.0.0 Integration Pattern

Resilient REST API Client

#### 1.5.2.5.0.0 Communication Protocol

HTTPS/REST

#### 1.5.2.6.0.0 Extraction Reasoning

This is a critical dependency for integrating with the external IdP (REQ-1-080). The IAM service must synchronize user and role state with Keycloak to ensure the integrity of the authentication and authorization process.

### 1.5.3.0.0.0 Interface Name

#### 1.5.3.1.0.0 Interface Name

IAuditService (gRPC)

#### 1.5.3.2.0.0 Source Repository

REPO-SVC-ADT

#### 1.5.3.3.0.0 Method Contracts

- {'method_name': 'LogAction', 'method_signature': 'rpc LogAction (LogActionRequest) returns (LogActionResponse);', 'method_purpose': 'To send a structured, immutable log of a significant action (e.g., user created, role assigned) to the central Audit Service.', 'integration_context': 'Called at the end of any command handler that performs a state change (Create, Update, Delete) to ensure the action is recorded for compliance.'}

#### 1.5.3.4.0.0 Integration Pattern

Internal gRPC Client Call

#### 1.5.3.5.0.0 Communication Protocol

gRPC

#### 1.5.3.6.0.0 Extraction Reasoning

Fulfills the system-wide requirement for auditable actions (REQ-1-040) by integrating with the dedicated Audit Service. The use of gRPC aligns with the architecture's mandate for high-performance internal service-to-service communication (REQ-1-072).

## 1.6.0.0.0.0 Exposed Interfaces

### 1.6.1.0.0.0 Interface Name

#### 1.6.1.1.0.0 Interface Name

IAM Management REST API

#### 1.6.1.2.0.0 Consumer Repositories

- REPO-GW-API

#### 1.6.1.3.0.0 Method Contracts

##### 1.6.1.3.1.0 Method Name

###### 1.6.1.3.1.1 Method Name

POST /api/v1/users

###### 1.6.1.3.1.2 Method Signature

POST /api/v1/users (body: UserCreateDto) -> Task<IActionResult>

###### 1.6.1.3.1.3 Method Purpose

Creates a new user for the current tenant.

###### 1.6.1.3.1.4 Implementation Requirements

Must validate the input DTO, check license limits, and call the Keycloak Admin API to create the user in the IdP.

##### 1.6.1.3.2.0 Method Name

###### 1.6.1.3.2.1 Method Name

PUT /api/v1/users/{id}/roles

###### 1.6.1.3.2.2 Method Signature

PUT /api/v1/users/{id}/roles (body: RoleAssignmentDto) -> Task<IActionResult>

###### 1.6.1.3.2.3 Method Purpose

Assigns or updates the roles for a specific user.

###### 1.6.1.3.2.4 Implementation Requirements

Must be an idempotent operation and synchronize role assignments to Keycloak.

##### 1.6.1.3.3.0 Method Name

###### 1.6.1.3.3.1 Method Name

POST /api/v1/tenants

###### 1.6.1.3.3.2 Method Signature

POST /api/v1/tenants (body: TenantCreateDto) -> Task<IActionResult>

###### 1.6.1.3.3.3 Method Purpose

Onboards a new tenant into the system.

###### 1.6.1.3.3.4 Implementation Requirements

Must create the tenant record and set up initial configuration, such as a default administrator user and license.

#### 1.6.1.4.0.0 Service Level Requirements

- P95 latency for all endpoints must be under 200ms.

#### 1.6.1.5.0.0 Implementation Constraints

- All endpoints must enforce tenant isolation, ensuring a user from Tenant A cannot access data for Tenant B.
- Input must be validated to prevent security vulnerabilities like injection attacks.
- The API must be versioned and documented using OpenAPI.

#### 1.6.1.6.0.0 Extraction Reasoning

This interface provides the primary administrative functions for managing identity and tenancy, consumed by the frontend application via the API Gateway.

### 1.6.2.0.0.0 Interface Name

#### 1.6.2.1.0.0 Interface Name

IAuthorizationService (gRPC)

#### 1.6.2.2.0.0 Consumer Repositories

- REPO-SVC-AST
- REPO-SVC-DVM
- Other backend microservices

#### 1.6.2.3.0.0 Method Contracts

- {'method_name': 'CheckPermission', 'method_signature': 'rpc CheckPermission (PermissionCheckRequest) returns (PermissionCheckResponse);', 'method_purpose': 'Provides a high-performance mechanism for other microservices to verify if a user has the required permission to perform an action on a specific resource.', 'implementation_requirements': 'The request must include user ID, tenant ID, permission string, and an optional resource ID (e.g., asset ID). The response is a simple boolean. The implementation must be highly optimized and leverage caching.'}

#### 1.6.2.4.0.0 Service Level Requirements

- P99 latency must be under 50ms, as this is on the critical path for most authorized API calls.

#### 1.6.2.5.0.0 Implementation Constraints

- This interface is for internal, trusted service-to-service communication only and should not be exposed via the API Gateway.

#### 1.6.2.6.0.0 Extraction Reasoning

This internal-facing gRPC interface is necessary to fulfill the service's role as the central authorization authority for the entire microservices ecosystem, using the high-performance protocol specified by the architecture (REQ-1-072).

## 1.7.0.0.0.0 Technology Context

### 1.7.1.0.0.0 Framework Requirements

Must be built using .NET 8, ASP.NET Core 8 for APIs, and Entity Framework Core 8 for data access against a PostgreSQL database.

### 1.7.2.0.0.0 Integration Technologies

- Npgsql.EntityFrameworkCore.PostgreSQL v8.0.4
- MediatR v12.3.0 for CQRS implementation
- FluentValidation.AspNetCore v11.3.0 for request validation
- A resilient REST Client for the Keycloak Admin API using IHttpClientFactory and Polly
- Grpc.AspNetCore for the internal authorization service

### 1.7.3.0.0.0 Performance Constraints

P95 latency must be under 200ms for management APIs. P99 latency for internal gRPC authorization checks must be under 50ms.

### 1.7.4.0.0.0 Security Requirements

All API endpoints must be protected with authorization policies. All Personally Identifiable Information (PII) must be encrypted at rest in the database. Communication with Keycloak must be secure.

## 1.8.0.0.0.0 Extraction Validation

| Property | Value |
|----------|-------|
| Mapping Completeness Check | All mappings to architecture, components, and requ... |
| Cross Reference Validation | The service's role, its dependencies (PostgreSQL, ... |
| Implementation Readiness Assessment | High. The context provides a specific technology s... |
| Quality Assurance Confirmation | The enhanced integration specification is internal... |

