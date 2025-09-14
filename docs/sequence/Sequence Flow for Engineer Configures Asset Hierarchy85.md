# 1 Overview

## 1.1 Diagram Id

SEQ-UJ-002

## 1.2 Name

Engineer Configures Asset Hierarchy

## 1.3 Description

An Engineer uses the web UI to build a hierarchical representation of a physical plant compliant with ISA-95. They create sites, areas, and machines, and then use a drag-and-drop interface to map specific OPC tags from a server namespace to the properties of these assets.

## 1.4 Type

🔹 UserJourney

## 1.5 Purpose

To provide essential context to raw OPC data by organizing it within a structured, intuitive model of the physical plant, which is a prerequisite for advanced features like AI model assignment and AR visualization (REQ-FR-021).

## 1.6 Complexity

Medium

## 1.7 Priority

🔴 High

## 1.8 Frequency

OnDemand

## 1.9 Participants

- REPO-FE-MPL
- REPO-GW-API
- REPO-SVC-AST

## 1.10 Key Interactions

- The Engineer navigates to the Asset Management module in the UI.
- They create new asset nodes (e.g., 'Site A', 'Line 1') via a form, building out the hierarchy.
- The UI displays the OPC server namespace, browsed live via the backend.
- The Engineer drags an OPC tag from the namespace and drops it onto an asset in the hierarchy.
- The frontend sends an API request to the Asset Service to create the mapping between the asset ID and the OPC tag's node ID.
- The Asset Service persists this relationship in its database.

## 1.11 Triggers

- An Engineer needs to configure the system for a new production line or make changes to the existing plant model.

## 1.12 Outcomes

- A structured asset model is created in the system that reflects the physical plant layout.
- OPC tags are mapped to the asset model, providing critical context for all data analysis.

## 1.13 Business Rules

- The asset hierarchy must be designed to be compatible with the ISA-95 standard (REQ-CON-004).
- The UI must support drag-and-drop tag configuration and namespace browsing (REQ-FR-009).
- The module must support asset templates to speed up configuration (REQ-FR-021).

## 1.14 Error Scenarios

- Attempting to create a duplicate asset name within the same parent node.
- The backend fails to browse the OPC server namespace due to connectivity issues.
- A database constraint prevents the asset-tag mapping from being saved.

## 1.15 Integration Points

*No items available*

# 2.0 Details

## 2.1 Diagram Id

SEQ-US-064

## 2.2 Name

User Accesses Context-Sensitive UI Help

## 2.3 Description

Technical sequence for a user interacting with a help icon within the React SPA. The sequence details the client-side logic for event handling, state management, fetching localized content from the i18n module, and rendering an accessible, theme-aware popover component. This is a purely frontend interaction designed to fulfill REQ-IFC-001 (Context-sensitive help) and REQ-FR-009 (Localization).

## 2.4 Participants

### 2.4.1 Actor

#### 2.4.1.1 Repository Id

User

#### 2.4.1.2 Display Name

User (Engineer/Admin)

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
| Color | #111111 |
| Stereotype | User |

### 2.4.2.0 Frontend

#### 2.4.2.1 Repository Id

REPO-FE-MPL

#### 2.4.2.2 Display Name

React SPA Component

#### 2.4.2.3 Type

🔹 Frontend

#### 2.4.2.4 Technology

React 18, TypeScript, Material-UI

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #4287f5 |
| Stereotype | UI Component |

### 2.4.3.0 Internal Library

#### 2.4.3.1 Repository Id

I18nModule

#### 2.4.3.2 Display Name

i18n Module

#### 2.4.3.3 Type

🔹 Internal Library

#### 2.4.3.4 Technology

i18next (or similar)

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | rectangle |
| Color | #f5b042 |
| Stereotype | Client-Side Module |

## 2.5.0.0 Interactions

### 2.5.1.0 UI Interaction

#### 2.5.1.1 Source Id

User

#### 2.5.1.2 Target Id

REPO-FE-MPL

#### 2.5.1.3 Message

Clicks the help icon ('?') next to a complex configuration field.

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 UI Interaction

#### 2.5.1.6 Is Synchronous

