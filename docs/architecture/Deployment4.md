# 1 System Overview

## 1.1 Analysis Date

2025-06-13

## 1.2 Technology Stack

- AWS EKS
- .NET 8
- React 18
- PostgreSQL 16 / TimescaleDB
- Redis 7
- Docker
- gRPC
- MQTT v5
- Terraform

## 1.3 Architecture Patterns

- Microservices
- Edge Computing
- Multi-Tenant SaaS
- API Gateway

## 1.4 Data Handling Needs

- High-volume time-series data
- Relational configuration data
- Personally Identifiable Information (PII)
- Immutable audit logs

## 1.5 Performance Expectations

High-throughput (10,000 values/sec/tenant), low-latency (<200ms API P95) system with 99.9% uptime for the cloud plane.

## 1.6 Regulatory Requirements

- GDPR (General Data Protection Regulation)
- FDA 21 CFR Part 11

# 2.0 Environment Strategy

## 2.1 Environment Types

### 2.1.1 Development

#### 2.1.1.1 Type

🔹 Development

#### 2.1.1.2 Purpose

Used by developers for feature development, unit testing, and local debugging.

#### 2.1.1.3 Usage Patterns

- CI builds
- Individual developer sandboxes

#### 2.1.1.4 Isolation Level

partial

#### 2.1.1.5 Data Policy

Seeded or synthetic data only. No production data allowed.

#### 2.1.1.6 Lifecycle Management

Ephemeral; can be torn down and recreated on demand by developers.

### 2.1.2.0 Testing

#### 2.1.2.1 Type

🔹 Testing

#### 2.1.2.2 Purpose

Dedicated environment for the QA team to perform integration testing, regression testing, and quality assurance.

#### 2.1.2.3 Usage Patterns

- Automated test suites
- Manual QA validation

#### 2.1.2.4 Isolation Level

complete

#### 2.1.2.5 Data Policy

Controlled, curated set of synthetic data designed to cover all test cases.

#### 2.1.2.6 Lifecycle Management

Persistent, updated on a regular schedule from the development branch.

### 2.1.3.0 Staging

#### 2.1.3.1 Type

🔹 Staging

#### 2.1.3.2 Purpose

A production-like environment for User Acceptance Testing (UAT), performance/load testing, and final validation before release.

#### 2.1.3.3 Usage Patterns

- UAT by stakeholders
- Pre-release performance testing (REQ-NFR-008)
- Deployment rehearsals

#### 2.1.3.4 Isolation Level

complete

#### 2.1.3.5 Data Policy

Anonymized and masked replica of production data. PII must be fully redacted.

#### 2.1.3.6 Lifecycle Management

Persistent, mirrors production infrastructure and configuration as closely as possible.

### 2.1.4.0 Production

#### 2.1.4.1 Type

🔹 Production

#### 2.1.4.2 Purpose

Live environment for serving customer traffic and processing real data.

#### 2.1.4.3 Usage Patterns

- Customer-facing services
- Real-time data ingestion and processing

#### 2.1.4.4 Isolation Level

complete

#### 2.1.4.5 Data Policy

Live customer data, subject to all security and compliance controls.

#### 2.1.4.6 Lifecycle Management

Highly controlled, persistent environment with strict change management (MOC).

### 2.1.5.0 DR

#### 2.1.5.1 Type

🔹 DR

#### 2.1.5.2 Purpose

Disaster Recovery environment in a separate geographic region for failover in case of a regional outage.

#### 2.1.5.3 Usage Patterns

- Cold/Warm standby
- Periodic DR testing

#### 2.1.5.4 Isolation Level

complete

#### 2.1.5.5 Data Policy

Asynchronously replicated production data.

#### 2.1.5.6 Lifecycle Management

Persistent, kept in sync with production within the defined RPO.

## 2.2.0.0 Promotion Strategy

### 2.2.1.0 Workflow

GitFlow-based: Feature Branch -> Develop (Dev) -> Release Branch (Testing/Staging) -> Main (Production)

### 2.2.2.0 Approval Gates

- Automated tests pass in 'Testing'
- UAT sign-off in 'Staging'
- Management of Change (MOC) approval for 'Production' deployment

### 2.2.3.0 Automation Level

automated

### 2.2.4.0 Rollback Procedure

Automated rollback to the previously deployed version via GitHub Actions if health checks fail post-deployment.

## 2.3.0.0 Isolation Strategies

### 2.3.1.0 Environment

#### 2.3.1.1 Environment

Production

#### 2.3.1.2 Isolation Type

complete

#### 2.3.1.3 Implementation

Dedicated AWS Account, separate VPC, distinct IAM roles and security principals.

#### 2.3.1.4 Justification

Maximum security and blast radius reduction for customer data.

### 2.3.2.0 Environment

#### 2.3.2.1 Environment

Staging

#### 2.3.2.2 Isolation Type

complete

#### 2.3.2.3 Implementation

Dedicated AWS Account, separate VPC. No shared resources with Production.

#### 2.3.2.4 Justification

Ensures accurate performance testing and prevents any accidental impact on production.

### 2.3.3.0 Environment

