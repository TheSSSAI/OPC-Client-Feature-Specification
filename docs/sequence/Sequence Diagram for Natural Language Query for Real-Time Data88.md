sequenceDiagram
    participant "Frontend SPA" as FrontendSPA
    participant "API Gateway" as APIGateway
    participant "NLQ Service" as NLQService
    participant "AWS Transcribe" as AWSTranscribe
    participant "AWS Comprehend" as AWSComprehend
    participant "Asset Service" as AssetService
    participant "Query Service" as QueryService
    participant "Audit Service" as AuditService

    activate APIGateway
    FrontendSPA->>APIGateway: 1. POST /api/v1/nlq/voice (Audio Blob)
    APIGateway-->>FrontendSPA: 200 OK { value: 'The temperature of Reactor 3 is 350.5 C.' }
    activate NLQService
    APIGateway->>NLQService: 2. Forward POST /voice
    NLQService-->>APIGateway: Forward 200 OK
    NLQService->>AWSTranscribe: 3. StartTranscriptionJob(audioData)
    AWSTranscribe-->>NLQService: TranscriptionJobResult { transcript: 'What is the temperature of Reactor 3?' }
    NLQService->>AWSComprehend: 4. DetectEntities(text)
    AWSComprehend-->>NLQService: DetectEntitiesResult { entities: [...] }
    NLQService->>AssetService: 5. ResolveAssetTag(assetName: 'Reactor 3', metric: 'temperature')
    AssetService-->>NLQService: ResolveAssetTagResponse { opcTagId: 'ns=2;s=Reactor3.Temperature' }
    NLQService->>QueryService: 6. GetRealTimeValue(tagId: 'ns=2;...')
    QueryService-->>NLQService: GetRealTimeValueResponse { value: 350.5, quality: 'Good', timestamp: ... }
    NLQService->>AuditService: 7. LogAction(details)

    note over NLQService: The NLQ Service is the core orchestrator. Its primary responsibility is to translate the unstruct...
    note over AssetService: Failure to map entities in Step 5 is a critical business logic failure. The service must return a...

    deactivate NLQService
    deactivate APIGateway
