sequenceDiagram
    actor "System User" as SystemUser
    participant "Web Browser" as WebBrowser
    participant "Central Management Plane UI" as CentralManagementPlaneUI

    SystemUser->>WebBrowser: 1. 1. Clicks the help icon ('?' button) next to a configuration field.
    activate CentralManagementPlaneUI
    WebBrowser->>CentralManagementPlaneUI: 2. 2. Dispatches 'onClick' DOM event to the HelpIconComponent.
    CentralManagementPlaneUI->>CentralManagementPlaneUI: 3. 3. Toggles internal state isPopoverOpen to true and triggers component re-render.
    CentralManagementPlaneUI->>CentralManagementPlaneUI: 4. 4. During render, calls i18nProvider to get localized help text for a specific key (e.g., 'help.failoverCondition').
    CentralManagementPlaneUI-->>CentralManagementPlaneUI: 5. Returns translated string for the current user locale (e.g., 'Bedingung für Failover-Auslösung').
    CentralManagementPlaneUI->>WebBrowser: 6. 6. Renders the PopoverComponent into the virtual DOM with the fetched text. React updates the actual DOM.
    WebBrowser->>SystemUser: 7. 7. Displays the positioned popover with the help text.
    SystemUser->>WebBrowser: 8. 8. Clicks on an area outside the PopoverComponent.
    WebBrowser->>CentralManagementPlaneUI: 9. 9. Dispatches 'onClick' event to the document-level event listener.
    CentralManagementPlaneUI->>CentralManagementPlaneUI: 10. 10. The event listener handler sets isPopoverOpen state to false.
    CentralManagementPlaneUI->>WebBrowser: 11. 11. React unmounts the PopoverComponent, removing it from the DOM.
    WebBrowser->>SystemUser: 12. 12. Hides the popover.

    note over CentralManagementPlaneUI: Accessibility (WCAG 2.1 AA): The help icon must be a <button> element. It must be focusable via k...
    note over CentralManagementPlaneUI: Responsiveness: The popover's rendering logic must include checks against window.innerWidth and e...
    note over WebBrowser: Hover Interaction: An alternative interaction pattern is to show a tooltip on hover. This can be ...
    note over CentralManagementPlaneUI: Graceful Degradation: If the i18n lookup in step 4 fails to find a translation key, the HelpIconC...

    deactivate CentralManagementPlaneUI
