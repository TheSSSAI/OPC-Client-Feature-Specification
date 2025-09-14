sequenceDiagram
    participant "Deployment Team" as DeploymentTeam
    participant "Site Engineer" as SiteEngineer
    participant "Legacy System" as LegacySystem
    participant "Asset & Topology Svc." as AssetTopologySvc
    actor "OPC Core Client" as OPCCoreClient

    DeploymentTeam->>LegacySystem: 1. 1. Execute Final Data Migration Script
    LegacySystem-->>DeploymentTeam: 2. Data Export Complete
    activate AssetTopologySvc
    DeploymentTeam->>AssetTopologySvc: 1.1. 1.1. Ingest Migrated Data via Bulk Import API
    AssetTopologySvc-->>DeploymentTeam: 1.2. HTTP 202 Accepted (Import Job ID)
    DeploymentTeam->>SiteEngineer: 3. 3. Confirm Pre-Cutover Checklist Complete & Hold Go/No-Go Meeting
    SiteEngineer-->>DeploymentTeam: 4. Go Decision Confirmed
    activate LegacySystem
    DeploymentTeam->>LegacySystem: 5. 5. Disable Write Access (Freeze System State)
    LegacySystem-->>DeploymentTeam: 6. Write Access Disabled
    activate OPCCoreClient
    DeploymentTeam->>OPCCoreClient: 7. 7. Send 'Activate Live Mode' Command
    OPCCoreClient->>AssetTopologySvc: 8. 8. [Loop] Stream Live Telemetry Data
    SiteEngineer->>AssetTopologySvc: 9. 9. Request Data for Validation
    AssetTopologySvc-->>SiteEngineer: 10. HTTP 200 OK (Time-series Data)
    SiteEngineer->>LegacySystem: 9.1. 9.1. Request Read-Only Data for Comparison
    LegacySystem-->>SiteEngineer: 9.2. Legacy Data
    SiteEngineer->>DeploymentTeam: 11. 11. [Alt] Validation Outcome


    deactivate OPCCoreClient
    deactivate LegacySystem
    deactivate AssetTopologySvc