#### 2.3.3.1 Environment

Development/Testing

#### 2.3.3.2 Isolation Type

network

#### 2.3.3.3 Implementation

Can coexist in a single AWS 'Non-Prod' account but must be in separate VPCs or use distinct subnets and security groups.

#### 2.3.3.4 Justification

Balances cost-effectiveness with necessary separation of concerns for development and QA activities.

## 2.4.0.0 Scaling Approaches

### 2.4.1.0 Environment

#### 2.4.1.1 Environment

Production

#### 2.4.1.2 Scaling Type

auto

#### 2.4.1.3 Triggers

- CPU Utilization
- Memory Utilization
- Custom metrics (e.g., gRPC stream count)

#### 2.4.1.4 Limits

Configured per service, up to 10,000 clients (REQ-NFR-005).

### 2.4.2.0 Environment

#### 2.4.2.1 Environment

Staging

#### 2.4.2.2 Scaling Type

auto

#### 2.4.2.3 Triggers

- CPU Utilization
- Memory Utilization

#### 2.4.2.4 Limits

Mirrors production limits to ensure accurate performance testing.

### 2.4.3.0 Environment

#### 2.4.3.1 Environment

Development/Testing

#### 2.4.3.2 Scaling Type

vertical

#### 2.4.3.3 Triggers

- Manual scaling by developers/QA for specific tests.

#### 2.4.3.4 Limits

Limited to smaller instance types to control costs.

## 2.5.0.0 Provisioning Automation

| Property | Value |
|----------|-------|
| Tool | terraform |
| Templating | Reusable Terraform modules for components like EKS... |
| State Management | Remote state management using Amazon S3 with state... |
| Cicd Integration | ✅ |

# 3.0.0.0 Resource Requirements Analysis

## 3.1.0.0 Workload Analysis

### 3.1.1.0 Workload Type

#### 3.1.1.1 Workload Type

Data Ingestion

#### 3.1.1.2 Expected Load

High, sustained (up to 10,000 values/sec/tenant).

#### 3.1.1.3 Peak Capacity

Must support 10,000 concurrent client connections.

#### 3.1.1.4 Resource Profile

cpu-intensive

### 3.1.2.0 Workload Type

#### 3.1.2.1 Workload Type

API Gateway & Services

#### 3.1.2.2 Expected Load

Medium, bursty. Up to 1,000 concurrent users/tenant.

#### 3.1.2.3 Peak Capacity

P95 latency < 200ms.

#### 3.1.2.4 Resource Profile

balanced

### 3.1.3.0 Workload Type

#### 3.1.3.1 Workload Type

Data Query & Reporting

#### 3.1.3.2 Expected Load

Low to Medium, analytical queries.

#### 3.1.3.3 Peak Capacity

Return 24h of data in < 1s.

#### 3.1.3.4 Resource Profile

memory-intensive

### 3.1.4.0 Workload Type

#### 3.1.4.1 Workload Type

OPC Core Client (Edge)

#### 3.1.4.2 Expected Load

Varies by site, typically high CPU for data processing and AI.

#### 3.1.4.3 Peak Capacity

AI inference < 100ms.

#### 3.1.4.4 Resource Profile

cpu-intensive

## 3.2.0.0 Compute Requirements

### 3.2.1.0 Environment

#### 3.2.1.1 Environment

Production

#### 3.2.1.2 Instance Type

Mix of General Purpose (e.g., AWS EC2 m6i.xlarge) and Compute Optimized (e.g., c6i.xlarge) for EKS worker nodes.

#### 3.2.1.3 Cpu Cores

4

#### 3.2.1.4 Memory Gb

16

#### 3.2.1.5 Instance Count

3

#### 3.2.1.6 Auto Scaling

##### 3.2.1.6.1 Enabled

✅ Yes

##### 3.2.1.6.2 Min Instances

3

##### 3.2.1.6.3 Max Instances

20

##### 3.2.1.6.4 Scaling Triggers

- Cluster CPU > 70%

#### 3.2.1.7.0 Justification

Provides a baseline for high availability and performance, with autoscaling to handle peak loads as per REQ-NFR-005.

### 3.2.2.0.0 Environment

#### 3.2.2.1.0 Environment

Staging

#### 3.2.2.2.0 Instance Type

Identical to Production to ensure valid performance testing.

#### 3.2.2.3.0 Cpu Cores

4

#### 3.2.2.4.0 Memory Gb

16

#### 3.2.2.5.0 Instance Count

2

#### 3.2.2.6.0 Auto Scaling

##### 3.2.2.6.1 Enabled

✅ Yes

##### 3.2.2.6.2 Min Instances

2

##### 3.2.2.6.3 Max Instances

10

##### 3.2.2.6.4 Scaling Triggers

- Cluster CPU > 70%

#### 3.2.2.7.0 Justification

Must be a functional replica of production for UAT and load testing (REQ-NFR-008).

### 3.2.3.0.0 Environment

#### 3.2.3.1.0 Environment

Development/Testing

#### 3.2.3.2.0 Instance Type

General Purpose, smaller size (e.g., AWS EC2 t3.large).

