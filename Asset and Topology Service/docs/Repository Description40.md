# 1 Id

REPO-SVC-AST

# 2 Name

Asset and Topology Service

# 3 Description

This microservice is the central authority for managing the physical and logical structure of the industrial environment. It is responsible for implementing the Asset Management module, allowing users to build an ISA-95 compatible hierarchical representation of their plant (e.g., Site > Area > Line > Machine) as per REQ-1-031 and REQ-CON-004. This service manages the creation of asset templates to speed up configuration (REQ-1-048) and, crucially, handles the mapping of raw OPC tags to specific assets and their properties (REQ-1-047). This contextualization of data is fundamental for all higher-level features, including analysis, AI model assignment, and AR visualization. The service will implement caching strategies for the asset hierarchy to ensure high-performance lookups.

# 4 Type

🔹 Microservice

# 5 Namespace

System.Services.AssetTopology

# 6 Output Path

services/asset-topology

# 7 Framework

ASP.NET Core v8.0

# 8 Language

C# 12

# 9 Technology

.NET v8.0, ASP.NET Core v8.0, EF Core v8.0.6, PostgreSQL, Redis

# 10 Thirdparty Libraries

- Npgsql.EntityFrameworkCore.PostgreSQL v8.0.4
- StackExchange.Redis v2.7.33

# 11 Layer Ids

- application
- domain

# 12 Dependencies

- REPO-DB-POSTGRES
- REPO-CACHE-REDIS
- REPO-LIB-SHARED

# 13 Requirements

## 13.1 Requirement Id

### 13.1.1 Requirement Id

REQ-1-031

## 13.2.0 Requirement Id

### 13.2.1 Requirement Id

REQ-1-047

## 13.3.0 Requirement Id

### 13.3.1 Requirement Id

REQ-1-048

# 14.0.0 Generate Tests

✅ Yes

# 15.0.0 Generate Documentation

✅ Yes

# 16.0.0 Architecture Style

Microservices

# 17.0.0 Architecture Map

- asset-service-005

# 18.0.0 Components Map

- asset-service-005

# 19.0.0 Requirements Map

- REQ-FR-021
- REQ-CON-004
- REQ-DM-001

# 20.0.0 Dependency Contracts

## 20.1.0 Repo-Db-Postgres

### 20.1.1 Required Interfaces

- {'interface': 'EF Core DbContext (Asset)', 'methods': ['DbSet<Asset>.FindAsync()', 'DbSet<OpcTag>.AddAsync()'], 'events': [], 'properties': ['DbSet<Asset>', 'DbSet<AssetTemplate>', 'DbSet<OpcTag>']}

### 20.1.2 Integration Pattern

Repository Pattern over EF Core

### 20.1.3 Communication Protocol

SQL over TCP/IP

## 20.2.0 Repo-Cache-Redis

### 20.2.1 Required Interfaces

- {'interface': 'IDatabase (Redis)', 'methods': ['StringGetAsync(RedisKey key)', 'StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry)'], 'events': [], 'properties': []}

### 20.2.2 Integration Pattern

Cache-Aside Pattern

### 20.2.3 Communication Protocol

Redis Protocol

# 21.0.0 Exposed Contracts

## 21.1.0 Public Interfaces

### 21.1.1 Interface

#### 21.1.1.1 Interface

IAssetService (REST API)

#### 21.1.1.2 Methods

- GET /api/v1/assets
- GET /api/v1/assets/{id}
- POST /api/v1/assets

#### 21.1.1.3 Events

*No items available*

#### 21.1.1.4 Properties

*No items available*

#### 21.1.1.5 Consumers

- REPO-GW-API

### 21.1.2.0 Interface

#### 21.1.2.1 Interface

ITagMappingService (REST API)

#### 21.1.2.2 Methods

- GET /api/v1/assets/{assetId}/tags
- POST /api/v1/tags

#### 21.1.2.3 Events

*No items available*

#### 21.1.2.4 Properties

*No items available*

#### 21.1.2.5 Consumers

- REPO-GW-API

# 22.0.0.0 Integration Patterns

| Property | Value |
|----------|-------|
| Dependency Injection | Constructor Injection via .NET's DI container. |
| Event Communication | Publishes cache invalidation events to a Redis cha... |
| Data Flow | REST API for configuration, with heavy use of Redi... |
| Error Handling | Global exception handler for standard error respon... |
| Async Patterns | async/await for all database and cache interaction... |

# 23.0.0.0 Scope Boundaries

## 23.1.0.0 Must Implement

- CRUD operations for Assets, Asset Templates, and OPC Tags.
- Management of the hierarchical relationship between assets.
- Business logic for asset template application.
- Caching of the asset hierarchy to optimize read performance.
- Validation to prevent circular dependencies in the hierarchy.

## 23.2.0.0 Must Not Implement

- Store or query any time-series data for tags.
- Manage OPC Client connections.
- Define user permissions related to assets (permissions are defined in IAM, just referenced here).

## 23.3.0.0 Integration Points

- PostgreSQL for persistence.
- Redis for caching.

## 23.4.0.0 Architectural Constraints

- Must be stateless.
- Asset hierarchy queries must be highly optimized to avoid performance bottlenecks.

# 24.0.0.0 Technology Standards

## 24.1.0.0 Framework Specific

| Property | Value |
|----------|-------|
| Pattern Usage | Implement recursive queries in PostgreSQL (using C... |
| Performance Requirements | P95 latency under 200ms. |
| Security Requirements | Enforce tenant isolation on all database and cache... |

# 25.0.0.0 Cognitive Load Instructions

## 25.1.0.0 Sds Generation Guidance

### 25.1.1.0 Focus Areas

- The self-referencing data model for the Asset hierarchy.
- The cache invalidation strategy for when the hierarchy changes.
- API design for navigating and modifying the asset tree.

### 25.1.2.0 Avoid Patterns

- Fetching the entire asset tree for every request; rely on caching and targeted queries.

## 25.2.0.0 Code Generation Guidance

### 25.2.1.0 Implementation Patterns

- Use EF Core to model the self-referencing foreign key on the Asset table.
- Use the StackExchange.Redis library for cache interactions.

