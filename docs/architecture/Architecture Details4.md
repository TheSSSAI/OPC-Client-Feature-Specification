# 1 Style

Microservices

# 2 Patterns

## 2.1 Cloud-Native Architecture

### 2.1.1 Name

Cloud-Native Architecture

### 2.1.2 Description

An approach to building and running applications that exploits the advantages of the cloud computing delivery model. The system is designed for deployment on AWS EKS, using managed services, containerization, and microservices to achieve high availability, scalability, and resilience.

### 2.1.3 Benefits

- Leverages managed cloud services (EKS, RDS, S3) to reduce operational overhead.
- Enables dynamic scaling of services based on load (REQ-1-085).
- Improves fault tolerance and resilience through distributed, containerized services (REQ-1-078).

### 2.1.4 Applicability

#### 2.1.4.1 Scenarios

- Building SaaS applications with variable workloads.
- Systems requiring high availability and automated disaster recovery.
- Applications needing to scale horizontally to support many tenants and devices.

## 2.2.0.0 Edge Computing

### 2.2.1.0 Name

Edge Computing

### 2.2.2.0 Description

A distributed computing paradigm that brings computation and data storage closer to the sources of data. The OPC Core Client performs data acquisition, buffering, and AI inference directly on-premise or at the edge, reducing latency and enabling autonomous operation.

### 2.2.3.0 Benefits

- Enables autonomous operation and data buffering during network disconnects (REQ-1-008, REQ-1-079).
- Reduces latency for real-time control and anomaly detection by processing data locally (REQ-1-052, REQ-1-075).
- Decreases cloud data transfer costs by pre-processing and aggregating data at the edge.

### 2.2.4.0 Applicability

#### 2.2.4.1 Scenarios

- Industrial IoT (IIoT) data collection and control.
- Applications requiring low-latency AI inference on real-time data streams.
- Systems deployed in locations with intermittent or low-bandwidth network connectivity.

## 2.3.0.0 Multi-Tenant SaaS Architecture

### 2.3.1.0 Name

Multi-Tenant SaaS Architecture

### 2.3.2.0 Description

A software architecture where a single instance of the software serves multiple customers (tenants). Data and configuration are logically isolated to ensure privacy and security, using a hybrid model of Row-Level Security and optional separate schemas/buckets.

### 2.3.3.0 Benefits

- Enables a scalable Software-as-a-Service (SaaS) business model (REQ-1-024).
- Centralizes management, maintenance, and updates for all customers.
- Optimizes resource utilization by sharing infrastructure across tenants.

### 2.3.4.0 Applicability

#### 2.3.4.1 Scenarios

- Commercial software products serving multiple independent organizations.
- Platforms where economies of scale in infrastructure are a key business driver.

## 2.4.0.0 API Gateway

### 2.4.1.0 Name

API Gateway

### 2.4.2.0 Description

A single entry point for all client requests, routing them to the appropriate backend microservice. The system uses Kong to handle concerns like authentication, rate limiting, and routing.

### 2.4.3.0 Benefits

- Provides a unified and secure interface for external clients (Frontend, external systems) (REQ-1-086).
- Decouples clients from the internal microservice architecture.
- Centralizes cross-cutting concerns like JWT validation and rate limiting (REQ-1-080).

### 2.4.4.0 Applicability

#### 2.4.4.1 Scenarios

- Microservice-based architectures to simplify client interaction.
- Exposing a consistent and secure set of public APIs.

# 3.0.0.0 Layers

## 3.1.0.0 Presentation

### 3.1.1.0 Id

presentation-layer-spa

### 3.1.2.0 Name

Presentation Layer (Central Management Plane)

### 3.1.3.0 Description

A web-based Single-Page Application (SPA) that serves as the primary user interface for administration, configuration, and data visualization. It communicates with backend services via the API Gateway.

### 3.1.4.0 Technologystack

React 18, TypeScript, Axios, WebSocket client libraries

### 3.1.5.0 Language

TypeScript

### 3.1.6.0 Type

🔹 Presentation

### 3.1.7.0 Responsibilities

- Provide a responsive web interface for system administration and monitoring (REQ-1-070).
- Render real-time data, historical trends, and alarm summaries (REQ-1-033, REQ-1-034, REQ-1-035).
- Enable configuration of tenants, users, assets, OPC clients, and AI models (REQ-1-005, REQ-1-046).
- Support internationalization for English, German, and Spanish (REQ-1-044).
- Implement user-specific, customizable dashboards (REQ-1-044).

