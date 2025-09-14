import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { Box, CircularProgress, Typography, Alert, Button } from '@mui/material';

import { authService } from '../services/authService';
import { setUser } from '../state/authSlice';
import { AppDispatch } from '../../../app/store';

/**
 * @description
 * AuthCallback is a functional component responsible for handling the OIDC redirect from the identity provider (Keycloak).
 * It is a "controller" component with no significant UI, rendered on a dedicated route (e.g., /auth/callback).
 * Its sole purpose is to process the authentication response, exchange the authorization code for tokens,
 * update the application's state, and redirect the user to the main application.
 *
 * @architecture
 * - This component is a critical part of the OIDC Authorization Code Flow with PKCE (Sequence 71).
 * - It directly depends on `authService` (Level 2) to interact with the oidc-client-ts library.
 * - It dispatches actions to the Redux store to update the global authentication state.
 * - It uses `react-router-dom`'s `useNavigate` hook for redirection after the flow is complete.
 * - It handles both success and error states of the authentication process, providing feedback to the user.
 */
const AuthCallback = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch<AppDispatch>();
  const [error, setError] = useState<string | null>(null);
  const [isProcessing, setIsProcessing] = useState(true);

  useEffect(() => {
    const processAuthCallback = async () => {
      try {
        const user = await authService.handleSignInCallback();
        if (user && !user.expired) {
          // Dispatch user information to the Redux store to update the global state
          dispatch(setUser({
            id: user.profile.sub,
            name: user.profile.name || 'Unknown',
            email: user.profile.email || 'No email',
            token: user.access_token,
            // You can map roles from user.profile.roles or similar claim
            roles: (user.profile.roles as string[]) || [], 
          }));
          
          // Redirect to the stored path or the application root
          const returnTo = sessionStorage.getItem('post_login_redirect') || '/';
          sessionStorage.removeItem('post_login_redirect');
          navigate(returnTo, { replace: true });

        } else {
          // Handle cases where user is null or expired after callback
          throw new Error('Authentication failed: Invalid user session returned.');
        }
      } catch (e) {
        console.error('Authentication callback error:', e);
        const errorMessage = e instanceof Error ? e.message : 'An unknown error occurred during authentication.';
        setError(`Authentication failed. ${errorMessage}. Please try logging in again.`);
        setIsProcessing(false);
      }
    };

    processAuthCallback();
  }, [navigate, dispatch]);

  const handleRetry = () => {
    setError(null);
    setIsProcessing(true);
    authService.signInRedirect();
  };

  return (
    <Box
      sx={{
        display: 'flex',
        flexDirection: 'column',
        justifyContent: 'center',
        alignItems: 'center',
        height: '100vh',
        bgcolor: 'background.default',
        p: 3,
      }}
    >
      {isProcessing && (
        <>
          <CircularProgress />
          <Typography variant="h6" sx={{ mt: 2 }}>
            Finalizing authentication, please wait...
          </Typography>
        </>
      )}
      {error && (
        <Alert severity="error" sx={{ maxWidth: '600px', width: '100%' }}>
          <Typography variant="h6" gutterBottom>Authentication Error</Typography>
          <Typography variant="body1" sx={{ mb: 2 }}>
            {error}
          </Typography>
          <Button variant="contained" color="primary" onClick={handleRetry}>
            Retry Login
          </Button>
        </Alert>
      )}
    </Box>
  );
};

export default AuthCallback;