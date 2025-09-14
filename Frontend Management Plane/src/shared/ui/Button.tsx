import * as React from 'react';
import MuiButton, { ButtonProps as MuiButtonProps } from '@mui/material/Button';
import CircularProgress from '@mui/material/CircularProgress';
import { styled } from '@mui/material/styles';

export interface ButtonProps extends MuiButtonProps {
  /**
   * If `true`, a loading indicator is shown.
   * @default false
   */
  isLoading?: boolean;
}

const StyledButton = styled(MuiButton)<ButtonProps>(({ theme, color, variant }) => ({
  // Custom styling can be added here if needed to override theme defaults
  // For example, a destructive button variant
  ...(color === 'error' &&
    variant === 'contained' && {
      backgroundColor: theme.palette.error.dark,
      '&:hover': {
        backgroundColor: theme.palette.error.main,
      },
    }),
}));

/**
 * A shared Button component that wraps Material-UI's Button.
 * It includes an `isLoading` prop to display a loading spinner,
 * which is a common requirement in the application.
 */
export const Button = React.forwardRef<HTMLButtonElement, ButtonProps>(
  ({ children, isLoading = false, disabled, sx, ...rest }, ref) => {
    return (
      <StyledButton
        ref={ref}
        disabled={isLoading || disabled}
        sx={{
          position: 'relative',
          ...sx,
        }}
        {...rest}
      >
        <span
          style={{
            visibility: isLoading ? 'hidden' : 'visible',
            display: 'flex',
            alignItems: 'center',
          }}
        >
          {children}
        </span>
        {isLoading && (
          <CircularProgress
            size={24}
            sx={{
              color: 'inherit',
              position: 'absolute',
              top: '50%',
              left: '50%',
              marginTop: '-12px',
              marginLeft: '-12px',
            }}
          />
        )}
      </StyledButton>
    );
  }
);

Button.displayName = 'Button';