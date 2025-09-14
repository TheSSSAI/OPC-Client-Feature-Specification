# 1 Overview

## 1.1 Diagram Id

SEQ-IF-005

## 1.2 Name

Connecting to a Digital Twin via Asset Administration Shell (AAS)

## 1.3 Description

The system connects to a Digital Twin hosted as an Asset Administration Shell. The OPC Core Client, instead of connecting to a physical OPC server, connects to the AAS endpoint. It browses the AAS's submodels, treats them like an OPC namespace, and reads/writes property values, clearly indicating in the UI that the connection is to a simulation.

## 1.4 Type

🔹 IntegrationFlow

## 1.5 Purpose

To enable simulation, testing, and what-if analysis by integrating with standardized Digital Twins, as per REQ-FR-020.

## 1.6 Complexity

High

## 1.7 Priority

🟡 Medium

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-EDGE-OPC
- Asset Administration Shell

## 1.10 Key Interactions

- A user configures a new connection in the system, selecting 'Digital Twin (AAS)' as the type.
- The OPC Core Client establishes a connection to the AAS HTTP/REST endpoint.
- The client browses the AAS structure, mapping submodels and their properties to a tag-like structure.
- The client can now read and write values to the Digital Twin's properties.
- The Central Management Plane UI clearly labels all data from this source as 'Simulation'.

## 1.11 Triggers

- A user needs to test a new configuration or AI model against a simulated asset.

## 1.12 Outcomes

- The system is connected to and exchanging data with a Digital Twin.
- Users can perform actions against the simulation without risk to physical equipment.

## 1.13 Business Rules

- The client must support connecting via the AAS standard (REQ-FR-020).
- The UI must clearly distinguish between physical and digital twin connections (REQ-FR-020).

## 1.14 Error Scenarios

- The AAS endpoint is unavailable.
- The AAS model is not compliant with the standard and cannot be browsed correctly.
- A user mistakes a digital twin connection for a physical one.

## 1.15 Integration Points

- Asset Administration Shell (AAS) compliant Digital Twins

# 2.0 Details

## 2.1 Diagram Id

SEQ-IF-005

## 2.2 Name

Implementation: Digital Twin Connection via AAS Adapter

## 2.3 Description

Provides a detailed technical sequence for the OPC Core Client connecting to an external Digital Twin that conforms to the Asset Administration Shell (AAS) REST API specification. This sequence illustrates the Adapter Pattern, where the client's internal module translates AAS REST concepts (shells, submodels, properties) into an OPC-like tag structure for internal processing and data streaming. All data originating from this source is explicitly flagged as simulation data.

## 2.4 Participants

### 2.4.1 Microservice

#### 2.4.1.1 Repository Id

REPO-SVC-DVM

#### 2.4.1.2 Display Name

Device Management Service

#### 2.4.1.3 Type

🔹 Microservice

#### 2.4.1.4 Technology

.NET 8, ASP.NET Core, MQTT Client

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | Cloud Service |

### 2.4.2.0 Edge Application

#### 2.4.2.1 Repository Id

REPO-EDGE-OPC

#### 2.4.2.2 Display Name

OPC Core Client

#### 2.4.2.3 Type

🔹 Edge Application

#### 2.4.2.4 Technology

.NET 8, Docker, AAS Adapter Module

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #4682B4 |
| Stereotype | Edge |

### 2.4.3.0 External System

#### 2.4.3.1 Repository Id

ext-aas-digital-twin

#### 2.4.3.2 Display Name

Asset Administration Shell

#### 2.4.3.3 Type

🔹 External System

#### 2.4.3.4 Technology

HTTP/REST API Server

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | boundary |
| Color | #999999 |
| Stereotype | Digital Twin |

### 2.4.4.0 Microservice

#### 2.4.4.1 Repository Id

REPO-SVC-ING

#### 2.4.4.2 Display Name

Data Ingestion Service

#### 2.4.4.3 Type

🔹 Microservice

#### 2.4.4.4 Technology

.NET 8, gRPC Server

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | Cloud Service |

## 2.5.0.0 Interactions

### 2.5.1.0 Asynchronous Command

#### 2.5.1.1 Source Id

REPO-SVC-DVM

#### 2.5.1.2 Target Id

REPO-EDGE-OPC

#### 2.5.1.3 Message

[MQTT] publishClientConfigurationUpdate(type='AAS', endpoint='...', auth={...})

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Asynchronous Command

#### 2.5.1.6 Is Synchronous

❌ No

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

❌ No

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | MQTT v5 over TLS |
| Method | PUBLISH |
| Parameters | Topic: tenants/{tenantId}/clients/{clientId}/comma... |
| Authentication | Client Certificate (TLS) |
| Error Handling | MQTT QoS Level 1 for at-least-once delivery. Clien... |
| Performance | Latency < 500ms for command delivery. |

### 2.5.2.0 Internal Processing

#### 2.5.2.1 Source Id

REPO-EDGE-OPC

#### 2.5.2.2 Target Id

REPO-EDGE-OPC

#### 2.5.2.3 Message

processConfigurationUpdate(): Initialize AAS Adapter & Circuit Breaker

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Internal Processing

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
| Protocol | In-Process |
| Method | AASAdapter.Initialize() |
| Parameters | AAS Connection Configuration object. |
| Authentication | N/A |
| Error Handling | Log InvalidConfigurationError if endpoint or auth ... |
| Performance | Initialization should be < 50ms. |

### 2.5.3.0 API Call

#### 2.5.3.1 Source Id

REPO-EDGE-OPC

#### 2.5.3.2 Target Id

ext-aas-digital-twin

#### 2.5.3.3 Message

[HTTP GET] /shells

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 API Call

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

