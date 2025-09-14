erDiagram
    Tenant {
        Guid tenantId PK
        VARCHAR name
    }
    User {
        Guid userId PK
        Guid tenantId FK
        VARCHAR email
    }
    Role {
        Guid roleId PK
        VARCHAR name
    }
    UserRole {
        Guid userId PK, FK
        Guid roleId PK, FK
        Guid assetScopeId FK
    }
    License {
        Guid licenseId PK
        Guid tenantId FK
        VARCHAR licenseKey
    }
    AuditLog {
        BIGINT auditLogId PK
        Guid tenantId FK
        Guid userId FK
    }
    ApprovalRequest {
        Guid approvalRequestId PK
        Guid tenantId FK
        Guid requestedByUserId FK
        Guid approvedByUserId FK
    }
    Asset {
        Guid assetId PK
        VARCHAR name
    }

    Tenant ||..o{ User : has
    Tenant ||--|| License : licensed with
    Tenant ||--o{ AuditLog : contains
    Tenant ||..o{ ApprovalRequest : has
    User }o--|| UserRole : assignments
    Role }o--|| UserRole : assignments
    User |o..o{ AuditLog : performed_by
    User ||..o{ ApprovalRequest : requested_by
    User |o..o{ ApprovalRequest : approved_by
    Asset |o..o{ UserRole : scoped_to