# 1 Overview

## 1.1 Diagram Id

SEQ-UJ-003

## 1.2 Name

Data Scientist Manages AI Model Lifecycle

## 1.3 Description

A Data Scientist logs in and interacts with the AI/ML Management module. They access a sandboxed environment with read-only access to production data to train a new predictive maintenance model. Once trained and packaged as ONNX, they upload the model and submit it for validation and approval.

## 1.4 Type

🔹 UserJourney

## 1.5 Purpose

To provide a dedicated, secure workflow for Data Scientists to develop, train, and manage AI/ML models without impacting production systems, as defined by their user role in REQ-USR-001.

## 1.6 Complexity

High

## 1.7 Priority

🟡 Medium

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-AML
- REPO-SVC-DQR
- Data Sandbox Environment

## 1.10 Key Interactions

- A Data Scientist logs in; their role grants them access to the AI module and read-only data APIs.
- They use the Query Service API to export historical production data for a specific asset or time range.
- They train a model in their dedicated sandbox environment (e.g., a Jupyter notebook) external to the application.
- They use the UI to upload the final ONNX model file to the AI/ML Service.
- The AI/ML Service stores the file in S3 and creates a new versioned model entry in the database.
- They initiate the approval workflow, which triggers SEQ-BP-001.

## 1.11 Triggers

- A Data Scientist needs to develop a new AI model or retrain an existing one with new data.

## 1.12 Outcomes

- A new, trained AI model is registered in the system with a 'Pending Approval' status.
- Production data is accessed securely and in a read-only manner for model training purposes.

## 1.13 Business Rules

- Data Scientists have Read/Write access to a dedicated data sandbox but only Read access to historical production data (REQ-USR-001).
- Data Scientists cannot configure or control production systems and have no access to production configuration (REQ-USR-001).

## 1.14 Error Scenarios

- The user attempts to access data they are not permissioned for.
- The uploaded model file is not a valid ONNX format or exceeds size limits.
- The S3 bucket for model storage is unavailable.

## 1.15 Integration Points

- Data Sandbox Environment (e.g., JupyterHub, Amazon SageMaker)

# 2.0 Details

## 2.1 Diagram Id

SEQ-UJ-003-IMPL

## 2.2 Name

Implementation: Data Scientist AI Model Lifecycle

## 2.3 Description

Technical sequence for a Data Scientist user journey. The sequence covers querying historical data for model training, interacting with an external sandbox, uploading a trained ONNX model via the frontend, and submitting it for an approval workflow. It emphasizes the specific API interactions, state management in the frontend, and backend processing by the AI/ML service.

## 2.4 Participants

### 2.4.1 UI/SPA

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Frontend: Management Plane

#### 2.4.1.3 Type

🔹 UI/SPA

#### 2.4.1.4 Technology

React 18, TypeScript, Redux Toolkit

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | User Interface |

### 2.4.2.0 APIGateway

#### 2.4.2.1 Repository Id

REPO-GW-API

#### 2.4.2.2 Display Name

API Gateway

#### 2.4.2.3 Type

🔹 APIGateway

#### 2.4.2.4 Technology

Kong v3.7.0

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

REPO-SVC-DQR

#### 2.4.3.2 Display Name

Service: Data Query & Reporting

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8, ASP.NET Core

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #32CD32 |
| Stereotype | Service |

### 2.4.4.0 External System

#### 2.4.4.1 Repository Id

Data Sandbox Environment

#### 2.4.4.2 Display Name

Data Sandbox Environment

#### 2.4.4.3 Type

🔹 External System

#### 2.4.4.4 Technology

JupyterHub / Amazon SageMaker

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #808080 |
| Stereotype | External |

### 2.4.5.0 Microservice

#### 2.4.5.1 Repository Id

REPO-SVC-AML

#### 2.4.5.2 Display Name

Service: AI/ML Management

#### 2.4.5.3 Type

🔹 Microservice

#### 2.4.5.4 Technology

.NET 8, ASP.NET Core

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

1. GET /api/v1/query/history?assetId=...&timeRange=...

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

Forwards request to DQR Service

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/REST |
| Method | GET |
| Parameters | Query parameters for asset, time range, and data f... |
| Authentication | JWT Bearer Token (Role: Data Scientist) |
| Error Handling | Returns 401 if unauthenticated, 403 if unauthorize... |
| Performance | P95 Latency < 200ms |

### 2.5.2.0 Proxy

#### 2.5.2.1 Source Id

REPO-GW-API

#### 2.5.2.2 Target Id

REPO-SVC-DQR

#### 2.5.2.3 Message

2. Forwards authorized GET request

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Proxy

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

