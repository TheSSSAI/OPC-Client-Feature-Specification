# 1 Overview

## 1.1 Diagram Id

SEQ-BP-003

## 1.2 Name

System Cutover for a New Site

## 1.3 Description

Following a pre-approved plan, a deployment team executes the cutover to the new system at a specific site. This involves a final data migration, a formal go/no-go decision, switching data sources to the new system, and a period of hyper-care support. A documented fallback plan is in place to revert if necessary.

## 1.4 Type

🔹 BusinessProcess

## 1.5 Purpose

To provide a structured, low-risk process for transitioning a production site from a legacy system to the new platform, as detailed in the Transition Requirements (REQ-TRN-004).

## 1.6 Complexity

Critical

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

OnDemand

## 1.9 Participants

- Deployment Team
- Site Engineer
- REPO-SVC-AST
- REPO-EDGE-OPC
- Legacy System

## 1.10 Key Interactions

- Complete pre-cutover checklist (user training, final data migration, network configuration).
- Hold a formal go/no-go meeting based on UAT results and checklist completion.
- Disable write access to the legacy system to freeze its state.
- Deploy and activate the OPC Core Client(s) for the site, pointing them to live OPC servers.
- Validate that data is flowing correctly into the new Central Management Plane.
- Initiate a 72-hour hyper-care support period with dedicated staff.
- After a successful parallel run period (e.g., 30 days), formally decommission the legacy system.

## 1.11 Triggers

- The completion of all pre-requisite tasks for a site deployment (e.g., Phase 1 pilot or Phase 2 regional rollout).

## 1.12 Outcomes

- The site is successfully transitioned to the new system.
- The new system becomes the single source of truth for the site's operational data.

## 1.13 Business Rules

- A documented fallback procedure must be in place to revert to the legacy system within a 2-hour window (REQ-TRN-004).
- The legacy system shall run in parallel in read-only mode for a defined period (minimum 30 days) for data validation (REQ-TRN-005).

## 1.14 Error Scenarios

- Critical success criteria are not met post-launch, triggering the execution of the fallback plan.
- Data validation post-cutover reveals significant discrepancies between the old and new systems.
- The new system experiences a critical failure during the hyper-care period.

## 1.15 Integration Points

- Legacy SCADA/Historian Systems (e.g., OSIsoft PI, Wonderware)

# 2.0 Details

## 2.1 Diagram Id

SEQ-BP-003-IMPL

## 2.2 Name

Implementation: System Cutover Process for a New Site

## 2.3 Description

Provides the detailed technical sequence for transitioning a production site to the new system. This process is orchestrated by a Deployment Team and involves state changes, manual verification, and a critical rollback path. The process state must be tracked in a persistent store, likely managed by a workflow or deployment management service.

## 2.4 Participants

### 2.4.1 Actor

#### 2.4.1.1 Repository Id

Deployment Team

#### 2.4.1.2 Display Name

Deployment Team

#### 2.4.1.3 Type

🔹 Actor

#### 2.4.1.4 Technology

Human Actor with Admin Permissions

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #E6F3FF |
| Stereotype | Human |

### 2.4.2.0 Actor

#### 2.4.2.1 Repository Id

Site Engineer

#### 2.4.2.2 Display Name

Site Engineer

#### 2.4.2.3 Type

🔹 Actor

#### 2.4.2.4 Technology

Human Actor with Engineer/Operator Permissions

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #E6F3FF |
| Stereotype | Human |

### 2.4.3.0 ExternalSystem

#### 2.4.3.1 Repository Id

Legacy System

#### 2.4.3.2 Display Name

Legacy System

#### 2.4.3.3 Type

🔹 ExternalSystem

#### 2.4.3.4 Technology

e.g., OSIsoft PI, Wonderware Historian

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #D3D3D3 |
| Stereotype | External |

### 2.4.4.0 Service

#### 2.4.4.1 Repository Id

REPO-SVC-AST

#### 2.4.4.2 Display Name

Asset & Topology Svc.

#### 2.4.4.3 Type

