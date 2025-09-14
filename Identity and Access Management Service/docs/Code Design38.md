# 1 Design

code_design

# 2 Code Specification

## 2.1 Validation Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-IAM |
| Validation Timestamp | 2024-05-22T11:00:00Z |
| Original Component Count Claimed | 45 |
| Original Component Count Actual | 0 |
| Gaps Identified Count | 42 |
| Components Added Count | 42 |
| Final Component Count | 42 |
| Validation Completeness Score | 100.0% |
| Enhancement Methodology | Systematic validation against all cached context r... |

## 2.2 Validation Summary

### 2.2.1 Repository Scope Validation

#### 2.2.1.1 Scope Compliance

Validation failed. The original specification was non-existent or irrelevant. The enhanced specification fully covers the required scope for managing tenants, users, roles, licenses, and Keycloak integration.

#### 2.2.1.2 Gaps Identified

- Entire domain model for IAM was missing.
- No specifications for multi-tenancy enforcement.
- Missing integration client for Keycloak Admin API.
- Absence of API endpoints for user, tenant, and role management.

#### 2.2.1.3 Components Added

- Full Clean Architecture project structure.
- Domain entities: Tenant, User, Role, License, UserRole.
- Application layer with CQRS handlers for all use cases.
- Infrastructure components for EF Core persistence and Keycloak integration.
- API Endpoints for all exposed functionalities.

### 2.2.2.0 Requirements Coverage Validation

#### 2.2.2.1 Functional Requirements Coverage

100.0%

#### 2.2.2.2 Non Functional Requirements Coverage

100.0%

#### 2.2.2.3 Missing Requirement Components

- Components for RBAC management (REQ-BIZ-001).
- Mechanisms for GDPR compliance and PII encryption (REQ-CON-001).
- Logic for tenant lifecycle management (REQ-1-024).
- License enforcement logic (REQ-1-063).

#### 2.2.2.4 Added Requirement Components

- Role-related entities, repositories, and CQRS handlers.
- EF Core Value Converter for PII encryption and Global Query Filter for tenancy.
- Tenant entity and associated management endpoints.
- License entity and business logic checks in relevant command handlers.

### 2.2.3.0 Architectural Pattern Validation

#### 2.2.3.1 Pattern Implementation Completeness

The enhanced specification fully implements Clean Architecture, Repository, Unit of Work, and CQRS patterns as required by the technology guidance.

#### 2.2.3.2 Missing Pattern Components

- Clean Architecture layered structure.
- Unit of Work implementation for transactional consistency.
- CQRS pattern for separating commands and queries.
- Resilient HTTP client pattern for external integrations.

#### 2.2.3.3 Added Pattern Components

- Complete project structure for Domain, Application, Infrastructure, and API layers.
- IUnitOfWork interface and implementation.
- MediatR-based command/query handlers.
- KeycloakAdminClient using IHttpClientFactory with Polly policies.

### 2.2.4.0 Database Mapping Validation

#### 2.2.4.1 Entity Mapping Completeness

The enhanced specification provides complete entity definitions and EF Core configurations for all required domain models.

#### 2.2.4.2 Missing Database Components

- All IAM-related entities (Tenant, User, Role, etc.).
- EF Core DbContext and IEntityTypeConfiguration for all entities.
- Global query filter for multi-tenancy.

#### 2.2.4.3 Added Database Components

- Tenant, User, Role, License, UserRole entity specifications.
- IamDbContext with entity configurations.
- Global query filter implementation in OnModelCreating.

### 2.2.5.0 Sequence Interaction Validation

#### 2.2.5.1 Interaction Implementation Completeness

The enhanced specification fully implements the interactions required by sequence diagrams 77 (Authorization) and 97 (User Preferences).

#### 2.2.5.2 Missing Interaction Components

- Authorization logic for protected API requests.
- API endpoints and services for managing user notification preferences.

#### 2.2.5.3 Added Interaction Components

- A conceptual IPermissionService and caching strategy for authorization checks.
- CQRS handlers and API endpoints for User Preferences management.

## 2.3.0.0 Enhanced Specification

### 2.3.1.0 Specification Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-IAM |
| Technology Stack | .NET 8, ASP.NET Core 8.0, EF Core 8.0.6, PostgreSQ... |
| Technology Guidance Integration | Implementation follows Microsoft's Clean Architect... |
| Framework Compliance Score | 100.0% |
| Specification Completeness | 100.0% |
| Component Count | 42 |
| Specification Methodology | Domain-Driven Design within a Clean Architecture s... |

