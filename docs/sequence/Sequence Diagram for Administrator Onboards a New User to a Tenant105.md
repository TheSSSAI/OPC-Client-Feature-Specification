sequenceDiagram
    actor "User (Actor)" as UserActor
    participant "MuiPopover (Material-UI)" as MuiPopoverMaterialUI
    actor "MuiPopover (Material-UI)" as MuiPopoverMaterialUI
    participant "MuiPopover (Material-UI)" as MuiPopoverMaterialUI

    activate MuiPopoverMaterialUI
    UserActor->>MuiPopoverMaterialUI: 1. 1. Clicks help icon or focuses and presses 'Enter'
    MuiPopoverMaterialUI->>MuiPopoverMaterialUI: 2. 2. Handles event and updates state
    MuiPopoverMaterialUI->>MuiPopoverMaterialUI: 3. 3. Get localized help string
    MuiPopoverMaterialUI-->>MuiPopoverMaterialUI: 4. Returns localized string (or fallback)
    MuiPopoverMaterialUI->>MuiPopoverMaterialUI: 5. 5. Renders popover component with content
    MuiPopoverMaterialUI->>UserActor: 6. 6. Displays popover with contextual help
    UserActor->>MuiPopoverMaterialUI: 7. 7. Clicks outside popover or presses 'Escape' key
    MuiPopoverMaterialUI->>MuiPopoverMaterialUI: 8. 8. Invokes close handler and updates state

    note over MuiPopoverMaterialUI: As per AC-006, if the 'helpTextKey' prop is not provided or is invalid, the HelpIconComponent sho...
    note over MuiPopoverMaterialUI: As per BR-001, the I18nProvider must be configured with a fallback language (English) to ensure t...

    deactivate MuiPopoverMaterialUI
