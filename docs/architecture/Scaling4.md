# 1 System Overview

## 1.1 Analysis Date

2025-06-13

## 1.2 Technology Stack

- .NET 8
- Kubernetes (EKS)
- PostgreSQL/TimescaleDB
- gRPC
- MQTT
- React

## 1.3 Architecture Patterns

- Microservices
- Edge Computing
- Hybrid Cloud/On-Premise

## 1.4 Resource Needs

- High-throughput data ingestion
- Real-time data processing
- Fleet management for edge devices
- Relational and Time-Series Database I/O

## 1.5 Performance Expectations

95th percentile (P95) API latency < 200ms; Ingestion of up to 10,000 values/sec per tenant; 99.9% uptime. (REQ-NFR-001, REQ-NFR-004)

## 1.6 Data Processing Volumes

High-volume time-series data streams from up to 10,000 edge clients. (REQ-NFR-005)

# 2.0 Workload Characterization

## 2.1 Processing Resource Consumption

### 2.1.1 Operation

#### 2.1.1.1 Operation

Data Ingestion Service (gRPC)

#### 2.1.1.2 Cpu Pattern

bursty

#### 2.1.1.3 Cpu Utilization

| Property | Value |
|----------|-------|
| Baseline | Low |
| Peak | High |
| Average | Moderate |

#### 2.1.1.4 Memory Pattern

steady

#### 2.1.1.5 Memory Requirements

| Property | Value |
|----------|-------|
| Baseline | Moderate |
| Peak | Moderate |
| Growth | Low |

#### 2.1.1.6 Io Characteristics

| Property | Value |
|----------|-------|
| Disk Iops | High (DB writes) |
| Network Throughput | Very High (gRPC streams) |
| Io Pattern | sequential |

### 2.1.2.0 Operation

#### 2.1.2.1 Operation

Query & Analytics Service (REST API)

#### 2.1.2.2 Cpu Pattern

event-driven

#### 2.1.2.3 Cpu Utilization

| Property | Value |
|----------|-------|
| Baseline | Low |
| Peak | High |
| Average | Low |

#### 2.1.2.4 Memory Pattern

fluctuating

#### 2.1.2.5 Memory Requirements

| Property | Value |
|----------|-------|
| Baseline | Moderate |
| Peak | High (large query results) |
| Growth | Low |

#### 2.1.2.6 Io Characteristics

| Property | Value |
|----------|-------|
| Disk Iops | High (DB reads) |
| Network Throughput | High |
| Io Pattern | random |

### 2.1.3.0 Operation

#### 2.1.3.1 Operation

General API Microservices (IAM, Tenant, Asset, etc.)

#### 2.1.3.2 Cpu Pattern

event-driven

#### 2.1.3.3 Cpu Utilization

| Property | Value |
|----------|-------|
| Baseline | Low |
| Peak | Moderate |
| Average | Low |

#### 2.1.3.4 Memory Pattern

steady

#### 2.1.3.5 Memory Requirements

| Property | Value |
|----------|-------|
| Baseline | Low |
| Peak | Moderate |
| Growth | Low |

#### 2.1.3.6 Io Characteristics

| Property | Value |
|----------|-------|
| Disk Iops | Low-Moderate |
| Network Throughput | Moderate |
| Io Pattern | mixed |

## 2.2.0.0 Concurrency Requirements

### 2.2.1.0 Operation

#### 2.2.1.1 Operation

User API Access

#### 2.2.1.2 Max Concurrent Jobs

1,000

#### 2.2.1.3 Thread Pool Size

0

#### 2.2.1.4 Connection Pool Size

50

#### 2.2.1.5 Queue Depth

0

### 2.2.2.0 Operation

#### 2.2.2.1 Operation

Data Ingestion Stream

#### 2.2.2.2 Max Concurrent Jobs

10,000

#### 2.2.2.3 Thread Pool Size

0

#### 2.2.2.4 Connection Pool Size

100

#### 2.2.2.5 Queue Depth

0

## 2.3.0.0 Database Access Patterns

### 2.3.1.0 Access Type

#### 2.3.1.1 Access Type

write-heavy

#### 2.3.1.2 Connection Requirements

High for TimescaleDB

#### 2.3.1.3 Query Complexity

simple

#### 2.3.1.4 Transaction Volume

Very High

#### 2.3.1.5 Cache Hit Ratio

N/A

### 2.3.2.0 Access Type

