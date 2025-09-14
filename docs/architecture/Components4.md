# 1 Components

## 1.1 Components

### 1.1.1 Frontend

#### 1.1.1.1 Id

frontend-spa-001

#### 1.1.1.2 Name

Central Management Plane UI

#### 1.1.1.3 Description

A web-based Single-Page Application (SPA) providing the user interface for system administration, monitoring, and data visualization. (REQ-1-001, REQ-1-070)

#### 1.1.1.4 Type

🔹 Frontend

#### 1.1.1.5 Dependencies

- api-gateway-001

#### 1.1.1.6 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |
| Internationalization | English, German, Spanish |

#### 1.1.1.7 Interfaces

- Web Browser UI

#### 1.1.1.8 Technology

React 18, TypeScript, Axios

#### 1.1.1.9 Resources

| Property | Value |
|----------|-------|
| Cpu | N/A (Client-side) |
| Memory | N/A (Client-side) |
| Network | Standard HTTPS |

#### 1.1.1.10 Configuration

##### 1.1.1.10.1 Api Gateway Url

/api

##### 1.1.1.10.2 Theme

Light/Dark

#### 1.1.1.11.0 Health Check

*Not specified*

#### 1.1.1.12.0 Responsible Features

- Dashboard Visualization
- Asset Hierarchy Management
- Alarm Console
- User & Tenant Administration
- AI Model Management
- OPC Client Fleet Management
- Reporting Configuration

#### 1.1.1.13.0 Security

##### 1.1.1.13.1 Requires Authentication

✅ Yes

##### 1.1.1.13.2 Auth Method

OIDC via Keycloak

### 1.1.2.0.0 APIGateway

#### 1.1.2.1.0 Id

api-gateway-001

#### 1.1.2.2.0 Name

API Gateway

#### 1.1.2.3.0 Description

Manages all incoming API traffic, providing routing, JWT validation, and rate limiting for all backend microservices. (REQ-1-086)

#### 1.1.2.4.0 Type

🔹 APIGateway

#### 1.1.2.5.0 Dependencies

- iam-service-002
- tenant-service-003
- device-service-004
- asset-service-005
- aiml-service-006
- alarm-service-007
- query-service-008
- audit-service-009
- keycloak-idp-011

#### 1.1.2.6.0 Properties

| Property | Value |
|----------|-------|
| Provider | Kong |

#### 1.1.2.7.0 Interfaces

- REST API (aggregated OpenAPI spec)

#### 1.1.2.8.0 Technology

Kong on Kubernetes

#### 1.1.2.9.0 Resources

| Property | Value |
|----------|-------|
| Cpu | 1 vCPU |
| Memory | 1Gi |
| Network | High |

#### 1.1.2.10.0 Configuration

##### 1.1.2.10.1 Jwt Validation Plugin

enabled

##### 1.1.2.10.2 Rate Limiting Rps

1000

#### 1.1.2.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /status |
| Interval | 30 |
| Timeout | 5 |

#### 1.1.2.12.0 Responsible Features

- Secure API Exposure
- Service Routing
- Authentication Enforcement
- Rate Limiting

#### 1.1.2.13.0 Security

##### 1.1.2.13.1 Requires Authentication

✅ Yes

##### 1.1.2.13.2 Auth Method

JWT Bearer Token Validation

### 1.1.3.0.0 Service

#### 1.1.3.1.0 Id

iam-service-002

#### 1.1.3.2.0 Name

Identity & Access Management Service

#### 1.1.3.3.0 Description

Manages users, roles, and permissions scoped to tenants and asset hierarchies. Integrates with Keycloak for identity federation. (REQ-1-011, REQ-1-061)

#### 1.1.3.4.0 Type

🔹 Service

#### 1.1.3.5.0 Dependencies

- api-gateway-001
- keycloak-idp-011
- db-postgres-012

#### 1.1.3.6.0 Properties

| Property | Value |
|----------|-------|
| Domain | Security |

#### 1.1.3.7.0 Interfaces

- REST API (OpenAPI)

#### 1.1.3.8.0 Technology

.NET 8, ASP.NET Core

#### 1.1.3.9.0 Resources

##### 1.1.3.9.1 Cpu

0.5 vCPU

##### 1.1.3.9.2 Memory

512Mi

#### 1.1.3.10.0 Configuration

##### 1.1.3.10.1 Keycloak Realm Url

...

##### 1.1.3.10.2 Db Connection String

...

#### 1.1.3.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 10 |

#### 1.1.3.12.0 Responsible Features

- User Management
- Role-Based Access Control (RBAC)
- Permission Scoping

#### 1.1.3.13.0 Security

##### 1.1.3.13.1 Requires Authentication

✅ Yes

##### 1.1.3.13.2 Requires Authorization

✅ Yes

### 1.1.4.0.0 Service

#### 1.1.4.1.0 Id

tenant-service-003

#### 1.1.4.2.0 Name

Tenant Management Service

#### 1.1.4.3.0 Description

Manages tenant lifecycle, data isolation models, licensing, and data residency policies. (REQ-1-024, REQ-1-025, REQ-1-063)

#### 1.1.4.4.0 Type

🔹 Service

#### 1.1.4.5.0 Dependencies

- api-gateway-001
- db-postgres-012

#### 1.1.4.6.0 Properties

| Property | Value |
|----------|-------|
| Domain | Administration |

#### 1.1.4.7.0 Interfaces

- REST API (OpenAPI)

#### 1.1.4.8.0 Technology

.NET 8, ASP.NET Core

#### 1.1.4.9.0 Resources

##### 1.1.4.9.1 Cpu

0.5 vCPU

##### 1.1.4.9.2 Memory

512Mi

#### 1.1.4.10.0 Configuration

##### 1.1.4.10.1 Default Isolation Model

RLS

#### 1.1.4.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 10 |

#### 1.1.4.12.0 Responsible Features

- Tenant Onboarding
- License Management
- Data Residency Enforcement
- Multi-Tenancy Configuration

#### 1.1.4.13.0 Security

##### 1.1.4.13.1 Requires Authentication

✅ Yes

##### 1.1.4.13.2 Requires Authorization

✅ Yes

### 1.1.5.0.0 Service

#### 1.1.5.1.0 Id