#### 3.2.3.3.0 Cpu Cores

2

#### 3.2.3.4.0 Memory Gb

8

#### 3.2.3.5.0 Instance Count

1

#### 3.2.3.6.0 Auto Scaling

##### 3.2.3.6.1 Enabled

❌ No

##### 3.2.3.6.2 Min Instances

1

##### 3.2.3.6.3 Max Instances

1

##### 3.2.3.6.4 Scaling Triggers

*No items available*

#### 3.2.3.7.0 Justification

Cost-effective configuration sufficient for development and functional testing workloads.

## 3.3.0.0.0 Storage Requirements

### 3.3.1.0.0 Environment

#### 3.3.1.1.0 Environment

Production

#### 3.3.1.2.0 Storage Type

block

#### 3.3.1.3.0 Capacity

AWS RDS: Start with 1TB (auto-scaling enabled).

#### 3.3.1.4.0 Iops Requirements

Provisioned IOPS (io2) for TimescaleDB to handle high ingestion rates.

#### 3.3.1.5.0 Throughput Requirements

High

#### 3.3.1.6.0 Redundancy

Multi-AZ RDS deployment.

#### 3.3.1.7.0 Encryption

✅ Yes

### 3.3.2.0.0 Environment

#### 3.3.2.1.0 Environment

Production

#### 3.3.2.2.0 Storage Type

object

#### 3.3.2.3.0 Capacity

Amazon S3: Scalable to Petabytes.

#### 3.3.2.4.0 Iops Requirements

N/A

#### 3.3.2.5.0 Throughput Requirements

High

#### 3.3.2.6.0 Redundancy

Cross-region replication for DR.

#### 3.3.2.7.0 Encryption

✅ Yes

### 3.3.3.0.0 Environment

#### 3.3.3.1.0 Environment

Staging

#### 3.3.3.2.0 Storage Type

block

#### 3.3.3.3.0 Capacity

AWS RDS: Start with 500GB.

#### 3.3.3.4.0 Iops Requirements

Provisioned IOPS (io2) to match production performance characteristics.

#### 3.3.3.5.0 Throughput Requirements

High

#### 3.3.3.6.0 Redundancy

Multi-AZ RDS deployment.

#### 3.3.3.7.0 Encryption

✅ Yes

## 3.4.0.0.0 Special Hardware Requirements

- {'requirement': 'gpu', 'justification': 'Required for Edge AI model execution on the OPC Core Client (REQ-ENV-001).', 'environment': 'Edge/On-Premise', 'specifications': 'NVIDIA Jetson series or equivalent.'}

## 3.5.0.0.0 Scaling Strategies

- {'environment': 'Production', 'strategy': 'reactive', 'implementation': 'Kubernetes Horizontal Pod Autoscalers (HPA) for microservices and Cluster Autoscaler for EKS nodes.', 'costOptimization': 'Scale down during off-peak hours based on utilization metrics.'}

# 4.0.0.0.0 Security Architecture

## 4.1.0.0.0 Authentication Controls

### 4.1.1.0.0 Method

#### 4.1.1.1.0 Method

sso

#### 4.1.1.2.0 Scope

User access to the Central Management Plane UI.

#### 4.1.1.3.0 Implementation

Keycloak as a centralized IdP with OIDC and OAuth 2.0 (REQ-NFR-003).

#### 4.1.1.4.0 Environment

All

### 4.1.2.0.0 Method

#### 4.1.2.1.0 Method

certificates

#### 4.1.2.2.0 Scope

Service-to-service communication and edge-to-cloud data streaming.

#### 4.1.2.3.0 Implementation

Mutual TLS (mTLS) for all gRPC traffic (REQ-ARC-001).

#### 4.1.2.4.0 Environment

Production, Staging

### 4.1.3.0.0 Method

#### 4.1.3.1.0 Method

api-keys

#### 4.1.3.2.0 Scope

Secure bootstrapping of new OPC Core Client instances.

#### 4.1.3.3.0 Implementation

One-time registration tokens generated by the Central Management Plane (REQ-NFR-003).

#### 4.1.3.4.0 Environment

All

## 4.2.0.0.0 Authorization Controls

- {'model': 'rbac', 'implementation': 'Permissions enforced at the Kong API Gateway and within each microservice based on JWT claims (REQ-NFR-003).', 'granularity': 'fine-grained', 'environment': 'All'}

## 4.3.0.0.0 Certificate Management

| Property | Value |
|----------|-------|
| Authority | hybrid |
| Rotation Policy | Automated rotation every 90 days for public-facing... |
| Automation | ✅ |
| Monitoring | ✅ |

## 4.4.0.0.0 Encryption Standards

### 4.4.1.0.0 Scope

#### 4.4.1.1.0 Scope

data-in-transit

#### 4.4.1.2.0 Algorithm

TLS 1.3

#### 4.4.1.3.0 Key Management

AWS Certificate Manager (ACM)

#### 4.4.1.4.0 Compliance

*No items available*

### 4.4.2.0.0 Scope

#### 4.4.2.1.0 Scope

data-at-rest