#### 2.3.2.1 Access Type

read-heavy

#### 2.3.2.2 Connection Requirements

High for TimescaleDB

#### 2.3.2.3 Query Complexity

complex

#### 2.3.2.4 Transaction Volume

High

#### 2.3.2.5 Cache Hit Ratio

High (for metadata)

## 2.4.0.0 Frontend Resource Demands

*No items available*

## 2.5.0.0 Load Patterns

- {'pattern': 'peak-trough', 'description': 'User activity follows typical business hours, while data ingestion from edge clients is expected to be more constant (24/7).', 'frequency': 'Daily', 'magnitude': 'High variation for user-facing APIs, steady for ingestion.', 'predictability': 'high'}

# 3.0.0.0 Scaling Strategy Design

## 3.1.0.0 Scaling Approaches

### 3.1.1.0 Component

#### 3.1.1.1 Component

All Cloud Microservices

#### 3.1.1.2 Primary Strategy

horizontal

#### 3.1.1.3 Justification

Requirement REQ-NFR-005 explicitly states that all cloud microservices shall be stateless and designed to scale horizontally using Kubernetes Horizontal Pod Autoscalers.

#### 3.1.1.4 Limitations

- Dependent on downstream database scalability.
- Potential for cold starts.
- Requires robust service discovery.

#### 3.1.1.5 Implementation

Kubernetes Horizontal Pod Autoscaler (HPA)

### 3.1.2.0 Component

#### 3.1.2.1 Component

Databases (PostgreSQL/TimescaleDB)

#### 3.1.2.2 Primary Strategy

vertical

#### 3.1.2.3 Justification

Relational databases are stateful and typically scale vertically (increasing instance size) for performance. Read scalability can be achieved horizontally with read replicas.

#### 3.1.2.4 Limitations

- Vertical scaling requires downtime or a failover event.
- Finite upper limit on instance size.

#### 3.1.2.5 Implementation

AWS RDS instance type modification and configuration of read replicas (as per REQ-NFR-002).

## 3.2.0.0 Instance Specifications

### 3.2.1.0 Workload Type

#### 3.2.1.1 Workload Type

Data Ingestion Service

#### 3.2.1.2 Instance Family

Compute Optimized (e.g., AWS c-series)

#### 3.2.1.3 Instance Size

c6i.xlarge

#### 3.2.1.4 V Cpus

4

#### 3.2.1.5 Memory Gb

8

#### 3.2.1.6 Storage Type

gp3

#### 3.2.1.7 Network Performance

High

#### 3.2.1.8 Optimization

compute

### 3.2.2.0 Workload Type

#### 3.2.2.1 Workload Type

Query & Analytics Service

#### 3.2.2.2 Instance Family

Memory Optimized (e.g., AWS r-series)

#### 3.2.2.3 Instance Size

r6i.large

#### 3.2.2.4 V Cpus

2

#### 3.2.2.5 Memory Gb

16

#### 3.2.2.6 Storage Type

gp3

#### 3.2.2.7 Network Performance

Moderate

#### 3.2.2.8 Optimization

memory

### 3.2.3.0 Workload Type

#### 3.2.3.1 Workload Type

General API Services

#### 3.2.3.2 Instance Family

General Purpose (e.g., AWS m-series)

#### 3.2.3.3 Instance Size

m6i.large

#### 3.2.3.4 V Cpus

2

#### 3.2.3.5 Memory Gb

8

#### 3.2.3.6 Storage Type

gp3

#### 3.2.3.7 Network Performance

Moderate

#### 3.2.3.8 Optimization

balanced

## 3.3.0.0 Multithreading Considerations

*No items available*

## 3.4.0.0 Specialized Hardware

- {'requirement': 'gpu', 'justification': 'Required for the Edge AI Module running on NVIDIA Jetson hardware as per REQ-ENV-001. This is an on-premise/edge requirement, not a cloud scaling factor.', 'availability': 'On-Premise', 'costImplications': 'Customer CAPEX'}

## 3.5.0.0 Storage Scaling

- {'storageType': 'database', 'scalingMethod': 'vertical', 'performance': 'Provisioned IOPS on AWS RDS', 'consistency': 'Strong'}

## 3.6.0.0 Licensing Implications

*No items available*

# 4.0.0.0 Auto Scaling Trigger Metrics

## 4.1.0.0 Cpu Utilization Triggers

