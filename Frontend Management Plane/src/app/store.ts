import { configureStore } from '@reduxjs/toolkit';
import { setupListeners } from '@reduxjs/toolkit/query';
import { apiSlice } from '../shared/api/apiSlice';
import authReducer from '../features/authentication/state/authSlice';
import uiReducer from '../features/ui/state/uiSlice';
import dashboardReducer from '../features/dashboard/state/dashboardSlice';

/**
 * Configures and creates the central Redux store for the application.
 * This function brings together all the feature-specific reducers and the
 * middleware required for handling asynchronous operations, particularly
 * data fetching with RTK Query.
 *
 * The store is structured as follows:
 * - Reducers:
 *   - [apiSlice.reducerPath]: Manages the cache and state for all RTK Query endpoints.
 *   - auth: Manages user authentication state, including tokens and user info.
 *   - ui: Manages global UI state such as theme, notifications, and loading indicators.
 *   - dashboard: Manages the state for customizable dashboards, like widget layouts.
 * - Middleware:
 *   - Includes default middleware from Redux Toolkit (thunk, immutability checks, etc.).
 *   - Adds the apiSlice.middleware to enable caching, invalidation, and polling for RTK Query.
 */
export const store = configureStore({
  reducer: {
    [apiSlice.reducerPath]: apiSlice.reducer,
    auth: authReducer,
    ui: uiReducer,
    dashboard: dashboardReducer,
  },
  // Adding the api middleware enables caching, invalidation, polling,
  // and other useful features of `rtk-query`.
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(apiSlice.middleware),
});

/**
 * A utility function from RTK Query that sets up listeners for query lifecycle events.
 * This is optional but recommended for features like `refetchOnFocus` and
 * `refetchOnReconnect`, which automatically keep the cached data up-to-date.
 */
setupListeners(store.dispatch);

/**
 * Type alias for the root state of the Redux store.
 * This is inferred from the store itself, so it automatically updates as new
 * slices are added. It's used for typing hooks like `useSelector`.
 * e.g., `const user = useSelector((state: RootState) => state.auth.user);`
 */
export type RootState = ReturnType<typeof store.getState>;

/**
 * Type alias for the dispatch function of the Redux store.
 * This provides strong typing for dispatching actions, including thunks.
 * It's used for typing hooks like `useDispatch`.
 * e.g., `const dispatch = useDispatch<AppDispatch>();`
 */
export type AppDispatch = typeof store.dispatch;