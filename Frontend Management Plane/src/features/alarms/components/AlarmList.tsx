import React, { useState, useMemo } from 'react';
import { DataGrid, GridColDef, GridRowSelectionModel } from '@mui/x-data-grid';
import { Box, CircularProgress, Alert, Button, Chip, Typography, Toolbar } from '@mui/material';
import { useGetActiveAlarmsQuery, useAcknowledgeAlarmsMutation } from '../api/alarmApi';
import { Alarm, AlarmPriority } from '../types';
import { formatDistanceToNow } from 'date-fns';

const priorityColorMap: Record<AlarmPriority, 'error' | 'warning' | 'info' | 'default'> = {
  Critical: 'error',
  High: 'warning',
  Medium: 'info',
  Low: 'default',
};

const columns: GridColDef<Alarm>[] = [
  {
    field: 'priority',
    headerName: 'Priority',
    width: 120,
    renderCell: (params) => (
      <Chip
        label={params.value}
        color={priorityColorMap[params.value as AlarmPriority]}
        size="small"
      />
    ),
  },
  { 
    field: 'timestamp', 
    headerName: 'Timestamp', 
    width: 180,
    valueGetter: (value: string) => new Date(value),
    renderCell: (params) => `${params.value.toLocaleString()} (${formatDistanceToNow(params.value, { addSuffix: true })})`,
  },
  { field: 'source', headerName: 'Source (Asset/Tag)', flex: 1 },
  { field: 'condition', headerName: 'Condition', width: 150 },
  { field: 'value', headerName: 'Value', width: 120,
    renderCell: (params) => <Typography variant="body2" sx={{ fontFamily: 'monospace' }}>{params.value}</Typography>
  },
  { 
    field: 'status', 
    headerName: 'Status', 
    width: 150,
    renderCell: (params) => {
        if(params.row.isShelved) return <Chip label="Shelved" size="small" color="secondary" />;
        if(params.row.isAcknowledged) return <Chip label="Acknowledged" size="small" variant="outlined" />;
        return <Chip label="Active" size="small" color="primary" variant="filled" />;
    }
  },
];


/**
 * @description
 * Displays a real-time, filterable, and sortable list of active alarms.
 * This is a critical component for operators, providing immediate situational awareness.
 *
 * @architecture
 * - A Level 4 component within the 'alarms' feature slice.
 * - Consumes the `useGetActiveAlarmsQuery` hook, which provides both the initial list
 *   and real-time updates via a WebSocket connection managed within the API slice.
 * - Uses the `DataGrid` component from `@mui/x-data-grid` for robust table functionality.
 * - Implements logic for bulk actions like acknowledging multiple alarms.
 * - Note: for extreme performance (>1000s of alarms), this component should be enhanced with
 *   list virtualization (`react-window`). The `DataGrid` has some built-in virtualization.
 */
const AlarmList: React.FC = () => {
  const { data: alarms = [], isLoading, isError, error } = useGetActiveAlarmsQuery();
  const [acknowledgeAlarms, { isLoading: isAcknowledging }] = useAcknowledgeAlarmsMutation();
  const [selectedAlarmIds, setSelectedAlarmIds] = useState<GridRowSelectionModel>([]);

  const handleAcknowledgeSelected = async () => {
    if (selectedAlarmIds.length > 0) {
      try {
        await acknowledgeAlarms(selectedAlarmIds as string[]).unwrap();
        setSelectedAlarmIds([]); // Clear selection on success
      } catch (err) {
        // In a real app, show a toast notification with the error
        console.error('Failed to acknowledge alarms:', err);
      }
    }
  };

  const unacknowledgedAlarms = useMemo(() => {
    return alarms.filter(alarm => !alarm.isAcknowledged && !alarm.isShelved);
  }, [alarms]);


  if (isLoading) {
    return <CircularProgress />;
  }
  if (isError) {
    return <Alert severity="error">Failed to load alarms: {JSON.stringify(error)}</Alert>;
  }

  return (
    <Box sx={{ height: 'calc(100vh - 200px)', width: '100%' }}>
      <Toolbar disableGutters sx={{ p: 1 }}>
        <Button
            variant="contained"
            onClick={handleAcknowledgeSelected}
            disabled={selectedAlarmIds.length === 0 || isAcknowledging}
        >
            Acknowledge Selected ({selectedAlarmIds.length})
        </Button>
        {isAcknowledging && <CircularProgress size={24} sx={{ ml: 2 }} />}
      </Toolbar>
      <DataGrid
        rows={unacknowledgedAlarms}
        columns={columns}
        checkboxSelection
        onRowSelectionModelChange={(newSelectionModel) => {
          setSelectedAlarmIds(newSelectionModel);
        }}
        rowSelectionModel={selectedAlarmIds}
        loading={isLoading}
        initialState={{
            sorting: {
              sortModel: [{ field: 'timestamp', sort: 'desc' }],
            },
        }}
        density="compact"
        sx={{
            border: 0,
            '& .MuiDataGrid-row.Mui-selected': {
                backgroundColor: 'action.selected',
                '&:hover': {
                    backgroundColor: 'action.hover',
                }
            }
        }}
      />
    </Box>
  );
};

export default AlarmList;