🔹 Service

#### 2.4.4.4 Technology

.NET 8, PostgreSQL/TimescaleDB

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #C2F0C2 |
| Stereotype | Microservice |

### 2.4.5.0 EdgeApplication

#### 2.4.5.1 Repository Id

REPO-EDGE-OPC

#### 2.4.5.2 Display Name

OPC Core Client

#### 2.4.5.3 Type

🔹 EdgeApplication

#### 2.4.5.4 Technology

.NET 8, Docker

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #FAD7A0 |
| Stereotype | Edge |

## 2.5.0.0 Interactions

### 2.5.1.0 Manual Operation

#### 2.5.1.1 Source Id

Deployment Team

#### 2.5.1.2 Target Id

Legacy System

#### 2.5.1.3 Message

1. Execute Final Data Migration Script

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Manual Operation

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

2. Data Export Complete

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

❌ No

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Legacy Protocol/ETL |
| Method | Data Extract & Transform |
| Parameters | Scope: Final delta of configuration and historical... |
| Authentication | Service Account Credentials |
| Error Handling | Log script errors; manual verification of record c... |
| Performance | Dependent on data volume; must complete within pre... |

#### 2.5.1.11 Nested Interactions

- {'sourceId': 'Deployment Team', 'targetId': 'REPO-SVC-AST', 'message': '1.1. Ingest Migrated Data via Bulk Import API', 'sequenceNumber': 1.1, 'type': 'API Call', 'isSynchronous': True, 'returnMessage': '1.2. HTTP 202 Accepted (Import Job ID)', 'hasReturn': True, 'isActivation': True, 'technicalDetails': {'protocol': 'HTTP/REST', 'method': 'POST /api/v1/migration/bulk-import', 'parameters': 'Body: Formatted JSON/CSV data (REQ-DM-003)', 'authentication': 'JWT Bearer Token (Admin Role)', 'errorHandling': 'HTTP 400 for validation errors; job status must be polled.', 'performance': 'Asynchronous processing; completion time depends on data volume.'}}

### 2.5.2.0 Manual Operation

#### 2.5.2.1 Source Id

Deployment Team

#### 2.5.2.2 Target Id

Site Engineer

#### 2.5.2.3 Message

3. Confirm Pre-Cutover Checklist Complete & Hold Go/No-Go Meeting

#### 2.5.2.4 Sequence Number

3

#### 2.5.2.5 Type

🔹 Manual Operation

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

4. Go Decision Confirmed

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

❌ No

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Verbal/Email Communication |
| Method | Formal Meeting |
| Parameters | Checklist: UAT Sign-off, Training Complete, Migrat... |
| Authentication | N/A |
| Error Handling | A 'No-Go' decision halts the cutover process. |
| Performance | N/A |

### 2.5.3.0 Manual Admin Operation

#### 2.5.3.1 Source Id

Deployment Team

#### 2.5.3.2 Target Id

Legacy System

#### 2.5.3.3 Message

5. Disable Write Access (Freeze System State)

#### 2.5.3.4 Sequence Number

5

#### 2.5.3.5 Type

🔹 Manual Admin Operation

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

6. Write Access Disabled

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Database/Application Admin Console |
| Method | Change user permissions to read-only. |
| Parameters | All operational user accounts. |
| Authentication | Legacy System Admin Credentials |
| Error Handling | Verify permissions post-change. Failure requires i... |
| Performance | Should be near-instantaneous. |

### 2.5.4.0 Command

#### 2.5.4.1 Source Id

Deployment Team

#### 2.5.4.2 Target Id

REPO-EDGE-OPC

#### 2.5.4.3 Message

7. Send 'Activate Live Mode' Command

#### 2.5.4.4 Sequence Number

7

#### 2.5.4.5 Type

🔹 Command

#### 2.5.4.6 Is Synchronous

❌ No

#### 2.5.4.7 Return Message



#### 2.5.4.8 Has Return

❌ No

#### 2.5.4.9 Is Activation

