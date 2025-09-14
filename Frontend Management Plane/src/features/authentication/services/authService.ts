import { UserManager, User, UserManagerSettings } from 'oidc-client-ts';
import { WebStorageStateStore } from 'oidc-client-ts';

// --- Configuration Validation ---
// Ensure that all required environment variables are present at build time.
// Vite will replace these with string literals during the build.
const requiredEnvVars = [
  'VITE_KEYCLOAK_AUTHORITY',
  'VITE_KEYCLOAK_CLIENT_ID',
  'VITE_APP_BASE_URL',
];

for (const varName of requiredEnvVars) {
  if (!import.meta.env[varName]) {
    throw new Error(
      `FATAL: Missing required environment variable "${varName}". Please check your .env configuration.`
    );
  }
}

/**
 * OIDC client configuration for Keycloak integration.
 * This configuration specifies the details for the OIDC Authorization Code Flow with PKCE.
 * It is crucial for secure user authentication against the identity provider.
 * 
 * @see REQ-1-080 - The system shall use Keycloak as its centralized Identity Provider (IdP).
 * @see Sequence 71 - User Authentication Flow.
 */
const oidcSettings: UserManagerSettings = {
  // The URL of the OIDC provider (Keycloak).
  authority: import.meta.env.VITE_KEYCLOAK_AUTHORITY as string,

  // The client ID registered in Keycloak for this application.
  client_id: import.meta.env.VITE_KEYCLOAK_CLIENT_ID as string,

  // The URL where the user is redirected back to after a successful login.
  redirect_uri: `${import.meta.env.VITE_APP_BASE_URL}/callback`,

  // The URL where the user is redirected back to after a successful logout.
  post_logout_redirect_uri: `${import.meta.env.VITE_APP_BASE_URL}/`,

  // The type of response expected from the authorization endpoint. 'code' enables the Authorization Code Flow.
  response_type: 'code',

  // The scopes define the permissions the application is requesting.
  // 'openid' and 'profile' are standard OIDC scopes. 'email' requests the user's email.
  // 'roles' is a custom scope often used with Keycloak to include user roles in the token.
  scope: 'openid profile email roles',

  // Enables automatic silent token renewal, improving user experience by avoiding frequent logins.
  automaticSilentRenew: true,

  // The interval in seconds to check if the session needs to be renewed.
  silentRequestTimeout: 10000,

  // The URI for the silent renewal iframe.
  silent_redirect_uri: `${import.meta.env.VITE_APP_BASE_URL}/silent-renew.html`,

  // Per security requirements (SDS), tokens should not be stored in localStorage.
  // Using session storage is more secure as it's cleared when the session ends.
  // For highest security, a custom in-memory store could be implemented, but
  // WebStorageStateStore with sessionStorage is a robust default that supports multi-tab scenarios.
  userStore: new WebStorageStateStore({ store: window.sessionStorage }),
};

/**
 * Manages all OIDC interactions with the Keycloak Identity Provider.
 * This instance is a singleton for the application.
 */
const userManager = new UserManager(oidcSettings);

/**
 * Initiates the OIDC login flow.
 * This will redirect the user's browser to the Keycloak login page.
 * Corresponds to step 3 in Sequence 71.
 */
const signInRedirect = (): Promise<void> => {
  console.log('Redirecting to Keycloak for sign-in...');
  return userManager.signinRedirect();
};

/**
 * Handles the OIDC callback after the user has authenticated with Keycloak.
 * This function is called on the redirect URI page (`/callback`). It exchanges the
 * authorization code for an access token, ID token, and refresh token.
 * Corresponds to step 8 in Sequence 71.
 * @returns {Promise<User>} A promise that resolves with the authenticated user object.
 */
const handleSignInCallback = (): Promise<User> => {
  console.log('Handling sign-in callback from Keycloak...');
  return userManager.signinRedirectCallback();
};

/**
 * Initiates the OIDC logout flow.
 * This clears the local user session and redirects the browser to Keycloak to end the session there.
 */
const signOutRedirect = (): Promise<void> => {
  console.log('Redirecting to Keycloak for sign-out...');
  return userManager.signoutRedirect();
};

/**
 * Retrieves the current authenticated user from storage.
 * @returns {Promise<User | null>} A promise that resolves with the user object if authenticated, otherwise null.
 */
const getUser = (): Promise<User | null> => {
  return userManager.getUser();
};

/**
 * Clears the stale user object from storage if the silent renew fails.
 */
const clearStaleUser = (): Promise<void> => {
  console.warn('Clearing stale user session from storage.');
  return userManager.removeUser();
};

// --- Event Handling ---
// Provides hooks into the user manager's lifecycle for logging and debugging.

userManager.events.addUserLoaded((user) => {
  console.log('User loaded:', user.profile.sub);
});

userManager.events.addUserUnloaded(() => {
  console.log('User unloaded/session ended.');
});

userManager.events.addAccessTokenExpiring(() => {
  console.log('Access token is expiring, silent renew will be attempted.');
});

userManager.events.addAccessTokenExpired(() => {
  console.warn('Access token expired. User must be re-authenticated.');
  // In a real app, you might trigger a forced sign-out here.
  // signOutRedirect();
});

userManager.events.addSilentRenewError((error) => {
  console.error('Silent renew error:', error);
  // This is a critical event. The user's session may be lost.
  // We should clear the stale user data to force a re-login on next protected action.
  clearStaleUser();
});

// Export the service functions for use throughout the application.
export const authService = {
  userManager,
  signInRedirect,
  handleSignInCallback,
  signOutRedirect,
  getUser,
  clearStaleUser,
};