❌ No

#### 2.5.1.7 Has Return

❌ No

#### 2.5.1.8 Is Activation

✅ Yes

#### 2.5.1.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | DOM Event |
| Method | onClick |
| Parameters | MouseEvent |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.2.0 State Management

#### 2.5.2.1 Source Id

REPO-FE-MPL

#### 2.5.2.2 Target Id

REPO-FE-MPL

#### 2.5.2.3 Message

Invokes onClickHandler(), toggles internal state `isPopoverOpen` to true.

#### 2.5.2.4 Sequence Number

2

#### 2.5.2.5 Type

🔹 State Management

#### 2.5.2.6 Is Synchronous

✅ Yes

#### 2.5.2.7 Has Return

❌ No

#### 2.5.2.8 Is Activation

❌ No

#### 2.5.2.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Function Call |
| Method | React.useState setter |
| Parameters | newState: boolean |
| Authentication | N/A |
| Error Handling | Handled by React framework. |

### 2.5.3.0 Data Retrieval

#### 2.5.3.1 Source Id

REPO-FE-MPL

#### 2.5.3.2 Target Id

I18nModule

#### 2.5.3.3 Message

Fetches localized help text for the specific UI element.

#### 2.5.3.4 Sequence Number

3

#### 2.5.3.5 Type

🔹 Data Retrieval

#### 2.5.3.6 Is Synchronous

✅ Yes

#### 2.5.3.7 Return Message

Returns the translated string for the user's current locale.

#### 2.5.3.8 Has Return

✅ Yes

#### 2.5.3.9 Is Activation

❌ No

#### 2.5.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Function Call |
| Method | i18n.t('help.config.failoverTriggerCondition') |
| Parameters | key: string |
| Authentication | N/A |
| Error Handling | If key is not found, falls back to default languag... |

### 2.5.4.0 UI Rendering

#### 2.5.4.1 Source Id

REPO-FE-MPL

#### 2.5.4.2 Target Id

REPO-FE-MPL

#### 2.5.4.3 Message

Conditionally renders the PopoverComponent with the localized text.

#### 2.5.4.4 Sequence Number

4

#### 2.5.4.5 Type

🔹 UI Rendering

#### 2.5.4.6 Is Synchronous

✅ Yes

#### 2.5.4.7 Has Return

❌ No

#### 2.5.4.8 Is Activation

❌ No

#### 2.5.4.9 Technical Details

| Property | Value |
|----------|-------|
| Protocol | React Render |
| Method | JSX Rendering |
| Parameters | props: { text: string, anchorEl: HTMLElement } |
| Authentication | N/A |
| Error Handling | The component does not render if the help text is ... |

#### 2.5.4.10 Nested Interactions

##### 2.5.4.10.1 Internal Logic

###### 2.5.4.10.1.1 Source Id

REPO-FE-MPL

###### 2.5.4.10.1.2 Target Id

REPO-FE-MPL

###### 2.5.4.10.1.3 Message

Popover position is calculated to remain within the viewport.

###### 2.5.4.10.1.4 Sequence Number

4.1

###### 2.5.4.10.1.5 Type

🔹 Internal Logic

###### 2.5.4.10.1.6 Is Synchronous

✅ Yes

###### 2.5.4.10.1.7 Has Return

❌ No

##### 2.5.4.10.2.0 Styling

###### 2.5.4.10.2.1 Source Id

REPO-FE-MPL

###### 2.5.4.10.2.2 Target Id

REPO-FE-MPL

###### 2.5.4.10.2.3 Message

Styles are applied based on the current theme (light/dark).

###### 2.5.4.10.2.4 Sequence Number

4.2

###### 2.5.4.10.2.5 Type

🔹 Styling

###### 2.5.4.10.2.6 Is Synchronous

✅ Yes

###### 2.5.4.10.2.7 Has Return

❌ No

##### 2.5.4.10.3.0 Accessibility

###### 2.5.4.10.3.1 Source Id

REPO-FE-MPL

###### 2.5.4.10.3.2 Target Id

REPO-FE-MPL

###### 2.5.4.10.3.3 Message