- {'component': 'All Cloud Microservices', 'scaleUpThreshold': 70, 'scaleDownThreshold': 40, 'evaluationPeriods': 3, 'dataPoints': 2, 'justification': 'Primary metric for scaling stateless compute workloads, directly supporting the horizontal scaling requirement of REQ-NFR-005.'}

## 4.2.0.0 Memory Consumption Triggers

- {'component': 'Query & Analytics Service', 'scaleUpThreshold': 75, 'scaleDownThreshold': 45, 'evaluationPeriods': 2, 'triggerCondition': 'used', 'justification': 'This service may handle large query results, making memory a key indicator of load and potential performance degradation.'}

## 4.3.0.0 Database Connection Triggers

*No items available*

## 4.4.0.0 Queue Length Triggers

- {'queueType': 'message', 'scaleUpThreshold': 100, 'scaleDownThreshold': 10, 'ageThreshold': '60s', 'priority': 'normal'}

## 4.5.0.0 Response Time Triggers

*No items available*

## 4.6.0.0 Custom Metric Triggers

- {'metricName': 'grpc_active_streams', 'description': 'Number of active gRPC streams on the Data Ingestion Service.', 'scaleUpThreshold': 50, 'scaleDownThreshold': 20, 'calculation': 'A gauge metric incremented/decremented by the service on stream start/end.', 'businessJustification': 'Provides a more direct measure of ingestion load than CPU, allowing for proactive scaling before CPU saturation occurs to meet the high-throughput requirement of REQ-NFR-001.'}

## 4.7.0.0 Disk Iotriggers

*No items available*

# 5.0.0.0 Scaling Limits And Safeguards

## 5.1.0.0 Instance Limits

- {'component': 'All Cloud Microservices', 'minInstances': 2, 'maxInstances': 20, 'justification': 'Minimum of 2 instances ensures high availability (supports 99.9% uptime REQ-NFR-004) across availability zones. Maximum of 20 provides a cap to control costs and prevent runaway scaling.', 'costImplication': 'Sets a predictable baseline and maximum operational cost for compute.'}

## 5.2.0.0 Cooldown Periods

### 5.2.1.0 Action

#### 5.2.1.1 Action

scale-up

#### 5.2.1.2 Duration

60s

#### 5.2.1.3 Reasoning

Allows new pods sufficient time to become ready and start processing traffic before another scale-up event is considered.

#### 5.2.1.4 Component

All Cloud Microservices

### 5.2.2.0 Action

#### 5.2.2.1 Action

scale-down

#### 5.2.2.2 Duration

300s

#### 5.2.2.3 Reasoning

Prevents premature scaling down during brief lulls in traffic, avoiding 'flapping' where pods are frequently added and removed.

#### 5.2.2.4 Component

All Cloud Microservices

## 5.3.0.0 Scaling Step Sizes

*No items available*

## 5.4.0.0 Runaway Protection

- {'safeguard': 'max-scaling-rate', 'implementation': 'Kubernetes HPA policy configuration.', 'trigger': 'Configuring `scaleUp.stabilizationWindowSeconds` and `policies` in HPA v2.', 'action': 'Limit the number of pods that can be added in a given time window.'}

## 5.5.0.0 Graceful Degradation

*No items available*

## 5.6.0.0 Resource Quotas

### 5.6.1.0 Environment

#### 5.6.1.1 Environment

production

#### 5.6.1.2 Quota Type

cpu

#### 5.6.1.3 Limit

200 vCPU

#### 5.6.1.4 Enforcement

hard

### 5.6.2.0 Environment

#### 5.6.2.1 Environment

production

#### 5.6.2.2 Quota Type

memory

#### 5.6.2.3 Limit

512Gi

#### 5.6.2.4 Enforcement

hard

## 5.7.0.0 Workload Prioritization

*No items available*

# 6.0.0.0 Cost Optimization Strategy

## 6.1.0.0 Instance Right Sizing

*No items available*

## 6.2.0.0 Time Based Scaling

*No items available*

## 6.3.0.0 Instance Termination Policies

*No items available*

## 6.4.0.0 Spot Instance Strategies

- {'component': 'Data Ingestion Service', 'spotPercentage': 50, 'fallbackStrategy': 'Automatically fall back to on-demand instances if spot capacity is unavailable.', 'interruptionHandling': 'Pod disruption budgets and graceful shutdown procedures to ensure in-flight data is processed before termination.', 'costSavings': 'Potentially significant savings on the most resource-intensive stateless workload.'}

