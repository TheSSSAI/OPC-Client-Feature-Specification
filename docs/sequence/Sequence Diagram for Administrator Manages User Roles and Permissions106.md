sequenceDiagram
    actor "User" as User
    participant "I18n Service" as I18nService
    participant "I18n Service" as I18nService
    participant "I18n Service" as I18nService

    activate I18nService
    User->>I18nService: 1. Clicks on help icon (?) next to a configuration field.
    I18nService->>I18nService: 2. Triggers onClick event handler bound to the Help Component.
    I18nService->>I18nService: 3. Updates internal state to show the popover: setState({ isVisible: true, anchorEl: event.currentTarget }).
    I18nService->>I18nService: 4. [Render] Fetch localized help text for the corresponding help key.
    I18nService-->>I18nService: Returns localized string.
    I18nService->>I18nService: 5. [Render] Renders Material-UI Popover component with fetched content.
    I18nService-->>I18nService: Updates virtual DOM.
    I18nService->>I18nService: 6. Commits changes to the actual DOM, displaying the popover.
    User->>I18nService: 7. Clicks anywhere outside the popover element.
    I18nService->>I18nService: 8. Triggers Popover's onClose event handler.
    I18nService->>I18nService: 9. Updates internal state to hide popover: setState({ isVisible: false, anchorEl: null }).
    I18nService->>I18nService: 10. Commits changes to DOM, removing the popover element.

    note over I18nService: Accessibility (AC-005): The help icon must be focusable via keyboard ('Tab' key). The popover mus...
    note over I18nService: Responsiveness (AC-004): The Popover component's positioning logic must be configured to prevent ...
    note over User: Tooltip Variant (AC-002): For simpler help text on elements like icon-only buttons, a similar flo...

    deactivate I18nService
