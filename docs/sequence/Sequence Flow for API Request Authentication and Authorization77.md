# 1 Overview

## 1.1 Diagram Id

SEQ-SF-001

## 1.2 Name

API Request Authentication and Authorization

## 1.3 Description

A client (e.g., the frontend) makes a request to a protected API endpoint. The API Gateway intercepts the request, validates the JWT's signature and expiry against the IdP's public keys. Upon successful validation, the request is forwarded to the backend microservice, which then performs fine-grained Role-Based Access Control (RBAC) checks based on claims within the token.

## 1.4 Type

🔹 SecurityFlow

## 1.5 Purpose

To enforce security for all API endpoints, ensuring that only authenticated users with the correct permissions can access or modify system resources, as per REQ-NFR-003.

## 1.6 Complexity

Medium

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- Keycloak
- REPO-SVC-AST

## 1.10 Key Interactions

- The frontend attaches the JWT as a Bearer token in the Authorization header of an API request.
- The API Gateway (Kong) uses its JWT plugin to validate the token's signature, issuer, and expiration against Keycloak's public keys.
- If valid, the API Gateway routes the request to the appropriate upstream service (e.g., Asset Service).
- The Asset Service's authorization middleware extracts user roles/permissions from the token payload.
- The service logic verifies if the user's role allows the requested action before executing the business logic.

## 1.11 Triggers

- Any API call to a protected endpoint within the Central Management Plane.

## 1.12 Outcomes

- Legitimate requests are processed successfully.
- Unauthorized or unauthenticated requests are rejected at the gateway or service layer with a 401 or 403 status code.

## 1.13 Business Rules

- All API access must be secured using JWT Bearer Tokens (REQ-NFR-003).
- The RBAC model must be enforced within each microservice to check for specific permissions (REQ-NFR-003).

## 1.14 Error Scenarios

- The JWT is expired, has an invalid signature, or is missing.
- The user does not have the required role/permission for the specific action.
- The API Gateway cannot retrieve the public keys from Keycloak to perform validation.

## 1.15 Integration Points

- Keycloak for JWT public key retrieval.

# 2.0 Details

## 2.1 Diagram Id

SEQ-SF-001

## 2.2 Name

API Request Authentication and Authorization Flow

## 2.3 Description

Provides a comprehensive technical specification for the defense-in-depth security flow for every protected API request. The sequence details the initial token validation at the API Gateway (Layer 1) and the subsequent fine-grained RBAC authorization check at the backend microservice (Layer 2), implementing a Zero Trust principle where every request is authenticated and authorized.

## 2.4 Participants

### 2.4.1 FrontendApplication

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Frontend Client

#### 2.4.1.3 Type

🔹 FrontendApplication

#### 2.4.1.4 Technology

React 18, TypeScript, Axios

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #4CAF50 |
| Stereotype | User Agent |

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
| Color | #FFC107 |
| Stereotype | Kong |

### 2.4.3.0 IdentityProvider

#### 2.4.3.1 Repository Id

Keycloak

#### 2.4.3.2 Display Name

Identity Provider

#### 2.4.3.3 Type

🔹 IdentityProvider

#### 2.4.3.4 Technology

Keycloak

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #9C27B0 |
| Stereotype | IdP |

### 2.4.4.0 Microservice

#### 2.4.4.1 Repository Id

REPO-SVC-AST

#### 2.4.4.2 Display Name

Asset Service

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
| Color | #2196F3 |
| Stereotype | Backend |

## 2.5.0.0 Interactions

### 2.5.1.0 APIRequest

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-GW-API

#### 2.5.1.3 Message

Request Protected Resource: GET /api/v1/assets

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 APIRequest

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message

Returns 200 OK with asset data, 401 Unauthorized, or 403 Forbidden

#### 2.5.1.8 Has Return

✅ Yes

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

##### 2.5.1.10.1 Protocol

HTTPS/1.1

##### 2.5.1.10.2 Method

GET

##### 2.5.1.10.3 Parameters

- {'name': 'Authorization Header', 'value': 'Bearer <JWT>', 'description': 'JWT obtained during user login flow.'}

##### 2.5.1.10.4 Authentication

JWT Bearer Token

##### 2.5.1.10.5 Error Handling

Client-side logic to handle 401 (redirect to login) and 403 (display access denied message) responses.

##### 2.5.1.10.6 Performance

###### 2.5.1.10.6.1 Latency

< 200ms P95 (end-to-end)

### 2.5.2.0.0.0 PublicKeyRetrieval

#### 2.5.2.1.0.0 Source Id

REPO-GW-API

#### 2.5.2.2.0.0 Target Id

Keycloak

#### 2.5.2.3.0.0 Message

