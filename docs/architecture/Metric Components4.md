# 1 System Overview

## 1.1 Analysis Date

2025-06-13

## 1.2 Technology Stack

- .NET 8
- React 18
- PostgreSQL 16
- TimescaleDB
- Redis 7
- gRPC
- MQTT v5
- Docker
- Kubernetes (EKS)
- Kong API Gateway

## 1.3 Monitoring Components

- Prometheus
- Grafana
- Alertmanager
- OpenSearch
- Fluentd
- OpenTelemetry

## 1.4 Requirements

- REQ-1-074
- REQ-1-075
- REQ-1-084
- REQ-1-085
- REQ-1-090
- REQ-1-062
- REQ-1-079

## 1.5 Environment

production

# 2.0 Standard System Metrics Selection

## 2.1 Hardware Utilization Metrics

### 2.1.1 gauge

#### 2.1.1.1 Name

kube_pod_container_resource_usage_cpu_cores

#### 2.1.1.2 Type

🔹 gauge

#### 2.1.1.3 Unit

cores

#### 2.1.1.4 Description

CPU usage for each container in the Kubernetes cluster.

#### 2.1.1.5 Collection

##### 2.1.1.5.1 Interval

30s

##### 2.1.1.5.2 Method

Prometheus scrape via cAdvisor

#### 2.1.1.6.0 Thresholds

##### 2.1.1.6.1 Warning

> 70% of limit

##### 2.1.1.6.2 Critical

> 85% of limit

#### 2.1.1.7.0 Justification

Essential for monitoring resource consumption and triggering Horizontal Pod Autoscalers (HPA) as per REQ-1-085.

### 2.1.2.0.0 gauge

#### 2.1.2.1.0 Name

kube_pod_container_resource_usage_memory_bytes

#### 2.1.2.2.0 Type

🔹 gauge

#### 2.1.2.3.0 Unit

bytes

#### 2.1.2.4.0 Description

Memory usage for each container in the Kubernetes cluster.

#### 2.1.2.5.0 Collection

##### 2.1.2.5.1 Interval

30s

##### 2.1.2.5.2 Method

Prometheus scrape via cAdvisor

#### 2.1.2.6.0 Thresholds

##### 2.1.2.6.1 Warning

> 75% of limit

##### 2.1.2.6.2 Critical

> 90% of limit

#### 2.1.2.7.0 Justification

Crucial for preventing OutOfMemory (OOM) kills and triggering HPA as per REQ-1-085.

### 2.1.3.0.0 gauge

#### 2.1.3.1.0 Name

opc_client_buffer_usage_bytes

#### 2.1.3.2.0 Type

🔹 gauge

#### 2.1.3.3.0 Unit

bytes

#### 2.1.3.4.0 Description

The current size of the on-disk data buffer on the OPC Core Client.

#### 2.1.3.5.0 Collection

##### 2.1.3.5.1 Interval

60s

##### 2.1.3.5.2 Method

Exposed via /metrics endpoint on OPC Core Client

#### 2.1.3.6.0 Thresholds

##### 2.1.3.6.1 Warning

> 80% of configured limit

##### 2.1.3.6.2 Critical

> 95% of configured limit

#### 2.1.3.7.0 Justification

Directly monitors the state of the store-and-forward mechanism required by REQ-1-079 to prevent data loss when the buffer is full.

## 2.2.0.0.0 Runtime Metrics

### 2.2.1.0.0 gauge

#### 2.2.1.1.0 Name

dotnet_gc_heap_size_bytes

#### 2.2.1.2.0 Type

🔹 gauge

#### 2.2.1.3.0 Unit

bytes

#### 2.2.1.4.0 Description

Total heap size for all generations in .NET microservices.

#### 2.2.1.5.0 Technology

.NET

#### 2.2.1.6.0 Collection

##### 2.2.1.6.1 Interval

60s

##### 2.2.1.6.2 Method

Prometheus scrape from /metrics endpoint

#### 2.2.1.7.0 Criticality

medium

