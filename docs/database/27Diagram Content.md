erDiagram
    AuditRecord {
        BIGINT auditLogId UK "Correlates with the primary key of the relational AuditLog table."
        Guid tenantId
        Guid userId
        DateTimeOffset timestamp
        VARCHAR actionType
        VARCHAR entityName
        VARCHAR entityId
        JSONB details
        VARCHAR sourceIpAddress
    }