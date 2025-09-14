sequenceDiagram
    actor "OPC Core Client" as OPCCoreClient
    participant "MQTT Broker" as MQTTBroker
    participant "Alarm & Notification Service" as AlarmNotificationService
    participant "Asset & Topology Service" as AssetTopologyService
    participant "Relational Database" as RelationalDatabase

    activate OPCCoreClient
    OPCCoreClient->>OPCCoreClient: 1. Receives real-time data value (e.g., Temperature=45.3) for a tag associated with a configured AI model.
    OPCCoreClient->>OPCCoreClient: 2. Executes local ONNX model inference using the received value.
    OPCCoreClient-->>OPCCoreClient: Anomaly Score (e.g., 0.98)
    OPCCoreClient->>OPCCoreClient: 3. alt: [ score > threshold (e.g., 0.98 > 0.95) ]
    OPCCoreClient->>MQTTBroker: 4. Publishes 'AnomalyDetected' event.
    activate AlarmNotificationService
    MQTTBroker->>AlarmNotificationService: 5. Delivers 'AnomalyDetected' event.
    AlarmNotificationService->>AssetTopologyService: 6. Requests asset context for the model assignment.
    AssetTopologyService-->>AlarmNotificationService: 200 OK with { assetId, tenantId, modelName }
    AlarmNotificationService->>RelationalDatabase: 7. Persists a new 'Alarm' record and logs the anomaly for user feedback.
    RelationalDatabase-->>AlarmNotificationService: Success confirmation
    AlarmNotificationService->>AlarmNotificationService: 8. [Async] Triggers notification workflow based on user preferences.

    note over OPCCoreClient: Edge Processing (Steps 1-3) is latency-critical and designed for autonomous operation during netw...
    note over MQTTBroker: Event Publishing (Step 4) is decoupled. The OPC Client's responsibility ends after successfully q...
    note over AlarmNotificationService: Cloud processing is designed to be resilient. Failure to get context or persist the alarm results...

    deactivate AlarmNotificationService
    deactivate OPCCoreClient
