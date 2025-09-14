# 1 Id

REPO-SVC-DVM

# 2 Name

Device Management Service

# 3 Description

This microservice is responsible for the complete lifecycle and fleet management of the distributed OPC Core Client instances. It provides functionalities for secure client provisioning using one-time tokens (REQ-1-082), remote health monitoring (CPU, memory, connection status), and centralized management via the web dashboard (REQ-1-062). It handles the pushing of configuration updates and the deployment of software updates to clients, utilizing the MQTT-based command and control channel for robust, asynchronous communication (REQ-1-010). This service acts as the bridge between the central management plane and the fleet of edge devices, ensuring they are healthy, secure, and correctly configured.

# 4 Type

🔹 Microservice

# 5 Namespace

System.Services.DeviceManagement

# 6 Output Path

services/device-management

# 7 Framework

ASP.NET Core v8.0

# 8 Language

C# 12

# 9 Technology

.NET v8.0, ASP.NET Core v8.0, EF Core v8.0.6, PostgreSQL, MQTT

# 10 Thirdparty Libraries

- MQTTnet v4.3.6
- Npgsql.EntityFrameworkCore.PostgreSQL v8.0.4

# 11 Layer Ids

- application
- domain

# 12 Dependencies

- REPO-DB-POSTGRES
- REPO-MSG-MQTT
- REPO-LIB-SHARED

# 13 Requirements

## 13.1 Requirement Id

### 13.1.1 Requirement Id

REQ-1-062

## 13.2.0 Requirement Id

### 13.2.1 Requirement Id

REQ-1-082

## 13.3.0 Requirement Id

### 13.3.1 Requirement Id

REQ-1-010

# 14.0.0 Generate Tests

✅ Yes

# 15.0.0 Generate Documentation

✅ Yes

# 16.0.0 Architecture Style

Microservices

# 17.0.0 Architecture Map

- device-service-004

# 18.0.0 Components Map

- device-service-004

# 19.0.0 Requirements Map

- REQ-BIZ-002
- REQ-NFR-003
- REQ-ARC-001
- REQ-BIZ-004

# 20.0.0 Dependency Contracts

## 20.1.0 Repo-Db-Postgres

### 20.1.1 Required Interfaces

- {'interface': 'EF Core DbContext (Device)', 'methods': ['DbSet<OpcCoreClient>.FindAsync()', 'DbSet<OpcServerConnection>.Where()'], 'events': [], 'properties': ['DbSet<OpcCoreClient>', 'DbSet<OpcServerConnection>']}

### 20.1.2 Integration Pattern

Repository Pattern over EF Core

### 20.1.3 Communication Protocol

SQL over TCP/IP

## 20.2.0 Repo-Msg-Mqtt

### 20.2.1 Required Interfaces

- {'interface': 'IMqttClient', 'methods': ['PublishAsync(MqttApplicationMessage message)', 'SubscribeAsync(MqttTopicFilter topicFilter)'], 'events': ['ApplicationMessageReceivedAsync'], 'properties': []}

### 20.2.2 Integration Pattern

Publish-Subscribe Client

### 20.2.3 Communication Protocol

MQTT v5 over TLS

# 21.0.0 Exposed Contracts

## 21.1.0 Public Interfaces

### 21.1.1 Interface

#### 21.1.1.1 Interface

IDeviceManagementService (REST API)

#### 21.1.1.2 Methods

- GET /api/v1/clients
- POST /api/v1/clients/provision-token
- POST /api/v1/clients/{id}/update-config

#### 21.1.1.3 Events

*No items available*

#### 21.1.1.4 Properties

*No items available*

#### 21.1.1.5 Consumers

- REPO-GW-API

### 21.1.2.0 Interface

#### 21.1.2.1 Interface

MQTT Command Publisher

#### 21.1.2.2 Methods

- Publishes commands to tenants/{tenantId}/clients/{clientId}/commands/...

#### 21.1.2.3 Events

*No items available*

#### 21.1.2.4 Properties

*No items available*

#### 21.1.2.5 Consumers

- REPO-EDG-OPC

# 22.0.0.0 Integration Patterns

| Property | Value |
|----------|-------|
| Dependency Injection | Constructor Injection via .NET's DI container. |
| Event Communication | Subscribes to client status topics and publishes c... |
| Data Flow | REST API for user-initiated actions, MQTT for asyn... |
| Error Handling | Use MQTT QoS 1 for guaranteed-at-least-once comman... |
| Async Patterns | Background service (IHostedService) for managing M... |

# 23.0.0.0 Scope Boundaries

## 23.1.0.0 Must Implement

- CRUD for OPC Core Client registrations.
- Generation and management of single-use provisioning tokens.
- Publishing configuration and software update commands to clients via MQTT.
- Processing and persisting incoming client health and status updates from MQTT.
- Providing API endpoints for the UI to manage and monitor the client fleet.

## 23.2.0.0 Must Not Implement

- Directly handle high-volume time-series data (this is gRPC to Data Ingestion).
- Manage asset or tag configuration (this is Asset & Topology service).
- Store software update packages (these are in S3, this service only points to them).

## 23.3.0.0 Integration Points

- PostgreSQL for device state.
- MQTT broker for command/control and status communication.

## 23.4.0.0 Architectural Constraints

- Must be stateless.
- Must maintain a persistent connection to the MQTT broker.

# 24.0.0.0 Technology Standards

## 24.1.0.0 Framework Specific

| Property | Value |
|----------|-------|
| Pattern Usage | Use ASP.NET Core for the REST API and a Background... |
| Performance Requirements | Must be able to handle status updates from up to 1... |
| Security Requirements | Must validate client identity from MQTT messages b... |

# 25.0.0.0 Cognitive Load Instructions

## 25.1.0.0 Sds Generation Guidance

### 25.1.1.0 Focus Areas

- Defining the MQTT topic structure for commands and status.
- Payload schemas for all MQTT messages.
- Database schema for OpcCoreClient and its configuration.
- Secure client provisioning workflow.

### 25.1.2.0 Avoid Patterns

- Using synchronous communication for device management.

## 25.2.0.0 Code Generation Guidance

### 25.2.1.0 Implementation Patterns

- Use the MQTTnet library for client implementation.
- Implement a robust topic parsing and validation logic.

