# 1 Overview

## 1.1 Diagram Id

SEQ-RF-003

## 1.2 Name

Full Disaster Recovery (Region Failure)

## 1.3 Description

A catastrophic failure makes an entire AWS region unavailable. The SRE team initiates the formal Disaster Recovery (DR) plan. This involves promoting cross-region database replicas, updating DNS to point to the DR region's load balancer, and using Infrastructure as Code (Terraform) to deploy the application infrastructure in the new region.

## 1.4 Type

🔹 RecoveryFlow

## 1.5 Purpose

To recover the entire service in a different geographic region in the event of a large-scale disaster, meeting the defined Recovery Time Objective (RTO) and Recovery Point Objective (RPO) from REQ-NFR-002.

## 1.6 Complexity

Critical

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

Rare

## 1.9 Participants

- SRE Team
- AWS Route 53
- AWS RDS (Cross-Region Replica)
- Amazon EKS (in DR region)
- Terraform

## 1.10 Key Interactions

- Monitoring detects a full region outage, triggering the DR plan and alerting the SRE team.
- The SRE team promotes the cross-region read replica of the database to a standalone, writable master.
- Terraform scripts are executed to provision the EKS cluster, networking, and other required infrastructure in the DR region.
- The CI/CD pipeline deploys the latest stable version of all microservices to the new cluster.
- DNS records (e.g., Route 53) are updated to point the main application URL to the load balancer in the DR region.
- The system is validated for core functionality before being declared fully operational.

## 1.11 Triggers

- A confirmed, widespread AWS region failure impacting all Availability Zones.

## 1.12 Outcomes

- The service is restored and available to customers from the DR region.
- The recovery meets the RTO of < 4 hours and RPO of < 1 hour (REQ-NFR-002).

## 1.13 Business Rules

- A formal DR plan must be defined and tested on a quarterly basis (REQ-NFR-002).
- Cross-region database and object storage replication must be enabled and monitored for lag.

## 1.14 Error Scenarios

- Cross-region replicas are significantly out of sync or corrupted, leading to data loss beyond the RPO.
- Terraform scripts fail to execute correctly in the DR region due to API changes or capacity issues.
- DNS propagation is slow, leading to extended downtime for a subset of users.

## 1.15 Integration Points

- AWS Infrastructure across multiple geographic regions

# 2.0 Details

## 2.1 Diagram Id

SEQ-IMPL-RF-003

## 2.2 Name

Implementation Sequence: Full Disaster Recovery (Region Failure)

## 2.3 Description

A comprehensive technical sequence for recovering the entire Central Management Plane service in a different geographic region following a catastrophic failure of the primary AWS region. This sequence is manually initiated by the SRE team based on a formal Disaster Recovery (DR) plan and leverages Infrastructure as Code (Terraform) and CI/CD automation to meet the RTO/RPO targets defined in REQ-NFR-002.

## 2.4 Participants

### 2.4.1 ObservabilityPlatform

#### 2.4.1.1 Repository Id

monitoring_system

#### 2.4.1.2 Display Name

Monitoring System

#### 2.4.1.3 Type

🔹 ObservabilityPlatform

#### 2.4.1.4 Technology

Prometheus & Alertmanager

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #FFC300 |
| Stereotype | Monitoring |

### 2.4.2.0 HumanActor

#### 2.4.2.1 Repository Id

sre_team

#### 2.4.2.2 Display Name

SRE Team

#### 2.4.2.3 Type

🔹 HumanActor

#### 2.4.2.4 Technology

AWS CLI, Terraform CLI, kubectl

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #DAF7A6 |
| Stereotype | Operator |

### 2.4.3.0 Database

#### 2.4.3.1 Repository Id

aws_rds_dr_replica

#### 2.4.3.2 Display Name

AWS RDS (Cross-Region Replica)

#### 2.4.3.3 Type

🔹 Database

#### 2.4.3.4 Technology

PostgreSQL 16 / TimescaleDB on AWS RDS

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #FF5733 |
| Stereotype | DR Data Store |

### 2.4.4.0 InfrastructureAsCode

#### 2.4.4.1 Repository Id

terraform_iac

#### 2.4.4.2 Display Name

Terraform (IaC)

#### 2.4.4.3 Type

🔹 InfrastructureAsCode

#### 2.4.4.4 Technology

Terraform v1.8.x with AWS Provider

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #9A2EFE |
| Stereotype | IaC |

### 2.4.5.0 Orchestrator

#### 2.4.5.1 Repository Id

aws_eks_dr_cluster

#### 2.4.5.2 Display Name

Amazon EKS (DR Region)

#### 2.4.5.3 Type

🔹 Orchestrator

#### 2.4.5.4 Technology

