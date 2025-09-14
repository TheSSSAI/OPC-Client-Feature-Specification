# 1 Overview

## 1.1 Diagram Id

SEQ-UJ-001

## 1.2 Name

Operator Acknowledges an Alarm

## 1.3 Description

An Operator views an active alarm in the web UI's alarm console. They select the alarm and click the 'Acknowledge' button, providing a comment if required. The system updates the alarm's state and records the action in the immutable audit trail.

## 1.4 Type

🔹 UserJourney

## 1.5 Purpose

To allow operations personnel to manage the lifecycle of alarms and events according to the OPC A&C specification, and to create an auditable record of their actions for compliance and analysis.

## 1.6 Complexity

Low

## 1.7 Priority

🔴 High

## 1.8 Frequency

Daily

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-ANM
- REPO-SVC-ADT

## 1.10 Key Interactions

- The frontend displays a list of active alarms received from the Alarm Service.
- The Operator clicks 'Acknowledge' for a specific alarm.
- The frontend sends a secure API request to the Alarm Service to update the alarm state.
- The Alarm Service updates its database and asynchronously calls the Audit Service.
- The Audit Service creates a new, immutable entry for the acknowledgement action, including the operator's ID, timestamp, and alarm details.

## 1.11 Triggers

- An alarm condition is active in the system and visible to an Operator with the appropriate permissions.

## 1.12 Outcomes

- The alarm's state is changed to 'Acknowledged' in the system.
- The alarm console UI is updated in real-time to reflect the new state.
- A detailed audit log is created for the action (REQ-FR-005).

## 1.13 Business Rules

- Only users with Operator, Engineer, or Administrator roles can acknowledge alarms (REQ-USR-001).
- All acknowledgement actions must be logged in the tamper-evident audit trail.

## 1.14 Error Scenarios

- The alarm was already acknowledged by another user (optimistic concurrency failure).
- The user lacks the required permissions for the asset associated with the alarm.
- The audit service is unavailable, potentially failing the operation depending on system configuration.

## 1.15 Integration Points

*No items available*

# 2.0 Details

## 2.1 Diagram Id

SEQ-UJ-064

## 2.2 Name

User Accesses Context-Sensitive Help

## 2.3 Description

A comprehensive technical sequence detailing how a user interacts with a context-sensitive help component within the frontend application. The sequence covers the user click, state management, retrieval of localized content from the i18n provider, rendering of an accessible popover, and dismissal of the popover. This implementation must be a reusable, theme-aware, and accessible React component.

## 2.4 Participants

### 2.4.1 Actor

#### 2.4.1.1 Repository Id

user

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
| Color | #999999 |
| Stereotype | User |

### 2.4.2.0 System

#### 2.4.2.1 Repository Id

browser

#### 2.4.2.2 Display Name

Web Browser

#### 2.4.2.3 Type

🔹 System

#### 2.4.2.4 Technology

Chrome, Firefox, Edge, Safari

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #4285F4 |
| Stereotype | Environment |

### 2.4.3.0 Frontend Application

#### 2.4.3.1 Repository Id

REPO-FE-MPL

#### 2.4.3.2 Display Name

Central Management Plane UI

#### 2.4.3.3 Type

🔹 Frontend Application

#### 2.4.3.4 Technology

React 18, TypeScript, Material-UI

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #61DAFB |
| Stereotype | React SPA |

## 2.5.0.0 Interactions

### 2.5.1.0 User Input

#### 2.5.1.1 Source Id

user

#### 2.5.1.2 Target Id

browser

#### 2.5.1.3 Message

1. Clicks the help icon ('?' button) next to a configuration field.

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 User Input

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
| Protocol | UI Event |
| Method | click |
| Parameters | PointerEvent details (coordinates, target element) |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.2.0 Event Dispatch

#### 2.5.2.1 Source Id

browser

#### 2.5.2.2 Target Id