### 2.2.2.0.0 gauge

#### 2.2.2.1.0 Name

dotnet_threadpool_queue_length

#### 2.2.2.2.0 Type

🔹 gauge

#### 2.2.2.3.0 Unit

threads

#### 2.2.2.4.0 Description

Number of work items currently queued in the .NET thread pool.

#### 2.2.2.5.0 Technology

.NET

#### 2.2.2.6.0 Collection

##### 2.2.2.6.1 Interval

30s

##### 2.2.2.6.2 Method

Prometheus scrape from /metrics endpoint

#### 2.2.2.7.0 Criticality

high

### 2.2.3.0.0 gauge

#### 2.2.3.1.0 Name

db_connection_pool_active_connections

#### 2.2.3.2.0 Type

🔹 gauge

#### 2.2.3.3.0 Unit

connections

#### 2.2.3.4.0 Description

Number of active database connections in the connection pool for each microservice.

#### 2.2.3.5.0 Technology

.NET

#### 2.2.3.6.0 Collection

##### 2.2.3.6.1 Interval

30s

##### 2.2.3.6.2 Method

Exposed by application via /metrics endpoint

#### 2.2.3.7.0 Criticality

high

## 2.3.0.0.0 Request Response Metrics

- {'name': 'http_request_duration_seconds', 'type': 'histogram', 'unit': 'seconds', 'description': 'Histogram of HTTP request latencies, measured at the API Gateway and individual microservices.', 'dimensions': ['service', 'route', 'method', 'status_code'], 'percentiles': ['p95', 'p99'], 'collection': {'interval': '15s', 'method': 'Prometheus scrape from Kong and .NET services'}}

## 2.4.0.0.0 Availability Metrics

### 2.4.1.0.0 gauge

#### 2.4.1.1.0 Name

up

#### 2.4.1.2.0 Type

🔹 gauge

#### 2.4.1.3.0 Unit

boolean

#### 2.4.1.4.0 Description

Indicates if a target service endpoint is successfully scraped by Prometheus.

#### 2.4.1.5.0 Calculation

Generated automatically by Prometheus for each scrape target.

#### 2.4.1.6.0 Sla Target

99.9%

### 2.4.2.0.0 gauge

#### 2.4.2.1.0 Name

opc_client_connected_status

#### 2.4.2.2.0 Type

🔹 gauge

#### 2.4.2.3.0 Unit

boolean

#### 2.4.2.4.0 Description

A gauge indicating if an OPC Core Client has an active connection to the Central Management Plane.

#### 2.4.2.5.0 Calculation

Derived from the presence of a recent heartbeat message.

#### 2.4.2.6.0 Sla Target

N/A

## 2.5.0.0.0 Scalability Metrics

### 2.5.1.0.0 gauge

#### 2.5.1.1.0 Name

kube_hpa_status_current_replicas

#### 2.5.1.2.0 Type

🔹 gauge

#### 2.5.1.3.0 Unit

pods

#### 2.5.1.4.0 Description

The current number of running pods for a given deployment managed by an HPA.

#### 2.5.1.5.0 Capacity Threshold

N/A

#### 2.5.1.6.0 Auto Scaling Trigger

✅ Yes

### 2.5.2.0.0 gauge

#### 2.5.2.1.0 Name

mqtt_queue_depth

#### 2.5.2.2.0 Type

🔹 gauge

#### 2.5.2.3.0 Unit

messages

#### 2.5.2.4.0 Description

Number of messages queued in the MQTT broker for command and control.

#### 2.5.2.5.0 Capacity Threshold

> 1000

#### 2.5.2.6.0 Auto Scaling Trigger

❌ No

# 3.0.0.0.0 Application Specific Metrics Design

## 3.1.0.0.0 Transaction Metrics

### 3.1.1.0.0 counter

#### 3.1.1.1.0 Name

opc_values_ingested_total

#### 3.1.1.2.0 Type

🔹 counter

#### 3.1.1.3.0 Unit

values

#### 3.1.1.4.0 Description

