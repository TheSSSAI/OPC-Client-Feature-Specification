# 1 Overview

## 1.1 Diagram Id

SEQ-UJ-004

## 1.2 Name

User Configures Notification Preferences

## 1.3 Description

A user navigates to their profile settings and accesses the Notification Preferences page. They can enable or disable specific notification types (e.g., critical alarms, system updates, reports) and choose their preferred delivery channel (e.g., email, SMS, in-app) for each type.

## 1.4 Type

🔹 UserJourney

## 1.5 Purpose

To give users granular control over the notifications they receive, preventing notification fatigue and ensuring they only get alerts relevant to them through their preferred channels (REQ-FR-022).

## 1.6 Complexity

Low

## 1.7 Priority

🟡 Medium

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-IAM
- REPO-SVC-ANM

## 1.10 Key Interactions

- A user accesses their profile and clicks on the 'Notification Preferences' tab.
- The UI fetches and displays their current settings from the IAM Service.
- The user modifies the settings (e.g., deselects 'Email' for 'Reports', selects 'SMS' for 'Critical Alarms').
- The frontend sends the updated preference object to the IAM Service.
- The IAM Service validates and stores the user's new preferences in its database.
- When the Notification Service needs to send an alert, it first queries the user's preferences to determine where (or if) to send it.

## 1.11 Triggers

- A user wants to customize their notification settings to reduce noise or change delivery methods.

## 1.12 Outcomes

- The user's notification settings are successfully saved.
- Future notifications sent to that user will respect their new preferences, routing to the correct channels.

## 1.13 Business Rules

- Users must be able to select which types of notifications they receive (REQ-FR-022).
- Users must be able to select their preferred delivery channels for each notification type (REQ-FR-022).

## 1.14 Error Scenarios

- The system fails to save the updated preferences due to a database error.
- The Notification Service fails to retrieve the user's preferences and uses a default setting instead.

## 1.15 Integration Points

*No items available*

# 2.0 Details

## 2.1 Diagram Id

SEQ-UJ-004

## 2.2 Name

User Configures Notification Preferences

## 2.3 Description

This sequence details the complete user journey for configuring notification preferences as per REQ-FR-022. It covers the initial data fetch, UI state management, user input validation, and the final data persistence via a secure API call. It also includes the subsequent consumption of these preferences by the Alarm & Notification Service.

## 2.4 Participants

### 2.4.1 Frontend SPA

#### 2.4.1.1 Repository Id

REPO-FE-MPL

#### 2.4.1.2 Display Name

Central Management Plane UI

#### 2.4.1.3 Type

🔹 Frontend SPA

#### 2.4.1.4 Technology

React 18, TypeScript, Redux Toolkit, Axios

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #1E90FF |
| Stereotype | User Interface |

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
| Shape | boundary |
| Color | #FFD700 |
| Stereotype | Gateway |

### 2.4.3.0 Microservice

#### 2.4.3.1 Repository Id

REPO-SVC-IAM

#### 2.4.3.2 Display Name

IAM Service

#### 2.4.3.3 Type

🔹 Microservice

#### 2.4.3.4 Technology

.NET 8, ASP.NET Core, EF Core, PostgreSQL

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #32CD32 |
| Stereotype | Service |

### 2.4.4.0 Microservice

#### 2.4.4.1 Repository Id

REPO-SVC-ANM

#### 2.4.4.2 Display Name

Alarm & Notification Service

#### 2.4.4.3 Type

🔹 Microservice

#### 2.4.4.4 Technology

.NET 8, ASP.NET Core

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #9370DB |
| Stereotype | Service |

## 2.5.0.0 Interactions

### 2.5.1.0 User Interaction

#### 2.5.1.1 Source Id

REPO-FE-MPL

#### 2.5.1.2 Target Id

REPO-FE-MPL

#### 2.5.1.3 Message

User navigates to '/profile/notifications'. Component mounts and dispatches Redux action `fetchPreferences()`.

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 User Interaction

#### 2.5.1.6 Is Synchronous

✅ Yes

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

❌ No

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | React Lifecycle |
| Method | useEffect hook |
| Parameters | None |
| Authentication | Requires authenticated user session |
| Error Handling | N/A |
| Performance | Should trigger immediately on component mount. |

### 2.5.2.0 API Call

#### 2.5.2.1 Source Id

REPO-FE-MPL

#### 2.5.2.2 Target Id

REPO-GW-API

#### 2.5.2.3 Message

GET /api/v1/users/me/notification-preferences

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 API Call

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Return Message

200 OK w/ UserNotificationPreferencesDto | 401 Unauthorized | 404 Not Found

#### 2.5.2.8 Has Return

✅ Yes

#### 2.5.2.9 Is Activation

❌ No

#### 2.5.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/REST |
| Method | GET |
| Parameters | None |
| Authentication | JWT Bearer Token in 'Authorization' header |
| Error Handling | Axios interceptor handles 4xx/5xx responses, dispa... |
| Performance | P95 Latency < 200ms as per REQ-NFR-001 |

