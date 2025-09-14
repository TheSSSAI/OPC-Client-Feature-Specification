sequenceDiagram
    actor "OPC Core Client" as OPCCoreClient
    participant "Primary OPC Server" as PrimaryOPCServer
    participant "Backup OPC Server" as BackupOPCServer
    participant "MQTT Broker" as MQTTBroker

    OPCCoreClient->>PrimaryOPCServer: 1. [Loop] checkServerStatus()
    PrimaryOPCServer-->>OPCCoreClient: ServerStatusDataType
    activate OPCCoreClient
    OPCCoreClient->>OPCCoreClient: 2. [Alt: failureCounter > config.failureThreshold] triggerFailover()
    OPCCoreClient->>PrimaryOPCServer: 3. disconnectSession()
    PrimaryOPCServer-->>OPCCoreClient: Status
    OPCCoreClient->>BackupOPCServer: 4. createSession(endpointUrl, securityPolicy)
    BackupOPCServer-->>OPCCoreClient: Session Object
    OPCCoreClient->>BackupOPCServer: 5. recreateSubscriptions()
    BackupOPCServer-->>OPCCoreClient: Subscription Statuses
    OPCCoreClient->>OPCCoreClient: 6. setActiveServer(backupServer)
    OPCCoreClient->>OPCCoreClient: 7. logFailoverEventToAuditTrail()
    OPCCoreClient->>MQTTBroker: 8. publish(topic, payload)

    note over BackupOPCServer: Backup Failure Scenario: If connection to the backup server fails after all retries (step 4), the...

    deactivate OPCCoreClient
