# 1 Overview

## 1.1 Diagram Id

SEQ-DF-002

## 1.2 Name

User Queries Historical Data for a Trend

## 1.3 Description

A user selects a tag and a time range in the web UI to view its historical trend. The frontend calls the Query Service, which executes an optimized SQL query against the TimescaleDB hypertable, performs any necessary server-side aggregation, and returns the time-series data for visualization in a chart.

## 1.4 Type

🔹 DataFlow

## 1.5 Purpose

To provide users with fast and efficient access to historical process data for analysis, reporting, and troubleshooting, as per REQ-FR-002.

## 1.6 Complexity

Medium

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-DQR
- TimescaleDB

## 1.10 Key Interactions

- A user defines a query (tag, start time, end time, aggregation function) in the UI.
- The frontend sends these query parameters to the Query Service API.
- The Query Service validates the request and constructs a SQL query optimized for TimescaleDB (e.g., using `time_bucket`).
- The query is executed against the TimescaleDB hypertable, leveraging time-based partitioning and indexes for performance.
- The data is returned to the frontend and rendered in a trend chart component.

## 1.11 Triggers

- A user wants to analyze the historical behavior of a process variable or asset property.

## 1.12 Outcomes

- The user can view a visual trend of the requested data within the UI.
- The query returns within the performance SLA (< 1 second for a 24-hour query).
- The user can export the queried data to CSV/Excel (REQ-FR-002).

## 1.13 Business Rules

- Queries for a single tag over a 24-hour period shall return in less than 1 second (REQ-NFR-001).
- The system must support data aggregation functions like min, max, and average (REQ-FR-002).

## 1.14 Error Scenarios

- The query times out due to a very large, unaggregated time range or an inefficient query plan.
- The TimescaleDB is unavailable.
- The user requests data for a tag that does not exist or they do not have permission to view.

## 1.15 Integration Points

- TimescaleDB

# 2.0 Details

## 2.1 Diagram Id

SEQ-DF-002

## 2.2 Name

Implementation: Historical Time-Series Data Query Flow

## 2.3 Description

This diagram provides a detailed technical specification for the sequence of interactions when a user queries historical time-series data. The flow begins with a user request in the React frontend, which is securely proxied through the Kong API Gateway to the .NET Query Service. The service performs validation and authorization, then constructs and executes a highly optimized SQL query against a TimescaleDB hypertable. The sequence emphasizes performance-critical aspects such as JWT validation, connection pooling, and the use of TimescaleDB-specific functions like `time_bucket` to meet the stringent performance SLA defined in REQ-NFR-001.

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
| Color | #3498DB |
| Stereotype | <<UI>> |

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
| Shape | component |
| Color | #F39C12 |
| Stereotype | <<Gateway>> |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-DQR

#### 2.4.3.2 Display Name

Query & Analytics Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8, ASP.NET Core, EF Core/Dapper

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #8E44AD |
| Stereotype | <<Service>> |

### 2.4.4.0 Database

#### 2.4.4.1 Repository Id

TimescaleDB

#### 2.4.4.2 Display Name

Time-Series Database

#### 2.4.4.3 Type

🔹 Database

#### 2.4.4.4 Technology

TimescaleDB on PostgreSQL 16

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #2ECC71 |
| Stereotype | <<Database>> |

## 2.5.0.0 Interactions

### 2.5.1.0 Request

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

1. GET /api/v1/query/historical?tagId=...&startTime=...&endTime=...&agg=avg

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | GET |
| Parameters | Query String: tagId (string), startTime (ISO 8601)... |
| Authentication | Authorization: Bearer <JWT> |
| Error Handling | Client-side handling of network errors or HTTP sta... |
| Performance | Initial request dispatch from browser. |

### 2.5.2.0 Security Check

#### 2.5.2.1 Source Id

REPO-GW-API

#### 2.5.2.2 Target Id

REPO-GW-API

#### 2.5.2.3 Message

2. Validate JWT

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Security Check

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message



#### 2.5.2.8 Has Return

❌ No

#### 2.5.2.9 Is Activation

❌ No

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | JWT Plugin Execution |
| Parameters | Validates token signature against IdP (Keycloak) p... |
| Authentication | N/A |
| Error Handling | If invalid, returns HTTP 401 Unauthorized to clien... |
| Performance | Sub-millisecond latency. |

### 2.5.3.0 Proxy

#### 2.5.3.1 Source Id

REPO-GW-API

#### 2.5.3.2 Target Id

REPO-SVC-DQR

#### 2.5.3.3 Message

3. Proxy validated request

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Proxy

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message



#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (via Kubernetes Service DNS) |
| Method | GET |
| Parameters | Forwards original request path and query string. A... |
| Authentication | Internal cluster traffic, potentially secured with... |
| Error Handling | If target service is unavailable, returns HTTP 503... |
| Performance | < 10ms latency contribution. |

### 2.5.4.0 Validation

#### 2.5.4.1 Source Id

REPO-SVC-DQR

#### 2.5.4.2 Target Id

REPO-SVC-DQR

