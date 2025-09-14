# 1 System Overview

## 1.1 Analysis Date

2025-06-13

## 1.2 Technology Stack

- .NET 8
- React 18
- PostgreSQL 16
- TimescaleDB
- Redis 7
- gRPC
- REST
- MQTT v5
- Docker
- Kubernetes (EKS)

## 1.3 Service Interfaces

- REST API (OpenAPI)
- gRPC Service
- MQTT Topic

## 1.4 Data Models

- Tenant
- User
- Asset
- OpcTag
- TagDataPoint
- Alarm
- AiModel

# 2.0 Data Mapping Strategy

## 2.1 Essential Mappings

### 2.1.1 Mapping Id

#### 2.1.1.1 Mapping Id

MAPPING-001

#### 2.1.1.2 Source

OPC Core Client (gRPC Stream)

#### 2.1.1.3 Target

Data Ingestion Service (TagDataPoint Model)

#### 2.1.1.4 Transformation

direct

#### 2.1.1.5 Configuration

*No data available*

#### 2.1.1.6 Mapping Technique

Direct mapping of gRPC message fields to TagDataPoint entity properties.

#### 2.1.1.7 Justification

Core real-time data flow from edge to cloud as per REQ-1-010. Requires high performance and low latency for up to 10,000 values/sec per tenant (REQ-1-075).

#### 2.1.1.8 Complexity

simple

### 2.1.2.0 Mapping Id

#### 2.1.2.1 Mapping Id

MAPPING-002

#### 2.1.2.2 Source

Legacy System (CSV/JSON File)

#### 2.1.2.3 Target

System Entities (Asset, OpcTag, User)

#### 2.1.2.4 Transformation

nested

#### 2.1.2.5 Configuration

| Property | Value |
|----------|-------|
| File Format | CSV/JSON |
| Delimiters | , |
| Header Mapping | ✅ |

#### 2.1.2.6 Mapping Technique

Parsing flat file rows and mapping them to the system's hierarchical Asset model and related entities.

#### 2.1.2.7 Justification

Required for data migration from legacy systems as specified in REQ-1-069 and REQ-1-097.

#### 2.1.2.8 Complexity

medium

### 2.1.3.0 Mapping Id

#### 2.1.3.1 Mapping Id

MAPPING-003

#### 2.1.3.2 Source

System OPC Tag Data

#### 2.1.3.3 Target

External IoT Platform Schema (e.g., AWS IoT)

#### 2.1.3.4 Transformation

custom

#### 2.1.3.5 Configuration

##### 2.1.3.5.1 Target Schema

JSON

##### 2.1.3.5.2 Field Mapping Rules

User-configurable via UI

#### 2.1.3.6.0 Mapping Technique

User-defined mapping and transformation tool to align system data with external platform schemas.

#### 2.1.3.7.0 Justification

Explicitly required by REQ-1-057 for integration with major IoT platforms.

#### 2.1.3.8.0 Complexity

complex

## 2.2.0.0.0 Object To Object Mappings

- {'sourceObject': 'Database Entity (e.g., User, Asset)', 'targetObject': 'API DTO (e.g., UserDto, AssetDto)', 'fieldMappings': [{'sourceField': 'All relevant fields', 'targetField': 'Corresponding fields', 'transformation': 'Direct mapping, potential flattening of related entities.', 'dataTypeConversion': 'None'}]}

## 2.3.0.0.0 Data Type Conversions

### 2.3.1.0.0 From

#### 2.3.1.1.0 From

OPC Data Quality Flag (Integer/Enum)

#### 2.3.1.2.0 To

TagDataPoint.quality (VARCHAR)

#### 2.3.1.3.0 Conversion Method

Lookup or switch statement to convert OPC quality codes to 'Good', 'Bad', 'Uncertain' strings.

#### 2.3.1.4.0 Validation Required

✅ Yes

### 2.3.2.0.0 From

#### 2.3.2.1.0 From

Boolean (0/1)

#### 2.3.2.2.0 To

String ('OFF'/'ON')

#### 2.3.2.3.0 Conversion Method

Conditional mapping.

#### 2.3.2.4.0 Validation Required