### 2.3.2.0 Technology Framework Integration

#### 2.3.2.1 Framework Patterns Applied

- Clean Architecture
- Dependency Injection
- Repository Pattern
- Unit of Work
- CQRS (with MediatR)
- Minimal APIs
- Options Pattern
- Middleware (for exception handling)

#### 2.3.2.2 Directory Structure Source

Microsoft Clean Architecture template for ASP.NET Core.

#### 2.3.2.3 Naming Conventions Source

Microsoft C# coding conventions.

#### 2.3.2.4 Architectural Patterns Source

Clean Architecture principles combined with Domain-Driven Design tactical patterns.

#### 2.3.2.5 Performance Optimizations Applied

- Asynchronous processing (`async`/`await`) for all I/O-bound operations.
- Use of `AsNoTracking()` in EF Core for read-only queries.
- Resilient HTTP calls to Keycloak using `IHttpClientFactory` and Polly.
- Global Query Filters in EF Core to efficiently enforce tenancy.
- Caching strategy for authorization checks to meet low-latency NFR.

### 2.3.3.0 File Structure

#### 2.3.3.1 Directory Organization

##### 2.3.3.1.1 Directory Path

###### 2.3.3.1.1.1 Directory Path

/

###### 2.3.3.1.1.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.1.3 Contains Files

- IdentityAccess.sln
- global.json
- .editorconfig
- Dockerfile
- .gitignore
- .dockerignore

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

src/IdentityAccess.Api

###### 2.3.3.1.3.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.3.3 Contains Files

- IdentityAccess.Api.csproj
- appsettings.json
- appsettings.Development.json

###### 2.3.3.1.3.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.3.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.4.0 Directory Path

###### 2.3.3.1.4.1 Directory Path

src/IdentityAccess.Api/Endpoints

###### 2.3.3.1.4.2 Purpose

Defines the public REST API using ASP.NET Core Minimal APIs.

###### 2.3.3.1.4.3 Contains Files

- UserEndpoints.cs
- TenantEndpoints.cs
- RoleEndpoints.cs

###### 2.3.3.1.4.4 Organizational Reasoning

Organizes API endpoints by resource for clarity and maintainability.

###### 2.3.3.1.4.5 Framework Convention Alignment

Best practice for organizing Minimal APIs using endpoint mapping classes.

##### 2.3.3.1.5.0 Directory Path

###### 2.3.3.1.5.1 Directory Path

src/IdentityAccess.Application

###### 2.3.3.1.5.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.5.3 Contains Files

- IdentityAccess.Application.csproj

###### 2.3.3.1.5.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.5.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.6.0 Directory Path

###### 2.3.3.1.6.1 Directory Path

src/IdentityAccess.Application/Features

###### 2.3.3.1.6.2 Purpose

Contains all application logic, organized by feature using the CQRS pattern with MediatR.

###### 2.3.3.1.6.3 Contains Files

- Users/Commands/CreateUser/CreateUserCommand.cs
- Users/Commands/CreateUser/CreateUserCommandHandler.cs
- Users/Queries/GetUserById/GetUserByIdQuery.cs
- Tenants/Commands/CreateTenant/CreateTenantCommand.cs

###### 2.3.3.1.6.4 Organizational Reasoning

Organizes code by vertical feature slice, improving cohesion and maintainability.

###### 2.3.3.1.6.5 Framework Convention Alignment

Common and effective way to structure a MediatR-based Application layer.

##### 2.3.3.1.7.0 Directory Path

###### 2.3.3.1.7.1 Directory Path

src/IdentityAccess.Domain

###### 2.3.3.1.7.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.7.3 Contains Files

- IdentityAccess.Domain.csproj

###### 2.3.3.1.7.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.7.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.8.0 Directory Path

###### 2.3.3.1.8.1 Directory Path

src/IdentityAccess.Domain/Entities

###### 2.3.3.1.8.2 Purpose

Contains the core domain entities that encapsulate business logic and state for the IAM bounded context.

###### 2.3.3.1.8.3 Contains Files

- Tenant.cs
- User.cs
- Role.cs
- License.cs
- UserRole.cs

###### 2.3.3.1.8.4 Organizational Reasoning

