# 1 Overview

## 1.1 Diagram Id

SEQ-BP-002

## 1.2 Name

Data Migration from Legacy System

## 1.3 Description

An Engineer exports asset and tag configuration from a legacy system into a predefined CSV format. They use the web UI or an administrative CLI to upload this file. The system parses the file, validates the data, and performs a bulk import to create the asset hierarchy and tag mappings in a transactional manner.

## 1.4 Type

🔹 BusinessProcess

## 1.5 Purpose

To facilitate the transition from legacy systems by providing tools for bulk import of existing configuration data, reducing manual setup time and errors (REQ-DM-003, REQ-TRN-002).

## 1.6 Complexity

High

## 1.7 Priority

🟡 Medium

## 1.8 Frequency

OnDemand

## 1.9 Participants

- Admin CLI / REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-AST

## 1.10 Key Interactions

- An Engineer prepares the data in the required CSV/JSON format according to provided templates.
- The Engineer uses the import tool to upload the file.
- A backend process is initiated to parse and validate the file against the system's data model rules.
- The Asset Service translates each valid row into asset and tag creation/update operations.
- These operations are executed as a single transaction against the database to ensure atomicity.
- A summary report is generated detailing successfully imported records and any failed records with specific error reasons.

## 1.11 Triggers

- A new site is being onboarded to the system and its configuration needs to be migrated.

## 1.12 Outcomes

- The legacy configuration is successfully migrated into the new system's database.
- A validation report is available for review and sign-off by the site engineer (REQ-TRN-002).

## 1.13 Business Rules

- The system must provide tools for bulk import/export of asset hierarchy and tag lists via CSV and JSON (REQ-DM-003).
- Post-migration validation checks, such as record counts, are mandatory (REQ-TRN-002).

## 1.14 Error Scenarios

- The file format is incorrect or headers are missing.
- Data validation fails (e.g., circular dependencies in the asset hierarchy, duplicate tags).
- The bulk import database transaction fails midway and is rolled back.

## 1.15 Integration Points

*No items available*

# 2.0 Details

## 2.1 Diagram Id

SEQ-BP-002

## 2.2 Name

Implementation of Asynchronous Bulk Asset & Tag Data Migration

## 2.3 Description

Detailed technical sequence for migrating asset and tag configurations via a file upload. An Engineer initiates the process through the UI or CLI. The request is authenticated and routed to the Asset Service, which starts an asynchronous background job to handle the parsing, validation, and transactional database import. The user is notified upon completion, and a detailed summary report is generated and stored, fulfilling REQ-DM-003 and REQ-TRN-002.

## 2.4 Participants

### 2.4.1 Client Application

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Engineer's Web UI / CLI

#### 2.4.1.3 Type

🔹 Client Application

#### 2.4.1.4 Technology

React 18 / .NET 8 CLI

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | User Interface |

### 2.4.2.0 Gateway

#### 2.4.2.1 Repository Id

REPO-GW-API

#### 2.4.2.2 Display Name

API Gateway

#### 2.4.2.3 Type

🔹 Gateway

#### 2.4.2.4 Technology

Kong v3.7.0 on Kubernetes

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #FFD700 |
| Stereotype | API Gateway |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-AST

#### 2.4.3.2 Display Name

Asset & Topology Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8 / ASP.NET Core

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #32CD32 |
| Stereotype | Application Service |

### 2.4.4.0 Background Process

#### 2.4.4.1 Repository Id

REPO-SVC-AST-BG-WORKER

#### 2.4.4.2 Display Name

Asset Service (Background Worker)

#### 2.4.4.3 Type

🔹 Background Process

#### 2.4.4.4 Technology

.NET 8 IHostedService

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #90EE90 |
| Stereotype | Background Worker |

### 2.4.5.0 Database

#### 2.4.5.1 Repository Id

postgresql-db-001

#### 2.4.5.2 Display Name

PostgreSQL Database

#### 2.4.5.3 Type

🔹 Database

#### 2.4.5.4 Technology

PostgreSQL 16

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #87CEEB |
| Stereotype | Database |

### 2.4.6.0 Microservice

#### 2.4.6.1 Repository Id

REPO-SVC-ADT

#### 2.4.6.2 Display Name

Audit Service

#### 2.4.6.3 Type

🔹 Microservice

#### 2.4.6.4 Technology

.NET 8 / gRPC

#### 2.4.6.5 Order

6

#### 2.4.6.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #FFA07A |
| Stereotype | Compliance Service |

### 2.4.7.0 Object Storage

#### 2.4.7.1 Repository Id

s3-storage-001

#### 2.4.7.2 Display Name

S3 Object Storage

#### 2.4.7.3 Type

🔹 Object Storage

#### 2.4.7.4 Technology

Amazon S3

#### 2.4.7.5 Order

7

