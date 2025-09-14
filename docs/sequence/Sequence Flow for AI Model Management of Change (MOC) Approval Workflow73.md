# 1 Overview

## 1.1 Diagram Id

SEQ-BP-001

## 1.2 Name

AI Model Management of Change (MOC) Approval Workflow

## 1.3 Description

A Data Scientist uploads a new AI model and submits it for approval. An Administrator is notified, reviews the model's details and validation metrics, and then either approves or rejects its use in production. The decision triggers an auditable event and changes the model's state, making it available (or not) for deployment.

## 1.4 Type

🔹 BusinessProcess

## 1.5 Purpose

To enforce a formal, auditable approval process for deploying new AI models to production assets, aligning with organizational Management of Change (MOC) policies (REQ-CON-005).

## 1.6 Complexity

Medium

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-AML
- REPO-SVC-ANM
- REPO-SVC-ADT

## 1.10 Key Interactions

- Data Scientist submits a model via the UI.
- AI/ML Service saves the model file to S3 and updates its database status to 'Pending Approval'.
- The service publishes an event or calls the Notification Service to alert users with the Administrator role.
- An Administrator reviews the model in the UI and clicks 'Approve'.
- The UI calls the AI/ML Service, which updates the model status to 'Approved'.
- The AI/ML Service asynchronously calls the Audit Service to log the approval action, including the approver's ID and timestamp.

## 1.11 Triggers

- A Data Scientist submits a trained AI model for validation and production use.

## 1.12 Outcomes

- The AI model is approved and becomes available in the library for Engineers to assign to assets.
- A tamper-evident audit log of the approval decision is created (REQ-FR-005).

## 1.13 Business Rules

- Only users with the Administrator role can import, validate, and approve AI models (REQ-USR-001).
- The approval workflow must be configurable to align with MOC procedures (REQ-CON-005).

## 1.14 Error Scenarios

- The model upload to S3 fails.
- The Administrator rejects the model, setting its status to 'Rejected'.
- The audit log creation fails, though the approval may still succeed (compensating transaction may be needed).

## 1.15 Integration Points

- Amazon S3 for model storage

# 2.0 Details

## 2.1 Diagram Id

SEQ-BP-001

## 2.2 Name

AI Model Management of Change (MOC) Approval Workflow

## 2.3 Description

Implementation-ready sequence for the AI Model approval business process. This sequence details the two primary phases: 1) A Data Scientist submits a model for review, triggering storage and an administrative notification. 2) An Administrator reviews and approves/rejects the model, which updates the model's state and creates a mandatory, tamper-evident audit record.

## 2.4 Participants

### 2.4.1 Web SPA

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Frontend: Management Plane

#### 2.4.1.3 Type

🔹 Web SPA

#### 2.4.1.4 Technology

React 18, TypeScript, Axios

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | UI |

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
| Shape | boundary |
| Color | #FFD700 |
| Stereotype | Gateway |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-AML

#### 2.4.3.2 Display Name

AI/ML Management Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8, ASP.NET Core, AWS S3 SDK

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #32CD32 |
| Stereotype | Service |

### 2.4.4.0 Microservice

#### 2.4.4.1 Repository Id

REPO-SVC-ANM

#### 2.4.4.2 Display Name

Alarm & Notification Service

#### 2.4.4.3 Type

🔹 Microservice

#### 2.4.4.4 Technology

.NET 8, ASP.NET Core

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #FF6347 |
| Stereotype | Service |

### 2.4.5.0 Microservice

#### 2.4.5.1 Repository Id

REPO-SVC-ADT

#### 2.4.5.2 Display Name

Audit Service

#### 2.4.5.3 Type

🔹 Microservice

#### 2.4.5.4 Technology

.NET 8, gRPC

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #9370DB |
| Stereotype | Service |

## 2.5.0.0 Interactions

### 2.5.1.0 Request

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

1. [Data Scientist] POST /api/v1/models (multipart/form-data)

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

