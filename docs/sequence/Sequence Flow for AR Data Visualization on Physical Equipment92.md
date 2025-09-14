# 1 Overview

## 1.1 Diagram Id

SEQ-IF-003

## 1.2 Name

AR Data Visualization on Physical Equipment

## 1.3 Description

An operator wearing an AR device (e.g., HoloLens 2) looks at a piece of equipment marked with a QR code. The AR application reads the code, which maps to an asset ID in the system. The app calls the Central Management Plane's API to fetch real-time OPC tag data for that asset and displays it as a holographic overlay in the operator's field of view.

## 1.4 Type

🔹 IntegrationFlow

## 1.5 Purpose

To provide operators with hands-free, in-context access to live operational data by visualizing it directly on physical equipment, improving efficiency and situational awareness (REQ-FR-018).

## 1.6 Complexity

High

## 1.7 Priority

🟡 Medium

## 1.8 Frequency

OnDemand

## 1.9 Participants

- AR Device (HoloLens)
- REPO-GW-API
- REPO-SVC-AST
- REPO-SVC-DQR

## 1.10 Key Interactions

- The AR device's camera scans a QR code mounted on a machine.
- The AR app sends the QR code data (containing the asset ID) to the Asset Service to validate and retrieve the list of associated OPC tags.
- The AR app then subscribes to or polls the Query Service for real-time values of those specific tags.
- The Query Service provides the latest data points.
- The AR app renders the data (e.g., 'Temperature: 95 C', 'Pressure: 3.2 bar') as a 3D overlay anchored to the QR code's physical location.

## 1.11 Triggers

- An operator uses an AR device to view a configured asset marker in their environment.

## 1.12 Outcomes

- The operator sees live data as a hologram on or near the machine.
- The operator can interact with the system and view data without using a separate screen or tablet.

## 1.13 Business Rules

- A configuration tool must exist to create and manage the mapping between OPC tags and their corresponding physical asset markers (REQ-FR-018).
- The system must provide a REST API for integration with AR devices (REQ-IFC-002).

## 1.14 Error Scenarios

- The QR code is not recognized or is not mapped to any asset in the system.
- The API is unavailable, and no data can be fetched.
- Network latency is too high, causing a noticeable lag in the displayed data, making it unreliable for operators.

## 1.15 Integration Points

- AR Devices (e.g., Microsoft HoloLens 2)

# 2.0 Details

## 2.1 Diagram Id

SEQ-IF-003

## 2.2 Name

Implementation: AR Device Real-Time Data Fetch via API Gateway

## 2.3 Description

This sequence details the technical implementation of an Augmented Reality client fetching real-time operational data for a specific asset. The process is initiated by scanning a QR code, followed by two distinct, sequential API calls through the API Gateway: first to the Asset & Topology Service to resolve the asset's associated tags, and second to the Query & Analytics Service to fetch the latest values for those tags. The sequence emphasizes secure, low-latency communication required for a responsive user experience.

## 2.4 Participants

### 2.4.1 External Client

#### 2.4.1.1 Repository Id

AR Device (HoloLens)

#### 2.4.1.2 Display Name

AR Device (HoloLens)

#### 2.4.1.3 Type

🔹 External Client

#### 2.4.1.4 Technology

UWP/Unity Application with REST Client

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1168bd |
| Stereotype | <<External System>> |

### 2.4.2.0 APIGateway

#### 2.4.2.1 Repository Id

REPO-GW-API

#### 2.4.2.2 Display Name

API Gateway (Kong)

#### 2.4.2.3 Type

🔹 APIGateway

#### 2.4.2.4 Technology

Kong v3.7.0 on Kubernetes

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #9055A2 |
| Stereotype | <<Gateway>> |

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
| Color | #218380 |
| Stereotype | <<Service>> |

### 2.4.4.0 Microservice

#### 2.4.4.1 Repository Id

REPO-SVC-DQR

#### 2.4.4.2 Display Name

Query & Analytics Service

#### 2.4.4.3 Type

🔹 Microservice

#### 2.4.4.4 Technology

.NET 8 / ASP.NET Core

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #D7263D |
| Stereotype | <<Service>> |

## 2.5.0.0 Interactions

### 2.5.1.0 Internal Processing

#### 2.5.1.1 Source Id

AR Device (HoloLens)

#### 2.5.1.2 Target Id

AR Device (HoloLens)

#### 2.5.1.3 Message

1. Decode QR Code and extract `assetId`

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Internal Processing

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

❌ No

#### 2.5.1.9 Is Activation

❌ No

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Local Library Call |
| Method | QRDecoder.decode() |
| Parameters | Camera stream buffer |
| Authentication | N/A |
| Error Handling | If decoding fails or `assetId` format is invalid, ... |

### 2.5.2.0 API Request

#### 2.5.2.1 Source Id

AR Device (HoloLens)

#### 2.5.2.2 Target Id

REPO-GW-API

#### 2.5.2.3 Message

2. Request associated tag identifiers for the asset

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 API Request

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

6. HTTP 200 OK with list of tag identifiers

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

