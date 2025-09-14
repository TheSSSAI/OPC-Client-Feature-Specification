import { useMemo } from 'react';
import { useSelector } from 'react-redux';
import { RootState } from '../../app/store';

/**
 * A custom hook to provide convenient access to the authentication state.
 *
 * This hook encapsulates the logic of selecting the authentication slice from
 * the Redux store. It provides a clean, memoized interface for components
 * to consume authentication information like the user object, authentication status,
 * and loading state.
 *
 * Using this hook simplifies components and decouples them from the underlying
 * shape of the Redux state, making the application easier to maintain and refactor.
 *
 * The returned object is memoized to prevent unnecessary re-renders in consumer
 * components when other parts of the Redux state change.
 *
 * @returns An object containing the current authentication state:
 * - `user`: The authenticated user object from OIDC, or null if not authenticated.
 * - `isAuthenticated`: A boolean indicating if the user is currently authenticated.
 * - `isLoading`: A boolean indicating if an authentication process is in progress.
 * - `accessToken`: The JWT access token, or null.
 */
export const useAuth = () => {
  const { user, isLoading, isAuthenticated, accessToken } = useSelector(
    (state: RootState) => state.auth
  );

  return useMemo(
    () => ({
      user,
      isLoading,
      isAuthenticated,
      accessToken,
    }),
    [user, isLoading, isAuthenticated, accessToken]
  );
};