Returns historical data or download link

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (Internal) |
| Method | GET |
| Parameters | Inherited from sequence 1. |
| Authentication | Propagated JWT and service identity. |
| Error Handling | Service mesh handles internal retries. Returns 5xx... |
| Performance | P95 Latency < 1s for 24h query (REQ-1-075) |

#### 2.5.2.11 Nested Interactions

##### 2.5.2.11.1 Internal Logic

###### 2.5.2.11.1.1 Source Id

REPO-SVC-DQR

###### 2.5.2.11.1.2 Target Id

REPO-SVC-DQR

###### 2.5.2.11.1.3 Message

2a. Validates user has 'Read' access to historical production data for the specified asset (REQ-USR-001)

###### 2.5.2.11.1.4 Sequence Number

2.1

###### 2.5.2.11.1.5 Type

🔹 Internal Logic

###### 2.5.2.11.1.6 Is Synchronous

✅ Yes

###### 2.5.2.11.1.7 Has Return

❌ No

###### 2.5.2.11.1.8 Is Activation

❌ No

###### 2.5.2.11.1.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | checkPermissions() |
| Parameters | User claims, assetId |
| Authentication | N/A |
| Error Handling | Throws AuthorizationException if permissions are i... |
| Performance | Latency < 10ms |

##### 2.5.2.11.2.0 Database Query

###### 2.5.2.11.2.1 Source Id

REPO-SVC-DQR

###### 2.5.2.11.2.2 Target Id

REPO-SVC-DQR

###### 2.5.2.11.2.3 Message

2b. Queries TimescaleDB for historical data and generates CSV export

###### 2.5.2.11.2.4 Sequence Number

2.2

###### 2.5.2.11.2.5 Type

🔹 Database Query

###### 2.5.2.11.2.6 Is Synchronous

✅ Yes

###### 2.5.2.11.2.7 Has Return

❌ No

###### 2.5.2.11.2.8 Is Activation

❌ No

###### 2.5.2.11.2.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | SELECT ... FROM tag_data WHERE ... |
| Parameters | assetId, time range |
| Authentication | Database credentials from Secrets Manager. |
| Error Handling | Returns 503 if database is unavailable. |
| Performance | Dependent on query range. |

### 2.5.3.0.0.0 Manual Interaction

#### 2.5.3.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.3.2.0.0 Target Id

Data Sandbox Environment

#### 2.5.3.3.0.0 Message

3. Data Scientist uses exported data to train model externally

#### 2.5.3.4.0.0 Sequence Number

3

#### 2.5.3.5.0.0 Type

🔹 Manual Interaction

#### 2.5.3.6.0.0 Is Synchronous

❌ No

#### 2.5.3.7.0.0 Has Return

❌ No

#### 2.5.3.8.0.0 Is Activation

❌ No

#### 2.5.3.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | N/A |
| Method | N/A |
| Parameters | CSV data file |
| Authentication | Handled by the Sandbox Environment |
| Error Handling | N/A |
| Performance | N/A |

### 2.5.4.0.0.0 User Input

#### 2.5.4.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.4.2.0.0 Target Id

REPO-FE-MPL

#### 2.5.4.3.0.0 Message

4. User selects ONNX file and enters metadata (name, description)

#### 2.5.4.4.0.0 Sequence Number

4

#### 2.5.4.5.0.0 Type

🔹 User Input

#### 2.5.4.6.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0 Has Return

❌ No

#### 2.5.4.8.0.0 Is Activation

❌ No

