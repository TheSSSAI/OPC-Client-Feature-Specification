sequenceDiagram
    participant "Device Management Service" as DeviceManagementService
    actor "OPC Core Client" as OPCCoreClient
    participant "Asset Administration Shell" as AssetAdministrationShell
    participant "Data Ingestion Service" as DataIngestionService

    activate OPCCoreClient
    DeviceManagementService->>OPCCoreClient: 1. [MQTT] publishClientConfigurationUpdate(type='AAS', endpoint='...', auth={...})
    OPCCoreClient->>OPCCoreClient: 2. processConfigurationUpdate(): Initialize AAS Adapter & Circuit Breaker
    activate AssetAdministrationShell
    OPCCoreClient->>AssetAdministrationShell: 3. [HTTP GET] /shells
    AssetAdministrationShell-->>OPCCoreClient: 200 OK, { shells: [...] }
    OPCCoreClient->>AssetAdministrationShell: 4. [HTTP GET] /shells/{shellId}/submodels
    AssetAdministrationShell-->>OPCCoreClient: 200 OK, { submodels: [...] }
    OPCCoreClient->>OPCCoreClient: 5. mapAasToInternalTagModel(): Translate submodels & properties to tag structure
    OPCCoreClient-->>OPCCoreClient: Internal Tag Namespace created
    OPCCoreClient->>AssetAdministrationShell: 6. [HTTP GET] /submodels/{submodelId}/submodel-elements/{elementId}/value
    AssetAdministrationShell-->>OPCCoreClient: 200 OK, { value: ... }
    OPCCoreClient->>DataIngestionService: 7. [gRPC Stream] streamDataPoint(tagId, timestamp, value, isSimulation=true)
    OPCCoreClient->>AssetAdministrationShell: 8. [HTTP PUT] /submodels/{submodelId}/submodel-elements/{elementId}/value
    AssetAdministrationShell-->>OPCCoreClient: 204 No Content

    note over OPCCoreClient: Adapter Pattern: The OPC Core Client's 'AAS Adapter' module encapsulates all logic for HTTP/REST ...
    note over DataIngestionService: Simulation Flag: The 'isSimulation=true' flag in the gRPC message is critical. The backend uses t...
    note over AssetAdministrationShell: Circuit Breaker: A circuit breaker is implemented for all calls to the AAS. If the AAS becomes un...

    deactivate AssetAdministrationShell
    deactivate OPCCoreClient