device-service-004

#### 1.1.5.2.0 Name

Device Management Service

#### 1.1.5.3.0 Description

Manages the fleet of OPC Core Client instances, including secure provisioning, health monitoring, and remote software/configuration updates. (REQ-1-062, REQ-1-082)

#### 1.1.5.4.0 Type

🔹 Service

#### 1.1.5.5.0 Dependencies

- api-gateway-001
- mqtt-broker-014
- db-postgres-012

#### 1.1.5.6.0 Properties

| Property | Value |
|----------|-------|
| Domain | FleetManagement |

#### 1.1.5.7.0 Interfaces

- REST API (OpenAPI)
- MQTT Command/Control Topic

#### 1.1.5.8.0 Technology

.NET 8, ASP.NET Core

#### 1.1.5.9.0 Resources

##### 1.1.5.9.1 Cpu

1 vCPU

##### 1.1.5.9.2 Memory

1Gi

#### 1.1.5.10.0 Configuration

##### 1.1.5.10.1 Mqtt Broker Url

...

##### 1.1.5.10.2 Update Package Repo Url

...

#### 1.1.5.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 10 |

#### 1.1.5.12.0 Responsible Features

- OPC Client Provisioning
- Remote Health Monitoring
- Software Update Deployment
- Remote Configuration

#### 1.1.5.13.0 Security

##### 1.1.5.13.1 Requires Authentication

✅ Yes

##### 1.1.5.13.2 Requires Authorization

✅ Yes

### 1.1.6.0.0 Service

#### 1.1.6.1.0 Id

asset-service-005

#### 1.1.6.2.0 Name

Asset & Topology Service

#### 1.1.6.3.0 Description

Manages the ISA-95 compliant asset hierarchy, asset templates, and the mapping of OPC tags to specific assets. (REQ-1-031, REQ-1-046, REQ-1-047)

#### 1.1.6.4.0 Type

🔹 Service

#### 1.1.6.5.0 Dependencies

- api-gateway-001
- db-postgres-012
- cache-redis-013

#### 1.1.6.6.0 Properties

| Property | Value |
|----------|-------|
| Domain | Core |

#### 1.1.6.7.0 Interfaces

- REST API (OpenAPI)

#### 1.1.6.8.0 Technology

.NET 8, ASP.NET Core

#### 1.1.6.9.0 Resources

##### 1.1.6.9.1 Cpu

1 vCPU

##### 1.1.6.9.2 Memory

1Gi

#### 1.1.6.10.0 Configuration

##### 1.1.6.10.1 Cache Ttl

3600s

#### 1.1.6.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 10 |

#### 1.1.6.12.0 Responsible Features

- Asset Hierarchy Management (ISA-95)
- Asset Templating
- OPC Tag Mapping

#### 1.1.6.13.0 Security

##### 1.1.6.13.1 Requires Authentication

✅ Yes

##### 1.1.6.13.2 Requires Authorization

✅ Yes

### 1.1.7.0.0 Service

#### 1.1.7.1.0 Id

aiml-service-006

#### 1.1.7.2.0 Name

AI/ML Management Service

#### 1.1.7.3.0 Description

Manages the lifecycle of AI models, including import, versioning, approval workflows (MOC), and assignment to assets for edge deployment. (REQ-1-049, REQ-1-032)

#### 1.1.7.4.0 Type

🔹 Service

#### 1.1.7.5.0 Dependencies

- api-gateway-001
- db-postgres-012
- storage-s3-015

#### 1.1.7.6.0 Properties

| Property | Value |
|----------|-------|
| Domain | Analytics |

#### 1.1.7.7.0 Interfaces

- REST API (OpenAPI)

#### 1.1.7.8.0 Technology

.NET 8, ASP.NET Core

#### 1.1.7.9.0 Resources

##### 1.1.7.9.1 Cpu

0.5 vCPU

##### 1.1.7.9.2 Memory

512Mi

#### 1.1.7.10.0 Configuration

##### 1.1.7.10.1 Model Storage Bucket

ai-models-prod

#### 1.1.7.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 10 |

#### 1.1.7.12.0 Responsible Features

- AI Model Lifecycle Management
- Model Versioning & Approval
- Model Deployment to Edge

#### 1.1.7.13.0 Security

##### 1.1.7.13.1 Requires Authentication

✅ Yes

##### 1.1.7.13.2 Requires Authorization

✅ Yes

### 1.1.8.0.0 Service

#### 1.1.8.1.0 Id

alarm-service-007

#### 1.1.8.2.0 Name

Alarm & Notification Service

#### 1.1.8.3.0 Description

Processes alarms and events, manages alarm states (Acknowledge, Shelve), and routes notifications based on configurable rules. (REQ-1-035, REQ-1-037, REQ-1-092)

#### 1.1.8.4.0 Type

🔹 Service

#### 1.1.8.5.0 Dependencies

- api-gateway-001
- db-postgres-012

#### 1.1.8.6.0 Properties

| Property | Value |
|----------|-------|
| Domain | Operations |

#### 1.1.8.7.0 Interfaces

- REST API (OpenAPI)

#### 1.1.8.8.0 Technology

.NET 8, ASP.NET Core

#### 1.1.8.9.0 Resources

##### 1.1.8.9.1 Cpu

1 vCPU

##### 1.1.8.9.2 Memory

1Gi

#### 1.1.8.10.0 Configuration

| Property | Value |
|----------|-------|
| Smtp Server | ... |
| Sms Gateway Url | ... |
| Pager Duty Webhook Url | ... |

#### 1.1.8.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 10 |

#### 1.1.8.12.0 Responsible Features

- Alarm & Event Processing (A&C)
- Alarm State Management
- Configurable Notification Engine

#### 1.1.8.13.0 Security

##### 1.1.8.13.1 Requires Authentication

✅ Yes

##### 1.1.8.13.2 Requires Authorization

✅ Yes

### 1.1.9.0.0 Service

#### 1.1.9.1.0 Id

query-service-008

#### 1.1.9.2.0 Name

Query Service

#### 1.1.9.3.0 Description

Provides optimized REST APIs for querying historical time-series data (HDA), serving data for dashboards, and generating reports. (REQ-1-034, REQ-1-065)

