# 1 Id

REPO-FE-MPL

# 2 Name

Frontend Management Plane

# 3 Description

A comprehensive web-based Single-Page Application (SPA) that serves as the centralized management and visualization plane for the entire system. This repository is responsible for delivering the user interface for all administrative, engineering, and operational tasks as defined in REQ-1-001 and REQ-1-070. It provides features such as multi-site client management, dashboard visualization, asset hierarchy configuration, alarm and event monitoring, user administration, and AI model lifecycle management. It is designed to be responsive, accessible (WCAG 2.1 AA), and supports internationalization (REQ-1-044). The application communicates exclusively with the backend via the API Gateway, ensuring a secure and decoupled architecture.

# 4 Type

🔹 WebFrontend

# 5 Namespace

System.Frontend.ManagementPlane

# 6 Output Path

apps/frontend-management-plane

# 7 Framework

React v18.3.1

# 8 Language

TypeScript v5.4.5

# 9 Technology

React v18.3.1, TypeScript v5.4.5, Redux Toolkit v2.2.5, Material-UI v5.15.20, Vite v5.3.1

# 10 Thirdparty Libraries

- axios v1.7.2
- oidc-client-ts v3.0.1
- react-router-dom v6.23.1
- vitest v1.6.0
- @testing-library/react v16.0.0

# 11 Layer Ids

- presentation

# 12 Dependencies

- REPO-GW-API

# 13 Requirements

## 13.1 Requirement Id

### 13.1.1 Requirement Id

REQ-1-001

## 13.2.0 Requirement Id

### 13.2.1 Requirement Id

REQ-1-070

## 13.3.0 Requirement Id

### 13.3.1 Requirement Id

REQ-1-044

# 14.0.0 Generate Tests

✅ Yes

# 15.0.0 Generate Documentation

✅ Yes

# 16.0.0 Architecture Style

MicroFrontends

# 17.0.0 Architecture Map

- frontend-spa-001

# 18.0.0 Components Map

- frontend-spa-001

# 19.0.0 Requirements Map

- REQ-SCP-001
- REQ-USR-001
- REQ-ENV-001
- REQ-FR-009
- REQ-IFC-001

# 20.0.0 Dependency Contracts

## 20.1.0 Repo-Gw-Api

### 20.1.1 Required Interfaces

- {'interface': 'Aggregated REST API (OpenAPI Specification)', 'methods': ['GET /api/v1/tenants', 'POST /api/v1/users', 'GET /api/v1/clients', 'GET /api/v1/assets', 'GET /api/v1/query/history', 'POST /api/v1/alarms/{id}/acknowledge'], 'events': [], 'properties': []}

### 20.1.2 Integration Pattern

Client-Side API Calls via Axios

### 20.1.3 Communication Protocol

HTTPS/REST

# 21.0.0 Exposed Contracts

## 21.1.0 Public Interfaces

- {'interface': 'Web User Interface', 'methods': [], 'events': [], 'properties': ['Dashboard Views', 'Configuration Forms', 'User Management Screens'], 'consumers': ['End Users (via Web Browser)']}

# 22.0.0 Integration Patterns

| Property | Value |
|----------|-------|
| Dependency Injection | N/A (Component-based) |
| Event Communication | SignalR for real-time UI updates (e.g., alarm noti... |
| Data Flow | Unidirectional data flow via Redux Toolkit |
| Error Handling | Display user-friendly error messages based on API ... |
| Async Patterns | React Query or RTK Query for managing asynchronous... |

# 23.0.0 Scope Boundaries

## 23.1.0 Must Implement

- All user-facing UI components.
- Client-side state management.
- Authentication flow handling with the IdP (Keycloak).
- Rendering of data fetched from the API Gateway.
- Support for light/dark themes and internationalization.

## 23.2.0 Must Not Implement

- Any business logic or data persistence.
- Direct communication with any backend service other than the API Gateway.
- User authentication; must delegate to Keycloak via OIDC.
- Direct database access.

## 23.3.0 Integration Points

- API Gateway for all data operations.
- Keycloak for user authentication.

## 23.4.0 Architectural Constraints

- Must be a Single-Page Application (SPA).
- Must adhere to WCAG 2.1 Level AA accessibility standards.

# 24.0.0 Technology Standards

## 24.1.0 Framework Specific

| Property | Value |
|----------|-------|
| Pattern Usage | Functional Components with Hooks, Redux Toolkit fo... |
| Performance Requirements | Initial dashboard load under 3 seconds (REQ-NFR-00... |
| Security Requirements | Store JWTs in memory, not local storage. Implement... |

# 25.0.0 Cognitive Load Instructions

## 25.1.0 Sds Generation Guidance

### 25.1.1 Focus Areas

- Component breakdown based on UI views (Dashboards, Asset Management, etc.).
- State management design for Redux slices.
- API interaction contracts with the gateway.

### 25.1.2 Avoid Patterns

- Mixing presentation logic with data fetching logic.

## 25.2.0 Code Generation Guidance

### 25.2.1 Implementation Patterns

- Generate typed API client layers from the OpenAPI specification.
- Use Material-UI components for UI consistency.