## 6.5.0.0 Reserved Instance Planning

- {'instanceType': 'General Purpose, Memory Optimized', 'reservationTerm': '1-year', 'utilizationForecast': 'High (baseline load)', 'baselineInstances': 2, 'paymentOption': 'partial-upfront'}

## 6.6.0.0 Resource Tracking

*No items available*

## 6.7.0.0 Cleanup Policies

*No items available*

# 7.0.0.0 Load Testing And Validation

## 7.1.0.0 Baseline Metrics

### 7.1.1.0 Metric

#### 7.1.1.1 Metric

P95 API Latency

#### 7.1.1.2 Baseline Value

< 200ms

#### 7.1.1.3 Acceptable Variation

10%

#### 7.1.1.4 Measurement Method

Automated load tests as per REQ-NFR-008.

### 7.1.2.0 Metric

#### 7.1.2.1 Metric

Data Ingestion Rate

#### 7.1.2.2 Baseline Value

10,000 values/sec

#### 7.1.2.3 Acceptable Variation

5%

#### 7.1.2.4 Measurement Method

Automated load tests.

## 7.2.0.0 Validation Procedures

- {'procedure': 'Execute automated load tests against the staging environment as part of the CI/CD pipeline.', 'frequency': 'Pre-deployment', 'successCriteria': ['Meet performance targets defined in REQ-NFR-001.', 'Scaling actions trigger at expected thresholds.', 'System remains stable under sustained peak load.'], 'failureActions': ['Fail the CI/CD pipeline.', 'Alert the development team.']}

## 7.3.0.0 Synthetic Load Scenarios

*No items available*

## 7.4.0.0 Scaling Event Monitoring

*No items available*

## 7.5.0.0 Policy Refinement

*No items available*

## 7.6.0.0 Effectiveness Kpis

*No items available*

## 7.7.0.0 Feedback Mechanisms

*No items available*

# 8.0.0.0 Project Specific Scaling Policies

## 8.1.0.0 Policies

### 8.1.1.0 Horizontal

#### 8.1.1.1 Id

api-services-hpa

#### 8.1.1.2 Type

🔹 Horizontal

#### 8.1.1.3 Component

General API Microservices (IAM, Tenant, Asset)

#### 8.1.1.4 Rules

- {'metric': 'CPU Utilization', 'threshold': 70, 'operator': 'GREATER_THAN', 'scaleChange': 0, 'cooldown': {'scaleUpSeconds': 60, 'scaleDownSeconds': 300}, 'evaluationPeriods': 3, 'dataPointsToAlarm': 2}

#### 8.1.1.5 Safeguards

| Property | Value |
|----------|-------|
| Min Instances | 2 |
| Max Instances | 20 |
| Max Scaling Rate | 4 pods per minute |
| Cost Threshold |  |

#### 8.1.1.6 Schedule

##### 8.1.1.6.1 Enabled

❌ No

##### 8.1.1.6.2 Timezone



##### 8.1.1.6.3 Rules

*No items available*

### 8.1.2.0.0 Horizontal

#### 8.1.2.1.0 Id

ingestion-service-hpa

#### 8.1.2.2.0 Type

🔹 Horizontal

#### 8.1.2.3.0 Component

Data Ingestion Service

#### 8.1.2.4.0 Rules

##### 8.1.2.4.1 Metric

###### 8.1.2.4.1.1 Metric

CPU Utilization

###### 8.1.2.4.1.2 Threshold

80

###### 8.1.2.4.1.3 Operator

GREATER_THAN

###### 8.1.2.4.1.4 Scale Change

0

###### 8.1.2.4.1.5 Cooldown

####### 8.1.2.4.1.5.1 Scale Up Seconds

60

####### 8.1.2.4.1.5.2 Scale Down Seconds

300

###### 8.1.2.4.1.6.0 Evaluation Periods

2

###### 8.1.2.4.1.7.0 Data Points To Alarm

2

##### 8.1.2.4.2.0.0 Metric

###### 8.1.2.4.2.1.0 Metric

grpc_active_streams (Custom Metric)

###### 8.1.2.4.2.2.0 Threshold

50

###### 8.1.2.4.2.3.0 Operator

GREATER_THAN

###### 8.1.2.4.2.4.0 Scale Change

0

###### 8.1.2.4.2.5.0 Cooldown