#### 1.1.9.4.0 Type

🔹 Service

#### 1.1.9.5.0 Dependencies

- api-gateway-001
- db-timescaledb-012

#### 1.1.9.6.0 Properties

| Property | Value |
|----------|-------|
| Domain | Data |

#### 1.1.9.7.0 Interfaces

- REST API (OpenAPI)

#### 1.1.9.8.0 Technology

.NET 8, ASP.NET Core

#### 1.1.9.9.0 Resources

##### 1.1.9.9.1 Cpu

2 vCPU

##### 1.1.9.9.2 Memory

4Gi

#### 1.1.9.10.0 Configuration

##### 1.1.9.10.1 Max Query Range Days

365

##### 1.1.9.10.2 Default Aggregation Interval

1m

#### 1.1.9.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 10 |

#### 1.1.9.12.0 Responsible Features

- Historical Data Access (HDA)
- Dashboard Data Provisioning
- Automated Reporting Module

#### 1.1.9.13.0 Security

##### 1.1.9.13.1 Requires Authentication

✅ Yes

##### 1.1.9.13.2 Requires Authorization

✅ Yes

### 1.1.10.0.0 Service

#### 1.1.10.1.0 Id

audit-service-009

#### 1.1.10.2.0 Name

Audit Service

#### 1.1.10.3.0 Description

Manages the immutable audit trail by persisting significant system and user actions and anchoring them to a quantum ledger database for verification. (REQ-1-040, REQ-1-059)

#### 1.1.10.4.0 Type

🔹 Service

#### 1.1.10.5.0 Dependencies

- api-gateway-001
- db-postgres-012
- ledger-qldb-016

#### 1.1.10.6.0 Properties

| Property | Value |
|----------|-------|
| Domain | Compliance |

#### 1.1.10.7.0 Interfaces

- Internal gRPC Service
- REST API (for querying)

#### 1.1.10.8.0 Technology

.NET 8, ASP.NET Core

#### 1.1.10.9.0 Resources

##### 1.1.10.9.1 Cpu

0.5 vCPU

##### 1.1.10.9.2 Memory

512Mi

#### 1.1.10.10.0 Configuration

##### 1.1.10.10.1 Qldb Ledger Name

system-audit-prod

##### 1.1.10.10.2 Batch Interval Seconds

300

#### 1.1.10.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 10 |

#### 1.1.10.12.0 Responsible Features

- Immutable Audit Trail
- 21 CFR Part 11 Compliance
- Cryptographic Verification

#### 1.1.10.13.0 Security

##### 1.1.10.13.1 Requires Authentication

✅ Yes

##### 1.1.10.13.2 Requires Authorization

✅ Yes

### 1.1.11.0.0 Service

#### 1.1.11.1.0 Id

ingestion-service-010

#### 1.1.11.2.0 Name

Data Ingestion Service

#### 1.1.11.3.0 Description

High-throughput service that receives real-time data streams from OPC Core Clients via gRPC with mTLS and persists them to TimescaleDB. (REQ-1-010, REQ-1-075)

#### 1.1.11.4.0 Type

🔹 Service

#### 1.1.11.5.0 Dependencies

- db-timescaledb-012

#### 1.1.11.6.0 Properties

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| High Volume | true |

#### 1.1.11.7.0 Interfaces

- gRPC Service (TimeSeriesIngest.proto)

#### 1.1.11.8.0 Technology

.NET 8, gRPC

#### 1.1.11.9.0 Resources

##### 1.1.11.9.1 Cpu

2 vCPU

##### 1.1.11.9.2 Memory

4Gi

#### 1.1.11.10.0 Configuration

| Property | Value |
|----------|-------|
| Max Concurrent Streams | 1000 |
| Db Batch Size | 5000 |
| Ingestion Port | 50051 |

#### 1.1.11.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 30 |
| Timeout | 5 |

#### 1.1.11.12.0 Responsible Features

- Real-time Data Ingestion
- Historical Data Persistence

#### 1.1.11.13.0 Security

##### 1.1.11.13.1 Requires Authentication

✅ Yes

##### 1.1.11.13.2 Auth Method

mTLS

### 1.1.12.0.0 EdgeApplication

#### 1.1.12.1.0 Id

opc-core-client-011

#### 1.1.12.2.0 Name

OPC Core Client

#### 1.1.12.3.0 Description

A cross-platform, containerized edge application for industrial data acquisition, local processing, and secure communication with the Central Management Plane. (REQ-1-001, REQ-1-008)

#### 1.1.12.4.0 Type

🔹 EdgeApplication

#### 1.1.12.5.0 Dependencies

- ingestion-service-010
- device-service-004
- mqtt-broker-014

#### 1.1.12.6.0 Properties

| Property | Value |
|----------|-------|
| Deployment | On-Premise/Edge |
| Containerized | true |

#### 1.1.12.7.0 Interfaces

- OPC DA/UA/XML-DA Client
- gRPC Client
- MQTT Client

#### 1.1.12.8.0 Technology

.NET 8, Docker, OPCFoundation.NetStandard.Opc.Ua

#### 1.1.12.9.0 Resources

| Property | Value |
|----------|-------|
| Cpu | x86-64 (IPC) |
| Memory | 1GB |
| Storage | 10GB (for buffering) |

#### 1.1.12.10.0 Configuration

| Property | Value |
|----------|-------|
| Central Management Plane Url | ... |
| Registration Token | ... |
| Buffer Size Gb | 1 |

#### 1.1.12.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 10 |

#### 1.1.12.12.0 Responsible Features

- OPC Data Collection
- Autonomous Operation
- Offline Data Buffering
- Edge AI Inference

#### 1.1.12.13.0 Security

##### 1.1.12.13.1 Provisioning Method

Token-based with CSR

##### 1.1.12.13.2 Runtime Auth

mTLS for gRPC, TLS for MQTT

### 1.1.13.0.0 IdentityProvider

#### 1.1.13.1.0 Id

keycloak-idp-011

#### 1.1.13.2.0 Name

Keycloak Identity Provider

#### 1.1.13.3.0 Description

Centralized Identity Provider (IdP) for user authentication and authorization token issuance, supporting OAuth 2.0 and OpenID Connect (OIDC). (REQ-1-080)

