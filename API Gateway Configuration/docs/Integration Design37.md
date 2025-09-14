# 1 Integration Specifications

## 1.1 Extraction Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-IAM |
| Extraction Timestamp | 2024-05-07T10:30:00Z |
| Mapping Validation Score | 100% |
| Context Completeness Score | 98% |
| Implementation Readiness Level | High |

## 1.2 Relevant Requirements

### 1.2.1 Requirement Id

#### 1.2.1.1 Requirement Id

REQ-1-011

#### 1.2.1.2 Requirement Text

The system shall implement a Role-Based Access Control (RBAC) model with five predefined, configurable user roles.

#### 1.2.1.3 Validation Criteria

- Administrators can create, view, update, and delete custom roles (US-049).
- Administrators can assign users to one or more roles (US-048).
- Access to system functions is denied by default and must be explicitly granted via a role.

#### 1.2.1.4 Implementation Implications

- The service must implement CRUD operations for Role and UserRole entities.
- A many-to-many relationship between Users and Roles must be persisted.
- Authorization policies in the service must be based on these roles and their associated permissions.

#### 1.2.1.5 Extraction Reasoning

This requirement is explicitly mapped and defines the core RBAC functionality that this service is responsible for implementing.

### 1.2.2.0 Requirement Id

#### 1.2.2.1 Requirement Id

REQ-1-024

#### 1.2.2.2 Requirement Text

The system shall be designed with a multi-tenant architecture to logically isolate the data and configurations of different customers.

#### 1.2.2.3 Validation Criteria

- The system can onboard multiple, logically separated tenants.
- A single deployed instance of the software serves all tenants.
- Data associated with Tenant A is never visible to users from Tenant B.

#### 1.2.2.4 Implementation Implications

- The service must manage the lifecycle of Tenant entities.
- The data model must associate all relevant data (users, roles, licenses) with a specific tenant ID.
- All database queries must be filtered by the current user's tenant ID.

#### 1.2.2.5 Extraction Reasoning

This is a foundational architectural requirement. The IAM service is the primary enforcer of multi-tenancy by managing tenant entities and scoping all user/role data to a specific tenant.

### 1.2.3.0 Requirement Id

#### 1.2.3.1 Requirement Id

REQ-1-061

#### 1.2.3.2 Requirement Text

The RBAC implementation must be granular, allowing permissions to be assigned not just globally, but also scoped to specific areas of the asset hierarchy.

#### 1.2.3.3 Validation Criteria

- An Administrator can create a custom role where a permission (e.g., 'Acknowledge Alarms') is limited to a specific asset node (e.g., 'Site B > Production Line 3').
- A user with this role can acknowledge alarms for assets under 'Line 3' but not for assets under 'Line 2'.

#### 1.2.3.4 Implementation Implications

- The UserRole entity must support an optional `assetScopeId` to link a role assignment to a node in the asset hierarchy.
- The authorization check logic must be capable of evaluating these resource-specific scopes.
- The service must integrate with the Asset & Topology Service (REPO-SVC-AST) to validate asset IDs.

#### 1.2.3.5 Extraction Reasoning

This requirement adds significant detail to the RBAC implementation, mandating a resource-scoped permission model which is a core complexity of this service.

### 1.2.4.0 Requirement Id

#### 1.2.4.1 Requirement Id

REQ-1-063

#### 1.2.4.2 Requirement Text

The system's licensing mechanism must be flexible to support various business models, including per-user, per-site, and recurring subscription licenses.

#### 1.2.4.3 Validation Criteria

- License details (e.g., maximum user count, feature flags) can be associated with a tenant.
- The system prevents actions that would violate the license terms (e.g., adding more users than allowed).

#### 1.2.4.4 Implementation Implications

- The service must implement the data model for a License entity linked to a Tenant.
- Business logic must be implemented to check license constraints before performing actions like creating a new user.

#### 1.2.4.5 Extraction Reasoning