Total number of time-series values successfully ingested into TimescaleDB.

#### 3.1.1.5.0 Business Context

Core data throughput of the system.

#### 3.1.1.6.0 Dimensions

- tenant_id

#### 3.1.1.7.0 Collection

##### 3.1.1.7.1 Interval

15s

##### 3.1.1.7.2 Method

Incremented by Data Ingestion Service

#### 3.1.1.8.0 Aggregation

##### 3.1.1.8.1 Functions

- rate

##### 3.1.1.8.2 Window

5m

### 3.1.2.0.0 counter

#### 3.1.2.1.0 Name

ai_inferences_total

#### 3.1.2.2.0 Type

🔹 counter

#### 3.1.2.3.0 Unit

inferences

#### 3.1.2.4.0 Description

Total number of AI/ML model inferences performed on edge devices.

#### 3.1.2.5.0 Business Context

Measures usage of the Edge AI feature.

#### 3.1.2.6.0 Dimensions

- tenant_id
- model_id
- model_version

#### 3.1.2.7.0 Collection

##### 3.1.2.7.1 Interval

60s

##### 3.1.2.7.2 Method

Exposed via /metrics on OPC Core Client

#### 3.1.2.8.0 Aggregation

##### 3.1.2.8.1 Functions

- rate

##### 3.1.2.8.2 Window

10m

## 3.2.0.0.0 Cache Performance Metrics

- {'name': 'redis_cache_hit_ratio', 'type': 'gauge', 'unit': 'ratio', 'description': 'The hit ratio of the Redis cache for asset and tag metadata.', 'cacheType': 'Redis', 'hitRatioTarget': '> 0.95'}

## 3.3.0.0.0 External Dependency Metrics

- {'name': 'db_query_duration_seconds', 'type': 'histogram', 'unit': 'seconds', 'description': 'Latency of queries executed against PostgreSQL/TimescaleDB.', 'dependency': 'TimescaleDB', 'circuitBreakerIntegration': False, 'sla': {'responseTime': '< 1s for 24h tag query (REQ-1-075)', 'availability': 'N/A'}}

## 3.4.0.0.0 Error Metrics

- {'name': 'grpc_server_handled_total', 'type': 'counter', 'unit': 'requests', 'description': 'Total number of gRPC requests handled by the Data Ingestion Service, labeled by status code.', 'errorTypes': ['UNAVAILABLE', 'INTERNAL', 'UNAUTHENTICATED'], 'dimensions': ['grpc_service', 'grpc_method', 'grpc_code'], 'alertThreshold': "rate(grpc_server_handled_total{grpc_code!='OK'}[5m]) > 10"}

## 3.5.0.0.0 Throughput And Latency Metrics

- {'name': 'grpc_server_handling_seconds', 'type': 'histogram', 'unit': 'seconds', 'description': 'Histogram of gRPC request processing time for the data ingestion pipeline.', 'percentiles': ['p95', 'p99'], 'buckets': ['0.005', '0.01', '0.025', '0.05', '0.1', '0.25', '0.5'], 'slaTargets': {'p95': 'N/A', 'p99': 'N/A'}}

# 4.0.0.0.0 Business Kpi Identification

## 4.1.0.0.0 Critical Business Metrics

- {'name': 'connected_opc_clients_total', 'type': 'gauge', 'unit': 'clients', 'description': 'Total number of OPC Core Client instances currently connected to the Central Management Plane.', 'businessOwner': 'Operations', 'calculation': 'count(opc_client_connected_status == 1)', 'reportingFrequency': 'real-time', 'target': 'As per customer license'}

## 4.2.0.0.0 User Engagement Metrics

- {'name': 'active_user_sessions', 'type': 'gauge', 'unit': 'sessions', 'description': 'Number of active user sessions in the Central Management Plane.', 'segmentation': ['tenant_id'], 'cohortAnalysis': False}

## 4.3.0.0.0 Conversion Metrics

*No items available*

## 4.4.0.0.0 Operational Efficiency Kpis