#### 1.1.13.4.0 Type

🔹 IdentityProvider

#### 1.1.13.5.0 Dependencies

*No items available*

#### 1.1.13.6.0 Properties

| Property | Value |
|----------|-------|
| Cots | true |

#### 1.1.13.7.0 Interfaces

- OIDC/.well-known endpoints
- OAuth 2.0 Token Endpoint

#### 1.1.13.8.0 Technology

Keycloak

#### 1.1.13.9.0 Resources

##### 1.1.13.9.1 Cpu

1 vCPU

##### 1.1.13.9.2 Memory

2Gi

#### 1.1.13.10.0 Configuration

##### 1.1.13.10.1 Realm Name

prod-realm

##### 1.1.13.10.2 Token Lifespan

300s

#### 1.1.13.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /health/ready |
| Interval | 30 |
| Timeout | 5 |

#### 1.1.13.12.0 Responsible Features

- User Authentication
- JWT Issuance
- Single Sign-On (SSO)

#### 1.1.13.13.0 Security

##### 1.1.13.13.1 Protocol

OAuth 2.0 / OIDC

## 1.2.0.0.0 Configuration

### 1.2.1.0.0 Environment

production

### 1.2.2.0.0 Logging Level

INFO

### 1.2.3.0.0 Observability Stack

| Property | Value |
|----------|-------|
| Prometheus Url | http://prometheus-server |
| Opensearch Url | http://opensearch-cluster |
| Grafana Url | http://grafana |

### 1.2.4.0.0 Cloud Provider

AWS

# 2.0.0.0.0 Component Relations

## 2.1.0.0.0 Architecture

### 2.1.1.0.0 Components

#### 2.1.1.1.0 UI

##### 2.1.1.1.1 Id

spa-frontend-001

##### 2.1.1.1.2 Name

Central Management Plane UI

##### 2.1.1.1.3 Description

A web-based Single-Page Application (SPA) that serves as the primary user interface for administration, configuration, and data visualization. (REQ-1-001, REQ-1-070)

##### 2.1.1.1.4 Type

🔹 UI

##### 2.1.1.1.5 Dependencies

- api-gateway-001

##### 2.1.1.1.6 Properties

| Property | Value |
|----------|-------|
| Framework Version | React 18 |
| Internationalization | i18next |
| Supported Languages | ['en', 'de', 'es'] |

##### 2.1.1.1.7 Interfaces

- Web Browser Interface (HTTPS)

##### 2.1.1.1.8 Technology

React, TypeScript

##### 2.1.1.1.9 Resources

| Property | Value |
|----------|-------|
| Cpu | N/A (Client-Side) |
| Memory | N/A (Client-Side) |
| Network | Standard HTTPS |

##### 2.1.1.1.10 Configuration

###### 2.1.1.1.10.1 Api Endpoint

URL of API Gateway

###### 2.1.1.1.10.2 Auth Provider Endpoint

URL of Keycloak

##### 2.1.1.1.11.0 Health Check

*Not specified*

##### 2.1.1.1.12.0 Responsible Features

- User Interface
- Dashboard Visualization (REQ-1-044)
- Asset Management UI (REQ-1-046)
- Alarm Console (REQ-1-035)
- Remote Client Management (REQ-1-005)

##### 2.1.1.1.13.0 Security

###### 2.1.1.1.13.1 Requires Authentication

✅ Yes

###### 2.1.1.1.13.2 Requires Authorization

✅ Yes

###### 2.1.1.1.13.3 Allowed Roles

- Administrator
- Data Scientist
- Engineer
- Operator
- Viewer

#### 2.1.1.2.0.0 Gateway

##### 2.1.1.2.1.0 Id

api-gateway-001

##### 2.1.1.2.2.0 Name

Kong API Gateway

##### 2.1.1.2.3.0 Description

Manages all incoming API traffic from external clients, providing routing, security (JWT validation), and traffic control. (REQ-1-086, REQ-1-080)

##### 2.1.1.2.4.0 Type

🔹 Gateway

##### 2.1.1.2.5.0 Dependencies

- iam-service-001
- tenant-service-002
- device-service-003
- asset-service-004
- query-service-005
- aiml-service-006
- alarm-service-007
- audit-service-008

##### 2.1.1.2.6.0 Properties

| Property | Value |
|----------|-------|
| Version | Latest Stable |

##### 2.1.1.2.7.0 Interfaces

- REST API (e.g., /api/v1/...)

##### 2.1.1.2.8.0 Technology

Kong on Kubernetes

##### 2.1.1.2.9.0 Resources

| Property | Value |
|----------|-------|
| Cpu | 2 cores |
| Memory | 4GB |
| Network | 1Gbps |

##### 2.1.1.2.10.0 Configuration

| Property | Value |
|----------|-------|
| Jwt Plugin Config | Validation against Keycloak's public keys |
| Rate Limiting | Configurable per tenant/route |
| Routing Rules | Path-based routing to microservices |

##### 2.1.1.2.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /status |
| Interval | 30 |
| Timeout | 5 |

##### 2.1.1.2.12.0 Responsible Features

- API Security
- API Routing
- API Versioning (REQ-1-028)

##### 2.1.1.2.13.0 Security

###### 2.1.1.2.13.1 Requires Authentication

✅ Yes

###### 2.1.1.2.13.2 Requires Authorization

❌ No

#### 2.1.1.3.0.0 Service

##### 2.1.1.3.1.0 Id

iam-service-001

##### 2.1.1.3.2.0 Name

Identity & Access Management Service

##### 2.1.1.3.3.0 Description

Wrapper service that integrates with Keycloak. Manages user profiles, role assignments, and permission scopes within the application context. (REQ-1-011, REQ-1-061)

##### 2.1.1.3.4.0 Type

🔹 Service

##### 2.1.1.3.5.0 Dependencies

- keycloak-idp-001
- postgresql-db-001
- redis-cache-001

##### 2.1.1.3.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |

##### 2.1.1.3.7.0 Interfaces

- REST API (/api/v1/users, /api/v1/roles)

##### 2.1.1.3.8.0 Technology

.NET 8

##### 2.1.1.3.9.0 Resources

###### 2.1.1.3.9.1 Cpu

