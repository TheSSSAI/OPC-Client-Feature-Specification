# 1 Overview

## 1.1 Diagram Id

SEQ-EH-003

## 1.2 Name

API Gateway Rate Limiting

## 1.3 Description

A client application begins making an excessive number of API requests, exceeding the configured threshold (e.g., requests per second). The API Gateway identifies this behavior and starts rejecting subsequent requests from that client with an HTTP 429 'Too Many Requests' status code, protecting backend services.

## 1.4 Type

🔹 ErrorHandling

## 1.5 Purpose

To protect backend services from being overloaded by misbehaving, malicious, or poorly configured clients, ensuring system stability and fair resource usage for all tenants (part of REQ-NFR-006).

## 1.6 Complexity

Low

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- API Client
- REPO-GW-API

## 1.10 Key Interactions

- A client sends a high volume of requests in a short time window.
- The API Gateway's rate-limiting plugin tracks the request count for the client (identified by IP, API key, or JWT claim).
- The count exceeds the configured limit (e.g., 100 requests/minute).
- The API Gateway blocks the next request from that client, immediately returning a 429 response with a `Retry-After` header, without forwarding the request to any backend service.

## 1.11 Triggers

- A client's API request rate surpasses the defined limit for their tier or for a specific endpoint.

## 1.12 Outcomes

- The targeted backend service is shielded from the excessive traffic, maintaining its performance and availability.
- The misbehaving client is temporarily blocked and receives an explicit error indicating they should reduce their request rate.

## 1.13 Business Rules

- Rate limiting policies must be configurable, potentially on a per-tenant or per-user basis.
- The API Gateway is the centralized point for enforcing traffic management policies (REQ-NFR-006).

## 1.14 Error Scenarios

- A legitimate but high-volume automated process gets rate-limited, disrupting business operations, requiring a policy adjustment.
- The rate-limiting configuration is too strict or too lenient, failing to provide adequate protection or causing unnecessary friction.

## 1.15 Integration Points

*No items available*

# 2.0 Details

## 2.1 Diagram Id

SEQ-EH-003

## 2.2 Name

Implementation of API Gateway Rate Limiting Enforcement

## 2.3 Description

Detailed sequence for the API Gateway (Kong) enforcing a configured rate limit. This diagram illustrates the internal logic of tracking requests against a distributed cache, detecting a threshold breach, and rejecting subsequent requests with an HTTP 429 status code to protect backend services from overload, as per REQ-NFR-006.

## 2.4 Participants

### 2.4.1 ExternalActor

#### 2.4.1.1 Repository Id

EXT-CLIENT-APP

#### 2.4.1.2 Display Name

API Client

#### 2.4.1.3 Type

🔹 ExternalActor

#### 2.4.1.4 Technology

Generic HTTP/1.1 Client

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #DDDDDD |
| Stereotype | External |

### 2.4.2.0 ApiGateway

#### 2.4.2.1 Repository Id

REPO-GW-API

#### 2.4.2.2 Display Name

API Gateway

#### 2.4.2.3 Type

🔹 ApiGateway

#### 2.4.2.4 Technology

Kong v3.7.0 on Kubernetes

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #8E44AD |
| Stereotype | Gateway |

### 2.4.3.0 Cache

#### 2.4.3.1 Repository Id

INT-REDIS-CACHE

#### 2.4.3.2 Display Name

Rate Limit Store

#### 2.4.3.3 Type

🔹 Cache

#### 2.4.3.4 Technology

Redis 7 (Distributed)

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #D35400 |
| Stereotype | Internal |

## 2.5.0.0 Interactions

### 2.5.1.0 Request

#### 2.5.1.1 Source Id

EXT-CLIENT-APP

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

[Loop: N times within limit] POST /api/v1/assets

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

200 OK

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

##### 2.5.1.10.1 Protocol

HTTP/1.1

##### 2.5.1.10.2 Method

POST

##### 2.5.1.10.3 Parameters

| Property | Value |
|----------|-------|
| Headers | ['Authorization: Bearer <jwt_token>', 'Content-Type: application/json'] |
| Body | JSON payload for new asset |

##### 2.5.1.10.4 Authentication

JWT Bearer Token validation via Kong JWT Plugin. Rate limit key is extracted from the 'sub' claim.

##### 2.5.1.10.5 Error Handling

Standard HTTP error codes (e.g., 400, 401, 403) for non-rate-limit errors.

##### 2.5.1.10.6 Performance

###### 2.5.1.10.6.1 Latency

<200ms P95 (including backend)

###### 2.5.1.10.6.2 Throughput

Up to configured limit

#### 2.5.1.11.0.0 Nested Interactions

- {'sourceId': 'REPO-GW-API', 'targetId': 'INT-REDIS-CACHE', 'message': "INCR 'ratelimit:<user_id>:<route_id>'", 'sequenceNumber': 2, 'type': 'CacheOperation', 'isSynchronous': True, 'returnMessage': 'Current count (<= limit)', 'hasReturn': True, 'isActivation': False, 'technicalDetails': {'protocol': 'Redis Protocol', 'method': 'INCR', 'parameters': {'Key': 'A composite key identifying the user and the specific API route being accessed.', 'TTL': 'The key is set with a TTL matching the rate limit window (e.g., 60 seconds).'}, 'authentication': 'Redis AUTH command with credentials from Kubernetes secret.', 'errorHandling': 'If Redis is unavailable, the gateway may fallback to in-memory limiting per-pod, or fail open/closed based on policy.', 'performance': {'latency': '<2ms'}}}

