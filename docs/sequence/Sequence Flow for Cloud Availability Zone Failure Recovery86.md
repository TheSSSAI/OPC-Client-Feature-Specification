# 1 Overview

## 1.1 Diagram Id

SEQ-RF-002

## 1.2 Name

Cloud Availability Zone Failure Recovery

## 1.3 Description

An AWS Availability Zone (AZ) hosting the primary database and service instances fails. The managed database service (RDS) automatically fails over to the standby replica in a different AZ. Kubernetes detects the failed service pods and reschedules them on nodes in the remaining healthy AZs, restoring service automatically.

## 1.4 Type

🔹 RecoveryFlow

## 1.5 Purpose

To ensure high availability of the Central Management Plane by automatically recovering from the failure of a single data center or availability zone, meeting the RTO specified in REQ-NFR-002.

## 1.6 Complexity

High

## 1.7 Priority

🚨 Critical

## 1.8 Frequency

Rare

## 1.9 Participants

- AWS RDS
- Amazon EKS
- REPO-GW-API
- All Microservices

## 1.10 Key Interactions

- AWS infrastructure detects a failure in one AZ.
- RDS promotes the standby database replica in a healthy AZ to become the new primary, updating its DNS endpoint.
- The Kubernetes control plane detects that nodes and pods in the failed AZ are unhealthy.
- The Kubernetes scheduler creates new pods on nodes in the healthy AZs to replace the failed ones, based on Deployment definitions.
- The new pods start, connect to the new primary database, and report as healthy.
- The API Gateway and other services resume full operation once readiness probes pass.

## 1.11 Triggers

- An entire AWS Availability Zone becomes unavailable due to power, network, or other failure.

## 1.12 Outcomes

- The system automatically recovers and becomes fully operational within the 15-minute Recovery Time Objective (RTO).
- Minimal to no data loss occurs, within the Recovery Point Objective (RPO) of less than 1 hour.

## 1.13 Business Rules

- The RTO shall be less than 15 minutes for an AZ failure (REQ-NFR-002).
- A database read replica (or multi-AZ standby) shall be maintained in a different availability zone (REQ-NFR-002).
- Cloud microservices must be stateless to allow for rescheduling.

## 1.14 Error Scenarios

- The database failover process fails or is slow.
- There are insufficient compute resources in the remaining AZs to reschedule all critical pods.
- Configuration (e.g., DNS) does not update quickly, causing connection errors to the new database.

## 1.15 Integration Points

- AWS Infrastructure (RDS, EKS)

# 2.0 Details

## 2.1 Diagram Id

SEQ-RF-002-IMPL

## 2.2 Name

Automated Recovery from AWS Availability Zone Failure

## 2.3 Description

A technical sequence detailing the automated failover and recovery process of the Central Management Plane following a complete AWS AZ failure. The sequence covers AWS RDS Multi-AZ failover, Kubernetes pod rescheduling by EKS, and service health validation, ensuring the system meets its RTO of <15 minutes as per REQ-NFR-002.

## 2.4 Participants

### 2.4.1 External System

#### 2.4.1.1 Repository Id

EXTERNAL-AWS-INFRA

#### 2.4.1.2 Display Name

External: AWS Infrastructure

#### 2.4.1.3 Type

🔹 External System

#### 2.4.1.4 Technology

AWS Global Infrastructure

#### 2.4.1.5 Order

1

#### 2.4.1.6 Style

| Property | Value |
|----------|-------|
| Shape | actor |
| Color | #999999 |
| Stereotype | «External» |

### 2.4.2.0 Managed Service

#### 2.4.2.1 Repository Id

EXTERNAL-AWS-RDS

#### 2.4.2.2 Display Name

AWS RDS (Multi-AZ)

#### 2.4.2.3 Type

🔹 Managed Service

#### 2.4.2.4 Technology

PostgreSQL/TimescaleDB on AWS RDS

#### 2.4.2.5 Order

2

#### 2.4.2.6 Style

| Property | Value |
|----------|-------|
| Shape | database |
| Color | #FF9900 |
| Stereotype | «Managed Database» |