✅ Yes

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 |
| Method | PUBLISH to tenants/{id}/clients/{id}/commands/acti... |
| Parameters | Payload: { config: 'live_opc_servers.json', mode: ... |
| Authentication | TLS Client Certificate |
| Error Handling | MQTT QoS 1 ensures at-least-once delivery. |
| Performance | Latency < 200ms. |

### 2.5.5.0 Data Stream

#### 2.5.5.1 Source Id

REPO-EDGE-OPC

#### 2.5.5.2 Target Id

REPO-SVC-AST

#### 2.5.5.3 Message

8. [Loop] Stream Live Telemetry Data

#### 2.5.5.4 Sequence Number

8

#### 2.5.5.5 Type

🔹 Data Stream

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
| Protocol | gRPC |
| Method | IngestService.StreamData |
| Parameters | Stream of DataPoint messages. |
| Authentication | mTLS Client Certificate |
| Error Handling | Client uses on-disk buffer and retry with exponent... |
| Performance | High-throughput (up to 10k values/sec), low-latenc... |

### 2.5.6.0 API Call

#### 2.5.6.1 Source Id

Site Engineer

#### 2.5.6.2 Target Id

REPO-SVC-AST

#### 2.5.6.3 Message

9. Request Data for Validation

#### 2.5.6.4 Sequence Number

9

#### 2.5.6.5 Type

🔹 API Call

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message

10. HTTP 200 OK (Time-series Data)

#### 2.5.6.8 Has Return

✅ Yes

#### 2.5.6.9 Is Activation

❌ No

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/REST |
| Method | GET /api/v1/query/history?tag=...&start=...&end=..... |
| Parameters | Query parameters for specific tags and time ranges... |
| Authentication | JWT Bearer Token (Engineer Role) |
| Error Handling | Standard HTTP error codes. UI should handle API er... |
| Performance | P95 latency < 1s for 24h query (REQ-1-075). |

#### 2.5.6.11 Nested Interactions

##### 2.5.6.11.1 Manual Query

###### 2.5.6.11.1.1 Source Id

Site Engineer

###### 2.5.6.11.1.2 Target Id

Legacy System

###### 2.5.6.11.1.3 Message

9.1. Request Read-Only Data for Comparison

###### 2.5.6.11.1.4 Sequence Number

9.1

###### 2.5.6.11.1.5 Type

🔹 Manual Query

###### 2.5.6.11.1.6 Is Synchronous

✅ Yes

###### 2.5.6.11.1.7 Return Message

9.2. Legacy Data

###### 2.5.6.11.1.8 Has Return

✅ Yes

###### 2.5.6.11.1.9 Is Activation

❌ No

###### 2.5.6.11.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Legacy UI/SQL Client |
| Method | Query historical data. |
| Parameters | Matching tags and time ranges. |
| Authentication | Read-Only User Credentials |
| Error Handling | N/A |
| Performance | N/A |

##### 2.5.6.11.2.0 Manual Operation

###### 2.5.6.11.2.1 Source Id

Site Engineer

###### 2.5.6.11.2.2 Target Id

Deployment Team

###### 2.5.6.11.2.3 Message

11. [Alt] Validation Outcome

###### 2.5.6.11.2.4 Sequence Number

11

###### 2.5.6.11.2.5 Type

🔹 Manual Operation

###### 2.5.6.11.2.6 Is Synchronous

✅ Yes

###### 2.5.6.11.2.7 Return Message



###### 2.5.6.11.2.8 Has Return

❌ No

###### 2.5.6.11.2.9 Is Activation

❌ No

###### 2.5.6.11.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Verbal/Email Communication |
| Method | Report Findings |
| Parameters | Validation success or failure with evidence. |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | N/A |

###### 2.5.6.11.2.11 Nested Interactions

####### 2.5.6.11.2.11.1 Conditional Flow

######## 2.5.6.11.2.11.1.1 Source Id

Deployment Team

######## 2.5.6.11.2.11.1.2 Target Id

Deployment Team

######## 2.5.6.11.2.11.1.3 Message

12. [Opt: Validation Success]

######## 2.5.6.11.2.11.1.4 Sequence Number

12

######## 2.5.6.11.2.11.1.5 Type

🔹 Conditional Flow

######## 2.5.6.11.2.11.1.6 Is Synchronous

✅ Yes

######## 2.5.6.11.2.11.1.7 Return Message



######## 2.5.6.11.2.11.1.8 Has Return

❌ No

######## 2.5.6.11.2.11.1.9 Is Activation

❌ No

######## 2.5.6.11.2.11.1.10 Technical Details

*No data available*

######## 2.5.6.11.2.11.1.11 Nested Interactions

######### 2.5.6.11.2.11.1.11.1 Manual Operation

########## 2.5.6.11.2.11.1.11.1.1 Source Id

Deployment Team

########## 2.5.6.11.2.11.1.11.1.2 Target Id

Site Engineer

########## 2.5.6.11.2.11.1.11.1.3 Message

12.1. Initiate 72-hour Hyper-Care Support & 30-day Parallel Run

########## 2.5.6.11.2.11.1.11.1.4 Sequence Number

12.1

########## 2.5.6.11.2.11.1.11.1.5 Type

🔹 Manual Operation

########## 2.5.6.11.2.11.1.11.1.6 Is Synchronous

✅ Yes

########## 2.5.6.11.2.11.1.11.1.7 Return Message

12.2. Acknowledge

########## 2.5.6.11.2.11.1.11.1.8 Has Return

✅ Yes

######### 2.5.6.11.2.11.1.11.2.0 Manual Admin Operation

########## 2.5.6.11.2.11.1.11.2.1 Source Id

Deployment Team

########## 2.5.6.11.2.11.1.11.2.2 Target Id

Legacy System

########## 2.5.6.11.2.11.1.11.2.3 Message

12.3. After 30 days, Decommission Legacy System

########## 2.5.6.11.2.11.1.11.2.4 Sequence Number

12.3

########## 2.5.6.11.2.11.1.11.2.5 Type

🔹 Manual Admin Operation

########## 2.5.6.11.2.11.1.11.2.6 Is Synchronous

✅ Yes

########## 2.5.6.11.2.11.1.11.2.7 Return Message

12.4. Decommissioning Complete

########## 2.5.6.11.2.11.1.11.2.8 Has Return

✅ Yes

####### 2.5.6.11.2.11.2.0.0.0 Conditional Flow

######## 2.5.6.11.2.11.2.1.0.0 Source Id

Deployment Team

######## 2.5.6.11.2.11.2.2.0.0 Target Id

Deployment Team

######## 2.5.6.11.2.11.2.3.0.0 Message

13. [Opt: Validation Failure - Execute Fallback Plan]

######## 2.5.6.11.2.11.2.4.0.0 Sequence Number

13

######## 2.5.6.11.2.11.2.5.0.0 Type

🔹 Conditional Flow

######## 2.5.6.11.2.11.2.6.0.0 Is Synchronous

✅ Yes

######## 2.5.6.11.2.11.2.7.0.0 Return Message



######## 2.5.6.11.2.11.2.8.0.0 Has Return

❌ No

######## 2.5.6.11.2.11.2.9.0.0 Is Activation

❌ No

######## 2.5.6.11.2.11.2.10.0.0 Technical Details

*No data available*

######## 2.5.6.11.2.11.2.11.0.0 Nested Interactions

######### 2.5.6.11.2.11.2.11.1.0 Command

########## 2.5.6.11.2.11.2.11.1.1 Source Id

Deployment Team

########## 2.5.6.11.2.11.2.11.1.2 Target Id

REPO-EDGE-OPC

########## 2.5.6.11.2.11.2.11.1.3 Message

13.1. Send 'Deactivate' Command

########## 2.5.6.11.2.11.2.11.1.4 Sequence Number

13.1

########## 2.5.6.11.2.11.2.11.1.5 Type

🔹 Command

########## 2.5.6.11.2.11.2.11.1.6 Is Synchronous

❌ No

########## 2.5.6.11.2.11.2.11.1.7 Return Message



########## 2.5.6.11.2.11.2.11.1.8 Has Return

❌ No

########## 2.5.6.11.2.11.2.11.1.9 Is Activation

❌ No

######### 2.5.6.11.2.11.2.11.2.0 Manual Admin Operation

########## 2.5.6.11.2.11.2.11.2.1 Source Id

Deployment Team

########## 2.5.6.11.2.11.2.11.2.2 Target Id

Legacy System

########## 2.5.6.11.2.11.2.11.2.3 Message

13.2. Re-enable Write Access

########## 2.5.6.11.2.11.2.11.2.4 Sequence Number

13.2

########## 2.5.6.11.2.11.2.11.2.5 Type

🔹 Manual Admin Operation

########## 2.5.6.11.2.11.2.11.2.6 Is Synchronous

✅ Yes

########## 2.5.6.11.2.11.2.11.2.7 Return Message

13.3. Write Access Restored

########## 2.5.6.11.2.11.2.11.2.8 Has Return

✅ Yes

########## 2.5.6.11.2.11.2.11.2.9 Is Activation

❌ No

######### 2.5.6.11.2.11.2.11.3.0 Manual Operation

########## 2.5.6.11.2.11.2.11.3.1 Source Id

Deployment Team

########## 2.5.6.11.2.11.2.11.3.2 Target Id

Site Engineer

########## 2.5.6.11.2.11.2.11.3.3 Message

13.4. Notify Stakeholders of Rollback

########## 2.5.6.11.2.11.2.11.3.4 Sequence Number

13.4

########## 2.5.6.11.2.11.2.11.3.5 Type

🔹 Manual Operation

########## 2.5.6.11.2.11.2.11.3.6 Is Synchronous

❌ No

########## 2.5.6.11.2.11.2.11.3.7 Return Message



########## 2.5.6.11.2.11.2.11.3.8 Has Return

❌ No

## 2.6.0.0.0.0.0.0.0.0 Notes

### 2.6.1.0.0.0.0.0.0.0 Content

#### 2.6.1.1.0.0.0.0.0.0 Content

Business Rule (REQ-TRN-004): Fallback plan must be executed within a 2-hour window if critical success criteria are not met post-launch.

#### 2.6.1.2.0.0.0.0.0.0 Position

bottom

#### 2.6.1.3.0.0.0.0.0.0 Participant Id

*Not specified*

#### 2.6.1.4.0.0.0.0.0.0 Sequence Number

13

### 2.6.2.0.0.0.0.0.0.0 Content

#### 2.6.2.1.0.0.0.0.0.0 Content

Business Rule (REQ-TRN-005): Legacy system runs in read-only parallel mode for a minimum of 30 days to allow for data validation.

#### 2.6.2.2.0.0.0.0.0.0 Position

bottom

#### 2.6.2.3.0.0.0.0.0.0 Participant Id

*Not specified*

#### 2.6.2.4.0.0.0.0.0.0 Sequence Number

12.1

### 2.6.3.0.0.0.0.0.0.0 Content

#### 2.6.3.1.0.0.0.0.0.0 Content

Hyper-care support (minimum 72 hours) with dedicated technical staff is provided immediately following the cutover.

#### 2.6.3.2.0.0.0.0.0.0 Position

bottom

#### 2.6.3.3.0.0.0.0.0.0 Participant Id

*Not specified*

#### 2.6.3.4.0.0.0.0.0.0 Sequence Number

12.1

## 2.7.0.0.0.0.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | The 'Deployment Team' actor requires Administrator... |
| Performance Targets | Data validation queries (Step 9) must meet the P95... |
| Error Handling Strategy | The primary error handling strategy is the documen... |
| Testing Considerations | A full-scale 'cutover drill' must be performed in ... |
| Monitoring Requirements | During the hyper-care and parallel run periods, a ... |
| Deployment Considerations | Each site cutover requires a detailed, time-boxed,... |

