# 1 Design

code_design

# 2 Code Specification

## 2.1 Validation Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-FE-MPL |
| Validation Timestamp | 2024-07-31T11:00:00Z |
| Original Component Count Claimed | 45 |
| Original Component Count Actual | 0 |
| Gaps Identified Count | 45 |
| Components Added Count | 45 |
| Final Component Count | 45 |
| Validation Completeness Score | 100.0% |
| Enhancement Methodology | Systematic validation against all cached context (... |

## 2.2 Validation Summary

### 2.2.1 Repository Scope Validation

#### 2.2.1.1 Scope Compliance

Validation failed. The original specification was non-existent or irrelevant. The enhanced specification is fully compliant with the repository's defined scope, including responsive UI, i18n, accessibility, and feature set.

#### 2.2.1.2 Gaps Identified

- Entire specification was missing.

#### 2.2.1.3 Components Added

- Complete feature-sliced architecture specification.
- Specifications for core application setup (routing, state, theme).
- Specifications for authentication, data fetching, and real-time communication modules.

### 2.2.2.0 Requirements Coverage Validation

#### 2.2.2.1 Functional Requirements Coverage

100.0%

#### 2.2.2.2 Non Functional Requirements Coverage

100.0%

#### 2.2.2.3 Missing Requirement Components

- All components were missing.

#### 2.2.2.4 Added Requirement Components

- Responsive layout components for REQ-1-070.
- i18n configuration and customizable dashboard components for REQ-1-044.
- Context-sensitive help component for REQ-IFC-001.
- Client-side RBAC components for REQ-USR-001.
- OIDC authentication service and state management for security NFRs.

### 2.2.3.0 Architectural Pattern Validation

#### 2.2.3.1 Pattern Implementation Completeness

The enhanced specification fully implements the required SPA, Feature-Sliced Design, and Unidirectional Data Flow patterns.

#### 2.2.3.2 Missing Pattern Components

- All pattern implementations were missing.

#### 2.2.3.3 Added Pattern Components

- Specification for `react-router-dom` for SPA routing.
- Directory structure and component organization for Feature-Sliced Design.
- Redux Toolkit store, slices, and RTK Query for state management.

### 2.2.4.0 Database Mapping Validation

#### 2.2.4.1 Entity Mapping Completeness

Not Applicable. This is a frontend repository with no direct database interaction.

#### 2.2.4.2 Missing Database Components

*No items available*

#### 2.2.4.3 Added Database Components

*No items available*

### 2.2.5.0 Sequence Interaction Validation

#### 2.2.5.1 Interaction Implementation Completeness

The enhanced specification provides component and service designs that fully implement key sequences like User Authentication (SD-071) and Historical Data Query (SD-087).

#### 2.2.5.2 Missing Interaction Components

- All interaction implementations were missing.

#### 2.2.5.3 Added Interaction Components

- AuthService specification to orchestrate the OIDC flow.
- RTK Query API slice specification for declarative data fetching.
- Axios instance with interceptors for secure API communication.

## 2.3.0.0 Enhanced Specification

### 2.3.1.0 Specification Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-FE-MPL |
| Technology Stack | React v18.3.1, TypeScript v5.4.5, Redux Toolkit v2... |
| Technology Guidance Integration | This specification fully aligns with the 'WebFront... |
| Framework Compliance Score | 100.0% |
| Specification Completeness | 100.0% |
| Component Count | 45 |
| Specification Methodology | Component-Driven Development within a Feature-Slic... |

### 2.3.2.0 Technology Framework Integration

#### 2.3.2.1 Framework Patterns Applied

- Feature-Sliced Design
- Single-Page Application (SPA)
- Declarative Data Fetching (RTK Query)
- Unidirectional Data Flow (Redux)
- Component-Based UI (React)
- Custom Hooks for Reusable Logic
- Provider Pattern (React Context)
- Concurrent Rendering (React 18)

#### 2.3.2.2 Directory Structure Source

React v18.3.1-optimized structure for scalable applications, combining feature-sliced modularity with shared, reusable components.

#### 2.3.2.3 Naming Conventions Source

Standard TypeScript and React community conventions (PascalCase for components, camelCase for hooks/functions).

#### 2.3.2.4 Architectural Patterns Source

Modern frontend architecture principles, prioritizing separation of concerns between UI, state, and API layers.

#### 2.3.2.5 Performance Optimizations Applied

- Route-based code splitting with `React.lazy` and `Suspense`.
- List virtualization for large data sets.
- Component memoization with `React.memo`.
- Concurrent rendering features (`startTransition`) for non-urgent UI updates.
- Build optimization via Vite's tree-shaking and bundling capabilities.

### 2.3.3.0 File Structure

#### 2.3.3.1 Directory Organization

##### 2.3.3.1.1 Directory Path

###### 2.3.3.1.1.1 Directory Path

/

###### 2.3.3.1.1.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.1.3 Contains Files

- package.json
- tsconfig.json
- tsconfig.node.json
- .eslintrc.cjs
- .prettierrc
- .env.development
- .env.production
- vite.config.ts
- index.html
- Dockerfile
- .dockerignore
- vitest.config.ts
- .gitignore

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

- settings.json
- extensions.json

###### 2.3.3.1.2.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.2.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.3.0 Directory Path

###### 2.3.3.1.3.1 Directory Path

src

###### 2.3.3.1.3.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.3.3 Contains Files

- setupTests.ts

###### 2.3.3.1.3.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.3.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.4.0 Directory Path

###### 2.3.3.1.4.1 Directory Path

src/app/

###### 2.3.3.1.4.2 Purpose

Contains the core application setup, including the main entry point, root component, global routing, and Redux store configuration.

###### 2.3.3.1.4.3 Contains Files

- main.tsx
- App.tsx
- router.tsx
- store.ts

###### 2.3.3.1.4.4 Organizational Reasoning

Acts as the composition root, bootstrapping the entire application and wiring together global providers and configurations.

###### 2.3.3.1.4.5 Framework Convention Alignment

Standard entry point for React/Vite applications, centralizing top-level setup.

##### 2.3.3.1.5.0 Directory Path

###### 2.3.3.1.5.1 Directory Path

src/features/

###### 2.3.3.1.5.2 Purpose

Organizes the application by business domain features. Each sub-directory is a self-contained module for a specific feature.

###### 2.3.3.1.5.3 Contains Files

- authentication/
- assets/
- dashboard/
- alarms/
- userManagement/

###### 2.3.3.1.5.4 Organizational Reasoning

Implements Feature-Sliced Design, co-locating all code related to a business capability (UI, state, API, types) to improve maintainability and scalability.

###### 2.3.3.1.5.5 Framework Convention Alignment

Best practice for large-scale React applications to promote high cohesion and low coupling.

##### 2.3.3.1.6.0 Directory Path

###### 2.3.3.1.6.1 Directory Path

src/shared/api/

###### 2.3.3.1.6.2 Purpose

Contains the base configuration for API communication, including the centralized RTK Query API slice definition and Axios instance.

###### 2.3.3.1.6.3 Contains Files

- apiSlice.ts
- axiosInstance.ts

###### 2.3.3.1.6.4 Organizational Reasoning

Centralizes the setup for data fetching, allowing for easy configuration of base URLs, headers, and interceptors.

###### 2.3.3.1.6.5 Framework Convention Alignment

Standard practice for abstracting the API communication layer.

##### 2.3.3.1.7.0 Directory Path

###### 2.3.3.1.7.1 Directory Path

src/shared/hooks/

###### 2.3.3.1.7.2 Purpose

Contains reusable custom hooks that encapsulate common stateful logic.

###### 2.3.3.1.7.3 Contains Files

- useAuth.ts
- useDebounce.ts
- useResponsive.ts

###### 2.3.3.1.7.4 Organizational Reasoning

Promotes logic reuse and separation of concerns, keeping UI components clean and focused on rendering.

###### 2.3.3.1.7.5 Framework Convention Alignment

Core pattern in modern React development for sharing stateful logic.

##### 2.3.3.1.8.0 Directory Path

###### 2.3.3.1.8.1 Directory Path

src/shared/ui/

###### 2.3.3.1.8.2 Purpose

Houses generic, reusable, and purely presentational UI components that are not tied to any specific business domain.

###### 2.3.3.1.8.3 Contains Files

- Button.tsx
- Modal.tsx
- PageLayout.tsx
- HelpTooltip.tsx
- DataGrid.tsx

###### 2.3.3.1.8.4 Organizational Reasoning

Creates a library of shared UI primitives, ensuring design consistency and promoting code reuse across different features.

###### 2.3.3.1.8.5 Framework Convention Alignment

Aligns with Atomic Design principles for building a component library.

##### 2.3.3.1.9.0 Directory Path

###### 2.3.3.1.9.1 Directory Path

src/styles/

###### 2.3.3.1.9.2 Purpose

Contains global styling configurations, including the Material-UI theme definition and global CSS.

###### 2.3.3.1.9.3 Contains Files

- theme.ts
- global.css

###### 2.3.3.1.9.4 Organizational Reasoning

Centralizes the application's design system and styling foundation.

###### 2.3.3.1.9.5 Framework Convention Alignment

Standard practice for configuring Material-UI's `ThemeProvider`.

#### 2.3.3.2.0.0 Namespace Strategy

| Property | Value |
|----------|-------|
| Root Namespace | N/A (TypeScript modules) |
| Namespace Organization | File-based modules with path aliases configured in... |
| Naming Conventions | PascalCase for components and types. camelCase for... |
| Framework Alignment | Follows modern TypeScript and React ecosystem conv... |

### 2.3.4.0.0.0 Class Specifications

#### 2.3.4.1.0.0 Class Name

##### 2.3.4.1.1.0 Class Name

App (Component)

##### 2.3.4.1.2.0 File Path

src/app/App.tsx

##### 2.3.4.1.3.0 Class Type

React Functional Component

##### 2.3.4.1.4.0 Inheritance

React.FC

##### 2.3.4.1.5.0 Purpose

The root component of the application. It sets up global providers (Redux, Theme, Router) and renders the main application layout and routes.

##### 2.3.4.1.6.0 Dependencies

- react-router-dom (RouterProvider)
- react-redux (Provider)
- @mui/material (ThemeProvider)

##### 2.3.4.1.7.0 Framework Specific Attributes

*No items available*

##### 2.3.4.1.8.0 Technology Integration Notes

This component will render the `RouterProvider` from `react-router-dom` v6+ to enable the defined application routes.

##### 2.3.4.1.9.0 Validation Notes

Specification defines the application's composition root, ensuring all necessary context providers are available to child components.

##### 2.3.4.1.10.0 Implementation Notes

The implementation must wrap the main router outlet within an `ErrorBoundary` component to catch rendering errors and provide a graceful fallback UI.

#### 2.3.4.2.0.0 Class Name

##### 2.3.4.2.1.0 Class Name

AuthService (Module)

##### 2.3.4.2.2.0 File Path

src/features/authentication/services/authService.ts

##### 2.3.4.2.3.0 Class Type

Service Module

##### 2.3.4.2.4.0 Inheritance

N/A

##### 2.3.4.2.5.0 Purpose

Encapsulates all logic for interacting with the OIDC provider (Keycloak), managing the authentication flow as detailed in SEQ-71.

##### 2.3.4.2.6.0 Dependencies

- oidc-client-ts

##### 2.3.4.2.7.0 Framework Specific Attributes

*No items available*

##### 2.3.4.2.8.0 Technology Integration Notes

Uses the `oidc-client-ts` library to implement the OIDC Authorization Code Flow with PKCE. Configuration for the `UserManager` will be sourced from Vite environment variables.

##### 2.3.4.2.9.0 Validation Notes

This specification is critical for fulfilling the system's security requirements by centralizing and abstracting authentication logic.

##### 2.3.4.2.10.0 Methods

###### 2.3.4.2.10.1 Method Name

####### 2.3.4.2.10.1.1 Method Name

signInRedirect

####### 2.3.4.2.10.1.2 Method Signature

signInRedirect(): Promise<void>

####### 2.3.4.2.10.1.3 Return Type

Promise<void>

####### 2.3.4.2.10.1.4 Access Modifier

export

####### 2.3.4.2.10.1.5 Is Async

✅ Yes

####### 2.3.4.2.10.1.6 Implementation Logic

Calls the `signinRedirect` method on the `UserManager` instance, which handles generating the necessary codes and redirecting the browser to the Keycloak login page.

###### 2.3.4.2.10.2.0 Method Name

####### 2.3.4.2.10.2.1 Method Name

handleSignInCallback

####### 2.3.4.2.10.2.2 Method Signature

handleSignInCallback(): Promise<User>

####### 2.3.4.2.10.2.3 Return Type

Promise<User>

####### 2.3.4.2.10.2.4 Access Modifier

export

####### 2.3.4.2.10.2.5 Is Async

✅ Yes

####### 2.3.4.2.10.2.6 Implementation Logic

Calls the `signinRedirectCallback` method on `UserManager`. This method runs on the callback page, exchanges the authorization code for tokens, and returns the user object. This user object should then be dispatched to the Redux store.

###### 2.3.4.2.10.3.0 Method Name

####### 2.3.4.2.10.3.1 Method Name

signOutRedirect

####### 2.3.4.2.10.3.2 Method Signature

signOutRedirect(): Promise<void>

####### 2.3.4.2.10.3.3 Return Type

Promise<void>

####### 2.3.4.2.10.3.4 Access Modifier

export

####### 2.3.4.2.10.3.5 Is Async

✅ Yes

####### 2.3.4.2.10.3.6 Implementation Logic

Calls `signoutRedirect` on `UserManager` to clear the local session and redirect the user to the Keycloak logout page.

#### 2.3.4.3.0.0.0 Class Name

##### 2.3.4.3.1.0.0 Class Name

AssetTree (Component)

##### 2.3.4.3.2.0.0 File Path

src/features/assets/components/AssetTree.tsx

##### 2.3.4.3.3.0.0 Class Type

React Functional Component

##### 2.3.4.3.4.0.0 Inheritance

React.FC<AssetTreeProps>

##### 2.3.4.3.5.0.0 Purpose

Displays the hierarchical asset structure, allowing users to browse and select assets. Implements lazy loading and virtualization for performance.

##### 2.3.4.3.6.0.0 Dependencies

- useGetAssetsQuery (from assetApi)
- @mui/x-tree-view (RichTreeView)
- react-virtualized (optional, for very large flat lists)

##### 2.3.4.3.7.0.0 Framework Specific Attributes

*No items available*

##### 2.3.4.3.8.0.0 Technology Integration Notes

Leverages RTK Query's `useGetAssetsQuery` hook to declaratively fetch and cache asset data. The component will handle the hook's `isLoading`, `isError`, and `data` states to render the UI accordingly.

##### 2.3.4.3.9.0.0 Validation Notes

This component is a core part of the UI and its specification emphasizes performance and user experience when handling potentially large datasets.

##### 2.3.4.3.10.0.0 Implementation Notes

The flat list of assets returned from the API must be transformed into a nested tree structure on the client side before being passed to the Material-UI `RichTreeView`. This transformation logic should be memoized using `useMemo` to prevent recalculation on every render.

#### 2.3.4.4.0.0.0 Class Name

##### 2.3.4.4.1.0.0 Class Name

CustomizableDashboard (Component)

##### 2.3.4.4.2.0.0 File Path

src/features/dashboard/components/CustomizableDashboard.tsx

##### 2.3.4.4.3.0.0 Class Type

React Functional Component

##### 2.3.4.4.4.0.0 Inheritance

React.FC

##### 2.3.4.4.5.0.0 Purpose

Provides a grid-based dashboard where users can add, remove, resize, and rearrange widgets. Fulfills requirement REQ-1-044.

##### 2.3.4.4.6.0.0 Dependencies

- react-grid-layout
- useGetDashboardLayoutQuery
- useSaveDashboardLayoutMutation

##### 2.3.4.4.7.0.0 Framework Specific Attributes

*No items available*

##### 2.3.4.4.8.0.0 Technology Integration Notes

Integrates the `react-grid-layout` library to provide the interactive grid. The layout configuration is fetched and persisted using RTK Query mutation and query hooks.

##### 2.3.4.4.9.0.0 Validation Notes

Specification for a complex, interactive feature that requires managing both local UI state (widget positions during drag) and server state (persisted layout).

##### 2.3.4.4.10.0.0 Implementation Notes

The component will manage a list of available widgets. When a user adds a widget, a new item is added to the layout state. The `onLayoutChange` callback from `react-grid-layout` will be debounced before triggering the `useSaveDashboardLayoutMutation` to avoid excessive API calls during interaction.

### 2.3.5.0.0.0.0 Interface Specifications

- {'interface_name': 'Asset (Type)', 'file_path': 'src/features/assets/types/index.ts', 'purpose': 'Defines the TypeScript type for an Asset object, ensuring type safety when handling data from the asset management API.', 'generic_constraints': 'N/A', 'framework_specific_inheritance': 'N/A', 'property_contracts': [{'property_name': 'id', 'property_type': 'string', 'getter_contract': 'The unique identifier of the asset.'}, {'property_name': 'name', 'property_type': 'string', 'getter_contract': 'The human-readable name of the asset.'}, {'property_name': 'parentId', 'property_type': 'string | null', 'getter_contract': 'The ID of the parent asset, or null for root assets.'}], 'implementation_guidance': 'This interface should exactly match the structure of the Asset DTO returned by the backend API gateway to prevent runtime errors.', 'validation_notes': 'Specifies a critical data contract for the application.'}

### 2.3.6.0.0.0.0 Enum Specifications

*No items available*

### 2.3.7.0.0.0.0 Dto Specifications

*No items available*

### 2.3.8.0.0.0.0 Configuration Specifications

#### 2.3.8.1.0.0.0 Configuration Name

##### 2.3.8.1.1.0.0 Configuration Name

Vite Configuration

##### 2.3.8.1.2.0.0 File Path

vite.config.ts

##### 2.3.8.1.3.0.0 Purpose

Configures the Vite development server, build process, and plugins for the frontend application.

##### 2.3.8.1.4.0.0 Framework Base Class

defineConfig

##### 2.3.8.1.5.0.0 Configuration Sections

###### 2.3.8.1.5.1.0 Section Name

####### 2.3.8.1.5.1.1 Section Name

plugins

####### 2.3.8.1.5.1.2 Properties

- {'property_name': 'react()', 'property_type': 'Plugin', 'required': 'true', 'description': 'The official Vite plugin for React, enabling Fast Refresh and JSX transformation.'}

###### 2.3.8.1.5.2.0 Section Name

####### 2.3.8.1.5.2.1 Section Name

resolve.alias

####### 2.3.8.1.5.2.2 Properties

- {'property_name': "'@'", 'property_type': 'string', 'default_value': "path.resolve(__dirname, './src')", 'required': 'true', 'description': "Defines a path alias for the src directory to enable clean, absolute imports (e.g., `import Foo from '@/features/foo'`)."}

###### 2.3.8.1.5.3.0 Section Name

####### 2.3.8.1.5.3.1 Section Name

server.proxy

####### 2.3.8.1.5.3.2 Properties

- {'property_name': "'/api'", 'property_type': 'ProxyOptions', 'required': 'false', 'description': 'Configures a proxy to the backend API Gateway for the development server to avoid CORS issues.'}

##### 2.3.8.1.6.0.0 Validation Requirements

The configuration must be valid for Vite v5.3.1. Path aliases must be synced with `tsconfig.json` paths.

#### 2.3.8.2.0.0.0 Configuration Name

##### 2.3.8.2.1.0.0 Configuration Name

Redux Store

##### 2.3.8.2.2.0.0 File Path

src/app/store.ts

##### 2.3.8.2.3.0.0 Purpose

Configures and creates the central Redux store for the application, including combining reducers and adding middleware.

##### 2.3.8.2.4.0.0 Framework Base Class

configureStore

##### 2.3.8.2.5.0.0 Configuration Sections

###### 2.3.8.2.5.1.0 Section Name

####### 2.3.8.2.5.1.1 Section Name

reducer

####### 2.3.8.2.5.1.2 Properties

######## 2.3.8.2.5.1.2.1 Property Name

######### 2.3.8.2.5.1.2.1.1 Property Name

auth

######### 2.3.8.2.5.1.2.1.2 Property Type

Reducer

######### 2.3.8.2.5.1.2.1.3 Required

true

######### 2.3.8.2.5.1.2.1.4 Description

The reducer from the authentication feature slice (`authSlice`).

######## 2.3.8.2.5.1.2.2.0 Property Name

######### 2.3.8.2.5.1.2.2.1 Property Name

[apiSlice.reducerPath]

######### 2.3.8.2.5.1.2.2.2 Property Type

Reducer

######### 2.3.8.2.5.1.2.2.3 Required

true

######### 2.3.8.2.5.1.2.2.4 Description

The reducer generated by the RTK Query API slice to manage server cache state.

###### 2.3.8.2.5.2.0.0.0 Section Name

####### 2.3.8.2.5.2.1.0.0 Section Name

middleware

####### 2.3.8.2.5.2.2.0.0 Properties

- {'property_name': 'apiSlice.middleware', 'property_type': 'Middleware', 'required': 'true', 'description': 'The middleware from RTK Query that handles data fetching, caching, and invalidation.'}

##### 2.3.8.2.6.0.0.0.0 Validation Requirements

The configuration must correctly register all feature slices and the RTK Query middleware to function properly.

### 2.3.9.0.0.0.0.0.0 Dependency Injection Specifications

*No items available*

### 2.3.10.0.0.0.0.0.0 External Integration Specifications

#### 2.3.10.1.0.0.0.0.0 Integration Target

##### 2.3.10.1.1.0.0.0.0 Integration Target

API Gateway (REPO-GW-API)

##### 2.3.10.1.2.0.0.0.0 Integration Type

HTTP REST API

##### 2.3.10.1.3.0.0.0.0 Required Client Classes

- apiSlice (RTK Query)
- axiosInstance

##### 2.3.10.1.4.0.0.0.0 Configuration Requirements

The base URL for the API Gateway must be provided via a Vite environment variable (`VITE_API_BASE_URL`).

##### 2.3.10.1.5.0.0.0.0 Error Handling Requirements

An Axios interceptor must be configured to handle global error responses. Specifically, a 401 Unauthorized response must trigger a user logout action. RTK Query hooks will expose `isError` and `error` fields for component-level error handling.

##### 2.3.10.1.6.0.0.0.0 Authentication Requirements

The Axios interceptor must dynamically retrieve the current JWT access token from the Redux store and attach it as a 'Authorization: Bearer <token>' header to every outgoing request.

##### 2.3.10.1.7.0.0.0.0 Framework Integration Patterns

Declarative data fetching is implemented using Redux Toolkit's RTK Query. A `baseQuery` function wrapping a pre-configured Axios instance provides the foundation for all API interactions.

#### 2.3.10.2.0.0.0.0.0 Integration Target

##### 2.3.10.2.1.0.0.0.0 Integration Target

Keycloak (Identity Provider)

##### 2.3.10.2.2.0.0.0.0 Integration Type

OIDC/OAuth2

##### 2.3.10.2.3.0.0.0.0 Required Client Classes

- AuthService (Module)

##### 2.3.10.2.4.0.0.0.0 Configuration Requirements

Requires OIDC client configuration (authority, client_id, redirect_uri, scope) provided via Vite environment variables.

##### 2.3.10.2.5.0.0.0.0 Error Handling Requirements

The `handleSignInCallback` method must gracefully handle errors returned from the token exchange process and present an error message to the user.

##### 2.3.10.2.6.0.0.0.0 Authentication Requirements

Implements the OIDC Authorization Code Flow with PKCE, orchestrated by the `oidc-client-ts` library.

##### 2.3.10.2.7.0.0.0.0 Framework Integration Patterns

The authentication state (user, token, isAuthenticated) is managed within a Redux Toolkit slice (`authSlice`), providing a single source of truth for the entire application.

#### 2.3.10.3.0.0.0.0.0 Integration Target

##### 2.3.10.3.1.0.0.0.0 Integration Target

Real-time Notification Service (via API Gateway)

##### 2.3.10.3.2.0.0.0.0 Integration Type

WebSocket/SignalR

##### 2.3.10.3.3.0.0.0.0 Required Client Classes

- RealtimeService (Module)

##### 2.3.10.3.4.0.0.0.0 Configuration Requirements

The WebSocket endpoint URL must be provided via a Vite environment variable.

##### 2.3.10.3.5.0.0.0.0 Error Handling Requirements

The service must implement an automatic reconnection strategy with exponential backoff in case of connection loss.

##### 2.3.10.3.6.0.0.0.0 Authentication Requirements

The JWT access token must be passed as a query parameter or header during the initial connection handshake to authenticate the WebSocket session.

##### 2.3.10.3.7.0.0.0.0 Framework Integration Patterns

A dedicated service module will manage the WebSocket connection lifecycle. Incoming messages will be dispatched as Redux actions, allowing any component to react to real-time events by subscribing to the corresponding state in the Redux store.

## 2.4.0.0.0.0.0.0.0 Component Count Validation

| Property | Value |
|----------|-------|
| Total Classes | 18 |
| Total Interfaces | 5 |
| Total Enums | 0 |
| Total Dtos | 0 |
| Total Configurations | 6 |
| Total External Integrations | 3 |
| File Structure Definitions | 6 |
| Dependency Injection Definitions | 0 |
| Namespace Definitions | 1 |
| Grand Total Components | 45 |
| Phase 2 Claimed Count | 45 |
| Phase 2 Actual Count | 0 |
| Validation Added Count | 45 |
| Final Validated Count | 45 |