#### 4.4.2.2.0 Algorithm

AES-256

#### 4.4.2.3.0 Key Management

AWS Key Management Service (KMS)

#### 4.4.2.4.0 Compliance

- GDPR

## 4.5.0.0.0 Access Control Mechanisms

### 4.5.1.0.0 security-groups

#### 4.5.1.1.0 Type

🔹 security-groups

#### 4.5.1.2.0 Configuration

Principle of least privilege; default deny, with explicit allow rules for required traffic between components.

#### 4.5.1.3.0 Environment

All

#### 4.5.1.4.0 Rules

- Allow traffic from Application Load Balancer to API Gateway on port 443.

### 4.5.2.0.0 iam

#### 4.5.2.1.0 Type

🔹 iam

#### 4.5.2.2.0 Configuration

IAM Roles for Service Accounts (IRSA) used to grant Kubernetes pods fine-grained access to AWS services.

#### 4.5.2.3.0 Environment

All

#### 4.5.2.4.0 Rules

*No items available*

## 4.6.0.0.0 Data Protection Measures

- {'dataType': 'pii', 'protectionMethod': 'encryption', 'implementation': 'Transparent Data Encryption (TDE) on RDS, Server-Side Encryption (SSE-KMS) on S3, and application-level encryption for sensitive user fields.', 'compliance': ['GDPR']}

## 4.7.0.0.0 Network Security

### 4.7.1.0.0 Control

#### 4.7.1.1.0 Control

waf

#### 4.7.1.2.0 Implementation

AWS WAF integrated with the Application Load Balancer in front of the API Gateway to protect against common web exploits.

#### 4.7.1.3.0 Rules

- AWS Managed Rules for SQL Injection and Cross-Site Scripting.

#### 4.7.1.4.0 Monitoring

✅ Yes

### 4.7.2.0.0 Control

#### 4.7.2.1.0 Control

ids

#### 4.7.2.2.0 Implementation

AWS GuardDuty enabled in all accounts to monitor for malicious activity.

#### 4.7.2.3.0 Rules

*No items available*

#### 4.7.2.4.0 Monitoring

✅ Yes

## 4.8.0.0.0 Security Monitoring

### 4.8.1.0.0 siem

#### 4.8.1.1.0 Type

🔹 siem

#### 4.8.1.2.0 Implementation

Forwarding of AWS CloudTrail, VPC Flow Logs, and application logs from OpenSearch to a centralized SIEM for threat analysis.

#### 4.8.1.3.0 Frequency

real-time

#### 4.8.1.4.0 Alerting

✅ Yes

### 4.8.2.0.0 pen-testing

#### 4.8.2.1.0 Type

🔹 pen-testing

#### 4.8.2.2.0 Implementation

Annual third-party penetration testing of the Central Management Plane.

#### 4.8.2.3.0 Frequency

annual

#### 4.8.2.4.0 Alerting

❌ No

## 4.9.0.0.0 Backup Security

| Property | Value |
|----------|-------|
| Encryption | ✅ |
| Access Control | Restricted IAM policies for backup access. |
| Offline Storage | ❌ |
| Testing Frequency | quarterly |

## 4.10.0.0.0 Compliance Frameworks

### 4.10.1.0.0 Framework

#### 4.10.1.1.0 Framework

gdpr

#### 4.10.1.2.0 Applicable Environments

- Production
- Staging
- DR

#### 4.10.1.3.0 Controls

- Data encryption at rest and in transit
- PII masking in non-prod
- Data residency enforcement

#### 4.10.1.4.0 Audit Frequency

annual

### 4.10.2.0.0 Framework

#### 4.10.2.1.0 Framework

fda 21 cfr part 11

#### 4.10.2.2.0 Applicable Environments

- Production

#### 4.10.2.3.0 Controls

- Tamper-evident audit trails (REQ-FR-005)
- Electronic signatures
- Strict access controls

#### 4.10.2.4.0 Audit Frequency

annual

# 5.0.0.0.0 Network Design

## 5.1.0.0.0 Network Segmentation

### 5.1.1.0.0 Environment

#### 5.1.1.1.0 Environment

Production

#### 5.1.1.2.0 Segment Type

private

#### 5.1.1.3.0 Purpose

EKS worker nodes, databases, and cache.

#### 5.1.1.4.0 Isolation

logical

### 5.1.2.0.0 Environment

#### 5.1.2.1.0 Environment

Production

#### 5.1.2.2.0 Segment Type

public

#### 5.1.2.3.0 Purpose

Application Load Balancers and NAT Gateways.

#### 5.1.2.4.0 Isolation

logical

## 5.2.0.0.0 Subnet Strategy

### 5.2.1.0.0 Environment

#### 5.2.1.1.0 Environment

Production

#### 5.2.1.2.0 Subnet Type

private

#### 5.2.1.3.0 Cidr Block

10.0.1.0/24

#### 5.2.1.4.0 Availability Zone

us-east-1a

#### 5.2.1.5.0 Routing Table

Route outbound traffic via NAT Gateway.

### 5.2.2.0.0 Environment

#### 5.2.2.1.0 Environment