This requirement is explicitly mapped in the repository's 'sds'. The IAM service is the logical owner for managing and enforcing tenant-level licensing.

### 1.2.5.0 Requirement Id

#### 1.2.5.1 Requirement Id

REQ-1-027

#### 1.2.5.2 Requirement Text

All Personally Identifiable Information (PII) must be classified as sensitive and be encrypted both at rest and in transit.

#### 1.2.5.3 Validation Criteria

- PII (e.g., user email, name) must be encrypted at rest in the database.
- The service must provide administrative tools to support data subject rights under GDPR.

#### 1.2.5.4 Implementation Implications

- EF Core value converters or equivalent database features must be used to encrypt/decrypt PII fields.
- The service must expose secure API endpoints for GDPR-related actions.

#### 1.2.5.5 Extraction Reasoning

The IAM service is the primary manager of user PII, making this a critical non-functional requirement for its implementation.

## 1.3.0.0 Relevant Components

- {'component_name': 'Identity & Access Management Service', 'component_specification': 'A microservice that manages tenants, users, roles, and application-specific permissions. It integrates with an external Identity Provider (Keycloak) for authentication, while handling all authorization logic, including granular, asset-scoped permissions.', 'implementation_requirements': ['Must be a stateless .NET 8 service.', 'Must expose secure REST APIs for management and gRPC APIs for high-performance internal authorization checks.', 'Must ensure strict data isolation between tenants.', 'Must synchronize user and role state with the Keycloak IdP via its Admin API.'], 'architectural_context': "Belongs to the Application Services Layer. It is a core backend microservice that underpins the system's security and multi-tenancy model, consumed by all other services for authorization.", 'extraction_reasoning': 'The repository REPO-SVC-IAM is the direct and sole implementation of this architectural component.'}

## 1.4.0.0 Architectural Layers

- {'layer_name': 'Application Services Layer (Microservices)', 'layer_responsibilities': 'Implements the core business logic and domain functionality of the Central Management Plane. Each service owns a specific business capability and exposes its functionality via APIs.', 'layer_constraints': ['Services should be stateless to allow for horizontal scaling.', 'Services must implement tenant data isolation.', 'Communication should be via well-defined APIs (REST, gRPC) or asynchronous messaging.'], 'implementation_patterns': ['Domain-Driven Design (DDD)', 'Microservices Architecture', 'Containerization (Docker)'], 'extraction_reasoning': "The repository is defined as a 'Microservice' and is a central component of this architectural layer, adhering to its principles."}

## 1.5.0.0 Dependency Interfaces

### 1.5.1.0 Interface Name

#### 1.5.1.1 Interface Name

PostgreSQL Database (IAM Schema)

#### 1.5.1.2 Source Repository

REPO-DB-POSTGRES

#### 1.5.1.3 Method Contracts

- {'method_name': 'EF Core Operations', 'method_signature': 'DbSet<TEntity>.Where(), .AddAsync(), .FindAsync(), DbContext.SaveChangesAsync()', 'method_purpose': 'To perform CRUD operations on IAM-related entities such as Tenants, Users, Roles, and Licenses.', 'integration_context': "Used within the service's repository implementations to persist and retrieve all state."}

#### 1.5.1.4 Integration Pattern

Repository Pattern over Entity Framework Core

#### 1.5.1.5 Communication Protocol

SQL over TCP/IP

#### 1.5.1.6 Extraction Reasoning

This is the primary mechanism for data persistence for the IAM service, as defined in its 'sds'.

### 1.5.2.0 Interface Name

#### 1.5.2.1 Interface Name

Keycloak Admin API Client

#### 1.5.2.2 Source Repository

REPO-IDP-KEYCLOAK

#### 1.5.2.3 Method Contracts

##### 1.5.2.3.1 Method Name

###### 1.5.2.3.1.1 Method Name

CreateUserAsync

###### 1.5.2.3.1.2 Method Signature

Task CreateUserAsync(UserRepresentation user)

