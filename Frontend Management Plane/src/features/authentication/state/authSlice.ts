import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { RootState } from '../../../app/store';

// Define the shape of the user object
interface UserProfile {
  id: string;
  name: string;
  email: string;
  roles: string[];
}

// Define the state for this slice
interface AuthState {
  isAuthenticated: boolean;
  user: UserProfile | null;
  token: string | null;
  isLoading: boolean;
}

// Define the initial state
const initialState: AuthState = {
  isAuthenticated: false,
  user: null,
  token: null,
  isLoading: true, // Assume loading until the first check is complete
};

/**
 * @description
 * The authSlice manages the global authentication state of the application using Redux Toolkit.
 * It provides reducers to handle user login and logout, and selectors to access auth state
 * from any component in the application.
 *
 * @architecture
 * - This slice is a core part of the 'authentication' feature slice.
 * - It is a Level 4 component because it defines the state shape and logic for a specific feature.
 * - It will be combined with other reducers in the main Redux store configuration at Level 5.
 * - Actions from this slice are dispatched by components like `AuthCallback` and hooks like `useAuth`.
 */
export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    // Action to set the user upon successful login
    setUser: (state, action: PayloadAction<{ id: string; name: string; email: string; token: string; roles: string[] }>) => {
      state.isAuthenticated = true;
      state.user = {
        id: action.payload.id,
        name: action.payload.name,
        email: action.payload.email,
        roles: action.payload.roles,
      };
      state.token = action.payload.token;
      state.isLoading = false;
    },
    // Action to clear user data on logout or session expiry
    clearUser: (state) => {
      state.isAuthenticated = false;
      state.user = null;
      state.token = null;
      state.isLoading = false;
    },
    // Action to set loading state, e.g., during silent renew
    setAuthLoading: (state, action: PayloadAction<boolean>) => {
        state.isLoading = action.payload;
    }
  },
});

// Export actions for use in other parts of the application
export const { setUser, clearUser, setAuthLoading } = authSlice.actions;

// Selectors to access parts of the auth state
export const selectIsAuthenticated = (state: RootState) => state.auth.isAuthenticated;
export const selectUser = (state: RootState) => state.auth.user;
export const selectAuthToken = (state: RootState) => state.auth.token;
export const selectAuthIsLoading = (state: RootState) => state.auth.isLoading;
export const selectUserRoles = (state: RootState) => state.auth.user?.roles || [];


// Export the reducer to be included in the store
export default authSlice.reducer;