❌ No

## 2.4.0.0.0 Bidirectional Mappings

- {'entity': 'OPC Tag Value', 'forwardMapping': 'MAPPING-003 (System to IoT Platform)', 'reverseMapping': 'IoT Platform Message to System OPC Tag Write Command', 'consistencyStrategy': 'Last-write-wins. The system must be able to transform a message from a cloud topic back into a value to be written to an OPC tag.'}

# 3.0.0.0.0 Schema Validation Requirements

## 3.1.0.0.0 Field Level Validations

### 3.1.1.0.0 Field

#### 3.1.1.1.0 Field

User.email

#### 3.1.1.2.0 Rules

- not-null
- valid-email-format

#### 3.1.1.3.0 Priority

🚨 critical

#### 3.1.1.4.0 Error Message

A valid email address is required.

### 3.1.2.0.0 Field

#### 3.1.2.1.0 Field

OpcServerConnection.endpointUrl

#### 3.1.2.2.0 Rules

- not-null
- valid-uri-or-ip

#### 3.1.2.3.0 Priority

🚨 critical

#### 3.1.2.4.0 Error Message

A valid OPC server endpoint URL or IP address is required.

## 3.2.0.0.0 Cross Field Validations

- {'validationId': 'VALIDATE-REDUNDANCY-001', 'fields': ['OpcServerConnection.isRedundantPair', 'OpcServerConnection.backupServerConnectionId'], 'rule': 'If isRedundantPair is true, then backupServerConnectionId must not be null.', 'condition': 'During creation or update of an OpcServerConnection.', 'errorHandling': 'Reject the request with a 400 Bad Request status.'}

## 3.3.0.0.0 Business Rule Validations

- {'ruleId': 'VALIDATE-OEE-TAGS-001', 'description': 'Ensures that an asset configured for OEE calculation has all required OPC tags mapped to it.', 'fields': ['Asset.properties', 'OpcTag'], 'logic': "For an asset with 'calculatesOEE' property set to true, verify that tags for 'GoodCount', 'TotalCount', and 'RunTime' are associated with it.", 'priority': 'high'}

## 3.4.0.0.0 Conditional Validations

- {'condition': 'User has a role scoped to a specific asset (e.g., UserRole.assetScopeId is not null).', 'applicableFields': ['Alarm.assetId'], 'validationRules': ["The assetId of the target entity (e.g., an alarm being acknowledged) must be within the user's permitted asset scope."]}

## 3.5.0.0.0 Validation Groups

- {'groupName': 'NewTenantOnboarding', 'validations': ['Tenant.name', 'User.email', 'License.licenseKey'], 'executionOrder': 1, 'stopOnFirstFailure': True}

# 4.0.0.0.0 Transformation Pattern Evaluation

## 4.1.0.0.0 Selected Patterns

### 4.1.1.0.0 Pattern

#### 4.1.1.1.0 Pattern

adapter

#### 4.1.1.2.0 Use Case

Connecting to external IoT platforms (REQ-1-057) and legacy systems (REQ-1-069).

#### 4.1.1.3.0 Implementation

A dedicated service or module responsible for mediating between the system's canonical data model and the external system's schema.

#### 4.1.1.4.0 Justification

Decouples the core system from the specifics of external integrations, allowing new platforms to be supported without core code changes.

### 4.1.2.0.0 Pattern

#### 4.1.2.1.0 Pattern

pipeline

#### 4.1.2.2.0 Use Case

Real-time data ingestion from OPC Core Clients.

#### 4.1.2.3.0 Implementation

A sequence of steps in the Data Ingestion Service: gRPC endpoint -> deserialization -> validation -> transformation to TagDataPoint -> batching -> TimescaleDB insertion.

#### 4.1.2.4.0 Justification

Provides a structured, high-performance approach to handling the high-volume data stream required by REQ-1-075.

## 4.2.0.0.0 Pipeline Processing

### 4.2.1.0.0 Required

✅ Yes

### 4.2.2.0.0 Stages

#### 4.2.2.1.0 Stage

##### 4.2.2.1.1 Stage

Ingest

