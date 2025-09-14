sequenceDiagram
    participant "Asset Service" as AssetService
    participant "Kubelet / CRI" as KubeletCRI
    participant "Fluentd Agent" as FluentdAgent
    participant "OpenSearch Cluster" as OpenSearchCluster

    activate KubeletCRI
    AssetService->>KubeletCRI: 1. Writes structured JSON log to stdout stream
    activate FluentdAgent
    KubeletCRI->>FluentdAgent: 2. Tails container log file from host path
    FluentdAgent->>FluentdAgent: 3. Parses JSON and enriches with Kubernetes metadata
    activate OpenSearchCluster
    FluentdAgent->>OpenSearchCluster: 4. Sends batch of enriched log documents via Bulk API
    OpenSearchCluster-->>FluentdAgent: 200 OK with batch processing results
    OpenSearchCluster->>OpenSearchCluster: 5. Asynchronously indexes documents for search

    note over AssetService: Structured Logging is Mandatory: All microservices MUST write logs in a consistent JSON format to...
    note over FluentdAgent: Resilience via Buffering: Fluentd MUST be configured with a file-based buffer. This allows logs t...
    note over KubeletCRI: DaemonSet Architecture: Fluentd runs as a DaemonSet, ensuring exactly one agent pod is running on...

    deactivate OpenSearchCluster
    deactivate FluentdAgent
    deactivate KubeletCRI
