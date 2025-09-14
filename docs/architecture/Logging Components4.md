# 1 System Overview

## 1.1 Analysis Date

2025-06-13

## 1.2 Technology Stack

- .NET 8
- React 18
- Kubernetes (EKS)
- Docker
- PostgreSQL/TimescaleDB
- gRPC
- MQTT v5

## 1.3 Monitoring Requirements

- REQ-1-090
- REQ-1-091
- REQ-1-074
- REQ-1-062

## 1.4 System Architecture

Microservices with Edge Computing

## 1.5 Environment

production

# 2.0 Log Level And Category Strategy

## 2.1 Default Log Level

INFO

## 2.2 Environment Specific Levels

### 2.2.1 Environment

#### 2.2.1.1 Environment

development

#### 2.2.1.2 Log Level

DEBUG

#### 2.2.1.3 Justification

Provides detailed information for developers during feature implementation and troubleshooting.

### 2.2.2.0 Environment

#### 2.2.2.1 Environment

staging

#### 2.2.2.2 Log Level

DEBUG

#### 2.2.2.3 Justification

Facilitates in-depth analysis during UAT and performance testing phases.

## 2.3.0.0 Component Categories

### 2.3.1.0 Component

#### 2.3.1.1 Component

opc-core-client

#### 2.3.1.2 Category

🔹 Connectivity

#### 2.3.1.3 Log Level

INFO

#### 2.3.1.4 Verbose Logging

✅ Yes

#### 2.3.1.5 Justification

Core functionality for data acquisition; requires dynamic log level changes to DEBUG for troubleshooting connection issues without redeployment.

### 2.3.2.0 Component

#### 2.3.2.1 Component

ingestion-service

#### 2.3.2.2 Category

🔹 DataIngestion

#### 2.3.2.3 Log Level

INFO

#### 2.3.2.4 Verbose Logging

✅ Yes

#### 2.3.2.5 Justification

High-throughput service critical for data flow; requires dynamic log level changes to DEBUG for performance analysis.

### 2.3.3.0 Component

#### 2.3.3.1 Component

All Other Microservices

#### 2.3.3.2 Category

🔹 General

#### 2.3.3.3 Log Level

INFO

#### 2.3.3.4 Verbose Logging

❌ No

#### 2.3.3.5 Justification

Standard logging for business logic, API requests, and database interactions.

## 2.4.0.0 Sampling Strategies

- {'component': 'ingestion-service', 'samplingRate': '1%', 'condition': 'Log level is INFO and event represents a successful data batch ingestion.', 'reason': 'Reduces log volume and cost given the high data rate (REQ-1-075) while retaining 100% of WARN/ERROR level logs for critical issue analysis.'}

## 2.5.0.0 Logging Approach

### 2.5.1.0 Structured

✅ Yes

### 2.5.2.0 Format

JSON

### 2.5.3.0 Standard Fields

- timestamp
- severity
- service_name
- correlation_id
- traceId
- spanId
- message
- stack_trace

### 2.5.4.0 Custom Fields

- tenantId
- userId
- assetId
- opcClientId

# 3.0.0.0 Log Aggregation Architecture

## 3.1.0.0 Collection Mechanism

### 3.1.1.0 Type

🔹 agent

### 3.1.2.0 Technology

Fluentd

### 3.1.3.0 Configuration

#### 3.1.3.1 Kubernetes

Run as a DaemonSet to collect logs from all pods on a node.

#### 3.1.3.2 Edge

Run as a sidecar or host-level agent to collect logs from the OPC Core Client container.

### 3.1.4.0 Justification

Mandated by REQ-1-090 for centralized log aggregation.

## 3.2.0.0 Strategy

| Property | Value |
|----------|-------|
| Approach | centralized |
| Reasoning | A central OpenSearch cluster is required by REQ-1-... |
| Local Retention | Buffer up to 24 hours on edge devices to handle ne... |

## 3.3.0.0 Shipping Methods

- {'protocol': 'HTTP', 'destination': 'OpenSearch Cluster', 'reliability': 'at-least-once', 'compression': True}