##### 4.2.2.1.2 Transformation

gRPC message to internal object

##### 4.2.2.1.3 Dependencies

*No items available*

#### 4.2.2.2.0 Stage

##### 4.2.2.2.1 Stage

Validate

##### 4.2.2.2.2 Transformation

Check for required fields and data integrity

##### 4.2.2.2.3 Dependencies

- Ingest

#### 4.2.2.3.0 Stage

##### 4.2.2.3.1 Stage

Enrich

##### 4.2.2.3.2 Transformation

Add tenantId and assetId based on opcTagId

##### 4.2.2.3.3 Dependencies

- Validate

#### 4.2.2.4.0 Stage

##### 4.2.2.4.1 Stage

Persist

##### 4.2.2.4.2 Transformation

Batch insert into TimescaleDB

##### 4.2.2.4.3 Dependencies

- Enrich

### 4.2.3.0.0 Parallelization

✅ Yes

## 4.3.0.0.0 Processing Mode

### 4.3.1.0.0 Real Time

#### 4.3.1.1.0 Required

✅ Yes

#### 4.3.1.2.0 Scenarios

- Data ingestion and visualization
- Edge AI anomaly detection

#### 4.3.1.3.0 Latency Requirements

<500ms for visualization, <100ms for edge inference (REQ-1-074, REQ-1-075).

### 4.3.2.0.0 Batch

| Property | Value |
|----------|-------|
| Required | ✅ |
| Batch Size | 1000 |
| Frequency | On-demand (migration) or scheduled (reporting) |

### 4.3.3.0.0 Streaming

| Property | Value |
|----------|-------|
| Required | ✅ |
| Streaming Framework | gRPC |
| Windowing Strategy | Not applicable for direct ingestion; windowing wou... |

## 4.4.0.0.0 Canonical Data Model

### 4.4.1.0.0 Applicable

✅ Yes

### 4.4.2.0.0 Scope

- Asset
- OpcTag
- TagDataPoint

### 4.4.3.0.0 Benefits

- Decouples internal services from the specifics of OPC protocols.
- Provides a consistent data structure for all internal processing, reporting, and external integrations.

# 5.0.0.0.0 Version Handling Strategy

## 5.1.0.0.0 Schema Evolution

### 5.1.1.0.0 Strategy

Additive changes with version identifiers.

### 5.1.2.0.0 Versioning Scheme

URI Path Versioning (e.g., /api/v1/...) for REST APIs.

### 5.1.3.0.0 Compatibility

| Property | Value |
|----------|-------|
| Backward | ✅ |
| Forward | ❌ |
| Reasoning | REQ-1-028 mandates API versioning to ensure backwa... |

## 5.2.0.0.0 Transformation Versioning

| Property | Value |
|----------|-------|
| Mechanism | Transformations are versioned alongside the code. ... |
| Version Identification | Git commit hash. |
| Migration Strategy | Blue-green or canary deployments for backend servi... |

## 5.3.0.0.0 Data Model Changes

