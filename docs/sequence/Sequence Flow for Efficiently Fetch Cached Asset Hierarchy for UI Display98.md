# 1 Overview

## 1.1 Diagram Id

SEQ-SI-001

## 1.2 Name

Efficiently Fetch Cached Asset Hierarchy for UI Display

## 1.3 Description

When a user navigates to the asset management page, the frontend requests the full asset hierarchy for their tenant. The Asset Service first checks its Redis cache. If the data is not present (a cache miss), it queries the PostgreSQL database, constructs the hierarchy, stores it in the cache for future requests, and returns it to the frontend.

## 1.4 Type

🔹 ServiceInteraction

## 1.5 Purpose

To efficiently provide the frontend with the data needed to render the asset tree, using caching to ensure a fast user experience and reduce load on the primary database for frequently accessed, slow-changing data.

## 1.6 Complexity

Medium

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-AST
- Redis
- PostgreSQL

## 1.10 Key Interactions

- Frontend calls GET /api/v1/assets.
- The Asset Service receives the request.
- It generates a cache key based on the tenant ID (e.g., 'tenant:{id}:asset-hierarchy') and attempts to get the hierarchy from Redis.
- Cache miss: It queries the PostgreSQL database for all assets belonging to the tenant.
- It processes the flat list into a hierarchical (tree) structure in application memory.
- It serializes the tree structure and stores it in Redis with a configured Time-To-Live (TTL).
- It returns the hierarchy to the frontend.
- Cache hit (subsequent request): The data is retrieved directly from Redis and returned, skipping steps 4-6.

## 1.11 Triggers

- A user opens a page that displays the asset hierarchy (e.g., asset management, tag mapping).

## 1.12 Outcomes

- The asset hierarchy is displayed quickly in the UI.
- Subsequent requests for the same data are served with very low latency from the cache.
- Database load is significantly reduced.

## 1.13 Business Rules

- Tenant data isolation must be enforced via tenant-specific cache keys to prevent data leakage (REQ-CON-001).
- Data from the database is the ultimate source of truth; the cache must be invalidated when the hierarchy is modified.

## 1.14 Error Scenarios

- The Redis cache is unavailable; the system should degrade gracefully by fetching from the database every time, albeit with higher latency.
- The database is unavailable; the request fails.
- Data serialization/deserialization from the cache fails, requiring a cache flush.

## 1.15 Integration Points

- Redis
- PostgreSQL

# 2.0 Details

## 2.1 Diagram Id

SEQ-SI-001

## 2.2 Name

Implementation: Efficiently Fetch Cached Asset Hierarchy

## 2.3 Description

Provides a detailed technical sequence for fetching the asset hierarchy for a given tenant, implementing a cache-aside pattern with Redis. This sequence prioritizes performance and scalability by first attempting to retrieve data from the cache, falling back to the primary PostgreSQL database only on a cache miss. The design includes graceful degradation for cache unavailability and enforces strict tenant data isolation.

## 2.4 Participants

### 2.4.1 Frontend

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Central Management Plane

#### 2.4.1.3 Type

🔹 Frontend

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

### 2.4.2.0 APIGateway

#### 2.4.2.1 Repository Id

REPO-GW-API

#### 2.4.2.2 Display Name

API Gateway

#### 2.4.2.3 Type

🔹 APIGateway

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

### 2.4.3.0 Service

#### 2.4.3.1 Repository Id

REPO-SVC-AST

#### 2.4.3.2 Display Name

Asset & Topology Service

#### 2.4.3.3 Type

🔹 Service

#### 2.4.3.4 Technology

.NET 8, ASP.NET Core

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #32CD32 |
| Stereotype | Microservice |

### 2.4.4.0 Cache

#### 2.4.4.1 Repository Id

Redis

#### 2.4.4.2 Display Name

Redis Cache

#### 2.4.4.3 Type

🔹 Cache

#### 2.4.4.4 Technology

Redis 7 on AWS ElastiCache

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #DC143C |
| Stereotype | Cache |

### 2.4.5.0 Database

#### 2.4.5.1 Repository Id

PostgreSQL

#### 2.4.5.2 Display Name

PostgreSQL Database

#### 2.4.5.3 Type

🔹 Database

#### 2.4.5.4 Technology

PostgreSQL 16 on AWS RDS

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #4682B4 |
| Stereotype | Database |

## 2.5.0.0 Interactions

### 2.5.1.0 Request

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