Fetch Public Keys (JWKS) if not cached

#### 2.5.2.4.0.0 Sequence Number

2

#### 2.5.2.5.0.0 Type

🔹 PublicKeyRetrieval

#### 2.5.2.6.0.0 Is Synchronous

✅ Yes

#### 2.5.2.7.0.0 Return Message

Returns JSON Web Key Set (JWKS)

#### 2.5.2.8.0.0 Has Return

✅ Yes

#### 2.5.2.9.0.0 Is Activation

❌ No

#### 2.5.2.10.0.0 Technical Details

##### 2.5.2.10.1.0 Protocol

HTTPS

##### 2.5.2.10.2.0 Method

GET /.well-known/openid-configuration/jwks_uri

##### 2.5.2.10.3.0 Parameters

*No items available*

##### 2.5.2.10.4.0 Authentication

None (Public endpoint)

##### 2.5.2.10.5.0 Error Handling

If Keycloak is unavailable, the Gateway will fail JWT validation and return a 503 Service Unavailable.

##### 2.5.2.10.6.0 Performance

###### 2.5.2.10.6.1 Latency

Critical path. Response should be cached with a TTL respecting Keycloak's key rotation policy.

### 2.5.3.0.0.0 InternalProcessing

#### 2.5.3.1.0.0 Source Id

REPO-GW-API

#### 2.5.3.2.0.0 Target Id

REPO-GW-API

#### 2.5.3.3.0.0 Message

Perform JWT Validation (Security Checkpoint 1)

#### 2.5.3.4.0.0 Sequence Number

3

#### 2.5.3.5.0.0 Type

🔹 InternalProcessing

#### 2.5.3.6.0.0 Is Synchronous

❌ No

#### 2.5.3.7.0.0 Return Message



#### 2.5.3.8.0.0 Has Return

❌ No

#### 2.5.3.9.0.0 Is Activation

❌ No

#### 2.5.3.10.0.0 Technical Details

##### 2.5.3.10.1.0 Protocol

Internal

##### 2.5.3.10.2.0 Method

Kong JWT Plugin Execution

##### 2.5.3.10.3.0 Parameters

###### 2.5.3.10.3.1 Token

####### 2.5.3.10.3.1.1 Name

Token

####### 2.5.3.10.3.1.2 Value

JWT from Authorization header

###### 2.5.3.10.3.2.0 Public Key

####### 2.5.3.10.3.2.1 Name

Public Key

####### 2.5.3.10.3.2.2 Value

Key from fetched/cached JWKS

##### 2.5.3.10.4.0.0 Authentication

N/A

##### 2.5.3.10.5.0.0 Error Handling

On failure (invalid signature, expired, invalid issuer/audience), aborts request and sends 401 Unauthorized to client.

##### 2.5.3.10.6.0.0 Performance

###### 2.5.3.10.6.1.0 Latency

< 10ms

### 2.5.4.0.0.0.0 ProxyRequest

#### 2.5.4.1.0.0.0 Source Id

REPO-GW-API

#### 2.5.4.2.0.0.0 Target Id

REPO-SVC-AST

#### 2.5.4.3.0.0.0 Message

Forward Validated Request: GET /assets

#### 2.5.4.4.0.0.0 Sequence Number

4

#### 2.5.4.5.0.0.0 Type

🔹 ProxyRequest

#### 2.5.4.6.0.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0.0 Return Message

Returns HTTP response from the service (e.g., 200 OK, 403 Forbidden)

#### 2.5.4.8.0.0.0 Has Return

✅ Yes

#### 2.5.4.9.0.0.0 Is Activation

✅ Yes

#### 2.5.4.10.0.0.0 Technical Details

##### 2.5.4.10.1.0.0 Protocol

HTTP (internal cluster)

##### 2.5.4.10.2.0.0 Method

GET

##### 2.5.4.10.3.0.0 Parameters

- {'name': 'Forwarded Headers', 'value': 'Original Authorization header with JWT is passed through.'}

##### 2.5.4.10.4.0.0 Authentication

Internal network trust, JWT is for service-level authorization

##### 2.5.4.10.5.0.0 Error Handling

Handles upstream service unavailability (returns 503).

##### 2.5.4.10.6.0.0 Performance

###### 2.5.4.10.6.1.0 Latency

Dependent on upstream service.

### 2.5.5.0.0.0.0 InternalProcessing

#### 2.5.5.1.0.0.0 Source Id

REPO-SVC-AST

#### 2.5.5.2.0.0.0 Target Id

REPO-SVC-AST

#### 2.5.5.3.0.0.0 Message

Perform RBAC Authorization (Security Checkpoint 2)

#### 2.5.5.4.0.0.0 Sequence Number