## 3.4.0.0 Buffering And Batching

| Property | Value |
|----------|-------|
| Buffer Size | 1GB on edge, 512MB in cloud |
| Batch Size | 1000 |
| Flush Interval | 5s |
| Backpressure Handling | On-disk buffering |

## 3.5.0.0 Transformation And Enrichment

### 3.5.1.0 Transformation

#### 3.5.1.1 Transformation

Add Kubernetes Metadata

#### 3.5.1.2 Purpose

Enrich logs with pod name, namespace, and labels for improved context.

#### 3.5.1.3 Stage

collection

### 3.5.2.0 Transformation

#### 3.5.2.1 Transformation

Parse JSON Message

#### 3.5.2.2 Purpose

Ensure nested JSON within the log message is correctly parsed and indexed.

#### 3.5.2.3 Stage

transport

## 3.6.0.0 High Availability

| Property | Value |
|----------|-------|
| Required | ✅ |
| Redundancy | Multi-node Fluentd forwarders and OpenSearch clust... |
| Failover Strategy | Load balancing across Fluentd forwarders and stand... |

# 4.0.0.0 Retention Policy Design

## 4.1.0.0 Retention Periods

### 4.1.1.0 Log Type

#### 4.1.1.1 Log Type

ApplicationLogs

#### 4.1.1.2 Retention Period

30 days (hot), 90 days (warm), 1 year (cold)

#### 4.1.1.3 Justification

Balances troubleshooting needs with storage costs. Recent logs are kept on fast storage for immediate access.

#### 4.1.1.4 Compliance Requirement

N/A

### 4.1.2.0 Log Type

#### 4.1.2.1 Log Type

AuditLogs

#### 4.1.2.2 Retention Period

7 years

#### 4.1.2.3 Justification

Aligns with data retention policies specified in REQ-1-088 and supports compliance requirements from REQ-1-023.

#### 4.1.2.4 Compliance Requirement

FDA 21 CFR Part 11

## 4.2.0.0 Compliance Requirements

- {'regulation': 'GDPR', 'applicableLogTypes': ['ApplicationLogs', 'AuditLogs'], 'minimumRetention': 'N/A', 'specialHandling': 'PII such as user email must be masked or redacted as per REQ-1-027.'}

## 4.3.0.0 Volume Impact Analysis

| Property | Value |
|----------|-------|
| Estimated Daily Volume | 1-5 TB/day |
| Storage Cost Projection | High, requires aggressive tiering and sampling to ... |
| Compression Ratio | 5:1 to 10:1 |

## 4.4.0.0 Storage Tiering

### 4.4.1.0 Hot Storage

| Property | Value |
|----------|-------|
| Duration | 30 days |
| Accessibility | immediate |
| Cost | high |

### 4.4.2.0 Warm Storage

| Property | Value |
|----------|-------|
| Duration | 90 days |
| Accessibility | minutes |
| Cost | medium |

### 4.4.3.0 Cold Storage

| Property | Value |
|----------|-------|
| Duration | 1 year |
| Accessibility | hours |
| Cost | low |

## 4.5.0.0 Compression Strategy

| Property | Value |
|----------|-------|
| Algorithm | DEFLATE (default in OpenSearch) |
| Compression Level | default |
| Expected Ratio | 8:1 |

## 4.6.0.0 Anonymization Requirements

- {'dataType': 'PII', 'method': 'mask', 'timeline': 'at-ingest', 'compliance': 'GDPR (REQ-1-027)'}

# 5.0.0.0 Search Capability Requirements

## 5.1.0.0 Essential Capabilities

### 5.1.1.0 Capability

#### 5.1.1.1 Capability

Full-text search on message content

#### 5.1.1.2 Performance Requirement

<5s

#### 5.1.1.3 Justification

Core requirement for troubleshooting application errors.

### 5.1.2.0 Capability

#### 5.1.2.1 Capability

Filtering by indexed fields (severity, service_name, tenantId, correlation_id)

#### 5.1.2.2 Performance Requirement

