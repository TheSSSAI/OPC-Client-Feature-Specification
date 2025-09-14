import * as React from 'react';
import { Tooltip, IconButton, TooltipProps } from '@mui/material';
import HelpOutlineIcon from '@mui/icons-material/HelpOutline';

export interface HelpTooltipProps extends Omit<TooltipProps, 'children'> {
  /**
   * The help text to display inside the tooltip.
   */
  helpText: React.ReactNode;
}

/**
 * A dedicated, accessible help tooltip component that displays a help icon.
 * On hover or focus, it shows a tooltip with the provided help text.
 * This directly implements REQ-IFC-001 and US-064 for context-sensitive help.
 */
export const HelpTooltip: React.FC<HelpTooltipProps> = ({ helpText, ...rest }) => {
  if (!helpText) {
    // Gracefully handle missing help content by not rendering the icon.
    // This fulfills AC-006 of US-064.
    return null;
  }

  return (
    <Tooltip title={helpText} {...rest}>
      {/* The span is needed to prevent Tooltip from complaining about the ref on a disabled button */}
      <span>
        <IconButton
          aria-label="Show help"
          size="small"
          // In a real form, this might be disabled, so the span wrapper helps.
        >
          <HelpOutlineIcon fontSize="inherit" />
        </IconButton>
      </span>
    </Tooltip>
  );
};