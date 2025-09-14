# 1 Overview

## 1.1 Diagram Id

SEQ-DF-001

## 1.2 Name

High-Throughput Time-Series Data Ingestion from Edge

## 1.3 Description

An OPC Core Client establishes a secure, bi-directional streaming gRPC connection to the cloud Data Ingestion Service using mutual TLS (mTLS) for authentication. It continuously streams OPC tag data points, which are then validated, enriched with tenant context, and batch-inserted into the TimescaleDB time-series database for maximum efficiency.

## 1.4 Type

🔹 DataFlow

## 1.5 Purpose

To reliably and efficiently transfer large volumes of real-time industrial data from the edge to the central cloud data store, meeting stringent performance and security targets.

## 1.6 Complexity

High

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

Continuous

## 1.9 Participants

- REPO-EDGE-OPC
- REPO-SVC-DIN
- TimescaleDB

## 1.10 Key Interactions

- OPC Client establishes a secure mTLS gRPC connection to the Ingestion Service.
- Client begins streaming a sequence of Protobuf-serialized DataPoint messages.
- Ingestion Service receives, deserializes, validates, and batches the data points in-memory.
- Service performs a high-performance bulk COPY/INSERT operation into the appropriate TimescaleDB hypertable to persist the batch.

## 1.11 Triggers

- OPC Core Client is running, online, and has data to send.

## 1.12 Outcomes

- Real-time data is durably persisted in the time-series database with low latency.
- Data is made available for querying by other services, such as dashboards and analytics.

## 1.13 Business Rules

- Communication must use gRPC with mutual TLS (mTLS) for high-throughput streaming (REQ-1-010).
- System must support ingestion of up to 10,000 values per second per tenant (REQ-1-075).
- Client certificate must be validated upon connection.

## 1.14 Error Scenarios

- gRPC connection fails due to network issues or invalid certificates.
- TimescaleDB is unavailable or cannot handle the ingestion rate.
- Data batch insertion fails due to constraint violations or data corruption.

## 1.15 Integration Points

- TimescaleDB Database

# 2.0 Details

## 2.1 Diagram Id

SEQ-DF-001

## 2.2 Name

High-Throughput Time-Series Data Ingestion from Edge

## 2.3 Description

A comprehensive sequence for the continuous, high-throughput ingestion of time-series data from an OPC Core Client at the edge to the central TimescaleDB. The sequence details the secure mTLS-authenticated gRPC stream establishment, Protobuf message flow, in-memory batch processing within the Data Ingestion Service, and the high-performance bulk persistence mechanism into the database, including critical error handling and performance considerations.

## 2.4 Participants

### 2.4.1 Edge Application

#### 2.4.1.1 Repository Id

REPO-EDGE-OPC

#### 2.4.1.2 Display Name

OPC Core Client

#### 2.4.1.3 Type

🔹 Edge Application

#### 2.4.1.4 Technology

.NET 8, gRPC Client, Docker

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | Edge |

### 2.4.2.0 Microservice

#### 2.4.2.1 Repository Id

REPO-SVC-DIN

#### 2.4.2.2 Display Name

Data Ingestion Service

#### 2.4.2.3 Type

🔹 Microservice

#### 2.4.2.4 Technology

.NET 8, ASP.NET Core gRPC Service, EKS

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #32CD32 |
| Stereotype | Cloud Service |

### 2.4.3.0 Database

#### 2.4.3.1 Repository Id

DB-TSDB-01

#### 2.4.3.2 Display Name

TimescaleDB

#### 2.4.3.3 Type

🔹 Database

#### 2.4.3.4 Technology

PostgreSQL 16 with TimescaleDB Extension on AWS RDS

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #FFD700 |
| Stereotype | Time-Series Data Store |

## 2.5.0.0 Interactions

### 2.5.1.0 gRPC Stream Initiation

#### 2.5.1.1 Source Id

REPO-EDGE-OPC

#### 2.5.1.2 Target Id

REPO-SVC-DIN

#### 2.5.1.3 Message

EstablishStream(stream DataPointRequest)

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 gRPC Stream Initiation

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

StreamHandle

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC (HTTP/2) |
| Method | Ingest.EstablishStream |
| Parameters | Client-side stream of `DataPointRequest` Protobuf ... |
| Authentication | Mutual TLS (mTLS). Server validates the client's X... |
| Error Handling | Connection fails if certificate is invalid, expire... |
| Performance | Connection must be established in < 500ms. A persi... |

### 2.5.2.0 gRPC Data Streaming

#### 2.5.2.1 Source Id

REPO-EDGE-OPC

#### 2.5.2.2 Target Id

REPO-SVC-DIN

#### 2.5.2.3 Message

stream.WriteAsync(DataPointRequest)

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 gRPC Data Streaming

#### 2.5.2.6 Is Synchronous

❌ No

#### 2.5.2.7 Return Message



#### 2.5.2.8 Has Return

❌ No

#### 2.5.2.9 Is Activation