###### 1.5.2.3.1.3 Method Purpose

Creates a new user principal within the Keycloak identity store.

###### 1.5.2.3.1.4 Integration Context

Called during the user creation workflow (US-047) to synchronize the new identity to the IdP.

##### 1.5.2.3.2.0 Method Name

###### 1.5.2.3.2.1 Method Name

AssignClientRoleAsync

###### 1.5.2.3.2.2 Method Signature

Task AssignClientRoleToUserAsync(string realm, string userId, string clientGuid, RoleRepresentation role)

###### 1.5.2.3.2.3 Method Purpose

Assigns an application-specific role to a user in Keycloak.

###### 1.5.2.3.2.4 Integration Context

Called when a user's roles are changed (US-048) to ensure the JWT claims are correct upon the user's next login.

##### 1.5.2.3.3.0 Method Name

###### 1.5.2.3.3.1 Method Name

ValidatePasswordAsync

###### 1.5.2.3.3.2 Method Signature

Task<bool> ValidatePasswordAsync(string username, string password)

###### 1.5.2.3.3.3 Method Purpose

Validates a user's password directly against Keycloak without issuing a token.

###### 1.5.2.3.3.4 Integration Context

Used during the electronic signature workflow (US-016) to re-authenticate the user for a critical action.

#### 1.5.2.4.0.0 Integration Pattern

Resilient REST API Client (with Polly)

#### 1.5.2.5.0.0 Communication Protocol

HTTPS/REST

#### 1.5.2.6.0.0 Extraction Reasoning

This is a critical external dependency for fulfilling the Externalized Authentication pattern. The service must interact with Keycloak's Admin API to keep user and role information synchronized.

### 1.5.3.0.0.0 Interface Name

#### 1.5.3.1.0.0 Interface Name

Asset Service Client (Internal gRPC)

#### 1.5.3.2.0.0 Source Repository

REPO-SVC-AST

#### 1.5.3.3.0.0 Method Contracts

- {'method_name': 'ValidateAssetExists', 'method_signature': 'rpc ValidateAssetExists(ValidateAssetRequest) returns (ValidateAssetResponse)', 'method_purpose': 'To confirm that a given asset ID exists within a specific tenant.', 'integration_context': 'Called during the role assignment process (US-049) when an administrator scopes a permission to a specific asset. This ensures that the asset ID is valid before saving the permission.'}

#### 1.5.3.4.0.0 Integration Pattern

gRPC Client

#### 1.5.3.5.0.0 Communication Protocol

gRPC

#### 1.5.3.6.0.0 Extraction Reasoning

This dependency is required to implement asset-scoped permissions (REQ-1-061). The IAM service needs a way to validate asset IDs against the authoritative Asset & Topology Service.

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

Task<IActionResult> CreateUserAsync([FromBody] UserCreateDto userDto)

###### 1.6.1.3.1.3 Method Purpose

Creates a new user account within the current administrator's tenant.

###### 1.6.1.3.1.4 Implementation Requirements

Must check license limits (e.g., max users) before creation and synchronize the new user to Keycloak.

##### 1.6.1.3.2.0 Method Name

###### 1.6.1.3.2.1 Method Name

PUT /api/v1/users/{id}/roles

###### 1.6.1.3.2.2 Method Signature

Task<IActionResult> AssignRolesToUserAsync(Guid id, [FromBody] RoleAssignmentDto rolesDto)

###### 1.6.1.3.2.3 Method Purpose

Assigns or updates the roles for a specific user, including asset-scoped permissions.

###### 1.6.1.3.2.4 Implementation Requirements

Must be an idempotent operation. Must synchronize role assignments to Keycloak. Must validate asset scope IDs against the Asset Service.

##### 1.6.1.3.3.0 Method Name

###### 1.6.1.3.3.1 Method Name

POST /api/v1/roles

###### 1.6.1.3.3.2 Method Signature

Task<IActionResult> CreateCustomRoleAsync([FromBody] RoleCreateDto roleDto)