ARIA attributes (`role`, `aria-describedby`) are set for accessibility.

###### 2.5.4.10.3.4 Sequence Number

4.3

###### 2.5.4.10.3.5 Type

🔹 Accessibility

###### 2.5.4.10.3.6 Is Synchronous

✅ Yes

###### 2.5.4.10.3.7 Has Return

❌ No

### 2.5.5.0.0.0 UI Interaction

#### 2.5.5.1.0.0 Source Id

User

#### 2.5.5.2.0.0 Target Id

REPO-FE-MPL

#### 2.5.5.3.0.0 Message

Presses 'Escape' key or clicks outside the popover.

#### 2.5.5.4.0.0 Sequence Number

5

#### 2.5.5.5.0.0 Type

🔹 UI Interaction

#### 2.5.5.6.0.0 Is Synchronous

❌ No

#### 2.5.5.7.0.0 Has Return

❌ No

#### 2.5.5.8.0.0 Is Activation

❌ No

#### 2.5.5.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | DOM Event |
| Method | onKeyDown \| onBlur |
| Parameters | KeyboardEvent \| FocusEvent |
| Authentication | N/A |
| Error Handling | N/A |

### 2.5.6.0.0.0 State Management

#### 2.5.6.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.6.2.0.0 Target Id

REPO-FE-MPL

#### 2.5.6.3.0.0 Message

Invokes onCloseHandler(), sets internal state `isPopoverOpen` to false.

#### 2.5.6.4.0.0 Sequence Number

6

#### 2.5.6.5.0.0 Type

🔹 State Management

#### 2.5.6.6.0.0 Is Synchronous

✅ Yes

#### 2.5.6.7.0.0 Has Return

❌ No

#### 2.5.6.8.0.0 Is Activation

❌ No

#### 2.5.6.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Internal Function Call |
| Method | React.useState setter |
| Parameters | newState: boolean |
| Authentication | N/A |
| Error Handling | Handled by React framework. |

### 2.5.7.0.0.0 UI Rendering

#### 2.5.7.1.0.0 Source Id

REPO-FE-MPL

#### 2.5.7.2.0.0 Target Id

REPO-FE-MPL

#### 2.5.7.3.0.0 Message

React reconciler unmounts the PopoverComponent from the DOM.

#### 2.5.7.4.0.0 Sequence Number

7

#### 2.5.7.5.0.0 Type

🔹 UI Rendering

#### 2.5.7.6.0.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0.0 Has Return

❌ No

#### 2.5.7.8.0.0 Is Activation

❌ No

#### 2.5.7.9.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | React Render |
| Method | Component Unmount |
| Parameters | N/A |
| Authentication | N/A |
| Error Handling | N/A |

## 2.6.0.0.0.0 Notes

### 2.6.1.0.0.0 Content

#### 2.6.1.1.0.0 Content

Tooltip Scenario: A similar, but simpler, flow occurs for tooltips. It is triggered by onMouseEnter/onMouseLeave events, displays a TooltipComponent, and typically does not require a click to dismiss.

#### 2.6.1.2.0.0 Position

bottom

#### 2.6.1.3.0.0 Participant Id

*Not specified*

#### 2.6.1.4.0.0 Sequence Number

*Not specified*

### 2.6.2.0.0.0 Content

#### 2.6.2.1.0.0 Content

Keyboard Accessibility (AC-005): The user can navigate to the help icon using the 'Tab' key. Pressing 'Enter' or 'Space' triggers the same onClickHandler() as a mouse click (Sequence #2).

#### 2.6.2.2.0.0 Position

right

#### 2.6.2.3.0.0 Participant Id

REPO-FE-MPL

#### 2.6.2.4.0.0 Sequence Number

1

## 2.7.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Help text must be rendered as plain text to preven... |
| Performance Targets | UI response time for displaying the popover or too... |
| Error Handling Strategy | The i18n module is responsible for gracefully hand... |
| Testing Considerations | Unit tests (Vitest/RTL) must cover state toggling,... |
| Monitoring Requirements | Integrate with the analytics platform to track `he... |
| Deployment Considerations | The help component and its associated logic should... |

