sequenceDiagram
    participant "External: AWS Infrastructure" as ExternalAWSInfrastructure
    participant "AWS RDS (Multi-AZ)" as AWSRDSMultiAZ
    participant "Amazon EKS Control Plane" as AmazonEKSControlPlane
    participant "All Microservices" as AllMicroservices
    participant "API Gateway" as APIGateway

    activate AWSRDSMultiAZ
    ExternalAWSInfrastructure->>AWSRDSMultiAZ: 1. 1. Availability Zone Failure Occurs
    AWSRDSMultiAZ->>AWSRDSMultiAZ: 2. 1a. [Internal] Detect primary instance failure and initiate automated failover
    AWSRDSMultiAZ-->>AWSRDSMultiAZ: Failover complete
    AWSRDSMultiAZ->>AWSRDSMultiAZ: 3. 1b. Promote standby replica in healthy AZ to new primary instance
    AWSRDSMultiAZ->>ExternalAWSInfrastructure: 4. 1c. Update DNS CNAME record for database endpoint to point to new primary IP
    activate AmazonEKSControlPlane
    ExternalAWSInfrastructure->>AmazonEKSControlPlane: 5. 2. AZ Failure impacts EKS nodes and pods
    AmazonEKSControlPlane->>AmazonEKSControlPlane: 6. 3. [Controller Manager] Marks nodes in failed AZ as 'NotReady'
    AmazonEKSControlPlane->>AmazonEKSControlPlane: 7. 4. [Scheduler] Identifies unsatisfied replicas for Deployments and schedules new pods on healthy nodes
    AmazonEKSControlPlane-->>AmazonEKSControlPlane: Pods scheduled
    activate AllMicroservices
    AmazonEKSControlPlane->>AllMicroservices: 8. 5. [Kubelet] Starts new service pod container
    AllMicroservices->>ExternalAWSInfrastructure: 9. 6. Resolve updated database DNS endpoint
    ExternalAWSInfrastructure-->>AllMicroservices: New Primary DB IP Address
    AllMicroservices->>AWSRDSMultiAZ: 10. 7. Establish database connection
    AWSRDSMultiAZ-->>AllMicroservices: Connection Successful
    AmazonEKSControlPlane->>AllMicroservices: 11. 8. [Kubelet] Executes Readiness Probe
    AllMicroservices-->>AmazonEKSControlPlane: HTTP 200 OK
    AmazonEKSControlPlane->>AmazonEKSControlPlane: 12. 9. Marks pod as 'Ready', adds to Service endpoint
    activate APIGateway
    AmazonEKSControlPlane->>APIGateway: 13. 10. Repeats pod startup and readiness probe process for API Gateway

    note over AmazonEKSControlPlane: Successful recovery is contingent on having sufficient compute capacity (EKS nodes) in the remain...
    note over APIGateway: Upon recovery, AWS Load Balancers automatically detect the new healthy API Gateway pods and route...

    deactivate APIGateway
    deactivate AllMicroservices
    deactivate AmazonEKSControlPlane
    deactivate AWSRDSMultiAZ
