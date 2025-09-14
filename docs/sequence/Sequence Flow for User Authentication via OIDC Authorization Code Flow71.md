# 1 Overview

## 1.1 Diagram Id

SEQ-AF-001

## 1.2 Name

User Authentication via OIDC Authorization Code Flow

## 1.3 Description

A user provides credentials to the web UI, which are validated by the centralized Identity Provider (Keycloak) via a standard OpenID Connect (OIDC) Authorization Code Flow. A successful authentication results in an ID Token and Access Token (JWT) being issued to the client for subsequent secure API calls.

## 1.4 Type

🔹 AuthenticationFlow

## 1.5 Purpose

To securely authenticate a user and establish a session for accessing the Central Management Plane, delegating identity management to a dedicated provider.

## 1.6 Complexity

Medium

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-IAM
- Keycloak

## 1.10 Key Interactions

- User submits credentials in the frontend application.
- Frontend redirects the user's browser to the Keycloak login page.
- Keycloak authenticates the user and redirects back with a single-use authorization code.
- Frontend sends the authorization code to the IAM service's token endpoint.
- IAM service securely exchanges the code for a JWT with Keycloak.
- The JWT is returned to the frontend and stored securely (e.g., in memory) for API requests.

## 1.11 Triggers

- An unauthenticated user attempts to access a protected page or clicks 'Login'.

## 1.12 Outcomes

- User is successfully logged into the application.
- A valid, short-lived JWT is available in the user's browser session.
- User is redirected to their default dashboard.

## 1.13 Business Rules

- Authentication must be handled by the central IdP supporting OAuth 2.0/OIDC (REQ-1-080).
- Configurable user session timeouts for inactivity must be enforced (REQ-1-039).

## 1.14 Error Scenarios

- User provides invalid credentials.
- Keycloak is unavailable.
- The authorization code is expired or invalid, causing the token exchange to fail.

## 1.15 Integration Points

- Keycloak Identity Provider

# 2.0 Details

## 2.1 Diagram Id

SEQ-AF-001

## 2.2 Name

User Authentication via OIDC Authorization Code Flow with PKCE

## 2.3 Description

A comprehensive technical sequence for user authentication using the OIDC Authorization Code Flow with PKCE. The flow starts with a user initiating a login from the frontend, which orchestrates a redirect to the Keycloak IdP. After successful credential validation, Keycloak returns an authorization code, which is securely exchanged by the backend IAM Service for JWTs (ID, Access, Refresh tokens). This sequence ensures a secure, standards-based authentication process.

## 2.4 Participants

### 2.4.1 FrontendApplication

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Frontend SPA

#### 2.4.1.3 Type

🔹 FrontendApplication

#### 2.4.1.4 Technology

React 18, TypeScript

#### 2.4.1.5 Order

2

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #4CAF50 |
| Stereotype | <<UI>> |

### 2.4.2.0 Actor

#### 2.4.2.1 Repository Id

UserBrowser

#### 2.4.2.2 Display Name

User's Browser

#### 2.4.2.3 Type

🔹 Actor

#### 2.4.2.4 Technology

Chrome, Firefox, etc.

#### 2.4.2.5 Order

1

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #000000 |
| Stereotype | <<User Agent>> |

### 2.4.3.0 IdentityProvider

#### 2.4.3.1 Repository Id

Keycloak

#### 2.4.3.2 Display Name

Keycloak IdP

#### 2.4.3.3 Type

🔹 IdentityProvider

#### 2.4.3.4 Technology

Keycloak v24.x

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #FFC107 |
| Stereotype | <<IdP>> |

### 2.4.4.0 ApiGateway

#### 2.4.4.1 Repository Id

REPO-GW-API

#### 2.4.4.2 Display Name

API Gateway

#### 2.4.4.3 Type

🔹 ApiGateway

#### 2.4.4.4 Technology

Kong v3.7.0

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | boundary |
| Color | #2196F3 |
| Stereotype | <<Gateway>> |

### 2.4.5.0 Microservice

#### 2.4.5.1 Repository Id

REPO-SVC-IAM

#### 2.4.5.2 Display Name

IAM Service

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
| Color | #9C27B0 |
| Stereotype | <<Service>> |

## 2.5.0.0 Interactions