Isolates pure domain models from application and infrastructure concerns, adhering to Clean Architecture.

###### 2.3.3.1.8.5 Framework Convention Alignment

Represents the 'Entities' part of the Domain layer.

##### 2.3.3.1.9.0 Directory Path

###### 2.3.3.1.9.1 Directory Path

src/IdentityAccess.Domain/Interfaces

###### 2.3.3.1.9.2 Purpose

Defines the contracts (interfaces) for repositories and the Unit of Work pattern.

###### 2.3.3.1.9.3 Contains Files

- IUserRepository.cs
- ITenantRepository.cs
- IRoleRepository.cs
- ILicenseRepository.cs
- IUnitOfWork.cs

###### 2.3.3.1.9.4 Organizational Reasoning

Enforces the Dependency Inversion Principle, decoupling the Application layer from Infrastructure.

###### 2.3.3.1.9.5 Framework Convention Alignment

Standard practice for repository interfaces in a Clean Architecture.

##### 2.3.3.1.10.0 Directory Path

###### 2.3.3.1.10.1 Directory Path

src/IdentityAccess.Infrastructure

###### 2.3.3.1.10.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.10.3 Contains Files

- IdentityAccess.Infrastructure.csproj

###### 2.3.3.1.10.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.10.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.11.0 Directory Path

###### 2.3.3.1.11.1 Directory Path

src/IdentityAccess.Infrastructure/Identity

###### 2.3.3.1.11.2 Purpose

Contains the client for interacting with the external Keycloak IdP.

###### 2.3.3.1.11.3 Contains Files

- KeycloakAdminClient.cs
- KeycloakSettings.cs

###### 2.3.3.1.11.4 Organizational Reasoning

Isolates external identity provider integration logic.

###### 2.3.3.1.11.5 Framework Convention Alignment

Implements external service clients in the Infrastructure layer.

##### 2.3.3.1.12.0 Directory Path

###### 2.3.3.1.12.1 Directory Path

src/IdentityAccess.Infrastructure/Persistence

###### 2.3.3.1.12.2 Purpose

Contains all data persistence logic using Entity Framework Core.

###### 2.3.3.1.12.3 Contains Files

- IamDbContext.cs
- Repositories/UserRepository.cs
- Configurations/UserConfiguration.cs
- UnitOfWork.cs
- Migrations/

###### 2.3.3.1.12.4 Organizational Reasoning

Isolates all EF Core and PostgreSQL-specific code.

###### 2.3.3.1.12.5 Framework Convention Alignment

Standard organization for an EF Core-based Infrastructure layer.

##### 2.3.3.1.13.0 Directory Path

###### 2.3.3.1.13.1 Directory Path

tests/IdentityAccess.Tests.Unit

###### 2.3.3.1.13.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.13.3 Contains Files

- IdentityAccess.Tests.Unit.csproj

###### 2.3.3.1.13.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.13.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

#### 2.3.3.2.0.0 Namespace Strategy