1 core

###### 2.1.1.3.9.2 Memory

2GB

##### 2.1.1.3.10.0 Configuration

###### 2.1.1.3.10.1 Keycloak Realm

SystemRealm

###### 2.1.1.3.10.2 Keycloak Client Id

system-backend

##### 2.1.1.3.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.3.12.0 Responsible Features

- User Management
- Role-Based Access Control (RBAC)
- GDPR Data Subject Rights (REQ-1-027)

##### 2.1.1.3.13.0 Security

###### 2.1.1.3.13.1 Requires Authentication

✅ Yes

###### 2.1.1.3.13.2 Requires Authorization

✅ Yes

###### 2.1.1.3.13.3 Allowed Roles

- Administrator

#### 2.1.1.4.0.0 Service

##### 2.1.1.4.1.0 Id

tenant-service-002

##### 2.1.1.4.2.0 Name

Tenant Management Service

##### 2.1.1.4.3.0 Description

Manages tenant lifecycle, data residency configuration, isolation model, and licensing. (REQ-1-024, REQ-1-030, REQ-1-063)

##### 2.1.1.4.4.0 Type

🔹 Service

##### 2.1.1.4.5.0 Dependencies

- postgresql-db-001

##### 2.1.1.4.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |

##### 2.1.1.4.7.0 Interfaces

- REST API (/api/v1/tenants, /api/v1/licenses)

##### 2.1.1.4.8.0 Technology

.NET 8

##### 2.1.1.4.9.0 Resources

###### 2.1.1.4.9.1 Cpu

1 core

###### 2.1.1.4.9.2 Memory

2GB

##### 2.1.1.4.10.0 Configuration

###### 2.1.1.4.10.1 Default Isolation Model

RLS

###### 2.1.1.4.10.2 Default Data Region

us-east-1

##### 2.1.1.4.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.4.12.0 Responsible Features

- Multi-Tenancy Management
- License Management
- Data Residency Enforcement

##### 2.1.1.4.13.0 Security

###### 2.1.1.4.13.1 Requires Authentication

✅ Yes

###### 2.1.1.4.13.2 Requires Authorization

✅ Yes

###### 2.1.1.4.13.3 Allowed Roles

- Administrator

#### 2.1.1.5.0.0 Service

##### 2.1.1.5.1.0 Id

device-service-003

##### 2.1.1.5.2.0 Name

Device Management Service

##### 2.1.1.5.3.0 Description

Manages the lifecycle of OPC Core Client instances, including registration, health monitoring, configuration pushes, and remote updates. (REQ-1-062, REQ-1-064, REQ-1-082)

##### 2.1.1.5.4.0 Type

🔹 Service

##### 2.1.1.5.5.0 Dependencies

- postgresql-db-001
- mqtt-broker-001
- s3-storage-001

##### 2.1.1.5.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |

##### 2.1.1.5.7.0 Interfaces

- REST API (/api/v1/clients)
- MQTT Command Topic Subscriber

##### 2.1.1.5.8.0 Technology

.NET 8

##### 2.1.1.5.9.0 Resources

###### 2.1.1.5.9.1 Cpu

1 core

###### 2.1.1.5.9.2 Memory

2GB

##### 2.1.1.5.10.0 Configuration

###### 2.1.1.5.10.1 Update Package Bucket

opc-client-updates

###### 2.1.1.5.10.2 Registration Token Ttl

24h

##### 2.1.1.5.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.5.12.0 Responsible Features

- OPC Client Fleet Management
- Secure Client Provisioning
- Remote Software Updates

##### 2.1.1.5.13.0 Security

###### 2.1.1.5.13.1 Requires Authentication

✅ Yes

###### 2.1.1.5.13.2 Requires Authorization

✅ Yes

###### 2.1.1.5.13.3 Allowed Roles

- Administrator

#### 2.1.1.6.0.0 Service

##### 2.1.1.6.1.0 Id

asset-service-004

##### 2.1.1.6.2.0 Name

Asset & Topology Service

##### 2.1.1.6.3.0 Description

Manages the ISA-95 asset hierarchy, asset templates, and the mapping of OPC tags to assets. (REQ-1-031, REQ-1-046, REQ-1-047, REQ-1-048)

##### 2.1.1.6.4.0 Type

🔹 Service

##### 2.1.1.6.5.0 Dependencies

- postgresql-db-001
- redis-cache-001

##### 2.1.1.6.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |

##### 2.1.1.6.7.0 Interfaces

- REST API (/api/v1/assets, /api/v1/tags)

##### 2.1.1.6.8.0 Technology

.NET 8

##### 2.1.1.6.9.0 Resources

###### 2.1.1.6.9.1 Cpu

1 core

###### 2.1.1.6.9.2 Memory

2GB

##### 2.1.1.6.10.0 Configuration

###### 2.1.1.6.10.1 Hierarchy Cache Ttl

3600s

##### 2.1.1.6.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.6.12.0 Responsible Features

- Asset Hierarchy Management
- OPC Tag Mapping
- Asset Templating

##### 2.1.1.6.13.0 Security

###### 2.1.1.6.13.1 Requires Authentication

✅ Yes

###### 2.1.1.6.13.2 Requires Authorization

✅ Yes

###### 2.1.1.6.13.3 Allowed Roles

- Administrator
- Engineer

#### 2.1.1.7.0.0 Service

##### 2.1.1.7.1.0 Id

ingestion-service-001

##### 2.1.1.7.2.0 Name

Data Ingestion Service

##### 2.1.1.7.3.0 Description

High-throughput service that receives time-series data streams from OPC Core Clients via gRPC and persists them to the time-series database. (REQ-1-010, REQ-1-075)

##### 2.1.1.7.4.0 Type

🔹 Service

##### 2.1.1.7.5.0 Dependencies

- timescaledb-db-001

##### 2.1.1.7.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |

##### 2.1.1.7.7.0 Interfaces

- gRPC Service (Ingest.StreamData)

##### 2.1.1.7.8.0 Technology

.NET 8, gRPC

##### 2.1.1.7.9.0 Resources

###### 2.1.1.7.9.1 Cpu

4 cores

###### 2.1.1.7.9.2 Memory

8GB