### 3.1.8.0 Components

- Dashboard Component
- Asset Hierarchy Management Component
- Alarm Console Component
- AI Model Management Component
- User & Role Administration Component
- OPC Client Fleet Management Component

### 3.1.9.0 Dependencies

- {'layerId': 'api-gateway-kong', 'type': 'Required'}

## 3.2.0.0 APIGateway

### 3.2.1.0 Id

api-gateway-kong

### 3.2.2.0 Name

API Gateway

### 3.2.3.0 Description

Manages all incoming API traffic from external clients, providing routing, security, and traffic control. It sits in front of all backend microservices.

### 3.2.4.0 Technologystack

Kong API Gateway on Kubernetes

### 3.2.5.0 Language

N/A

### 3.2.6.0 Type

🔹 APIGateway

### 3.2.7.0 Responsibilities

- Serve as the single, secure entry point for the frontend SPA and external REST clients.
- Perform JWT validation for all incoming requests to secure API endpoints (REQ-1-080).
- Route requests to the appropriate backend microservice based on the URL path.
- Implement rate limiting and traffic shaping policies to protect backend services.
- Aggregate/Expose OpenAPI specifications from downstream services for documentation (REQ-1-086).

### 3.2.8.0 Dependencies

- {'layerId': 'app-services-microservices', 'type': 'Required'}

## 3.3.0.0 ApplicationServices

### 3.3.1.0 Id

app-services-microservices

### 3.3.2.0 Name

Application Services Layer (Microservices)

### 3.3.3.0 Description

A collection of independent, stateless microservices that implement the core business logic and domain functionality of the Central Management Plane. Each service owns a specific business capability.

### 3.3.4.0 Technologystack

.NET 8, gRPC, REST (ASP.NET Core Web API)

### 3.3.5.0 Language

C#

### 3.3.6.0 Type

🔹 ApplicationServices

### 3.3.7.0 Responsibilities

- Implement domain-specific business logic according to DDD principles (REQ-1-029).
- Expose functionality via secure REST and gRPC APIs (REQ-1-072).
- Ensure data isolation between tenants (REQ-1-025).
- Communicate with other services via synchronous (gRPC/REST) or asynchronous (messaging) patterns.
- Scale horizontally and independently to meet demand (REQ-1-085).

### 3.3.8.0 Components

- Identity & Access Management Service (Manages Users, Roles, Permissions via Keycloak)
- Tenant Management Service (Manages Tenants, Licenses, Data Residency)
- Device Management Service (Manages OPC Core Client instances, status, updates)
- Asset & Topology Service (Manages ISA-95 asset hierarchy and tag mapping)
- Data Ingestion Service (Handles high-volume gRPC data streams from clients)
- AI/ML Management Service (Manages model lifecycle, versioning, and approval workflows)
- Alarm & Notification Service (Processes alarms and routes notifications)
- Query & Analytics Service (Provides APIs for historical data, HDA, and reporting)
- Audit Service (Manages the immutable audit trail and QLDB integration)

### 3.3.9.0 Dependencies

#### 3.3.9.1 Optional

##### 3.3.9.1.1 Layer Id

messaging-layer

##### 3.3.9.1.2 Type

🔹 Optional

#### 3.3.9.2.0 Required

##### 3.3.9.2.1 Layer Id

infrastructure-data-layer

##### 3.3.9.2.2 Type

🔹 Required

## 3.4.0.0.0 BusinessLogic

### 3.4.1.0.0 Id

edge-application-opc-client

### 3.4.2.0.0 Name

Edge Application Layer (OPC Core Client)

### 3.4.3.0.0 Description

A cross-platform, containerized application deployed on-premise or at the edge. It is responsible for direct communication with industrial control systems, local data processing, and secure communication with the Central Management Plane.

### 3.4.4.0.0 Technologystack

.NET 8, Docker, OPCFoundation.NetStandard.Opc.Ua, Microsoft.ML.OnnxRuntime

### 3.4.5.0.0 Language

C#

### 3.4.6.0.0 Type

🔹 BusinessLogic

### 3.4.7.0.0 Responsibilities

