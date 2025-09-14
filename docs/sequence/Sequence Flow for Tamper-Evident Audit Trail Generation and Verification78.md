# 1 Overview

## 1.1 Diagram Id

SEQ-CF-001

## 1.2 Name

Tamper-Evident Audit Trail Generation and Verification

## 1.3 Description

A user performs a significant action (e.g., changes a setpoint). The responsible microservice sends a detailed event to the Audit Service, which logs it immutably. Periodically, the Audit Service batches these logs, calculates a cryptographic hash, and anchors it in the Amazon QLDB ledger, providing a verifiable proof of integrity.

## 1.4 Type

🔹 ComplianceFlow

## 1.5 Purpose

To create a comprehensive, immutable, and cryptographically verifiable log of all significant actions, supporting compliance with regulations like FDA 21 CFR Part 11 (REQ-FR-005, REQ-FR-019).

## 1.6 Complexity

High

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-SVC-AST
- REPO-SVC-ADT
- PostgreSQL
- Amazon QLDB

## 1.10 Key Interactions

- The Asset Service processes a request to change a tag's value.
- It calls the Audit Service's gRPC endpoint with details (user, old value, new value, timestamp).
- The Audit Service writes a new, immutable record to its PostgreSQL audit table.
- A scheduled background job in the Audit Service retrieves all new log entries since the last batch.
- The job creates a Merkle tree hash of the batch and writes it to the Amazon QLDB ledger.
- The transaction ID from QLDB is stored back in the relational DB to link the batch to its cryptographic proof.

## 1.11 Triggers

- Any action defined as 'significant' in the SRS, such as data writes, configuration changes, or alarm acknowledgements.

## 1.12 Outcomes

- An immutable audit log is created for the action.
- The integrity of the entire audit log history can be independently and cryptographically verified against the QLDB ledger.

## 1.13 Business Rules

- Audit logs must be immutable, preventing modification or deletion by any user, including administrators (REQ-FR-005).
- Each log entry must contain the responsible user, timestamp, action, and relevant context (REQ-FR-005).
- The ledger is used for verification, not as the primary high-speed logger (REQ-FR-019).

## 1.14 Error Scenarios

- The Audit Service is unavailable, potentially failing the primary action.
- Writing the proof hash to QLDB fails, requiring a retry mechanism.

## 1.15 Integration Points

- Amazon Quantum Ledger Database (QLDB)

# 2.0 Details

## 2.1 Diagram Id

SEQ-CF-001

## 2.2 Name

Implementation: Tamper-Evident Audit Trail Generation and Anchoring

## 2.3 Description

Comprehensive technical flow detailing how a significant user action is captured as an immutable log entry and cryptographically anchored. The sequence begins when the Asset Service handles a state-changing operation, triggering a synchronous gRPC call to the Audit Service. The Audit Service persists this as an immutable record in its PostgreSQL database. A separate, asynchronous background process within the Audit Service periodically batches these logs, calculates a Merkle root hash, and anchors this proof in Amazon QLDB, fulfilling requirements for FDA 21 CFR Part 11.

## 2.4 Participants

### 2.4.1 ExternalActor

#### 2.4.1.1 Repository Id

User via REPO-GW-API

#### 2.4.1.2 Display Name

User via API Gateway

#### 2.4.1.3 Type

🔹 ExternalActor

#### 2.4.1.4 Technology

REST/JSON over HTTPS

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #999999 |
| Stereotype | User |

### 2.4.2.0 Microservice

#### 2.4.2.1 Repository Id

REPO-SVC-AST

#### 2.4.2.2 Display Name

Asset Service

#### 2.4.2.3 Type

🔹 Microservice

#### 2.4.2.4 Technology

.NET 8, gRPC Client

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #1168BD |
| Stereotype | Service |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-ADT

#### 2.4.3.2 Display Name

Audit Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8, gRPC Server, IHostedService

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #1168BD |
| Stereotype | Service |

### 2.4.4.0 Database

#### 2.4.4.1 Repository Id

PostgreSQL

#### 2.4.4.2 Display Name

Audit DB

#### 2.4.4.3 Type

🔹 Database

#### 2.4.4.4 Technology

PostgreSQL 16

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #336791 |
| Stereotype | Relational |

### 2.4.5.0 LedgerDatabase

#### 2.4.5.1 Repository Id

Amazon QLDB

#### 2.4.5.2 Display Name

Amazon QLDB Ledger

#### 2.4.5.3 Type

🔹 LedgerDatabase

