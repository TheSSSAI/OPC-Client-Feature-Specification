# 1 Overview

## 1.1 Diagram Id

SEQ-OF-001

## 1.2 Name

Secure OPC Core Client Provisioning

## 1.3 Description

An Administrator generates a one-time registration token in the UI. A newly deployed OPC Core Client uses this token on its first connection to authenticate, submit a Certificate Signing Request (CSR), and receive a unique, long-term client certificate for all subsequent mTLS communication.

## 1.4 Type

🔹 OperationalFlow

## 1.5 Purpose

To securely bootstrap and provision new edge client instances, establishing a unique, trusted identity for each client without manual certificate handling, as per REQ-NFR-003.

## 1.6 Complexity

High

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-DVM
- REPO-EDGE-OPC

## 1.10 Key Interactions

- Admin generates a one-time token in the Central Management Plane.
- Token is securely provided to the new edge client via an out-of-band mechanism.
- Client connects to the Device Management Service's public provisioning endpoint using the token for authentication.
- Client generates a private key, creates a CSR, and sends it to the service.
- Device Management Service validates the request, signs the certificate using an internal CA, and returns the signed public certificate to the client.
- Client stores the private key and signed certificate and uses them for all future mTLS communication with services like data ingestion.

## 1.11 Triggers

- A new OPC Core Client instance is deployed to an edge location and needs to be registered.

## 1.12 Outcomes

- The OPC Core Client is securely registered with a unique identity within the system.
- The client possesses a unique, long-term certificate for mTLS authentication, preventing unauthorized access.

## 1.13 Business Rules

- Registration tokens must be single-use and have a short expiry time to limit the window of opportunity for misuse (REQ-NFR-003).
- Upon successful registration, the token is invalidated immediately.

## 1.14 Error Scenarios

- The provided token is expired, invalid, or has already been used.
- The certificate signing process fails on the server.
- The client fails to securely store the new certificate, requiring the process to be restarted.

## 1.15 Integration Points

*No items available*

# 2.0 Details

## 2.1 Diagram Id

SEQ-OF-001

## 2.2 Name

Implementation of Secure OPC Core Client Provisioning

## 2.3 Description

A detailed technical sequence for the secure bootstrapping of a new OPC Core Client instance. It covers the administrator-initiated one-time token generation, and the client's use of that token to authenticate, submit a Certificate Signing Request (CSR), and receive a signed client certificate for mTLS communication. This flow is critical for establishing a trusted identity for edge devices as per REQ-NFR-003.

## 2.4 Participants

### 2.4.1 Web SPA

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Frontend (Management Plane)

#### 2.4.1.3 Type

🔹 Web SPA

#### 2.4.1.4 Technology

React 18, TypeScript

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #4285F4 |
| Stereotype | UI |

### 2.4.2.0 Gateway

#### 2.4.2.1 Repository Id

REPO-GW-API

#### 2.4.2.2 Display Name

API Gateway

#### 2.4.2.3 Type

🔹 Gateway

#### 2.4.2.4 Technology

Kong v3.7.0 on K8s

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #34A853 |
| Stereotype | Gateway |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-DVM

#### 2.4.3.2 Display Name

Device Management Service

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
| Color | #FBBC05 |
| Stereotype | Service |

### 2.4.4.0 Edge Application

#### 2.4.4.1 Repository Id

REPO-EDGE-OPC

#### 2.4.4.2 Display Name

OPC Core Client (Unprovisioned)

#### 2.4.4.3 Type

🔹 Edge Application

#### 2.4.4.4 Technology

.NET 8, Docker

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #EA4335 |
| Stereotype | Edge Client |

## 2.5.0.0 Interactions

### 2.5.1.0 REST Request

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

1. Request new client registration token

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 REST Request

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

4. Return one-time token and expiry

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

##### 2.5.1.10.1 Protocol

HTTPS/1.1

##### 2.5.1.10.2 Method

POST /api/v1/clients/registration-token

##### 2.5.1.10.3 Parameters

Body: { "clientDescription": "SiteA-Line1-Client", "tenantId": "<tenant_id>" }

##### 2.5.1.10.4 Authentication