#### 2.4.7.6 Style

| Property | Value |
|----------|-------|
| Shape | cloud |
| Color | #DA70D6 |
| Stereotype | Storage |

## 2.5.0.0 Interactions

### 2.5.1.0 API Request

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

1. Uploads asset/tag configuration file

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 API Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

4. Returns Job ID for status polling

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | POST /api/v1/assets/import |
| Parameters | Content-Type: multipart/form-data; file: <CSV/JSON... |
| Authentication | Authorization: Bearer <JWT>. JWT must have 'Engine... |
| Error Handling | Client handles 400 (Bad Request), 401 (Unauthorize... |
| Performance | Request timeout: 60s. File size limit: 10MB. |

### 2.5.2.0 Internal API Request

#### 2.5.2.1 Source Id

REPO-GW-API

#### 2.5.2.2 Target Id

REPO-SVC-AST

#### 2.5.2.3 Message

2. Forwards authenticated file upload request

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Internal API Request

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

3. Acknowledges request and returns Job ID

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP |
| Method | POST /assets/import |
| Parameters | Forwards multipart/form-data and Authorization hea... |
| Authentication | Trusts JWT already validated by the Gateway. |
| Error Handling | Propagates errors back to the Gateway. |
| Performance | Internal network latency < 5ms. |

#### 2.5.2.11 Nested Interactions

##### 2.5.2.11.1 Storage Operation

###### 2.5.2.11.1.1 Source Id

REPO-SVC-AST

###### 2.5.2.11.1.2 Target Id

s3-storage-001

###### 2.5.2.11.1.3 Message

2.1. Temporarily stores uploaded file for processing

###### 2.5.2.11.1.4 Sequence Number

2.1

###### 2.5.2.11.1.5 Type

🔹 Storage Operation

###### 2.5.2.11.1.6 Is Synchronous

✅ Yes

###### 2.5.2.11.1.7 Has Return

❌ No

###### 2.5.2.11.1.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | AWS SDK (S3 API) |
| Method | PutObject |
| Parameters | Bucket: 'temp-uploads', Key: '{tenantId}/{jobId}.c... |
| Authentication | IAM Role for EKS Pod. |
| Error Handling | Retry on transient errors. Fails request if upload... |
| Performance | Latency target < 200ms. |

##### 2.5.2.11.2.0 Database Write

###### 2.5.2.11.2.1 Source Id

REPO-SVC-AST

###### 2.5.2.11.2.2 Target Id

postgresql-db-001

###### 2.5.2.11.2.3 Message

2.2. Creates import job record

###### 2.5.2.11.2.4 Sequence Number

2.2

###### 2.5.2.11.2.5 Type

🔹 Database Write

###### 2.5.2.11.2.6 Is Synchronous

✅ Yes

###### 2.5.2.11.2.7 Has Return

❌ No

###### 2.5.2.11.2.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | INSERT INTO AssetImportJobs (JobId, TenantId, Stat... |
| Parameters | Values ('{jobId}', '{tenantId}', 'PENDING', ...) |
| Authentication | DB connection string from Secrets Manager. |
| Error Handling | Fails request if record creation fails. |
| Performance | Latency target < 50ms. |

##### 2.5.2.11.3.0 gRPC Request

###### 2.5.2.11.3.1 Source Id

REPO-SVC-AST

###### 2.5.2.11.3.2 Target Id

REPO-SVC-ADT

###### 2.5.2.11.3.3 Message

2.3. Logs import initiation event

###### 2.5.2.11.3.4 Sequence Number

2.3

###### 2.5.2.11.3.5 Type

🔹 gRPC Request

###### 2.5.2.11.3.6 Is Synchronous

❌ No

###### 2.5.2.11.3.7 Has Return

❌ No

###### 2.5.2.11.3.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | Audit.LogAction |
| Parameters | Action: 'AssetImportInitiated', UserId: '{userId}'... |
| Authentication | Internal mTLS. |
| Error Handling | Fire-and-forget; failure to log does not block the... |
| Performance | Latency target < 20ms. |

##### 2.5.2.11.4.0 Internal Task Dispatch

###### 2.5.2.11.4.1 Source Id

REPO-SVC-AST

###### 2.5.2.11.4.2 Target Id

REPO-SVC-AST-BG-WORKER

###### 2.5.2.11.4.3 Message

2.4. Enqueues background job for processing

###### 2.5.2.11.4.4 Sequence Number

2.4

###### 2.5.2.11.4.5 Type

🔹 Internal Task Dispatch

###### 2.5.2.11.4.6 Is Synchronous

✅ Yes

###### 2.5.2.11.4.7 Has Return

❌ No

###### 2.5.2.11.4.8 Technical Details

| Property | Value |
|----------|-------|
| Protocol | In-Memory Channel/Queue |
| Method | ChannelWriter.WriteAsync |
| Parameters | JobId: '{jobId}' |
| Authentication | N/A (In-process). |
| Error Handling | Fails request if queue is full or cannot be writte... |
| Performance | Latency target < 1ms. |

### 2.5.3.0.0.0 Self Call

#### 2.5.3.1.0.0 Source Id

REPO-SVC-AST-BG-WORKER

#### 2.5.3.2.0.0 Target Id

REPO-SVC-AST-BG-WORKER

#### 2.5.3.3.0.0 Message

5. [Async] Begins processing job

#### 2.5.3.4.0.0 Sequence Number

5

#### 2.5.3.5.0.0 Type

🔹 Self Call

#### 2.5.3.6.0.0 Is Synchronous

✅ Yes

#### 2.5.3.7.0.0 Has Return

❌ No

#### 2.5.3.8.0.0 Is Activation

✅ Yes

#### 2.5.3.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | N/A |
| Method | ProcessImportJob(jobId) |
| Parameters | JobId retrieved from queue. |
| Authentication | N/A |
| Error Handling | Entire job is wrapped in a try-catch block to upda... |
| Performance | N/A |

#### 2.5.3.10.0.0 Nested Interactions

- {'sourceId': 'REPO-SVC-AST-BG-WORKER', 'targetId': 'postgresql-db-001', 'message': '5.1. Updates job status to PROCESSING', 'sequenceNumber': 5.1, 'type': 'Database Write', 'isSynchronous': True, 'hasReturn': False, 'technicalDetails': {'protocol': 'SQL', 'method': "UPDATE AssetImportJobs SET Status = 'PROCESSING' WHERE JobId = '{jobId}'", 'parameters': 'N/A', 'authentication': 'DB connection string from Secrets Manager.', 'errorHandling': 'Logs error and terminates job if update fails.', 'performance': 'Latency target < 50ms.'}}

### 2.5.4.0.0.0 Database Transaction

#### 2.5.4.1.0.0 Source Id

REPO-SVC-AST-BG-WORKER

#### 2.5.4.2.0.0 Target Id

postgresql-db-001

#### 2.5.4.3.0.0 Message

7. BEGIN TRANSACTION

#### 2.5.4.4.0.0 Sequence Number

7

#### 2.5.4.5.0.0 Type

🔹 Database Transaction

#### 2.5.4.6.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0 Return Message

9. COMMIT TRANSACTION (or ROLLBACK)

#### 2.5.4.8.0.0 Has Return

✅ Yes

#### 2.5.4.9.0.0 Is Activation

✅ Yes

#### 2.5.4.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | BEGIN |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | Any failure within this block triggers a ROLLBACK. |
| Performance | Transaction should not exceed 5 minutes. |

#### 2.5.4.11.0.0 Nested Interactions

- {'sourceId': 'REPO-SVC-AST-BG-WORKER', 'targetId': 'postgresql-db-001', 'message': '8. [Loop] Executes bulk insert/update operations for valid rows', 'sequenceNumber': 8, 'type': 'Database Write', 'isSynchronous': True, 'hasReturn': False, 'technicalDetails': {'protocol': 'SQL (Bulk Copy/Insert)', 'method': 'COPY or batched INSERT statements', 'parameters': 'Validated and transformed data records.', 'authentication': 'N/A', 'errorHandling': 'A single failed insert will cause the entire transaction to be rolled back.', 'performance': 'Optimized for bulk operations to minimize round trips.'}}

### 2.5.5.0.0.0 Storage Operation

#### 2.5.5.1.0.0 Source Id

REPO-SVC-AST-BG-WORKER

#### 2.5.5.2.0.0 Target Id

s3-storage-001

#### 2.5.5.3.0.0 Message

11. Uploads summary report

#### 2.5.5.4.0.0 Sequence Number

11

#### 2.5.5.5.0.0 Type

🔹 Storage Operation

#### 2.5.5.6.0.0 Is Synchronous

✅ Yes

#### 2.5.5.7.0.0 Has Return

❌ No

#### 2.5.5.8.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | AWS SDK (S3 API) |
| Method | PutObject |
| Parameters | Bucket: 'import-reports', Key: '{tenantId}/{jobId}... |
| Authentication | IAM Role for EKS Pod. |
| Error Handling | Retry on transient errors. Logs failure but does n... |
| Performance | Latency target < 200ms. |

### 2.5.6.0.0.0 Database Write

#### 2.5.6.1.0.0 Source Id

REPO-SVC-AST-BG-WORKER

#### 2.5.6.2.0.0 Target Id

postgresql-db-001

#### 2.5.6.3.0.0 Message

12. Updates job record with final status and report URL

#### 2.5.6.4.0.0 Sequence Number

12

#### 2.5.6.5.0.0 Type

🔹 Database Write

#### 2.5.6.6.0.0 Is Synchronous

✅ Yes

#### 2.5.6.7.0.0 Has Return

❌ No

#### 2.5.6.8.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | UPDATE AssetImportJobs SET Status = 'COMPLETED/FAI... |
| Parameters | Final state of the job. |
| Authentication | N/A |
| Error Handling | Logs error if update fails. |
| Performance | Latency target < 50ms. |

### 2.5.7.0.0.0 API Request

#### 2.5.7.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.7.2.0.0 Target Id

REPO-GW-API

#### 2.5.7.3.0.0 Message

14. [Polls] Requests job status

#### 2.5.7.4.0.0 Sequence Number

14

#### 2.5.7.5.0.0 Type

🔹 API Request

#### 2.5.7.6.0.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0.0 Return Message

18. Returns job status and report URL to Engineer

#### 2.5.7.8.0.0 Has Return

✅ Yes

#### 2.5.7.9.0.0 Is Activation

❌ No

#### 2.5.7.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | GET /api/v1/assets/import/status/{jobId} |
| Parameters | Path parameter: jobId |
| Authentication | Authorization: Bearer <JWT> |
| Error Handling | Client implements polling logic with exponential b... |
| Performance | Polling interval starts at 2s and increases to 30s... |

### 2.5.8.0.0.0 Internal API Request

#### 2.5.8.1.0.0 Source Id

REPO-GW-API

#### 2.5.8.2.0.0 Target Id

REPO-SVC-AST

#### 2.5.8.3.0.0 Message

15. Forwards status request

#### 2.5.8.4.0.0 Sequence Number

15

#### 2.5.8.5.0.0 Type

🔹 Internal API Request

#### 2.5.8.6.0.0 Is Synchronous

✅ Yes

#### 2.5.8.7.0.0 Return Message

17. Returns current job status

#### 2.5.8.8.0.0 Has Return

✅ Yes

#### 2.5.8.9.0.0 Is Activation

✅ Yes

#### 2.5.8.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP |
| Method | GET /assets/import/status/{jobId} |
| Parameters | N/A |
| Authentication | Forwards validated JWT. |
| Error Handling | Returns 404 if job not found. |
| Performance | Internal latency < 5ms. |

#### 2.5.8.11.0.0 Nested Interactions

- {'sourceId': 'REPO-SVC-AST', 'targetId': 'postgresql-db-001', 'message': '16. Fetches job status from database', 'sequenceNumber': 16, 'type': 'Database Read', 'isSynchronous': True, 'hasReturn': True, 'technicalDetails': {'protocol': 'SQL', 'method': "SELECT Status, ReportUrl FROM AssetImportJobs WHERE JobId = '{jobId}'", 'parameters': 'N/A', 'authentication': 'N/A', 'errorHandling': 'Returns an error if the query fails.', 'performance': 'Query must be indexed by JobId. Latency < 20ms.'}}

## 2.6.0.0.0.0 Notes

### 2.6.1.0.0.0 Content

#### 2.6.1.1.0.0 Content

Business Rule Enforcement (REQ-DM-003): File parsing includes structural checks (headers), data type validation, and business logic validation (e.g., preventing circular dependencies in asset hierarchy). All errors are logged to the summary report.

#### 2.6.1.2.0.0 Position

top-right

#### 2.6.1.3.0.0 Participant Id

REPO-SVC-AST-BG-WORKER

#### 2.6.1.4.0.0 Sequence Number

6

### 2.6.2.0.0.0 Content

#### 2.6.2.1.0.0 Content

Transactionality & Rollback: All database modifications are performed within a single transaction. If any part of the bulk import fails, the entire transaction is rolled back to ensure data consistency, leaving the system in its original state.

#### 2.6.2.2.0.0 Position

bottom-left

#### 2.6.2.3.0.0 Participant Id

postgresql-db-001

#### 2.6.2.4.0.0 Sequence Number

9

### 2.6.3.0.0.0 Content

#### 2.6.3.1.0.0 Content

Validation & Sign-off (REQ-TRN-002): The final summary report provides the necessary details for an engineer to validate the migration, comparing record counts and spot-checking data, before formal sign-off.

#### 2.6.3.2.0.0 Position

bottom-right

#### 2.6.3.3.0.0 Participant Id

REPO-FE-MPL

#### 2.6.3.4.0.0 Sequence Number

18

## 2.7.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | User must have 'Engineer' or 'Administrator' role ... |
| Performance Targets | The initial API response (job creation) must be <5... |
| Error Handling Strategy | File-level errors (e.g., invalid format) result in... |
| Testing Considerations | Unit tests must cover the file parser and validato... |
| Monitoring Requirements | Key metrics to monitor: number of import jobs star... |
| Deployment Considerations | The Asset Service Background Worker can be impleme... |

