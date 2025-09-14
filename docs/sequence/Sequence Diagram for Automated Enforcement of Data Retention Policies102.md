sequenceDiagram
    actor "User" as User
    participant "Browser" as Browser
    participant "Help Icon Component" as HelpIconComponent
    participant "State Management (Redux)" as StateManagementRedux
    participant "i18n Service" as i18nService
    participant "Tooltip/Popover Component" as TooltipPopoverComponent

    activate Browser
    User->>Browser: 1. Clicks help icon ('?') next to a configuration field.
    activate HelpIconComponent
    Browser->>HelpIconComponent: 2. Triggers onClick event handler.
    HelpIconComponent->>StateManagementRedux: 3. Get current user language preference.
    StateManagementRedux-->>HelpIconComponent: Returns language code (e.g., 'de').
    HelpIconComponent->>i18nService: 4. Request localized help string using a unique key.
    i18nService-->>HelpIconComponent: Returns localized string for the specified key.
    HelpIconComponent->>HelpIconComponent: 5. Update component state to show popover.
    activate TooltipPopoverComponent
    HelpIconComponent->>TooltipPopoverComponent: 6. Render with localized content and anchor.
    TooltipPopoverComponent-->>HelpIconComponent: Returns JSX for the popover.
    TooltipPopoverComponent->>TooltipPopoverComponent: 6.1. Calculate optimal position to remain in viewport.
    TooltipPopoverComponent-->>TooltipPopoverComponent: Position coordinates.
    TooltipPopoverComponent->>TooltipPopoverComponent: 6.2. Set ARIA attributes for accessibility.
    TooltipPopoverComponent->>Browser: 7. Renders popover HTML/CSS.
    Browser->>User: 8. Displays localized help popover.

    note over HelpIconComponent: Graceful Degradation (AC-006): To prevent rendering a non-functional icon, the parent component s...
    note over Browser: Hover Interaction (AC-002): A similar sequence occurs for hover. The trigger would be onMouseEnte...
    note over User: Keyboard Accessibility (AC-005): The HelpIconComponent must be focusable (e.g., <button>). The on...

    deactivate TooltipPopoverComponent
    deactivate HelpIconComponent
    deactivate Browser
