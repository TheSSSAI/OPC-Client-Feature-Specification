# 1 Integration Specifications

## 1.1 Extraction Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-DIN |
| Extraction Timestamp | 2024-07-31T12:00:00Z |
| Mapping Validation Score | 100% |
| Context Completeness Score | 100% |
| Implementation Readiness Level | High |

## 1.2 Relevant Requirements

### 1.2.1 Requirement Id

#### 1.2.1.1 Requirement Id

REQ-1-010

#### 1.2.1.2 Requirement Text

Communication between the OPC Core Client and the Central Management Plane shall be secured and optimized using a dual-protocol strategy. High-volume, low-latency data streaming shall use gRPC with mutual TLS (mTLS) authentication.

#### 1.2.1.3 Validation Criteria

- The service must expose a gRPC endpoint.
- The service must enforce mTLS for all incoming connections.
- The gRPC stream must be capable of handling high-volume time-series data.

#### 1.2.1.4 Implementation Implications

- A gRPC service must be implemented using Grpc.AspNetCore.
- The gRPC contract (.proto file) must be defined in the Shared Kernel Library (REPO-LIB-SHARED).
- The Kestrel web server must be configured to require and validate client certificates against a trusted CA.

#### 1.2.1.5 Extraction Reasoning

This requirement directly mandates the core technology (gRPC) and security model (mTLS) for this service, defining its primary and only ingress communication interface.

### 1.2.2.0 Requirement Id

#### 1.2.2.1 Requirement Id

REQ-1-075

#### 1.2.2.2 Requirement Text

The time-series database must support a sustained ingestion rate of at least 10,000 values per second for a single tenant.

#### 1.2.2.3 Validation Criteria

- The service, under load test, must sustain the specified ingestion rate without significant queue buildup or data loss.
- The database write operations must not become a bottleneck.

#### 1.2.2.4 Implementation Implications

- The data persistence logic cannot use a standard row-by-row ORM approach.
- A high-performance bulk-write mechanism, specifically the PostgreSQL `COPY` protocol via Npgsql's binary importer, is required.
- The service must implement an in-memory batching mechanism to collect data points before writing to the database to maximize throughput.

#### 1.2.2.5 Extraction Reasoning

This is the primary non-functional requirement driving the internal architecture of the service. The choice of bulk-writing to TimescaleDB is a direct consequence of this performance target.

### 1.2.3.0 Requirement Id

#### 1.2.3.1 Requirement Id

REQ-1-085

#### 1.2.3.2 Requirement Text

The system must be designed to scale to support up to 1,000 concurrent users per tenant and a total of 10,000 managed OPC Core Client instances. All cloud-based microservices must be stateless and configured to scale horizontally.

#### 1.2.3.3 Validation Criteria

- The service must not store any session state on the local file system or in memory.
- The service must be containerized and deployable as multiple replicas behind a load balancer.
- Kubernetes deployment manifests must support Horizontal Pod Autoscaling (HPA).

#### 1.2.3.4 Implementation Implications

- The service must be implemented as a stateless application.
- Any state required for data enrichment (e.g., client-to-tenant mapping) must be externalized to a distributed cache like Redis.

#### 1.2.3.5 Extraction Reasoning

This requirement dictates the operational and deployment model of the service, mandating a stateless design to ensure it can scale horizontally to meet load demands from thousands of clients.

## 1.3.0.0 Relevant Components

- {'component_name': 'Data Ingestion Service', 'component_specification': 'A stateless microservice that handles high-volume gRPC data streams from OPC Core Clients. Its responsibilities are to receive data, enrich it with tenant context, and persist it to the TimescaleDB database using a high-performance bulk-writing strategy.', 'implementation_requirements': ['Must be implemented in .NET 8 using ASP.NET Core for the gRPC host.', "Must use the Npgsql library's binary copy feature for database writes.", 'Must be containerized for deployment on Kubernetes and support horizontal scaling.'], 'architectural_context': "Belongs to the 'Application Services Layer'. Acts as the primary write-path endpoint for all time-series data originating from the edge. It is a specialized, high-throughput component.", 'extraction_reasoning': 'This is the definition of the target repository (REPO-SVC-DIN) itself, providing the foundational context for its design and implementation.'}

## 1.4.0.0 Architectural Layers