Amazon EKS v1.29

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #2E86C1 |
| Stereotype | DR Compute |

### 2.4.6.0 DevOpsPipeline

#### 2.4.6.1 Repository Id

ci_cd_pipeline

#### 2.4.6.2 Display Name

CI/CD Pipeline

#### 2.4.6.3 Type

🔹 DevOpsPipeline

#### 2.4.6.4 Technology

GitHub Actions

#### 2.4.6.5 Order

6

#### 2.4.6.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #581845 |
| Stereotype | CI/CD |

### 2.4.7.0 DNSProvider

#### 2.4.7.1 Repository Id

aws_route53

#### 2.4.7.2 Display Name

AWS Route 53

#### 2.4.7.3 Type

🔹 DNSProvider

#### 2.4.7.4 Technology

AWS Route 53

#### 2.4.7.5 Order

7

#### 2.4.7.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #F4D03F |
| Stereotype | DNS |

## 2.5.0.0 Interactions

### 2.5.1.0 Alert

#### 2.5.1.1 Source Id

monitoring_system

#### 2.5.1.2 Target Id

sre_team

#### 2.5.1.3 Message

Triggers CRITICAL 'Region Unreachable' alert via PagerDuty

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 Alert

#### 2.5.1.6 Is Synchronous

❌ No

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

❌ No

#### 2.5.1.9 Is Activation

❌ No

#### 2.5.1.10 Technical Details

##### 2.5.1.10.1 Protocol

PagerDuty API

##### 2.5.1.10.2 Method

POST /v2/enqueue

##### 2.5.1.10.3 Parameters

- {'name': 'payload', 'type': 'JSON', 'description': "Alert payload with summary 'Primary AWS Region is offline' and severity 'critical'."}

##### 2.5.1.10.4 Authentication

API Integration Key

##### 2.5.1.10.5 Error Handling

Alertmanager will retry delivery. SRE team has secondary notification channels.

##### 2.5.1.10.6 Performance

###### 2.5.1.10.6.1 Latency

< 1 minute for notification delivery

### 2.5.2.0.0.0 DatabasePromotion

#### 2.5.2.1.0.0 Source Id

sre_team

#### 2.5.2.2.0.0 Target Id

aws_rds_dr_replica

#### 2.5.2.3.0.0 Message

[Manual] Executes command to promote Read Replica to standalone master instance

#### 2.5.2.4.0.0 Sequence Number

2

#### 2.5.2.5.0.0 Type

🔹 DatabasePromotion

#### 2.5.2.6.0.0 Is Synchronous

✅ Yes

#### 2.5.2.7.0.0 Return Message

DB instance status: 'available' (no longer a replica)

#### 2.5.2.8.0.0 Has Return

✅ Yes

#### 2.5.2.9.0.0 Is Activation

✅ Yes

#### 2.5.2.10.0.0 Technical Details

##### 2.5.2.10.1.0 Protocol

AWS CLI / AWS API

##### 2.5.2.10.2.0 Method

aws rds promote-read-replica

##### 2.5.2.10.3.0 Parameters

- {'name': '--db-instance-identifier', 'type': 'String', 'description': 'Identifier of the cross-region replica.'}

##### 2.5.2.10.4.0 Authentication

Break-glass SRE IAM role credentials with MFA

##### 2.5.2.10.5.0 Error Handling

If promotion fails, check replica health and lag. If corrupt, initiate restore from latest snapshot (increases RTO/RPO).

##### 2.5.2.10.6.0 Performance

###### 2.5.2.10.6.1 Latency

5-15 minutes, a significant portion of RTO.

### 2.5.3.0.0.0 InfrastructureProvisioning

#### 2.5.3.1.0.0 Source Id

sre_team

#### 2.5.3.2.0.0 Target Id

terraform_iac

#### 2.5.3.3.0.0 Message

[Manual] Executes 'terraform apply' using DR environment configuration

#### 2.5.3.4.0.0 Sequence Number

3

#### 2.5.3.5.0.0 Type

🔹 InfrastructureProvisioning

#### 2.5.3.6.0.0 Is Synchronous

✅ Yes

#### 2.5.3.7.0.0 Return Message

Terraform apply complete. Outputs: EKS cluster endpoint, VPC ID, etc.

#### 2.5.3.8.0.0 Has Return

✅ Yes

#### 2.5.3.9.0.0 Is Activation

✅ Yes

#### 2.5.3.10.0.0 Technical Details

##### 2.5.3.10.1.0 Protocol

Terraform CLI

##### 2.5.3.10.2.0 Method

terraform apply -var-file=dr-region.tfvars

##### 2.5.3.10.3.0 Parameters

*No items available*

##### 2.5.3.10.4.0 Authentication

