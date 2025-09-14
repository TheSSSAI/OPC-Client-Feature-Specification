# 1 System Overview

## 1.1 Analysis Date

2025-06-13

## 1.2 Technology Stack

- GitHub Actions
- Docker
- Amazon ECR
- Kubernetes (EKS)
- Terraform
- Snyk
- SonarQube
- Playwright

## 1.3 Architecture Patterns

- Multi-Pipeline Strategy (Backend, Frontend, Infrastructure)
- GitOps for configuration management
- Immutable Infrastructure
- DevSecOps Integration

## 1.4 Data Sensitivity

- Source Code
- Container Images
- Test Data
- Infrastructure State

## 1.5 Regulatory Considerations

- Pipeline must enforce security scans to support REQ-NFR-003
- Audit trail of all deployments and approvals required for compliance (REQ-FR-005)

## 1.6 System Criticality

mission-critical

# 2.0 Data Classification And Protection Requirements

## 2.1 Sensitive Data Components

### 2.1.1 Data Type

#### 2.1.1.1 Data Type

ContainerImage

#### 2.1.1.2 Location

Amazon ECR

#### 2.1.1.3 Volume

High (multiple services, frequent builds)

#### 2.1.1.4 Sensitivity

medium

#### 2.1.1.5 Regulatory Requirements

- OWASP Dependency-Check / Snyk scan for vulnerabilities
- Static analysis of container image layers (e.g., Trivy)

#### 2.1.1.6 Access Patterns

Pull by Kubernetes, Push by CI/CD pipeline

### 2.1.2.0 Data Type

#### 2.1.2.1 Data Type

TerraformPlan

#### 2.1.2.2 Location

CI/CD job artifacts

#### 2.1.2.3 Volume

Medium

#### 2.1.2.4 Sensitivity

high

#### 2.1.2.5 Regulatory Requirements

- Requires manual review and approval before apply
- Sensitive variable values must be redacted from logs

#### 2.1.2.6 Access Patterns

Read by DevOps engineers for approval

### 2.1.3.0 Data Type

#### 2.1.3.1 Data Type

TestCoverageReport

#### 2.1.3.2 Location

CI/CD job artifacts

#### 2.1.3.3 Volume

High

#### 2.1.3.4 Sensitivity

low

#### 2.1.3.5 Regulatory Requirements

- Backend code coverage must be >= 80% (REQ-NFR-006)

#### 2.1.3.6 Access Patterns

Read by development team and quality gates

## 2.2.0.0 Regulatory Compliance

*No items available*

## 2.3.0.0 Data Sensitivity Levels

*No items available*

## 2.4.0.0 Data Location Mapping

*No items available*

## 2.5.0.0 Critical System Configurations

*No items available*

## 2.6.0.0 Recovery Prioritization

*No items available*

## 2.7.0.0 Backup Verification Requirements

*No items available*

# 3.0.0.0 Backup Strategy Design

## 3.1.0.0 Backup Types

### 3.1.1.0 Docker Image Push

#### 3.1.1.1 Type

🔹 Docker Image Push

#### 3.1.1.2 Applicable Data

- Backend Microservices
- Frontend SPA
- OPC Core Client

#### 3.1.1.3 Frequency

On every successful build on main branch

#### 3.1.1.4 Retention

Based on ECR lifecycle policy

#### 3.1.1.5 Storage Location

Amazon ECR

#### 3.1.1.6 Justification

Stores versioned, immutable build artifacts for deployment.

### 3.1.2.0 Terraform State Backup

#### 3.1.2.1 Type

🔹 Terraform State Backup

#### 3.1.2.2 Applicable Data

- AWS Infrastructure State

#### 3.1.2.3 Frequency

Continuous (on every apply)

#### 3.1.2.4 Retention

Versioned history enabled

#### 3.1.2.5 Storage Location

Amazon S3

#### 3.1.2.6 Justification

Critical for infrastructure integrity and recovery as per REQ-1-089.

