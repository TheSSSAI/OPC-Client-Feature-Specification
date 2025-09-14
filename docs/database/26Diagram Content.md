erDiagram
    Tenant {
        Guid tenantId PK
    }
    User {
        Guid userId PK
    }
    Role {
        Guid roleId PK
    }
    UserRole {
        Guid userId PK
        Guid roleId PK
    }
    AuditLog {
        BIGINT auditLogId PK
    }
    OpcCoreClient {
        Guid opcCoreClientId PK
    }
    OpcServerConnection {
        Guid opcServerConnectionId PK
    }
    Asset {
        Guid assetId PK
    }
    OpcTag {
        Guid opcTagId PK
    }
    TagDataPoint {
        DateTimeOffset timestamp PK
        Guid opcTagId PK
    }
    Alarm {
        Guid alarmId PK
    }
    AlarmHistory {
        BIGINT alarmHistoryId PK
    }
    AiModel {
        Guid aiModelId PK
    }
    AiModelVersion {
        Guid aiModelVersionId PK
    }
    ModelAssignment {
        Guid modelAssignmentId PK
    }
    AnomalyEvent {
        DateTimeOffset timestamp PK
        Guid modelAssignmentId PK
    }
    License {
        Guid licenseId PK
    }
    ReportTemplate {
        Guid reportTemplateId PK
    }
    ApprovalRequest {
        Guid approvalRequestId PK
    }
    Dashboard {
        Guid dashboardId PK
    }
    Widget {
        Guid widgetId PK
    }
    ArTagMapping {
        Guid arTagMappingId PK
    }
    DataImportJob {
        Guid dataImportJobId PK
    }

    Tenant ||--o{ User : "TenantUsers"
    Tenant ||--o{ OpcCoreClient : "TenantOpcCoreClients"
    Tenant ||--o{ Asset : "TenantAssets"
    Tenant ||--o{ AuditLog : "TenantAuditLogs"
    User ||--o{ AuditLog : "UserAuditLogs"
    User ||--o{ UserRole : "has"
    Role ||--o{ UserRole : "defines"
    Asset ||--o{ UserRole : "AssetRoleScope"
    OpcCoreClient ||--o{ OpcServerConnection : "ClientServerConnections"
    OpcServerConnection |o--o| OpcServerConnection : "OpcServerRedundancy"
    Asset |o--o{ Asset : "AssetHierarchy"
    Asset ||--o{ OpcTag : "AssetOpcTags"
    OpcServerConnection ||--o{ OpcTag : "ConnectionOpcTags"
    OpcTag ||--o{ TagDataPoint : "OpcTagDataPoints"
    OpcTag ||--o{ Alarm : "OpcTagAlarms"
    Alarm ||--|{ AlarmHistory : "AlarmHistoryEntries"
    User ||--o{ AlarmHistory : "UserAlarmActions"
    Tenant ||--o{ AiModel : "TenantAiModels"
    AiModel ||--|{ AiModelVersion : "AiModelVersions"
    User ||--o{ AiModelVersion : "UserSubmittedModelVersions"
    User ||--o{ AiModelVersion : "UserApprovedModelVersions"
    Asset ||--o{ ModelAssignment : "AssetModelAssignments"
    AiModelVersion ||--o{ ModelAssignment : "VersionModelAssignments"
    OpcCoreClient ||--o{ ModelAssignment : "ClientModelAssignments"
    ModelAssignment ||--o{ AnomalyEvent : "AssignmentAnomalyEvents"
    Tenant ||--|| License : "TenantLicense"
    Tenant ||--o{ ReportTemplate : "TenantReportTemplates"
    Tenant ||--o{ ApprovalRequest : "TenantApprovalRequests"
    User ||--o{ ApprovalRequest : "UserRequestedApprovals"
    User ||--o{ ApprovalRequest : "UserApprovedApprovals"
    Tenant ||--o{ Dashboard : "TenantDashboards"
    User ||--o{ Dashboard : "UserDashboards"
    Dashboard ||--o{ Widget : "DashboardWidgets"
    Tenant ||--o{ ArTagMapping : "TenantArTagMappings"
    Asset ||--o{ ArTagMapping : "AssetArTagMappings"
    OpcTag ||--o{ ArTagMapping : "OpcTagArTagMappings"
    Tenant ||--o{ DataImportJob : "TenantDataImportJobs"
    User ||--o{ DataImportJob : "UserDataImportJobs"