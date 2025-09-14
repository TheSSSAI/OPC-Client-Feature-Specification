sequenceDiagram
    actor "User (Engineer/Admin)" as UserEngineerAdmin
    participant "React SPA Component" as ReactSPAComponent
    participant "i18n Module" as i18nModule

    activate ReactSPAComponent
    UserEngineerAdmin->>ReactSPAComponent: 1. Clicks the help icon ('?') next to a complex configuration field.
    ReactSPAComponent->>ReactSPAComponent: 2. Invokes onClickHandler(), toggles internal state isPopoverOpen to true.
    ReactSPAComponent->>i18nModule: 3. Fetches localized help text for the specific UI element.
    i18nModule-->>ReactSPAComponent: Returns the translated string for the user's current locale.
    ReactSPAComponent->>ReactSPAComponent: 4. Conditionally renders the PopoverComponent with the localized text.
    ReactSPAComponent->>ReactSPAComponent: 4.1. Popover position is calculated to remain within the viewport.
    ReactSPAComponent->>ReactSPAComponent: 4.2. Styles are applied based on the current theme (light/dark).
    ReactSPAComponent->>ReactSPAComponent: 4.3. ARIA attributes (role, aria-describedby) are set for accessibility.
    UserEngineerAdmin->>ReactSPAComponent: 5. Presses 'Escape' key or clicks outside the popover.
    ReactSPAComponent->>ReactSPAComponent: 6. Invokes onCloseHandler(), sets internal state isPopoverOpen to false.
    ReactSPAComponent->>ReactSPAComponent: 7. React reconciler unmounts the PopoverComponent from the DOM.

    note over ReactSPAComponent: Keyboard Accessibility (AC-005): The user can navigate to the help icon using the 'Tab' key. Pres...

    deactivate ReactSPAComponent