- Connect to OPC DA, UA, and XML-DA servers for data acquisition (REQ-1-002, REQ-1-041).
- Operate autonomously and buffer data during network disconnects (REQ-1-008, REQ-1-079).
- Stream high-volume data to the cloud via gRPC and use MQTT for command/control (REQ-1-010).
- Execute AI/ML models in ONNX format for real-time inference on edge hardware (REQ-1-049, REQ-1-056).
- Manage its own lifecycle, including secure provisioning and software updates (REQ-1-082, REQ-1-064).
- Run on supported Windows and Linux operating systems (REQ-1-017).

### 3.4.8.0.0 Components

- OPC Connectivity Module (Handles DA, UA, XML-DA connections and subscriptions)
- Cloud Communication Module (Manages gRPC and MQTT connections)
- Persistent Data Buffer Module (Implements on-disk circular buffer for offline data)
- Edge AI Runtime Module (Loads and executes ONNX models)
- Configuration & Update Module (Receives and applies updates from the central plane)
- Health Monitoring Module (Reports status and diagnostics)

### 3.4.9.0.0 Dependencies

- {'layerId': 'infrastructure-data-layer', 'type': 'Required'}

## 3.5.0.0.0 Messaging

### 3.5.1.0.0 Id

messaging-layer

### 3.5.2.0.0 Name

Messaging & Eventing Layer

### 3.5.3.0.0 Description

Provides asynchronous communication channels for command and control messaging between the cloud and edge, and potentially for inter-service communication.

### 3.5.4.0.0 Technologystack

MQTT v5 Broker (e.g., AWS IoT Core, EMQX), gRPC

### 3.5.5.0.0 Language

N/A

### 3.5.6.0.0 Type

🔹 Messaging

### 3.5.7.0.0 Responsibilities

- Provide a secure and robust transport for command, control, and status messages between the Central Management Plane and OPC Core Clients using MQTT (REQ-1-010).
- Enable high-volume, low-latency data streaming from OPC Core Clients to the Data Ingestion Service using gRPC with mTLS (REQ-1-010).
- Facilitate event-driven communication between microservices to promote loose coupling.

### 3.5.8.0.0 Components

- MQTT Broker
- gRPC Service Endpoints

## 3.6.0.0.0 DataAccess

### 3.6.1.0.0 Id

infrastructure-data-layer

### 3.6.2.0.0 Name

Infrastructure & Data Persistence Layer

### 3.6.3.0.0 Description

The foundation of the system, comprising all data stores, caches, identity providers, and cloud infrastructure for persistence, storage, and runtime orchestration.

### 3.6.4.0.0 Technologystack

PostgreSQL 16, TimescaleDB, Redis 7, Amazon S3, Amazon QLDB, Keycloak

### 3.6.5.0.0 Language

N/A

### 3.6.6.0.0 Type

🔹 DataAccess

### 3.6.7.0.0 Responsibilities

- Persist relational data (tenants, users, assets) in PostgreSQL (REQ-1-089).
- Store and query high-volume time-series data efficiently using TimescaleDB (REQ-1-075).
- Cache frequently accessed data (e.g., user sessions, asset hierarchy) in Redis (REQ-1-026).
- Store large binary objects like AI models and reports in Amazon S3 (REQ-1-026).
- Provide a verifiable, immutable ledger for the audit trail using Amazon QLDB (REQ-1-059).
- Manage user identity and authentication via Keycloak (REQ-1-080).
- Ensure all data is encrypted at rest (REQ-1-081).

### 3.6.8.0.0 Components

- PostgreSQL + TimescaleDB on AWS RDS
- Redis on AWS ElastiCache
- Amazon S3 Buckets
- Amazon Quantum Ledger Database (QLDB)
- Keycloak Identity Provider

## 3.7.0.0.0 CrossCutting

### 3.7.1.0.0 Id

cross-cutting-concerns

### 3.7.2.0.0 Name

Cross-Cutting Concerns (Observability & DevOps)

### 3.7.3.0.0 Description

A set of tools and practices that apply across all layers of the architecture to ensure operational excellence, maintainability, and security.

### 3.7.4.0.0 Technologystack

Prometheus, Grafana, OpenSearch, Fluentd, OpenTelemetry, AWS Secrets Manager, GitHub Actions, Terraform

### 3.7.5.0.0 Language

N/A