### 2.5.3.0 Proxy Request

#### 2.5.3.1 Source Id

REPO-GW-API

#### 2.5.3.2 Target Id

REPO-SVC-IAM

#### 2.5.3.3 Message

Route request to IAM service: GET /users/{userId}/notification-preferences

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Proxy Request

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

Forwards response from IAM Service

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

✅ Yes

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (internal cluster DNS) |
| Method | GET |
| Parameters | userId extracted from JWT subject claim |
| Authentication | JWT is validated; claims are forwarded in request ... |
| Error Handling | Forwards HTTP 5xx from upstream. |
| Performance | Adds < 10ms latency. |

### 2.5.4.0 Database Query

#### 2.5.4.1 Source Id

REPO-SVC-IAM

#### 2.5.4.2 Target Id

REPO-SVC-IAM

#### 2.5.4.3 Message

Retrieve user preferences from 'UserPreferences' table where userId matches.

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 Database Query

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

Returns UserPreferences entity or null.

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | SELECT |
| Parameters | userId |
| Authentication | Via connection string credentials from AWS Secrets... |
| Error Handling | Throws internal exception on DB connection failure... |
| Performance | Query time < 20ms. Index on userId column. |

### 2.5.5.0 API Response

#### 2.5.5.1 Source Id

REPO-SVC-IAM

#### 2.5.5.2 Target Id

REPO-GW-API

#### 2.5.5.3 Message

Return 200 OK with UserNotificationPreferencesDto

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 API Response

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

JSON payload: { preferences: [{type: 'CRITICAL_ALARM', channels: ['EMAIL', 'SMS']}, ...] }

#### 2.5.5.8 Has Return

❌ No

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP |
| Method | 200 OK |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Serialization < 5ms |

### 2.5.6.0 State Update

#### 2.5.6.1 Source Id

REPO-FE-MPL

#### 2.5.6.2 Target Id

REPO-FE-MPL

#### 2.5.6.3 Message

Receives data, updates Redux store, and renders preference form with current settings.

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 State Update

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
| Protocol | Redux |
| Method | dispatch(fetchPreferencesSuccess(data)) |
| Parameters | UserNotificationPreferencesDto |
| Authentication | N/A |
| Error Handling | UI displays loading spinner until data arrives or ... |
| Performance | React re-render should be optimized to avoid layou... |

### 2.5.7.0 User Interaction

#### 2.5.7.1 Source Id

REPO-FE-MPL

#### 2.5.7.2 Target Id

REPO-FE-MPL

#### 2.5.7.3 Message

User modifies settings and clicks 'Save'. `onSubmit` handler is triggered.

#### 2.5.7.4 Sequence Number

7

#### 2.5.7.5 Type

🔹 User Interaction

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
| Protocol | DOM Event |
| Method | onClick |
| Parameters | Event object |
| Authentication | N/A |
| Error Handling | Form validation prevents submission if data is inv... |
| Performance | Immediate UI feedback. |

### 2.5.8.0 API Call

#### 2.5.8.1 Source Id

REPO-FE-MPL

#### 2.5.8.2 Target Id

REPO-GW-API

#### 2.5.8.3 Message

PUT /api/v1/users/me/notification-preferences

#### 2.5.8.4 Sequence Number

8

#### 2.5.8.5 Type

🔹 API Call

#### 2.5.8.6 Is Synchronous

✅ Yes

#### 2.5.8.7 Return Message

200 OK | 400 Bad Request | 401 Unauthorized

#### 2.5.8.8 Has Return

✅ Yes

#### 2.5.8.9 Is Activation

❌ No

#### 2.5.8.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTPS/REST |
| Method | PUT |
| Parameters | Request Body: UserNotificationPreferencesDto |
| Authentication | JWT Bearer Token in 'Authorization' header |
| Error Handling | Displays error message from server if validation f... |
| Performance | P95 Latency < 200ms. |

### 2.5.9.0 Proxy Request

#### 2.5.9.1 Source Id

REPO-GW-API

#### 2.5.9.2 Target Id

REPO-SVC-IAM

#### 2.5.9.3 Message

Route request to IAM service: PUT /users/{userId}/notification-preferences

#### 2.5.9.4 Sequence Number

9

#### 2.5.9.5 Type

🔹 Proxy Request

#### 2.5.9.6 Is Synchronous

✅ Yes

#### 2.5.9.7 Return Message

Forwards response from IAM Service

#### 2.5.9.8 Has Return

✅ Yes

#### 2.5.9.9 Is Activation

❌ No

#### 2.5.9.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP (internal cluster DNS) |
| Method | PUT |
| Parameters | userId from JWT claim, body from original request. |
| Authentication | JWT validated. |
| Error Handling | Forwards 4xx/5xx codes from upstream. |
| Performance | Adds < 10ms latency. |

### 2.5.10.0 Database Transaction

#### 2.5.10.1 Source Id

REPO-SVC-IAM

#### 2.5.10.2 Target Id

REPO-SVC-IAM

