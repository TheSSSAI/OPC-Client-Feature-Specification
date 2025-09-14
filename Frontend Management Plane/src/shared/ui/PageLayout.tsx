import * as React from 'react';
import { Box, Container, Typography, Breadcrumbs, Link as MuiLink, ContainerProps } from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';

interface BreadcrumbLink {
  label: string;
  href: string;
}

export interface PageLayoutProps extends ContainerProps {
  /**
   * The main title of the page, rendered as an h1 element for accessibility.
   */
  title: string;
  /**
   * An optional React node for actions, typically buttons, displayed next to the title.
   */
  actions?: React.ReactNode;
  /**
   * An array of breadcrumb links to display for navigation context.
   */
  breadcrumbs?: BreadcrumbLink[];
  /**
   * The main content of the page.
   */
  children: React.ReactNode;
}

/**
 * A standardized layout component for pages within the application.
 * It provides a consistent structure with a title, optional breadcrumbs,
 * an action area, and the main content body.
 */
export const PageLayout: React.FC<PageLayoutProps> = ({
  title,
  actions,
  breadcrumbs,
  children,
  ...containerProps
}) => {
  return (
    <Container maxWidth="xl" sx={{ py: 3 }} {...containerProps}>
      <Box
        component="header"
        sx={{
          display: 'flex',
          flexDirection: { xs: 'column', md: 'row' },
          alignItems: { xs: 'flex-start', md: 'center' },
          justifyContent: 'space-between',
          mb: 3,
        }}
      >
        <Box>
          {breadcrumbs && breadcrumbs.length > 0 && (
            <Breadcrumbs aria-label="breadcrumb" sx={{ mb: 1 }}>
              {breadcrumbs.map((crumb, index) =>
                index < breadcrumbs.length - 1 ? (
                  <MuiLink
                    key={index}
                    component={RouterLink}
                    underline="hover"
                    color="inherit"
                    to={crumb.href}
                  >
                    {crumb.label}
                  </MuiLink>
                ) : (
                  <Typography key={index} color="text.primary">
                    {crumb.label}
                  </Typography>
                )
              )}
            </Breadcrumbs>
          )}
          <Typography variant="h4" component="h1" gutterBottom>
            {title}
          </Typography>
        </Box>
        {actions && <Box sx={{ mt: { xs: 2, md: 0 } }}>{actions}</Box>}
      </Box>
      <Box component="main">{children}</Box>
    </Container>
  );
};