### 3.7.6.0.0 Type

🔹 CrossCutting

### 3.7.7.0.0 Responsibilities

- Aggregate logs from all services into a centralized OpenSearch cluster for analysis (REQ-1-090).
- Scrape and visualize metrics from all components using Prometheus and Grafana (REQ-1-090).
- Enable distributed tracing across microservices with OpenTelemetry (REQ-1-090).
- Securely manage all secrets, keys, and certificates using AWS Secrets Manager (REQ-1-081).
- Automate the build, testing, and deployment of all components via a CI/CD pipeline (REQ-1-086).
- Define and manage all cloud infrastructure as code using Terraform (REQ-1-089).

### 3.7.8.0.0 Components

- Monitoring Stack (Prometheus, Grafana)
- Logging Stack (Fluentd, OpenSearch)
- CI/CD Pipeline (GitHub Actions)
- Infrastructure as Code (Terraform)
- Secret Management (AWS Secrets Manager)

# 4.0.0.0.0 Quality Attributes

## 4.1.0.0.0 Security

### 4.1.1.0.0 Name

Security

### 4.1.2.0.0 Priority

🔴 High

### 4.1.3.0.0 Requirements

- REQ-1-007
- REQ-1-080
- REQ-1-081
- REQ-1-082
- REQ-1-073

### 4.1.4.0.0 Tactics

- Utilize Keycloak for centralized authentication (OAuth 2.0/OIDC) and RBAC.
- Enforce encryption in transit (TLS 1.3) and at rest (AES-256).
- Use mTLS for client-to-cloud communication (gRPC).
- Implement a secure, token-based provisioning workflow for new OPC Core Clients.
- Store all credentials and secrets in AWS Secrets Manager.
- Maintain a tamper-evident audit trail anchored to Amazon QLDB.

## 4.2.0.0.0 Scalability

### 4.2.1.0.0 Name

Scalability

### 4.2.2.0.0 Priority

🔴 High

### 4.2.3.0.0 Requirements

- REQ-1-085
- REQ-1-075

### 4.2.4.0.0 Tactics

- Design all cloud microservices to be stateless.
- Utilize Kubernetes Horizontal Pod Autoscalers (HPA) on EKS for automatic scaling.
- Employ TimescaleDB for high-rate, scalable time-series data ingestion.
- Use a distributed caching layer (Redis) to offload database pressure.

## 4.3.0.0.0 Availability & Reliability

### 4.3.1.0.0 Name

Availability & Reliability

### 4.3.2.0.0 Priority

🔴 High

### 4.3.3.0.0 Requirements

- REQ-1-084
- REQ-1-078
- REQ-1-079
- REQ-1-045

### 4.3.4.0.0 Tactics

- Deploy the Central Management Plane across multiple AWS Availability Zones (AZs).
- Implement database replication with automated failover to a standby instance in a different AZ.
- Design the OPC Core Client with a persistent on-disk buffer for autonomous operation during network outages.
- Support redundant OPC server pairs with automatic failover at the edge.
- Implement a documented Disaster Recovery plan with cross-region replication.

## 4.4.0.0.0 Performance

### 4.4.1.0.0 Name

Performance

### 4.4.2.0.0 Priority

🔴 High

### 4.4.3.0.0 Requirements

- REQ-1-074
- REQ-1-075

### 4.4.4.0.0 Tactics

- Use gRPC for high-performance, low-latency data streaming from the edge.
- Leverage TimescaleDB hypertables and continuous aggregates for fast time-series queries.
- Perform AI inference at the edge to minimize latency.
- Use OPC UA subscriptions instead of polling to reduce network and server load.
- Implement caching for frequently accessed, slow-changing data (e.g., asset hierarchy).

## 4.5.0.0.0 Maintainability

### 4.5.1.0.0 Name

Maintainability

### 4.5.2.0.0 Priority

🟡 Medium

### 4.5.3.0.0 Requirements

- REQ-1-086
- REQ-1-093

### 4.5.4.0.0 Tactics

- Adopt a microservices architecture to create loosely coupled, independently deployable services.
- Enforce high unit test coverage (80%) in the CI/CD pipeline.
- Use containerization (Docker) for consistent deployments across environments.
- Automate infrastructure provisioning with Terraform (Infrastructure as Code).
- Document all external REST APIs using the OpenAPI specification.