REPO-FE-MPL

#### 2.5.2.3 Message

2. Dispatches 'onClick' DOM event to the HelpIconComponent.

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 Event Dispatch

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
| Protocol | JavaScript Event Loop |
| Method | React SyntheticEvent Handler |
| Parameters | Event object |
| Authentication | N/A |
| Error Handling | React Error Boundary catches rendering errors. |

### 2.5.3.0 State Management

#### 2.5.3.1 Source Id

REPO-FE-MPL

#### 2.5.3.2 Target Id

REPO-FE-MPL

#### 2.5.3.3 Message

3. Toggles internal state `isPopoverOpen` to `true` and triggers component re-render.

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
| Protocol | Internal Function Call |
| Method | React.useState setter |
| Parameters | New state value (`true`) |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.4.0 Data Retrieval

#### 2.5.4.1 Source Id

REPO-FE-MPL

#### 2.5.4.2 Target Id

REPO-FE-MPL

#### 2.5.4.3 Message

4. During render, calls i18nProvider to get localized help text for a specific key (e.g., 'help.failoverCondition').

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 Data Retrieval

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Return Message

5. Returns translated string for the current user locale (e.g., 'Bedingung für Failover-Auslösung').

#### 2.5.4.8 Has Return

✅ Yes

#### 2.5.4.9 Is Activation

❌ No

#### 2.5.4.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Function Call |
| Method | i18n.t('help.failoverCondition') |
| Parameters | Translation key (string), options (optional) |
| Authentication | N/A |
| Error Handling | If key is not found for current locale, fallback t... |

### 2.5.5.0 UI Render

#### 2.5.5.1 Source Id

REPO-FE-MPL

#### 2.5.5.2 Target Id

browser

#### 2.5.5.3 Message

6. Renders the PopoverComponent into the virtual DOM with the fetched text. React updates the actual DOM.

#### 2.5.5.4 Sequence Number

6

#### 2.5.5.5 Type

🔹 UI Render

#### 2.5.5.6 Is Synchronous

✅ Yes

#### 2.5.5.7 Return Message



#### 2.5.5.8 Has Return

❌ No

#### 2.5.5.9 Is Activation

❌ No

#### 2.5.5.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | DOM API |
| Method | DOM Manipulation |
| Parameters | HTML elements for the popover |
| Authentication | N/A |
| Error Handling | The component calculates its position to remain wi... |

### 2.5.6.0 UI Feedback

#### 2.5.6.1 Source Id

browser

#### 2.5.6.2 Target Id

user

#### 2.5.6.3 Message

7. Displays the positioned popover with the help text.

#### 2.5.6.4 Sequence Number

7

#### 2.5.6.5 Type

🔹 UI Feedback

#### 2.5.6.6 Is Synchronous

✅ Yes

#### 2.5.6.7 Return Message



#### 2.5.6.8 Has Return

❌ No

#### 2.5.6.9 Is Activation

❌ No

### 2.5.7.0 User Input

#### 2.5.7.1 Source Id

user

#### 2.5.7.2 Target Id

browser

#### 2.5.7.3 Message

8. Clicks on an area outside the PopoverComponent.

#### 2.5.7.4 Sequence Number

8

#### 2.5.7.5 Type

🔹 User Input

#### 2.5.7.6 Is Synchronous

✅ Yes

#### 2.5.7.7 Return Message



#### 2.5.7.8 Has Return

❌ No

#### 2.5.7.9 Is Activation

❌ No

### 2.5.8.0 Event Dispatch

#### 2.5.8.1 Source Id

browser

#### 2.5.8.2 Target Id

REPO-FE-MPL

#### 2.5.8.3 Message

9. Dispatches 'onClick' event to the document-level event listener.

#### 2.5.8.4 Sequence Number

9

#### 2.5.8.5 Type

