erDiagram
    LogEntry {
        DateTime timestamp "Index"
        Keyword level "Index, e.g., INFO, WARN, ERROR, DEBUG"
        Text message "FullTextIndex, The log message content, searchable."
        Keyword serviceName "Index, Name of the originating microservice (e.g., 'DataIngestionService')."
        Keyword opcCoreClientId "Index, Identifier for logs originating from an edge client."
        Keyword tenantId "Index, Tenant context, if available."
        Keyword correlationId "Index, ID for tracing a request across multiple services."
        Object exceptionDetails "Structured exception information, including stack trace."
    }