- {'name': 'opc_server_failovers_total', 'type': 'counter', 'unit': 'events', 'description': 'Total number of automatic failover events between redundant OPC servers at the edge.', 'calculation': 'Incremented by OPC Core Client on failover event.', 'benchmarkTarget': '< 1 per month per pair'}

## 4.5.0.0.0 Revenue And Cost Metrics

*No items available*

## 4.6.0.0.0 Customer Satisfaction Indicators

*No items available*

# 5.0.0.0.0 Collection Interval Optimization

## 5.1.0.0.0 Sampling Frequencies

### 5.1.1.0.0 Metric Category

#### 5.1.1.1.0 Metric Category

Critical API Performance

#### 5.1.1.2.0 Interval

15s

#### 5.1.1.3.0 Justification

Required to accurately calculate P95 latency for the 200ms API SLA in REQ-1-074.

#### 5.1.1.4.0 Resource Impact

medium

### 5.1.2.0.0 Metric Category

#### 5.1.2.1.0 Metric Category

Kubernetes Pod Resources

#### 5.1.2.2.0 Interval

30s

#### 5.1.2.3.0 Justification

Standard interval for HPA evaluation and general resource monitoring.

#### 5.1.2.4.0 Resource Impact

medium

### 5.1.3.0.0 Metric Category

#### 5.1.3.1.0 Metric Category

Edge Client Health

#### 5.1.3.2.0 Interval

60s

#### 5.1.3.3.0 Justification

Provides timely health status (REQ-1-062) without overwhelming edge devices or the network.

#### 5.1.3.4.0 Resource Impact

low

## 5.2.0.0.0 High Frequency Metrics

- {'name': 'http_request_duration_seconds', 'interval': '15s', 'criticality': 'high', 'costJustification': 'Essential for SLA compliance (REQ-1-074).'}

## 5.3.0.0.0 Cardinality Considerations

- {'metricName': 'http_request_duration_seconds', 'estimatedCardinality': 'medium', 'dimensionStrategy': 'Use fixed set of dimensions: service, route, method, status_code. Avoid user_id, request_id.', 'mitigationApproach': 'Route templates are used to keep cardinality low (e.g., /api/v1/assets/{assetId} becomes one dimension value, not one per ID).'}

## 5.4.0.0.0 Aggregation Periods

- {'metricType': 'Performance Latency (Histograms)', 'periods': ['1m', '5m', '30m'], 'retentionStrategy': 'Raw data for 15 days, aggregated data for 1 year.'}

## 5.5.0.0.0 Collection Methods

- {'method': 'real-time', 'applicableMetrics': ['http_request_duration_seconds', 'opc_values_ingested_total'], 'implementation': 'Prometheus scrape endpoints', 'performance': 'high'}

# 6.0.0.0.0 Aggregation Method Selection

## 6.1.0.0.0 Statistical Aggregations

- {'metricName': 'opc_values_ingested_total', 'aggregationFunctions': ['rate'], 'windows': ['1m', '5m'], 'justification': 'To calculate the ingestion rate (values/sec) to monitor against the 10,000 val/sec target in REQ-1-075.'}

## 6.2.0.0.0 Histogram Requirements

- {'metricName': 'http_request_duration_seconds', 'buckets': ['0.01', '0.05', '0.1', '0.2', '0.5', '1.0'], 'percentiles': ['p95', 'p99'], 'accuracy': 'High accuracy needed for the 200ms P95 target.'}

## 6.3.0.0.0 Percentile Calculations

- {'metricName': 'http_request_duration_seconds', 'percentiles': ['p95'], 'algorithm': 'histogram_quantile', 'accuracy': 'Sufficient for SLA monitoring.'}

## 6.4.0.0.0 Metric Types

### 6.4.1.0.0 opc_values_ingested_total

#### 6.4.1.1.0 Name

opc_values_ingested_total

#### 6.4.1.2.0 Implementation

counter

#### 6.4.1.3.0 Reasoning

This is a monotonically increasing value.