SRE IAM role credentials configured in AWS provider

##### 2.5.3.10.5.0 Error Handling

Apply may fail due to API limits or transient errors. SRE must analyze plan and re-run. State file must be available.

##### 2.5.3.10.6.0 Performance

###### 2.5.3.10.6.1 Latency

30-60 minutes, typically the longest step impacting RTO.

#### 2.5.3.11.0.0 Nested Interactions

- {'sourceId': 'terraform_iac', 'targetId': 'aws_eks_dr_cluster', 'message': 'Makes AWS API calls to create VPC, Subnets, EKS Cluster, Node Groups, and Load Balancers', 'sequenceNumber': 3.1, 'type': 'APICall', 'isSynchronous': True, 'returnMessage': 'Resource creation success responses', 'hasReturn': True, 'isActivation': True, 'technicalDetails': {'protocol': 'HTTPS (AWS SDK)', 'method': 'Various e.g., CreateCluster, CreateNodegroup', 'parameters': [], 'authentication': 'Inherited from Terraform provider', 'errorHandling': 'Terraform handles transient API errors with retries.', 'performance': {'latency': 'Dependent on AWS provisioning times.'}}}

### 2.5.4.0.0.0 DeploymentTrigger

#### 2.5.4.1.0.0 Source Id

sre_team

#### 2.5.4.2.0.0 Target Id

ci_cd_pipeline

#### 2.5.4.3.0.0 Message

[Manual] Triggers 'Deploy to DR' workflow

#### 2.5.4.4.0.0 Sequence Number

4

#### 2.5.4.5.0.0 Type

🔹 DeploymentTrigger

#### 2.5.4.6.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0 Return Message

Workflow execution started successfully

#### 2.5.4.8.0.0 Has Return

✅ Yes

#### 2.5.4.9.0.0 Is Activation

❌ No

#### 2.5.4.10.0.0 Technical Details

##### 2.5.4.10.1.0 Protocol

GitHub API / UI

##### 2.5.4.10.2.0 Method

POST /repos/{owner}/{repo}/actions/workflows/{workflow_id}/dispatches

##### 2.5.4.10.3.0 Parameters

###### 2.5.4.10.3.1 String

####### 2.5.4.10.3.1.1 Name

ref

####### 2.5.4.10.3.1.2 Type

🔹 String

####### 2.5.4.10.3.1.3 Description

main

###### 2.5.4.10.3.2.0 JSON

####### 2.5.4.10.3.2.1 Name

inputs

####### 2.5.4.10.3.2.2 Type

🔹 JSON

####### 2.5.4.10.3.2.3 Description

{ 'target_cluster': 'dr-eks-cluster-name' }

##### 2.5.4.10.4.0.0 Authentication

SRE GitHub credentials

##### 2.5.4.10.5.0.0 Error Handling

If trigger fails, verify permissions and workflow status.

##### 2.5.4.10.6.0.0 Performance

###### 2.5.4.10.6.1.0 Latency

1-2 minutes for initiation.

### 2.5.5.0.0.0.0 ApplicationDeployment

#### 2.5.5.1.0.0.0 Source Id

ci_cd_pipeline

#### 2.5.5.2.0.0.0 Target Id

aws_eks_dr_cluster

#### 2.5.5.3.0.0.0 Message

Builds, pushes images, and applies Kubernetes manifests for all microservices

#### 2.5.5.4.0.0.0 Sequence Number

5

#### 2.5.5.5.0.0.0 Type

🔹 ApplicationDeployment

#### 2.5.5.6.0.0.0 Is Synchronous

✅ Yes

#### 2.5.5.7.0.0.0 Return Message

All pods are in 'Running' state and readiness probes are passing

#### 2.5.5.8.0.0.0 Has Return

✅ Yes

#### 2.5.5.9.0.0.0 Is Activation

✅ Yes

#### 2.5.5.10.0.0.0 Technical Details

##### 2.5.5.10.1.0.0 Protocol

Kubernetes API

##### 2.5.5.10.2.0.0 Method

kubectl apply -f ...

##### 2.5.5.10.3.0.0 Parameters

- {'name': 'manifests', 'type': 'YAML', 'description': 'Kubernetes Deployment, Service, Ingress, etc. manifests'}

##### 2.5.5.10.4.0.0 Authentication

CI/CD runner's IAM role for EKS

##### 2.5.5.10.5.0.0 Error Handling

Deployment failures (e.g., ImagePullBackOff, CrashLoopBackOff) must be investigated by SRE via `kubectl describe pod`.

##### 2.5.5.10.6.0.0 Performance

###### 2.5.5.10.6.1.0 Latency

15-30 minutes for all services to deploy and become healthy.

### 2.5.6.0.0.0.0 DNSUpdate

