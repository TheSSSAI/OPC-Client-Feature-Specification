# 1 Overview

## 1.1 Diagram Id

SEQ-UJ-006

## 1.2 Name

Administrator Manages User Roles and Permissions

## 1.3 Description

An Administrator uses the user management interface to assign a role (e.g., 'Operator') to a specific user. The system provides options to scope the role to a particular part of the asset hierarchy (e.g., 'Operator for Line 1 only'), providing granular access control.

## 1.4 Type

🔹 UserJourney

## 1.5 Purpose

To allow administrators to implement the principle of least privilege by configuring the RBAC system as per REQ-BIZ-001.

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

- Admin selects a user in the UI.
- They are presented with a list of available roles.
- They assign the 'Operator' role and are prompted to optionally select an asset scope (e.g., 'Site A / Area 2').
- The frontend sends the role mapping (user ID, role ID, scope ID) to the IAM service.
- The IAM service persists this mapping and, if necessary, updates the user's group/role membership in Keycloak.

## 1.11 Triggers

- A user's responsibilities change, requiring different system permissions.

## 1.12 Outcomes

- The user's permissions are updated.
- The user's JWT will contain the new role/scope information upon next login.

## 1.13 Business Rules

- The RBAC system must allow for granular permission settings for specific plant areas (REQ-BIZ-001).

## 1.14 Error Scenarios

- The administrator attempts to assign a non-existent role.
- The asset scope provided is invalid.

## 1.15 Integration Points

- Keycloak

# 2.0 Details

## 2.1 Diagram Id

SEQ-UJ-064

## 2.2 Name

User Accesses Context-Sensitive UI Help

## 2.3 Description

A comprehensive technical sequence diagram detailing the user journey for accessing contextual help within the UI. This sequence covers the click-to-display popover interaction, including component state management, integration with the internationalization service for localized content, and the necessary steps for ensuring accessibility and responsiveness as per system requirements.

## 2.4 Participants

### 2.4.1 Actor

#### 2.4.1.1 Repository Id

User

#### 2.4.1.2 Display Name

User

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
| Color | #999999 |
| Stereotype | Human |

### 2.4.2.0 Frontend Environment

#### 2.4.2.1 Repository Id

REPO-FE-MPL

#### 2.4.2.2 Display Name

Browser/DOM

#### 2.4.2.3 Type

🔹 Frontend Environment

#### 2.4.2.4 Technology

Chrome, Firefox, Edge, Safari

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | boundary |
| Color | #4285F4 |
| Stereotype | Browser |

### 2.4.3.0 React Component

#### 2.4.3.1 Repository Id

REPO-FE-MPL

#### 2.4.3.2 Display Name

Help Component

#### 2.4.3.3 Type

🔹 React Component

#### 2.4.3.4 Technology

React 18, Material-UI

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #00ACC1 |
| Stereotype | UI Component |

### 2.4.4.0 Frontend Service

#### 2.4.4.1 Repository Id

REPO-FE-MPL

#### 2.4.4.2 Display Name

I18n Service

#### 2.4.4.3 Type

🔹 Frontend Service

#### 2.4.4.4 Technology

i18next/react-i18next

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #FBBC05 |
| Stereotype | Localization Store |

## 2.5.0.0 Interactions

### 2.5.1.0 User Interaction

#### 2.5.1.1 Source Id

User

#### 2.5.1.2 Target Id

REPO-FE-MPL

#### 2.5.1.3 Message

Clicks on help icon (?) next to a configuration field.

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
| Protocol | DOM Event |
| Method | click |
| Parameters | MouseEvent |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate visual feedback (e.g., ripple effect) < ... |

#### 2.5.1.11 Nested Interactions

*No items available*

### 2.5.2.0 Event Handling

#### 2.5.2.1 Source Id

REPO-FE-MPL

#### 2.5.2.2 Target Id

REPO-FE-MPL

#### 2.5.2.3 Message

Triggers onClick event handler bound to the Help Component.

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Event Handling

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
| Protocol | React SyntheticEvent |
| Method | handleIconClick(event) |
| Parameters | The event object. |
| Authentication | N/A |
| Error Handling | event.stopPropagation() to prevent unintended pare... |
| Performance | Handler execution should be trivial (< 1ms). |

### 2.5.3.0 State Management

#### 2.5.3.1 Source Id

REPO-FE-MPL

#### 2.5.3.2 Target Id

REPO-FE-MPL

#### 2.5.3.3 Message

Updates internal state to show the popover: setState({ isVisible: true, anchorEl: event.currentTarget }).

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 State Management

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message



#### 2.5.3.8 Has Return

❌ No

#### 2.5.3.9 Is Activation

❌ No

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | React State Hook |
| Method | useState setter |
| Parameters | New state object containing visibility flag and an... |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Triggers an asynchronous re-render. |

### 2.5.4.0 Data Fetch

#### 2.5.4.1 Source Id

REPO-FE-MPL

#### 2.5.4.2 Target Id

REPO-FE-MPL

#### 2.5.4.3 Message

[Render] Fetch localized help text for the corresponding help key.

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 Data Fetch

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

