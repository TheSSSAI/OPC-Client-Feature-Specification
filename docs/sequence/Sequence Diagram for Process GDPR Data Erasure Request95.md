sequenceDiagram
    actor "User (Engineer/Admin)" as UserEngineerAdmin
    participant "Web Browser" as WebBrowser
    participant "React Application" as ReactApplication
    participant "Help Component" as HelpComponent
    participant "i18n Module" as i18nModule

    UserEngineerAdmin->>WebBrowser: 1. Clicks on help icon ('?') next to a configuration field.
    activate ReactApplication
    WebBrowser->>ReactApplication: 2. Dispatches 'onClick' DOM event to the React event system.
    activate HelpComponent
    ReactApplication->>HelpComponent: 3. Invokes 'handleClick' event handler.
    HelpComponent->>HelpComponent: 4. Updates internal state to show popover (e.g., 'setAnchorEl(event.currentTarget)').
    activate i18nModule
    HelpComponent->>i18nModule: 5. Requests localized string using a specific key (e.g., 'help.failoverTriggerCondition').
    i18nModule-->>HelpComponent: Returns translated string for the current user locale.
    HelpComponent->>ReactApplication: 6. Triggers re-render with the visible popover containing the localized help text.
    ReactApplication->>WebBrowser: 7. Updates the DOM to display the popover element.
    WebBrowser->>UserEngineerAdmin: 8. Displays the rendered popover with help text.

    note over HelpComponent: Accessibility (AC-005): The help icon must be focusable via keyboard. The 'Enter' or 'Space' key ...
    note over i18nModule: Localization (AC-003): The i18n module is initialized with the user's preferred language (from RE...
    note over ReactApplication: Responsiveness (AC-004): The popover positioning logic must detect viewport boundaries and adjust...

    deactivate i18nModule
    deactivate HelpComponent
    deactivate ReactApplication
