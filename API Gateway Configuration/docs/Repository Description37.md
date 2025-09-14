# 1 Id

REPO-GW-API

# 2 Name

API Gateway Configuration

# 3 Description

This repository contains the declarative configuration for the Kong API Gateway, which serves as the single, secure entry point for all external traffic to the Central Management Plane. It is a critical coordination repository responsible for routing incoming REST API requests to the appropriate backend microservices. Its responsibilities include JWT validation against the IdP (Keycloak) as per REQ-1-080, applying rate limiting and throttling policies, request/response transformations, and aggregating microservice OpenAPI specifications into a unified, versioned API for consumers like the frontend. This decouples clients from the internal service architecture and provides a centralized point for applying cross-cutting security and traffic management policies (REQ-1-086).

# 4 Type

🔹 ApiGateway

# 5 Namespace

System.Gateway

# 6 Output Path

infra/api-gateway

# 7 Framework

Kong v3.7.0

# 8 Language

YAML

# 9 Technology

Kong v3.7.0 on Kubernetes, Kong Ingress Controller v3.2

# 10 Thirdparty Libraries

*No items available*

# 11 Layer Ids

- application

# 12 Dependencies

- REPO-SVC-IAM
- REPO-SVC-DVM
- REPO-SVC-AST
- REPO-SVC-DQR
- REPO-SVC-ANM
- REPO-SVC-AML
- REPO-SVC-ADT

# 13 Requirements

## 13.1 Requirement Id

### 13.1.1 Requirement Id

REQ-1-086

## 13.2.0 Requirement Id

### 13.2.1 Requirement Id

REQ-1-080

## 13.3.0 Requirement Id

### 13.3.1 Requirement Id

REQ-1-028

# 14.0.0 Generate Tests

❌ No

# 15.0.0 Generate Documentation

✅ Yes

# 16.0.0 Architecture Style

APIGateway

# 17.0.0 Architecture Map

- api-gateway-001

# 18.0.0 Components Map

- api-gateway-001

# 19.0.0 Requirements Map

- REQ-NFR-003
- REQ-NFR-006
- REQ-CON-001

# 20.0.0 Dependency Contracts

## 20.1.0 Repo-Svc-Iam

### 20.1.1 Required Interfaces

- {'interface': 'IAM Service REST API', 'methods': ['GET /users', 'POST /roles'], 'events': [], 'properties': []}

### 20.1.2 Integration Pattern

HTTP Proxy/Routing

### 20.1.3 Communication Protocol

HTTP (internal cluster DNS)

## 20.2.0 Repo-Svc-Ast

### 20.2.1 Required Interfaces

- {'interface': 'Asset Service REST API', 'methods': ['GET /assets', 'POST /tags'], 'events': [], 'properties': []}

### 20.2.2 Integration Pattern

HTTP Proxy/Routing

### 20.2.3 Communication Protocol

HTTP (internal cluster DNS)

# 21.0.0 Exposed Contracts

## 21.1.0 Public Interfaces

- {'interface': 'Central Management Plane REST API v1', 'methods': ['GET /api/v1/users', 'GET /api/v1/assets', 'POST /api/v1/clients/{id}/config'], 'events': [], 'properties': [], 'consumers': ['REPO-FE-MPL', 'External API Clients']}

# 22.0.0 Integration Patterns

| Property | Value |
|----------|-------|
| Dependency Injection | N/A |
| Event Communication | N/A |
| Data Flow | Request routing based on URI path. |
| Error Handling | Standardize error responses from backend services. |
| Async Patterns | N/A |

# 23.0.0 Scope Boundaries

## 23.1.0 Must Implement

- Route requests to appropriate backend services.
- Validate JWTs for every incoming request.
- Enforce rate limiting and throttling.
- Implement URI versioning (e.g., /api/v1/).
- Aggregate service-level Swagger/OpenAPI specs.

## 23.2.0 Must Not Implement

- Any business logic.
- User authentication (delegated to Keycloak).
- Service discovery (handled by Kubernetes).

## 23.3.0 Integration Points

- All public-facing backend microservices.
- Keycloak for JWT public key retrieval.

## 23.4.0 Architectural Constraints

- Must be deployed as the Ingress controller for the EKS cluster.

# 24.0.0 Technology Standards

## 24.1.0 Framework Specific

| Property | Value |
|----------|-------|
| Pattern Usage | Use Kong's declarative configuration (DB-less) wit... |
| Performance Requirements | Contribute less than 10ms of latency to the overal... |
| Security Requirements | All external traffic must be over TLS 1.3. JWT val... |

# 25.0.0 Cognitive Load Instructions

## 25.1.0 Sds Generation Guidance

### 25.1.1 Focus Areas

- Defining routing rules for each microservice.
- Specifying JWT validation policies.
- Documenting rate-limiting configurations.

### 25.1.2 Avoid Patterns

- Embedding business logic in Lua scripts.

## 25.2.0 Code Generation Guidance

### 25.2.1 Implementation Patterns

- Generate Kubernetes YAML files for KongIngress, KongPlugin, etc., from a structured configuration.

