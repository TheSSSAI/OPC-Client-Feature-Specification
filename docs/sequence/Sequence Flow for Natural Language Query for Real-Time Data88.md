# 1 Overview

## 1.1 Diagram Id

SEQ-IF-002

## 1.2 Name

Natural Language Query for Real-Time Data

## 1.3 Description

A user types or speaks a query like 'What is the temperature of Reactor 3?'. The system sends this input to AWS Transcribe (for voice-to-text) and AWS Comprehend to interpret the intent and entities. The system then translates this into a formal data query and returns the value to the user.

## 1.4 Type

🔹 IntegrationFlow

## 1.5 Purpose

To provide an intuitive, natural language interface for accessing system data, improving usability and accessibility for operators and managers (REQ-FR-014).

## 1.6 Complexity

High

## 1.7 Priority

🟡 Medium

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-NLQ
- AWS Transcribe
- AWS Comprehend
- REPO-SVC-DQR

## 1.10 Key Interactions

- The frontend captures the user's text or voice input.
- If voice, it's sent to AWS Transcribe for conversion to text.
- The text is sent to a Natural Language Query (NLQ) service.
- The NLQ service calls AWS Comprehend to parse the query and extract intent ('get value') and entities ('temperature', 'Reactor 3').
- The NLQ service maps the extracted entities to a specific OPC tag in the system using the asset hierarchy.
- The NLQ service calls the Query Service to get the real-time value for that tag.
- The value is returned and displayed to the user in a human-readable format.

## 1.11 Triggers

- A user interacts with the natural language query interface in the web application.

## 1.12 Outcomes

- The user receives a direct answer to their data query without needing to navigate complex dashboards.
- The query and its interpretation are logged for auditing and continuous improvement (REQ-FR-014).

## 1.13 Business Rules

- The system must integrate with AWS Transcribe and AWS Comprehend (REQ-FR-014).
- The supported grammar must cover retrieving real-time/historical data and acknowledging alarms (REQ-FR-014).

## 1.14 Error Scenarios

- AWS Comprehend cannot understand the user's intent or returns a low confidence score.
- The extracted entities cannot be mapped to a known asset or tag in the system.
- One of the AWS services is unavailable.

## 1.15 Integration Points

- AWS Transcribe
- AWS Comprehend

# 2.0 Details

## 2.1 Diagram Id

SEQ-IF-002-IMPL

## 2.2 Name

Implementation: Natural Language Query for Real-Time Data via AWS AI

## 2.3 Description

Provides a detailed technical implementation for processing a user's natural language query (voice or text). The sequence involves orchestrating calls to external AWS AI services for transcription and intent recognition, followed by internal service calls to map recognized entities to system assets and retrieve real-time data, and finally logging the transaction for audit purposes. The Natural Language Query Service (REPO-SVC-NLQ) acts as the central orchestrator.

## 2.4 Participants

### 2.4.1 Web Application

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Frontend SPA

#### 2.4.1.3 Type

🔹 Web Application

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

### 2.4.2.0 API Gateway

#### 2.4.2.1 Repository Id

REPO-GW-API

#### 2.4.2.2 Display Name

API Gateway

#### 2.4.2.3 Type

🔹 API Gateway

#### 2.4.2.4 Technology

Kong v3.7.0 on Kubernetes

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #FFD700 |
| Stereotype | Gateway |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-NLQ

#### 2.4.3.2 Display Name

NLQ Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8, ASP.NET Core, AWS SDK

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #32CD32 |
| Stereotype | Service |

### 2.4.4.0 External Cloud Service

#### 2.4.4.1 Repository Id

AWS Transcribe

#### 2.4.4.2 Display Name

AWS Transcribe

#### 2.4.4.3 Type

🔹 External Cloud Service

#### 2.4.4.4 Technology

AWS SDK for .NET

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | cloud |
| Color | #FF8C00 |
| Stereotype | External |

### 2.4.5.0 External Cloud Service