<1s

#### 5.1.2.3 Justification

Essential for narrowing down investigations and operational monitoring.

## 5.2.0.0 Performance Characteristics

| Property | Value |
|----------|-------|
| Search Latency | < 5 seconds for queries over last 24 hours |
| Concurrent Users | 50 |
| Query Complexity | complex |
| Indexing Strategy | Time-based daily indices |

## 5.3.0.0 Indexed Fields

### 5.3.1.0 Field

#### 5.3.1.1 Field

correlation_id

#### 5.3.1.2 Index Type

keyword

#### 5.3.1.3 Search Pattern

exact match

#### 5.3.1.4 Frequency

high

### 5.3.2.0 Field

#### 5.3.2.1 Field

tenantId

#### 5.3.2.2 Index Type

keyword

#### 5.3.2.3 Search Pattern

exact match

#### 5.3.2.4 Frequency

high

### 5.3.3.0 Field

#### 5.3.3.1 Field

traceId

#### 5.3.3.2 Index Type

keyword

#### 5.3.3.3 Search Pattern

exact match

#### 5.3.3.4 Frequency

medium

## 5.4.0.0 Full Text Search

### 5.4.1.0 Required

✅ Yes

### 5.4.2.0 Fields

- message
- stack_trace

### 5.4.3.0 Search Engine

OpenSearch

### 5.4.4.0 Relevance Scoring

✅ Yes

## 5.5.0.0 Correlation And Tracing

### 5.5.1.0 Correlation Ids

- correlation_id

### 5.5.2.0 Trace Id Propagation

OpenTelemetry with W3C Trace Context headers

### 5.5.3.0 Span Correlation

✅ Yes

### 5.5.4.0 Cross Service Tracing

✅ Yes

## 5.6.0.0 Dashboard Requirements

### 5.6.1.0 Dashboard

#### 5.6.1.1 Dashboard

System-Wide Error Dashboard

#### 5.6.1.2 Purpose

Provides an at-a-glance view of error rates, top error sources, and recent critical logs.

#### 5.6.1.3 Refresh Interval

1m

#### 5.6.1.4 Audience

SRE/Operations

### 5.6.2.0 Dashboard

#### 5.6.2.1 Dashboard

Service-Specific Health

#### 5.6.2.2 Purpose

Filtered view of logs and error rates for a single microservice.

#### 5.6.2.3 Refresh Interval

5m

#### 5.6.2.4 Audience

Developers, SRE

# 6.0.0.0 Storage Solution Selection

## 6.1.0.0 Selected Technology

### 6.1.1.0 Primary

OpenSearch

### 6.1.2.0 Reasoning

Mandated by the system requirements in REQ-1-090. It is a scalable, open-source solution with features like RBAC and storage tiering.

### 6.1.3.0 Alternatives

- Elasticsearch
- Loki

## 6.2.0.0 Scalability Requirements

