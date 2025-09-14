import React from 'react';
import ReactDOM from 'react-dom/client';
import { Provider } from 'react-redux';
import { RouterProvider } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import CssBaseline from '@mui/material/CssBaseline';

import { store } from './store';
import { router } from './router';
import { theme } from '../styles/theme';
// Assuming an ErrorBoundary component is defined in shared/ui
// import ErrorBoundary from '../shared/ui/ErrorBoundary'; 

// Application entry point.
// This file is responsible for setting up the top-level providers
// and rendering the main application component into the DOM.
const rootElement = document.getElementById('root');
if (!rootElement) {
  throw new Error("Failed to find the root element. The 'root' div is missing from index.html.");
}

const root = ReactDOM.createRoot(rootElement);

// The order of providers is important:
// 1. React.StrictMode: For highlighting potential problems in an application.
// 2. Redux Provider: Makes the Redux store available to the entire app.
// 3. MUI ThemeProvider: Provides the custom theme to all Material-UI components.
// 4. CssBaseline: A MUI component that kicks off an elegant, consistent, and simple baseline to build upon.
// 5. RouterProvider: Provides the routing context to the application.
root.render(
  <React.StrictMode>
    {/* <ErrorBoundary> */}
      <Provider store={store}>
        <ThemeProvider theme={theme}>
          {/* CssBaseline is a lightweight reset for styles */}
          <CssBaseline />
          <RouterProvider router={router} />
        </ThemeProvider>
      </Provider>
    {/* </ErrorBoundary> */}
  </React.StrictMode>,
);