- {'layer_name': 'Application Services Layer (Microservices)', 'layer_responsibilities': 'Implements the core business logic and domain functionality of the Central Management Plane. Consists of independent, stateless services that own specific business capabilities.', 'layer_constraints': ['Services should be stateless.', 'Services must be independently deployable.', 'Services must enforce tenant data isolation.'], 'implementation_patterns': ['Domain-Driven Design (DDD)', 'gRPC for internal communication', 'Horizontal Scaling'], 'extraction_reasoning': 'The Data Ingestion Service is a key component of this layer, as explicitly listed in the architecture documentation. It must adhere to the principles and constraints of this layer, such as being stateless and scalable.'}

## 1.5.0.0 Dependency Interfaces

### 1.5.1.0 Interface Name

#### 1.5.1.1 Interface Name

TimescaleDB Hypertable Writer

#### 1.5.1.2 Source Repository

REPO-IAC-MAIN

#### 1.5.1.3 Method Contracts

- {'method_name': 'BinaryCopyImport', 'method_signature': "N/A (Uses Npgsql's NpgsqlBinaryImporter)", 'method_purpose': 'To perform a high-speed, bulk insertion of batched time-series data directly into the database, bypassing the overhead of traditional SQL INSERT statements.', 'integration_context': "This method is called when the service's internal data buffer is full or a time-based flush trigger is activated. The entire batch of data is written within a single database transaction, wrapped in a resilience policy."}

#### 1.5.1.4 Integration Pattern

Bulk-Insert via PostgreSQL COPY Protocol

#### 1.5.1.5 Communication Protocol

SQL over TCP/IP

#### 1.5.1.6 Extraction Reasoning

This is the primary downstream dependency for the service. The choice of a high-performance bulk-insert pattern is critical to meeting REQ-1-075 and is a non-negotiable part of the service's implementation. Resilience via Polly is required for production readiness.

### 1.5.2.0 Interface Name

#### 1.5.2.1 Interface Name

Redis Cache Reader

#### 1.5.2.2 Source Repository

REPO-IAC-MAIN

#### 1.5.2.3 Method Contracts

- {'method_name': 'GetClientMetadataAsync', 'method_signature': 'Task<ClientMetadata> GetClientMetadataAsync(string clientId)', 'method_purpose': 'To retrieve essential metadata for a connecting client, such as its associated `tenantId`, for data enrichment.', 'integration_context': "Called upon initial connection of a gRPC stream. The `clientId` is extracted from the client's mTLS certificate, and this metadata is used to enrich all subsequent data points in the stream."}

#### 1.5.2.4 Integration Pattern

Distributed Cache Lookup (Cache-Aside)

#### 1.5.2.5 Communication Protocol

Redis Protocol (RESP)

#### 1.5.2.6 Extraction Reasoning

To maintain statelessness and high performance, the service cannot query the primary database for enrichment on every request. This dependency on a fast, distributed cache is critical for fulfilling both the scalability requirement (REQ-1-085) and the tenant isolation requirement (REQ-1-025).

### 1.5.3.0 Interface Name

#### 1.5.3.1 Interface Name

Shared gRPC Contracts

#### 1.5.3.2 Source Repository

REPO-LIB-SHARED

#### 1.5.3.3 Method Contracts

- {'method_name': 'Consume Protocol Buffer Definitions', 'method_signature': 'N/A (NuGet Package Reference)', 'method_purpose': 'To obtain the authoritative, version-controlled contract for gRPC communication, including service and message definitions.', 'integration_context': 'The `ingestion.proto` file from this library is used at compile time with `Grpc.Tools` to generate the server-side base class that this service implements.'}

#### 1.5.3.4 Integration Pattern

Compile-Time Library Dependency

#### 1.5.3.5 Communication Protocol

N/A

#### 1.5.3.6 Extraction Reasoning

This dependency is crucial for ensuring contract consistency between the client (`REPO-EDG-OPC`) and the server (`REPO-SVC-DIN`), preventing integration failures due to mismatched data structures or service definitions.

## 1.6.0.0 Exposed Interfaces

### 1.6.1.0 Interface Name

#### 1.6.1.1 Interface Name

IIngestionService (gRPC)

#### 1.6.1.2 Consumer Repositories

- REPO-EDG-OPC

#### 1.6.1.3 Method Contracts