201 Created

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/REST |
| Method | POST |
| Parameters | Multipart form data containing: { modelName: strin... |
| Authentication | JWT Bearer Token required (Data Scientist role) |
| Error Handling | 400 Bad Request if form validation fails. 401 Unau... |
| Performance | P95 latency < 200ms (gateway overhead only) |

#### 2.5.1.11 Nested Interactions

- {'sourceId': 'REPO-GW-API', 'targetId': 'REPO-SVC-AML', 'message': '1.1. Route validated request to service', 'sequenceNumber': 2, 'type': 'Proxy', 'isSynchronous': True, 'returnMessage': '201 Created', 'hasReturn': True, 'isActivation': True, 'technicalDetails': {'protocol': 'HTTP (internal)', 'method': 'POST', 'parameters': 'Forwarded multipart form data', 'authentication': 'JWT is validated; claims are forwarded in request headers.', 'errorHandling': '502 Bad Gateway if upstream service is unreachable.', 'performance': 'Internal network latency < 5ms'}, 'nestedInteractions': [{'sourceId': 'REPO-SVC-AML', 'targetId': 'REPO-SVC-AML', 'message': '1.1.1. Upload model file to S3 object storage', 'sequenceNumber': 3, 'type': 'ExternalCall', 'isSynchronous': True, 'returnMessage': 'S3 object URL', 'hasReturn': True, 'technicalDetails': {'protocol': 'AWS S3 API', 'method': 'PutObject', 'parameters': 'Bucket name, object key (e.g., tenantId/modelId.onnx), file stream', 'authentication': 'IAM Role credentials for S3 write access', 'errorHandling': 'If upload fails, rollback transaction and return 503 Service Unavailable.', 'performance': 'Dependent on file size and network bandwidth.'}}, {'sourceId': 'REPO-SVC-AML', 'targetId': 'REPO-SVC-AML', 'message': "1.1.2. Persist model metadata with status 'PendingApproval'", 'sequenceNumber': 4, 'type': 'DatabaseQuery', 'isSynchronous': True, 'returnMessage': 'New model record with ID', 'hasReturn': True, 'technicalDetails': {'protocol': 'SQL', 'method': 'INSERT INTO Models', 'parameters': "{ name, description, s3Url, status: 'PendingApproval', submittedByUserId }", 'authentication': 'Database connection string with credentials', 'errorHandling': 'Standard transaction error handling; rollback if insert fails.', 'performance': 'P99 latency < 50ms'}}, {'sourceId': 'REPO-SVC-AML', 'targetId': 'REPO-SVC-ANM', 'message': '1.1.3. POST /internal/v1/notifications', 'sequenceNumber': 5, 'type': 'Request', 'isSynchronous': True, 'returnMessage': '202 Accepted', 'hasReturn': True, 'technicalDetails': {'protocol': 'HTTP/REST', 'method': 'POST', 'parameters': "Payload: { targetRole: 'Administrator', message: 'New AI Model for review...', deepLink: '/models/review/{modelId}' }", 'authentication': 'Internal service authentication (mTLS or internal JWT)', 'errorHandling': 'Log error if notification fails, but do not fail the primary transaction.', 'performance': 'P95 latency < 100ms'}}]}

### 2.5.2.0 Request

#### 2.5.2.1 Source Id

REPO-FE-MPL

#### 2.5.2.2 Target Id

REPO-GW-API

#### 2.5.2.3 Message

2. [Administrator] POST /api/v1/models/{modelId}/approve

#### 2.5.2.4 Sequence Number

6

#### 2.5.2.5 Type

🔹 Request

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

200 OK

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/REST |
| Method | POST |
| Parameters | Path parameter: {modelId}. Optional body: { commen... |
| Authentication | JWT Bearer Token required (Administrator role) |
| Error Handling | 401 Unauthorized if token missing. 403 Forbidden i... |
| Performance | P95 latency < 200ms |

#### 2.5.2.11 Nested Interactions

- {'sourceId': 'REPO-GW-API', 'targetId': 'REPO-SVC-AML', 'message': '2.1. Route validated approval request to service', 'sequenceNumber': 7, 'type': 'Proxy', 'isSynchronous': True, 'returnMessage': '200 OK', 'hasReturn': True, 'isActivation': True, 'technicalDetails': {'protocol': 'HTTP (internal)', 'method': 'POST', 'parameters': 'Forwarded request with user claims in headers.', 'authentication': 'JWT validated, claims forwarded.', 'errorHandling': '502 Bad Gateway if service is down.', 'performance': 'Internal network latency < 5ms'}, 'nestedInteractions': [{'sourceId': 'REPO-SVC-AML', 'targetId': 'REPO-SVC-AML', 'message': "2.1.1. [Security Check] Verify user role is 'Administrator'", 'sequenceNumber': 8, 'type': 'Validation', 'isSynchronous': True, 'returnMessage': 'Role check pass/fail', 'hasReturn': True, 'technicalDetails': {'protocol': 'Internal Logic', 'method': 'Authorization Check', 'parameters': 'User claims from JWT', 'authentication': 'N/A', 'errorHandling': 'If check fails, return 403 Forbidden immediately.', 'performance': 'Latency < 1ms'}}, {'sourceId': 'REPO-SVC-AML', 'targetId': 'REPO-SVC-AML', 'message': "2.1.2. Update model status to 'Approved'", 'sequenceNumber': 9, 'type': 'DatabaseQuery', 'isSynchronous': True, 'returnMessage': 'Updated record count', 'hasReturn': True, 'technicalDetails': {'protocol': 'SQL', 'method': "UPDATE Models SET status = 'Approved', approvedByUserId = ?, approvedAt = NOW() WHERE id = ?", 'parameters': '{ approverUserId, modelId }', 'authentication': 'Database connection string', 'errorHandling': 'Rollback transaction on failure, return 500 Internal Server Error.', 'performance': 'P99 latency < 50ms'}}, {'sourceId': 'REPO-SVC-AML', 'targetId': 'REPO-SVC-ADT', 'message': '2.1.3. [Async] LogAction(LogActionRequest)', 'sequenceNumber': 10, 'type': 'AsynchronousCall', 'isSynchronous': False, 'returnMessage': '', 'hasReturn': False, 'technicalDetails': {'protocol': 'gRPC', 'method': 'Audit.LogAction', 'parameters': "LogActionRequest { action: 'ModelApproved', entityId: modelId, oldState: 'PendingApproval', newState: 'Approved', actingUserId: approverUserId }", 'authentication': 'Internal mTLS', 'errorHandling': 'Fire-and-forget. Failure is handled by Audit service; raises a high-priority alert for manual reconciliation.', 'performance': 'P99 latency < 20ms'}}]}

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content



```
Phase 1: Model Submission
Data Scientist uploads the model. The system stores it and notifies administrators.
```

#### 2.6.1.2 Position

top-left

#### 2.6.1.3 Participant Id

*Not specified*

#### 2.6.1.4 Sequence Number

1

### 2.6.2.0 Content

#### 2.6.2.1 Content



```
Phase 2: Model Approval
Administrator reviews the model and approves it. The state change is persisted and audited.
```

#### 2.6.2.2 Position

bottom-left

#### 2.6.2.3 Participant Id

*Not specified*

#### 2.6.2.4 Sequence Number

6

### 2.6.3.0 Content

#### 2.6.3.1 Content

The call to the Audit Service (2.1.3) is intentionally asynchronous (fire-and-forget) to ensure that the user's approval action is not blocked by the auditing process. Failures in auditing are treated as a separate operational issue.

#### 2.6.3.2 Position

bottom-right

#### 2.6.3.3 Participant Id

REPO-SVC-AML

#### 2.6.3.4 Sequence Number

10

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Strict role-based access control (RBAC) must be en... |
| Performance Targets | API response time for the approval action (Step 2)... |
| Error Handling Strategy | The process must be transactional. If the S3 uploa... |
| Testing Considerations | End-to-end tests must cover the full workflow for ... |
| Monitoring Requirements | Monitor the rate of model submissions and approval... |
| Deployment Considerations | The approval workflow logic within the AI/ML Manag... |

