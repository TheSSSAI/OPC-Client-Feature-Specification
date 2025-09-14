import * as React from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  IconButton,
  Typography,
  Divider,
  DialogProps,
  Box,
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';

export interface ModalProps extends Omit<DialogProps, 'title'> {
  /**
   * The title to display in the modal header.
   */
  title: React.ReactNode;
  /**
   * If `true`, the modal is open.
   */
  open: boolean;
  /**
   * Callback fired when the component requests to be closed.
   */
  onClose: () => void;
  /**
   * The content of the modal.
   */
  children: React.ReactNode;
  /**
   * The actions to display in the modal footer.
   */
  actions?: React.ReactNode;
  /**
   * Disables the backdrop click handler.
   * @default false
   */
  disableBackdropClick?: boolean;
}

/**
 * A shared, accessible Modal component built on Material-UI's Dialog.
 * It provides a standardized structure with a title, close button, content area, and an optional actions footer.
 * Complies with accessibility best practices by managing focus and ARIA attributes.
 */
export const Modal: React.FC<ModalProps> = ({
  title,
  open,
  onClose,
  children,
  actions,
  disableBackdropClick = false,
  ...rest
}) => {
  const handleClose = (event: {}, reason: 'backdropClick' | 'escapeKeyDown') => {
    if (reason === 'backdropClick' && disableBackdropClick) {
      return;
    }
    onClose();
  };

  return (
    <Dialog
      onClose={handleClose}
      open={open}
      aria-labelledby="modal-title"
      PaperProps={{
        sx: {
          minWidth: { xs: '90%', md: '500px' },
        },
      }}
      {...rest}
    >
      <DialogTitle sx={{ p: 2 }}>
        <Box display="flex" alignItems="center" justifyContent="space-between">
          <Typography variant="h6" component="span" id="modal-title">
            {title}
          </Typography>
          <IconButton
            aria-label="close"
            onClick={onClose}
            sx={{
              color: (theme) => theme.palette.grey[500],
            }}
          >
            <CloseIcon />
          </IconButton>
        </Box>
      </DialogTitle>
      <Divider />
      <DialogContent sx={{ p: 3, '.MuiDialogContent-root + .MuiDivider-root': { mt: 0 } }}>
        {children}
      </DialogContent>
      {actions && (
        <>
          <Divider />
          <DialogActions sx={{ p: 2 }}>{actions}</DialogActions>
        </>
      )}
    </Dialog>
  );
};