5

#### 2.5.5.5.0.0.0 Type

🔹 InternalProcessing

#### 2.5.5.6.0.0.0 Is Synchronous

❌ No

#### 2.5.5.7.0.0.0 Return Message



#### 2.5.5.8.0.0.0 Has Return

❌ No

#### 2.5.5.9.0.0.0 Is Activation

❌ No

#### 2.5.5.10.0.0.0 Technical Details

##### 2.5.5.10.1.0.0 Protocol

Internal

##### 2.5.5.10.2.0.0 Method

ASP.NET Core Authorization Middleware

##### 2.5.5.10.3.0.0 Parameters

###### 2.5.5.10.3.1.0 JWT Claims

####### 2.5.5.10.3.1.1 Name

JWT Claims

####### 2.5.5.10.3.1.2 Value

Parsed roles, permissions, tenant_id from token payload.

###### 2.5.5.10.3.2.0 Policy

####### 2.5.5.10.3.2.1 Name

Policy

####### 2.5.5.10.3.2.2 Value

e.g., [Authorize(Policy="CanReadAssets")]

##### 2.5.5.10.4.0.0 Authentication

N/A

##### 2.5.5.10.5.0.0 Error Handling

On failure (user lacks required role/permission), aborts request and returns 403 Forbidden to the gateway.

##### 2.5.5.10.6.0.0 Performance

###### 2.5.5.10.6.1.0 Latency

< 5ms

### 2.5.6.0.0.0.0 BusinessLogic

#### 2.5.6.1.0.0.0 Source Id

REPO-SVC-AST

#### 2.5.6.2.0.0.0 Target Id

REPO-SVC-AST

#### 2.5.6.3.0.0.0 Message

Execute Business Logic (e.g., query database)

#### 2.5.6.4.0.0.0 Sequence Number

6

#### 2.5.6.5.0.0.0 Type

🔹 BusinessLogic

#### 2.5.6.6.0.0.0 Is Synchronous

❌ No

#### 2.5.6.7.0.0.0 Return Message



#### 2.5.6.8.0.0.0 Has Return

❌ No

#### 2.5.6.9.0.0.0 Is Activation

❌ No

#### 2.5.6.10.0.0.0 Technical Details

##### 2.5.6.10.1.0.0 Protocol

Internal

##### 2.5.6.10.2.0.0 Method

AssetController.GetAssets()

##### 2.5.6.10.3.0.0 Parameters

*No items available*

##### 2.5.6.10.4.0.0 Authentication

N/A

##### 2.5.6.10.5.0.0 Error Handling

Standard application error handling (e.g., database connection issues).

##### 2.5.6.10.6.0.0 Performance

###### 2.5.6.10.6.1.0 Latency

Variable

## 2.6.0.0.0.0.0 Notes

### 2.6.1.0.0.0.0 Content

#### 2.6.1.1.0.0.0 Content

Defense in Depth: This sequence demonstrates two layers of security. Layer 1 (Gateway) handles authentication, rejecting invalid requests early. Layer 2 (Service) handles fine-grained authorization, ensuring business rules are enforced.

#### 2.6.1.2.0.0.0 Position

top

#### 2.6.1.3.0.0.0 Participant Id

*Not specified*

#### 2.6.1.4.0.0.0 Sequence Number

*Not specified*

### 2.6.2.0.0.0.0 Content

#### 2.6.2.1.0.0.0 Content

JWKS Caching is critical for performance. The gateway should only fetch the JWKS from Keycloak on a cache miss or when keys are rotated.

#### 2.6.2.2.0.0.0 Position

right

#### 2.6.2.3.0.0.0 Participant Id

REPO-GW-API

#### 2.6.2.4.0.0.0 Sequence Number

2

### 2.6.3.0.0.0.0 Content

#### 2.6.3.1.0.0.0 Content

JWT Claims used for authorization must include tenant_id to enforce data isolation (REQ-CON-001) and user roles/permissions (REQ-USR-001).

#### 2.6.3.2.0.0.0 Position

right

#### 2.6.3.3.0.0.0 Participant Id

REPO-SVC-AST

#### 2.6.3.4.0.0.0 Sequence Number

5

## 2.7.0.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | 1. JWTs must be signed using an asymmetric algorit... |
| Performance Targets | 1. The entire authentication and authorization pro... |
| Error Handling Strategy | 1. **401 Unauthorized**: Returned by the API Gatew... |
| Testing Considerations | 1. Create automated tests for all token validation... |
| Monitoring Requirements | 1. **API Gateway**: Monitor the rate of 401 and 40... |
| Deployment Considerations | 1. The API Gateway's configuration for the JWT plu... |

