# 1 Id

REPO-SVC-DIN

# 2 Name

Data Ingestion Service

# 3 Description

This microservice is a highly specialized, high-throughput component designed for one critical purpose: ingesting massive volumes of real-time time-series data from the distributed OPC Core Clients. As specified in REQ-1-010, it exposes a gRPC endpoint with mutual TLS (mTLS) for secure, low-latency, and efficient binary communication. Its sole responsibility is to receive data streams, perform minimal validation and transformation, and efficiently batch-write the data into the TimescaleDB time-series database. The service must be architected to meet the demanding performance requirement of ingesting up to 10,000 values per second per tenant (REQ-1-075) and must be horizontally scalable to handle the load from up to 10,000 client instances (REQ-NFR-005).

# 4 Type

🔹 Microservice

# 5 Namespace

System.Services.DataIngestion

# 6 Output Path

services/data-ingestion

# 7 Framework

ASP.NET Core v8.0

# 8 Language

C# 12

# 9 Technology

.NET v8.0, gRPC, TimescaleDB

# 10 Thirdparty Libraries

- Grpc.AspNetCore v2.63.0
- Npgsql v8.0.3
- Polly v8.4.1

# 11 Layer Ids

- application
- infrastructure

# 12 Dependencies

- REPO-DB-TIMESCALEDB
- REPO-LIB-SHARED

# 13 Requirements

## 13.1 Requirement Id

### 13.1.1 Requirement Id

REQ-1-010

## 13.2.0 Requirement Id

### 13.2.1 Requirement Id

REQ-1-075

## 13.3.0 Requirement Id

### 13.3.1 Requirement Id

REQ-1-085

# 14.0.0 Generate Tests

✅ Yes

# 15.0.0 Generate Documentation

✅ Yes

# 16.0.0 Architecture Style

Microservices

# 17.0.0 Architecture Map

- ingestion-service-010

# 18.0.0 Components Map

- ingestion-service-010

# 19.0.0 Requirements Map

- REQ-ARC-001
- REQ-NFR-001
- REQ-NFR-005

# 20.0.0 Dependency Contracts

## 20.1.0 Repo-Db-Timescaledb

### 20.1.1 Required Interfaces

- {'interface': 'TimescaleDB Hypertable (TagDataPoint)', 'methods': ['High-performance bulk-copy/insert operations.'], 'events': [], 'properties': ['TagDataPoint Hypertable']}

### 20.1.2 Integration Pattern

Bulk-Insert via Npgsql's Binary Copy feature

### 20.1.3 Communication Protocol

SQL over TCP/IP

# 21.0.0 Exposed Contracts

## 21.1.0 Public Interfaces

- {'interface': 'IIngestionService (gRPC)', 'methods': ['rpc StreamData(stream DataPoint) returns (IngestAck)'], 'events': [], 'properties': [], 'consumers': ['REPO-EDG-OPC']}

# 22.0.0 Integration Patterns

| Property | Value |
|----------|-------|
| Dependency Injection | Constructor Injection via .NET's DI container. |
| Event Communication | N/A |
| Data Flow | Bi-directional gRPC stream. |
| Error Handling | Return gRPC status codes to the client on failure.... |
| Async Patterns | Full async pipeline from gRPC stream reading to da... |

# 23.0.0 Scope Boundaries

## 23.1.0 Must Implement

- Expose a secure gRPC endpoint for data streaming.
- Authenticate clients via mTLS certificates.
- Receive, decode, and batch Process Protocol Buffer messages.
- Perform high-performance bulk writes to TimescaleDB.
- Be fully stateless for horizontal scaling.

## 23.2.0 Must Not Implement

- Expose any REST APIs.
- Handle command and control logic (this is MQTT).
- Perform complex business logic or data enrichment.
- Query any data.

## 23.3.0 Integration Points

- Listens for incoming gRPC connections from OPC Core Clients.
- Connects to the TimescaleDB cluster.

## 23.4.0 Architectural Constraints

- Must use gRPC, not REST, for its primary interface.
- The data writing process must be heavily optimized for throughput.

# 24.0.0 Technology Standards

## 24.1.0 Framework Specific

| Property | Value |
|----------|-------|
| Pattern Usage | Implement the gRPC service using Grpc.AspNetCore. ... |
| Performance Requirements | Must sustain 10,000 writes/sec/tenant. |
| Security Requirements | Must enforce mTLS for all client connections. |

# 25.0.0 Cognitive Load Instructions

## 25.1.0 Sds Generation Guidance

### 25.1.1 Focus Areas

- The Protocol Buffer schema definition for the data stream.
- The high-performance batching and database writing strategy.
- Scalability design using Kubernetes HPA.

### 25.1.2 Avoid Patterns

- Writing data row-by-row to the database.
- Adding any synchronous dependencies to other services.

## 25.2.0 Code Generation Guidance

### 25.2.1 Implementation Patterns

- Generate C# gRPC stubs from the `.proto` file.
- Use concurrent collections and background tasks to implement the batching mechanism.