## 3.2.0.0 Backup Frequency

*No items available*

## 3.3.0.0 Rotation And Retention

- {'backupType': 'ContainerImage', 'rotationScheme': 'simple', 'retentionPeriod': 'Retain tagged images indefinitely, untagged images for 30 days', 'archivalPolicy': 'N/A', 'deletionPolicy': 'ECR Lifecycle Policy'}

## 3.4.0.0 Storage Requirements

### 3.4.1.0 Storage Type

#### 3.4.1.1 Storage Type

cloud

#### 3.4.1.2 Location

Amazon ECR (artifact repository)

#### 3.4.1.3 Redundancy

multi-az

#### 3.4.1.4 Access Time

immediate

#### 3.4.1.5 Cost Tier

hot

### 3.4.2.0 Storage Type

#### 3.4.2.1 Storage Type

cloud

#### 3.4.2.2 Location

Amazon S3 (for Terraform state)

#### 3.4.2.3 Redundancy

multi-az

#### 3.4.2.4 Access Time

immediate

#### 3.4.2.5 Cost Tier

hot

## 3.5.0.0 Backup Handling Procedures

*No items available*

## 3.6.0.0 Verification Processes

*No items available*

## 3.7.0.0 Catalog And Indexing

*No data available*

## 3.8.0.0 Database Specific Backups

*No items available*

# 4.0.0.0 Recovery Objectives Definition

## 4.1.0.0 Recovery Point Objectives

- {'dataCategory': 'Production Deployment', 'rpo': 'N/A', 'justification': 'Rollback recovers to the last known good versioned artifact.', 'businessImpact': 'High', 'complianceRequirement': 'N/A'}

## 4.2.0.0 Recovery Time Objectives

- {'systemComponent': 'Production Microservice', 'rto': '< 15 minutes', 'justification': 'Time to trigger a rollback workflow and have the previous version serving traffic via Blue/Green deployment switch.', 'criticality': 'mission-critical', 'dependencies': ['Amazon ECR', 'Kubernetes API']}

## 4.3.0.0 Recovery Prioritization

*No items available*

## 4.4.0.0 Recovery Verification Procedures

*No items available*

## 4.5.0.0 Recovery Testing Schedule

*No items available*

## 4.6.0.0 Recovery Team Roles

*No items available*

## 4.7.0.0 Recovery Runbooks

- {'scenario': 'Production Deployment Failure', 'procedures': ['1. Halt the Blue/Green promotion.', "2. Re-route 100% of traffic to the stable 'blue' environment.", '3. Trigger an alert to the development team.', '4. Automatically create a high-priority incident ticket.'], 'decisionPoints': [], 'escalationCriteria': [], 'updateFrequency': 'quarterly'}

## 4.8.0.0 Alternate Site Capabilities

*No items available*

# 5.0.0.0 Encryption And Security Design

## 5.1.0.0 Backup Encryption Requirements

### 5.1.1.0 Data Type

#### 5.1.1.1 Data Type

All Source Code

#### 5.1.1.2 Encryption Algorithm

SAST (Static Application Security Testing)

#### 5.1.1.3 Key Length

N/A

#### 5.1.1.4 Compliance Standard

SonarQube Quality Profile

#### 5.1.1.5 Mandatory Fields

*No items available*

### 5.1.2.0 Data Type

#### 5.1.2.1 Data Type

All Dependencies

#### 5.1.2.2 Encryption Algorithm

SCA (Software Composition Analysis)

#### 5.1.2.3 Key Length

N/A

#### 5.1.2.4 Compliance Standard

Snyk / OWASP Dependency-Check

#### 5.1.2.5 Mandatory Fields

*No items available*

## 5.2.0.0 Key Management Procedures

- {'keyType': 'Pipeline Secrets', 'keyRotationFrequency': 'As per policy', 'keyEscrowPolicy': 'N/A', 'keyRecoveryProcedure': 'Managed by AWS Secrets Manager', 'accessControls': ['GitHub Actions OIDC integration with AWS IAM roles']}