#### 2.4.5.1 Repository Id

AWS Comprehend

#### 2.4.5.2 Display Name

AWS Comprehend

#### 2.4.5.3 Type

🔹 External Cloud Service

#### 2.4.5.4 Technology

AWS SDK for .NET

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | cloud |
| Color | #FF8C00 |
| Stereotype | External |

### 2.4.6.0 Microservice

#### 2.4.6.1 Repository Id

REPO-SVC-AST

#### 2.4.6.2 Display Name

Asset Service

#### 2.4.6.3 Type

🔹 Microservice

#### 2.4.6.4 Technology

.NET 8, gRPC

#### 2.4.6.5 Order

6

#### 2.4.6.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #9370DB |
| Stereotype | Service |

### 2.4.7.0 Microservice

#### 2.4.7.1 Repository Id

REPO-SVC-DQR

#### 2.4.7.2 Display Name

Query Service

#### 2.4.7.3 Type

🔹 Microservice

#### 2.4.7.4 Technology

.NET 8, gRPC

#### 2.4.7.5 Order

7

#### 2.4.7.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #9370DB |
| Stereotype | Service |

### 2.4.8.0 Microservice

#### 2.4.8.1 Repository Id

REPO-SVC-ADT

#### 2.4.8.2 Display Name

Audit Service

#### 2.4.8.3 Type

🔹 Microservice

#### 2.4.8.4 Technology

.NET 8, gRPC

#### 2.4.8.5 Order

8

#### 2.4.8.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #9370DB |
| Stereotype | Service |

## 2.5.0.0 Interactions

### 2.5.1.0 HTTP Request

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

POST /api/v1/nlq/voice (Audio Blob)

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 HTTP Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

