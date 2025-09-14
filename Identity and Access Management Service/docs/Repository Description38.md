# 1 Id

REPO-SVC-IAM

# 2 Name

Identity and Access Management Service

# 3 Description

This microservice is the authoritative source for managing identities and access control within the system's multi-tenant architecture. It is responsible for the full lifecycle of tenants, users, roles, and permissions as outlined in REQ-1-011 and REQ-1-061. It handles tenant onboarding, data isolation model configuration (REQ-1-025), and manages flexible software licensing (REQ-1-063). While it integrates with Keycloak for the actual authentication and token issuance (REQ-1-080), this service manages the application-specific user profile data, role assignments, and granular permissions, including scoping access to specific parts of the asset hierarchy. All PII is handled according to GDPR and encrypted at rest (REQ-1-027).

# 4 Type

🔹 Microservice

# 5 Namespace

System.Services.IdentityAccess

# 6 Output Path

services/identity-access

# 7 Framework

ASP.NET Core v8.0

# 8 Language

C# 12

# 9 Technology

.NET v8.0, ASP.NET Core v8.0, EF Core v8.0.6, PostgreSQL

# 10 Thirdparty Libraries

- Npgsql.EntityFrameworkCore.PostgreSQL v8.0.4
- MediatR v12.3.0
- FluentValidation.AspNetCore v11.3.0

# 11 Layer Ids

- application
- domain

# 12 Dependencies

- REPO-DB-POSTGRES
- REPO-IDP-KEYCLOAK
- REPO-LIB-SHARED

# 13 Requirements

## 13.1 Requirement Id

### 13.1.1 Requirement Id

REQ-1-011

## 13.2.0 Requirement Id

### 13.2.1 Requirement Id

REQ-1-061

## 13.3.0 Requirement Id

### 13.3.1 Requirement Id

REQ-1-024

## 13.4.0 Requirement Id

### 13.4.1 Requirement Id

REQ-1-063

# 14.0.0 Generate Tests

✅ Yes

# 15.0.0 Generate Documentation

✅ Yes

# 16.0.0 Architecture Style

Microservices

# 17.0.0 Architecture Map

- iam-service-002

# 18.0.0 Components Map

- iam-service-002

# 19.0.0 Requirements Map

- REQ-USR-001
- REQ-BIZ-001
- REQ-CON-001
- REQ-CON-003
- REQ-BIZ-003

# 20.0.0 Dependency Contracts

## 20.1.0 Repo-Db-Postgres

### 20.1.1 Required Interfaces

- {'interface': 'EF Core DbContext (IAM)', 'methods': ['DbSet<Tenant>.FindAsync()', 'DbSet<User>.AddAsync()', 'DbContext.SaveChangesAsync()'], 'events': [], 'properties': ['DbSet<Tenant>', 'DbSet<User>', 'DbSet<Role>', 'DbSet<License>']}

### 20.1.2 Integration Pattern

Repository Pattern over Entity Framework Core

### 20.1.3 Communication Protocol

SQL over TCP/IP

## 20.2.0 Repo-Idp-Keycloak

### 20.2.1 Required Interfaces

- {'interface': 'Keycloak Admin API Client', 'methods': ['CreateUser()', 'GetUser()', 'AssignRole()'], 'events': [], 'properties': []}

### 20.2.2 Integration Pattern

REST API Client

### 20.2.3 Communication Protocol

HTTPS/REST

# 21.0.0 Exposed Contracts

## 21.1.0 Public Interfaces

### 21.1.1 Interface

#### 21.1.1.1 Interface

IUserService (REST API)

#### 21.1.1.2 Methods

- GET /api/v1/users/{id}
- POST /api/v1/users
- PUT /api/v1/users/{id}/roles

#### 21.1.1.3 Events

*No items available*

#### 21.1.1.4 Properties

*No items available*

#### 21.1.1.5 Consumers

- REPO-GW-API

### 21.1.2.0 Interface

#### 21.1.2.1 Interface

ITenantService (REST API)

#### 21.1.2.2 Methods

- GET /api/v1/tenants
- POST /api/v1/tenants

#### 21.1.2.3 Events

*No items available*

#### 21.1.2.4 Properties

*No items available*

#### 21.1.2.5 Consumers

- REPO-GW-API

# 22.0.0.0 Integration Patterns

| Property | Value |
|----------|-------|
| Dependency Injection | Constructor Injection via .NET's built-in DI conta... |
| Event Communication | N/A (Primarily synchronous request/response). |
| Data Flow | MediatR for implementing CQRS pattern internally. |
| Error Handling | Global exception handler middleware to return stan... |
| Async Patterns | Extensive use of async/await for all I/O bound ope... |

# 23.0.0.0 Scope Boundaries

## 23.1.0.0 Must Implement

- CRUD operations for Tenants, Users, Roles, and Licenses.
- Management of tenant-level settings like data residency and isolation model.
- Scoping of user roles to specific assets or areas.
- Synchronization of user state with Keycloak.
- Enforcement of license limits (e.g., max users).

## 23.2.0.0 Must Not Implement

- User password management or authentication (delegated to Keycloak).
- Management of any non-identity related entities like Assets or OPC Clients.
- Session management (handled by JWTs).

## 23.3.0.0 Integration Points

- PostgreSQL for persistence.
- Keycloak for identity federation.
- Consumed by API Gateway.

## 23.4.0.0 Architectural Constraints

- Must be stateless to support horizontal scaling.
- Must enforce tenant data isolation in all database queries.

# 24.0.0.0 Technology Standards

## 24.1.0.0 Framework Specific

| Property | Value |
|----------|-------|
| Pattern Usage | Use EF Core for data access with the Repository pa... |
| Performance Requirements | P95 latency under 200ms for all API endpoints (REQ... |
| Security Requirements | All endpoints must have authorization policies. PI... |

# 25.0.0.0 Cognitive Load Instructions

## 25.1.0.0 Sds Generation Guidance

### 25.1.1.0 Focus Areas

- Data models for Tenant, User, Role, License.
- API endpoints for managing these entities.
- Business logic for license enforcement and RBAC.

### 25.1.2.0 Avoid Patterns

- Implementing custom authentication logic.

## 25.2.0.0 Code Generation Guidance

### 25.2.1.0 Implementation Patterns

- Generate API controllers using ASP.NET Core minimal APIs or standard controllers.
- Use EF Core Migrations for schema management.

