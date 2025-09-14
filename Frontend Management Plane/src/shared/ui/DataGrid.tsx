import * as React from 'react';
import { DataGridPro, DataGridProProps, GridColDef, GridRenderCellParams } from '@mui/x-data-grid-pro';
import { Box, LinearProgress, Paper, Typography, Alert } from '@mui/material';

// Assuming a Pro license for advanced features like pinning.
// If not, this can be swapped for DataGrid from @mui/x-data-grid.

export interface DataGridProps extends Omit<DataGridProProps, 'columns' | 'rows'> {
  /**
   * Array of column definitions for the grid.
   */
  columns: GridColDef[];
  /**
   * Array of data rows for the grid.
   */
  rows: readonly any[];
  /**
   * If true, a loading indicator is shown over the grid.
   */
  isLoading?: boolean;
  /**
   * If an error object is provided, an error message is shown.
   */
  error?: { message: string } | null;
  /**
   * A message to display when there are no rows.
   * @default "No rows"
   */
  noRowsMessage?: string;
}

const CustomNoRowsOverlay = ({ message }: { message: string }) => (
  <Box
    sx={{
      display: 'flex',
      justifyContent: 'center',
      alignItems: 'center',
      height: '100%',
    }}
  >
    <Typography color="text.secondary">{message}</Typography>
  </Box>
);

/**
 * A shared, enterprise-grade DataGrid component built on Material-UI X DataGridPro.
 * It provides a standardized look and feel and built-in support for common states
 * like loading, error, and empty data. It is designed to be controlled by parent
 * components, making it ideal for server-side pagination, sorting, and filtering.
 */
export const DataGrid: React.FC<DataGridProps> = ({
  columns,
  rows,
  isLoading = false,
  error = null,
  noRowsMessage = 'No data to display',
  ...rest
}) => {
  if (error) {
    return (
      <Alert severity="error" variant="outlined">
        {error.message || 'An error occurred while fetching data.'}
      </Alert>
    );
  }

  return (
    <Paper sx={{ height: 600, width: '100%' }} elevation={1}>
      <DataGridPro
        columns={columns}
        rows={rows}
        loading={isLoading}
        slots={{
          loadingOverlay: LinearProgress,
          noRowsOverlay: () => <CustomNoRowsOverlay message={noRowsMessage} />,
        }}
        // Default props for a consistent experience
        disableRowSelectionOnClick
        rowHeight={52}
        sx={{
          border: 0,
          '& .MuiDataGrid-columnHeaders': {
            backgroundColor: (theme) => theme.palette.action.hover,
          },
        }}
        {...rest}
      />
    </Paper>
  );
};

// Example of a reusable cell renderer for quality status
export const renderQualityCell = (params: GridRenderCellParams<any, string>) => {
  const quality = params.value?.toLowerCase();
  let color;
  switch (quality) {
    case 'good':
      color = 'success.main';
      break;
    case 'bad':
      color = 'error.main';
      break;
    case 'uncertain':
      color = 'warning.main';
      break;
    default:
      color = 'text.secondary';
  }
  return <Typography color={color}>{params.value}</Typography>;
};