#### 2.4.5.4 Technology

Amazon Quantum Ledger Database

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #FF9900 |
| Stereotype | Ledger |

## 2.5.0.0 Interactions

### 2.5.1.0 APIRequest

#### 2.5.1.1 Source Id

User via REPO-GW-API

#### 2.5.1.2 Target Id

REPO-SVC-AST

#### 2.5.1.3 Message

1. POST /api/v1/tags/{id}/value (Request setpoint change)

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 APIRequest

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Has Return

✅ Yes

#### 2.5.1.8 Return Message

8. 200 OK / 500 Internal Server Error

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

##### 2.5.1.10.1 Protocol

HTTPS

##### 2.5.1.10.2 Method

POST

##### 2.5.1.10.3 Parameters

Request body with new value. JWT Bearer token in Authorization header.

##### 2.5.1.10.4 Authentication

JWT validation at API Gateway (REPO-GW-API).

##### 2.5.1.10.5 Error Handling

Standard HTTP error codes.

##### 2.5.1.10.6 Performance

###### 2.5.1.10.6.1 Latency

<200ms P95

### 2.5.2.0.0.0 gRPC

#### 2.5.2.1.0.0 Source Id

REPO-SVC-AST

#### 2.5.2.2.0.0 Target Id

REPO-SVC-ADT

#### 2.5.2.3.0.0 Message

2. Call LogAction(LogActionRequest)

#### 2.5.2.4.0.0 Sequence Number

2

#### 2.5.2.5.0.0 Type

🔹 gRPC

#### 2.5.2.6.0.0 Is Synchronous

✅ Yes

#### 2.5.2.7.0.0 Has Return

✅ Yes

#### 2.5.2.8.0.0 Return Message

4. LogActionResponse

#### 2.5.2.9.0.0 Is Activation

❌ No

#### 2.5.2.10.0.0 Technical Details

##### 2.5.2.10.1.0 Protocol

gRPC

##### 2.5.2.10.2.0 Method

audit.AuditService.LogAction

##### 2.5.2.10.3.0 Parameters

LogActionRequest { actor, action, target, old_value, new_value }. Metadata includes propagated user JWT.

##### 2.5.2.10.4.0 Authentication

Requires valid, propagated JWT. Service-to-service mTLS.

##### 2.5.2.10.5.0 Error Handling

If this call fails after retries, the entire parent operation (step 1) MUST fail and return a 500 error. No setpoint change occurs if it cannot be audited.

##### 2.5.2.10.6.0 Performance

###### 2.5.2.10.6.1 Latency

<50ms

### 2.5.3.0.0.0 Database

#### 2.5.3.1.0.0 Source Id

REPO-SVC-ADT

#### 2.5.3.2.0.0 Target Id

PostgreSQL

#### 2.5.3.3.0.0 Message

3. INSERT INTO audit_logs (...)

#### 2.5.3.4.0.0 Sequence Number

3

#### 2.5.3.5.0.0 Type

🔹 Database

#### 2.5.3.6.0.0 Is Synchronous

✅ Yes

#### 2.5.3.7.0.0 Has Return

✅ Yes

#### 2.5.3.8.0.0 Return Message

Success/Failure

#### 2.5.3.9.0.0 Is Activation

✅ Yes

#### 2.5.3.10.0.0 Technical Details

##### 2.5.3.10.1.0 Protocol

SQL

##### 2.5.3.10.2.0 Method

```sql
INSERT
```

##### 2.5.3.10.3.0 Parameters

```sql
INSERT INTO audit_logs (actor_id, action_type, target_entity, old_value, new_value, timestamp) VALUES (...).
```

##### 2.5.3.10.4.0 Authentication

Connection string credentials.

##### 2.5.3.10.5.0 Error Handling

Database exceptions are caught and propagated back as a gRPC INTERNAL error.

##### 2.5.3.10.6.0 Performance

###### 2.5.3.10.6.1 Latency

<10ms

### 2.5.4.0.0.0 InternalProcessing

#### 2.5.4.1.0.0 Source Id

REPO-SVC-AST

#### 2.5.4.2.0.0 Target Id

REPO-SVC-AST

#### 2.5.4.3.0.0 Message

5. Commit primary business logic (e.g., write to OPC server)

#### 2.5.4.4.0.0 Sequence Number

5

#### 2.5.4.5.0.0 Type

🔹 InternalProcessing

#### 2.5.4.6.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0 Has Return

❌ No

#### 2.5.4.8.0.0 Return Message