200 OK { value: 'The temperature of Reactor 3 is 350.5 C.' }

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | POST |
| Parameters | Request body contains audio data (e.g., WAV format... |
| Authentication | JWT Bearer Token in Authorization header. |
| Error Handling | Client-side retry on 5xx errors. Handles 4xx error... |
| Performance | Upload latency depends on network; request timeout... |

### 2.5.2.0 HTTP Proxy

#### 2.5.2.1 Source Id

REPO-GW-API

#### 2.5.2.2 Target Id

REPO-SVC-NLQ

#### 2.5.2.3 Message

Forward POST /voice

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 HTTP Proxy

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

Forward 200 OK

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (internal) |
| Method | POST |
| Parameters | Forwards request body and headers. |
| Authentication | Validates JWT. Propagates tenantId and userId in c... |
| Error Handling | Returns 503 if upstream service is unavailable. St... |
| Performance | Adds <10ms latency. |

### 2.5.3.0 API Call

#### 2.5.3.1 Source Id

REPO-SVC-NLQ

#### 2.5.3.2 Target Id

AWS Transcribe

#### 2.5.3.3 Message

StartTranscriptionJob(audioData)

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 API Call

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

TranscriptionJobResult { transcript: 'What is the temperature of Reactor 3?' }

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

❌ No

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS (AWS SDK) |
| Method | StartTranscriptionJob |
| Parameters | Audio stream, language code, media format. |
| Authentication | AWS IAM Role for Service Account (IRSA) assigned t... |
| Error Handling | Circuit Breaker pattern. Retry (x3, exponential ba... |
| Performance | Monitored via CloudWatch Metrics. Expected latency... |

### 2.5.4.0 API Call

#### 2.5.4.1 Source Id

REPO-SVC-NLQ

#### 2.5.4.2 Target Id

AWS Comprehend

#### 2.5.4.3 Message

DetectEntities(text)

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 API Call

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

DetectEntitiesResult { entities: [...] }

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS (AWS SDK) |
| Method | DetectEntities |
| Parameters | Transcribed text, language code. |
| Authentication | AWS IAM Role for Service Account (IRSA). |
| Error Handling | Circuit Breaker pattern. If confidence score is be... |
| Performance | Monitored via CloudWatch Metrics. Expected latency... |

### 2.5.5.0 gRPC Call

#### 2.5.5.1 Source Id

REPO-SVC-NLQ

#### 2.5.5.2 Target Id

REPO-SVC-AST

#### 2.5.5.3 Message

ResolveAssetTag(assetName: 'Reactor 3', metric: 'temperature')

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 gRPC Call

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

ResolveAssetTagResponse { opcTagId: 'ns=2;s=Reactor3.Temperature' }

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | AssetService.ResolveAssetTag |
| Parameters | Proto message with asset and metric strings. |
| Authentication | Internal cluster communication (no external auth). |
| Error Handling | Circuit Breaker. If tag not found, return 404 erro... |
| Performance | Expected latency < 50ms. Service should cache asse... |

### 2.5.6.0 gRPC Call

#### 2.5.6.1 Source Id

REPO-SVC-NLQ

#### 2.5.6.2 Target Id

REPO-SVC-DQR

#### 2.5.6.3 Message

GetRealTimeValue(tagId: 'ns=2;...')

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 gRPC Call

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message

GetRealTimeValueResponse { value: 350.5, quality: 'Good', timestamp: ... }

#### 2.5.6.8 Has Return

✅ Yes

#### 2.5.6.9 Is Activation

❌ No

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | QueryService.GetRealTimeValue |
| Parameters | Proto message with opcTagId. |
| Authentication | Internal cluster communication. |
| Error Handling | Circuit Breaker. If data retrieval fails, return 5... |
| Performance | Expected latency < 100ms. |

### 2.5.7.0 gRPC Call

#### 2.5.7.1 Source Id

REPO-SVC-NLQ

#### 2.5.7.2 Target Id

REPO-SVC-ADT

#### 2.5.7.3 Message

LogAction(details)

#### 2.5.7.4 Sequence Number

7

#### 2.5.7.5 Type

🔹 gRPC Call

#### 2.5.7.6 Is Synchronous

❌ No

#### 2.5.7.7 Return Message



#### 2.5.7.8 Has Return

❌ No

#### 2.5.7.9 Is Activation

❌ No

#### 2.5.7.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | AuditService.LogAction |
| Parameters | Proto message containing: userId, originalQuery, i... |
| Authentication | Internal cluster communication. |
| Error Handling | Fire-and-forget. Logging failure should not fail t... |
| Performance | N/A (async). |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

The voice query path is shown. A text query would skip steps 3 and start at step 4 after receiving the text from the frontend via a separate POST /api/v1/nlq/text endpoint.

#### 2.6.1.2 Position

Top

#### 2.6.1.3 Participant Id

*Not specified*

#### 2.6.1.4 Sequence Number

*Not specified*

### 2.6.2.0 Content

#### 2.6.2.1 Content

The NLQ Service is the core orchestrator. Its primary responsibility is to translate the unstructured user query into a series of structured internal API calls.

#### 2.6.2.2 Position

Right

#### 2.6.2.3 Participant Id

REPO-SVC-NLQ

#### 2.6.2.4 Sequence Number

2

### 2.6.3.0 Content

#### 2.6.3.1 Content

Failure to map entities in Step 5 is a critical business logic failure. The service must return a user-friendly error message indicating which part of the query could not be resolved.

#### 2.6.3.2 Position

Right

#### 2.6.3.3 Participant Id

REPO-SVC-AST

#### 2.6.3.4 Sequence Number

5

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | AWS credentials must be managed via IAM Roles for ... |
| Performance Targets | The P95 end-to-end latency for a text-based query ... |
| Error Handling Strategy | Implement the Circuit Breaker pattern for all exte... |
| Testing Considerations | Unit tests for the NLQ service should mock the AWS... |
| Monitoring Requirements | Monitor AWS service usage and latency via CloudWat... |
| Deployment Considerations | The NLQ service is a new microservice and must be ... |

