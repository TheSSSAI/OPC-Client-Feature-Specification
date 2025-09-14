# 1 Overview

## 1.1 Diagram Id

SEQ-UJ-005

## 1.2 Name

Engineer Configures OPC Server Redundancy

## 1.3 Description

An Engineer uses the Central Management Plane UI to define a redundant OPC server pair. They specify the primary server, the backup server, and the health check parameters and failover trigger conditions that the OPC Core Client will use.

## 1.4 Type

🔹 UserJourney

## 1.5 Purpose

To enable the configuration of high-availability data sources at the edge, as required by REQ-FR-011.

## 1.6 Complexity

Medium

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-DVM

## 1.10 Key Interactions

- Engineer navigates to the OPC Client configuration page.
- They select an option to create or modify a redundant server pair.
- The UI provides fields to input primary/backup server endpoints and failover conditions (e.g., connection loss, status code).
- On save, the frontend sends the configuration to the Device Management Service.
- The service validates and persists the redundancy configuration, which will be pushed to the client.

## 1.11 Triggers

- A new high-availability data source needs to be configured.

## 1.12 Outcomes

- The OPC Core Client's configuration is updated with the server redundancy settings.
- The client is now capable of performing an automatic failover as described in SEQ-EH-001.

## 1.13 Business Rules

- The interface must allow configuration of primary/backup pairs and failover triggers (REQ-FR-011).

## 1.14 Error Scenarios

- Invalid server endpoints are provided.
- The configuration fails validation (e.g., primary and backup servers are the same).

## 1.15 Integration Points

*No items available*

# 2.0 Details

## 2.1 Diagram Id

SEQ-UI-001

## 2.2 Name

User Accesses Context-Sensitive Help Component

## 2.3 Description

A technical sequence diagram illustrating how a system user interacts with a reusable, accessible, and theme-aware help component within the React SPA. This covers the component's lifecycle from mounting and fetching localized content to displaying a popover on user interaction, and handling graceful degradation when content is unavailable.

## 2.4 Participants

### 2.4.1 Actor

#### 2.4.1.1 Repository Id

User

#### 2.4.1.2 Display Name

System User

#### 2.4.1.3 Type

🔹 Actor

#### 2.4.1.4 Technology

Human

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #E6E6E6 |
| Stereotype | <<Actor>> |

### 2.4.2.0 FrontendApplication

#### 2.4.2.1 Repository Id

REPO-FE-MPL

#### 2.4.2.2 Display Name

Browser (React SPA)

#### 2.4.2.3 Type

🔹 FrontendApplication

#### 2.4.2.4 Technology

React 18, TypeScript, Vite

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | boundary |
| Color | #82CFFD |
| Stereotype | <<Frontend>> |

### 2.4.3.0 UIComponent

#### 2.4.3.1 Repository Id

HelpComponent

#### 2.4.3.2 Display Name

Help Component

#### 2.4.3.3 Type

🔹 UIComponent

#### 2.4.3.4 Technology

React Component (Material-UI)

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #B4E4D8 |
| Stereotype | <<Component>> |

### 2.4.4.0 ClientSideService

#### 2.4.4.1 Repository Id

I18nService

#### 2.4.4.2 Display Name

i18n Service

#### 2.4.4.3 Type

🔹 ClientSideService

#### 2.4.4.4 Technology

i18next / react-i18next

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #F4D03F |
| Stereotype | <<Service>> |

## 2.5.0.0 Interactions

### 2.5.1.0 UserInteraction

#### 2.5.1.1 Source Id

User

#### 2.5.1.2 Target Id

REPO-FE-MPL

#### 2.5.1.3 Message

1. Navigates to a configuration page

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

| Property | Value |
|----------|-------|
| Protocol | HTTPS |
| Method | GET |
| Parameters | URL: /app/config/client/123 |
| Authentication | OIDC Session Cookie |
| Error Handling | Standard browser error pages (404, 500). |
| Performance | Page load time < 3s as per REQ-NFR-001. |

### 2.5.2.0 ComponentLifecycle

#### 2.5.2.1 Source Id

REPO-FE-MPL

#### 2.5.2.2 Target Id

HelpComponent

#### 2.5.2.3 Message