Requests asset hierarchy for the current user's tenant.

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

Returns asset hierarchy in JSON format or an error response.

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/2 |
| Method | GET /api/v1/assets |
| Parameters | None in body. Tenant context derived from JWT. |
| Authentication | Authorization: Bearer <JWT> |
| Error Handling | Client-side handling of 4xx/5xx HTTP status codes. |

### 2.5.2.0 SecurityCheck

#### 2.5.2.1 Source Id

REPO-GW-API

#### 2.5.2.2 Target Id

REPO-GW-API

#### 2.5.2.3 Message

Validates JWT and extracts tenantId from claims.

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 SecurityCheck

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message



#### 2.5.2.8 Has Return

❌ No

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | Kong JWT Plugin Verification |
| Parameters | JWT from Authorization header. |
| Authentication | N/A |
| Error Handling | If invalid, returns 401 Unauthorized immediately. |

### 2.5.3.0 ProxyRequest

#### 2.5.3.1 Source Id

REPO-GW-API

#### 2.5.3.2 Target Id

REPO-SVC-AST

#### 2.5.3.3 Message

Forwards validated request to the Asset Service.

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 ProxyRequest

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

Proxies HTTP response from Asset Service.

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (Internal Cluster DNS) |
| Method | GET /assets |
| Parameters | Adds 'X-Tenant-ID' header with value from JWT clai... |
| Authentication | Internal traffic, mTLS may be enforced by service ... |
| Error Handling | Forwards 5xx errors from upstream service. |

### 2.5.4.0 InternalProcessing

#### 2.5.4.1 Source Id

REPO-SVC-AST

#### 2.5.4.2 Target Id

REPO-SVC-AST

#### 2.5.4.3 Message

Generates tenant-specific cache key.

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 InternalProcessing

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message



#### 2.5.4.8 Has Return

❌ No

#### 2.5.4.9 Is Activation

