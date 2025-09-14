import React, { useMemo, useState, useCallback } from 'react';
import {
  Box,
  CircularProgress,
  TextField,
  Typography,
  IconButton,
  Menu,
  MenuItem,
  ListItemIcon,
  ListItemText,
  Alert,
} from '@mui/material';
import { RichTreeView } from '@mui/x-tree-view/RichTreeView';
import { TreeViewBaseItem } from '@mui/x-tree-view/models';
import MoreVertIcon from '@mui/icons-material/MoreVert';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import { useGetAssetsQuery, useCreateAssetMutation, useUpdateAssetMutation, useDeleteAssetMutation } from '../api/assetApi';
import { Asset, AssetNode } from '../types';

// Helper function to build the tree structure from a flat list of assets
const buildTree = (assets: Asset[]): AssetNode[] => {
  const assetMap = new Map<string, AssetNode>();
  const roots: AssetNode[] = [];

  assets.forEach(asset => {
    assetMap.set(asset.id, { ...asset, children: [] });
  });

  assets.forEach(asset => {
    if (asset.parentId && assetMap.has(asset.parentId)) {
      assetMap.get(asset.parentId)?.children?.push(assetMap.get(asset.id)!);
    } else {
      roots.push(assetMap.get(asset.id)!);
    }
  });

  return roots;
};


/**
 * @description
 * The AssetTree component is a core UI element for asset management.
 * It fetches the asset hierarchy and displays it in an interactive tree view.
 * It supports lazy loading, searching, and context-sensitive actions like add, rename, and delete.
 *
 * @architecture
 * - This Level 4 component consumes hooks from `assetApi` (also Level 4) for data fetching and mutations.
 * - It uses the `RichTreeView` component from Material-X, a 3rd party library.
 * - It manages local UI state for search, expansion, and context menu interactions.
 * - It implements performance optimizations like `useMemo` for the expensive tree transformation.
 */
const AssetTree: React.FC = () => {
  const { data: assets = [], isLoading, isError, error } = useGetAssetsQuery();
  const [createAsset] = useCreateAssetMutation();
  const [updateAsset] = useUpdateAssetMutation();
  const [deleteAsset] = useDeleteAssetMutation();

  const [searchTerm, setSearchTerm] = useState('');
  const [contextMenu, setContextMenu] = useState<{ mouseX: number; mouseY: number; item: TreeViewBaseItem } | null>(null);

  const assetTree = useMemo(() => buildTree(assets), [assets]);

  // TODO: Implement a more robust filtering logic that retains parent context
  const filteredTree = useMemo(() => {
    if (!searchTerm) return assetTree;
    // This is a simple filter; a production implementation might need a more complex algorithm
    // to keep parent nodes of matched children visible.
    const filter = (nodes: AssetNode[]): AssetNode[] => {
        return nodes.reduce((acc, node) => {
            if (node.name.toLowerCase().includes(searchTerm.toLowerCase())) {
                acc.push({ ...node, children: filter(node.children || [])});
            } else if (node.children) {
                const filteredChildren = filter(node.children);
                if (filteredChildren.length > 0) {
                    acc.push({ ...node, children: filteredChildren });
                }
            }
            return acc;
        }, [] as AssetNode[]);
    };
    return filter(assetTree);
  }, [assetTree, searchTerm]);

  const handleContextMenu = (event: React.MouseEvent, item: TreeViewBaseItem) => {
    event.preventDefault();
    event.stopPropagation();
    setContextMenu(
      contextMenu === null
        ? {
            mouseX: event.clientX + 2,
            mouseY: event.clientY - 6,
            item,
          }
        : null,
    );
  };

  const handleCloseContextMenu = () => {
    setContextMenu(null);
  };
  
  const handleAddChild = useCallback(async () => {
    if (!contextMenu) return;
    const parentId = contextMenu.item.id;
    const newName = prompt('Enter name for the new child asset:');
    if (newName) {
        try {
            await createAsset({ name: newName, parentId }).unwrap();
        } catch(err) {
            alert(`Error creating asset: ${JSON.stringify(err)}`);
        }
    }
    handleCloseContextMenu();
  }, [contextMenu, createAsset]);

  const handleRename = useCallback(async () => {
    if (!contextMenu) return;
    const { id, label } = contextMenu.item;
    const newName = prompt('Enter new name for the asset:', label as string);
    if (newName && newName !== label) {
        try {
            await updateAsset({ id, name: newName }).unwrap();
        } catch(err) {
            alert(`Error updating asset: ${JSON.stringify(err)}`);
        }
    }
    handleCloseContextMenu();
  }, [contextMenu, updateAsset]);

  const handleDelete = useCallback(async () => {
    if (!contextMenu) return;
    const { id, label } = contextMenu.item;
    if (window.confirm(`Are you sure you want to delete asset "${label}" and all its children?`)) {
        try {
            await deleteAsset(id).unwrap();
        } catch(err) {
            alert(`Error deleting asset: ${JSON.stringify(err)}`);
        }
    }
    handleCloseContextMenu();
  }, [contextMenu, deleteAsset]);


  if (isLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100%' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (isError) {
    return <Alert severity="error">Error loading asset tree: {JSON.stringify(error)}</Alert>;
  }

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
      <Typography variant="h6" sx={{ p: 2, pb: 1 }}>Asset Hierarchy</Typography>
      <Box sx={{ px: 2, pb: 2 }}>
        <TextField
          fullWidth
          variant="outlined"
          label="Search Assets"
          size="small"
          value={searchTerm}
          onChange={(e) => setSearchTerm(e.target.value)}
        />
      </Box>
      <Box sx={{ flexGrow: 1, overflowY: 'auto', p: 1 }}>
        <RichTreeView
          items={filteredTree}
          getItemLabel={(item) => item.name}
          getItemId={(item) => item.id}
          slotProps={{
            item: {
              onContextMenu: handleContextMenu,
            },
          }}
        />
        <Menu
            open={contextMenu !== null}
            onClose={handleCloseContextMenu}
            anchorReference="anchorPosition"
            anchorPosition={
              contextMenu !== null ? { top: contextMenu.mouseY, left: contextMenu.mouseX } : undefined
            }
        >
            <MenuItem onClick={handleAddChild}>
                <ListItemIcon><AddCircleOutlineIcon fontSize="small" /></ListItemIcon>
                <ListItemText>Add Child Asset</ListItemText>
            </MenuItem>
            <MenuItem onClick={handleRename}>
                <ListItemIcon><EditIcon fontSize="small" /></ListItemIcon>
                <ListItemText>Rename</ListItemText>
            </MenuItem>
            <MenuItem onClick={handleDelete} sx={{ color: 'error.main' }}>
                <ListItemIcon sx={{ color: 'error.main' }}><DeleteIcon fontSize="small" /></ListItemIcon>
                <ListItemText>Delete</ListItemText>
            </MenuItem>
        </Menu>
      </Box>
    </Box>
  );
};

export default AssetTree;