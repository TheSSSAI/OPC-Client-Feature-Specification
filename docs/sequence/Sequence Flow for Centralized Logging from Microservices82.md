# 1 Overview

## 1.1 Diagram Id

SEQ-OF-002

## 1.2 Name

Centralized Logging from Microservices

## 1.3 Description

A backend microservice writes a structured JSON log to its standard output. A Fluentd agent, running as a DaemonSet in Kubernetes, collects the log entry, enriches it with Kubernetes metadata (e.g., pod name, namespace), and forwards it to a centralized OpenSearch cluster for indexing and long-term storage.

## 1.4 Type

🔹 OperationalFlow

## 1.5 Purpose

To aggregate logs from all distributed system components into a single, searchable location for troubleshooting, monitoring, and analysis, as required by REQ-MON-001.

## 1.6 Complexity

Medium

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

Continuous

## 1.9 Participants

- REPO-SVC-AST
- Fluentd
- OpenSearch

## 1.10 Key Interactions

- The Asset Service writes a log entry (e.g., 'Asset created successfully') to stdout using a logging library that enforces JSON structure.
- The Fluentd agent on the Kubernetes node reads the container's log stream from the host.
- Fluentd's filter plugins parse the JSON and add metadata like `k8s_pod_name` and `k8s_namespace`.
- Fluentd's output plugin securely sends the enriched log document in a batch to the OpenSearch cluster.
- OpenSearch indexes the document, making it available for search in Grafana or OpenSearch Dashboards.

## 1.11 Triggers

- Any microservice or system component generates a log message at any severity level.

## 1.12 Outcomes

- The log entry is securely stored and indexed in the central OpenSearch cluster.
- The log can be searched, visualized, and correlated with other logs and traces in Grafana.

## 1.13 Business Rules

- Logs from all containers and services shall be aggregated using Fluentd and stored in OpenSearch (REQ-MON-001).
- Logs shall be structured in JSON format and include a correlation ID (REQ-MON-001).

## 1.14 Error Scenarios

- The OpenSearch cluster is down or overloaded, causing Fluentd to buffer logs on disk.
- The Fluentd agent fails, resulting in a temporary loss of log shipping from that node.
- A log message format is invalid and cannot be parsed correctly by the pipeline.

## 1.15 Integration Points

- OpenSearch Cluster

# 2.0 Details

## 2.1 Diagram Id

SEQ-OP-LOGGING-001

## 2.2 Name

Implementation: Centralized Log Aggregation via Fluentd and OpenSearch

## 2.3 Description

This sequence details the operational flow for centralized log aggregation from a backend microservice running in Kubernetes. It illustrates how a structured JSON log, written to standard output by the service, is collected by a node-level Fluentd agent, enriched with Kubernetes metadata, and securely forwarded in batches to a central OpenSearch cluster for indexing, fulfilling the 'Logging' pillar of the system's observability strategy as per REQ-MON-001.

## 2.4 Participants

### 2.4.1 Microservice

#### 2.4.1.1 Repository Id

REPO-SVC-AST

#### 2.4.1.2 Display Name

Asset Service

#### 2.4.1.3 Type

🔹 Microservice

#### 2.4.1.4 Technology

.NET 8 / ASP.NET Core

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | Container |

### 2.4.2.0 Orchestration Runtime

#### 2.4.2.1 Repository Id

SYS-K8S-RUNTIME

#### 2.4.2.2 Display Name

Kubelet / CRI

#### 2.4.2.3 Type

🔹 Orchestration Runtime

#### 2.4.2.4 Technology

Container Runtime Interface (e.g., containerd)

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #90EE90 |
| Stereotype | Node Component |

### 2.4.3.0 Log Aggregator

#### 2.4.3.1 Repository Id

SYS-LOG-AGENT

#### 2.4.3.2 Display Name

Fluentd Agent

#### 2.4.3.3 Type

🔹 Log Aggregator

#### 2.4.3.4 Technology

Fluentd DaemonSet

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #FFD700 |
| Stereotype | DaemonSet |

### 2.4.4.0 Search Database

#### 2.4.4.1 Repository Id

DB-OPS-LOGS

#### 2.4.4.2 Display Name

OpenSearch Cluster

#### 2.4.4.3 Type

🔹 Search Database

#### 2.4.4.4 Technology

OpenSearch

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #FA8072 |
| Stereotype | Log Store |

## 2.5.0.0 Interactions

### 2.5.1.0 Log Generation

#### 2.5.1.1 Source Id

REPO-SVC-AST

#### 2.5.1.2 Target Id

SYS-K8S-RUNTIME

#### 2.5.1.3 Message

Writes structured JSON log to stdout stream

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Log Generation

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