- {'method_name': 'StreamData', 'method_signature': 'rpc StreamData(stream DataPoint) returns (IngestAck)', 'method_purpose': 'To provide a persistent, high-performance, and network-efficient channel for edge clients to continuously stream time-series data points to the cloud.', 'implementation_requirements': "The server-side implementation must be fully asynchronous to handle concurrent streams from many clients. It must read from the client's stream, enrich data with tenant context derived from the mTLS certificate, and pass the data to an internal batching mechanism."}

#### 1.6.1.4 Service Level Requirements

- Must support a sustained ingestion rate of 10,000 values per second per tenant.
- Must be horizontally scalable to support up to 10,000 concurrent client connections.

#### 1.6.1.5 Implementation Constraints

- Communication must be secured using mutual TLS (mTLS), requiring the service to validate client certificates.
- The service must not expose any REST endpoints.
- The service must be fully stateless.

#### 1.6.1.6 Extraction Reasoning

This gRPC interface is the sole data entry point to the service. It defines the contract with its only consumer, the OPC Core Client, and is dictated by the system-level requirements for high-throughput, secure data streaming (REQ-1-010).

### 1.6.2.0 Interface Name

#### 1.6.2.1 Interface Name

IMetricsEndpoint

#### 1.6.2.2 Consumer Repositories

- Prometheus (from REPO-IAC-MAIN)

#### 1.6.2.3 Method Contracts

- {'method_name': 'GET /metrics', 'method_signature': 'HTTP GET request to /metrics', 'method_purpose': 'Exposes internal application metrics in the Prometheus text-based format for monitoring, alerting, and autoscaling.', 'implementation_requirements': "Metrics must include `grpc_requests_total`, `ingestion_batch_size`, `database_write_latency_seconds`, and `active_streams`. The endpoint must be exposed on the service's HTTP port."}

#### 1.6.2.4 Service Level Requirements

*No items available*

#### 1.6.2.5 Implementation Constraints

*No items available*

#### 1.6.2.6 Extraction Reasoning

This interface is required to fulfill the system's observability strategy (REQ-1-090) and is a standard requirement for all microservices in the architecture to integrate with the monitoring stack.

### 1.6.3.0 Interface Name

#### 1.6.3.1 Interface Name

IStructuredLogging

#### 1.6.3.2 Consumer Repositories

- Fluentd (from REPO-IAC-MAIN)

#### 1.6.3.3 Method Contracts

- {'method_name': 'Write to stdout', 'method_signature': 'N/A (Behavioral Contract)', 'method_purpose': 'Outputs structured logs (JSON) to the standard output stream for collection by a log aggregator.', 'implementation_requirements': 'All logs must be in a structured JSON format and include a timestamp, severity level, service name, `tenantId` (where applicable), and a correlation ID propagated from the gRPC request.'}

#### 1.6.3.4 Service Level Requirements

*No items available*

#### 1.6.3.5 Implementation Constraints

*No items available*

#### 1.6.3.6 Extraction Reasoning

This behavioral interface is required to fulfill the system's observability strategy (REQ-1-090) and enables centralized logging and analysis in OpenSearch.

## 1.7.0.0 Technology Context

### 1.7.1.0 Framework Requirements

The service must be built using .NET 8 and hosted with ASP.NET Core 8.0, leveraging the Grpc.AspNetCore v2.63.0 package for the gRPC implementation.

### 1.7.2.0 Integration Technologies

- Npgsql v8.0.3 for optimized PostgreSQL/TimescaleDB connectivity.
- StackExchange.Redis v2.7.33 for distributed caching.
- Polly v8.4.1 for implementing resilient database access patterns (e.g., retries, circuit breakers).
- Grpc.Tools v2.64.0 for Protobuf code generation.

### 1.7.3.0 Performance Constraints

The end-to-end data pipeline within the service must be highly optimized to sustain an ingestion rate of 10,000 values per second per tenant. This mandates the use of the PostgreSQL `COPY` protocol.

### 1.7.4.0 Security Requirements

All incoming gRPC connections must be authenticated using mutual TLS (mTLS). The service must be configured to require and validate client certificates against a trusted Certificate Authority. Credentials for database and cache access must be retrieved from AWS Secrets Manager.

## 1.8.0.0 Extraction Validation

| Property | Value |
|----------|-------|
| Mapping Completeness Check | All repository mappings for requirements, architec... |
| Cross Reference Validation | The service's role is consistent across the archit... |
| Implementation Readiness Assessment | The provided context is highly actionable. It spec... |
| Quality Assurance Confirmation | The analysis confirms a high degree of quality and... |