❌ No

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC Stream |
| Method | N/A (writing to established stream) |
| Parameters | `DataPointRequest` { string OpcTagId, google.proto... |
| Authentication | Authentication is established at the stream level ... |
| Error Handling | If the stream breaks, the client buffers data to i... |
| Performance | Client streams data points as they are collected t... |

### 2.5.3.0 Internal Processing Loop

#### 2.5.3.1 Source Id

REPO-SVC-DIN

#### 2.5.3.2 Target Id

REPO-SVC-DIN

#### 2.5.3.3 Message

Process incoming data points in a concurrent pipeline

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Internal Processing Loop

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message



#### 2.5.3.8 Has Return

❌ No

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Nested Interactions

##### 2.5.3.10.1 Data Validation

###### 2.5.3.10.1.1 Source Id

REPO-SVC-DIN

###### 2.5.3.10.1.2 Target Id

REPO-SVC-DIN

###### 2.5.3.10.1.3 Message

Deserialize Protobuf message and validate schema

###### 2.5.3.10.1.4 Sequence Number

3.1

###### 2.5.3.10.1.5 Type

🔹 Data Validation

###### 2.5.3.10.1.6 Is Synchronous

✅ Yes

###### 2.5.3.10.1.7 Has Return

❌ No

###### 2.5.3.10.1.8 Technical Details

####### 2.5.3.10.1.8.1 Protocol

In-Memory

####### 2.5.3.10.1.8.2 Error Handling

Malformed or invalid messages are dropped and logged with a 'WARN' severity level, incrementing a `malformed_messages_total` metric.

##### 2.5.3.10.2.0.0 Data Enrichment

###### 2.5.3.10.2.1.0 Source Id

REPO-SVC-DIN

###### 2.5.3.10.2.2.0 Target Id

REPO-SVC-DIN

###### 2.5.3.10.2.3.0 Message

Enrich with TenantID and AssetID from metadata cache

###### 2.5.3.10.2.4.0 Sequence Number

3.2

###### 2.5.3.10.2.5.0 Type

🔹 Data Enrichment

###### 2.5.3.10.2.6.0 Is Synchronous

✅ Yes

###### 2.5.3.10.2.7.0 Has Return

❌ No

###### 2.5.3.10.2.8.0 Technical Details

####### 2.5.3.10.2.8.1 Protocol

In-Memory/Redis

####### 2.5.3.10.2.8.2 Error Handling

If OpcTagId is not found in cache/DB, the data point is dropped and logged, triggering a potential alert for misconfiguration.

##### 2.5.3.10.3.0.0 Batching

###### 2.5.3.10.3.1.0 Source Id

REPO-SVC-DIN

###### 2.5.3.10.3.2.0 Target Id

REPO-SVC-DIN

###### 2.5.3.10.3.3.0 Message

Add enriched data point to in-memory batch buffer

###### 2.5.3.10.3.4.0 Sequence Number

3.3

###### 2.5.3.10.3.5.0 Type

🔹 Batching

###### 2.5.3.10.3.6.0 Is Synchronous

✅ Yes

###### 2.5.3.10.3.7.0 Has Return

❌ No

###### 2.5.3.10.3.8.0 Technical Details

####### 2.5.3.10.3.8.1 Protocol

In-Memory

####### 2.5.3.10.3.8.2 Performance

Batching is triggered when the batch reaches a configurable size (e.g., 5000 records) or a time threshold is met (e.g., 1 second).

### 2.5.4.0.0.0.0 Database Bulk Insert

#### 2.5.4.1.0.0.0 Source Id

REPO-SVC-DIN

#### 2.5.4.2.0.0.0 Target Id

DB-TSDB-01

#### 2.5.4.3.0.0.0 Message

COPY tag_data (timestamp, tag_id, value, quality, tenant_id) FROM STDIN (BINARY)

#### 2.5.4.4.0.0.0 Sequence Number

4

#### 2.5.4.5.0.0.0 Type

🔹 Database Bulk Insert

#### 2.5.4.6.0.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0.0 Return Message

COPY N (number of rows)

#### 2.5.4.8.0.0.0 Has Return

✅ Yes

#### 2.5.4.9.0.0.0 Is Activation

❌ No

#### 2.5.4.10.0.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | PostgreSQL Wire Protocol |
| Method | SQL COPY Command |
| Parameters | A binary-formatted stream of the batched data poin... |
| Authentication | Uses credentials from a secure secret store (e.g.,... |
| Error Handling | On failure, the entire batch transaction is rolled... |
| Performance | This is the most performant way to insert data int... |

## 2.6.0.0.0.0.0 Notes

### 2.6.1.0.0.0.0 Content

#### 2.6.1.1.0.0.0 Content

The enrichment step (3.2) should utilize a Redis cache for tag metadata (OpcTagId -> AssetId, TenantId) to avoid database lookups on the hot path. The cache is populated by the Asset & Topology Service.

#### 2.6.1.2.0.0.0 Position

topRight

#### 2.6.1.3.0.0.0 Participant Id

REPO-SVC-DIN

#### 2.6.1.4.0.0.0 Sequence Number

3.2

### 2.6.2.0.0.0.0 Content

#### 2.6.2.1.0.0.0 Content

The batch persistence in step 4 is a transactional 'Unit of Work'. The entire batch succeeds or fails together to ensure data consistency.

#### 2.6.2.2.0.0.0 Position

bottomRight

#### 2.6.2.3.0.0.0 Participant Id

REPO-SVC-DIN

#### 2.6.2.4.0.0.0 Sequence Number

4

## 2.7.0.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | mTLS is mandatory (REQ-1-010). The Data Ingestion ... |
| Performance Targets | The end-to-end pipeline must sustain an ingestion ... |
| Error Handling Strategy | The OPC Client is responsible for reliability duri... |
| Testing Considerations | A dedicated performance testing environment is req... |
| Monitoring Requirements | Key metrics to monitor: [Data Ingestion Service] a... |
| Deployment Considerations | The Data Ingestion Service should be deployed as a... |