#### 2.5.4.9.0.0 Is Activation

❌ No

#### 2.5.4.10.0.0 Technical Details

##### 2.5.4.10.1.0 Protocol

Internal

##### 2.5.4.10.2.0 Method



##### 2.5.4.10.3.0 Parameters



##### 2.5.4.10.4.0 Authentication



##### 2.5.4.10.5.0 Error Handling

If this step fails, the audit log entry remains but the action did not complete. This state must be clearly identifiable.

##### 2.5.4.10.6.0 Performance

*No data available*

### 2.5.5.0.0.0 ScheduledTask

#### 2.5.5.1.0.0 Source Id

REPO-SVC-ADT

#### 2.5.5.2.0.0 Target Id

REPO-SVC-ADT

#### 2.5.5.3.0.0 Message

6. [Async] Scheduled background job triggers

#### 2.5.5.4.0.0 Sequence Number

6

#### 2.5.5.5.0.0 Type

🔹 ScheduledTask

#### 2.5.5.6.0.0 Is Synchronous

❌ No

#### 2.5.5.7.0.0 Has Return

❌ No

#### 2.5.5.8.0.0 Return Message



#### 2.5.5.9.0.0 Is Activation

✅ Yes

#### 2.5.5.10.0.0 Technical Details

##### 2.5.5.10.1.0 Protocol

Internal

##### 2.5.5.10.2.0 Method

.NET IHostedService with PeriodicTimer

##### 2.5.5.10.3.0 Parameters

e.g., every 5 minutes.

##### 2.5.5.10.4.0 Authentication

N/A

##### 2.5.5.10.5.0 Error Handling

Job execution is logged. Failures trigger alerts and are retried on the next interval.

##### 2.5.5.10.6.0 Performance

*No data available*

### 2.5.6.0.0.0 Database

#### 2.5.6.1.0.0 Source Id

REPO-SVC-ADT

#### 2.5.6.2.0.0 Target Id

PostgreSQL

#### 2.5.6.3.0.0 Message

7. SELECT * FROM audit_logs WHERE qldb_tx_id IS NULL

#### 2.5.6.4.0.0 Sequence Number

7

#### 2.5.6.5.0.0 Type

🔹 Database

#### 2.5.6.6.0.0 Is Synchronous

✅ Yes

#### 2.5.6.7.0.0 Has Return

✅ Yes

#### 2.5.6.8.0.0 Return Message

List of un-anchored log entries

#### 2.5.6.9.0.0 Is Activation

❌ No

#### 2.5.6.10.0.0 Technical Details

##### 2.5.6.10.1.0 Protocol

SQL

##### 2.5.6.10.2.0 Method

```sql
SELECT
```

##### 2.5.6.10.3.0 Parameters

Selects all log entries that have not yet been included in an anchored batch.

##### 2.5.6.10.4.0 Authentication

Connection string credentials.

##### 2.5.6.10.5.0 Error Handling

If query fails, log error and retry on next job interval.

##### 2.5.6.10.6.0 Performance

*No data available*

### 2.5.7.0.0.0 InternalProcessing

#### 2.5.7.1.0.0 Source Id

REPO-SVC-ADT

#### 2.5.7.2.0.0 Target Id

REPO-SVC-ADT

#### 2.5.7.3.0.0 Message

8. Calculate Merkle root hash for the batch

#### 2.5.7.4.0.0 Sequence Number

8

#### 2.5.7.5.0.0 Type

🔹 InternalProcessing

#### 2.5.7.6.0.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0.0 Has Return

❌ No

#### 2.5.7.8.0.0 Return Message



#### 2.5.7.9.0.0 Is Activation

❌ No

#### 2.5.7.10.0.0 Technical Details

##### 2.5.7.10.1.0 Protocol

Internal

##### 2.5.7.10.2.0 Method

Cryptographic hashing algorithm (e.g., SHA-256)

##### 2.5.7.10.3.0 Parameters

The list of log entries from step 7.

##### 2.5.7.10.4.0 Authentication

N/A

##### 2.5.7.10.5.0 Error Handling

Calculation errors are critical and must be logged and alerted.

##### 2.5.7.10.6.0 Performance

*No data available*

### 2.5.8.0.0.0 Database

#### 2.5.8.1.0.0 Source Id

REPO-SVC-ADT

#### 2.5.8.2.0.0 Target Id

Amazon QLDB

#### 2.5.8.3.0.0 Message

9. INSERT INTO audit_proofs ...