Requires valid Administrator JWT. `Authorization: Bearer <admin_jwt>`

##### 2.5.1.10.5 Error Handling

Handles 401 (Unauthorized), 403 (Forbidden), 50x from downstream.

##### 2.5.1.10.6 Performance

###### 2.5.1.10.6.1 Latency

<200ms P95

### 2.5.2.0.0.0 REST Request

#### 2.5.2.1.0.0 Source Id

REPO-GW-API

#### 2.5.2.2.0.0 Target Id

REPO-SVC-DVM

#### 2.5.2.3.0.0 Message

2. Forward token generation request

#### 2.5.2.4.0.0 Sequence Number

2

#### 2.5.2.5.0.0 Type

🔹 REST Request

#### 2.5.2.6.0.0 Is Synchronous

✅ Yes

#### 2.5.2.7.0.0 Return Message

3. Return generated token details

#### 2.5.2.8.0.0 Has Return

✅ Yes

#### 2.5.2.9.0.0 Is Activation

✅ Yes

#### 2.5.2.10.0.0 Technical Details

##### 2.5.2.10.1.0 Protocol

HTTP (in-cluster)

##### 2.5.2.10.2.0 Method

POST /registration-token

##### 2.5.2.10.3.0 Parameters

Body: { ... }

##### 2.5.2.10.4.0 Authentication

JWT validated by Gateway; service trusts forwarded claims (e.g., in `X-User-Info` header).

##### 2.5.2.10.5.0 Error Handling

Service returns 400 for bad input, 500 for generation failure.

##### 2.5.2.10.6.0 Performance

###### 2.5.2.10.6.1 Latency

<150ms

#### 2.5.2.11.0.0 Nested Interactions

*No items available*

### 2.5.3.0.0.0 REST Request

#### 2.5.3.1.0.0 Source Id

REPO-EDGE-OPC

#### 2.5.3.2.0.0 Target Id

REPO-SVC-DVM

#### 2.5.3.3.0.0 Message

5. Register with token and submit CSR

#### 2.5.3.4.0.0 Sequence Number

5

#### 2.5.3.5.0.0 Type

🔹 REST Request

#### 2.5.3.6.0.0 Is Synchronous

✅ Yes

#### 2.5.3.7.0.0 Return Message

8. Return signed client certificate

#### 2.5.3.8.0.0 Has Return

✅ Yes

#### 2.5.3.9.0.0 Is Activation

✅ Yes

#### 2.5.3.10.0.0 Technical Details

##### 2.5.3.10.1.0 Protocol

HTTPS/TLS 1.3

##### 2.5.3.10.2.0 Method

POST /provision/register

##### 2.5.3.10.3.0 Parameters

Body: { "csr": "<PEM-encoded CSR>" }

##### 2.5.3.10.4.0 Authentication

Authentication via one-time token. `Authorization: Bearer <one_time_token>`

##### 2.5.3.10.5.0 Error Handling

Handles 401 (Invalid/Expired Token), 400 (Malformed CSR), 500 (CA signing failure).

##### 2.5.3.10.6.0 Performance

###### 2.5.3.10.6.1 Latency

<1000ms

### 2.5.4.0.0.0 Internal Process

#### 2.5.4.1.0.0 Source Id

REPO-SVC-DVM

#### 2.5.4.2.0.0 Target Id

REPO-SVC-DVM

#### 2.5.4.3.0.0 Message

6. Validate token, generate/sign certificate

#### 2.5.4.4.0.0 Sequence Number

6

#### 2.5.4.5.0.0 Type

🔹 Internal Process

#### 2.5.4.6.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0 Return Message



#### 2.5.4.8.0.0 Has Return

❌ No

#### 2.5.4.9.0.0 Is Activation

❌ No

#### 2.5.4.10.0.0 Technical Details

##### 2.5.4.10.1.0 Protocol

Internal Logic

##### 2.5.4.10.2.0 Method

ValidateTokenAndSignCsr()

##### 2.5.4.10.3.0 Parameters

Input: token, CSR. Process: 1. Hash token and query DB. 2. Check expiry and 'used' flag. 3. If valid, mark as used. 4. Validate CSR fields. 5. Load internal CA key from Secrets Manager. 6. Sign CSR.