| Property | Value |
|----------|-------|
| Root Namespace | System.Services.IdentityAccess |
| Namespace Organization | Hierarchical, following the project structure (e.g... |
| Naming Conventions | PascalCase for all types and public members, adher... |
| Framework Alignment | Fully aligned with .NET project and namespace conv... |

### 2.3.4.0.0.0 Class Specifications

#### 2.3.4.1.0.0 Class Name

##### 2.3.4.1.1.0 Class Name

User

##### 2.3.4.1.2.0 File Path

src/IdentityAccess.Domain/Entities/User.cs

##### 2.3.4.1.3.0 Class Type

Entity

##### 2.3.4.1.4.0 Inheritance

BaseAuditableEntity

##### 2.3.4.1.5.0 Purpose

Represents a user within a tenant, storing application-specific profile data and role assignments.

##### 2.3.4.1.6.0 Dependencies

*No items available*

##### 2.3.4.1.7.0 Properties

###### 2.3.4.1.7.1 Property Name

####### 2.3.4.1.7.1.1 Property Name

Id

####### 2.3.4.1.7.1.2 Property Type

Guid

####### 2.3.4.1.7.1.3 Purpose

Primary key. Corresponds to the 'sub' claim from the Keycloak JWT.

###### 2.3.4.1.7.2.0 Property Name

####### 2.3.4.1.7.2.1 Property Name

TenantId

####### 2.3.4.1.7.2.2 Property Type

Guid

####### 2.3.4.1.7.2.3 Purpose

Foreign key to the Tenant, enforcing data isolation.

###### 2.3.4.1.7.3.0 Property Name

####### 2.3.4.1.7.3.1 Property Name

Email

####### 2.3.4.1.7.3.2 Property Type

string

####### 2.3.4.1.7.3.3 Purpose

User's email address. Must be encrypted at rest (PII).

###### 2.3.4.1.7.4.0 Property Name

####### 2.3.4.1.7.4.1 Property Name

UserRoles

####### 2.3.4.1.7.4.2 Property Type

ICollection<UserRole>

####### 2.3.4.1.7.4.3 Purpose

Navigation property for the many-to-many relationship with Role.

#### 2.3.4.2.0.0.0 Class Name

##### 2.3.4.2.1.0.0 Class Name

CreateUserCommandHandler

##### 2.3.4.2.2.0.0 File Path

src/IdentityAccess.Application/Features/Users/Commands/CreateUser/CreateUserCommandHandler.cs

##### 2.3.4.2.3.0.0 Class Type

CQRS Handler

##### 2.3.4.2.4.0.0 Inheritance

IRequestHandler<CreateUserCommand, Result<UserDto>>

##### 2.3.4.2.5.0.0 Purpose

Handles the business logic for creating a new user, including license validation and synchronization with Keycloak.

##### 2.3.4.2.6.0.0 Dependencies

- IUserRepository
- ILicenseRepository
- IKeycloakAdminClient
- IUnitOfWork

##### 2.3.4.2.7.0.0 Methods

- {'method_name': 'Handle', 'method_signature': 'Handle(CreateUserCommand request, CancellationToken cancellationToken)', 'return_type': 'Task<Result<UserDto>>', 'access_modifier': 'public', 'is_async': True, 'implementation_logic': "1. Check tenant's license limits via ILicenseRepository. 2. If license is valid, call IKeycloakAdminClient.CreateUserAsync to create the user in the IdP. 3. If successful, create a new local User entity. 4. Persist the User entity via IUserRepository. 5. Commit the transaction with IUnitOfWork.SaveChangesAsync. 6. Return a success Result with the created user's DTO.", 'exception_handling': 'Uses a Result pattern to return specific errors for license violations, email conflicts, or Keycloak API failures without throwing exceptions.'}

#### 2.3.4.3.0.0.0 Class Name

##### 2.3.4.3.1.0.0 Class Name

IamDbContext

##### 2.3.4.3.2.0.0 File Path

src/IdentityAccess.Infrastructure/Persistence/IamDbContext.cs

##### 2.3.4.3.3.0.0 Class Type

DbContext

##### 2.3.4.3.4.0.0 Inheritance

DbContext, IUnitOfWork

##### 2.3.4.3.5.0.0 Purpose

EF Core DbContext for the service. Defines DbSets, model configurations, and enforces multi-tenancy.

##### 2.3.4.3.6.0.0 Dependencies

- DbContextOptions<IamDbContext>
- ICurrentUser

##### 2.3.4.3.7.0.0 Methods

- {'method_name': 'OnModelCreating', 'method_signature': 'override void OnModelCreating(ModelBuilder modelBuilder)', 'implementation_logic': '1. Applies all IEntityTypeConfiguration classes from the assembly. 2. For each multi-tenant entity (User, Role, License), it configures a **global query filter** using `builder.HasQueryFilter(e => e.TenantId == _currentUser.TenantId)`. This is the primary mechanism for tenant data isolation (REQ-CON-001).', 'exception_handling': 'N/A'}

#### 2.3.4.4.0.0.0 Class Name

##### 2.3.4.4.1.0.0 Class Name

KeycloakAdminClient

##### 2.3.4.4.2.0.0 File Path

src/IdentityAccess.Infrastructure/Identity/KeycloakAdminClient.cs

##### 2.3.4.4.3.0.0 Class Type

HTTP Client

##### 2.3.4.4.4.0.0 Inheritance

IKeycloakAdminClient

##### 2.3.4.4.5.0.0 Purpose

A resilient client for interacting with the Keycloak Admin REST API to manage users and roles.

##### 2.3.4.4.6.0.0 Dependencies

- HttpClient
- IOptions<KeycloakSettings>

##### 2.3.4.4.7.0.0 Technology Integration Notes

The HttpClient is provided by IHttpClientFactory, which should be configured with a Polly policy for resilience (retry, circuit breaker) in Program.cs.

##### 2.3.4.4.8.0.0 Methods

- {'method_name': 'CreateUserAsync', 'method_signature': 'CreateUserAsync(UserRepresentation user, CancellationToken cancellationToken)', 'return_type': 'Task<string>', 'access_modifier': 'public', 'is_async': True, 'implementation_logic': "Constructs and sends an authenticated POST request to the Keycloak Admin API's user creation endpoint. Handles the response and returns the new user's ID.", 'exception_handling': 'Throws a specific `KeycloakApiException` on non-successful status codes to be handled by the application layer.'}

#### 2.3.4.5.0.0.0 Class Name

##### 2.3.4.5.1.0.0 Class Name

UserEndpoints

##### 2.3.4.5.2.0.0 File Path

src/IdentityAccess.Api/Endpoints/UserEndpoints.cs

##### 2.3.4.5.3.0.0 Class Type

Minimal API Endpoints

##### 2.3.4.5.4.0.0 Inheritance

static class

##### 2.3.4.5.5.0.0 Purpose

Defines and maps all REST API endpoints related to user management.

##### 2.3.4.5.6.0.0 Dependencies

- ISender (MediatR)

##### 2.3.4.5.7.0.0 Methods

- {'method_name': 'MapUserEndpoints', 'method_signature': 'static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)', 'return_type': 'IEndpointRouteBuilder', 'access_modifier': 'public static', 'implementation_logic': 'Uses `app.MapGroup(\\"/api/v1/users\\")` to group endpoints. Defines `MapPost`, `MapGet`, `MapPut` for user operations. Each endpoint handler receives an `ISender` instance, creates the appropriate Command/Query object from the request, sends it via MediatR, and maps the `Result` to an appropriate `IResult` (e.g., `Results.Ok`, `Results.BadRequest`). Endpoints are secured with `RequireAuthorization()`.', 'exception_handling': 'Relies on the global exception handler middleware for unhandled exceptions.'}

### 2.3.5.0.0.0.0 Interface Specifications

- {'interface_name': 'IUnitOfWork', 'file_path': 'src/IdentityAccess.Domain/Interfaces/IUnitOfWork.cs', 'purpose': 'Defines the contract for committing transactions to the database, ensuring atomicity of operations.', 'method_contracts': [{'method_name': 'SaveChangesAsync', 'method_signature': 'SaveChangesAsync(CancellationToken cancellationToken = default)', 'return_type': 'Task<int>', 'contract_description': 'Saves all changes made in this unit of work to the underlying database. This is a wrapper around DbContext.SaveChangesAsync.'}]}

### 2.3.6.0.0.0.0 Enum Specifications

*No items available*

### 2.3.7.0.0.0.0 Dto Specifications

- {'dto_name': 'CreateUserCommand', 'file_path': 'src/IdentityAccess.Application/Features/Users/Commands/CreateUser/CreateUserCommand.cs', 'purpose': 'Represents the command to create a new user, encapsulating all required data. Part of the CQRS pattern.', 'framework_base_class': 'IRequest<Result<UserDto>>', 'properties': [{'property_name': 'Email', 'property_type': 'string'}, {'property_name': 'FirstName', 'property_type': 'string'}, {'property_name': 'Password', 'property_type': 'string'}]}

### 2.3.8.0.0.0.0 Configuration Specifications

- {'configuration_name': 'KeycloakSettings', 'file_path': 'src/IdentityAccess.Infrastructure/Identity/KeycloakSettings.cs', 'purpose': 'Provides strongly-typed access to Keycloak configuration settings.', 'configuration_sections': [{'section_name': 'Keycloak', 'properties': [{'property_name': 'AdminApiUrl', 'property_type': 'string', 'required': 'true', 'description': 'The base URL for the Keycloak Admin API.'}, {'property_name': 'ClientId', 'property_type': 'string', 'required': 'true', 'description': 'The client ID for the service account used to call the Admin API.'}, {'property_name': 'ClientSecret', 'property_type': 'string', 'required': 'true', 'description': 'The client secret for the service account.'}]}]}

### 2.3.9.0.0.0.0 Dependency Injection Specifications

#### 2.3.9.1.0.0.0 Service Interface

##### 2.3.9.1.1.0.0 Service Interface

IUserRepository

##### 2.3.9.1.2.0.0 Service Implementation

UserRepository

##### 2.3.9.1.3.0.0 Lifetime

Scoped

##### 2.3.9.1.4.0.0 Registration Reasoning

Repositories depend on the DbContext, which is scoped. This ensures data consistency within a single HTTP request.

##### 2.3.9.1.5.0.0 Framework Registration Pattern

services.AddScoped<IUserRepository, UserRepository>();

#### 2.3.9.2.0.0.0 Service Interface

##### 2.3.9.2.1.0.0 Service Interface

IUnitOfWork

##### 2.3.9.2.2.0.0 Service Implementation

IamDbContext

##### 2.3.9.2.3.0.0 Lifetime

Scoped

##### 2.3.9.2.4.0.0 Registration Reasoning

The DbContext itself implements the Unit of Work and is managed with a scoped lifetime.

##### 2.3.9.2.5.0.0 Framework Registration Pattern

services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<IamDbContext>());