## 5.3.0.0 Backup Access Controls

*No items available*

## 5.4.0.0 Secure Transport

- {'transportMethod': 'https', 'encryptionInTransit': True, 'certificateValidation': True, 'integrityChecking': True}

## 5.5.0.0 Backup Audit Logging

- {'eventType': 'build|test|scan|deploy|approve', 'logDetail': 'full', 'retentionPeriod': '1 year', 'logProtection': 'Immutable GitHub Actions logs'}

## 5.6.0.0 Chain Of Custody Procedures

*No items available*

## 5.7.0.0 Secure Erasure Procedures

*No items available*

# 6.0.0.0 Disaster Recovery Planning

## 6.1.0.0 Disaster Scenarios

### 6.1.1.0 Scenario

#### 6.1.1.1 Scenario

Production Deployment Failure (Critical Bug)

#### 6.1.1.2 Impact

partial-failure

#### 6.1.1.3 Likelihood

medium

#### 6.1.1.4 Response Strategy

Automated Rollback via Blue/Green Traffic Switch

### 6.1.2.0 Scenario

#### 6.1.2.1 Scenario

Critical Vulnerability Found in Security Scan

#### 6.1.2.2 Impact

extended-outage

#### 6.1.2.3 Likelihood

low

#### 6.1.2.4 Response Strategy

Block promotion to Production via Quality Gate

## 6.2.0.0 System Recovery Procedures

*No items available*

## 6.3.0.0 Alternate Processing Capabilities

- {'capability': 'Blue/Green Deployment Model', 'location': 'Same Kubernetes Cluster', 'capacity': 'Full duplicate capacity during deployment window', 'activationTriggers': ['Manual approval for production promotion']}

## 6.4.0.0 Communication Procedures

*No items available*

## 6.5.0.0 Recovery Sequence And Dependencies

*No items available*

## 6.6.0.0 Data Synchronization

*No items available*

## 6.7.0.0 Disaster Declaration Criteria

*No items available*

## 6.8.0.0 Post Recovery Validation

*No items available*

# 7.0.0.0 Testing And Validation Strategy

## 7.1.0.0 Recovery Testing Procedures

### 7.1.1.0 Test Type

#### 7.1.1.1 Test Type

Unit & Integration Tests

#### 7.1.1.2 Frequency

On every commit

#### 7.1.1.3 Scope

- Backend Microservices
- Frontend SPA

#### 7.1.1.4 Environment

CI runner

#### 7.1.1.5 Impact Minimization

*No items available*

### 7.1.2.0 Test Type

#### 7.1.2.1 Test Type

End-to-End (E2E) Tests

#### 7.1.2.2 Frequency

After every deployment to Staging

#### 7.1.2.3 Scope

- User-facing workflows

#### 7.1.2.4 Environment

staging

#### 7.1.2.5 Impact Minimization

*No items available*

### 7.1.3.0 Test Type

#### 7.1.3.1 Test Type

Performance & Load Tests

#### 7.1.3.2 Frequency

Before promotion to Production

#### 7.1.3.3 Scope

- Critical API endpoints
- Data ingestion pipeline

#### 7.1.3.4 Environment

staging

#### 7.1.3.5 Impact Minimization

*No items available*

## 7.2.0.0 Backup Verification Schedule

*No items available*

## 7.3.0.0 Recovery Testing Success Criteria

*No items available*

## 7.4.0.0 Tabletop Exercises

- {'scenario': 'Manual Approval Gate for Production Deployment', 'frequency': 'On every production release', 'participants': ['Release Manager', 'Lead Engineer'], 'objectives': ['Verify completion of UAT and performance tests (REQ-NFR-008)', 'Ensure change aligns with MOC procedures (REQ-CON-005)'], 'improvementTracking': False}

## 7.5.0.0 Backup Integrity Validation

*No items available*

## 7.6.0.0 Testing Documentation Requirements