##### 2.1.1.7.10.0 Configuration

###### 2.1.1.7.10.1 Batch Size

1,000

###### 2.1.1.7.10.2 Batch Timeout Ms

500

##### 2.1.1.7.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.7.12.0 Responsible Features

- High-Volume Data Ingestion

##### 2.1.1.7.13.0 Security

###### 2.1.1.7.13.1 Requires Authentication

✅ Yes

###### 2.1.1.7.13.2 Requires Authorization

❌ No

#### 2.1.1.8.0.0 Service

##### 2.1.1.8.1.0 Id

query-service-005

##### 2.1.1.8.2.0 Name

Query & Analytics Service

##### 2.1.1.8.3.0 Description

Provides APIs for querying historical data (HDA), generating reports, and performing non-real-time analytics. (REQ-1-034, REQ-1-065)

##### 2.1.1.8.4.0 Type

🔹 Service

##### 2.1.1.8.5.0 Dependencies

- timescaledb-db-001
- postgresql-db-001
- s3-storage-001

##### 2.1.1.8.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |

##### 2.1.1.8.7.0 Interfaces

- REST API (/api/v1/query/history, /api/v1/reports)

##### 2.1.1.8.8.0 Technology

.NET 8

##### 2.1.1.8.9.0 Resources

###### 2.1.1.8.9.1 Cpu

2 cores

###### 2.1.1.8.9.2 Memory

4GB

##### 2.1.1.8.10.0 Configuration

###### 2.1.1.8.10.1 Max Query Range

30d

###### 2.1.1.8.10.2 Report Output Bucket

system-generated-reports

##### 2.1.1.8.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.8.12.0 Responsible Features

- Historical Data Access (HDA)
- Automated Reporting
- Data Export (CSV, Excel)

##### 2.1.1.8.13.0 Security

###### 2.1.1.8.13.1 Requires Authentication

✅ Yes

###### 2.1.1.8.13.2 Requires Authorization

✅ Yes

###### 2.1.1.8.13.3 Allowed Roles

- Administrator
- Data Scientist
- Engineer
- Operator
- Viewer

#### 2.1.1.9.0.0 Service

##### 2.1.1.9.1.0 Id

aiml-service-006

##### 2.1.1.9.2.0 Name

AI/ML Management Service

##### 2.1.1.9.3.0 Description

Manages the lifecycle of AI models, including versioning, approval workflows, and assignments to assets. (REQ-1-049, REQ-1-012, REQ-1-013, REQ-1-032)

##### 2.1.1.9.4.0 Type

🔹 Service

##### 2.1.1.9.5.0 Dependencies

- postgresql-db-001
- s3-storage-001

##### 2.1.1.9.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |

##### 2.1.1.9.7.0 Interfaces

- REST API (/api/v1/models, /api/v1/model-assignments)

##### 2.1.1.9.8.0 Technology

.NET 8

##### 2.1.1.9.9.0 Resources

###### 2.1.1.9.9.1 Cpu

1 core

###### 2.1.1.9.9.2 Memory

2GB

##### 2.1.1.9.10.0 Configuration

###### 2.1.1.9.10.1 Model Storage Bucket

ai-models-repository

##### 2.1.1.9.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.9.12.0 Responsible Features

- AI Model Lifecycle Management
- Model Approval Workflow
- Model Deployment to Edge

##### 2.1.1.9.13.0 Security

###### 2.1.1.9.13.1 Requires Authentication

✅ Yes

###### 2.1.1.9.13.2 Requires Authorization

✅ Yes

###### 2.1.1.9.13.3 Allowed Roles

- Administrator
- Data Scientist
- Engineer

#### 2.1.1.10.0.0 Service

##### 2.1.1.10.1.0 Id

alarm-service-007

##### 2.1.1.10.2.0 Name

Alarm & Notification Service

##### 2.1.1.10.3.0 Description

Processes incoming alarms, manages their state (active, ack, shelved), and routes notifications based on configured rules. (REQ-1-035, REQ-1-036, REQ-1-037)

##### 2.1.1.10.4.0 Type

🔹 Service

##### 2.1.1.10.5.0 Dependencies

- postgresql-db-001
- redis-cache-001

##### 2.1.1.10.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |

##### 2.1.1.10.7.0 Interfaces

- REST API (/api/v1/alarms)
- gRPC Service (Ingest.StreamAlarms)

##### 2.1.1.10.8.0 Technology

.NET 8

##### 2.1.1.10.9.0 Resources

###### 2.1.1.10.9.1 Cpu

1 core

###### 2.1.1.10.9.2 Memory

2GB

##### 2.1.1.10.10.0 Configuration

###### 2.1.1.10.10.1 Notification Webhooks

Configurable per tenant (e.g., PagerDuty, Slack)

##### 2.1.1.10.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.10.12.0 Responsible Features

- Alarm & Event (A&C) Management
- Alarm Shelving
- Configurable Notifications

##### 2.1.1.10.13.0 Security

###### 2.1.1.10.13.1 Requires Authentication

✅ Yes

###### 2.1.1.10.13.2 Requires Authorization

✅ Yes

###### 2.1.1.10.13.3 Allowed Roles

- Administrator
- Engineer
- Operator

#### 2.1.1.11.0.0 Service

##### 2.1.1.11.1.0 Id

audit-service-008

##### 2.1.1.11.2.0 Name

Audit Service

##### 2.1.1.11.3.0 Description

Provides an API to create immutable audit log entries for all significant system and user actions. Integrates with QLDB for cryptographic verification. (REQ-1-040, REQ-1-059)

##### 2.1.1.11.4.0 Type

🔹 Service

##### 2.1.1.11.5.0 Dependencies

- postgresql-db-001
- qldb-ledger-001

##### 2.1.1.11.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 1.0.0 |

##### 2.1.1.11.7.0 Interfaces

- gRPC Service (Audit.LogAction)

##### 2.1.1.11.8.0 Technology

.NET 8

##### 2.1.1.11.9.0 Resources

###### 2.1.1.11.9.1 Cpu

1 core

###### 2.1.1.11.9.2 Memory

2GB

##### 2.1.1.11.10.0 Configuration

###### 2.1.1.11.10.1 Qldb Ledger Name

SystemAuditTrail

