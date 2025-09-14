sequenceDiagram
    actor "System User" as SystemUser
    participant "Browser (React SPA)" as BrowserReactSPA
    participant "Help Component" as HelpComponent
    participant "i18n Service" as i18nService

    activate BrowserReactSPA
    SystemUser->>BrowserReactSPA: 1. 1. Navigates to a configuration page
    activate HelpComponent
    BrowserReactSPA->>HelpComponent: 2. 2. Renders page and mounts HelpComponent instance
    HelpComponent->>i18nService: 3. 3. Requests localized help text using key
    i18nService-->>HelpComponent: Returns translated string or fallback.
    i18nService->>i18nService: 3.1. 3a. Looks up translation for current language (e.g., 'de')
    i18nService->>i18nService: 3.2. 3b. [Alt] Falls back to default language ('en') if translation is missing
    HelpComponent->>HelpComponent: 4. 4. Caches localized text in component state and renders help icon
    SystemUser->>HelpComponent: 5. 5. Activates help icon (Click or Keyboard 'Enter'/'Space')
    HelpComponent->>HelpComponent: 6. 6. Toggles visibility state and re-renders with popover
    HelpComponent->>BrowserReactSPA: 7. 7. Renders accessible popover adjacent to icon
    BrowserReactSPA-->>HelpComponent: Displays popover to user.
    SystemUser->>BrowserReactSPA: 8. 8. Closes popover ('Escape' key or clicking outside)
    BrowserReactSPA->>HelpComponent: 9. 9. Triggers component's onClose handler
    HelpComponent->>HelpComponent: 10. 10. Hides popover by updating state

    note over HelpComponent: The HelpComponent is designed as a reusable, high-order component or hook that can wrap other UI ...
    note over HelpComponent: Accessibility (WCAG 2.1 AA) is critical. The component must manage focus, use correct ARIA roles,...

    deactivate HelpComponent
    deactivate BrowserReactSPA