#### 2.5.10.3 Message

Validate DTO and persist (UPSERT) changes into 'UserPreferences' table.

#### 2.5.10.4 Sequence Number

10

#### 2.5.10.5 Type

🔹 Database Transaction

#### 2.5.10.6 Is Synchronous

✅ Yes

#### 2.5.10.7 Return Message

Success or Failure

#### 2.5.10.8 Has Return

✅ Yes

#### 2.5.10.9 Is Activation

❌ No

#### 2.5.10.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | SQL |
| Method | UPDATE/INSERT |
| Parameters | UserPreferences entity |
| Authentication | Via connection string credentials. |
| Error Handling | Rollbacks transaction on failure. Returns 400 if D... |
| Performance | Transaction time < 50ms. |

### 2.5.11.0 API Response

#### 2.5.11.1 Source Id

REPO-SVC-IAM

#### 2.5.11.2 Target Id

REPO-GW-API

#### 2.5.11.3 Message

Return 200 OK

#### 2.5.11.4 Sequence Number

11

#### 2.5.11.5 Type

🔹 API Response

#### 2.5.11.6 Is Synchronous

✅ Yes

#### 2.5.11.7 Return Message

Empty body

#### 2.5.11.8 Has Return

❌ No

#### 2.5.11.9 Is Activation

❌ No

#### 2.5.11.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP |
| Method | 200 OK |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | N/A |

### 2.5.12.0 UI Feedback

#### 2.5.12.1 Source Id

REPO-FE-MPL

#### 2.5.12.2 Target Id

REPO-FE-MPL

#### 2.5.12.3 Message

Displays a success toast: 'Preferences saved successfully'.

#### 2.5.12.4 Sequence Number

12

#### 2.5.12.5 Type

🔹 UI Feedback

#### 2.5.12.6 Is Synchronous

❌ No

#### 2.5.12.7 Return Message



#### 2.5.12.8 Has Return

❌ No

#### 2.5.12.9 Is Activation

❌ No

#### 2.5.12.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | UI Framework |
| Method | toast.success() |
| Parameters | Message string |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Toast appears immediately and auto-dismisses after... |

### 2.5.13.0 Internal Service Call

#### 2.5.13.1 Source Id

REPO-SVC-ANM

#### 2.5.13.2 Target Id

REPO-SVC-IAM

#### 2.5.13.3 Message

GetUserNotificationPreferences(userId)

#### 2.5.13.4 Sequence Number

13

#### 2.5.13.5 Type

🔹 Internal Service Call

#### 2.5.13.6 Is Synchronous

✅ Yes

#### 2.5.13.7 Return Message

UserNotificationPreferencesResponse | NotFoundError

#### 2.5.13.8 Has Return

✅ Yes

#### 2.5.13.9 Is Activation

❌ No

#### 2.5.13.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | gRPC |
| Method | GetUserNotificationPreferences |
| Parameters | Request: { userId: '...' } |
| Authentication | Internal service-to-service auth (mTLS or JWT) |
| Error Handling | Retry on transient errors. Fallback to default not... |
| Performance | Internal latency < 20ms. IAM Service should cache ... |

### 2.5.14.0 Business Logic

#### 2.5.14.1 Source Id

REPO-SVC-ANM

#### 2.5.14.2 Target Id

REPO-SVC-ANM

#### 2.5.14.3 Message

Use preferences to route notification to correct channels (e.g., Email, SMS).

#### 2.5.14.4 Sequence Number

14

#### 2.5.14.5 Type

🔹 Business Logic

#### 2.5.14.6 Is Synchronous

✅ Yes

#### 2.5.14.7 Return Message



#### 2.5.14.8 Has Return

❌ No

#### 2.5.14.9 Is Activation

❌ No

#### 2.5.14.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | N/A |
| Method | RouteNotification |
| Parameters | Notification payload, User preferences |
| Authentication | N/A |
| Error Handling | Logs an error if a selected channel fails (e.g., S... |
| Performance | Logic should execute in < 5ms. |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Interactions 1-12 represent the primary user journey of configuring preferences. This is the core scope of the user story.

#### 2.6.1.2 Position

top

#### 2.6.1.3 Participant Id

*Not specified*

#### 2.6.1.4 Sequence Number

1

### 2.6.2.0 Content

#### 2.6.2.1 Content

Interactions 13-14 demonstrate the consumption of the configured data by another service, fulfilling the purpose of the feature. This flow happens asynchronously at a later time.

#### 2.6.2.2 Position

bottom

#### 2.6.2.3 Participant Id

*Not specified*

#### 2.6.2.4 Sequence Number

13

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | The frontend must sanitize all user-facing data to... |
| Performance Targets | The initial load of the preferences page should fe... |
| Error Handling Strategy | The frontend must handle API errors gracefully, di... |
| Testing Considerations | E2E tests (Playwright) must cover the full journey... |
| Monitoring Requirements | The `/api/v1/users/me/notification-preferences` en... |
| Deployment Considerations | The React application is a static build deployed t... |