Production

#### 5.2.2.2.0 Subnet Type

public

#### 5.2.2.3.0 Cidr Block

10.0.101.0/24

#### 5.2.2.4.0 Availability Zone

us-east-1a

#### 5.2.2.5.0 Routing Table

Route traffic via Internet Gateway.

## 5.3.0.0.0 Security Group Rules

### 5.3.1.0.0 Group Name

#### 5.3.1.1.0 Group Name

sg-database

#### 5.3.1.2.0 Direction

inbound

#### 5.3.1.3.0 Protocol

tcp

#### 5.3.1.4.0 Port Range

5432

#### 5.3.1.5.0 Source

sg-application-tier

#### 5.3.1.6.0 Purpose

Allow application services to connect to the PostgreSQL database.

### 5.3.2.0.0 Group Name

#### 5.3.2.1.0 Group Name

sg-alb

#### 5.3.2.2.0 Direction

inbound

#### 5.3.2.3.0 Protocol

tcp

#### 5.3.2.4.0 Port Range

443

#### 5.3.2.5.0 Source

0.0.0.0/0

#### 5.3.2.6.0 Purpose

Allow public HTTPS traffic to the load balancer.

## 5.4.0.0.0 Connectivity Requirements

- {'source': 'OPC Core Client', 'destination': 'Central Management Plane', 'protocol': 'gRPC, MQTT', 'bandwidth': 'High', 'latency': 'Low'}

## 5.5.0.0.0 Network Monitoring

- {'type': 'flow-logs', 'implementation': 'VPC Flow Logs enabled and shipped to OpenSearch/SIEM for analysis.', 'alerting': True, 'retention': '90 days'}

## 5.6.0.0.0 Bandwidth Controls

- {'scope': 'N/A', 'limits': 'N/A', 'prioritization': 'N/A', 'enforcement': 'N/A'}

## 5.7.0.0.0 Service Discovery

| Property | Value |
|----------|-------|
| Method | dns |
| Implementation | Kubernetes native service discovery (CoreDNS). |
| Health Checks | ✅ |

## 5.8.0.0.0 Environment Communication

- {'sourceEnvironment': 'Production', 'targetEnvironment': 'DR', 'communicationType': 'replication', 'securityControls': ['VPC Peering', 'Encrypted traffic']}

# 6.0.0.0.0 Data Management Strategy

## 6.1.0.0.0 Data Isolation

- {'environment': 'Production', 'isolationLevel': 'complete', 'method': 'separate-instances', 'justification': 'Complete separation of production data from all non-production environments is required for security and compliance.'}

## 6.2.0.0.0 Backup And Recovery

- {'environment': 'Production', 'backupFrequency': 'Daily automated snapshots, continuous Point-in-Time Recovery (PITR) enabled.', 'retentionPeriod': '30 days (REQ-NFR-002).', 'recoveryTimeObjective': '< 15 minutes for AZ failure.', 'recoveryPointObjective': '< 5 minutes (PITR).', 'testingSchedule': 'quarterly'}

## 6.3.0.0.0 Data Masking Anonymization

- {'environment': 'Staging', 'dataType': 'PII', 'maskingMethod': 'static', 'coverage': 'complete', 'compliance': ['GDPR']}

## 6.4.0.0.0 Migration Processes

- {'sourceEnvironment': 'Testing', 'targetEnvironment': 'Staging', 'migrationMethod': 'dump-restore', 'validation': 'Schema validation and record counts.', 'rollbackPlan': 'Restore from pre-migration snapshot.'}

## 6.5.0.0.0 Retention Policies

### 6.5.1.0.0 Environment

#### 6.5.1.1.0 Environment

Production

#### 6.5.1.2.0 Data Type

Audit Logs

#### 6.5.1.3.0 Retention Period

7 years

#### 6.5.1.4.0 Archival Method

Automated via TimescaleDB lifecycle policies.

#### 6.5.1.5.0 Compliance Requirement

REQ-NFR-007

### 6.5.2.0.0 Environment

#### 6.5.2.1.0 Environment

Production

#### 6.5.2.2.0 Data Type

Raw Time-Series Data

#### 6.5.2.3.0 Retention Period

1 year

#### 6.5.2.4.0 Archival Method

Automated via TimescaleDB lifecycle policies.

#### 6.5.2.5.0 Compliance Requirement

REQ-NFR-007

## 6.6.0.0.0 Data Classification

- {'classification': 'restricted', 'handlingRequirements': ['Encrypt at rest and in transit', 'Strict access controls'], 'accessControls': ['RBAC with least privilege'], 'environments': ['Production']}

## 6.7.0.0.0 Disaster Recovery

- {'environment': 'Production', 'drSite': 'Different AWS Region (e.g., eu-west-1 if primary is us-east-1).', 'replicationMethod': 'asynchronous', 'failoverTime': '< 4 hours (RTO)', 'testingFrequency': 'quarterly'}

# 7.0.0.0.0 Monitoring And Observability

## 7.1.0.0.0 Monitoring Components

### 7.1.1.0.0 Component

#### 7.1.1.1.0 Component