#### 2.5.8.4.0.0 Sequence Number

9

#### 2.5.8.5.0.0 Type

🔹 Database

#### 2.5.8.6.0.0 Is Synchronous

✅ Yes

#### 2.5.8.7.0.0 Has Return

✅ Yes

#### 2.5.8.8.0.0 Return Message

Transaction ID

#### 2.5.8.9.0.0 Is Activation

✅ Yes

#### 2.5.8.10.0.0 Technical Details

##### 2.5.8.10.1.0 Protocol

PartiQL via AWS SDK

##### 2.5.8.10.2.0 Method

```sql
INSERT
```

##### 2.5.8.10.3.0 Parameters

```sql
INSERT INTO audit_proofs VALUE {'batch_hash': '...', 'start_timestamp': '...', 'end_timestamp': '...', 'log_ids': [...] }
```

##### 2.5.8.10.4.0 Authentication

AWS IAM Role associated with the Audit Service pod.

##### 2.5.8.10.5.0 Error Handling

Failure triggers a retry with exponential backoff. The batch remains un-anchored until success.

##### 2.5.8.10.6.0 Performance

*No data available*

### 2.5.9.0.0.0 Database

#### 2.5.9.1.0.0 Source Id

REPO-SVC-ADT

#### 2.5.9.2.0.0 Target Id

PostgreSQL

#### 2.5.9.3.0.0 Message

10. UPDATE audit_logs SET qldb_tx_id = ...

#### 2.5.9.4.0.0 Sequence Number

10

#### 2.5.9.5.0.0 Type

🔹 Database

#### 2.5.9.6.0.0 Is Synchronous

✅ Yes

#### 2.5.9.7.0.0 Has Return

✅ Yes

#### 2.5.9.8.0.0 Return Message

Success/Failure

#### 2.5.9.9.0.0 Is Activation

❌ No

#### 2.5.9.10.0.0 Technical Details

##### 2.5.9.10.1.0 Protocol

SQL

##### 2.5.9.10.2.0 Method

```sql
UPDATE
```

##### 2.5.9.10.3.0 Parameters

```sql
UPDATE audit_logs SET qldb_tx_id = '[tx_id]', qldb_doc_id = '[doc_id]' WHERE id IN (...log_ids...).
```

##### 2.5.9.10.4.0 Authentication

Connection string credentials.

##### 2.5.9.10.5.0 Error Handling

This is a critical step. A failure here after QLDB write causes inconsistency. The operation must be idempotent and retried until successful. Generate a CRITICAL alert after N failures.

##### 2.5.9.10.6.0 Performance

*No data available*

## 2.6.0.0.0.0 Notes

### 2.6.1.0.0.0 Content

#### 2.6.1.1.0.0 Content

The 'audit_logs' table in PostgreSQL MUST be made immutable. This can be achieved via database triggers that prevent UPDATE/DELETE operations, or by granting only INSERT and SELECT permissions to the application's database user.

#### 2.6.1.2.0.0 Position

bottom-left

#### 2.6.1.3.0.0 Participant Id

PostgreSQL

#### 2.6.1.4.0.0 Sequence Number

3

### 2.6.2.0.0.0 Content

#### 2.6.2.1.0.0 Content

The synchronous part of this flow (steps 2-4) is critical for compliance. The user's action cannot succeed unless it is successfully logged. This design choice prioritizes data integrity and auditability over availability in case of an Audit Service failure.

#### 2.6.2.2.0.0 Position

top-right

#### 2.6.2.3.0.0 Participant Id

REPO-SVC-ADT

#### 2.6.2.4.0.0 Sequence Number

2

### 2.6.3.0.0.0 Content

#### 2.6.3.1.0.0 Content

The asynchronous batching process decouples the performance of the high-speed logger from the ledger database, as required by REQ-FR-019.

#### 2.6.3.2.0.0 Position

bottom-right

#### 2.6.3.3.0.0 Participant Id

REPO-SVC-ADT

#### 2.6.3.4.0.0 Sequence Number

6

## 2.7.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | User's JWT must be propagated from the Asset Servi... |
| Performance Targets | The synchronous gRPC `LogAction` call (step 2) mus... |
| Error Handling Strategy | A failure in the synchronous path (steps 2, 3) mus... |
| Testing Considerations | Unit tests must verify the Merkle hash calculation... |
| Monitoring Requirements | Monitor the gRPC error rate and latency for the `L... |
| Deployment Considerations | Database schema migrations for the `audit_logs` ta... |

