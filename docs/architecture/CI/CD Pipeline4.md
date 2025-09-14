# 1 Pipelines

## 1.1 pl-backend-svc-001

### 1.1.1 Id

pl-backend-svc-001

### 1.1.2 Name

Backend Microservice CI/CD Pipeline

### 1.1.3 Description

Builds, tests, scans, and deploys a .NET 8 microservice to the Amazon EKS cluster. This pipeline includes mandatory quality gates for test coverage, security, and performance as required by system specifications.

### 1.1.4 Stages

#### 1.1.4.1 Build and Unit Test

##### 1.1.4.1.1 Name

Build and Unit Test

##### 1.1.4.1.2 Steps

- dotnet restore
- dotnet build --configuration Release
- dotnet test --collect:"XPlat Code Coverage"

##### 1.1.4.1.3 Environment

###### 1.1.4.1.3.1 Dotnet Version

8.0

##### 1.1.4.1.4.0 Quality Gates

- {'name': 'Unit Test and Coverage Check', 'criteria': ['Unit tests must pass', 'Code coverage >= 80% (REQ-NFR-006)'], 'blocking': True}

#### 1.1.4.2.0.0 Security Analysis

##### 1.1.4.2.1.0 Name

Security Analysis

##### 1.1.4.2.2.0 Steps

- Run SAST (Static Application Security Testing) scan
- Run dependency vulnerability scan for NuGet packages

##### 1.1.4.2.3.0 Environment

*No data available*

##### 1.1.4.2.4.0 Quality Gates

- {'name': 'Vulnerability Scan', 'criteria': ['Zero critical vulnerabilities found'], 'blocking': True}

#### 1.1.4.3.0.0 Package and Push Container

##### 1.1.4.3.1.0 Name

Package and Push Container

##### 1.1.4.3.2.0 Steps

- docker build -t $ECR_REPO/$SERVICE_NAME:$TAG .
- docker scan $ECR_REPO/$SERVICE_NAME:$TAG
- docker push $ECR_REPO/$SERVICE_NAME:$TAG

##### 1.1.4.3.3.0 Environment

###### 1.1.4.3.3.1 Ecr Repo

aws_account_id.dkr.ecr.region.amazonaws.com

##### 1.1.4.3.4.0 Quality Gates

*No items available*

#### 1.1.4.4.0.0 Deploy to Staging

##### 1.1.4.4.1.0 Name

Deploy to Staging

##### 1.1.4.4.2.0 Steps

- Run EF Core database migration job on Staging DB
- kubectl apply -f k8s/staging-deployment.yaml --namespace staging

##### 1.1.4.4.3.0 Environment

###### 1.1.4.4.3.1 Kube Context

staging-eks-cluster

##### 1.1.4.4.4.0 Quality Gates

*No items available*

#### 1.1.4.5.0.0 Integration and Performance Testing

##### 1.1.4.5.1.0 Name

Integration and Performance Testing

##### 1.1.4.5.2.0 Steps

- Run Playwright E2E tests against staging endpoint (REQ-1-089)
- Execute automated load tests against staging endpoint (REQ-NFR-008)

##### 1.1.4.5.3.0 Environment

###### 1.1.4.5.3.1 Api Endpoint