#### 2.5.6.1.0.0.0 Source Id

sre_team

#### 2.5.6.2.0.0.0 Target Id

aws_route53

#### 2.5.6.3.0.0.0 Message

[Manual] Updates primary CNAME record to point to DR region's Load Balancer DNS

#### 2.5.6.4.0.0.0 Sequence Number

6

#### 2.5.6.5.0.0.0 Type

🔹 DNSUpdate

#### 2.5.6.6.0.0.0 Is Synchronous

✅ Yes

#### 2.5.6.7.0.0.0 Return Message

Change request status: 'INSYNC'

#### 2.5.6.8.0.0.0 Has Return

✅ Yes

#### 2.5.6.9.0.0.0 Is Activation

✅ Yes

#### 2.5.6.10.0.0.0 Technical Details

##### 2.5.6.10.1.0.0 Protocol

AWS CLI / AWS API

##### 2.5.6.10.2.0.0 Method

aws route53 change-resource-record-sets

##### 2.5.6.10.3.0.0 Parameters

- {'name': '--change-batch', 'type': 'JSON', 'description': "JSON payload with 'UPSERT' action for the primary CNAME record."}

##### 2.5.6.10.4.0.0 Authentication

Break-glass SRE IAM role credentials with MFA

##### 2.5.6.10.5.0.0 Error Handling

Failed updates require manual verification of DNS settings and permissions. Slow propagation is a risk to full recovery time.

##### 2.5.6.10.6.0.0 Performance

###### 2.5.6.10.6.1.0 Latency

1-60 minutes for global DNS propagation.

### 2.5.7.0.0.0.0 HealthCheck

#### 2.5.7.1.0.0.0 Source Id

sre_team

#### 2.5.7.2.0.0.0 Target Id

aws_eks_dr_cluster

#### 2.5.7.3.0.0.0 Message

[Manual] Performs validation smoke tests against public endpoints

#### 2.5.7.4.0.0.0 Sequence Number

7

#### 2.5.7.5.0.0.0 Type

🔹 HealthCheck

#### 2.5.7.6.0.0.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0.0.0 Return Message

HTTP 200 OK, application homepage loads

#### 2.5.7.8.0.0.0 Has Return

✅ Yes

#### 2.5.7.9.0.0.0 Is Activation

❌ No

#### 2.5.7.10.0.0.0 Technical Details

##### 2.5.7.10.1.0.0 Protocol

HTTPS

##### 2.5.7.10.2.0.0 Method

GET /

##### 2.5.7.10.3.0.0 Parameters

*No items available*

##### 2.5.7.10.4.0.0 Authentication

N/A for public endpoint

##### 2.5.7.10.5.0.0 Error Handling

Any non-200 responses indicate a problem with the ingress or backend services.

##### 2.5.7.10.6.0.0 Performance

###### 2.5.7.10.6.1.0 Latency

< 5 seconds for basic checks.

## 2.6.0.0.0.0.0 Notes

### 2.6.1.0.0.0.0 Content

#### 2.6.1.1.0.0.0 Content



```
RTO Target: < 4 Hours
RPO Target: < 1 Hour
This entire process is time-critical and relies on a well-documented and frequently tested runbook.
```

#### 2.6.1.2.0.0.0 Position

top-left

#### 2.6.1.3.0.0.0 Participant Id

*Not specified*

#### 2.6.1.4.0.0.0 Sequence Number

*Not specified*

### 2.6.2.0.0.0.0 Content

#### 2.6.2.1.0.0.0 Content

The Terraform state file must be stored in a globally redundant location (e.g., S3 with Cross-Region Replication enabled) to be accessible during a region failure.

#### 2.6.2.2.0.0.0 Position

bottom-right

#### 2.6.2.3.0.0.0 Participant Id

terraform_iac

#### 2.6.2.4.0.0.0 Sequence Number

3

### 2.6.3.0.0.0.0 Content

#### 2.6.3.1.0.0.0 Content

This is the point of no return. Once the replica is promoted, it cannot be reverted. The primary region database is considered lost.

#### 2.6.3.2.0.0.0 Position

top-right

#### 2.6.3.3.0.0.0 Participant Id

aws_rds_dr_replica

#### 2.6.3.4.0.0.0 Sequence Number

2

## 2.7.0.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | A highly privileged, break-glass IAM role, secured... |
| Performance Targets | The end-to-end Recovery Time Objective (RTO) must ... |
| Error Handling Strategy | The DR plan must have explicit sub-procedures for ... |
| Testing Considerations | The entire DR procedure must be tested quarterly a... |
| Monitoring Requirements | The primary monitoring system triggers the event. ... |
| Deployment Considerations | Container images must be stored in a registry (e.g... |

