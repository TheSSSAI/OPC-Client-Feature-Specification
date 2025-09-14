sequenceDiagram
    actor "User (Engineer/Admin)" as UserEngineerAdmin
    participant "Frontend: Help Component" as FrontendHelpComponent
    participant "i18n Resource Provider" as i18nResourceProvider

    activate FrontendHelpComponent
    UserEngineerAdmin->>FrontendHelpComponent: 1. 1. Clicks help icon (?) next to a configuration field.
    FrontendHelpComponent->>FrontendHelpComponent: 2. 2. Invokes internal handleTogglePopover handler, identifying content key (e.g., 'help.failoverCondition').
    activate i18nResourceProvider
    FrontendHelpComponent->>i18nResourceProvider: 3. 3. Requests localized string for the identified key and current user locale.
    i18nResourceProvider-->>FrontendHelpComponent: Returns the localized string.
    FrontendHelpComponent->>FrontendHelpComponent: 4. 4. Updates component state with the fetched content and anchor element, triggering a re-render.
    FrontendHelpComponent->>UserEngineerAdmin: 5. 5. Renders and displays Material-UI Popover component with localized content.
    UserEngineerAdmin->>FrontendHelpComponent: 6. 6. Dismisses the popover by clicking outside or pressing 'Escape' key.
    FrontendHelpComponent->>FrontendHelpComponent: 7. 7. Sets component state to hide the popover, triggering a final re-render to unmount it.

    note over FrontendHelpComponent: Accessibility (WCAG 2.1 AA): The help icon must be keyboard-focusable and activatable with 'Enter...
    note over FrontendHelpComponent: Graceful Degradation: If a helpKey is not provided to the component or if the resolved text is em...
    note over FrontendHelpComponent: Responsiveness: The Popover component must use a positioning library (like Popper.js, integrated ...

    deactivate i18nResourceProvider
    deactivate FrontendHelpComponent
