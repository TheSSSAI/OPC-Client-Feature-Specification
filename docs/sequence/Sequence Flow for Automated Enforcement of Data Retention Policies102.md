# 1 Overview

## 1.1 Diagram Id

SEQ-DF-003

## 1.2 Name

Automated Enforcement of Data Retention Policies

## 1.3 Description

TimescaleDB's built-in data lifecycle features automatically execute based on pre-configured, tenant-specific policies. For example, a policy might drop raw time-series data chunks older than 1 year, while another policy retains hourly aggregated data for 5 years.

## 1.4 Type

🔹 DataFlow

## 1.5 Purpose

To automatically manage storage costs and meet compliance requirements by enforcing data retention policies for different data types without manual intervention (REQ-NFR-007).

## 1.6 Complexity

Medium

## 1.7 Priority

🟡 Medium

## 1.8 Frequency

Daily

## 1.9 Participants

- TimescaleDB
- REPO-SVC-TNM

## 1.10 Key Interactions

- An Administrator configures retention policies for a tenant via the Tenant Management Service (REPO-SVC-TNM).
- The service translates this into TimescaleDB `add_retention_policy` commands on the relevant hypertables.
- TimescaleDB's internal scheduler runs periodically (typically daily).
- The scheduler identifies data chunks in hypertables that are older than the policy's specified retention period.
- It executes a `DROP_CHUNK` command to permanently and efficiently delete the old data chunks.

## 1.11 Triggers

- The passage of time causes data chunks to exceed their configured retention period.

## 1.12 Outcomes

- Old data is automatically and permanently deleted from the database.
- Storage growth is managed according to defined policies, controlling costs.
- Compliance with data retention regulations is automated.

## 1.13 Business Rules

- Data retention policies must be configurable on a per-tenant basis (REQ-NFR-007).
- Default policies shall be: 1 year for raw time-series data, 5 years for aggregated data, and 7 years for audit logs (REQ-NFR-007).

## 1.14 Error Scenarios

- A TimescaleDB lifecycle policy job fails to execute due to database locks or other internal errors.
- A misconfiguration causes critical data to be deleted prematurely, requiring recovery from backup.
- The policies are not aggressive enough, leading to unexpected storage cost increases.

## 1.15 Integration Points

- TimescaleDB

# 2.0 Details

## 2.1 Diagram Id

SEQ-UI-064

## 2.2 Name

Displaying Context-Sensitive Help for UI Elements

## 2.3 Description

This sequence details the client-side interactions within the React Single-Page Application when a user requests context-sensitive help for a UI element. It covers fetching localized content from the i18n service, managing component state, rendering the help popover, and ensuring accessibility and responsiveness.

## 2.4 Participants

### 2.4.1 Actor

#### 2.4.1.1 Repository Id

USER

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

### 2.4.2.0 System

#### 2.4.2.1 Repository Id

BROWSER

#### 2.4.2.2 Display Name

Browser

#### 2.4.2.3 Type

🔹 System

#### 2.4.2.4 Technology

Chrome, Firefox, Edge, Safari

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | boundary |
| Color | #cccccc |
| Stereotype | Environment |

### 2.4.3.0 React Component

#### 2.4.3.1 Repository Id

REPO-FE-MPL

#### 2.4.3.2 Display Name

Help Icon Component

#### 2.4.3.3 Type

🔹 React Component

#### 2.4.3.4 Technology

React 18, Material-UI

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #2ECC71 |
| Stereotype | UI Component |

### 2.4.4.0 Client-side Service

#### 2.4.4.1 Repository Id

STATE_MANAGEMENT_REDUX

#### 2.4.4.2 Display Name

State Management (Redux)

#### 2.4.4.3 Type

🔹 Client-side Service

#### 2.4.4.4 Technology

Redux Toolkit

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #7D3C98 |
| Stereotype | State Store |

### 2.4.5.0 Client-side Service

#### 2.4.5.1 Repository Id

I18N_SERVICE

#### 2.4.5.2 Display Name

i18n Service

#### 2.4.5.3 Type

🔹 Client-side Service

#### 2.4.5.4 Technology

i18next

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #E67E22 |
| Stereotype | Service |

### 2.4.6.0 React Component

#### 2.4.6.1 Repository Id

TOOLTIP_COMPONENT

#### 2.4.6.2 Display Name

Tooltip/Popover Component

#### 2.4.6.3 Type

🔹 React Component

#### 2.4.6.4 Technology

React 18, Material-UI

#### 2.4.6.5 Order

6

#### 2.4.6.6 Style

| Property | Value |
|----------|-------|
| Shape | participant |
| Color | #3498DB |
| Stereotype | UI Component |

## 2.5.0.0 Interactions

### 2.5.1.0 User Input