### 2.5.1.0 UserInteraction

#### 2.5.1.1 Source Id

UserBrowser

#### 2.5.1.2 Target Id

REPO-FE-MPL

#### 2.5.1.3 Message

1. User initiates login (clicks 'Login' button).

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 UserInteraction

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

❌ No

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

##### 2.5.1.10.1 Protocol

DOM Event

##### 2.5.1.10.2 Method

onClick

##### 2.5.1.10.3 Parameters

*No items available*

##### 2.5.1.10.4 Authentication

N/A

##### 2.5.1.10.5 Error Handling

N/A

### 2.5.2.0.0 InternalProcessing

#### 2.5.2.1.0 Source Id

REPO-FE-MPL

#### 2.5.2.2.0 Target Id

REPO-FE-MPL

#### 2.5.2.3.0 Message

2. Generate PKCE code_verifier and code_challenge.

#### 2.5.2.4.0 Sequence Number

2

#### 2.5.2.5.0 Type

🔹 InternalProcessing

#### 2.5.2.6.0 Is Synchronous

✅ Yes

#### 2.5.2.7.0 Return Message

code_verifier, code_challenge

#### 2.5.2.8.0 Has Return

✅ Yes

#### 2.5.2.9.0 Is Activation

❌ No

#### 2.5.2.10.0 Technical Details

##### 2.5.2.10.1 Protocol

Internal Function Call

##### 2.5.2.10.2 Method

pkce.generate()

##### 2.5.2.10.3 Parameters

- Store `code_verifier` in secure, temporary storage (e.g., sessionStorage).

##### 2.5.2.10.4 Authentication

N/A

##### 2.5.2.10.5 Error Handling

Cryptography library errors are caught and logged.

### 2.5.3.0.0 Redirect

#### 2.5.3.1.0 Source Id

REPO-FE-MPL

#### 2.5.3.2.0 Target Id

UserBrowser

#### 2.5.3.3.0 Message

3. Construct and trigger redirect to Keycloak authorization endpoint.

#### 2.5.3.4.0 Sequence Number

3

#### 2.5.3.5.0 Type

🔹 Redirect

#### 2.5.3.6.0 Is Synchronous

❌ No

#### 2.5.3.7.0 Return Message



#### 2.5.3.8.0 Has Return

❌ No

#### 2.5.3.9.0 Is Activation

❌ No

#### 2.5.3.10.0 Technical Details

##### 2.5.3.10.1 Protocol

HTTP/1.1

##### 2.5.3.10.2 Method

window.location.href =

##### 2.5.3.10.3 Parameters

- `response_type`: 'code'
- `client_id`: 'central-management-plane-ui'
- `scope`: 'openid profile email'
- `redirect_uri`: 'https://cmp.example.com/callback'
- `state`: 'opaque-random-string-for-csrf-protection'
- `code_challenge`: 'base64url(sha256(code_verifier))'
- `code_challenge_method`: 'S256'

##### 2.5.3.10.4 Authentication

N/A

##### 2.5.3.10.5 Error Handling

N/A

### 2.5.4.0.0 Request

#### 2.5.4.1.0 Source Id

UserBrowser

#### 2.5.4.2.0 Target Id

Keycloak

#### 2.5.4.3.0 Message

4. GET /auth/realms/{realm}/protocol/openid-connect/auth?params...

#### 2.5.4.4.0 Sequence Number

4

#### 2.5.4.5.0 Type

🔹 Request

#### 2.5.4.6.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0 Return Message

HTML Login Page

#### 2.5.4.8.0 Has Return

✅ Yes

#### 2.5.4.9.0 Is Activation

✅ Yes

#### 2.5.4.10.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | GET |
| Parameters | See step 3. |
| Authentication | N/A |
| Error Handling | If client_id or redirect_uri is misconfigured, Key... |

### 2.5.5.0.0 UserInteraction

#### 2.5.5.1.0 Source Id

UserBrowser

#### 2.5.5.2.0 Target Id

Keycloak

#### 2.5.5.3.0 Message

5. User submits credentials (and MFA if required).

#### 2.5.5.4.0 Sequence Number

5

#### 2.5.5.5.0 Type

🔹 UserInteraction

#### 2.5.5.6.0 Is Synchronous

✅ Yes