*No items available*

## 7.7.0.0 Continuous Improvement Process

*No data available*

# 8.0.0.0 Project Specific Backup Strategy

## 8.1.0.0 Strategy

### 8.1.1.0 Id

cicd-backend-service

### 8.1.2.0 Type

🔹 Hybrid

### 8.1.3.0 Schedule

On commit to main (Staging), On git tag v* (Production)

### 8.1.4.0 Retention Period Days

30

### 8.1.5.0 Backup Locations

- Amazon ECR

### 8.1.6.0 Configuration

| Property | Value |
|----------|-------|
| Backup Window | N/A |
| Compression | enabled |
| Verification | automated |
| Throttling | N/A |
| Priority | high |
| Max Concurrent Transfers | N/A |
| Parallelism | 4 |
| Checksum Validation | ✅ |

### 8.1.7.0 Encryption

#### 8.1.7.1 Enabled

✅ Yes

#### 8.1.7.2 Algorithm

AES-256

#### 8.1.7.3 Key Management Service

AWS KMS

#### 8.1.7.4 Encrypted Fields

*No items available*

#### 8.1.7.5 Configuration

| Property | Value |
|----------|-------|
| Key Rotation | 1 year |
| Access Policy | strict |
| Key Identifier | ecr-key |
| Multi Factor | disabled |

#### 8.1.7.6 Transit Encryption

✅ Yes

#### 8.1.7.7 At Rest Encryption

✅ Yes

## 8.2.0.0 Component Specific Strategies

### 8.2.1.0 Component

#### 8.2.1.1 Component

cicd-frontend-spa

#### 8.2.1.2 Backup Type

Hybrid

#### 8.2.1.3 Frequency

On commit to main (Staging), On git tag v* (Production)

#### 8.2.1.4 Retention

30 days (untagged)

#### 8.2.1.5 Special Requirements

- Build pipeline uses Node.js and Vite.
- Resulting Docker image contains Nginx to serve static files.

### 8.2.2.0 Component

#### 8.2.2.1 Component

cicd-opc-core-client

#### 8.2.2.2 Backup Type

Hybrid

#### 8.2.2.3 Frequency

On git tag v*

#### 8.2.2.4 Retention

30 days (untagged)

#### 8.2.2.5 Special Requirements

- Builds multi-platform (linux/amd64, windows/amd64) Docker image.
- Pipeline only publishes artifact; deployment is managed by the application.

### 8.2.3.0 Component

#### 8.2.3.1 Component

cicd-infrastructure-terraform

#### 8.2.3.2 Backup Type

Full

#### 8.2.3.3 Frequency

On commit to main

#### 8.2.3.4 Retention

State file versioning enabled

#### 8.2.3.5 Special Requirements

- Pipeline performs terraform plan and requires manual approval for apply.
- Distinct workspaces for staging and production environments.

## 8.3.0.0 Configuration

### 8.3.1.0 Rpo

Time to Detect Build Failure: < 10 minutes

### 8.3.2.0 Rto

Time to Rollback Failed Deployment: < 15 minutes

### 8.3.3.0 Backup Verification

Automated security and quality scans in pipeline

### 8.3.4.0 Disaster Recovery Site

Blue/Green deployment in the same region

### 8.3.5.0 Compliance Standard

Backend test coverage > 80%

### 8.3.6.0 Audit Logging

enabled

### 8.3.7.0 Test Restore Frequency

N/A

### 8.3.8.0 Notification Channel

Slack

### 8.3.9.0 Alert Thresholds

No critical security vulnerabilities

### 8.3.10.0 Retry Policy

3 attempts on transient errors

### 8.3.11.0 Backup Admin

N/A

### 8.3.12.0 Escalation Path

N/A

### 8.3.13.0 Reporting Schedule

On every build

### 8.3.14.0 Cost Optimization

disabled

### 8.3.15.0 Maintenance Window

N/A

### 8.3.16.0 Environment Specific