##### 2.5.4.10.4.0 Authentication

N/A

##### 2.5.4.10.5.0 Error Handling

Throws exceptions for token validation failure, CSR parsing error, or CA signing error.

##### 2.5.4.10.6.0 Performance

###### 2.5.4.10.6.1 Latency

<800ms

### 2.5.5.0.0.0 Internal Process

#### 2.5.5.1.0.0 Source Id

REPO-EDGE-OPC

#### 2.5.5.2.0.0 Target Id

REPO-EDGE-OPC

#### 2.5.5.3.0.0 Message

9. Securely store private key and certificate

#### 2.5.5.4.0.0 Sequence Number

9

#### 2.5.5.5.0.0 Type

🔹 Internal Process

#### 2.5.5.6.0.0 Is Synchronous

✅ Yes

#### 2.5.5.7.0.0 Return Message



#### 2.5.5.8.0.0 Has Return

❌ No

#### 2.5.5.9.0.0 Is Activation

❌ No

#### 2.5.5.10.0.0 Technical Details

##### 2.5.5.10.1.0 Protocol

File System / Secure Storage

##### 2.5.5.10.2.0 Method

StoreCredentials()

##### 2.5.5.10.3.0 Parameters

Saves the private key and received certificate to a persistent, access-controlled location (e.g., a protected Docker volume).

##### 2.5.5.10.4.0 Authentication

N/A

##### 2.5.5.10.5.0 Error Handling

Critical failure if storage fails. The client must not proceed and should enter a failed state, requiring a new provisioning attempt.

##### 2.5.5.10.6.0 Performance

###### 2.5.5.10.6.1 Latency

<50ms

## 2.6.0.0.0.0 Notes

### 2.6.1.0.0.0 Content

#### 2.6.1.1.0.0 Content

Administrator securely transfers the one-time token to the new client via an out-of-band mechanism (e.g., environment variable in a deployment script, cloud-init, etc.). This transfer is outside the scope of this sequence.

#### 2.6.1.2.0.0 Position

top

#### 2.6.1.3.0.0 Participant Id

*Not specified*

#### 2.6.1.4.0.0 Sequence Number

4

### 2.6.2.0.0.0 Content

#### 2.6.2.1.0.0 Content

The `/provision/register` endpoint must be publicly accessible but secured via the one-time token. It can be exposed via a dedicated Ingress route that uses a different authentication plugin than the main `/api/v1` routes.

#### 2.6.2.2.0.0 Position

bottom

#### 2.6.2.3.0.0 Participant Id

REPO-SVC-DVM

#### 2.6.2.4.0.0 Sequence Number

5

### 2.6.3.0.0.0 Content

#### 2.6.3.1.0.0 Content



```
Database Table `ClientRegistrationTokens`:
- `token_hash` (string, PK)
- `tenant_id` (uuid, FK)
- `client_id` (uuid, FK, nullable)
- `expires_at` (timestamp)
- `is_used` (boolean, default: false)
- `created_by` (string)
```

#### 2.6.3.2.0.0 Position

bottom

#### 2.6.3.3.0.0 Participant Id

REPO-SVC-DVM

#### 2.6.3.4.0.0 Sequence Number

2

### 2.6.4.0.0.0 Content

#### 2.6.4.1.0.0 Content

Client is now provisioned. All subsequent communications (e.g., gRPC data streaming) will use the stored certificate for mTLS authentication.

#### 2.6.4.2.0.0 Position

bottom

#### 2.6.4.3.0.0 Participant Id

REPO-EDGE-OPC

#### 2.6.4.4.0.0 Sequence Number

9

## 2.7.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | 1. **Token Security**: Registration tokens must be... |
| Performance Targets | This is not a high-throughput workflow. The primar... |
| Error Handling Strategy | 1. **Invalid/Expired Token**: The `/provision/regi... |
| Testing Considerations | 1. **Unit Tests**: Test the token generation and v... |
| Monitoring Requirements | 1. **Metrics**: The Device Management Service shou... |
| Deployment Considerations | The process of providing the one-time token to the... |