####### 8.1.2.4.2.5.1 Scale Up Seconds

60

####### 8.1.2.4.2.5.2 Scale Down Seconds

300

###### 8.1.2.4.2.6.0 Evaluation Periods

2

###### 8.1.2.4.2.7.0 Data Points To Alarm

2

#### 8.1.2.5.0.0.0 Safeguards

| Property | Value |
|----------|-------|
| Min Instances | 2 |
| Max Instances | 50 |
| Max Scaling Rate | 10 pods per minute |
| Cost Threshold |  |

#### 8.1.2.6.0.0.0 Schedule

##### 8.1.2.6.1.0.0 Enabled

❌ No

##### 8.1.2.6.2.0.0 Timezone



##### 8.1.2.6.3.0.0 Rules

*No items available*

## 8.2.0.0.0.0.0 Configuration

### 8.2.1.0.0.0.0 Min Instances

2

### 8.2.2.0.0.0.0 Max Instances

50

### 8.2.3.0.0.0.0 Default Timeout

30s

### 8.2.4.0.0.0.0 Region

Per Tenant Agreement (REQ-CON-003)

### 8.2.5.0.0.0.0 Resource Group



### 8.2.6.0.0.0.0 Notification Endpoint



### 8.2.7.0.0.0.0 Logging Level

INFO

### 8.2.8.0.0.0.0 Vpc Id



### 8.2.9.0.0.0.0 Instance Type

Variable

### 8.2.10.0.0.0.0 Enable Detailed Monitoring

true

### 8.2.11.0.0.0.0 Scaling Mode

reactive

### 8.2.12.0.0.0.0 Cost Optimization

| Property | Value |
|----------|-------|
| Spot Instances Enabled | ✅ |
| Spot Percentage | 50 |
| Reserved Instances Planned | ✅ |

### 8.2.13.0.0.0.0 Performance Targets

| Property | Value |
|----------|-------|
| Response Time | <200ms |
| Throughput | 10000 vps |
| Availability | 99.9% |

## 8.3.0.0.0.0.0 Environment Specific Policies

### 8.3.1.0.0.0.0 Environment

#### 8.3.1.1.0.0.0 Environment

production

#### 8.3.1.2.0.0.0 Scaling Enabled

✅ Yes

#### 8.3.1.3.0.0.0 Aggressiveness

moderate

#### 8.3.1.4.0.0.0 Cost Priority

balanced

### 8.3.2.0.0.0.0 Environment

#### 8.3.2.1.0.0.0 Environment

staging

#### 8.3.2.2.0.0.0 Scaling Enabled

✅ Yes

#### 8.3.2.3.0.0.0 Aggressiveness

aggressive

#### 8.3.2.4.0.0.0 Cost Priority

performance

### 8.3.3.0.0.0.0 Environment

#### 8.3.3.1.0.0.0 Environment

development

#### 8.3.3.2.0.0.0 Scaling Enabled

❌ No

#### 8.3.3.3.0.0.0 Aggressiveness

conservative

#### 8.3.3.4.0.0.0 Cost Priority

cost-optimized

# 9.0.0.0.0.0.0 Implementation Priority

## 9.1.0.0.0.0.0 Component

### 9.1.1.0.0.0.0 Component

Baseline HPA for General API Services

### 9.1.2.0.0.0.0 Priority

🔴 high

### 9.1.3.0.0.0.0 Dependencies

- EKS Cluster Setup
- Prometheus Metrics Server

### 9.1.4.0.0.0.0 Estimated Effort

Low

### 9.1.5.0.0.0.0 Risk Level

low

## 9.2.0.0.0.0.0 Component

### 9.2.1.0.0.0.0 Component

Advanced HPA for Data Ingestion Service

### 9.2.2.0.0.0.0 Priority

🔴 high

### 9.2.3.0.0.0.0 Dependencies

- Baseline HPA
- Application-level custom metrics implementation

### 9.2.4.0.0.0.0 Estimated Effort

Medium

### 9.2.5.0.0.0.0 Risk Level

medium

## 9.3.0.0.0.0.0 Component

### 9.3.1.0.0.0.0 Component

Database Read Replica Strategy

### 9.3.2.0.0.0.0 Priority

🟡 medium

### 9.3.3.0.0.0.0 Dependencies

- RDS Deployment

### 9.3.4.0.0.0.0 Estimated Effort

Medium

### 9.3.5.0.0.0.0 Risk Level

