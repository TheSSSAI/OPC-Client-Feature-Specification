sequenceDiagram
    participant "Monitoring System" as MonitoringSystem
    participant "SRE Team" as SRETeam
    participant "AWS RDS (Cross-Region Replica)" as AWSRDSCrossRegionReplica
    participant "Terraform (IaC)" as TerraformIaC
    participant "Amazon EKS (DR Region)" as AmazonEKSDRRegion
    participant "CI/CD Pipeline" as CICDPipeline
    participant "AWS Route 53" as AWSRoute53

    MonitoringSystem->>SRETeam: 1. Triggers CRITICAL 'Region Unreachable' alert via PagerDuty
    activate AWSRDSCrossRegionReplica
    SRETeam->>AWSRDSCrossRegionReplica: 2. [Manual] Executes command to promote Read Replica to standalone master instance
    AWSRDSCrossRegionReplica-->>SRETeam: DB instance status: 'available' (no longer a replica)
    activate TerraformIaC
    SRETeam->>TerraformIaC: 3. [Manual] Executes 'terraform apply' using DR environment configuration
    TerraformIaC-->>SRETeam: Terraform apply complete. Outputs: EKS cluster endpoint, VPC ID, etc.
    activate AmazonEKSDRRegion
    TerraformIaC->>AmazonEKSDRRegion: 3.1. Makes AWS API calls to create VPC, Subnets, EKS Cluster, Node Groups, and Load Balancers
    AmazonEKSDRRegion-->>TerraformIaC: Resource creation success responses
    SRETeam->>CICDPipeline: 4. [Manual] Triggers 'Deploy to DR' workflow
    CICDPipeline-->>SRETeam: Workflow execution started successfully
    CICDPipeline->>AmazonEKSDRRegion: 5. Builds, pushes images, and applies Kubernetes manifests for all microservices
    AmazonEKSDRRegion-->>CICDPipeline: All pods are in 'Running' state and readiness probes are passing
    activate AWSRoute53
    SRETeam->>AWSRoute53: 6. [Manual] Updates primary CNAME record to point to DR region's Load Balancer DNS
    AWSRoute53-->>SRETeam: Change request status: 'INSYNC'
    SRETeam->>AmazonEKSDRRegion: 7. [Manual] Performs validation smoke tests against public endpoints
    AmazonEKSDRRegion-->>SRETeam: HTTP 200 OK, application homepage loads

    note over TerraformIaC: The Terraform state file must be stored in a globally redundant location (e.g., S3 with Cross-Reg...
    note over AWSRDSCrossRegionReplica: This is the point of no return. Once the replica is promoted, it cannot be reverted. The primary ...

    deactivate AWSRoute53
    deactivate AmazonEKSDRRegion
    deactivate TerraformIaC
    deactivate AWSRDSCrossRegionReplica