infrastructure

#### 7.1.1.2.0 Tool

Prometheus

#### 7.1.1.3.0 Implementation

Prometheus Operator on Kubernetes to scrape metrics from nodes and pods.

#### 7.1.1.4.0 Environments

- All

### 7.1.2.0.0 Component

#### 7.1.2.1.0 Component

logs

#### 7.1.2.2.0 Tool

Fluentd & OpenSearch

#### 7.1.2.3.0 Implementation

Fluentd DaemonSet collects logs and forwards to a central OpenSearch cluster.

#### 7.1.2.4.0 Environments

- All

### 7.1.3.0.0 Component

#### 7.1.3.1.0 Component

tracing

#### 7.1.3.2.0 Tool

OpenTelemetry

#### 7.1.3.3.0 Implementation

.NET auto-instrumentation libraries to generate and propagate traces.

#### 7.1.3.4.0 Environments

- All

### 7.1.4.0.0 Component

#### 7.1.4.1.0 Component

alerting

#### 7.1.4.2.0 Tool

Alertmanager

#### 7.1.4.3.0 Implementation

Configured with rules to send notifications to PagerDuty and Slack.

#### 7.1.4.4.0 Environments

- Production
- Staging

## 7.2.0.0.0 Environment Specific Thresholds

### 7.2.1.0.0 Environment

#### 7.2.1.1.0 Environment

Production

#### 7.2.1.2.0 Metric

API P95 Latency

#### 7.2.1.3.0 Warning Threshold

> 150ms for 5m

#### 7.2.1.4.0 Critical Threshold

> 200ms for 5m

#### 7.2.1.5.0 Justification

Aligned with SLA defined in REQ-NFR-001.

### 7.2.2.0.0 Environment

#### 7.2.2.1.0 Environment

Staging

#### 7.2.2.2.0 Metric

API P95 Latency

#### 7.2.2.3.0 Warning Threshold

> 150ms for 5m

#### 7.2.2.4.0 Critical Threshold

> 200ms for 5m

#### 7.2.2.5.0 Justification

Must match production to validate performance.

### 7.2.3.0.0 Environment

#### 7.2.3.1.0 Environment

Development

#### 7.2.3.2.0 Metric

API P95 Latency

#### 7.2.3.3.0 Warning Threshold

> 500ms for 10m

#### 7.2.3.4.0 Critical Threshold

> 1000ms for 10m

#### 7.2.3.5.0 Justification

Looser thresholds to avoid noise during development and debugging.

## 7.3.0.0.0 Metrics Collection

- {'category': 'application', 'metrics': ['http_request_duration_seconds', 'grpc_server_handling_seconds', 'opc_values_ingested_total'], 'collectionInterval': '30s', 'retention': '30 days raw, 1 year aggregated.'}

## 7.4.0.0.0 Health Check Endpoints

### 7.4.1.0.0 Component

#### 7.4.1.1.0 Component

All microservices

#### 7.4.1.2.0 Endpoint

/healthz

#### 7.4.1.3.0 Check Type

liveness

#### 7.4.1.4.0 Timeout

5s

#### 7.4.1.5.0 Frequency

30s

### 7.4.2.0.0 Component

#### 7.4.2.1.0 Component

All microservices

#### 7.4.2.2.0 Endpoint

/readyz

#### 7.4.2.3.0 Check Type

readiness

#### 7.4.2.4.0 Timeout

5s

#### 7.4.2.5.0 Frequency

15s

## 7.5.0.0.0 Logging Configuration

### 7.5.1.0.0 Environment

#### 7.5.1.1.0 Environment

Production

#### 7.5.1.2.0 Log Level

info

#### 7.5.1.3.0 Destinations

- OpenSearch

#### 7.5.1.4.0 Retention

As per data type (e.g., Audit logs 7 years).

#### 7.5.1.5.0 Sampling

1% sampling on high-volume success logs from Ingestion Service.

### 7.5.2.0.0 Environment

#### 7.5.2.1.0 Environment

Development

#### 7.5.2.2.0 Log Level

debug

#### 7.5.2.3.0 Destinations

- OpenSearch

#### 7.5.2.4.0 Retention

14 days

#### 7.5.2.5.0 Sampling

100%

## 7.6.0.0.0 Escalation Policies

### 7.6.1.0.0 Environment

#### 7.6.1.1.0 Environment

Production

#### 7.6.1.2.0 Severity

Critical

#### 7.6.1.3.0 Escalation Path

- Primary On-Call Engineer
- Secondary On-Call Engineer
- Engineering Manager

#### 7.6.1.4.0 Timeouts

- 15m
- 15m

#### 7.6.1.5.0 Channels

- PagerDuty
- Slack

### 7.6.2.0.0 Environment

#### 7.6.2.1.0 Environment

Staging

#### 7.6.2.2.0 Severity

Critical

#### 7.6.2.3.0 Escalation Path

- On-Duty QA Engineer
- Development Team Lead

#### 7.6.2.4.0 Timeouts

- 30m

#### 7.6.2.5.0 Channels

- Slack

## 7.7.0.0.0 Dashboard Configurations

