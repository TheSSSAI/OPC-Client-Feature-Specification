# 1 Title

Observability Log Store

# 2 Name

cmp_logs

# 3 Db Type

- search

# 4 Db Technology

OpenSearch

# 5 Entities

- {'name': 'LogEntry', 'description': 'Represents a structured log document from any system component (microservice or edge client), aggregated for centralized analysis and monitoring. (REQ-MON-001)', 'attributes': [{'name': 'timestamp', 'type': 'DateTime', 'isRequired': True, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index'}, {'name': 'level', 'type': 'Keyword', 'isRequired': True, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index', 'comment': 'e.g., INFO, WARN, ERROR, DEBUG'}, {'name': 'message', 'type': 'Text', 'isRequired': True, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'FullTextIndex', 'comment': 'The log message content, searchable.'}, {'name': 'serviceName', 'type': 'Keyword', 'isRequired': True, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index', 'comment': "Name of the originating microservice (e.g., 'DataIngestionService')."}, {'name': 'opcCoreClientId', 'type': 'Keyword', 'isRequired': False, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index', 'comment': 'Identifier for logs originating from an edge client.'}, {'name': 'tenantId', 'type': 'Keyword', 'isRequired': False, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index', 'comment': 'Tenant context, if available.'}, {'name': 'correlationId', 'type': 'Keyword', 'isRequired': False, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'Index', 'comment': 'ID for tracing a request across multiple services.'}, {'name': 'exceptionDetails', 'type': 'Object', 'isRequired': False, 'isPrimaryKey': False, 'isUnique': False, 'indexType': 'None', 'comment': 'Structured exception information, including stack trace.'}], 'primaryKeys': [], 'uniqueConstraints': [], 'indexes': [], 'optimizations': [{'type': 'Index Lifecycle Management (ILM)', 'suggestion': 'Implement ILM policies to automatically manage log indices: move old data to warm/cold tiers and delete after the retention period.', 'comment': 'Manages storage costs and maintains performance.'}]}

# 6 Relations

*No items available*