✅ Yes

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | GET /api/v1/assets/{assetId}/tags |
| Parameters | URL Path: `assetId` (string, from QR code) |
| Authentication | Requires `Authorization: Bearer <JWT>` header. Tok... |
| Error Handling | Client-side retry (3 attempts, exponential backoff... |
| Performance | End-to-end latency must be < 200ms (P95). |

### 2.5.3.0 Internal API Call

#### 2.5.3.1 Source Id

REPO-GW-API

#### 2.5.3.2 Target Id

REPO-SVC-AST

#### 2.5.3.3 Message

3. Forward request to Asset Service

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Internal API Call

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

5. Return asset tag data

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (ClusterIP) |
| Method | GET /assets/{assetId}/tags |
| Parameters | URL Path: `assetId` (string) |
| Authentication | None (within trusted Kubernetes network boundary). |
| Error Handling | Service returns HTTP 404 if asset ID is not found.... |
| Performance | Service-level latency must be < 50ms (P95). Levera... |

### 2.5.4.0 Data Access

#### 2.5.4.1 Source Id

REPO-SVC-AST

#### 2.5.4.2 Target Id

REPO-SVC-AST

#### 2.5.4.3 Message

4. Retrieve tag mappings from DB/Cache

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 Data Access

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

Return data

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | SELECT ... FROM OpcTags WHERE AssetId = @assetId |
| Parameters | assetId |
| Authentication | Database connection string with credentials from A... |
| Error Handling | Handle database exceptions and return appropriate ... |

### 2.5.5.0 API Request

#### 2.5.5.1 Source Id

AR Device (HoloLens)

#### 2.5.5.2 Target Id

REPO-GW-API

#### 2.5.5.3 Message

7. Request real-time values for identified tags

#### 2.5.5.4 Sequence Number

7

#### 2.5.5.5 Type

🔹 API Request

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

11. HTTP 200 OK with latest data points for each tag

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

✅ Yes

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/1.1 |
| Method | GET /api/v1/query/realtime |
| Parameters | Query String: `tagIds=<id1>,<id2>,...` (comma-sepa... |
| Authentication | Requires `Authorization: Bearer <JWT>` header (sam... |
| Error Handling | Client-side retry (3 attempts, exponential backoff... |
| Performance | End-to-end latency must be < 250ms (P95) to ensure... |

### 2.5.6.0 Internal API Call

#### 2.5.6.1 Source Id

REPO-GW-API

#### 2.5.6.2 Target Id

REPO-SVC-DQR

#### 2.5.6.3 Message

8. Forward request to Query Service

#### 2.5.6.4 Sequence Number

8

#### 2.5.6.5 Type

🔹 Internal API Call

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message

10. Return latest data points

#### 2.5.6.8 Has Return

✅ Yes

#### 2.5.6.9 Is Activation

✅ Yes

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (ClusterIP) |
| Method | GET /query/realtime |
| Parameters | Query String: `tagIds=<id1>,<id2>,...` |
| Authentication | None (within trusted Kubernetes network boundary). |
| Error Handling | Returns 5xx on database connection issues. If some... |
| Performance | Service-level latency must be < 150ms (P95). |

### 2.5.7.0 Data Access

#### 2.5.7.1 Source Id

REPO-SVC-DQR

#### 2.5.7.2 Target Id

REPO-SVC-DQR

#### 2.5.7.3 Message

9. Query TimescaleDB for latest value of each tag

#### 2.5.7.4 Sequence Number

9

#### 2.5.7.5 Type

🔹 Data Access

#### 2.5.7.6 Is Synchronous

✅ Yes

#### 2.5.7.7 Return Message

Return latest data points

#### 2.5.7.8 Has Return

✅ Yes

#### 2.5.7.9 Is Activation

❌ No

#### 2.5.7.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | Optimized query using `LAST(value, timestamp)` or ... |
| Parameters | List of tag identifiers |
| Authentication | Database connection string with credentials from A... |
| Error Handling | Handle database exceptions and return appropriate ... |
| Performance | Query execution time must be < 100ms. Requires app... |

### 2.5.8.0 Internal Processing

#### 2.5.8.1 Source Id

AR Device (HoloLens)

#### 2.5.8.2 Target Id

AR Device (HoloLens)

#### 2.5.8.3 Message

12. Render data as holographic overlay

#### 2.5.8.4 Sequence Number

12

#### 2.5.8.5 Type

🔹 Internal Processing

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
| Protocol | Local Graphics API Call |
| Method | HologramRenderer.updateData() |
| Parameters | List of `(tag, value, timestamp, quality)` objects |
| Authentication | N/A |
| Error Handling | If a value's timestamp is older than a configurabl... |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Trigger: Operator wearing the AR device physically looks at a piece of equipment that has a pre-configured QR code marker.

#### 2.6.1.2 Position

top-left

#### 2.6.1.3 Participant Id

AR Device (HoloLens)

#### 2.6.1.4 Sequence Number

0

### 2.6.2.0 Content

#### 2.6.2.1 Content

The API Gateway is responsible for JWT validation using the public key from the IdP (Keycloak) and enforcing rate limits on the AR API endpoints.

#### 2.6.2.2 Position

bottom

#### 2.6.2.3 Participant Id

REPO-GW-API

#### 2.6.2.4 Sequence Number

2

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | The AR Device must be provisioned with a unique Cl... |
| Performance Targets | The total time from QR code scan to data display m... |
| Error Handling Strategy | The AR application must provide clear, user-facing... |
| Testing Considerations | E2E testing should be performed with a physical AR... |
| Monitoring Requirements | The API Gateway should expose Prometheus metrics f... |
| Deployment Considerations | The mapping of QR codes to asset IDs is a critical... |

