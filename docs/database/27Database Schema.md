# 1 Title

Audit Verification Ledger

# 2 Name

cmp_audit_ledger

# 3 Db Type

- ledger

# 4 Db Technology

Amazon QLDB

# 5 Entities

- {'name': 'AuditRecord', 'description': 'Represents an immutable, cryptographically verifiable document in the QLDB ledger corresponding to each record in the relational AuditLog table. (REQ-FR-019)', 'attributes': [{'name': 'auditLogId', 'type': 'BIGINT', 'isRequired': True, 'isPrimaryKey': False, 'isUnique': True, 'indexType': 'UniqueIndex', 'comment': 'Correlates with the primary key of the relational AuditLog table.'}, {'name': 'tenantId', 'type': 'Guid', 'isRequired': True, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index'}, {'name': 'userId', 'type': 'Guid', 'isRequired': False, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index'}, {'name': 'timestamp', 'type': 'DateTimeOffset', 'isRequired': True, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index'}, {'name': 'actionType', 'type': 'VARCHAR', 'isRequired': True, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index', 'size': 100}, {'name': 'entityName', 'type': 'VARCHAR', 'isRequired': False, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index', 'size': 100}, {'name': 'entityId', 'type': 'VARCHAR', 'isRequired': False, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index', 'size': 255}, {'name': 'details', 'type': 'JSONB', 'isRequired': False, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'None'}, {'name': 'sourceIpAddress', 'type': 'VARCHAR', 'isRequired': False, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'None', 'size': 45}], 'primaryKeys': [], 'uniqueConstraints': [], 'indexes': [{'name': 'IX_AuditRecord_auditLogId', 'columns': ['auditLogId'], 'type': 'Standard'}, {'name': 'IX_AuditRecord_tenantId_timestamp', 'columns': ['tenantId', 'timestamp'], 'type': 'Standard'}, {'name': 'IX_AuditRecord_entityName_entityId', 'columns': ['entityName', 'entityId'], 'type': 'Standard'}]}

# 6 Relations

*No items available*

