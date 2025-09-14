import React, { Suspense } from 'react';
import {
  createBrowserRouter,
  Navigate,
  Outlet,
  useLocation,
} from 'react-router-dom';
import { useAuth } from '../shared/hooks/useAuth';
import PageLayout from '../shared/ui/PageLayout';
import { CircularProgress, Box } from '@mui/material';

// Lazy-load feature pages for route-based code splitting.
// This improves initial load performance by only downloading the code needed for the current view.
const DashboardPage = React.lazy(
  () => import('../features/dashboard/pages/DashboardPage')
);
const AssetsPage = React.lazy(
  () => import('../features/assets/pages/AssetsPage')
);
const AlarmsPage = React.lazy(
  () => import('../features/alarms/pages/AlarmsPage')
);
const UserManagementPage = React.lazy(
  () => import('../features/userManagement/pages/UserManagementPage')
);
const LoginPage = React.lazy(
  () => import('../features/authentication/pages/LoginPage')
);
const AuthCallbackPage = React.lazy(
  () => import('../features/authentication/pages/AuthCallbackPage')
);
const NotFoundPage = React.lazy(
  () => import('../features/ui/pages/NotFoundPage')
);
const SettingsPage = React.lazy(
  () => import('../features/settings/pages/SettingsPage')
);

const LoadingFallback = () => (
  <Box
    display="flex"
    justifyContent="center"
    alignItems="center"
    height="100vh"
  >
    <CircularProgress />
  </Box>
);

/**
 * A component that protects routes requiring authentication.
 * It uses the `useAuth` hook to check the user's authentication status.
 * - If the session is still loading, it shows a loading indicator.
 * - If the user is not authenticated, it redirects them to the login page,
 *   preserving the intended destination for redirection after login.
 * - If the user is authenticated, it renders the child routes.
 */
const ProtectedRoute = () => {
  const { isAuthenticated, isLoading } = useAuth();
  const location = useLocation();

  if (isLoading) {
    return <LoadingFallback />;
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return <Outlet />;
};

/**
 * The main application layout for all authenticated routes.
 * It wraps the content of each page with shared UI elements like
 * a navigation bar, header, etc., provided by the PageLayout component.
 */
const MainLayout = () => {
  return (
    <PageLayout>
      <Suspense fallback={<LoadingFallback />}>
        <Outlet />
      </Suspense>
    </PageLayout>
  );
};

/**
 * Defines the application's routing structure using `react-router-dom`.
 * It includes public routes, authentication-related routes, and a nested
 * structure for protected routes that are rendered within the MainLayout.
 */
const router = createBrowserRouter([
  {
    path: '/login',
    element: (
      <Suspense fallback={<LoadingFallback />}>
        <LoginPage />
      </Suspense>
    ),
  },
  {
    path: '/auth/callback',
    element: (
      <Suspense fallback={<LoadingFallback />}>
        <AuthCallbackPage />
      </Suspense>
    ),
  },
  {
    path: '/',
    element: <ProtectedRoute />, // Protects all nested routes
    children: [
      {
        element: <MainLayout />,
        children: [
          { index: true, element: <Navigate to="/dashboard" replace /> },
          { path: 'dashboard', element: <DashboardPage /> },
          { path: 'assets', element: <AssetsPage /> },
          { path: 'alarms', element: <AlarmsPage /> },
          { path: 'users', element: <UserManagementPage /> },
          { path: 'settings/*', element: <SettingsPage /> },
          // Add other feature routes here
        ],
      },
    ],
  },
  {
    path: '*',
    element: (
      <Suspense fallback={<LoadingFallback />}>
        <NotFoundPage />
      </Suspense>
    ),
  },
]);

export default router;