Returns localized string.

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

✅ Yes

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Function Call |
| Method | t('help.failoverTriggerCondition') |
| Parameters | A unique key string for the help content. |
| Authentication | N/A |
| Error Handling | If key is not found for the current language, the ... |
| Performance | Synchronous lookup from in-memory JSON object, ver... |

### 2.5.5.0 UI Render

#### 2.5.5.1 Source Id

REPO-FE-MPL

#### 2.5.5.2 Target Id

REPO-FE-MPL

#### 2.5.5.3 Message

[Render] Renders Material-UI Popover component with fetched content.

#### 2.5.5.4 Sequence Number

5

#### 2.5.5.5 Type

🔹 UI Render

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message

Updates virtual DOM.

#### 2.5.5.8 Has Return

✅ Yes

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | React Render |
| Method | JSX Transformation |
| Parameters | Props including `open={isVisible}`, `anchorEl`, `o... |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Component rendering should complete within a singl... |

### 2.5.6.0 DOM Update

#### 2.5.6.1 Source Id

REPO-FE-MPL

#### 2.5.6.2 Target Id

REPO-FE-MPL

#### 2.5.6.3 Message

Commits changes to the actual DOM, displaying the popover.

#### 2.5.6.4 Sequence Number

6

#### 2.5.6.5 Type

🔹 DOM Update

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
| Protocol | Browser API |
| Method | DOM Manipulation |
| Parameters | DOM nodes representing the popover. |
| Authentication | N/A |
| Error Handling | The browser handles rendering errors. |
| Performance | Total time from click to popover visible should be... |

### 2.5.7.0 User Interaction

#### 2.5.7.1 Source Id

User

#### 2.5.7.2 Target Id

REPO-FE-MPL

#### 2.5.7.3 Message

Clicks anywhere outside the popover element.

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
| Method | click |
| Parameters | MouseEvent |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | N/A |

### 2.5.8.0 Event Handling

#### 2.5.8.1 Source Id

REPO-FE-MPL

#### 2.5.8.2 Target Id

REPO-FE-MPL

#### 2.5.8.3 Message

Triggers Popover's onClose event handler.

#### 2.5.8.4 Sequence Number

8

#### 2.5.8.5 Type

🔹 Event Handling

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
| Protocol | React Prop Callback |
| Method | handlePopoverClose() |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Handler execution should be trivial (< 1ms). |

### 2.5.9.0 State Management

#### 2.5.9.1 Source Id

REPO-FE-MPL

#### 2.5.9.2 Target Id

REPO-FE-MPL

#### 2.5.9.3 Message

Updates internal state to hide popover: setState({ isVisible: false, anchorEl: null }).

#### 2.5.9.4 Sequence Number

9

#### 2.5.9.5 Type

🔹 State Management

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
| Protocol | React State Hook |
| Method | useState setter |
| Parameters | New state object with visibility set to false. |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Triggers an asynchronous re-render to remove the p... |

### 2.5.10.0 DOM Update

#### 2.5.10.1 Source Id

REPO-FE-MPL

#### 2.5.10.2 Target Id

REPO-FE-MPL

#### 2.5.10.3 Message

Commits changes to DOM, removing the popover element.

#### 2.5.10.4 Sequence Number

10

#### 2.5.10.5 Type

🔹 DOM Update

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
| Protocol | Browser API |
| Method | DOM Manipulation |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | UI should respond instantly to the click. |

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Accessibility (AC-005): The help icon must be focusable via keyboard ('Tab' key). The popover must be triggered by 'Enter' or 'Space' keys. The 'Escape' key must close the popover. The icon must have an `aria-label` like 'Help for Failover Trigger Condition'.

#### 2.6.1.2 Position

bottom

#### 2.6.1.3 Participant Id

REPO-FE-MPL

#### 2.6.1.4 Sequence Number

1

### 2.6.2.0 Content

#### 2.6.2.1 Content

Responsiveness (AC-004): The Popover component's positioning logic must be configured to prevent it from rendering outside the viewport, especially on smaller screens as defined in REQ-IFC-001.

#### 2.6.2.2 Position

top

#### 2.6.2.3 Participant Id

REPO-FE-MPL

#### 2.6.2.4 Sequence Number

6

### 2.6.3.0 Content

#### 2.6.3.1 Content

Tooltip Variant (AC-002): For simpler help text on elements like icon-only buttons, a similar flow occurs using `onMouseEnter` and `onMouseLeave` events to toggle a Material-UI Tooltip component after a 500ms delay. The core logic of fetching localized content remains the same.

#### 2.6.3.2 Position

bottom

#### 2.6.3.3 Participant Id

User

#### 2.6.3.4 Sequence Number

1

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | To prevent Cross-Site Scripting (XSS) vulnerabilit... |
| Performance Targets | The end-to-end latency from user interaction (clic... |
| Error Handling Strategy | If the I18n Service cannot find a key for the curr... |
| Testing Considerations | Unit tests (Vitest/RTL) must cover state changes, ... |
| Monitoring Requirements | User interaction with help icons can be tracked as... |
| Deployment Considerations | The Help Component should be developed as a generi... |

