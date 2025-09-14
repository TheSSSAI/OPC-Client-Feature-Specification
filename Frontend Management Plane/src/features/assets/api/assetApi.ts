import { apiSlice } from '../../../shared/api/apiSlice';
import { Asset, AssetNode } from '../types';

/**
 * @description
 * This file defines the RTK Query API slice for asset management.
 * It injects endpoints into the main `apiSlice` for handling all asset-related
 * CRUD operations, ensuring a centralized and consistent approach to data fetching and caching.
 *
 * @architecture
 * - A Level 4 component within the 'assets' feature slice.
 * - Extends the `apiSlice` from Level 3.
 * - Provides hooks (`useGetAssetsQuery`, `useUpdateAssetMutation`, etc.) that will be consumed
 *   by UI components at Level 4 and above.
 * - Uses a 'Asset' tag to manage cache invalidation for all asset-related data.
 */
export const assetApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    /**
     * @description
     * Fetches the entire asset hierarchy for the current tenant.
     * Returns a flat list of assets, which should be transformed into a tree structure
     * on the client-side for display. This approach is often more performant and flexible.
     */
    getAssets: builder.query<Asset[], void>({
      query: () => '/assets',
      providesTags: (result) =>
        result
          ? [
              ...result.map(({ id }) => ({ type: 'Asset' as const, id })),
              { type: 'Asset', id: 'LIST' },
            ]
          : [{ type: 'Asset', id: 'LIST' }],
    }),

    /**
     * @description
     * Fetches details for a single asset.
     */
    getAssetById: builder.query<Asset, string>({
      query: (id) => `/assets/${id}`,
      providesTags: (result, error, id) => [{ type: 'Asset', id }],
    }),

    /**
     * @description
     * Creates a new asset. `parentId` can be null for root assets.
     */
    createAsset: builder.mutation<Asset, { name: string; parentId: string | null }>({
      query: (newAsset) => ({
        url: '/assets',
        method: 'POST',
        body: newAsset,
      }),
      invalidatesTags: [{ type: 'Asset', id: 'LIST' }],
    }),

    /**
     * @description
     * Updates an existing asset's details, such as its name or parent.
     */
    updateAsset: builder.mutation<Asset, { id: string; name?: string; parentId?: string | null }>({
      query: ({ id, ...patch }) => ({
        url: `/assets/${id}`,
        method: 'PATCH',
        body: patch,
      }),
      invalidatesTags: (result, error, { id }) => [{ type: 'Asset', id }, { type: 'Asset', id: 'LIST' }],
    }),
    
    /**
     * @description
     * Deletes an asset. The backend is expected to handle recursive deletion of children.
     */
    deleteAsset: builder.mutation<{ success: boolean; id: string }, string>({
      query: (id) => ({
        url: `/assets/${id}`,
        method: 'DELETE',
      }),
      invalidatesTags: [{ type: 'Asset', id: 'LIST' }],
    }),
  }),
});

// Export hooks for use in functional components
export const {
  useGetAssetsQuery,
  useGetAssetByIdQuery,
  useCreateAssetMutation,
  useUpdateAssetMutation,
  useDeleteAssetMutation,
} = assetApi;