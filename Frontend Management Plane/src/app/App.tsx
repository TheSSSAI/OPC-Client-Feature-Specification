import React from 'react';
import { Provider } from 'react-redux';
import { RouterProvider } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';
import { store } from './store';
import router from './router';
import { lightTheme, darkTheme } from '../styles/theme';
import { useAppSelector } from '../shared/hooks/useAppSelector';
import { AuthProvider } from '../features/authentication/components/AuthProvider';

// A simple Error Boundary to catch rendering errors in child components.
// This prevents a single component crash from taking down the entire application.
class ErrorBoundary extends React.Component<
  { children: React.ReactNode },
  { hasError: boolean }
> {
  constructor(props: { children: React.ReactNode }) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(_: Error) {
    return { hasError: true };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    // In a real application, you would log this error to a service like Sentry or LogRocket.
    console.error('Uncaught error:', error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return <h1>Something went wrong. Please refresh the page.</h1>;
    }
    return this.props.children;
  }
}

/**
 * The component responsible for applying the correct theme based on the Redux state.
 * It selects the theme preference from the `ui` slice and applies the corresponding
 * Material-UI theme object.
 */
const ThemedApp = () => {
  const themeMode = useAppSelector((state) => state.ui.theme);
  const theme = themeMode === 'dark' ? darkTheme : lightTheme;

  return (
    <ThemeProvider theme={theme}>
      {/* CssBaseline kickstarts an elegant, consistent, and simple baseline to build upon. */}
      <CssBaseline />
      <RouterProvider router={router} />
    </ThemeProvider>
  );
};

/**
 * The root component of the application.
 * It establishes the provider hierarchy required for the entire app to function:
 * 1. ErrorBoundary: Catches JavaScript errors anywhere in the app.
 * 2. Redux Provider: Makes the Redux store available to all components.
 * 3. AuthProvider: Manages the authentication state and user session.
 * 4. ThemedApp: Applies the Material-UI theme and renders the router.
 */
const App: React.FC = () => {
  return (
    <React.StrictMode>
      <ErrorBoundary>
        <Provider store={store}>
          <AuthProvider>
            <ThemedApp />
          </AuthProvider>
        </Provider>
      </ErrorBoundary>
    </React.StrictMode>
  );
};

export default App;