🔗 [https://staging.api.example.com](https://staging.api.example.com)

##### 1.1.4.5.4.0 Quality Gates

- {'name': 'E2E and Performance Validation', 'criteria': ['All E2E tests must pass', 'P95 latency < 200ms (REQ-NFR-001)'], 'blocking': True}

#### 1.1.4.6.0.0 Production Deployment Approval

##### 1.1.4.6.1.0 Name

Production Deployment Approval

##### 1.1.4.6.2.0 Steps

- Wait for manual approval from 'Administrator' or 'Lead Engineer' role

##### 1.1.4.6.3.0 Environment

*No data available*

##### 1.1.4.6.4.0 Quality Gates

*No items available*

#### 1.1.4.7.0.0 Deploy to Production

##### 1.1.4.7.1.0 Name

Deploy to Production

##### 1.1.4.7.2.0 Steps

- Run EF Core database migration job on Production DB
- kubectl apply -f k8s/prod-deployment.yaml --namespace production
- Run smoke tests to confirm service health

##### 1.1.4.7.3.0 Environment

###### 1.1.4.7.3.1 Kube Context

production-eks-cluster

##### 1.1.4.7.4.0 Quality Gates

*No items available*

## 1.2.0.0.0.0 pl-frontend-spa-002

### 1.2.1.0.0.0 Id

pl-frontend-spa-002

### 1.2.2.0.0.0 Name

Frontend Application CI/CD Pipeline

### 1.2.3.0.0.0 Description

Builds, tests, scans, and deploys the React 18 single-page application to a static hosting environment like AWS S3/CloudFront.

### 1.2.4.0.0.0 Stages

#### 1.2.4.1.0.0 Build and Unit Test

##### 1.2.4.1.1.0 Name

Build and Unit Test

##### 1.2.4.1.2.0 Steps

- npm install
- npm run lint
- npm test -- --coverage

##### 1.2.4.1.3.0 Environment

###### 1.2.4.1.3.1 Node Version

20

##### 1.2.4.1.4.0 Quality Gates

*No items available*

#### 1.2.4.2.0.0 Security Analysis

##### 1.2.4.2.1.0 Name

Security Analysis

##### 1.2.4.2.2.0 Steps

- npm audit --audit-level=critical

##### 1.2.4.2.3.0 Environment

*No data available*

##### 1.2.4.2.4.0 Quality Gates

- {'name': 'Dependency Vulnerability Scan', 'criteria': ['Zero critical vulnerabilities found'], 'blocking': True}

#### 1.2.4.3.0.0 Build Production Artifact

##### 1.2.4.3.1.0 Name

Build Production Artifact

##### 1.2.4.3.2.0 Steps

- npm run build

##### 1.2.4.3.3.0 Environment

###### 1.2.4.3.3.1 Vite Api Url

🔗 [https://api.example.com](https://api.example.com)

##### 1.2.4.3.4.0 Quality Gates

*No items available*

#### 1.2.4.4.0.0 Deploy and Test Staging

##### 1.2.4.4.1.0 Name

Deploy and Test Staging

##### 1.2.4.4.2.0 Steps

- aws s3 sync dist/ s3://staging-bucket/
- Run Playwright E2E tests against staging URL (REQ-1-089)

##### 1.2.4.4.3.0 Environment

*No data available*

##### 1.2.4.4.4.0 Quality Gates

- {'name': 'Staging E2E Validation', 'criteria': ['All E2E tests must pass'], 'blocking': True}

#### 1.2.4.5.0.0 Production Deployment Approval

##### 1.2.4.5.1.0 Name

Production Deployment Approval

##### 1.2.4.5.2.0 Steps

- Wait for manual approval from 'Administrator' or 'Lead Engineer' role

##### 1.2.4.5.3.0 Environment

*No data available*

##### 1.2.4.5.4.0 Quality Gates

*No items available*

#### 1.2.4.6.0.0 Deploy to Production

##### 1.2.4.6.1.0 Name

Deploy to Production

##### 1.2.4.6.2.0 Steps

- aws s3 sync dist/ s3://production-bucket/
- aws cloudfront create-invalidation --distribution-id YOUR_DIST_ID --paths "/*"

##### 1.2.4.6.3.0 Environment

*No data available*

##### 1.2.4.6.4.0 Quality Gates

*No items available*

## 1.3.0.0.0.0 pl-opc-client-003

### 1.3.1.0.0.0 Id

pl-opc-client-003

### 1.3.2.0.0.0 Name

OPC Core Client CI Pipeline

### 1.3.3.0.0.0 Description

Builds, tests, and packages the OPC Core Client .NET application as a versioned Docker container. The pipeline's responsibility ends at publishing the artifact to the container registry. Deployment is handled by the Central Management Plane (REQ-BIZ-002).

### 1.3.4.0.0.0 Stages

#### 1.3.4.1.0.0 Build and Unit Test

##### 1.3.4.1.1.0 Name

Build and Unit Test

##### 1.3.4.1.2.0 Steps

- dotnet restore
- dotnet build --configuration Release
- dotnet test --collect:"XPlat Code Coverage"

##### 1.3.4.1.3.0 Environment

*No data available*

##### 1.3.4.1.4.0 Quality Gates

- {'name': 'Unit Test and Coverage Check', 'criteria': ['Unit tests must pass', 'Code coverage >= 80% (REQ-NFR-006)'], 'blocking': True}

#### 1.3.4.2.0.0 Security Analysis

##### 1.3.4.2.1.0 Name

Security Analysis

##### 1.3.4.2.2.0 Steps

- Run SAST (Static Application Security Testing) scan
- Run dependency vulnerability scan for NuGet packages

##### 1.3.4.2.3.0 Environment

*No data available*

##### 1.3.4.2.4.0 Quality Gates

- {'name': 'Vulnerability Scan', 'criteria': ['Zero critical vulnerabilities found'], 'blocking': True}

#### 1.3.4.3.0.0 Package and Publish Container

##### 1.3.4.3.1.0 Name

Package and Publish Container

##### 1.3.4.3.2.0 Steps

- docker build -f Dockerfile.x86_64 -t $ECR_REPO/opc-core-client:$TAG-amd64 .
- docker build -f Dockerfile.jetson -t $ECR_REPO/opc-core-client:$TAG-arm64 .
- docker scan $ECR_REPO/opc-core-client:$TAG-amd64
- docker push --all-tags $ECR_REPO/opc-core-client

##### 1.3.4.3.3.0 Environment

###### 1.3.4.3.3.1 Ecr Repo

aws_account_id.dkr.ecr.region.amazonaws.com

##### 1.3.4.3.4.0 Quality Gates

*No items available*

## 1.4.0.0.0.0 pl-iac-terraform-004

### 1.4.1.0.0.0 Id

pl-iac-terraform-004

### 1.4.2.0.0.0 Name

Infrastructure (Terraform) Pipeline

### 1.4.3.0.0.0 Description

Manages cloud infrastructure using Terraform as required by REQ-1-089. This pipeline ensures infrastructure changes are planned, reviewed, and applied systematically.

### 1.4.4.0.0.0 Stages

#### 1.4.4.1.0.0 Validate and Plan

##### 1.4.4.1.1.0 Name

Validate and Plan

##### 1.4.4.1.2.0 Steps

- terraform init -backend-config=backend.conf
- terraform validate
- terraform plan -out=tfplan

##### 1.4.4.1.3.0 Environment

*No data available*

##### 1.4.4.1.4.0 Quality Gates

*No items available*

#### 1.4.4.2.0.0 Manual Approval of Plan

##### 1.4.4.2.1.0 Name

Manual Approval of Plan

##### 1.4.4.2.2.0 Steps

- Publish Terraform plan output for review
- Wait for manual approval from 'Administrator' role

##### 1.4.4.2.3.0 Environment

*No data available*

##### 1.4.4.2.4.0 Quality Gates

*No items available*

#### 1.4.4.3.0.0 Apply Infrastructure Changes

##### 1.4.4.3.1.0 Name

Apply Infrastructure Changes

##### 1.4.4.3.2.0 Steps

- terraform apply "tfplan"

##### 1.4.4.3.3.0 Environment

*No data available*

##### 1.4.4.3.4.0 Quality Gates

*No items available*

# 2.0.0.0.0.0 Configuration

| Property | Value |
|----------|-------|
| Artifact Repository | AWS ECR (aws_account_id.dkr.ecr.region.amazonaws.c... |
| Default Branch | main |
| Retention Policy | Retain last 20 builds and all tagged release build... |
| Notification Channel | slack#devops-alerts |

