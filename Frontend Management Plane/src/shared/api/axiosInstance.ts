import axios, { AxiosInstance, InternalAxiosRequestConfig } from 'axios';
import { authService } from '../../features/authentication/services/authService';

/**
 * A pre-configured Axios instance for making API calls to the system's backend gateway.
 * This singleton instance includes interceptors for automatically attaching the
 * JWT bearer token to requests and for handling global error responses, such as 401 Unauthorized.
 *
 * @see REQ-1-072 - All services exposed to external clients or the frontend must use REST APIs.
 * @see REQ-1-080 - All API endpoints must be secured, requiring a valid JWT Bearer Token.
 */

// --- Configuration Validation ---
if (!import.meta.env.VITE_API_BASE_URL) {
  throw new Error(
    'FATAL: Missing required environment variable "VITE_API_BASE_URL". Please check your .env configuration.'
  );
}

const axiosInstance: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL as string,
  headers: {
    'Content-Type': 'application/json',
  },
});

/**
 * Axios Request Interceptor
 * This function is executed before any request is sent. Its primary purposes are:
 * 1. To retrieve the current user's access token from the authentication service.
 * 2. To attach the access token to the request's 'Authorization' header as a Bearer token.
 * This ensures that every API call sent to the backend is properly authenticated.
 *
 * @see Sequence 77 - Secure API Request Flow. The interceptor performs the first part of Security Checkpoint 1.
 */
axiosInstance.interceptors.request.use(
  async (config: InternalAxiosRequestConfig) => {
    const user = await authService.getUser();

    if (user && !user.expired) {
      config.headers.Authorization = `Bearer ${user.access_token}`;
    }
    return config;
  },
  (error) => {
    // This part handles errors that occur during the request setup phase.
    console.error('Axios request interceptor error:', error);
    return Promise.reject(error);
  }
);

/**
 * Axios Response Interceptor
 * This function is executed for every response received from the API. Its primary purpose is
 * to handle global error conditions, particularly authentication failures.
 * If a 401 Unauthorized response is received, it means the user's session is no longer valid
 * (e.g., token expired, revoked), and they must be logged out.
 */
axiosInstance.interceptors.response.use(
  // For successful responses (2xx status codes), just pass them through.
  (response) => response,

  // For error responses, inspect the error and handle globally if necessary.
  (error) => {
    // Check if the error is an Axios error and has a response from the server
    if (axios.isAxiosError(error) && error.response) {
      const { status } = error.response;

      if (status === 401) {
        // A 401 Unauthorized error is a critical, global condition.
        // The user's token is invalid or expired. The best course of action
        // is to terminate the session and redirect to the login page.
        console.warn(
          'Received 401 Unauthorized response. Terminating session.'
        );

        // We use signoutRedirect with a specific state to indicate an automatic logout.
        // The application can use this state on the landing page to show a message like
        // "Your session has expired. Please log in again."
        authService.signOutRedirect();
      }
    }

    // For all other errors, we reject the promise so that the calling code
    // (e.g., an RTK Query hook) can handle it specifically.
    return Promise.reject(error);
  }
);

export default axiosInstance;