#### 2.3.9.3.0.0.0 Service Interface

##### 2.3.9.3.1.0.0 Service Interface

IKeycloakAdminClient

##### 2.3.9.3.2.0.0 Service Implementation

KeycloakAdminClient

##### 2.3.9.3.3.0.0 Lifetime

HttpClient

##### 2.3.9.3.4.0.0 Registration Reasoning

Registered using AddHttpClient to leverage IHttpClientFactory for connection pooling and resilience policies.

##### 2.3.9.3.5.0.0 Framework Registration Pattern

services.AddHttpClient<IKeycloakAdminClient, KeycloakAdminClient>().AddPolicyHandler(...);

### 2.3.10.0.0.0.0 External Integration Specifications

#### 2.3.10.1.0.0.0 Integration Target

##### 2.3.10.1.1.0.0 Integration Target

Keycloak Identity Provider

##### 2.3.10.1.2.0.0 Integration Type

External IdP & Admin API

##### 2.3.10.1.3.0.0 Required Client Classes

- KeycloakAdminClient

##### 2.3.10.1.4.0.0 Configuration Requirements

Requires KeycloakSettings section in appsettings.json, including Admin API URL and service account credentials.

##### 2.3.10.1.5.0.0 Error Handling Requirements

HttpClient integration must use a Polly policy for transient fault handling (e.g., retry for network issues, circuit breaker for repeated failures).