###### 2.1.1.11.10.2 Batching Interval

5m

##### 2.1.1.11.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.11.12.0 Responsible Features

- Immutable Audit Trail
- Regulatory Compliance (21 CFR Part 11)
- Cryptographic Verification

##### 2.1.1.11.13.0 Security

###### 2.1.1.11.13.1 Requires Authentication

✅ Yes

###### 2.1.1.11.13.2 Requires Authorization

❌ No

#### 2.1.1.12.0.0 Edge Application

##### 2.1.1.12.1.0 Id

opc-core-client-001

##### 2.1.1.12.2.0 Name

OPC Core Client

##### 2.1.1.12.3.0 Description

A cross-platform, containerized application deployed on-premise. It connects to OPC servers, buffers data, and communicates with the Central Management Plane. (REQ-1-001, REQ-1-008)

##### 2.1.1.12.4.0 Type

🔹 Edge Application

##### 2.1.1.12.5.0 Dependencies

- ingestion-service-001
- mqtt-broker-001

##### 2.1.1.12.6.0 Properties

| Property | Value |
|----------|-------|
| Distribution | Docker Image |
| Supported Os | Windows, Ubuntu, RHEL (REQ-1-017) |

##### 2.1.1.12.7.0 Interfaces

- OPC UA Client
- OPC DA (COM/DCOM) Client
- OPC XML-DA Client
- gRPC Client (Data Streaming)
- MQTT Client (Command/Control)

##### 2.1.1.12.8.0 Technology

.NET 8, Docker

##### 2.1.1.12.9.0 Resources

| Property | Value |
|----------|-------|
| Cpu | 2 cores |
| Memory | 4GB |
| Storage | 10GB (for buffer) |

##### 2.1.1.12.10.0 Configuration

| Property | Value |
|----------|-------|
| Registration Token | Single-use token for provisioning |
| Buffer Size Gb | 1 |
| Management Plane Url | URL of the Central Management Plane |

##### 2.1.1.12.11.0 Health Check

| Property | Value |
|----------|-------|
| Path | /healthz |
| Interval | 60 |
| Timeout | 5 |

##### 2.1.1.12.12.0 Responsible Features

- OPC Connectivity (DA, UA, XML-DA)
- Autonomous Operation
- Offline Data Buffering (REQ-1-079)
- Edge AI Inference (REQ-1-056)

##### 2.1.1.12.13.0 Security

###### 2.1.1.12.13.1 Requires Authentication

✅ Yes

###### 2.1.1.12.13.2 Requires Authorization

❌ No

#### 2.1.1.13.0.0 Messaging Broker

##### 2.1.1.13.1.0 Id

mqtt-broker-001

##### 2.1.1.13.2.0 Name

MQTT Broker

##### 2.1.1.13.3.0 Description

Provides secure, brokered messaging for command, control, and status updates between the cloud and edge clients. (REQ-1-010)

##### 2.1.1.13.4.0 Type

🔹 Messaging Broker

##### 2.1.1.13.5.0 Dependencies

*No items available*

##### 2.1.1.13.6.0 Properties

| Property | Value |
|----------|-------|
| Protocol Version | MQTT v5 |

##### 2.1.1.13.7.0 Interfaces

- MQTT over TLS

##### 2.1.1.13.8.0 Technology

AWS IoT Core or EMQX

##### 2.1.1.13.9.0 Resources

###### 2.1.1.13.9.1 Cpu

N/A (Managed Service)

###### 2.1.1.13.9.2 Memory

N/A (Managed Service)

##### 2.1.1.13.10.0 Configuration

###### 2.1.1.13.10.1 Topic Structure

tenant/{tenantId}/client/{clientId}/...

##### 2.1.1.13.11.0 Health Check

*Not specified*

##### 2.1.1.13.12.0 Responsible Features

- Cloud-to-Edge Command and Control

##### 2.1.1.13.13.0 Security

###### 2.1.1.13.13.1 Requires Authentication

✅ Yes

###### 2.1.1.13.13.2 Requires Authorization

✅ Yes

#### 2.1.1.14.0.0 Database

##### 2.1.1.14.1.0 Id

postgresql-db-001

##### 2.1.1.14.2.0 Name

PostgreSQL Relational Database

##### 2.1.1.14.3.0 Description

Primary database for storing structured, relational data such as tenants, users, assets, and configurations. (REQ-1-089)

##### 2.1.1.14.4.0 Type

🔹 Database

##### 2.1.1.14.5.0 Dependencies

*No items available*

##### 2.1.1.14.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 16 |

##### 2.1.1.14.7.0 Interfaces

- SQL

##### 2.1.1.14.8.0 Technology

PostgreSQL on AWS RDS

##### 2.1.1.14.9.0 Resources

###### 2.1.1.14.9.1 Storage

Scalable

##### 2.1.1.14.10.0 Configuration

###### 2.1.1.14.10.1 Row Level Security

Enabled

###### 2.1.1.14.10.2 Encryption At Rest

Enabled

##### 2.1.1.14.11.0 Health Check

*Not specified*

##### 2.1.1.14.12.0 Responsible Features

- Relational Data Persistence

##### 2.1.1.14.13.0 Security

###### 2.1.1.14.13.1 Requires Authentication

✅ Yes

###### 2.1.1.14.13.2 Requires Authorization

✅ Yes

#### 2.1.1.15.0.0 Database

##### 2.1.1.15.1.0 Id

timescaledb-db-001

##### 2.1.1.15.2.0 Name

TimescaleDB Time-Series Database

##### 2.1.1.15.3.0 Description

Specialized database for storing and efficiently querying high-volume time-series data from OPC tags. (REQ-1-089, REQ-1-075)

##### 2.1.1.15.4.0 Type

🔹 Database

##### 2.1.1.15.5.0 Dependencies

*No items available*

##### 2.1.1.15.6.0 Properties

| Property | Value |
|----------|-------|
| Hypertable Strategy | Partitioned by time |

##### 2.1.1.15.7.0 Interfaces

- SQL

##### 2.1.1.15.8.0 Technology

TimescaleDB on AWS RDS

##### 2.1.1.15.9.0 Resources

###### 2.1.1.15.9.1 Storage

Scalable