### 2.4.3.0 Orchestrator

#### 2.4.3.1 Repository Id

EXTERNAL-EKS-CP

#### 2.4.3.2 Display Name

Amazon EKS Control Plane

#### 2.4.3.3 Type

🔹 Orchestrator

#### 2.4.3.4 Technology

Kubernetes 1.29

#### 2.4.3.5 Order

3

#### 2.4.3.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #232F3E |
| Stereotype | «Orchestrator» |

### 2.4.4.0 Microservice

#### 2.4.4.1 Repository Id

REPO-SVC-ALL

#### 2.4.4.2 Display Name

All Microservices

#### 2.4.4.3 Type

🔹 Microservice

#### 2.4.4.4 Technology

.NET 8 on Kubernetes

#### 2.4.4.5 Order

4

#### 2.4.4.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #5A6B86 |
| Stereotype | «Microservice ReplicaSet» |

### 2.4.5.0 ApiGateway

#### 2.4.5.1 Repository Id

REPO-GW-API

#### 2.4.5.2 Display Name

API Gateway

#### 2.4.5.3 Type

🔹 ApiGateway

#### 2.4.5.4 Technology

Kong v3.7.0 on Kubernetes

#### 2.4.5.5 Order

5

#### 2.4.5.6 Style

| Property | Value |
|----------|-------|
| Shape | component |
| Color | #344563 |
| Stereotype | «API Gateway» |

## 2.5.0.0 Interactions

### 2.5.1.0 External Event

#### 2.5.1.1 Source Id

EXTERNAL-AWS-INFRA

#### 2.5.1.2 Target Id

EXTERNAL-AWS-RDS

#### 2.5.1.3 Message

1. Availability Zone Failure Occurs

#### 2.5.1.4 Sequence Number

1

#### 2.5.1.5 Type

🔹 External Event

#### 2.5.1.6 Is Synchronous

❌ No

#### 2.5.1.7 Return Message



#### 2.5.1.8 Has Return

❌ No

#### 2.5.1.9 Is Activation

✅ Yes

#### 2.5.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Infrastructure Event |
| Method | AZ Failure |
| Parameters | Failure of power, networking, or hardware in a sin... |
| Authentication | N/A |
| Error Handling | This is the root failure event. |
| Performance | Detection time is variable, typically < 60s. |

#### 2.5.1.11 Nested Interactions

##### 2.5.1.11.1 Internal Process

###### 2.5.1.11.1.1 Source Id

EXTERNAL-AWS-RDS

###### 2.5.1.11.1.2 Target Id

EXTERNAL-AWS-RDS

###### 2.5.1.11.1.3 Message

1a. [Internal] Detect primary instance failure and initiate automated failover

###### 2.5.1.11.1.4 Sequence Number

2

###### 2.5.1.11.1.5 Type

🔹 Internal Process

###### 2.5.1.11.1.6 Is Synchronous

✅ Yes

###### 2.5.1.11.1.7 Return Message

Failover complete

###### 2.5.1.11.1.8 Has Return

✅ Yes

###### 2.5.1.11.1.9 Is Activation

❌ No

###### 2.5.1.11.1.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | AWS Internal |
| Method | Multi-AZ Failover |
| Parameters | Health check failures, instance unreachable. |
| Authentication | N/A |
| Error Handling | If failover fails, a CRITICAL AWS Health event is ... |
| Performance | Failover typically takes 1-2 minutes. |

##### 2.5.1.11.2.0 State Change

###### 2.5.1.11.2.1 Source Id

EXTERNAL-AWS-RDS

###### 2.5.1.11.2.2 Target Id

EXTERNAL-AWS-RDS

###### 2.5.1.11.2.3 Message

1b. Promote standby replica in healthy AZ to new primary instance

###### 2.5.1.11.2.4 Sequence Number

3

###### 2.5.1.11.2.5 Type

🔹 State Change

###### 2.5.1.11.2.6 Is Synchronous

✅ Yes

###### 2.5.1.11.2.7 Return Message