###### 1.6.1.3.3.3 Method Purpose

Creates a new custom role with granular functional and asset-scoped permissions.

###### 1.6.1.3.3.4 Implementation Requirements

The new role is created in both the local database and Keycloak.

##### 1.6.1.3.4.0 Method Name

###### 1.6.1.3.4.1 Method Name

GET /api/v1/tenants/me/license

###### 1.6.1.3.4.2 Method Signature

Task<IActionResult> GetMyTenantLicenseAsync()

###### 1.6.1.3.4.3 Method Purpose

Retrieves the current license plan and usage metrics for the administrator's tenant.

###### 1.6.1.3.4.4 Implementation Requirements

Fetches license definition locally and aggregates usage counts from relevant services or its own data.

#### 1.6.1.4.0.0 Service Level Requirements

- P95 latency for all endpoints must be under 200ms.

#### 1.6.1.5.0.0 Implementation Constraints

- All endpoints must enforce tenant isolation, ensuring a user from Tenant A cannot access data for Tenant B.
- Input must be validated to prevent security vulnerabilities like injection attacks.

#### 1.6.1.6.0.0 Extraction Reasoning

This interface is the primary way the frontend (via the API Gateway) manages users, roles, and tenants, as required by numerous user stories (US-047, US-048, US-049, US-054).

### 1.6.2.0.0.0 Interface Name

#### 1.6.2.1.0.0 Interface Name

Internal Authorization gRPC Service

#### 1.6.2.2.0.0 Consumer Repositories

- REPO-SVC-AST
- REPO-SVC-DVM
- All other microservices

#### 1.6.2.3.0.0 Method Contracts

- {'method_name': 'CheckPermission', 'method_signature': 'rpc CheckPermission(PermissionCheckRequest) returns (PermissionCheckResponse)', 'method_purpose': 'Performs a high-performance authorization check to determine if a user has a specific permission, potentially for a given resource.', 'implementation_requirements': 'The implementation must be highly optimized and cached to avoid becoming a system-wide bottleneck. It will query the user-role-asset scope relationships.'}

#### 1.6.2.4.0.0 Service Level Requirements

- P95 latency must be under 50ms.

#### 1.6.2.5.0.0 Implementation Constraints

- This service is for internal, trusted communication only and should not be exposed via the API Gateway.

#### 1.6.2.6.0.0 Extraction Reasoning

This exposed gRPC service is the concrete implementation of the 'Authorization Authority' role described in the repository's 'sds'. It is a critical integration point for enforcing granular security across the entire microservices ecosystem.

## 1.7.0.0.0.0 Technology Context

### 1.7.1.0.0.0 Framework Requirements

Must be built using .NET 8, ASP.NET Core 8 for APIs, and Entity Framework Core 8 for data access against a PostgreSQL database.

### 1.7.2.0.0.0 Integration Technologies

- Npgsql.EntityFrameworkCore.PostgreSQL v8.0.4
- MediatR v12.3.0
- FluentValidation.AspNetCore v11.3.0
- A REST Client for the Keycloak Admin API
- Grpc.AspNetCore for the internal authorization service

### 1.7.3.0.0.0 Performance Constraints

The internal gRPC authorization check endpoint is performance-critical and must have a P95 latency under 50ms. This necessitates a robust caching strategy.

### 1.7.4.0.0.0 Security Requirements

All API endpoints must be protected with authorization policies. All Personally Identifiable Information (PII) must be encrypted at rest in the database. The service must integrate with Keycloak as an OIDC resource server and an admin client.

## 1.8.0.0.0.0 Extraction Validation

| Property | Value |
|----------|-------|
| Mapping Completeness Check | All repository connections identified in the archi... |
| Cross Reference Validation | The service's role, dependencies (PostgreSQL, Keyc... |
| Implementation Readiness Assessment | High. The context provides a specific technology s... |
| Quality Assurance Confirmation | The extracted context is internally consistent. Re... |