#### 2.5.1.1 Source Id

USER

#### 2.5.1.2 Target Id

BROWSER

#### 2.5.1.3 Message

Clicks help icon ('?') next to a configuration field.

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

✅ Yes

#### 2.5.1.10 Technical Details

##### 2.5.1.10.1 Protocol

DOM Event

##### 2.5.1.10.2 Method

click

##### 2.5.1.10.3 Parameters

Event object with target element

##### 2.5.1.10.4 Authentication

N/A

##### 2.5.1.10.5 Error Handling

N/A

##### 2.5.1.10.6 Performance

###### 2.5.1.10.6.1 Latency

<10ms

### 2.5.2.0.0.0 Event Handling

#### 2.5.2.1.0.0 Source Id

BROWSER

#### 2.5.2.2.0.0 Target Id

REPO-FE-MPL

#### 2.5.2.3.0.0 Message

Triggers onClick event handler.

#### 2.5.2.4.0.0 Sequence Number

2

#### 2.5.2.5.0.0 Type

🔹 Event Handling

#### 2.5.2.6.0.0 Is Synchronous

✅ Yes

#### 2.5.2.7.0.0 Return Message



#### 2.5.2.8.0.0 Has Return

❌ No

#### 2.5.2.9.0.0 Is Activation

✅ Yes

#### 2.5.2.10.0.0 Technical Details

##### 2.5.2.10.1.0 Protocol

JavaScript

##### 2.5.2.10.2.0 Method

handleIconClick()

##### 2.5.2.10.3.0 Parameters

React.MouseEvent

##### 2.5.2.10.4.0 Authentication

N/A

##### 2.5.2.10.5.0 Error Handling

Standard JS try/catch

##### 2.5.2.10.6.0 Performance

###### 2.5.2.10.6.1 Latency

<5ms

### 2.5.3.0.0.0 State Retrieval

#### 2.5.3.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.3.2.0.0 Target Id

STATE_MANAGEMENT_REDUX

#### 2.5.3.3.0.0 Message

Get current user language preference.

#### 2.5.3.4.0.0 Sequence Number

3

#### 2.5.3.5.0.0 Type

🔹 State Retrieval

#### 2.5.3.6.0.0 Is Synchronous

✅ Yes

#### 2.5.3.7.0.0 Return Message

Returns language code (e.g., 'de').

#### 2.5.3.8.0.0 Has Return

✅ Yes

#### 2.5.3.9.0.0 Is Activation

❌ No

#### 2.5.3.10.0.0 Technical Details

##### 2.5.3.10.1.0 Protocol

JavaScript Function Call

##### 2.5.3.10.2.0 Method

useSelector(selectUserLanguage)

##### 2.5.3.10.3.0 Parameters

Redux selector function

##### 2.5.3.10.4.0 Authentication

N/A

##### 2.5.3.10.5.0 Error Handling

Selector returns default state ('en') if not set.

##### 2.5.3.10.6.0 Performance

###### 2.5.3.10.6.1 Latency

<1ms

### 2.5.4.0.0.0 Data Retrieval

#### 2.5.4.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.4.2.0.0 Target Id

I18N_SERVICE

#### 2.5.4.3.0.0 Message

Request localized help string using a unique key.

#### 2.5.4.4.0.0 Sequence Number

4

#### 2.5.4.5.0.0 Type

🔹 Data Retrieval

#### 2.5.4.6.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0 Return Message

Returns localized string for the specified key.

#### 2.5.4.8.0.0 Has Return

✅ Yes

#### 2.5.4.9.0.0 Is Activation

❌ No

#### 2.5.4.10.0.0 Technical Details

##### 2.5.4.10.1.0 Protocol

JavaScript Function Call

##### 2.5.4.10.2.0 Method

t('help.failoverCondition', { lng: 'de' })

##### 2.5.4.10.3.0 Parameters

key: string, options: { lng: string }

##### 2.5.4.10.4.0 Authentication

N/A

##### 2.5.4.10.5.0 Error Handling

If key for 'de' is not found, service automatically falls back to default language 'en' as per BR-001.

##### 2.5.4.10.6.0 Performance

###### 2.5.4.10.6.1 Latency

<5ms (assumes language resources are pre-loaded)

### 2.5.5.0.0.0 State Update

#### 2.5.5.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.5.2.0.0 Target Id

REPO-FE-MPL

#### 2.5.5.3.0.0 Message

Update component state to show popover.

#### 2.5.5.4.0.0 Sequence Number

5

#### 2.5.5.5.0.0 Type

🔹 State Update

#### 2.5.5.6.0.0 Is Synchronous

❌ No

#### 2.5.5.7.0.0 Return Message



#### 2.5.5.8.0.0 Has Return