- {'dashboardType': 'operational', 'audience': 'SRE/Ops Team', 'refreshInterval': '1m', 'metrics': ['System-wide error rates', 'API Latency (P95, P99)', 'Data Ingestion Rate', 'Active OPC Clients']}

# 8.0.0.0.0 Project Specific Environments

## 8.1.0.0.0 Environments

### 8.1.1.0.0 Production

#### 8.1.1.1.0 Id

prod-us-east-1

#### 8.1.1.2.0 Name

Production (US East 1)

#### 8.1.1.3.0 Type

🔹 Production

#### 8.1.1.4.0 Provider

aws

#### 8.1.1.5.0 Region

us-east-1

#### 8.1.1.6.0 Configuration

| Property | Value |
|----------|-------|
| Instance Type | m6i.xlarge |
| Auto Scaling | enabled |
| Backup Enabled | ✅ |
| Monitoring Level | enhanced |

#### 8.1.1.7.0 Security Groups

- sg-prod-alb
- sg-prod-app
- sg-prod-db

#### 8.1.1.8.0 Network

##### 8.1.1.8.1 Vpc Id

vpc-prod-use1

##### 8.1.1.8.2 Subnets

- subnet-prod-private-a
- subnet-prod-private-b
- subnet-prod-public-a
- subnet-prod-public-b

##### 8.1.1.8.3 Security Groups

*No items available*

##### 8.1.1.8.4 Internet Gateway

igw-prod

##### 8.1.1.8.5 Nat Gateway

nat-prod

#### 8.1.1.9.0 Monitoring

##### 8.1.1.9.1 Enabled

✅ Yes

##### 8.1.1.9.2 Metrics

*No items available*

##### 8.1.1.9.3 Alerts

*No data available*

##### 8.1.1.9.4 Dashboards

*No items available*

#### 8.1.1.10.0 Compliance

##### 8.1.1.10.1 Frameworks

- GDPR
- FDA 21 CFR Part 11

##### 8.1.1.10.2 Controls

*No items available*

##### 8.1.1.10.3 Audit Schedule

annual

#### 8.1.1.11.0 Data Management

| Property | Value |
|----------|-------|
| Backup Schedule | daily |
| Retention Policy | 30 days |
| Encryption Enabled | ✅ |
| Data Masking | ❌ |

### 8.1.2.0.0 DR

#### 8.1.2.1.0 Id

dr-eu-west-1

#### 8.1.2.2.0 Name

Disaster Recovery (EU West 1)

#### 8.1.2.3.0 Type

🔹 DR

#### 8.1.2.4.0 Provider

aws

#### 8.1.2.5.0 Region

eu-west-1

#### 8.1.2.6.0 Configuration

| Property | Value |
|----------|-------|
| Instance Type | m6i.xlarge |
| Auto Scaling | disabled |
| Backup Enabled | ✅ |
| Monitoring Level | standard |

#### 8.1.2.7.0 Security Groups

- sg-dr-alb
- sg-dr-app
- sg-dr-db

#### 8.1.2.8.0 Network

##### 8.1.2.8.1 Vpc Id

vpc-dr-euw1

##### 8.1.2.8.2 Subnets

- subnet-dr-private-a
- subnet-dr-private-b
- subnet-dr-public-a
- subnet-dr-public-b

##### 8.1.2.8.3 Security Groups

*No items available*

##### 8.1.2.8.4 Internet Gateway

igw-dr

##### 8.1.2.8.5 Nat Gateway

nat-dr

#### 8.1.2.9.0 Monitoring

##### 8.1.2.9.1 Enabled

✅ Yes

##### 8.1.2.9.2 Metrics

*No items available*

##### 8.1.2.9.3 Alerts

*No data available*

##### 8.1.2.9.4 Dashboards

*No items available*

#### 8.1.2.10.0 Compliance

##### 8.1.2.10.1 Frameworks

- GDPR

##### 8.1.2.10.2 Controls

*No items available*

##### 8.1.2.10.3 Audit Schedule

annual

#### 8.1.2.11.0 Data Management

| Property | Value |
|----------|-------|
| Backup Schedule | N/A (replication) |
| Retention Policy | N/A (replication) |
| Encryption Enabled | ✅ |
| Data Masking | ❌ |

## 8.2.0.0.0 Configuration

| Property | Value |
|----------|-------|
| Global Timeout | 30s |
| Max Instances | 20 |
| Backup Schedule | 04:00 UTC |
| Deployment Strategy | blue-green |
| Rollback Strategy | automated |
| Maintenance Window | Sunday 02:00-06:00 UTC |

## 8.3.0.0.0 Cross Environment Policies

- {'policy': 'data-flow', 'implementation': 'Production data can only flow to DR (replication) or Staging (after anonymization). No direct flow to Dev/Test.', 'enforcement': 'automated'}

# 9.0.0.0.0 Implementation Priority

## 9.1.0.0.0 Component

### 9.1.1.0.0 Component

Infrastructure as Code (Terraform) for VPCs and IAM

### 9.1.2.0.0 Priority

🔴 high