#### 2.5.4.3 Message

4. Validate & Authorize Request

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 Validation

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message



#### 2.5.4.8 Has Return

❌ No

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | ApplicationService.GetHistoricalDataAsync() |
| Parameters | Input: HistoricalQueryDto. Validates time range an... |
| Authentication | Authorization check against user permissions. |
| Error Handling | Returns HTTP 400 for invalid params, HTTP 403 for ... |
| Performance | Permission checks should be cached to minimize lat... |

### 2.5.5.0 Database Query

#### 2.5.5.1 Source Id

REPO-SVC-DQR

#### 2.5.5.2 Target Id

TimescaleDB

#### 2.5.5.3 Message

5. Execute Optimized Time-Series Query

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 Database Query

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

6. Return Result Set

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

✅ Yes

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | PostgreSQL Wire Protocol |
| Method | SQL SELECT |
| Parameters | Parameterized SQL query using functions like time_... |
| Authentication | Connection pooling with credentials from AWS Secre... |
| Error Handling | Handles SQL exceptions (e.g., connection errors, t... |
| Performance | Leverages hypertable partitioning and composite in... |

### 2.5.6.0 Data Transformation

#### 2.5.6.1 Source Id

REPO-SVC-DQR

#### 2.5.6.2 Target Id

REPO-SVC-DQR

#### 2.5.6.3 Message

7. Map Data to DTO

#### 2.5.6.4 Sequence Number

7

#### 2.5.6.5 Type

🔹 Data Transformation

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message



#### 2.5.6.8 Has Return

❌ No

#### 2.5.6.9 Is Activation

❌ No

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal |
| Method | AutoMapper or manual mapping |
| Parameters | Maps the database result set (e.g., list of DataRo... |
| Authentication | N/A |
| Error Handling | Handles potential data type conversion errors. |
| Performance | Should be highly optimized to minimize overhead. |

### 2.5.7.0 Response

#### 2.5.7.1 Source Id

REPO-SVC-DQR

#### 2.5.7.2 Target Id

REPO-GW-API

#### 2.5.7.3 Message

8. HTTP 200 OK with JSON Payload

#### 2.5.7.4 Sequence Number

8

#### 2.5.7.5 Type

🔹 Response

#### 2.5.7.6 Is Synchronous

✅ Yes

#### 2.5.7.7 Return Message



#### 2.5.7.8 Has Return

❌ No

#### 2.5.7.9 Is Activation

❌ No

#### 2.5.7.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP |
| Method | Response |
| Parameters | Body: [{ "timestamp": "...", "value": 123.45 }, ..... |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Serialization of DTO list to JSON. |

### 2.5.8.0 Response

#### 2.5.8.1 Source Id

REPO-GW-API

#### 2.5.8.2 Target Id

REPO-FE-MPL

#### 2.5.8.3 Message

9. Proxy HTTP 200 OK Response

#### 2.5.8.4 Sequence Number

9

#### 2.5.8.5 Type

🔹 Response

#### 2.5.8.6 Is Synchronous

✅ Yes

#### 2.5.8.7 Return Message



#### 2.5.8.8 Has Return

❌ No

#### 2.5.8.9 Is Activation

❌ No

#### 2.5.8.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | Response |
| Parameters | Forwards the JSON payload from the backend service... |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Minimal latency. |

### 2.5.9.0 UI Update

#### 2.5.9.1 Source Id

REPO-FE-MPL

#### 2.5.9.2 Target Id

REPO-FE-MPL

#### 2.5.9.3 Message

10. Render Data in Trend Chart

#### 2.5.9.4 Sequence Number

10

#### 2.5.9.5 Type

🔹 UI Update

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
| Protocol | Internal (React State) |
| Method | Updates component state with fetched data. |
| Parameters | The received array of time-series points. |
| Authentication | N/A |
| Error Handling | Displays a user-friendly error message in the UI i... |
| Performance | Efficient rendering using a virtualized charting l... |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

REQ-NFR-001 Performance Target: The total time from step 5 to step 6 must be less than 1 second for a 24-hour query range.

#### 2.6.1.2 Position

bottom-right

#### 2.6.1.3 Participant Id

TimescaleDB

#### 2.6.1.4 Sequence Number

6

### 2.6.2.0 Content

#### 2.6.2.1 Content

REQ-FR-002 Aggregation: The SQL query in step 5 dynamically constructs the aggregation function (MIN, MAX, AVG) based on the user's request.

#### 2.6.2.2 Position

top-right

#### 2.6.2.3 Participant Id

REPO-SVC-DQR

#### 2.6.2.4 Sequence Number

5

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | The Query Service MUST enforce tenant isolation by... |
| Performance Targets | The P95 latency for the API endpoint (measured at ... |
| Error Handling Strategy | If TimescaleDB is unavailable, the Query Service s... |
| Testing Considerations | Performance tests must be created to validate the ... |
| Monitoring Requirements | A Grafana dashboard is required to monitor key met... |
| Deployment Considerations | Database schema migrations, particularly adding or... |