❌ No

#### 2.5.5.9.0.0 Is Activation

❌ No

#### 2.5.5.10.0.0 Technical Details

##### 2.5.5.10.1.0 Protocol

React Hook

##### 2.5.5.10.2.0 Method

setPopoverState({ isVisible: true, content: 'Localized Text', anchorEl: event.currentTarget })

##### 2.5.5.10.3.0 Parameters

New state object

##### 2.5.5.10.4.0 Authentication

N/A

##### 2.5.5.10.5.0 Error Handling

N/A

##### 2.5.5.10.6.0 Performance

###### 2.5.5.10.6.1 Latency

N/A (triggers async re-render)

### 2.5.6.0.0.0 Component Rendering

#### 2.5.6.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.6.2.0.0 Target Id

TOOLTIP_COMPONENT

#### 2.5.6.3.0.0 Message

Render with localized content and anchor.

#### 2.5.6.4.0.0 Sequence Number

6

#### 2.5.6.5.0.0 Type

🔹 Component Rendering

#### 2.5.6.6.0.0 Is Synchronous

✅ Yes

#### 2.5.6.7.0.0 Return Message

Returns JSX for the popover.

#### 2.5.6.8.0.0 Has Return

✅ Yes

#### 2.5.6.9.0.0 Is Activation

✅ Yes

#### 2.5.6.10.0.0 Technical Details

##### 2.5.6.10.1.0 Protocol

React Render

##### 2.5.6.10.2.0 Method

<PopoverComponent .../>

##### 2.5.6.10.3.0 Parameters

props: { content: string, anchorEl: HTMLElement, onClose: function }

##### 2.5.6.10.4.0 Authentication

N/A

##### 2.5.6.10.5.0 Error Handling

Component internally handles positioning logic to stay within viewport.

##### 2.5.6.10.6.0 Performance

###### 2.5.6.10.6.1 Latency

<50ms

#### 2.5.6.11.0.0 Nested Interactions

##### 2.5.6.11.1.0 Internal Logic

###### 2.5.6.11.1.1 Source Id

TOOLTIP_COMPONENT

###### 2.5.6.11.1.2 Target Id

TOOLTIP_COMPONENT

###### 2.5.6.11.1.3 Message

Calculate optimal position to remain in viewport.

###### 2.5.6.11.1.4 Sequence Number

6.1

###### 2.5.6.11.1.5 Type

🔹 Internal Logic

###### 2.5.6.11.1.6 Is Synchronous

✅ Yes

###### 2.5.6.11.1.7 Return Message

Position coordinates.

###### 2.5.6.11.1.8 Has Return

✅ Yes

###### 2.5.6.11.1.9 Is Activation

❌ No

###### 2.5.6.11.1.10 Technical Details

####### 2.5.6.11.1.10.1 Protocol

JavaScript

####### 2.5.6.11.1.10.2 Method

calculatePosition()

####### 2.5.6.11.1.10.3 Parameters

anchorEl.getBoundingClientRect(), window.innerWidth, window.innerHeight

####### 2.5.6.11.1.10.4 Authentication

N/A

####### 2.5.6.11.1.10.5 Error Handling

Defaults to a standard position if calculation fails.

####### 2.5.6.11.1.10.6 Performance

######## 2.5.6.11.1.10.6.1 Latency

<10ms

##### 2.5.6.11.2.0.0.0 Accessibility Enhancement

###### 2.5.6.11.2.1.0.0 Source Id

TOOLTIP_COMPONENT

###### 2.5.6.11.2.2.0.0 Target Id

TOOLTIP_COMPONENT

###### 2.5.6.11.2.3.0.0 Message

Set ARIA attributes for accessibility.

###### 2.5.6.11.2.4.0.0 Sequence Number

6.2

###### 2.5.6.11.2.5.0.0 Type

🔹 Accessibility Enhancement

###### 2.5.6.11.2.6.0.0 Is Synchronous

✅ Yes

###### 2.5.6.11.2.7.0.0 Return Message



###### 2.5.6.11.2.8.0.0 Has Return

❌ No

###### 2.5.6.11.2.9.0.0 Is Activation

❌ No

###### 2.5.6.11.2.10.0.0 Technical Details

####### 2.5.6.11.2.10.1.0 Protocol

React Render

####### 2.5.6.11.2.10.2.0 Method

render()

####### 2.5.6.11.2.10.3.0 Parameters

aria-live, role='tooltip', etc.

####### 2.5.6.11.2.10.4.0 Authentication

N/A

####### 2.5.6.11.2.10.5.0 Error Handling

N/A

####### 2.5.6.11.2.10.6.0 Performance

######## 2.5.6.11.2.10.6.1 Latency

N/A