200 OK, { shells: [...] }

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 over TLS 1.3 |
| Method | GET |
| Parameters | Headers: 'Authorization: Bearer {token}' or 'X-API... |
| Authentication | Bearer Token or API Key |
| Error Handling | Retry on 5xx/timeout (3 attempts, exponential back... |
| Performance | Timeout set to 5 seconds. |

### 2.5.4.0 API Call

#### 2.5.4.1 Source Id

REPO-EDGE-OPC

#### 2.5.4.2 Target Id

ext-aas-digital-twin

#### 2.5.4.3 Message

[HTTP GET] /shells/{shellId}/submodels

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 API Call

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

200 OK, { submodels: [...] }

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 over TLS 1.3 |
| Method | GET |
| Parameters | Path parameter: shellId from previous step. |
| Authentication | Bearer Token or API Key |
| Error Handling | Same as initial connection: retry, circuit breaker... |
| Performance | Timeout set to 5 seconds. |

### 2.5.5.0 Internal Data Transformation

#### 2.5.5.1 Source Id

REPO-EDGE-OPC

#### 2.5.5.2 Target Id

REPO-EDGE-OPC

#### 2.5.5.3 Message

mapAasToInternalTagModel(): Translate submodels & properties to tag structure

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 Internal Data Transformation

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

Internal Tag Namespace created

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | In-Process |
| Method | AASMapper.Transform() |
| Parameters | JSON payload from AAS API. |
| Authentication | N/A |
| Error Handling | Log and skip any non-compliant submodels or proper... |
| Performance | Transformation for a large model (< 1000 propertie... |

### 2.5.6.0 Data Read

#### 2.5.6.1 Source Id

REPO-EDGE-OPC

#### 2.5.6.2 Target Id

ext-aas-digital-twin

#### 2.5.6.3 Message

[HTTP GET] /submodels/{submodelId}/submodel-elements/{elementId}/value

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 Data Read

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message

200 OK, { value: ... }

#### 2.5.6.8 Has Return

✅ Yes

#### 2.5.6.9 Is Activation

❌ No

#### 2.5.6.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 over TLS 1.3 |
| Method | GET |
| Parameters | Path parameters for specific property. |
| Authentication | Bearer Token or API Key |
| Error Handling | Retry on transient errors. Handle 404 if property ... |
| Performance | P95 latency should be < 500ms. |

### 2.5.7.0 Data Streaming

#### 2.5.7.1 Source Id

REPO-EDGE-OPC

#### 2.5.7.2 Target Id

REPO-SVC-ING

#### 2.5.7.3 Message

[gRPC Stream] streamDataPoint(tagId, timestamp, value, isSimulation=true)

#### 2.5.7.4 Sequence Number

7

#### 2.5.7.5 Type

🔹 Data Streaming

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
| Protocol | gRPC over HTTP/2 |
| Method | DataIngestion.StreamData |
| Parameters | Protocol Buffers message. The 'isSimulation' boole... |
| Authentication | Mutual TLS (mTLS) with client certificates. |
| Error Handling | Client-side buffering and retry with exponential b... |
| Performance | High throughput for real-time data. |

### 2.5.8.0 Data Write

#### 2.5.8.1 Source Id

REPO-EDGE-OPC

#### 2.5.8.2 Target Id

ext-aas-digital-twin

#### 2.5.8.3 Message

[HTTP PUT] /submodels/{submodelId}/submodel-elements/{elementId}/value

#### 2.5.8.4 Sequence Number

8

#### 2.5.8.5 Type

🔹 Data Write

#### 2.5.8.6 Is Synchronous

✅ Yes

#### 2.5.8.7 Return Message

204 No Content

#### 2.5.8.8 Has Return

✅ Yes

#### 2.5.8.9 Is Activation

❌ No

#### 2.5.8.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 over TLS 1.3 |
| Method | PUT |
| Parameters | Request Body: JSON containing the new value. |
| Authentication | Bearer Token or API Key |
| Error Handling | Handle 400 Bad Request for invalid data formats. H... |
| Performance | P95 latency should be < 500ms. |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Adapter Pattern: The OPC Core Client's 'AAS Adapter' module encapsulates all logic for HTTP/REST communication and transformation. Internally, the rest of the client interacts with the adapter via a standardized interface, unaware of the underlying REST protocol.

#### 2.6.1.2 Position

top

#### 2.6.1.3 Participant Id

REPO-EDGE-OPC

#### 2.6.1.4 Sequence Number

2

### 2.6.2.0 Content

#### 2.6.2.1 Content

Simulation Flag: The 'isSimulation=true' flag in the gRPC message is critical. The backend uses this flag to segregate simulation data and ensures the frontend applies a 'Simulation' label to all related visualizations, preventing user confusion as per REQ-FR-020.

#### 2.6.2.2 Position

right

#### 2.6.2.3 Participant Id

REPO-SVC-ING

#### 2.6.2.4 Sequence Number

7

### 2.6.3.0 Content

#### 2.6.3.1 Content

Circuit Breaker: A circuit breaker is implemented for all calls to the AAS. If the AAS becomes unresponsive, the breaker trips, preventing the client from making further requests for a configured cool-down period. The client's connection status is reported to the cloud as 'Degraded'.

#### 2.6.3.2 Position

bottom

#### 2.6.3.3 Participant Id

ext-aas-digital-twin

#### 2.6.3.4 Sequence Number

3

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Authentication credentials (API Key/Bearer Token) ... |
| Performance Targets | The initial connection and namespace browsing (ste... |
| Error Handling Strategy | The AAS Adapter must gracefully handle non-complia... |
| Testing Considerations | A mock AAS server that implements the required RES... |
| Monitoring Requirements | The AAS adapter module must expose Prometheus metr... |
| Deployment Considerations | The AAS Adapter is a software module within the `R... |