### 9.1.3.0.0 Dependencies

*No items available*

### 9.1.4.0.0 Estimated Effort

High

### 9.1.5.0.0 Risk Level

medium

## 9.2.0.0.0 Component

### 9.2.1.0.0 Component

Production EKS Cluster & Core Services Deployment

### 9.2.2.0.0 Priority

🔴 high

### 9.2.3.0.0 Dependencies

- Infrastructure as Code (Terraform) for VPCs and IAM

### 9.2.4.0.0 Estimated Effort

High

### 9.2.5.0.0 Risk Level

high

## 9.3.0.0.0 Component

### 9.3.1.0.0 Component

Observability Stack (Prometheus, Grafana, OpenSearch)

### 9.3.2.0.0 Priority

🔴 high

### 9.3.3.0.0 Dependencies

- Production EKS Cluster & Core Services Deployment

### 9.3.4.0.0 Estimated Effort

Medium

### 9.3.5.0.0 Risk Level

medium

## 9.4.0.0.0 Component

### 9.4.1.0.0 Component

Staging Environment (Prod Replica)

### 9.4.2.0.0 Priority

🟡 medium

### 9.4.3.0.0 Dependencies

- Production EKS Cluster & Core Services Deployment

### 9.4.4.0.0 Estimated Effort

Medium

### 9.4.5.0.0 Risk Level

medium

## 9.5.0.0.0 Component

### 9.5.1.0.0 Component

Disaster Recovery Environment

### 9.5.2.0.0 Priority

🟢 low

### 9.5.3.0.0 Dependencies

- Production EKS Cluster & Core Services Deployment

### 9.5.4.0.0 Estimated Effort

Medium

### 9.5.5.0.0 Risk Level

high

# 10.0.0.0.0 Risk Assessment

## 10.1.0.0.0 Risk

### 10.1.1.0.0 Risk

Cloud Cost Overrun

### 10.1.2.0.0 Impact

high

### 10.1.3.0.0 Probability

high

### 10.1.4.0.0 Mitigation

Implement strict resource tagging, budget alerts via AWS Budgets, use cost-effective instance types for non-prod, and regularly review costs.

### 10.1.5.0.0 Contingency Plan

Downscale non-critical environments (Dev/Test) and optimize resource requests/limits for services.

## 10.2.0.0.0 Risk

### 10.2.1.0.0 Risk

Data Residency Compliance Breach

### 10.2.2.0.0 Impact

high

### 10.2.3.0.0 Probability

medium

### 10.2.4.0.0 Mitigation

Use Terraform to enforce deployment to specific regions based on tenant configuration. Use IAM Service Control Policies (SCPs) to restrict creation of resources outside allowed regions.

### 10.2.5.0.0 Contingency Plan

Documented incident response plan for data breaches, including legal and customer notification procedures.

## 10.3.0.0.0 Risk

### 10.3.1.0.0 Risk

Failure to meet RTO/RPO for Disaster Recovery

### 10.3.2.0.0 Impact

high

### 10.3.3.0.0 Probability

medium

### 10.3.4.0.0 Mitigation

Automate DR infrastructure provisioning with Terraform. Conduct mandatory quarterly DR tests to validate the process and timing.

### 10.3.5.0.0 Contingency Plan

Have a documented manual failover procedure as a backup to the automated process.

# 11.0.0.0.0 Recommendations

## 11.1.0.0.0 Category

### 11.1.1.0.0 Category

🔹 Security

### 11.1.2.0.0 Recommendation

Adopt a multi-account AWS strategy to provide the strongest possible isolation between Production, Staging, and Non-Prod environments.

### 11.1.3.0.0 Justification

This significantly reduces the blast radius of a security incident or misconfiguration, protecting sensitive production data and ensuring staging environment integrity.

### 11.1.4.0.0 Priority

🔴 high

### 11.1.5.0.0 Implementation Notes

Use AWS Organizations to centrally manage policies (SCPs) and billing across accounts.

## 11.2.0.0.0 Category

### 11.2.1.0.0 Category

🔹 Cost Optimization

### 11.2.2.0.0 Recommendation

Implement automated shutdown/startup schedules for Development and Testing environments during non-business hours.

### 11.2.3.0.0 Justification

These environments are not required 24/7, and shutting them down can lead to significant (~65%) cost savings on compute resources.

### 11.2.4.0.0 Priority

🟡 medium

### 11.2.5.0.0 Implementation Notes

Use a scheduled Lambda function or a tool like AWS Instance Scheduler to manage the lifecycle of EKS node groups and other resources.

## 11.3.0.0.0 Category

### 11.3.1.0.0 Category

🔹 Operations

### 11.3.2.0.0 Recommendation

Define and implement a 'Well-Architected Review' process to be conducted annually against the production environment.

### 11.3.3.0.0 Justification

Ensures the architecture continuously aligns with AWS best practices for security, reliability, performance efficiency, cost optimization, and operational excellence.

### 11.3.4.0.0 Priority

🟡 medium

### 11.3.5.0.0 Implementation Notes

Use the AWS Well-Architected Tool to guide the review process and track remediation items.