### 2.5.2.0.0.0 Request

#### 2.5.2.1.0.0 Source Id

EXT-CLIENT-APP

#### 2.5.2.2.0.0 Target Id

REPO-GW-API

#### 2.5.2.3.0.0 Message

POST /api/v1/assets (Request N+1)

#### 2.5.2.4.0.0 Sequence Number

3

#### 2.5.2.5.0.0 Type

🔹 Request

#### 2.5.2.6.0.0 Is Synchronous

✅ Yes

#### 2.5.2.7.0.0 Return Message

429 Too Many Requests

#### 2.5.2.8.0.0 Has Return

✅ Yes

#### 2.5.2.9.0.0 Is Activation

✅ Yes

#### 2.5.2.10.0.0 Technical Details

##### 2.5.2.10.1.0 Protocol

HTTP/1.1

##### 2.5.2.10.2.0 Method

POST

##### 2.5.2.10.3.0 Parameters

| Property | Value |
|----------|-------|
| Headers | ['Authorization: Bearer <jwt_token>'] |

##### 2.5.2.10.4.0 Authentication

JWT Bearer Token validation succeeds, but rate limit check is performed before proxying.

##### 2.5.2.10.5.0 Error Handling

###### 2.5.2.10.5.1 Strategy

Immediate rejection without proxying to backend service.

###### 2.5.2.10.5.2 Response

####### 2.5.2.10.5.2.1 Status Code

429

####### 2.5.2.10.5.2.2 Headers

- Retry-After: <seconds_until_window_resets>

####### 2.5.2.10.5.2.3 Body

{ "message": "API rate limit exceeded" }

##### 2.5.2.10.6.0.0 Performance

###### 2.5.2.10.6.1.0 Latency

<5ms (Gateway response only)

#### 2.5.2.11.0.0.0 Nested Interactions

##### 2.5.2.11.1.0.0 CacheOperation

###### 2.5.2.11.1.1.0 Source Id

REPO-GW-API

###### 2.5.2.11.1.2.0 Target Id

INT-REDIS-CACHE

###### 2.5.2.11.1.3.0 Message

INCR 'ratelimit:<user_id>:<route_id>'

###### 2.5.2.11.1.4.0 Sequence Number

4

###### 2.5.2.11.1.5.0 Type

🔹 CacheOperation

###### 2.5.2.11.1.6.0 Is Synchronous

✅ Yes

###### 2.5.2.11.1.7.0 Return Message

Current count (> limit)

###### 2.5.2.11.1.8.0 Has Return

✅ Yes

###### 2.5.2.11.1.9.0 Is Activation

❌ No

###### 2.5.2.11.1.10.0 Technical Details

####### 2.5.2.11.1.10.1 Protocol

Redis Protocol

####### 2.5.2.11.1.10.2 Method

INCR

####### 2.5.2.11.1.10.3 Parameters

*No data available*

####### 2.5.2.11.1.10.4 Authentication

Redis AUTH

####### 2.5.2.11.1.10.5 Error Handling

N/A

####### 2.5.2.11.1.10.6 Performance

######## 2.5.2.11.1.10.6.1 Latency

<2ms

##### 2.5.2.11.2.0.0.0 InternalLogic

###### 2.5.2.11.2.1.0.0 Source Id

REPO-GW-API

###### 2.5.2.11.2.2.0.0 Target Id

REPO-GW-API

###### 2.5.2.11.2.3.0.0 Message

[Decision] Count exceeds configured limit. Block request.

###### 2.5.2.11.2.4.0.0 Sequence Number

5

###### 2.5.2.11.2.5.0.0 Type

🔹 InternalLogic

###### 2.5.2.11.2.6.0.0 Is Synchronous

❌ No

###### 2.5.2.11.2.7.0.0 Has Return

❌ No

###### 2.5.2.11.2.8.0.0 Is Activation

❌ No

###### 2.5.2.11.2.9.0.0 Technical Details

*No data available*

## 2.6.0.0.0.0.0.0 Notes

### 2.6.1.0.0.0.0.0 Content

#### 2.6.1.1.0.0.0.0 Content

The identifier for rate limiting (`user_id`) is configurable. It can be derived from the JWT 'sub' claim, a consumer API key, or the client's IP address. Using the JWT claim is preferred for multi-tenant accuracy.

#### 2.6.1.2.0.0.0.0 Position

top-right

#### 2.6.1.3.0.0.0.0 Participant Id

REPO-GW-API

#### 2.6.1.4.0.0.0.0 Sequence Number

1

### 2.6.2.0.0.0.0.0 Content

#### 2.6.2.1.0.0.0.0 Content

The rate limit (e.g., 100 requests/minute) and policy are defined declaratively in the Kong API Gateway configuration (via Kubernetes CRDs) and are not hardcoded.

#### 2.6.2.2.0.0.0.0 Position

bottom-right

#### 2.6.2.3.0.0.0.0 Participant Id

REPO-GW-API

#### 2.6.2.4.0.0.0.0 Sequence Number

3

## 2.7.0.0.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | The identifier used for rate limiting (e.g., 'sub'... |
| Performance Targets | The overhead of the rate-limiting check must be ex... |
| Error Handling Strategy | This sequence is the primary error handling strate... |
| Testing Considerations | Load testing is required to validate the accuracy ... |
| Monitoring Requirements | Prometheus metrics from the Kong gateway must be s... |
| Deployment Considerations | Rate limiting policies must be deployed declarativ... |