##### 2.1.1.15.10.0 Configuration

###### 2.1.1.15.10.1 Continuous Aggregates

Enabled for hourly/daily rollups

###### 2.1.1.15.10.2 Data Retention Policies

Configurable per tenant (REQ-1-088)

##### 2.1.1.15.11.0 Health Check

*Not specified*

##### 2.1.1.15.12.0 Responsible Features

- Time-Series Data Persistence

##### 2.1.1.15.13.0 Security

###### 2.1.1.15.13.1 Requires Authentication

✅ Yes

###### 2.1.1.15.13.2 Requires Authorization

✅ Yes

#### 2.1.1.16.0.0 Cache

##### 2.1.1.16.1.0 Id

redis-cache-001

##### 2.1.1.16.2.0 Name

Redis In-Memory Cache

##### 2.1.1.16.3.0 Description

Provides a distributed, in-memory cache to store frequently accessed data like user sessions, permissions, and asset hierarchy, reducing database load. (REQ-1-026)

##### 2.1.1.16.4.0 Type

🔹 Cache

##### 2.1.1.16.5.0 Dependencies

*No items available*

##### 2.1.1.16.6.0 Properties

| Property | Value |
|----------|-------|
| Version | 7 |

##### 2.1.1.16.7.0 Interfaces

- Redis Protocol

##### 2.1.1.16.8.0 Technology

Redis on AWS ElastiCache

##### 2.1.1.16.9.0 Resources

###### 2.1.1.16.9.1 Memory

Scalable

##### 2.1.1.16.10.0 Configuration

###### 2.1.1.16.10.1 Key Prefixing

Mandatory tenantId prefix

###### 2.1.1.16.10.2 Eviction Policy

LRU

##### 2.1.1.16.11.0 Health Check

*Not specified*

##### 2.1.1.16.12.0 Responsible Features

- Performance Optimization via Caching

##### 2.1.1.16.13.0 Security

###### 2.1.1.16.13.1 Requires Authentication

✅ Yes

###### 2.1.1.16.13.2 Requires Authorization

❌ No

#### 2.1.1.17.0.0 Object Storage

##### 2.1.1.17.1.0 Id

s3-storage-001

##### 2.1.1.17.2.0 Name

Amazon S3 Object Storage

##### 2.1.1.17.3.0 Description

Stores large binary objects, including AI models in ONNX format, generated reports, and software update packages. (REQ-1-026)

##### 2.1.1.17.4.0 Type

🔹 Object Storage

##### 2.1.1.17.5.0 Dependencies

*No items available*

##### 2.1.1.17.6.0 Properties

*No data available*

##### 2.1.1.17.7.0 Interfaces

- S3 API

##### 2.1.1.17.8.0 Technology

Amazon S3

##### 2.1.1.17.9.0 Resources

###### 2.1.1.17.9.1 Storage

Effectively unlimited

##### 2.1.1.17.10.0 Configuration

###### 2.1.1.17.10.1 Folder Structure

Tenant-specific folders within shared buckets

###### 2.1.1.17.10.2 Encryption At Rest

Enabled (SSE-S3)

##### 2.1.1.17.11.0 Health Check

*Not specified*

##### 2.1.1.17.12.0 Responsible Features

- Large Object Storage

##### 2.1.1.17.13.0 Security

###### 2.1.1.17.13.1 Requires Authentication

✅ Yes

###### 2.1.1.17.13.2 Requires Authorization

✅ Yes

#### 2.1.1.18.0.0 Ledger Database

##### 2.1.1.18.1.0 Id

qldb-ledger-001

##### 2.1.1.18.2.0 Name

Amazon QLDB Ledger

##### 2.1.1.18.3.0 Description

Provides a cryptographic, immutable ledger to anchor hashes of audit log batches, ensuring the integrity of the audit trail. (REQ-1-059)

##### 2.1.1.18.4.0 Type

🔹 Ledger Database

##### 2.1.1.18.5.0 Dependencies

*No items available*

##### 2.1.1.18.6.0 Properties

*No data available*

##### 2.1.1.18.7.0 Interfaces

- PartiQL

##### 2.1.1.18.8.0 Technology

Amazon QLDB

##### 2.1.1.18.9.0 Resources

*No data available*

##### 2.1.1.18.10.0 Configuration

###### 2.1.1.18.10.1 Ledger Name

SystemAuditTrail

##### 2.1.1.18.11.0 Health Check

*Not specified*

##### 2.1.1.18.12.0 Responsible Features

- Audit Trail Integrity Verification

##### 2.1.1.18.13.0 Security

###### 2.1.1.18.13.1 Requires Authentication

✅ Yes

###### 2.1.1.18.13.2 Requires Authorization

✅ Yes

#### 2.1.1.19.0.0 Identity Provider

##### 2.1.1.19.1.0 Id

keycloak-idp-001

##### 2.1.1.19.2.0 Name

Keycloak Identity Provider

##### 2.1.1.19.3.0 Description

Centralized Identity Provider (IdP) for user authentication and token issuance, supporting OAuth 2.0 and OpenID Connect. (REQ-1-080)

##### 2.1.1.19.4.0 Type

🔹 Identity Provider

##### 2.1.1.19.5.0 Dependencies

*No items available*

##### 2.1.1.19.6.0 Properties

*No data available*

##### 2.1.1.19.7.0 Interfaces

- OIDC Endpoints

##### 2.1.1.19.8.0 Technology

Keycloak

##### 2.1.1.19.9.0 Resources

*No data available*

##### 2.1.1.19.10.0 Configuration

###### 2.1.1.19.10.1 Realm

SystemRealm

##### 2.1.1.19.11.0 Health Check

*Not specified*

##### 2.1.1.19.12.0 Responsible Features

- User Authentication

##### 2.1.1.19.13.0 Security

###### 2.1.1.19.13.1 Requires Authentication

❌ No

###### 2.1.1.19.13.2 Requires Authorization

❌ No

### 2.1.2.0.0.0 Configuration

| Property | Value |
|----------|-------|
| Environment | production |
| Logging Level | INFO |
| Database Url | jdbc:postgresql://prod-db.rds.amazonaws.com:5432/a... |
| Cache Ttl | 3600 |
| Max Threads | 50 |