| Property | Value |
|----------|-------|
| Expected Growth Rate | 10% month-over-month |
| Peak Load Handling | Must handle logs from 10,000 edge clients (REQ-1-0... |
| Horizontal Scaling | ✅ |

## 6.3.0.0 Cost Performance Analysis

- {'solution': 'OpenSearch on AWS', 'costPerGB': 'Variable based on storage tier (hot/warm/cold).', 'queryPerformance': 'High on hot tier, moderate on warm/cold.', 'operationalComplexity': 'medium'}

## 6.4.0.0 Backup And Recovery

| Property | Value |
|----------|-------|
| Backup Frequency | daily snapshots |
| Recovery Time Objective | 4 hours |
| Recovery Point Objective | 24 hours |
| Testing Frequency | quarterly |

## 6.5.0.0 Geo Distribution

### 6.5.1.0 Required

✅ Yes

### 6.5.2.0 Regions

- us-east-1
- eu-west-1

### 6.5.3.0 Replication Strategy

Deploy separate OpenSearch clusters per region to comply with data residency requirements (REQ-1-030).

## 6.6.0.0 Data Sovereignty

- {'region': 'EU', 'requirements': ['Logs containing PII must be stored within EU data centers.'], 'complianceFramework': 'GDPR'}

# 7.0.0.0 Access Control And Compliance

## 7.1.0.0 Access Control Requirements

### 7.1.1.0 Role

#### 7.1.1.1 Role

SRE/Administrator

#### 7.1.1.2 Permissions

- read
- write
- delete

#### 7.1.1.3 Log Types

- *

#### 7.1.1.4 Justification

Full access required for system administration and incident response.

### 7.1.2.0 Role

#### 7.1.2.1 Role

Developer

#### 7.1.2.2 Permissions

- read

#### 7.1.2.3 Log Types

- ApplicationLogs (non-production)

#### 7.1.2.4 Justification

Read-only access to non-production logs for debugging purposes.

## 7.2.0.0 Sensitive Data Handling

### 7.2.1.0 Data Type

#### 7.2.1.1 Data Type

PII

#### 7.2.1.2 Handling Strategy

mask

#### 7.2.1.3 Fields

- user.email
- user.name

#### 7.2.1.4 Compliance Requirement

GDPR (REQ-1-027)

### 7.2.2.0 Data Type

#### 7.2.2.1 Data Type

Secrets

#### 7.2.2.2 Handling Strategy

exclude

#### 7.2.2.3 Fields

- password
- apiKey
- token

#### 7.2.2.4 Compliance Requirement

Security Best Practices (REQ-1-081)

## 7.3.0.0 Encryption Requirements

### 7.3.1.0 In Transit

| Property | Value |
|----------|-------|
| Required | ✅ |
| Protocol | TLS 1.3 |
| Certificate Management | AWS Certificate Manager (ACM) |

### 7.3.2.0 At Rest

| Property | Value |
|----------|-------|
| Required | ✅ |
| Algorithm | AES-256 |
| Key Management | AWS Key Management Service (KMS) |

## 7.4.0.0 Audit Trail

| Property | Value |
|----------|-------|
| Log Access | ✅ |
| Retention Period | 1 year |
| Audit Log Location | Separate OpenSearch index |
| Compliance Reporting | ✅ |

## 7.5.0.0 Regulatory Compliance

- {'regulation': 'GDPR', 'applicableComponents': ['All services handling user data'], 'specificRequirements': ['Data minimization', 'PII masking in logs'], 'evidenceCollection': 'Review of logging configurations and sample logs.'}

## 7.6.0.0 Data Protection Measures

- {'measure': 'PII Masking Filter', 'implementation': 'A custom filter in the Fluentd pipeline or a logging library middleware to redact sensitive fields before shipping.', 'monitoringRequired': True}

# 8.0.0.0 Project Specific Logging Config

## 8.1.0.0 Logging Config

### 8.1.1.0 Level

🔹 INFO (Production)

### 8.1.2.0 Retention

Multi-tiered (30d hot, 90d warm, 1yr cold)

### 8.1.3.0 Aggregation

Centralized via Fluentd to OpenSearch

### 8.1.4.0 Storage

OpenSearch

### 8.1.5.0 Configuration

*No data available*

## 8.2.0.0 Component Configurations

### 8.2.1.0 Component

#### 8.2.1.1 Component

opc-core-client

#### 8.2.1.2 Log Level

INFO

#### 8.2.1.3 Output Format

JSON

#### 8.2.1.4 Destinations

- stdout

#### 8.2.1.5 Sampling

##### 8.2.1.5.1 Enabled

❌ No

##### 8.2.1.5.2 Rate

100%

#### 8.2.1.6.0 Custom Fields

- clientId
- tenantId

### 8.2.2.0.0 Component

#### 8.2.2.1.0 Component

ingestion-service

#### 8.2.2.2.0 Log Level

INFO

#### 8.2.2.3.0 Output Format

JSON

#### 8.2.2.4.0 Destinations

- stdout

#### 8.2.2.5.0 Sampling

##### 8.2.2.5.1 Enabled

✅ Yes

##### 8.2.2.5.2 Rate

1%

#### 8.2.2.6.0 Custom Fields

- tenantId

## 8.3.0.0.0 Metrics

### 8.3.1.0.0 Custom Metrics

*No data available*

## 8.4.0.0.0 Alert Rules

- {'name': 'High Log Error Rate', 'condition': 'SUM(rate(log_messages_total{level="error"}[5m])) > 10', 'severity': 'High', 'actions': [{'type': 'PagerDuty', 'target': 'SRE On-Call', 'configuration': {}}], 'suppressionRules': [], 'escalationPath': []}

# 9.0.0.0.0 Implementation Priority

## 9.1.0.0.0 Component

### 9.1.1.0.0 Component

Foundation: Structured Logging & Centralized Collection

### 9.1.2.0.0 Priority

🔴 high

### 9.1.3.0.0 Dependencies

- .NET shared logging library
- OpenSearch Cluster
- Fluentd DaemonSet

### 9.1.4.0.0 Estimated Effort

High

### 9.1.5.0.0 Risk Level

medium

## 9.2.0.0.0 Component

### 9.2.1.0.0 Component

Edge Client Log Forwarding

### 9.2.2.0.0 Priority

🟡 medium

### 9.2.3.0.0 Dependencies

- Foundation

### 9.2.4.0.0 Estimated Effort

Medium

### 9.2.5.0.0 Risk Level

high

## 9.3.0.0.0 Component

### 9.3.1.0.0 Component

Advanced Features: PII Masking & Storage Tiering

### 9.3.2.0.0 Priority

🟢 low

### 9.3.3.0.0 Dependencies

- Foundation

### 9.3.4.0.0 Estimated Effort

Medium

### 9.3.5.0.0 Risk Level

low

# 10.0.0.0.0 Risk Assessment

## 10.1.0.0.0 Risk

### 10.1.1.0.0 Risk

Uncontrolled log volume leads to excessive cost and poor search performance.

### 10.1.2.0.0 Impact

high

### 10.1.3.0.0 Probability

high

### 10.1.4.0.0 Mitigation

Implement sampling for high-volume success logs, enforce retention policies, and use storage tiering from day one.

### 10.1.5.0.0 Contingency Plan

Temporarily increase log level to WARN for noisy services while optimizing sampling rules.

## 10.2.0.0.0 Risk

### 10.2.1.0.0 Risk

PII or other sensitive data is leaked into logs.

### 10.2.2.0.0 Impact

high

### 10.2.3.0.0 Probability

medium

### 10.2.4.0.0 Mitigation

Implement automated PII detection and masking in the logging pipeline. Conduct regular code reviews focused on logging practices. Prohibit logging of raw request/response objects.

### 10.2.5.0.0 Contingency Plan

Have a documented procedure for purging sensitive data from the log store if a leak is discovered.

# 11.0.0.0.0 Recommendations

## 11.1.0.0.0 Category

### 11.1.1.0.0 Category

🔹 Implementation

### 11.1.2.0.0 Recommendation

Develop a shared .NET logging library, pre-configured for structured JSON output with standard fields, to be used by all backend microservices.

### 11.1.3.0.0 Justification

Ensures consistency across all services, reduces boilerplate code, and simplifies enforcement of logging standards like including correlation IDs.

### 11.1.4.0.0 Priority

🔴 high

### 11.1.5.0.0 Implementation Notes

The library should integrate with ASP.NET Core middleware to automatically enrich logs with request-specific data.

## 11.2.0.0.0 Category

### 11.2.1.0.0 Category

🔹 Operations

### 11.2.2.0.0 Recommendation

Automate the entire observability stack (Fluentd, OpenSearch, Prometheus, Grafana) deployment using Terraform.

### 11.2.3.0.0 Justification

Aligns with the IaC requirement (REQ-1-089), ensures environment consistency, and simplifies disaster recovery and regional deployments.

### 11.2.4.0.0 Priority

🔴 high

### 11.2.5.0.0 Implementation Notes

Store Terraform state in S3 with state locking.

