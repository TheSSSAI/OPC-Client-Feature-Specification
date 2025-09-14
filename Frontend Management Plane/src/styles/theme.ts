/**
 * @file Defines the Material-UI theme for the entire application.
 * This includes color palettes for both light and dark modes, typography settings,
 * and global component style overrides. This centralization ensures a consistent
 * look and feel across all features and components.
 * Fulfills REQ-IFC-001 (Light/Dark theme) and REQ-1-070 (Modern and intuitive design).
 */

import { createTheme, ThemeOptions } from '@mui/material/styles';

// Common theme settings shared between light and dark modes
const commonSettings: ThemeOptions = {
  typography: {
    fontFamily: '"Roboto", "Helvetica", "Arial", sans-serif',
    h1: { fontSize: '2.5rem', fontWeight: 500 },
    h2: { fontSize: '2rem', fontWeight: 500 },
    h3: { fontSize: '1.75rem', fontWeight: 500 },
    h4: { fontSize: '1.5rem', fontWeight: 500 },
    h5: { fontSize: '1.25rem', fontWeight: 500 },
    h6: { fontSize: '1.1rem', fontWeight: 500 },
  },
  shape: {
    borderRadius: 8,
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          textTransform: 'none',
          fontWeight: 600,
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          backgroundImage: 'none', // Disables MUI's default gradient background image
        },
      },
    },
    MuiTooltip: {
      styleOverrides: {
        tooltip: {
          fontSize: '0.875rem',
        },
      },
    },
  },
};

// Light Theme Palette - Professional and clean for industrial applications
export const lightTheme = createTheme({
  ...commonSettings,
  palette: {
    mode: 'light',
    primary: {
      main: '#00529B', // A strong, professional blue
      light: '#4F7DCB',
      dark: '#002B6C',
      contrastText: '#ffffff',
    },
    secondary: {
      main: '#4CAF50', // A clear, positive green for secondary actions
      light: '#80E27E',
      dark: '#087F23',
      contrastText: '#000000',
    },
    background: {
      default: '#F4F6F8',
      paper: '#FFFFFF',
    },
    text: {
      primary: '#172B4D',
      secondary: '#5E6C84',
    },
    error: { main: '#D32F2F' },
    warning: { main: '#FFA000' },
    info: { main: '#1976D2' },
    success: { main: '#388E3C' },
  },
});

// Dark Theme Palette - Designed for low-light environments like control rooms
export const darkTheme = createTheme({
  ...commonSettings,
  palette: {
    mode: 'dark',
    primary: {
      main: '#4dabf5', // A lighter blue for better contrast on dark backgrounds
      light: '#81d4fa',
      dark: '#007bbf',
      contrastText: '#000000',
    },
    secondary: {
      main: '#81C784', // A lighter green
      light: '#B2FAB4',
      dark: '#519657',
      contrastText: '#000000',
    },
    background: {
      default: '#121212',
      paper: '#1E1E1E',
    },
    text: {
      primary: '#E0E0E0',
      secondary: '#B0B0B0',
    },
    error: { main: '#FF5252' },
    warning: { main: '#FFC107' },
    info: { main: '#29B6F6' },
    success: { main: '#66BB6A' },
  },
});