#### 2.5.4.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | DOM Event |
| Method | onChange, onSubmit |
| Parameters | File object, form data |
| Authentication | N/A |
| Error Handling | UI displays validation errors (e.g., 'File must be... |
| Performance | UI response time < 50ms |

### 2.5.5.0.0.0 Request

#### 2.5.5.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.5.2.0.0 Target Id

REPO-GW-API

#### 2.5.5.3.0.0 Message

5. POST /api/v1/models (multipart/form-data)

#### 2.5.5.4.0.0 Sequence Number

5

#### 2.5.5.5.0.0 Type

🔹 Request

#### 2.5.5.6.0.0 Is Synchronous

✅ Yes

#### 2.5.5.7.0.0 Return Message

Forwards request to AML Service

#### 2.5.5.8.0.0 Has Return

✅ Yes

#### 2.5.5.9.0.0 Is Activation

✅ Yes

#### 2.5.5.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/REST |
| Method | POST |
| Parameters | Request body: { file: ONNX file, name: 'string', d... |
| Authentication | JWT Bearer Token (Role: Data Scientist) |
| Error Handling | UI shows a progress bar during upload. Handles 4xx... |
| Performance | Dependent on file size and network speed. |

### 2.5.6.0.0.0 Proxy

#### 2.5.6.1.0.0 Source Id

REPO-GW-API

#### 2.5.6.2.0.0 Target Id

REPO-SVC-AML

#### 2.5.6.3.0.0 Message

6. Forwards authorized POST request

#### 2.5.6.4.0.0 Sequence Number

6

#### 2.5.6.5.0.0 Type

🔹 Proxy

#### 2.5.6.6.0.0 Is Synchronous

✅ Yes

#### 2.5.6.7.0.0 Return Message

Returns 201 Created with new model details

#### 2.5.6.8.0.0 Has Return

✅ Yes

#### 2.5.6.9.0.0 Is Activation

✅ Yes

#### 2.5.6.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (Internal) |
| Method | POST |
| Parameters | Inherited from sequence 5. |
| Authentication | Propagated JWT. |
| Error Handling | Returns 400 for invalid model format, 413 if file ... |
| Performance | Dependent on file size. |

#### 2.5.6.11.0.0 Nested Interactions

##### 2.5.6.11.1.0 Internal Logic

###### 2.5.6.11.1.1 Source Id

REPO-SVC-AML

###### 2.5.6.11.1.2 Target Id

REPO-SVC-AML

###### 2.5.6.11.1.3 Message

6a. Validates ONNX file format and metadata

###### 2.5.6.11.1.4 Sequence Number

6.1

###### 2.5.6.11.1.5 Type

🔹 Internal Logic

###### 2.5.6.11.1.6 Is Synchronous

✅ Yes

###### 2.5.6.11.1.7 Has Return

❌ No

###### 2.5.6.11.1.8 Is Activation

❌ No

###### 2.5.6.11.1.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | validateModelUpload() |
| Parameters | File stream, metadata |
| Authentication | N/A |
| Error Handling | Throws ValidationException, resulting in a 400 res... |
| Performance | Latency < 200ms |

##### 2.5.6.11.2.0 External Call

###### 2.5.6.11.2.1 Source Id

REPO-SVC-AML

###### 2.5.6.11.2.2 Target Id

REPO-SVC-AML

###### 2.5.6.11.2.3 Message

6b. Streams ONNX file to Amazon S3 object storage

###### 2.5.6.11.2.4 Sequence Number

6.2

###### 2.5.6.11.2.5 Type

🔹 External Call

###### 2.5.6.11.2.6 Is Synchronous

✅ Yes

###### 2.5.6.11.2.7 Has Return

❌ No

###### 2.5.6.11.2.8 Is Activation

❌ No

###### 2.5.6.11.2.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | AWS SDK (HTTPS) |
| Method | PutObjectAsync() |
| Parameters | Bucket name, key (tenantId/modelId/version.onnx), ... |
| Authentication | IAM Role credentials for EKS pod. |
| Error Handling | Throws S3Exception, resulting in a 503 response. |
| Performance | Dependent on file size. |

##### 2.5.6.11.3.0 Database Query

###### 2.5.6.11.3.1 Source Id

REPO-SVC-AML

###### 2.5.6.11.3.2 Target Id

REPO-SVC-AML

###### 2.5.6.11.3.3 Message

6c. Persists model metadata to PostgreSQL DB with status 'Pending Approval'

###### 2.5.6.11.3.4 Sequence Number

6.3

###### 2.5.6.11.3.5 Type

🔹 Database Query

###### 2.5.6.11.3.6 Is Synchronous

✅ Yes

###### 2.5.6.11.3.7 Has Return

❌ No

###### 2.5.6.11.3.8 Is Activation

❌ No

###### 2.5.6.11.3.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | INSERT INTO Models ... |
| Parameters | Model name, S3 path, version, status, creatorId, t... |
| Authentication | Database credentials from Secrets Manager. |
| Error Handling | Throws DbUpdateException, resulting in a 503 respo... |
| Performance | Latency < 50ms |

### 2.5.7.0.0.0 UI Update

#### 2.5.7.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.7.2.0.0 Target Id

REPO-FE-MPL

#### 2.5.7.3.0.0 Message

7. UI state updates to show new model with status 'Pending Approval'

#### 2.5.7.4.0.0 Sequence Number

7

#### 2.5.7.5.0.0 Type

🔹 UI Update

#### 2.5.7.6.0.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0.0 Has Return

❌ No

#### 2.5.7.8.0.0 Is Activation

❌ No

#### 2.5.7.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Redux Action |
| Method | dispatch(models.addModelSuccess(payload)) |
| Parameters | New model object from API response. |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | UI render time < 100ms |

### 2.5.8.0.0.0 Request

#### 2.5.8.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.8.2.0.0 Target Id

REPO-GW-API

#### 2.5.8.3.0.0 Message

8. POST /api/v1/models/{modelId}/approvals

#### 2.5.8.4.0.0 Sequence Number

8

#### 2.5.8.5.0.0 Type

🔹 Request

#### 2.5.8.6.0.0 Is Synchronous

✅ Yes

#### 2.5.8.7.0.0 Return Message

Returns 202 Accepted

#### 2.5.8.8.0.0 Has Return

✅ Yes

#### 2.5.8.9.0.0 Is Activation

✅ Yes

#### 2.5.8.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/REST |
| Method | POST |
| Parameters | Route parameter: modelId. Body: { comment: 'string... |
| Authentication | JWT Bearer Token (Role: Data Scientist) |
| Error Handling | Handles 404 if model not found, 409 if already sub... |
| Performance | P95 Latency < 200ms |

### 2.5.9.0.0.0 Proxy

#### 2.5.9.1.0.0 Source Id

REPO-GW-API

#### 2.5.9.2.0.0 Target Id

REPO-SVC-AML

#### 2.5.9.3.0.0 Message

9. Forwards request to initiate approval workflow

#### 2.5.9.4.0.0 Sequence Number

9

#### 2.5.9.5.0.0 Type

🔹 Proxy

#### 2.5.9.6.0.0 Is Synchronous

✅ Yes

#### 2.5.9.7.0.0 Return Message

Returns 202 Accepted

#### 2.5.9.8.0.0 Has Return

✅ Yes

#### 2.5.9.9.0.0 Is Activation

✅ Yes

#### 2.5.9.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (Internal) |
| Method | POST |
| Parameters | Inherited from sequence 8. |
| Authentication | Propagated JWT. |
| Error Handling | Internal service error handling. |
| Performance | P95 Latency < 150ms |

#### 2.5.9.11.0.0 Nested Interactions

- {'sourceId': 'REPO-SVC-AML', 'targetId': 'REPO-SVC-AML', 'message': '9a. Updates model status in DB and triggers approval workflow (e.g., creates approval task)', 'sequenceNumber': 9.1, 'type': 'Internal Logic', 'isSynchronous': True, 'hasReturn': False, 'isActivation': False, 'technicalDetails': {'protocol': 'Internal', 'method': 'startApprovalWorkflow()', 'parameters': 'modelId, submittedById', 'authentication': 'N/A', 'errorHandling': 'Logs error and returns 500 if workflow fails to start.', 'performance': 'Latency < 100ms'}}

## 2.6.0.0.0.0 Notes

### 2.6.1.0.0.0 Content

#### 2.6.1.1.0.0 Content

Phase 1: Data Gathering. The Data Scientist must first acquire read-only production data to train the model.

#### 2.6.1.2.0.0 Position

Top

#### 2.6.1.3.0.0 Participant Id

*Not specified*

#### 2.6.1.4.0.0 Sequence Number

1

### 2.6.2.0.0.0 Content

#### 2.6.2.1.0.0 Content

Phase 2: Model Upload. After offline training, the packaged ONNX model is uploaded to the system.

#### 2.6.2.2.0.0 Position

Middle

#### 2.6.2.3.0.0 Participant Id

*Not specified*

#### 2.6.2.4.0.0 Sequence Number

4

### 2.6.3.0.0.0 Content

#### 2.6.3.1.0.0 Content

S3 Storage: The AML service is responsible for securely storing the model file in a tenant-isolated S3 path.

#### 2.6.3.2.0.0 Position

Right

#### 2.6.3.3.0.0 Participant Id

REPO-SVC-AML

#### 2.6.3.4.0.0 Sequence Number

6.2

### 2.6.4.0.0.0 Content

#### 2.6.4.1.0.0 Content

Phase 3: Initiate Approval. The final step is to formally submit the uploaded model for review by an Administrator.

#### 2.6.4.2.0.0 Position

Bottom

#### 2.6.4.3.0.0 Participant Id

*Not specified*

#### 2.6.4.4.0.0 Sequence Number

8

## 2.7.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | API Gateway must enforce that only users with the ... |
| Performance Targets | API endpoints for metadata operations should respo... |
| Error Handling Strategy | The UI must provide clear, user-friendly feedback ... |
| Testing Considerations | E2E tests should cover the entire flow from data q... |
| Monitoring Requirements | Monitor the latency and error rates of the /models... |
| Deployment Considerations | The REPO-SVC-AML service requires an IAM role with... |