###### 2.5.1.11.2.8 Has Return

❌ No

###### 2.5.1.11.2.9 Is Activation

❌ No

###### 2.5.1.11.2.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | AWS Internal |
| Method | PromoteReplica |
| Parameters | *N/A* |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Part of the overall failover time. |

##### 2.5.1.11.3.0 DNS Update

###### 2.5.1.11.3.1 Source Id

EXTERNAL-AWS-RDS

###### 2.5.1.11.3.2 Target Id

EXTERNAL-AWS-INFRA

###### 2.5.1.11.3.3 Message

1c. Update DNS CNAME record for database endpoint to point to new primary IP

###### 2.5.1.11.3.4 Sequence Number

4

###### 2.5.1.11.3.5 Type

🔹 DNS Update

###### 2.5.1.11.3.6 Is Synchronous

❌ No

###### 2.5.1.11.3.7 Return Message



###### 2.5.1.11.3.8 Has Return

❌ No

###### 2.5.1.11.3.9 Is Activation

❌ No

###### 2.5.1.11.3.10 Technical Details

| Property | Value |
|----------|-------|
| Protocol | DNS |
| Method | UpdateRecord |
| Parameters | New primary instance IP address. |
| Authentication | IAM Role |
| Error Handling | Propagation delays can cause connection issues. Cl... |
| Performance | DNS update is fast, but propagation depends on cac... |

### 2.5.2.0.0.0 External Event

#### 2.5.2.1.0.0 Source Id

EXTERNAL-AWS-INFRA

#### 2.5.2.2.0.0 Target Id

EXTERNAL-EKS-CP

#### 2.5.2.3.0.0 Message

2. AZ Failure impacts EKS nodes and pods

#### 2.5.2.4.0.0 Sequence Number

5

#### 2.5.2.5.0.0 Type

🔹 External Event

#### 2.5.2.6.0.0 Is Synchronous

❌ No

#### 2.5.2.7.0.0 Return Message



#### 2.5.2.8.0.0 Has Return

❌ No

#### 2.5.2.9.0.0 Is Activation

✅ Yes

#### 2.5.2.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Infrastructure Event |
| Method | Node/Network Failure |
| Parameters | Loss of connectivity to nodes in the failed AZ. |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Detection is based on kubelet heartbeat timeouts. |

### 2.5.3.0.0.0 State Change

#### 2.5.3.1.0.0 Source Id

EXTERNAL-EKS-CP

#### 2.5.3.2.0.0 Target Id

EXTERNAL-EKS-CP

#### 2.5.3.3.0.0 Message

3. [Controller Manager] Marks nodes in failed AZ as 'NotReady'

#### 2.5.3.4.0.0 Sequence Number

6

#### 2.5.3.5.0.0 Type

🔹 State Change

#### 2.5.3.6.0.0 Is Synchronous

✅ Yes

#### 2.5.3.7.0.0 Return Message



#### 2.5.3.8.0.0 Has Return

❌ No

#### 2.5.3.9.0.0 Is Activation

❌ No