#### 2.5.5.7.0 Return Message

HTTP 302 Redirect

#### 2.5.5.8.0 Has Return

✅ Yes

#### 2.5.5.9.0 Is Activation

❌ No

#### 2.5.5.10.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | POST /login-actions/authenticate |
| Parameters | Form data including username, password. |
| Authentication | User Credentials |
| Error Handling | If credentials are invalid, Keycloak re-renders th... |

### 2.5.6.0.0 Redirect

#### 2.5.6.1.0 Source Id

Keycloak

#### 2.5.6.2.0 Target Id

UserBrowser

#### 2.5.6.3.0 Message

6. Redirect back to application's redirect_uri with authorization code.

#### 2.5.6.4.0 Sequence Number

6

#### 2.5.6.5.0 Type

🔹 Redirect

#### 2.5.6.6.0 Is Synchronous

❌ No

#### 2.5.6.7.0 Return Message



#### 2.5.6.8.0 Has Return

❌ No

#### 2.5.6.9.0 Is Activation

❌ No

#### 2.5.6.10.0 Technical Details

##### 2.5.6.10.1 Protocol

HTTP/1.1 302 Found

##### 2.5.6.10.2 Method

Location Header

##### 2.5.6.10.3 Parameters

- URL: https://cmp.example.com/callback
- Query Param `code`: 'single-use-authorization-code'
- Query Param `state`: 'same-opaque-string-from-step-3'

##### 2.5.6.10.4 Authentication

N/A

##### 2.5.6.10.5 Error Handling

N/A

### 2.5.7.0.0 Request

#### 2.5.7.1.0 Source Id

UserBrowser

#### 2.5.7.2.0 Target Id

REPO-FE-MPL

#### 2.5.7.3.0 Message

7. GET /callback?code=...&state=...

#### 2.5.7.4.0 Sequence Number

7

#### 2.5.7.5.0 Type

🔹 Request

#### 2.5.7.6.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0 Return Message

Application SPA loaded

#### 2.5.7.8.0 Has Return

✅ Yes

#### 2.5.7.9.0 Is Activation

❌ No

#### 2.5.7.10.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | GET |
| Parameters | code, state |
| Authentication | N/A |
| Error Handling | If state parameter mismatch, abort flow and displa... |

### 2.5.8.0.0 Request

#### 2.5.8.1.0 Source Id

REPO-FE-MPL

#### 2.5.8.2.0 Target Id

REPO-GW-API

#### 2.5.8.3.0 Message

8. Exchange authorization code for tokens.

#### 2.5.8.4.0 Sequence Number

8

#### 2.5.8.5.0 Type

🔹 Request

#### 2.5.8.6.0 Is Synchronous

✅ Yes

#### 2.5.8.7.0 Return Message

JWT Payload { accessToken, ... }

#### 2.5.8.8.0 Has Return

✅ Yes

#### 2.5.8.9.0 Is Activation

❌ No

#### 2.5.8.10.0 Technical Details

##### 2.5.8.10.1 Protocol

HTTPS

##### 2.5.8.10.2 Method

POST /api/v1/auth/token

##### 2.5.8.10.3 Parameters

- Body `grant_type`: 'authorization_code'
- Body `code`: 'code-from-step-7'
- Body `redirect_uri`: 'https://cmp.example.com/callback'
- Body `code_verifier`: 'retrieved-from-sessionStorage'

##### 2.5.8.10.4 Authentication

N/A (public endpoint)

##### 2.5.8.10.5 Error Handling

If exchange fails (e.g., 400, 503), display user-friendly error message.

### 2.5.9.0.0 Proxy

#### 2.5.9.1.0 Source Id

REPO-GW-API

#### 2.5.9.2.0 Target Id

REPO-SVC-IAM

#### 2.5.9.3.0 Message

9. Route token exchange request to IAM service.

#### 2.5.9.4.0 Sequence Number

9

#### 2.5.9.5.0 Type

🔹 Proxy

#### 2.5.9.6.0 Is Synchronous

✅ Yes

#### 2.5.9.7.0 Return Message

JWT Payload

#### 2.5.9.8.0 Has Return

✅ Yes

#### 2.5.9.9.0 Is Activation

✅ Yes

