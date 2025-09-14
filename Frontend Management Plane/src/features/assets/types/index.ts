/**
 * @file Defines the core data structures and types for the Asset feature domain.
 * These types are used throughout the application to ensure consistency when
 * handling asset-related data fetched from the backend.
 * This aligns with the database design for the Asset entity and related models.
 */

// REQ-FR-021, REQ-CON-004: Represents a single asset in the physical plant hierarchy.
// Aligns with the ISA-95 model of Site, Area, Production Line, etc.
export interface Asset {
  id: string; // UUID
  tenantId: string; // UUID
  name: string;
  parentId: string | null; // UUID of the parent asset, or null for root assets
  // Additional properties can be added here as needed, e.g., description, assetType
}

// A hierarchical node structure used for building the client-side tree view.
// This is a client-side representation derived from the flat list of Assets.
export interface AssetNode extends Asset {
  children: AssetNode[];
}

// REQ-FR-026: Represents the mapping of a raw OPC Tag to a specific asset.
export interface OpcTagMapping {
  id: string; // UUID of the mapping record
  assetId: string; // FK to Asset
  opcTagId: string; // Identifier of the OPC Tag
  opcTagNodeId: string; // Full NodeID of the tag from the OPC server
  assetPropertyName: string; // The contextual property on the asset (e.g., "InletPressure")
}

// REQ-FR-021, US-027: Represents a template for creating similar assets.
export interface AssetTemplate {
  id: string; // UUID
  tenantId: string; // UUID
  name: string;
  description: string;
  properties: AssetTemplateProperty[];
}

export interface AssetTemplateProperty {
  id: string; // UUID
  name: string; // e.g., "MotorCurrent"
  dataType: 'String' | 'Float' | 'Integer' | 'Boolean';
  unit: string | null;
}

// REQ-FR-018, US-042: Defines the configuration for an Augmented Reality overlay for an asset.
export interface ArMappingConfiguration {
  id: string; // UUID
  assetId: string; // FK to Asset
  markerId: string; // The unique ID embedded in the QR code
  layout: ArLayoutDefinition; // JSON object defining the visual layout
}

// Represents the layout definition for AR overlays.
// This structure would be defined in collaboration with the AR client application team.
export interface ArLayoutDefinition {
  version: string;
  elements: ArLayoutElement[];
}

export interface ArLayoutElement {
  opcTagMappingId: string; // The specific tag mapping to display
  position: { x: number; y: number; z: number }; // Relative to the marker
  displayType: 'text' | 'gauge' | 'trend-line';
  label: string;
  color: string;
}