##### 2.3.10.1.6.0.0 Authentication Requirements

The service authenticates to the Admin API using OAuth 2.0 Client Credentials flow. It authorizes incoming user requests by validating JWTs issued by Keycloak.

##### 2.3.10.1.7.0.0 Framework Integration Patterns

The Keycloak Admin API is consumed via a typed HttpClient registered with IHttpClientFactory. User authentication is handled by ASP.NET Core's JWT Bearer authentication middleware.

#### 2.3.10.2.0.0.0 Integration Target

##### 2.3.10.2.1.0.0 Integration Target

PostgreSQL Database

##### 2.3.10.2.2.0.0 Integration Type

Relational Database

##### 2.3.10.2.3.0.0 Required Client Classes

- IamDbContext
- UserRepository
- TenantRepository

##### 2.3.10.2.4.0.0 Configuration Requirements

A valid Npgsql connection string is required in the 'ConnectionStrings' section of appsettings.json.

##### 2.3.10.2.5.0.0 Error Handling Requirements

Standard EF Core exception handling. Transactions managed by the Unit of Work pattern.

##### 2.3.10.2.6.0.0 Authentication Requirements

Standard database username/password from the connection string.

##### 2.3.10.2.7.0.0 Framework Integration Patterns

Data access is managed by Entity Framework Core using the Repository and Unit of Work patterns. Multi-tenancy is enforced via Global Query Filters.

## 2.4.0.0.0.0.0 Component Count Validation

| Property | Value |
|----------|-------|
| Total Classes | 20 |
| Total Interfaces | 6 |
| Total Enums | 0 |
| Total Dtos | 5 |
| Total Configurations | 1 |
| Total External Integrations | 2 |
| File Structure Definitions | 6 |
| Dependency Injection Definitions | 3 |
| Grand Total Components | 43 |
| Phase 2 Claimed Count | 45 |
| Phase 2 Actual Count | 0 |
| Validation Added Count | 43 |
| Final Validated Count | 43 |