#### 2.5.9.10.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 (Cluster Internal) |
| Method | POST /auth/token |
| Parameters | Forwarded from step 8. |
| Authentication | N/A |
| Error Handling | Gateway handles routing errors (e.g., service unav... |

### 2.5.10.0.0 Request

#### 2.5.10.1.0 Source Id

REPO-SVC-IAM

#### 2.5.10.2.0 Target Id

Keycloak

#### 2.5.10.3.0 Message

10. POST to Keycloak token endpoint (back-channel).

#### 2.5.10.4.0 Sequence Number

10

#### 2.5.10.5.0 Type

🔹 Request

#### 2.5.10.6.0 Is Synchronous

✅ Yes

#### 2.5.10.7.0 Return Message

{ id_token, access_token, refresh_token }

#### 2.5.10.8.0 Has Return

✅ Yes

#### 2.5.10.9.0 Is Activation

❌ No

#### 2.5.10.10.0 Technical Details

##### 2.5.10.10.1 Protocol

HTTPS

##### 2.5.10.10.2 Method

POST /auth/realms/{realm}/protocol/openid-connect/token

##### 2.5.10.10.3 Parameters

- Body `grant_type`: 'authorization_code'
- Body `code`: 'code-from-frontend'
- Body `redirect_uri`: 'https://cmp.example.com/callback'
- Body `code_verifier`: 'verifier-from-frontend'

##### 2.5.10.10.4 Authentication

Client Credentials (client_id + client_secret in Authorization header)

##### 2.5.10.10.5 Error Handling

On failure (invalid code, expired code, PKCE mismatch), logs detailed error and returns 401/400 to its caller.

### 2.5.11.0.0 InternalProcessing

#### 2.5.11.1.0 Source Id

Keycloak

#### 2.5.11.2.0 Target Id

Keycloak

#### 2.5.11.3.0 Message

11. Validate code, PKCE verifier, and client credentials. Issue tokens.

#### 2.5.11.4.0 Sequence Number

11

#### 2.5.11.5.0 Type

🔹 InternalProcessing

#### 2.5.11.6.0 Is Synchronous

✅ Yes

#### 2.5.11.7.0 Return Message

JWTs

#### 2.5.11.8.0 Has Return

✅ Yes

#### 2.5.11.9.0 Is Activation

❌ No

#### 2.5.11.10.0 Technical Details

##### 2.5.11.10.1 Protocol

N/A

##### 2.5.11.10.2 Method

Internal OIDC Token Issuance Logic

##### 2.5.11.10.3 Parameters

*No items available*

##### 2.5.11.10.4 Authentication

N/A

##### 2.5.11.10.5 Error Handling

Returns JSON error object as per OAuth 2.0 spec on any validation failure.

### 2.5.12.0.0 Response

#### 2.5.12.1.0 Source Id

Keycloak

#### 2.5.12.2.0 Target Id

REPO-SVC-IAM

#### 2.5.12.3.0 Message

12. Return tokens to IAM Service.

#### 2.5.12.4.0 Sequence Number

12

#### 2.5.12.5.0 Type

🔹 Response

#### 2.5.12.6.0 Is Synchronous

✅ Yes

#### 2.5.12.7.0 Return Message



#### 2.5.12.8.0 Has Return

❌ No

#### 2.5.12.9.0 Is Activation

❌ No

#### 2.5.12.10.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | 200 OK |
| Parameters | JSON body with tokens. |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.13.0.0 Audit

#### 2.5.13.1.0 Source Id

REPO-SVC-IAM

#### 2.5.13.2.0 Target Id

REPO-SVC-IAM

#### 2.5.13.3.0 Message

13. Log successful authentication event.

#### 2.5.13.4.0 Sequence Number

13

#### 2.5.13.5.0 Type

🔹 Audit

#### 2.5.13.6.0 Is Synchronous

✅ Yes

#### 2.5.13.7.0 Return Message



#### 2.5.13.8.0 Has Return

❌ No

#### 2.5.13.9.0 Is Activation

❌ No

#### 2.5.13.10.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Function Call |
| Method | auditLogger.logSuccess('user-login', { userId }) |
| Parameters | User ID from token claims, IP address. |
| Authentication | N/A |
| Error Handling | Logging failures are handled gracefully. |

### 2.5.14.0.0 Response

#### 2.5.14.1.0 Source Id

REPO-SVC-IAM

#### 2.5.14.2.0 Target Id

REPO-GW-API

#### 2.5.14.3.0 Message

14. Return tokens.

#### 2.5.14.4.0 Sequence Number

14

#### 2.5.14.5.0 Type

🔹 Response

#### 2.5.14.6.0 Is Synchronous

✅ Yes

#### 2.5.14.7.0 Return Message



#### 2.5.14.8.0 Has Return

❌ No

#### 2.5.14.9.0 Is Activation

❌ No

#### 2.5.14.10.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP/1.1 |
| Method | 200 OK |
| Parameters | JSON body containing accessToken, refreshToken, et... |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.15.0.0 Response

#### 2.5.15.1.0 Source Id

REPO-GW-API

#### 2.5.15.2.0 Target Id

REPO-FE-MPL

#### 2.5.15.3.0 Message

15. Forward tokens to frontend.

#### 2.5.15.4.0 Sequence Number

15

#### 2.5.15.5.0 Type

🔹 Response

#### 2.5.15.6.0 Is Synchronous

✅ Yes

#### 2.5.15.7.0 Return Message



#### 2.5.15.8.0 Has Return

❌ No

#### 2.5.15.9.0 Is Activation

❌ No

#### 2.5.15.10.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | 200 OK |
| Parameters | JSON Body |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.16.0.0 InternalProcessing

#### 2.5.16.1.0 Source Id

REPO-FE-MPL

#### 2.5.16.2.0 Target Id

REPO-FE-MPL

#### 2.5.16.3.0 Message

16. Securely store tokens and redirect to dashboard.

#### 2.5.16.4.0 Sequence Number

16

#### 2.5.16.5.0 Type

🔹 InternalProcessing

#### 2.5.16.6.0 Is Synchronous

✅ Yes

#### 2.5.16.7.0 Return Message



#### 2.5.16.8.0 Has Return

❌ No

#### 2.5.16.9.0 Is Activation

❌ No

#### 2.5.16.10.0 Technical Details

##### 2.5.16.10.1 Protocol

Internal Function Call

##### 2.5.16.10.2 Method

authContext.setTokens(tokens)

##### 2.5.16.10.3 Parameters

- Store tokens in-memory within the SPA's state management.
- Optionally use secure, HttpOnly cookies for refresh token.
- Redirect user to the application's main dashboard.

##### 2.5.16.10.4 Authentication

N/A

##### 2.5.16.10.5 Error Handling

N/A

## 2.6.0.0.0 Notes

### 2.6.1.0.0 Content

#### 2.6.1.1.0 Content

PKCE (Proof Key for Code Exchange) is critical for securing this flow in a public client like an SPA. The `code_verifier` must NEVER be sent in the initial redirect to Keycloak.

#### 2.6.1.2.0 Position

Top

#### 2.6.1.3.0 Participant Id

*Not specified*

#### 2.6.1.4.0 Sequence Number

2

### 2.6.2.0.0 Content

#### 2.6.2.1.0 Content

The token exchange (step 10) is a confidential, back-channel communication. The frontend SPA must never handle the client_secret.

#### 2.6.2.2.0 Position

Right

#### 2.6.2.3.0 Participant Id

REPO-SVC-IAM

#### 2.6.2.4.0 Sequence Number

10

### 2.6.3.0.0 Content

#### 2.6.3.1.0 Content

Tokens must be stored securely in the frontend. Using browser memory (e.g., React Context/Redux) is preferred over localStorage to mitigate XSS risks.

#### 2.6.3.2.0 Position

Left

#### 2.6.3.3.0 Participant Id

REPO-FE-MPL

#### 2.6.3.4.0 Sequence Number

16

## 2.7.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | 1. **PKCE Enforcement**: Keycloak client must be c... |
| Performance Targets | End-to-end authentication latency, from user login... |
| Error Handling Strategy | 1. **Invalid Credentials**: Keycloak handles this,... |
| Testing Considerations | 1. **E2E Tests**: Use Playwright to automate the f... |
| Monitoring Requirements | 1. **Audit Logs**: The IAM Service and Keycloak mu... |
| Deployment Considerations | 1. **Redirect URIs**: The `redirect_uri` must be e... |

