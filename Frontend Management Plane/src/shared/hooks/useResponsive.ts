import { useTheme, Breakpoint } from '@mui/material/styles';
import useMediaQuery from '@mui/material/useMediaQuery';

type Query = 'up' | 'down' | 'between' | 'only';
type BreakpointOrNumber = Breakpoint | number;

/**
 * A custom hook for easily checking Material-UI breakpoints.
 * This helps in creating responsive layouts programmatically.
 * Fulfills the need for responsiveness as per REQ-1-070.
 *
 * @param query - The type of media query to perform.
 * @param start - The starting breakpoint or pixel value.
 * @param end - The ending breakpoint or pixel value (only for 'between').
 * @returns `true` if the media query matches, otherwise `false`.
 */
export function useResponsive(query: Query, start: BreakpointOrNumber, end?: BreakpointOrNumber) {
  const theme = useTheme();

  const mediaUp = useMediaQuery(theme.breakpoints.up(start as BreakpointOrNumber));
  const mediaDown = useMediaQuery(theme.breakpoints.down(start as BreakpointOrNumber));
  const mediaBetween = useMediaQuery(
    theme.breakpoints.between(start as BreakpointOrNumber, (end || 'xl') as BreakpointOrNumber)
  );
  const mediaOnly = useMediaQuery(theme.breakpoints.only(start as Breakpoint));

  if (query === 'up') {
    return mediaUp;
  }

  if (query === 'down') {
    return mediaDown;
  }

  if (query === 'between') {
    return mediaBetween;
  }

  return mediaOnly;
}

/**
 * A convenience hook that returns boolean flags for common screen sizes.
 * @returns An object with boolean flags: isXs, isSm, isMd, isLg, isXl.
 */
export function useBreakpoints() {
  const isXs = useResponsive('down', 'sm');
  const isSm = useResponsive('between', 'sm', 'md');
  const isMd = useResponsive('between', 'md', 'lg');
  const isLg = useResponsive('between', 'lg', 'xl');
  const isXl = useResponsive('up', 'xl');

  return {
    isXs,
    isSm,
    isMd,
    isLg,
    isXl,
  };
}