❌ No

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | stdout |
| Method | loggingFramework.LogInformation() |
| Parameters | Log Message: `Asset created successfully`, Log Con... |
| Authentication | N/A |
| Error Handling | Logging framework handles internal errors; failure... |
| Performance | Sub-millisecond latency. |

### 2.5.2.0 Log Collection

#### 2.5.2.1 Source Id

SYS-K8S-RUNTIME

#### 2.5.2.2 Target Id

SYS-LOG-AGENT

#### 2.5.2.3 Message

Tails container log file from host path

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Log Collection

#### 2.5.2.6 Is Synchronous

❌ No

#### 2.5.2.7 Return Message



#### 2.5.2.8 Has Return

❌ No

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | File I/O |
| Method | Fluentd `in_tail` plugin |
| Parameters | Path: `/var/log/pods/<namespace>_<pod_name>_<uid>/... |
| Authentication | Requires hostPath volume mount and appropriate per... |
| Error Handling | Fluentd maintains position in file to resume after... |
| Performance | Depends on disk I/O; low overhead. |

### 2.5.3.0 Log Processing

#### 2.5.3.1 Source Id

SYS-LOG-AGENT

#### 2.5.3.2 Target Id

SYS-LOG-AGENT

#### 2.5.3.3 Message

Parses JSON and enriches with Kubernetes metadata

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Log Processing

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message



#### 2.5.3.8 Has Return

❌ No

#### 2.5.3.9 Is Activation

❌ No

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | Fluentd Filter Chain |
| Parameters | Plugins: `filter_kubernetes_metadata`, `parser_jso... |
| Authentication | N/A |
| Error Handling | Malformed JSON logs are tagged with `_parse_failur... |
| Performance | In-memory processing, must be configured efficient... |

### 2.5.4.0 Log Shipping

#### 2.5.4.1 Source Id

SYS-LOG-AGENT

#### 2.5.4.2 Target Id

DB-OPS-LOGS

#### 2.5.4.3 Message

Sends batch of enriched log documents via Bulk API

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 Log Shipping

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

200 OK with batch processing results

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

✅ Yes

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | POST /_bulk |
| Parameters | Body: NDJSON payload containing multiple log docum... |
| Authentication | AWS IAM role with SigV4 signing or Basic Authentic... |
| Error Handling | On 5xx or network errors, Fluentd retries with exp... |
| Performance | Batch size (e.g., 1MB or 1000 docs) and flush inte... |

### 2.5.5.0 Indexing

#### 2.5.5.1 Source Id

DB-OPS-LOGS

#### 2.5.5.2 Target Id

DB-OPS-LOGS

#### 2.5.5.3 Message

Asynchronously indexes documents for search

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 Indexing

#### 2.5.5.6 Is Synchronous

❌ No

#### 2.5.5.7 Return Message



#### 2.5.5.8 Has Return

❌ No

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | Lucene Indexing |
| Parameters | Shard allocation, index mapping, refresh interval. |
| Authentication | N/A |
| Error Handling | Indexing failures are logged internally by OpenSea... |
| Performance | Indexing latency is a key metric. Should be < 1 se... |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Structured Logging is Mandatory: All microservices MUST write logs in a consistent JSON format to stdout. The format must include standard fields like 'timestamp', 'severity', 'service_name', and 'correlation_id' to enable effective searching and correlation (REQ-MON-001).

#### 2.6.1.2 Position

top-left

#### 2.6.1.3 Participant Id

REPO-SVC-AST

#### 2.6.1.4 Sequence Number

1

### 2.6.2.0 Content

#### 2.6.2.1 Content

Resilience via Buffering: Fluentd MUST be configured with a file-based buffer. This allows logs to be queued on the node's disk if the OpenSearch cluster is unavailable, preventing log loss during transient outages. Buffer size and retention must be monitored.

#### 2.6.2.2 Position

bottom-right

#### 2.6.2.3 Participant Id

SYS-LOG-AGENT

#### 2.6.2.4 Sequence Number

4

### 2.6.3.0 Content

#### 2.6.3.1 Content

DaemonSet Architecture: Fluentd runs as a DaemonSet, ensuring exactly one agent pod is running on each worker node in the Kubernetes cluster. This provides decentralized, scalable log collection at the source.

#### 2.6.3.2 Position

bottom-center

#### 2.6.3.3 Participant Id

SYS-K8S-RUNTIME

#### 2.6.3.4 Sequence Number

2

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Communication between Fluentd and OpenSearch must ... |
| Performance Targets | The end-to-end logging pipeline latency (from stdo... |
| Error Handling Strategy | Fluentd's configuration must include a robust retr... |
| Testing Considerations | Test scenarios should include: OpenSearch unavaila... |
| Monitoring Requirements | The logging pipeline itself must be monitored. Key... |
| Deployment Considerations | The Fluentd configuration (including input sources... |

