import { apiSlice } from '../../../shared/api/apiSlice';
import { Alarm, AlarmSummary } from '../types';

/**
 * @description
 * This file defines the RTK Query API slice for the Alarms & Events feature.
 * It includes endpoints for fetching active alarms, acknowledging alarms, and shelving alarms.
 * It also demonstrates how to integrate WebSockets for real-time updates using the
 * `onCacheEntryAdded` lifecycle hook, which is a key pattern for this application.
 *
 * @architecture
 * - A Level 4 component within the 'alarms' feature slice.
 * - Extends the `apiSlice` from Level 3.
 * - Uses 'Alarm' tags for cache management.
 * - Provides a clear example of combining RESTful queries with real-time WebSocket updates.
 */
export const alarmApi = apiSlice.injectEndpoints({
  endpoints: (builder) => ({
    /**
     * @description
     * Fetches the initial list of active alarms. Subsequent updates are pushed via WebSocket.
     */
    getActiveAlarms: builder.query<Alarm[], void>({
      query: () => '/alarms/active',
      providesTags: (result) =>
        result
          ? [...result.map(({ id }) => ({ type: 'Alarm' as const, id })), { type: 'Alarm', id: 'LIST' }]
          : [{ type: 'Alarm', id: 'LIST' }],
      
      // Real-time WebSocket integration
      async onCacheEntryAdded(arg, { updateCachedData, cacheDataLoaded, cacheEntryRemoved }) {
        // Create a WebSocket connection when the first subscriber appears
        const ws = new WebSocket('wss://api.example.com/ws/alarms'); // URL from environment config
        try {
          // Wait for the initial query to resolve
          await cacheDataLoaded;

          // When a message is received from the WebSocket, update the cache
          const listener = (event: MessageEvent) => {
            const data = JSON.parse(event.data);
            // Assuming the WS pushes 'alarmCreated', 'alarmUpdated', 'alarmCleared' events
            updateCachedData((draft) => {
              if (data.type === 'alarmCreated') {
                draft.push(data.payload);
              }
              if (data.type === 'alarmUpdated') {
                const index = draft.findIndex(alarm => alarm.id === data.payload.id);
                if (index !== -1) draft[index] = { ...draft[index], ...data.payload };
              }
              if (data.type === 'alarmCleared') {
                return draft.filter(alarm => alarm.id !== data.payload.id);
              }
            });
          };

          ws.addEventListener('message', listener);
        } catch {
          // No-op if the connection fails
        }
        // The cache entry is removed when the last subscriber unsubscribes
        await cacheEntryRemoved;
        ws.close();
      },
    }),

    /**
     * @description
     * Fetches a summary of alarm counts (e.g., by priority).
     */
    getAlarmSummary: builder.query<AlarmSummary, void>({
      query: () => '/alarms/summary',
      providesTags: ['AlarmSummary'],
    }),

    /**
     * @description
     * Acknowledges one or more alarms.
     */
    acknowledgeAlarms: builder.mutation<{ success: boolean }, string[]>({
      query: (alarmIds) => ({
        url: '/alarms/acknowledge',
        method: 'POST',
        body: { alarmIds },
      }),
      // Optimistic update: we assume the ack will succeed and update the cache immediately
      async onQueryStarted(alarmIds, { dispatch, queryFulfilled }) {
        const patchResult = dispatch(
          alarmApi.util.updateQueryData('getActiveAlarms', undefined, (draft) => {
            alarmIds.forEach(id => {
                const alarm = draft.find(a => a.id === id);
                if (alarm) alarm.isAcknowledged = true;
            });
          })
        );
        try {
          await queryFulfilled;
        } catch {
          patchResult.undo(); // Revert on failure
        }
      },
      invalidatesTags: ['AlarmSummary'], // Refresh summary counts
    }),

    /**
     * @description
     * Shelves an alarm for a specified duration with a justification.
     */
    shelveAlarm: builder.mutation<{ success: boolean }, { alarmId: string; durationMinutes: number; justification: string }>({
        query: ({ alarmId, ...body }) => ({
            url: `/alarms/${alarmId}/shelve`,
            method: 'POST',
            body: body,
        }),
        invalidatesTags: (result, error, { alarmId }) => [{ type: 'Alarm', id: alarmId }, { type: 'Alarm', id: 'LIST' }, 'AlarmSummary'],
    }),
  }),
});

export const {
  useGetActiveAlarmsQuery,
  useGetAlarmSummaryQuery,
  useAcknowledgeAlarmsMutation,
  useShelveAlarmMutation,
} = alarmApi;