| Property | Value |
|----------|-------|
| Migration Path | Use a database migration tool (e.g., Entity Framew... |
| Rollback Strategy | Database migrations include 'down' scripts to reve... |
| Validation Strategy | Run integration tests against the migrated schema ... |

## 5.4.0.0.0 Schema Registry

| Property | Value |
|----------|-------|
| Required | ❌ |
| Technology | N/A |
| Governance | API contracts are managed via OpenAPI specificatio... |

# 6.0.0.0.0 Performance Optimization

## 6.1.0.0.0 Critical Requirements

### 6.1.1.0.0 Operation

#### 6.1.1.1.0 Operation

Time-series data ingestion

#### 6.1.1.2.0 Max Latency

N/A

#### 6.1.1.3.0 Throughput Target

10,000 values/sec/tenant

#### 6.1.1.4.0 Justification

REQ-1-075

### 6.1.2.0.0 Operation

#### 6.1.2.1.0 Operation

Real-time data visualization

#### 6.1.2.2.0 Max Latency

500ms (end-to-end)

#### 6.1.2.3.0 Throughput Target

N/A

#### 6.1.2.4.0 Justification

REQ-1-074

### 6.1.3.0.0 Operation

#### 6.1.3.1.0 Operation

API response time

#### 6.1.3.2.0 Max Latency

200ms (P95)

#### 6.1.3.3.0 Throughput Target

N/A

#### 6.1.3.4.0 Justification

REQ-1-074

## 6.2.0.0.0 Parallelization Opportunities

- {'transformation': 'Bulk data import from CSV/JSON (MAPPING-002)', 'parallelizationStrategy': 'Split the input file into chunks and process them in parallel threads or workers.', 'expectedGain': 'Significant reduction in total migration time for large datasets.'}

## 6.3.0.0.0 Caching Strategies

- {'cacheType': 'In-memory (Redis)', 'cacheScope': 'Per-tenant', 'evictionPolicy': 'LRU (Least Recently Used)', 'applicableTransformations': ['Enrichment stage of ingestion pipeline (caching asset/tag metadata)']}

## 6.4.0.0.0 Memory Optimization

### 6.4.1.0.0 Techniques

- Use of data streaming for large file processing to avoid loading entire files into memory.
- Efficient object pooling for gRPC message handling in the Data Ingestion Service.

### 6.4.2.0.0 Thresholds

Container memory limits defined in Kubernetes.

### 6.4.3.0.0 Monitoring Required

✅ Yes

## 6.5.0.0.0 Lazy Evaluation

### 6.5.1.0.0 Applicable

❌ No

### 6.5.2.0.0 Scenarios

*No items available*

### 6.5.3.0.0 Implementation

Not a primary optimization strategy for the identified critical paths.

## 6.6.0.0.0 Bulk Processing

### 6.6.1.0.0 Required

✅ Yes

### 6.6.2.0.0 Batch Sizes

#### 6.6.2.1.0 Optimal

1,000

#### 6.6.2.2.0 Maximum

5,000

### 6.6.3.0.0 Parallelism

4

# 7.0.0.0.0 Error Handling And Recovery

## 7.1.0.0.0 Error Handling Strategies

### 7.1.1.0.0 Error Type

#### 7.1.1.1.0 Error Type

Data ingestion failure (transient DB error)

#### 7.1.1.2.0 Strategy

Retry with exponential backoff

#### 7.1.1.3.0 Fallback Action

Log error and move message to a dead-letter queue.

#### 7.1.1.4.0 Escalation Path

- Alertmanager
- PagerDuty

### 7.1.2.0.0 Error Type

#### 7.1.2.1.0 Error Type

OPC Client buffer full

#### 7.1.2.2.0 Strategy

Overwrite oldest data (circular buffer)

#### 7.1.2.3.0 Fallback Action

Log a critical alert to the Central Management Plane.

#### 7.1.2.4.0 Escalation Path

- Central Management Plane UI Alert

## 7.2.0.0.0 Logging Requirements

### 7.2.1.0.0 Log Level

info

### 7.2.2.0.0 Included Data

- timestamp
- severity
- service name
- correlation ID
- error message
- stack trace (on error)

### 7.2.3.0.0 Retention Period

7 years for audit logs, 30 days for application logs.

### 7.2.4.0.0 Alerting

✅ Yes

## 7.3.0.0.0 Partial Success Handling

### 7.3.1.0.0 Strategy

For bulk imports, process valid rows and report errors on invalid rows in a summary file.

### 7.3.2.0.0 Reporting Mechanism

Generate a result file listing successfully imported records and failed records with error reasons.

### 7.3.3.0.0 Recovery Actions

- User corrects the failed records in the source file and re-uploads.

## 7.4.0.0.0 Circuit Breaking

- {'dependency': 'Central Management Plane (from OPC Core Client)', 'threshold': '5 consecutive connection failures', 'timeout': '30s', 'fallbackStrategy': 'Enter autonomous buffering mode (REQ-1-079).'}

## 7.5.0.0.0 Retry Strategies

- {'operation': 'OPC Core Client connection to cloud', 'maxRetries': 10, 'backoffStrategy': 'exponential', 'retryConditions': ['Network timeout', 'Transient server errors (5xx)']}

## 7.6.0.0.0 Error Notifications

- {'condition': 'Failed data migration batch', 'recipients': ['Administrator'], 'severity': 'high', 'channel': 'Email, In-App Notification'}

# 8.0.0.0.0 Project Specific Transformations

## 8.1.0.0.0 OPC Raw Data to TimescaleDB Format

### 8.1.1.0.0 Transformation Id

PST-001

### 8.1.2.0.0 Name

OPC Raw Data to TimescaleDB Format

### 8.1.3.0.0 Description

Transforms real-time gRPC data stream from edge clients into the structured TagDataPoint model for persistence in TimescaleDB.

### 8.1.4.0.0 Source

#### 8.1.4.1.0 Service

OPC Core Client

#### 8.1.4.2.0 Model

gRPC DataPoint Message

#### 8.1.4.3.0 Fields

- opcTagId
- timestamp
- value
- qualityCode

### 8.1.5.0.0 Target

#### 8.1.5.1.0 Service

Data Ingestion Service

#### 8.1.5.2.0 Model

TagDataPoint

#### 8.1.5.3.0 Fields

- opcTagId
- timestamp
- value
- quality
- tenantId
- assetId

### 8.1.6.0.0 Transformation

#### 8.1.6.1.0 Type

🔹 custom

#### 8.1.6.2.0 Logic

Directly map value and timestamp. Convert qualityCode to quality string. Enrich with tenantId and assetId by looking up opcTagId metadata (from cache or DB).

#### 8.1.6.3.0 Configuration

*No data available*

### 8.1.7.0.0 Frequency

real-time

### 8.1.8.0.0 Criticality

critical

### 8.1.9.0.0 Dependencies

- MAPPING-001

### 8.1.10.0.0 Validation

#### 8.1.10.1.0 Pre Transformation

- Ensure opcTagId is valid and exists in the system.

#### 8.1.10.2.0 Post Transformation

- Verify all target fields are non-null.

### 8.1.11.0.0 Performance

| Property | Value |
|----------|-------|
| Expected Volume | Up to 10,000 records/sec/tenant |
| Latency Requirement | < 50ms for the transformation logic |
| Optimization Strategy | Batch inserts, caching of tag metadata. |

## 8.2.0.0.0 Legacy CSV/JSON to System Entities

### 8.2.1.0.0 Transformation Id

PST-002

### 8.2.2.0.0 Name

Legacy CSV/JSON to System Entities

### 8.2.3.0.0 Description

Transforms flat-file data from legacy systems into the system's relational and hierarchical data models for bulk import.

### 8.2.4.0.0 Source

#### 8.2.4.1.0 Service

File System / S3

#### 8.2.4.2.0 Model

CSV/JSON File

#### 8.2.4.3.0 Fields

- asset_name
- parent_asset
- tag_name
- node_id
- user_email

### 8.2.5.0.0 Target

#### 8.2.5.1.0 Service

Asset & Topology Service, Identity & Access Management Service

#### 8.2.5.2.0 Model

Asset, OpcTag, User

#### 8.2.5.3.0 Fields

- Asset.name
- Asset.parentAssetId
- OpcTag.name
- User.email

### 8.2.6.0.0 Transformation

#### 8.2.6.1.0 Type

🔹 nested

#### 8.2.6.2.0 Logic

Parse each row. Look up parent_asset to establish asset hierarchy. Create Asset, OpcTag, and User entities and link them via foreign keys.

#### 8.2.6.3.0 Configuration

*No data available*

### 8.2.7.0.0 Frequency

batch

### 8.2.8.0.0 Criticality

high

### 8.2.9.0.0 Dependencies

- MAPPING-002
- REQ-1-069

### 8.2.10.0.0 Validation

#### 8.2.10.1.0 Pre Transformation

- Validate file format and headers.

#### 8.2.10.2.0 Post Transformation

- Check for duplicates. Ensure all parent assets exist.

### 8.2.11.0.0 Performance

| Property | Value |
|----------|-------|
| Expected Volume | Up to 100,000 records per file |
| Latency Requirement | N/A (batch job) |
| Optimization Strategy | Parallel processing of file chunks, bulk database ... |

# 9.0.0.0.0 Implementation Priority

## 9.1.0.0.0 Component

### 9.1.1.0.0 Component

PST-001: OPC Raw Data Ingestion Transformation

### 9.1.2.0.0 Priority

🔴 high

### 9.1.3.0.0 Dependencies

- Data Ingestion Service
- TimescaleDB Schema

### 9.1.4.0.0 Estimated Effort

Medium

### 9.1.5.0.0 Risk Level

high

## 9.2.0.0.0 Component

### 9.2.1.0.0 Component

PST-002: Legacy Data Migration Transformation

### 9.2.2.0.0 Priority

🟡 medium

### 9.2.3.0.0 Dependencies

- Asset & Topology Service
- Database Schema

### 9.2.4.0.0 Estimated Effort

Medium

### 9.2.5.0.0 Risk Level

medium

## 9.3.0.0.0 Component

### 9.3.1.0.0 Component

MAPPING-003: IoT Platform Integration Adapter

### 9.3.2.0.0 Priority

🟢 low

### 9.3.3.0.0 Dependencies

- Query & Analytics Service

### 9.3.4.0.0 Estimated Effort

High

### 9.3.5.0.0 Risk Level

medium

# 10.0.0.0.0 Risk Assessment

## 10.1.0.0.0 Risk

### 10.1.1.0.0 Risk

Data ingestion pipeline cannot meet the 10,000 values/sec throughput target.

### 10.1.2.0.0 Impact

high

### 10.1.3.0.0 Probability

medium

### 10.1.4.0.0 Mitigation

Conduct early and continuous load testing of the PST-001 transformation. Optimize database batch insert sizes and leverage metadata caching heavily.

### 10.1.5.0.0 Contingency Plan

Horizontally scale the Data Ingestion Service pods. Use a more powerful database instance.

## 10.2.0.0.0 Risk

### 10.2.1.0.0 Risk

Complex or inconsistent data from legacy systems makes the PST-002 migration transformation unreliable.

### 10.2.2.0.0 Impact

medium

### 10.2.3.0.0 Probability

high

### 10.2.4.0.0 Mitigation

Establish a strict data format for import files and provide a pre-validation tool for customers to check their data before uploading.

### 10.2.5.0.0 Contingency Plan

Provide professional services for data cleansing and migration as a fallback.

# 11.0.0.0.0 Recommendations

## 11.1.0.0.0 Category

### 11.1.1.0.0 Category

🔹 Implementation

### 11.1.2.0.0 Recommendation

Use a dedicated library like AutoMapper for internal .NET object-to-object mappings (Entity to DTO).

### 11.1.3.0.0 Justification

Standardizes mapping logic, reduces boilerplate code, and improves maintainability.

### 11.1.4.0.0 Priority

🔴 high

### 11.1.5.0.0 Implementation Notes

Define mapping profiles at application startup to ensure consistency.

## 11.2.0.0.0 Category

### 11.2.1.0.0 Category

🔹 Performance

### 11.2.2.0.0 Recommendation

Implement and aggressively test the Redis caching strategy for tag and asset metadata early in the development cycle.

### 11.2.3.0.0 Justification

The data enrichment step in the ingestion pipeline (PST-001) is a likely bottleneck. Caching is critical to meeting the throughput requirement of REQ-1-075.

### 11.2.4.0.0 Priority

🔴 high

### 11.2.5.0.0 Implementation Notes

Monitor cache hit/miss ratio to ensure effectiveness.

## 11.3.0.0.0 Category

### 11.3.1.0.0 Category

🔹 Design

### 11.3.2.0.0 Recommendation

Design the user-configurable transformation tool for IoT integration (REQ-1-057) using a simple, rule-based engine.

### 11.3.3.0.0 Justification

Avoids over-engineering a complex scripting solution. A UI-driven, rule-based approach (e.g., 'map source field A to target field X', 'if value is 0, output OFF') will cover most use cases and be more user-friendly.

### 11.3.4.0.0 Priority

🟡 medium

### 11.3.5.0.0 Implementation Notes

Store user-defined rules as JSON in the database.

