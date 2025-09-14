import React, { useState, useCallback, useEffect } from 'react';
import { Responsive, WidthProvider, Layout, Layouts } from 'react-grid-layout';
import { Box, Button, CircularProgress, Alert, Paper, Typography } from '@mui/material';
import 'react-grid-layout/css/styles.css';
import 'react-resizable/css/styles.css';

import { useGetDashboardQuery, useSaveDashboardMutation, DashboardConfig } from '../api/dashboardApi';
import { useDebounce } from '../../../shared/hooks/useDebounce';

const ResponsiveGridLayout = WidthProvider(Responsive);

// Dummy widget component for demonstration
const ExampleWidget: React.FC<{ id: string }> = ({ id }) => (
  <Paper sx={{ height: '100%', display: 'flex', alignItems: 'center', justifyContent: 'center', p: 1 }}>
    <Typography>Widget {id}</Typography>
  </Paper>
);


/**
 * @description
 * Implements a user-customizable dashboard using a grid layout system.
 * It allows users to add, remove, resize, and rearrange widgets, with the layout
 * state persisted on the backend.
 *
 * @architecture
 * - This Level 4 component is the core of the 'dashboard' feature.
 * - It consumes `useGetDashboardQuery` and `useSaveDashboardMutation` hooks from `dashboardApi`.
 * - It integrates the third-party `react-grid-layout` library for the interactive grid.
 * - It uses the `useDebounce` hook (Level 1) to optimize layout saving, preventing excessive API calls.
 * - Manages an "edit mode" state to toggle between interactive and static views.
 */
const CustomizableDashboard: React.FC<{ dashboardId: string }> = ({ dashboardId }) => {
  const { data: dashboardConfig, isLoading, isError, error } = useGetDashboardQuery(dashboardId);
  const [saveDashboard, { isLoading: isSaving }] = useSaveDashboardMutation();

  const [isEditMode, setIsEditMode] = useState(false);
  const [currentLayouts, setCurrentLayouts] = useState<Layouts>({});

  // Update local layout state when data is fetched from the backend
  useEffect(() => {
    if (dashboardConfig?.layout) {
      // react-grid-layout expects layouts as an object with breakpoint keys
      setCurrentLayouts({ lg: dashboardConfig.layout });
    }
  }, [dashboardConfig]);

  const debouncedSaveLayout = useDebounce(async (layouts: Layouts) => {
    if (!dashboardConfig) return;
    try {
      const updatedConfig: DashboardConfig = {
        ...dashboardConfig,
        layout: layouts.lg || [], // Assuming 'lg' is our primary breakpoint
      };
      await saveDashboard(updatedConfig).unwrap();
    } catch (err) {
      console.error('Failed to save dashboard layout:', err);
      // Here you would dispatch a toast notification
    }
  }, 1000); // Debounce save requests by 1 second

  const handleLayoutChange = useCallback((layout: Layout[], layouts: Layouts) => {
    // This is called on every drag/resize. We update local state immediately for responsiveness
    // and trigger the debounced save for persistence.
    setCurrentLayouts(layouts);
    if (isEditMode) {
      debouncedSaveLayout(layouts);
    }
  }, [isEditMode, debouncedSaveLayout]);
  
  // TODO: Implement widget adding/removing logic
  // For now, we'll just render the widgets from the config
  const renderWidgets = () => {
    return dashboardConfig?.widgets.map(widget => (
      <Box key={widget.i} data-grid={{ i: widget.i, x: 0, y: 0, w: 3, h: 2, ...dashboardConfig.layout.find(l => l.i === widget.i) }}>
        <ExampleWidget id={widget.i} />
      </Box>
    ));
  };


  if (isLoading) {
    return <CircularProgress />;
  }
  if (isError) {
    return <Alert severity="error">Error loading dashboard: {JSON.stringify(error)}</Alert>;
  }

  return (
    <Box sx={{ p: 2, display: 'flex', flexDirection: 'column', height: '100%' }}>
      <Box sx={{ mb: 2, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <Typography variant="h5">My Dashboard</Typography>
        <Box>
            {isSaving && <CircularProgress size={24} sx={{ mr: 2 }} />}
            <Button variant="contained" onClick={() => setIsEditMode(!isEditMode)}>
                {isEditMode ? 'Finish Editing' : 'Edit Layout'}
            </Button>
        </Box>
      </Box>
      <ResponsiveGridLayout
        className="layout"
        layouts={currentLayouts}
        breakpoints={{ lg: 1200, md: 996, sm: 768, xs: 480, xxs: 0 }}
        cols={{ lg: 12, md: 10, sm: 6, xs: 4, xxs: 2 }}
        rowHeight={100}
        onLayoutChange={handleLayoutChange}
        isDraggable={isEditMode}
        isResizable={isEditMode}
        draggableHandle=".widget-drag-handle" // A class you'd put on a widget's header
      >
        {/* In a real app, this would dynamically render different widget types */}
        {(dashboardConfig?.layout || []).map(l => (
            <Paper key={l.i} sx={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                <Typography>Widget {l.i}</Typography>
            </Paper>
        ))}
      </ResponsiveGridLayout>
    </Box>
  );
};

export default CustomizableDashboard;