#### 6.4.1.4.0 Resets Handling

The rate() function in Prometheus automatically handles counter resets.

### 6.4.2.0.0 connected_opc_clients_total

#### 6.4.2.1.0 Name

connected_opc_clients_total

#### 6.4.2.2.0 Implementation

gauge

#### 6.4.2.3.0 Reasoning

This value can go up or down.

#### 6.4.2.4.0 Resets Handling

N/A

## 6.5.0.0.0 Dimensional Aggregation

- {'metricName': 'http_requests_total', 'dimensions': ['service', 'status_code'], 'aggregationStrategy': 'sum(rate(http_requests_total[5m])) by (service)', 'cardinalityImpact': 'low'}

## 6.6.0.0.0 Derived Metrics

- {'name': 'service:error_rate_5m', 'calculation': 'sum(rate(http_requests_total{status_code=~"5.."}[5m])) by (service) / sum(rate(http_requests_total[5m])) by (service)', 'sourceMetrics': ['http_requests_total'], 'updateFrequency': 'Every 30s'}

# 7.0.0.0.0 Storage Requirements Planning

## 7.1.0.0.0 Retention Periods

### 7.1.1.0.0 Metric Type

#### 7.1.1.1.0 Metric Type

High-resolution performance metrics

#### 7.1.1.2.0 Retention Period

15 days

#### 7.1.1.3.0 Justification

Sufficient for short-term debugging and incident response.

#### 7.1.1.4.0 Compliance Requirement

None

### 7.1.2.0.0 Metric Type

#### 7.1.2.1.0 Metric Type

Aggregated metrics (1h resolution)

#### 7.1.2.2.0 Retention Period

1 year

#### 7.1.2.3.0 Justification

Required for long-term capacity planning and trend analysis.

#### 7.1.2.4.0 Compliance Requirement

None

## 7.2.0.0.0 Data Resolution

### 7.2.1.0.0 Time Range

#### 7.2.1.1.0 Time Range

0-15 days

#### 7.2.1.2.0 Resolution

15s-30s (raw scrape interval)

#### 7.2.1.3.0 Query Performance

high

#### 7.2.1.4.0 Storage Optimization

None

### 7.2.2.0.0 Time Range

#### 7.2.2.1.0 Time Range

15 days - 1 year

#### 7.2.2.2.0 Resolution

1 hour

#### 7.2.2.3.0 Query Performance

medium

#### 7.2.2.4.0 Storage Optimization

Downsampling via Prometheus recording rules

## 7.3.0.0.0 Downsampling Strategies

- {'sourceResolution': '30s', 'targetResolution': '1h', 'aggregationMethod': 'avg, sum, max, quantile', 'triggerCondition': 'Prometheus recording rule evaluation cycle'}

## 7.4.0.0.0 Storage Performance

| Property | Value |
|----------|-------|
| Write Latency | < 10ms |
| Query Latency | < 1s for typical dashboard queries |
| Throughput Requirements | Able to handle scrapes from all services every 15s... |
| Scalability Needs | Must scale to support 10,000 edge clients and asso... |

## 7.5.0.0.0 Query Optimization

- {'queryPattern': 'Long-term trend analysis', 'optimizationStrategy': 'Query pre-aggregated data from recording rules instead of raw data.', 'indexingRequirements': ['Prometheus TSDB handles this automatically.']}

## 7.6.0.0.0 Cost Optimization

- {'strategy': 'Downsampling and Retention Policies', 'implementation': 'Use Prometheus recording rules to create aggregated metrics and set shorter retention for high-resolution raw data.', 'expectedSavings': 'Significant reduction in long-term storage costs.', 'tradeoffs': 'Loss of fine-grained historical data beyond the raw retention period.'}

# 8.0.0.0.0 Project Specific Metrics Config

*Not specified*

# 9.0.0.0.0 Implementation Priority

*Not specified*

# 10.0.0.0.0 Risk Assessment

*Not specified*

# 11.0.0.0.0 Recommendations

*Not specified*