low

## 9.4.0.0.0.0.0 Component

### 9.4.1.0.0.0.0 Component

Cost Optimization (Spot/Reserved Instances)

### 9.4.2.0.0.0.0 Priority

🟢 low

### 9.4.3.0.0.0.0 Dependencies

- Stable production workload analysis

### 9.4.4.0.0.0.0 Estimated Effort

Medium

### 9.4.5.0.0.0.0 Risk Level

medium

# 10.0.0.0.0.0.0 Risk Assessment

## 10.1.0.0.0.0.0 Risk

### 10.1.1.0.0.0.0 Risk

Misconfigured scaling policies lead to excessive costs.

### 10.1.2.0.0.0.0 Impact

high

### 10.1.3.0.0.0.0 Probability

medium

### 10.1.4.0.0.0.0 Mitigation

Set hard maximum instance limits (maxInstances) on all HPAs. Implement budget alerts in the cloud provider console.

### 10.1.5.0.0.0.0 Contingency Plan

Manually scale down workloads and revise HPA thresholds.

## 10.2.0.0.0.0.0 Risk

### 10.2.1.0.0.0.0 Risk

Scaling is too slow to react to a sudden load spike, causing performance degradation and violating SLOs.

### 10.2.2.0.0.0.0 Impact

high

### 10.2.3.0.0.0.0 Probability

medium

### 10.2.4.0.0.0.0 Mitigation

Tune HPA evaluation periods and scale-up policies to be more aggressive. Maintain a higher minimum number of instances to absorb initial bursts.

### 10.2.5.0.0.0.0 Contingency Plan

Manually increase the number of pod replicas during the event.

## 10.3.0.0.0.0.0 Risk

### 10.3.1.0.0.0.0 Risk

Database becomes a bottleneck, preventing stateless services from scaling effectively.

### 10.3.2.0.0.0.0 Impact

high

### 10.3.3.0.0.0.0 Probability

high

### 10.3.4.0.0.0.0 Mitigation

Implement database connection pooling, optimize queries, and deploy read replicas for read-heavy workloads. Monitor database performance metrics closely.

### 10.3.5.0.0.0.0 Contingency Plan

Vertically scale the primary database instance during an emergency.

# 11.0.0.0.0.0.0 Recommendations

## 11.1.0.0.0.0.0 Category

### 11.1.1.0.0.0.0 Category

🔹 Implementation

### 11.1.2.0.0.0.0 Recommendation

Implement Kubernetes Pod Disruption Budgets (PDBs) for all microservices.

### 11.1.3.0.0.0.0 Justification

Ensures that a minimum number of replicas are always running during voluntary disruptions like node upgrades or maintenance, which is critical for meeting the 99.9% availability requirement (REQ-NFR-004).

### 11.1.4.0.0.0.0 Priority

🔴 high

### 11.1.5.0.0.0.0 Implementation Notes

Set `minAvailable` to at least 1 for deployments with `minInstances` of 2.

## 11.2.0.0.0.0.0 Category

### 11.2.1.0.0.0.0 Category

🔹 Testing

### 11.2.2.0.0.0.0 Recommendation

Conduct regular load testing that specifically targets the scaling triggers.

### 11.2.3.0.0.0.0 Justification

Validates that the defined scaling policies are effective and react as expected under realistic conditions, ensuring the system can meet its performance and scalability NFRs (REQ-NFR-001, REQ-NFR-005).

### 11.2.4.0.0.0.0 Priority

🔴 high

### 11.2.5.0.0.0.0 Implementation Notes

Integrate load testing into the CI/CD pipeline against a dedicated staging environment as per REQ-NFR-008.

## 11.3.0.0.0.0.0 Category

### 11.3.1.0.0.0.0 Category

🔹 Cost Optimization

### 11.3.2.0.0.0.0 Recommendation

Use a tool like Karpenter or the Cluster Autoscaler for node-level scaling in EKS.

### 11.3.3.0.0.0.0 Justification

While HPA scales pods, a cluster/node autoscaler is essential for adding or removing EC2 instances to the cluster as needed. This ensures that you only pay for the underlying compute resources required to run your pods, which is a key component of an efficient, scalable deployment.

### 11.3.4.0.0.0.0 Priority

🟡 medium

### 11.3.5.0.0.0.0 Implementation Notes

Configure node pools with a mix of on-demand and spot instances to align with the cost optimization strategy.