✅ Yes

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | In-Memory |
| Method | string.Format("tenant:{0}:asset-hierarchy", tenant... |
| Parameters | tenantId from 'X-Tenant-ID' header. |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.5.0 CacheRead

#### 2.5.5.1 Source Id

REPO-SVC-AST

#### 2.5.5.2 Target Id

Redis

#### 2.5.5.3 Message

Attempts to retrieve asset hierarchy from cache.

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 CacheRead

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

Returns serialized hierarchy string or nil.

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

✅ Yes

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Redis Protocol |
| Method | GET tenant:{tenantId}:asset-hierarchy |
| Parameters | Cache Key |
| Authentication | Redis AUTH command if configured. |
| Error Handling | Implemented with Polly retry/circuit breaker. On f... |

### 2.5.6.0 DatabaseQuery

#### 2.5.6.1 Source Id

REPO-SVC-AST

#### 2.5.6.2 Target Id

PostgreSQL

#### 2.5.6.3 Message

Queries all assets for the specified tenant.

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 DatabaseQuery

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message

Returns a flat list of asset records.

#### 2.5.6.8 Has Return

✅ Yes

#### 2.5.6.9 Is Activation

✅ Yes

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | PostgreSQL Wire Protocol |
| Method | SELECT id, name, parent_id FROM assets WHERE tenan... |
| Parameters | tenantId |
| Authentication | Connection string credentials. |
| Error Handling | Implemented with Polly retry/circuit breaker. On f... |

### 2.5.7.0 InternalProcessing

#### 2.5.7.1 Source Id

REPO-SVC-AST

#### 2.5.7.2 Target Id

REPO-SVC-AST

#### 2.5.7.3 Message

Builds hierarchical tree structure from flat list.

#### 2.5.7.4 Sequence Number

7

#### 2.5.7.5 Type

🔹 InternalProcessing

#### 2.5.7.6 Is Synchronous

✅ Yes

#### 2.5.7.7 Return Message



#### 2.5.7.8 Has Return

❌ No

#### 2.5.7.9 Is Activation

✅ Yes

#### 2.5.7.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | In-Memory |
| Method | Recursive or iterative algorithm to build tree. |
| Parameters | List of asset records from database. |
| Authentication | N/A |
| Error Handling | Logs error if tree construction fails. |

### 2.5.8.0 CacheWrite

#### 2.5.8.1 Source Id

REPO-SVC-AST

#### 2.5.8.2 Target Id

Redis

#### 2.5.8.3 Message

Stores the newly built hierarchy in the cache with a TTL.

#### 2.5.8.4 Sequence Number

8

#### 2.5.8.5 Type

🔹 CacheWrite

#### 2.5.8.6 Is Synchronous

❌ No

#### 2.5.8.7 Return Message

Returns OK.

#### 2.5.8.8 Has Return

✅ Yes

#### 2.5.8.9 Is Activation

✅ Yes

#### 2.5.8.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Redis Protocol |
| Method | SETEX tenant:{tenantId}:asset-hierarchy 3600 <seri... |
| Parameters | Cache Key, TTL (in seconds), Serialized Hierarchy ... |
| Authentication | Redis AUTH command if configured. |
| Error Handling | Fire-and-forget approach. Failure is logged but do... |

### 2.5.9.0 Response

#### 2.5.9.1 Source Id

REPO-SVC-AST

#### 2.5.9.2 Target Id

REPO-GW-API

#### 2.5.9.3 Message

Returns the asset hierarchy.

#### 2.5.9.4 Sequence Number

9

#### 2.5.9.5 Type

🔹 Response

#### 2.5.9.6 Is Synchronous

✅ Yes

#### 2.5.9.7 Return Message



#### 2.5.9.8 Has Return

❌ No

#### 2.5.9.9 Is Activation

❌ No

#### 2.5.9.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP |
| Method | 200 OK |
| Parameters | Body: Asset hierarchy JSON payload. |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.10.0 Response

#### 2.5.10.1 Source Id

REPO-GW-API

#### 2.5.10.2 Target Id

REPO-FE-MPL

#### 2.5.10.3 Message

Forwards the successful response.

#### 2.5.10.4 Sequence Number

10

#### 2.5.10.5 Type

🔹 Response

#### 2.5.10.6 Is Synchronous

✅ Yes

#### 2.5.10.7 Return Message



#### 2.5.10.8 Has Return

❌ No

#### 2.5.10.9 Is Activation

❌ No

#### 2.5.10.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/2 |
| Method | 200 OK |
| Parameters | Body: Asset hierarchy JSON payload. |
| Authentication | N/A |
| Error Handling | N/A |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content



```
[ALT PATH: Cache Hit]
If Redis returns a value at Step 5, the service deserializes the cached data and immediately proceeds to Step 9, skipping database query (6), tree construction (7), and cache write (8). This is the expected path for >95% of requests.
```

#### 2.6.1.2 Position

RightOf

#### 2.6.1.3 Participant Id

Redis

#### 2.6.1.4 Sequence Number

5

### 2.6.2.0 Content

#### 2.6.2.1 Content



```
[ALT PATH: Cache Unavailable]
If the Redis request at Step 5 fails (e.g., timeout, connection error), the service logs a WARN, bypasses caching entirely, and proceeds directly to Step 6 (DB Query). The cache write at Step 8 is also skipped. The user receives data, but with higher latency.
```

#### 2.6.2.2 Position

RightOf

#### 2.6.2.3 Participant Id

Redis

#### 2.6.2.4 Sequence Number

5

### 2.6.3.0 Content

#### 2.6.3.1 Content



```
[ERROR PATH: Database Unavailable]
If the PostgreSQL query at Step 6 fails after retries, the service logs an ERROR and returns an HTTP 503 Service Unavailable response, which is propagated back to the user.
```

#### 2.6.3.2 Position

RightOf

#### 2.6.3.3 Participant Id

PostgreSQL

#### 2.6.3.4 Sequence Number

6

### 2.6.4.0 Content

#### 2.6.4.1 Content

Cache Invalidation: A separate process (e.g., an event-driven mechanism or a direct API call on asset modification) is responsible for invalidating this cache key (DELETE tenant:{tenantId}:asset-hierarchy) whenever an asset is created, updated, or deleted to prevent stale data.

#### 2.6.4.2 Position

Bottom

#### 2.6.4.3 Participant Id

*Not specified*

#### 2.6.4.4 Sequence Number

*Not specified*

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | The API Gateway MUST enforce JWT validation on eve... |
| Performance Targets | P95 Latency (Cache Hit): < 50ms. P95 Latency (Cach... |
| Error Handling Strategy | Utilize a library like Polly in the Asset Service ... |
| Testing Considerations | Unit tests must cover the hierarchy-building algor... |
| Monitoring Requirements | The Asset Service must expose Prometheus metrics f... |
| Deployment Considerations | Consider a cache warming strategy for high-priorit... |