#### 2.5.3.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Kubernetes API |
| Method | UpdateNodeStatus |
| Parameters | Condition: Ready=False |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Happens after node-monitor-grace-period (default 4... |

### 2.5.4.0.0.0 Scheduling

#### 2.5.4.1.0.0 Source Id

EXTERNAL-EKS-CP

#### 2.5.4.2.0.0 Target Id

EXTERNAL-EKS-CP

#### 2.5.4.3.0.0 Message

4. [Scheduler] Identifies unsatisfied replicas for Deployments and schedules new pods on healthy nodes

#### 2.5.4.4.0.0 Sequence Number

7

#### 2.5.4.5.0.0 Type

🔹 Scheduling

#### 2.5.4.6.0.0 Is Synchronous

✅ Yes

#### 2.5.4.7.0.0 Return Message

Pods scheduled

#### 2.5.4.8.0.0 Has Return

✅ Yes

#### 2.5.4.9.0.0 Is Activation

❌ No

#### 2.5.4.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Kubernetes API |
| Method | CreatePod |
| Parameters | PodSpec, Node affinity/anti-affinity rules. |
| Authentication | N/A |
| Error Handling | If insufficient capacity exists in remaining AZs, ... |
| Performance | Scheduling is typically very fast (<1s per pod). |

### 2.5.5.0.0.0 Lifecycle Management

#### 2.5.5.1.0.0 Source Id

EXTERNAL-EKS-CP

#### 2.5.5.2.0.0 Target Id

REPO-SVC-ALL

#### 2.5.5.3.0.0 Message

5. [Kubelet] Starts new service pod container

#### 2.5.5.4.0.0 Sequence Number

8

#### 2.5.5.5.0.0 Type

🔹 Lifecycle Management

#### 2.5.5.6.0.0 Is Synchronous

❌ No

#### 2.5.5.7.0.0 Return Message



#### 2.5.5.8.0.0 Has Return

❌ No

#### 2.5.5.9.0.0 Is Activation

✅ Yes

#### 2.5.5.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | CRI |
| Method | RunPodSandbox |
| Parameters | Container image, environment variables from Config... |
| Authentication | N/A |
| Error Handling | ImagePullBackOff, CrashLoopBackOff are common erro... |
| Performance | Container start time depends on image size and app... |

### 2.5.6.0.0.0 DNS Query

#### 2.5.6.1.0.0 Source Id

REPO-SVC-ALL

#### 2.5.6.2.0.0 Target Id

EXTERNAL-AWS-INFRA

#### 2.5.6.3.0.0 Message

6. Resolve updated database DNS endpoint

#### 2.5.6.4.0.0 Sequence Number

9

#### 2.5.6.5.0.0 Type

🔹 DNS Query

#### 2.5.6.6.0.0 Is Synchronous

✅ Yes

#### 2.5.6.7.0.0 Return Message

New Primary DB IP Address

#### 2.5.6.8.0.0 Has Return

✅ Yes

#### 2.5.6.9.0.0 Is Activation

❌ No

#### 2.5.6.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | DNS |
| Method | A/CNAME Record Lookup |
| Parameters | Database endpoint hostname. |
| Authentication | N/A |
| Error Handling | DNS caching within the pod could lead to connectio... |
| Performance | Typically < 50ms. |

### 2.5.7.0.0.0 Database Connection

#### 2.5.7.1.0.0 Source Id

REPO-SVC-ALL

#### 2.5.7.2.0.0 Target Id

EXTERNAL-AWS-RDS

#### 2.5.7.3.0.0 Message

7. Establish database connection

#### 2.5.7.4.0.0 Sequence Number

10

#### 2.5.7.5.0.0 Type

🔹 Database Connection

#### 2.5.7.6.0.0 Is Synchronous

✅ Yes

#### 2.5.7.7.0.0 Return Message

Connection Successful

#### 2.5.7.8.0.0 Has Return

✅ Yes

#### 2.5.7.9.0.0 Is Activation

❌ No

#### 2.5.7.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | TCP/SQL |
| Method | Connect |
| Parameters | Credentials fetched from AWS Secrets Manager. |
| Authentication | Username/Password |
| Error Handling | Connection timeouts; application must implement re... |
| Performance | Should be < 100ms. |

### 2.5.8.0.0.0 Health Check

#### 2.5.8.1.0.0 Source Id

EXTERNAL-EKS-CP

#### 2.5.8.2.0.0 Target Id

REPO-SVC-ALL

#### 2.5.8.3.0.0 Message

8. [Kubelet] Executes Readiness Probe

#### 2.5.8.4.0.0 Sequence Number

11

#### 2.5.8.5.0.0 Type

🔹 Health Check

#### 2.5.8.6.0.0 Is Synchronous

✅ Yes

#### 2.5.8.7.0.0 Return Message

HTTP 200 OK

#### 2.5.8.8.0.0 Has Return

✅ Yes

#### 2.5.8.9.0.0 Is Activation

❌ No

#### 2.5.8.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | HTTP |
| Method | GET /readyz |
| Parameters | *N/A* |
| Authentication | N/A |
| Error Handling | If probe fails, pod is not added to the service en... |
| Performance | Probe should return in < 1s. |

### 2.5.9.0.0.0 State Change

#### 2.5.9.1.0.0 Source Id

EXTERNAL-EKS-CP

#### 2.5.9.2.0.0 Target Id

EXTERNAL-EKS-CP

#### 2.5.9.3.0.0 Message

9. Marks pod as 'Ready', adds to Service endpoint

#### 2.5.9.4.0.0 Sequence Number

12

#### 2.5.9.5.0.0 Type

🔹 State Change

#### 2.5.9.6.0.0 Is Synchronous

✅ Yes

#### 2.5.9.7.0.0 Return Message



#### 2.5.9.8.0.0 Has Return

❌ No

#### 2.5.9.9.0.0 Is Activation

❌ No

#### 2.5.9.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | Kubernetes API |
| Method | UpdatePodStatus |
| Parameters | Condition: Ready=True |
| Authentication | N/A |
| Error Handling | N/A |
| Performance | Immediate. |

### 2.5.10.0.0.0 Lifecycle Management

#### 2.5.10.1.0.0 Source Id

EXTERNAL-EKS-CP

#### 2.5.10.2.0.0 Target Id

REPO-GW-API

#### 2.5.10.3.0.0 Message

10. Repeats pod startup and readiness probe process for API Gateway

#### 2.5.10.4.0.0 Sequence Number

13

#### 2.5.10.5.0.0 Type

🔹 Lifecycle Management

#### 2.5.10.6.0.0 Is Synchronous

❌ No

#### 2.5.10.7.0.0 Return Message



#### 2.5.10.8.0.0 Has Return

❌ No

#### 2.5.10.9.0.0 Is Activation

✅ Yes

#### 2.5.10.10.0.0 Technical Details

| Property | Value |
|----------|-------|
| Protocol | CRI, HTTP |
| Method | RunPodSandbox, GET /readyz |
| Parameters | As per steps 5-8. |
| Authentication | N/A |
| Error Handling | Same as microservice pods. Gateway readiness may d... |
| Performance | Slightly longer start time due to plugin loading. |

## 2.6.0.0.0.0 Notes

### 2.6.1.0.0.0 Content

#### 2.6.1.1.0.0 Content

Entire recovery process is automated by AWS managed services and Kubernetes orchestration. No manual intervention is required for a single AZ failure.

#### 2.6.1.2.0.0 Position

top-left

#### 2.6.1.3.0.0 Participant Id

*Not specified*

#### 2.6.1.4.0.0 Sequence Number

0

### 2.6.2.0.0.0 Content

#### 2.6.2.1.0.0 Content

Successful recovery is contingent on having sufficient compute capacity (EKS nodes) in the remaining healthy AZs.

#### 2.6.2.2.0.0 Position

bottom-right

#### 2.6.2.3.0.0 Participant Id

EXTERNAL-EKS-CP

#### 2.6.2.4.0.0 Sequence Number

7

### 2.6.3.0.0.0 Content

#### 2.6.3.1.0.0 Content

Upon recovery, AWS Load Balancers automatically detect the new healthy API Gateway pods and route traffic to them, fully restoring user access.

#### 2.6.3.2.0.0 Position

bottom-right

#### 2.6.3.3.0.0 Participant Id

REPO-GW-API

#### 2.6.3.4.0.0 Sequence Number

13

## 2.7.0.0.0.0 Implementation Guidance

| Property | Value |
|----------|-------|
| Security Requirements | Service pods must have IAM roles (IRSA) configured... |
| Performance Targets | RTO: The entire sequence from failure detection (S... |
| Error Handling Strategy | The primary risk is insufficient capacity in remai... |
| Testing Considerations | Regular disaster recovery testing is mandatory (RE... |
| Monitoring Requirements | Prometheus must monitor `kube_hpa_status_condition... |
| Deployment Considerations | All Kubernetes Deployments MUST use Pod Anti-Affin... |