🔹 Event Dispatch

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
| Protocol | JavaScript Event Loop |
| Method | Event listener callback |
| Parameters | Event object |
| Authentication | N/A |
| Error Handling | The listener must check if the click target is out... |

### 2.5.9.0 State Management

#### 2.5.9.1 Source Id

REPO-FE-MPL

#### 2.5.9.2 Target Id

REPO-FE-MPL

#### 2.5.9.3 Message

10. The event listener handler sets `isPopoverOpen` state to `false`.

#### 2.5.9.4 Sequence Number

10

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
| Protocol | Internal Function Call |
| Method | React.useState setter |
| Parameters | New state value (`false`) |
| Authentication | N/A |
| Error Handling | The component cleans up its event listener on unmo... |

### 2.5.10.0 UI Update

#### 2.5.10.1 Source Id

REPO-FE-MPL

#### 2.5.10.2 Target Id

browser

#### 2.5.10.3 Message

11. React unmounts the PopoverComponent, removing it from the DOM.

#### 2.5.10.4 Sequence Number

11

#### 2.5.10.5 Type

🔹 UI Update

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
| Protocol | DOM API |
| Method | DOM Manipulation |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.11.0 UI Feedback

#### 2.5.11.1 Source Id

browser

#### 2.5.11.2 Target Id

user

#### 2.5.11.3 Message

12. Hides the popover.

#### 2.5.11.4 Sequence Number

12

#### 2.5.11.5 Type

🔹 UI Feedback

#### 2.5.11.6 Is Synchronous

✅ Yes

#### 2.5.11.7 Return Message



#### 2.5.11.8 Has Return

❌ No

#### 2.5.11.9 Is Activation

❌ No

## 2.6.0.0 Notes

### 2.6.1.0 Content

#### 2.6.1.1 Content

Accessibility (WCAG 2.1 AA): The help icon must be a <button> element. It must be focusable via keyboard ('Tab' key) and activated with 'Enter' or 'Space'. The popover should be dismissible with the 'Escape' key. ARIA attributes (e.g., 'aria-describedby') must link the icon to its popover content.

#### 2.6.1.2 Position

top-right

#### 2.6.1.3 Participant Id

REPO-FE-MPL

#### 2.6.1.4 Sequence Number

2

### 2.6.2.0 Content

#### 2.6.2.1 Content

Responsiveness: The popover's rendering logic must include checks against `window.innerWidth` and `element.getBoundingClientRect()` to dynamically adjust its position (e.g., left, right, top, bottom) to avoid overflowing the viewport.

#### 2.6.2.2 Position

bottom-right

#### 2.6.2.3 Participant Id

REPO-FE-MPL

#### 2.6.2.4 Sequence Number

6

### 2.6.3.0 Content

#### 2.6.3.1 Content

Hover Interaction: An alternative interaction pattern is to show a tooltip on hover. This can be achieved using `onMouseEnter` and `onMouseLeave` events with a debounce timer (~500ms) to prevent flickering.

#### 2.6.3.2 Position

bottom-left

#### 2.6.3.3 Participant Id

browser

#### 2.6.3.4 Sequence Number

1

### 2.6.4.0 Content

#### 2.6.4.1 Content

Graceful Degradation: If the i18n lookup in step 4 fails to find a translation key, the HelpIconComponent must not render itself in the UI to avoid showing a non-functional element to the user.

#### 2.6.4.2 Position

bottom-right

#### 2.6.4.3 Participant Id

REPO-FE-MPL

#### 2.6.4.4 Sequence Number

4

## 2.7.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | All help text content fetched from i18n resource f... |
| Performance Targets | The latency from user interaction (click/hover) to... |
| Error Handling Strategy | The primary error condition is a missing translati... |
| Testing Considerations | Unit tests (Vitest/RTL) should cover state togglin... |
| Monitoring Requirements | While not critical, user interactions with help ic... |
| Deployment Considerations | The HelpIconComponent and PopoverComponent should ... |