2. Renders page and mounts HelpComponent instance

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 ComponentLifecycle

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
| Protocol | Internal Function Call |
| Method | React.createElement |
| Parameters | Props: { helpKey: 'config.failover.condition.help'... |
| Authentication | N/A |
| Error Handling | React Error Boundary catches rendering errors. |
| Performance | Component mount should be negligible. |

### 2.5.3.0 DataRequest

#### 2.5.3.1 Source Id

HelpComponent

#### 2.5.3.2 Target Id

I18nService

#### 2.5.3.3 Message

3. Requests localized help text using key

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 DataRequest

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

Returns translated string or fallback.

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

❌ No

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Function Call |
| Method | i18n.t('config.failover.condition.help') |
| Parameters | Key: 'config.failover.condition.help' |
| Authentication | N/A |
| Error Handling | If key is not found, the service returns the key s... |
| Performance | Lookup from in-memory JSON object, latency < 1ms. |

#### 2.5.3.11 Nested Interactions

##### 2.5.3.11.1 InternalLogic

###### 2.5.3.11.1.1 Source Id

I18nService

###### 2.5.3.11.1.2 Target Id

I18nService

###### 2.5.3.11.1.3 Message

3a. Looks up translation for current language (e.g., 'de')

###### 2.5.3.11.1.4 Sequence Number

3.1

###### 2.5.3.11.1.5 Type

🔹 InternalLogic

###### 2.5.3.11.1.6 Is Synchronous

✅ Yes

###### 2.5.3.11.1.7 Return Message



###### 2.5.3.11.1.8 Has Return

❌ No

###### 2.5.3.11.1.9 Is Activation

❌ No

###### 2.5.3.11.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Logic |
| Method | lookup(key, 'de') |
| Parameters | *N/A* |
| Authentication | N/A |
| Error Handling | If key is missing for 'de', triggers fallback logi... |
| Performance | Sub-millisecond. |

##### 2.5.3.11.2.0 InternalLogic

###### 2.5.3.11.2.1 Source Id

I18nService

###### 2.5.3.11.2.2 Target Id

I18nService

###### 2.5.3.11.2.3 Message

3b. [Alt] Falls back to default language ('en') if translation is missing

###### 2.5.3.11.2.4 Sequence Number

3.2

###### 2.5.3.11.2.5 Type

🔹 InternalLogic

###### 2.5.3.11.2.6 Is Synchronous

✅ Yes

###### 2.5.3.11.2.7 Return Message



###### 2.5.3.11.2.8 Has Return

❌ No

###### 2.5.3.11.2.9 Is Activation

❌ No

###### 2.5.3.11.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Logic |
| Method | lookup(key, 'en') |
| Parameters | *N/A* |
| Authentication | N/A |
| Error Handling | If key is missing in fallback language, return key... |
| Performance | Sub-millisecond. |

### 2.5.4.0.0.0 StateUpdate

#### 2.5.4.1.0.0 Source Id

HelpComponent

#### 2.5.4.2.0.0 Target Id

HelpComponent

#### 2.5.4.3.0.0 Message

4. Caches localized text in component state and renders help icon

#### 2.5.4.4.0.0 Sequence Number

4

#### 2.5.4.5.0.0 Type

🔹 StateUpdate

#### 2.5.4.6.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0 Return Message



#### 2.5.4.8.0.0 Has Return

❌ No

#### 2.5.4.9.0.0 Is Activation

❌ No

#### 2.5.4.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal State Management |
| Method | React.useState |
| Parameters | State: { helpText: '...' } |
| Authentication | N/A |
| Error Handling | [Edge Case] If helpText is null/empty, the compone... |
| Performance | Negligible. |

### 2.5.5.0.0.0 UserInteraction

#### 2.5.5.1.0.0 Source Id

User

#### 2.5.5.2.0.0 Target Id

HelpComponent

#### 2.5.5.3.0.0 Message

5. Activates help icon (Click or Keyboard 'Enter'/'Space')

#### 2.5.5.4.0.0 Sequence Number

5

#### 2.5.5.5.0.0 Type

🔹 UserInteraction

#### 2.5.5.6.0.0 Is Synchronous

✅ Yes

#### 2.5.5.7.0.0 Return Message



#### 2.5.5.8.0.0 Has Return

❌ No

#### 2.5.5.9.0.0 Is Activation

❌ No

#### 2.5.5.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | DOM Event |
| Method | onClick / onKeyDown |
| Parameters | Event object |
| Authentication | N/A |
| Error Handling | Event bubbling is managed to prevent unintended si... |
| Performance | UI response must be immediate (<100ms). |

### 2.5.6.0.0.0 StateUpdate

#### 2.5.6.1.0.0 Source Id

HelpComponent

#### 2.5.6.2.0.0 Target Id

HelpComponent

#### 2.5.6.3.0.0 Message

6. Toggles visibility state and re-renders with popover

#### 2.5.6.4.0.0 Sequence Number

6

#### 2.5.6.5.0.0 Type

🔹 StateUpdate

#### 2.5.6.6.0.0 Is Synchronous

✅ Yes

#### 2.5.6.7.0.0 Return Message



#### 2.5.6.8.0.0 Has Return

❌ No

#### 2.5.6.9.0.0 Is Activation

❌ No

#### 2.5.6.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal State Management |
| Method | setState({ isVisible: true }) |
| Parameters | *N/A* |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Re-render should be optimized via React memoizatio... |

### 2.5.7.0.0.0 UIRender

#### 2.5.7.1.0.0 Source Id

HelpComponent

#### 2.5.7.2.0.0 Target Id

REPO-FE-MPL

#### 2.5.7.3.0.0 Message

7. Renders accessible popover adjacent to icon

#### 2.5.7.4.0.0 Sequence Number

7

#### 2.5.7.5.0.0 Type

🔹 UIRender

#### 2.5.7.6.0.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0.0 Return Message

Displays popover to user.

#### 2.5.7.8.0.0 Has Return

✅ Yes

#### 2.5.7.9.0.0 Is Activation

❌ No

#### 2.5.7.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | DOM Manipulation |
| Method | React DOM update |
| Parameters | JSX for popover with ARIA attributes (e.g., role='... |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Rendering must be performant, styles should adapt ... |

### 2.5.8.0.0.0 UserInteraction

#### 2.5.8.1.0.0 Source Id

User

#### 2.5.8.2.0.0 Target Id

REPO-FE-MPL

#### 2.5.8.3.0.0 Message

8. Closes popover ('Escape' key or clicking outside)

#### 2.5.8.4.0.0 Sequence Number

8

#### 2.5.8.5.0.0 Type

🔹 UserInteraction

#### 2.5.8.6.0.0 Is Synchronous

✅ Yes

#### 2.5.8.7.0.0 Return Message



#### 2.5.8.8.0.0 Has Return

❌ No

#### 2.5.8.9.0.0 Is Activation

❌ No

#### 2.5.8.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | DOM Event |
| Method | onKeyDown / onBlur |
| Parameters | Event object |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate UI feedback. |

### 2.5.9.0.0.0 EventHandling

#### 2.5.9.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.9.2.0.0 Target Id

HelpComponent

#### 2.5.9.3.0.0 Message

9. Triggers component's onClose handler

#### 2.5.9.4.0.0 Sequence Number

9

#### 2.5.9.5.0.0 Type

🔹 EventHandling

#### 2.5.9.6.0.0 Is Synchronous

✅ Yes

#### 2.5.9.7.0.0 Return Message



#### 2.5.9.8.0.0 Has Return

❌ No

#### 2.5.9.9.0.0 Is Activation

❌ No

#### 2.5.9.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Callback |
| Method | handleClose() |
| Parameters | *N/A* |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate. |

### 2.5.10.0.0.0 StateUpdate

#### 2.5.10.1.0.0 Source Id

HelpComponent

#### 2.5.10.2.0.0 Target Id

HelpComponent

#### 2.5.10.3.0.0 Message

10. Hides popover by updating state

#### 2.5.10.4.0.0 Sequence Number

10

#### 2.5.10.5.0.0 Type

🔹 StateUpdate

#### 2.5.10.6.0.0 Is Synchronous

✅ Yes

#### 2.5.10.7.0.0 Return Message



#### 2.5.10.8.0.0 Has Return

❌ No

#### 2.5.10.9.0.0 Is Activation

❌ No

#### 2.5.10.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal State Management |
| Method | setState({ isVisible: false }) |
| Parameters | *N/A* |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate. |

## 2.6.0.0.0.0 Notes

### 2.6.1.0.0.0 Content

#### 2.6.1.1.0.0 Content

The HelpComponent is designed as a reusable, high-order component or hook that can wrap other UI elements to provide tooltips or be used standalone for icon-based popovers.

#### 2.6.1.2.0.0 Position

top-right

#### 2.6.1.3.0.0 Participant Id

HelpComponent

#### 2.6.1.4.0.0 Sequence Number

2

### 2.6.2.0.0.0 Content

#### 2.6.2.1.0.0 Content

Accessibility (WCAG 2.1 AA) is critical. The component must manage focus, use correct ARIA roles, and be fully operable via keyboard.

#### 2.6.2.2.0.0 Position

bottom-right

#### 2.6.2.3.0.0 Participant Id

HelpComponent

#### 2.6.2.4.0.0 Sequence Number

7

## 2.7.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | All help text content fetched from i18n resource f... |
| Performance Targets | UI interaction latency for showing/hiding the popo... |
| Error Handling Strategy | The component must handle missing translation keys... |
| Testing Considerations | Unit tests (Vitest/RTL) must cover state changes, ... |
| Monitoring Requirements | User interaction with help icons can be tracked as... |
| Deployment Considerations | Localized help text content is bundled with the fr... |