### 2.5.7.0.0.0.0.0 UI Update

#### 2.5.7.1.0.0.0.0 Source Id

TOOLTIP_COMPONENT

#### 2.5.7.2.0.0.0.0 Target Id

BROWSER

#### 2.5.7.3.0.0.0.0 Message

Renders popover HTML/CSS.

#### 2.5.7.4.0.0.0.0 Sequence Number

7

#### 2.5.7.5.0.0.0.0 Type

🔹 UI Update

#### 2.5.7.6.0.0.0.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0.0.0.0 Return Message



#### 2.5.7.8.0.0.0.0 Has Return

❌ No

#### 2.5.7.9.0.0.0.0 Is Activation

❌ No

#### 2.5.7.10.0.0.0.0 Technical Details

##### 2.5.7.10.1.0.0.0 Protocol

DOM Manipulation

##### 2.5.7.10.2.0.0.0 Method

Browser Paint

##### 2.5.7.10.3.0.0.0 Parameters

HTML, CSS

##### 2.5.7.10.4.0.0.0 Authentication

N/A

##### 2.5.7.10.5.0.0.0 Error Handling

N/A

##### 2.5.7.10.6.0.0.0 Performance

###### 2.5.7.10.6.1.0.0 Latency

Variable, dependent on browser

### 2.5.8.0.0.0.0.0 Visual Feedback

#### 2.5.8.1.0.0.0.0 Source Id

BROWSER

#### 2.5.8.2.0.0.0.0 Target Id

USER

#### 2.5.8.3.0.0.0.0 Message

Displays localized help popover.

#### 2.5.8.4.0.0.0.0 Sequence Number

8

#### 2.5.8.5.0.0.0.0 Type

🔹 Visual Feedback

#### 2.5.8.6.0.0.0.0 Is Synchronous

✅ Yes

#### 2.5.8.7.0.0.0.0 Return Message



#### 2.5.8.8.0.0.0.0 Has Return

❌ No

#### 2.5.8.9.0.0.0.0 Is Activation

❌ No

#### 2.5.8.10.0.0.0.0 Technical Details

##### 2.5.8.10.1.0.0.0 Protocol

Visual

##### 2.5.8.10.2.0.0.0 Method

N/A

##### 2.5.8.10.3.0.0.0 Parameters

N/A

##### 2.5.8.10.4.0.0.0 Authentication

N/A

##### 2.5.8.10.5.0.0.0 Error Handling

N/A

##### 2.5.8.10.6.0.0.0 Performance

###### 2.5.8.10.6.1.0.0 Latency

N/A

## 2.6.0.0.0.0.0.0 Notes

### 2.6.1.0.0.0.0.0 Content

#### 2.6.1.1.0.0.0.0 Content

Graceful Degradation (AC-006): To prevent rendering a non-functional icon, the parent component should check if a help key exists in the i18n service (`i18n.exists(key)`) before deciding to render the HelpIconComponent.

#### 2.6.1.2.0.0.0.0 Position

top

#### 2.6.1.3.0.0.0.0 Participant Id

REPO-FE-MPL

#### 2.6.1.4.0.0.0.0 Sequence Number

0

### 2.6.2.0.0.0.0.0 Content

#### 2.6.2.1.0.0.0.0 Content

Hover Interaction (AC-002): A similar sequence occurs for hover. The trigger would be `onMouseEnter` / `onMouseLeave` DOM events, and the component would use a timeout (e.g., 500ms) before initiating the state update to display the tooltip.

#### 2.6.2.2.0.0.0.0 Position

bottom

#### 2.6.2.3.0.0.0.0 Participant Id

BROWSER

#### 2.6.2.4.0.0.0.0 Sequence Number

1

### 2.6.3.0.0.0.0.0 Content

#### 2.6.3.1.0.0.0.0 Content

Keyboard Accessibility (AC-005): The HelpIconComponent must be focusable (e.g., `<button>`). The `onClick` handler is also triggered by 'Enter' or 'Space' key presses. An `onKeyDown` handler for the 'Escape' key must be added to the PopoverComponent to close it.

#### 2.6.3.2.0.0.0.0 Position

bottom

#### 2.6.3.3.0.0.0.0 Participant Id

USER

#### 2.6.3.4.0.0.0.0 Sequence Number

1

## 2.7.0.0.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | To prevent XSS vulnerabilities, the localized cont... |
| Performance Targets | The entire interaction, from user click to popover... |
| Error Handling Strategy | The primary failure case is missing help content o... |
| Testing Considerations | Unit tests (Vitest/RTL) must verify the component'... |
| Monitoring Requirements | N/A for this client-side feature. Optional: Implem... |
| Deployment Considerations | The HelpIconComponent and Tooltip/PopoverComponent... |

