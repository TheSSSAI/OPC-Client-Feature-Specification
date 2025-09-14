import { apiSlice } from '../../../shared/api/apiSlice';
import { Layout } from 'react-grid-layout';

// Define the type for a dashboard configuration
export interface DashboardConfig {
  id: string; // Could be 'default' or a user-specific ID
  layout: Layout[];
  widgets: { i: string; component: string; config: any }[];
}

/**
 * @description
 * Defines the RTK Query API slice for dashboard configurations.
 * It provides endpoints for fetching and saving user-specific dashboard layouts,
 * enabling the personalization features of the system.
 *
 * @architecture
 * - A Level 4 component within the 'dashboard' feature slice.
 * - Extends the `apiSlice` from Level 3.
 * - Manages caching of dashboard layouts using the 'Dashboard' tag.
 * - Consumed by the `CustomizableDashboard` component.
 */
export const dashboardApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    /**
     * @description
     * Fetches the dashboard configuration for a given dashboard ID.
     * In a real application, this ID might be user-specific or a shared dashboard ID.
     */
    getDashboard: builder.query<DashboardConfig, string>({
      query: (dashboardId) => `/dashboards/${dashboardId}`,
      providesTags: (result, error, id) => [{ type: 'Dashboard', id }],
      // Provide a default empty dashboard if the backend returns a 404
      transformErrorResponse: (response) => {
        if (response.status === 404) {
          return {
            id: 'new',
            layout: [],
            widgets: [],
          };
        }
        return response;
      }
    }),

    /**
     * @description
     * Saves or updates a dashboard configuration.
     * The backend should handle create vs. update logic (upsert).
     */
    saveDashboard: builder.mutation<DashboardConfig, Partial<DashboardConfig> & { id: string }>({
      query: (dashboardConfig) => ({
        url: `/dashboards/${dashboardConfig.id}`,
        method: 'PUT',
        body: dashboardConfig,
      }),
      invalidatesTags: (result, error, { id }) => [{ type: 'Dashboard', id }],
    }),
  }),
});

export const { useGetDashboardQuery, useSaveDashboardMutation } = dashboardApi;