#### 8.3.16.1 Production

| Property | Value |
|----------|-------|
| Rpo | N/A |
| Rto | < 15 minutes |
| Testing Frequency | Triggered by git tag |

#### 8.3.16.2 Staging

| Property | Value |
|----------|-------|
| Rpo | N/A |
| Rto | < 30 minutes |
| Testing Frequency | Triggered by push to main |

#### 8.3.16.3 Development

| Property | Value |
|----------|-------|
| Rpo | N/A |
| Rto | N/A |
| Testing Frequency | Triggered by push to feature branch |

# 9.0.0.0 Implementation Priority

## 9.1.0.0 Component

### 9.1.1.0 Component

Backend Microservice CI/CD Pipeline Foundation

### 9.1.2.0 Priority

🔴 high

### 9.1.3.0 Dependencies

- ECR Repository
- GitHub Actions Runners

### 9.1.4.0 Estimated Effort

Medium

### 9.1.5.0 Risk Level

medium

## 9.2.0.0 Component

### 9.2.1.0 Component

Infrastructure (Terraform) CI/CD Pipeline

### 9.2.2.0 Priority

🟡 medium

### 9.2.3.0 Dependencies

- S3 Bucket for state
- IAM Roles for pipeline execution

### 9.2.4.0 Estimated Effort

Medium

### 9.2.5.0 Risk Level

high

## 9.3.0.0 Component

### 9.3.1.0 Component

Frontend SPA and OPC Client Pipelines

### 9.3.2.0 Priority

🟢 low

### 9.3.3.0 Dependencies

- Backend Pipeline Foundation

### 9.3.4.0 Estimated Effort

Low

### 9.3.5.0 Risk Level

low

# 10.0.0.0 Risk Assessment

## 10.1.0.0 Risk

### 10.1.1.0 Risk

Slow pipeline execution time delays feedback to developers.

### 10.1.2.0 Impact

medium

### 10.1.3.0 Probability

high

### 10.1.4.0 Mitigation

Optimize Docker image layer caching. Run test and scan stages in parallel. Use dedicated, appropriately-sized runners.

### 10.1.5.0 Contingency Plan

Provide developers with tools to run all tests and scans locally before pushing code.

## 10.2.0.0 Risk

### 10.2.1.0 Risk

Flaky E2E tests block valid deployments.

### 10.2.2.0 Impact

high

### 10.2.3.0 Probability

medium

### 10.2.4.0 Mitigation

Implement a retry mechanism for failed E2E tests. Invest in making tests more robust. Establish a clear process for temporarily disabling a known flaky test.

### 10.2.5.0 Contingency Plan

Allow a senior engineer to manually override a failed E2E test quality gate with justification.

# 11.0.0.0 Recommendations

## 11.1.0.0 Category

### 11.1.1.0 Category

🔹 Pipeline Design

### 11.1.2.0 Recommendation

Implement reusable GitHub Actions workflows (e.g., '.NET Build & Test', 'Docker Build & Push') to ensure consistency across all microservice pipelines.

### 11.1.3.0 Justification

Reduces code duplication, simplifies maintenance, and ensures all services adhere to the same quality and security standards.

### 11.1.4.0 Priority

🔴 high

### 11.1.5.0 Implementation Notes

Use composite actions or reusable workflow calls in GitHub Actions.

## 11.2.0.0 Category

### 11.2.1.0 Category

🔹 Deployment Strategy

### 11.2.2.0 Recommendation

Separate database schema migrations (EF Core) from application deployment. Run migrations as a one-off, privileged Kubernetes Job before deploying the new application version.

### 11.2.3.0 Justification

Decouples schema changes from application startup, preventing failed deployments due to migration errors and avoiding race conditions in multi-pod environments.

### 11.2.4.0 Priority

🔴 high

### 11.2.5.0 Implementation Notes

The pipeline should have a distinct 'migrate' step that requires acknowledgment before the 'deploy' step proceeds.

