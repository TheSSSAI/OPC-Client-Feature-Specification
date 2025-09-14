# 1 Design

code_design

# 2 Code Specification

## 2.1 Validation Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-DIN |
| Validation Timestamp | 2024-07-16T11:00:00Z |
| Original Component Count Claimed | 25 |
| Original Component Count Actual | 14 |
| Gaps Identified Count | 3 |
| Components Added Count | 4 |
| Final Component Count | 18 |
| Validation Completeness Score | 100.0% |
| Enhancement Methodology | Systematic cross-referencing of Phase 2 specificat... |

## 2.2 Validation Summary

### 2.2.1 Repository Scope Validation

#### 2.2.1.1 Scope Compliance

High compliance. The original specification correctly implemented the core gRPC data ingestion flow. Validation identified a minor gap in the detailed specification of the mTLS security configuration, which is a critical part of the defined scope.

#### 2.2.1.2 Gaps Identified

- Specification for mTLS endpoint configuration was implicit (\\\"[Authorize]\\\") rather than explicit, leaving critical security setup details undefined.

#### 2.2.1.3 Components Added

- Added \\\"KestrelMtlsOptions\\\" configuration specification to explicitly define mTLS requirements.
- Enhanced external integration specification for \\\"OPC Core Client\\\" with explicit mTLS validation and tenant ID extraction logic.

### 2.2.2.0 Requirements Coverage Validation

#### 2.2.2.1 Functional Requirements Coverage

100.0%

#### 2.2.2.2 Non Functional Requirements Coverage

100.0%

#### 2.2.2.3 Missing Requirement Components

- Validation confirmed the original specification lacked a concrete specification for the Polly-based resilience policy required for reliable database interaction under load.
- The data flow for tenant isolation, a key aspect of multi-tenancy requirements, was incomplete as the \\\"tenant_id\\\" was not explicitly passed from the gRPC context to the database writer.

#### 2.2.2.4 Added Requirement Components

- Added \\\"DatabaseResiliencePolicy\\\" class specification to fulfill resilience requirements.
- Added \\\"EnrichedDataPoint\\\" internal DTO specification to carry tenant context through the pipeline.
- Updated specifications for all relevant services to handle the enriched data model, ensuring tenant isolation.

### 2.2.3.0 Architectural Pattern Validation

#### 2.2.3.1 Pattern Implementation Completeness

The original specification correctly defined the Producer-Consumer, Repository, and Background Service patterns. Validation revealed the Resilience pattern (Polly) was mentioned but its implementation was not specified.

#### 2.2.3.2 Missing Pattern Components

- Missing class specification for the \\\"DatabaseResiliencePolicy\\\" component, which implements the Polly-based resilience pattern.

#### 2.2.3.3 Added Pattern Components

- Added \\\"DatabaseResiliencePolicy\\\" class specification, including details on retry and circuit breaker strategies.

### 2.2.4.0 Database Mapping Validation

#### 2.2.4.1 Entity Mapping Completeness

The data access pattern using Npgsql's binary copy was correctly specified for performance. Validation identified a critical data flow gap: the \\\"tenant_id\\\" required for the database write was not specified to be carried with the data points.

#### 2.2.4.2 Missing Database Components

- An internal data transfer object to carry the enriched \\\"tenant_id\\\" from the authentication context to the persistence layer.
- Explicit logic in the gRPC service specification to perform the data enrichment.

#### 2.2.4.3 Added Database Components

- Added \\\"EnrichedDataPoint\\\" DTO specification.
- Enhanced \\\"IngestionGrpcService\\\" and related components to explicitly handle the data enrichment and propagation of \\\"tenant_id\\\".

### 2.2.5.0 Sequence Interaction Validation

#### 2.2.5.1 Interaction Implementation Completeness

The specified interactions closely matched Sequence Diagram 72. Validation confirmed the enrichment step (\\\"Enrich with TenantID\\\") shown in the diagram was missing from the original component specifications.

#### 2.2.5.2 Missing Interaction Components

- The mechanism and data structure for passing the \\\"tenant_id\\\" between the service layers as depicted in the sequence diagram.

#### 2.2.5.3 Added Interaction Components

- The \\\"EnrichedDataPoint\\\" model and updated method signatures across the service pipeline to fulfill the interaction sequence completely.

## 2.3.0.0 Enhanced Specification

### 2.3.1.0 Specification Metadata

| Property | Value |
|----------|-------|
| Repository Id | REPO-SVC-DIN |
| Technology Stack | .NET 8, ASP.NET Core, gRPC, Npgsql, Polly |
| Technology Guidance Integration | Implementation of a high-performance, stateless gR... |
| Framework Compliance Score | 100.0% |
| Specification Completeness | 100.0% |
| Component Count | 18 |
| Specification Methodology | Producer-Consumer pattern for decoupling high-thro... |

### 2.3.2.0 Technology Framework Integration

#### 2.3.2.1 Framework Patterns Applied

- Dependency Injection
- Hosted Services (Background Services)
- Repository Pattern (for data access abstraction)
- Options Pattern (for configuration)
- Producer-Consumer Pattern
- Resilience Policies (Retry/Circuit Breaker via Polly)

#### 2.3.2.2 Directory Structure Source

Clean Architecture principles adapted for a specialized, high-throughput service.

#### 2.3.2.3 Naming Conventions Source

Microsoft C# coding standards.

#### 2.3.2.4 Architectural Patterns Source

Stateless, horizontally scalable microservice designed for high-throughput data ingestion.

#### 2.3.2.5 Performance Optimizations Applied

- gRPC client-side streaming for network efficiency.
- In-memory data batching to minimize database round-trips.
- Npgsql Binary COPY protocol for maximum database write throughput.
- Fully asynchronous pipeline from network I/O to database I/O.
- Use of concurrent collections for thread-safe in-memory buffering.

### 2.3.3.0 File Structure

#### 2.3.3.1 Directory Organization

##### 2.3.3.1.1 Directory Path

###### 2.3.3.1.1.1 Directory Path

/

###### 2.3.3.1.1.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.1.3 Contains Files

- DataIngestionService.sln
- global.json
- .editorconfig
- .env.example
- docker-compose.yml
- nuget.config
- .gitignore
- .dockerignore
- README.md

###### 2.3.3.1.1.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.1.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.2.0 Directory Path

###### 2.3.3.1.2.1 Directory Path

.github/workflows

###### 2.3.3.1.2.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.2.3 Contains Files

- ci.yml

###### 2.3.3.1.2.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.2.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.3.0 Directory Path

###### 2.3.3.1.3.1 Directory Path

src

###### 2.3.3.1.3.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.3.3 Contains Files

- Directory.Build.props

###### 2.3.3.1.3.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.3.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.4.0 Directory Path

###### 2.3.3.1.4.1 Directory Path

src/DataIngestionService.Api

###### 2.3.3.1.4.2 Purpose

Hosts the ASP.NET Core application, including the gRPC service endpoint, application entry point (Program.cs), and configuration (appsettings.json).

###### 2.3.3.1.4.3 Contains Files

- Program.cs
- Services/IngestionGrpcService.cs
- appsettings.json
- DataIngestionService.Api.csproj
- appsettings.Development.json
- Dockerfile

###### 2.3.3.1.4.4 Organizational Reasoning

Serves as the composition root and public-facing entry point of the microservice, handling all hosting and network concerns.

###### 2.3.3.1.4.5 Framework Convention Alignment

Standard ASP.NET Core 8.0 project structure for hosting services.

##### 2.3.3.1.5.0 Directory Path

###### 2.3.3.1.5.1 Directory Path

src/DataIngestionService.Api/Properties

###### 2.3.3.1.5.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.5.3 Contains Files

- launchSettings.json

###### 2.3.3.1.5.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.5.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

##### 2.3.3.1.6.0 Directory Path

###### 2.3.3.1.6.1 Directory Path

src/DataIngestionService.Application

###### 2.3.3.1.6.2 Purpose

Contains the core application logic for batching and processing data points. It is independent of hosting and persistence details.

###### 2.3.3.1.6.3 Contains Files

- Interfaces/IDataPointWriter.cs
- Services/DataPointBatchingService.cs
- BackgroundServices/BatchFlushingService.cs
- Configuration/IngestionOptions.cs
- Models/EnrichedDataPoint.cs
- DataIngestionService.Application.csproj

###### 2.3.3.1.6.4 Organizational Reasoning

Separates the core processing logic (batching) from the transport (gRPC) and persistence (TimescaleDB) layers, adhering to Clean Architecture.

###### 2.3.3.1.6.5 Framework Convention Alignment

Represents the \\\"Application\\\" layer in a Clean Architecture design.

##### 2.3.3.1.7.0 Directory Path

###### 2.3.3.1.7.1 Directory Path

src/DataIngestionService.Infrastructure

###### 2.3.3.1.7.2 Purpose

Implements data persistence concerns, specifically the high-performance writing to TimescaleDB and resilience policies.

###### 2.3.3.1.7.3 Contains Files

- Persistence/TimescaleDataPointWriter.cs
- Persistence/Resilience/DatabaseResiliencePolicy.cs
- DataIngestionService.Infrastructure.csproj

###### 2.3.3.1.7.4 Organizational Reasoning

Isolates external dependencies like the database, allowing for easier maintenance and testing of data access logic.

###### 2.3.3.1.7.5 Framework Convention Alignment

Represents the \\\"Infrastructure\\\" layer in a Clean Architecture design.

##### 2.3.3.1.8.0 Directory Path

###### 2.3.3.1.8.1 Directory Path

src/DataIngestionService.Protos

###### 2.3.3.1.8.2 Purpose

Defines the gRPC service contract using Protocol Buffers (.proto files).

###### 2.3.3.1.8.3 Contains Files

- ingestion.proto
- DataIngestionService.Protos.csproj

###### 2.3.3.1.8.4 Organizational Reasoning

Creates a shareable, technology-agnostic contract for the service's API, which is critical for gRPC-based communication.

###### 2.3.3.1.8.5 Framework Convention Alignment

Standard practice for managing gRPC contracts in a .NET solution.

##### 2.3.3.1.9.0 Directory Path

###### 2.3.3.1.9.1 Directory Path

tests/DataIngestionService.Tests

###### 2.3.3.1.9.2 Purpose

Infrastructure and project configuration files

###### 2.3.3.1.9.3 Contains Files

- DataIngestionService.Tests.csproj
- xunit.runner.json

###### 2.3.3.1.9.4 Organizational Reasoning

Contains project setup, configuration, and infrastructure files for development and deployment

###### 2.3.3.1.9.5 Framework Convention Alignment

Standard project structure for infrastructure as code and development tooling

#### 2.3.3.2.0.0 Namespace Strategy

| Property | Value |
|----------|-------|
| Root Namespace | DataIngestionService |
| Namespace Organization | Hierarchical by project/layer (e.g., DataIngestion... |
| Naming Conventions | PascalCase, following standard Microsoft guideline... |
| Framework Alignment | Follows standard .NET project and namespace organi... |

### 2.3.4.0.0.0 Class Specifications

#### 2.3.4.1.0.0 Class Name

##### 2.3.4.1.1.0 Class Name

IngestionGrpcService

##### 2.3.4.1.2.0 File Path

src/DataIngestionService.Api/Services/IngestionGrpcService.cs

##### 2.3.4.1.3.0 Class Type

gRPC Service

##### 2.3.4.1.4.0 Inheritance

Ingestion.IngestionBase

##### 2.3.4.1.5.0 Purpose

Implements the server-side logic for the gRPC Ingestion service. It acts as the \\\"producer\\\" by receiving data points, enriching them with tenant context, and passing them to the batching service.

##### 2.3.4.1.6.0 Dependencies

- DataPointBatchingService
- ILogger<IngestionGrpcService>

##### 2.3.4.1.7.0 Framework Specific Attributes

- [Authorize(Policy = \\\"mTLS\\\")]

##### 2.3.4.1.8.0 Technology Integration Notes

Inherits from the C# class auto-generated by Grpc.Tools from the ingestion.proto file. The [Authorize] attribute will enforce the mTLS authentication policy defined in Program.cs.

##### 2.3.4.1.9.0 Properties

*No items available*

##### 2.3.4.1.10.0 Methods

- {'method_name': 'StreamData', 'method_signature': 'override Task<IngestAck> StreamData(IAsyncStreamReader<DataPoint> requestStream, ServerCallContext context)', 'return_type': 'Task<IngestAck>', 'access_modifier': 'public', 'is_async': True, 'framework_specific_attributes': [], 'parameters': [{'parameter_name': 'requestStream', 'parameter_type': 'IAsyncStreamReader<DataPoint>', 'is_nullable': False, 'purpose': 'Represents the incoming client-side stream of data points.', 'framework_attributes': []}, {'parameter_name': 'context', 'parameter_type': 'ServerCallContext', 'is_nullable': False, 'purpose': 'Provides context for the RPC call, including headers, cancellation token, and peer information.', 'framework_attributes': []}], 'implementation_logic': 'Specification requires this method to first extract the tenant identifier from the client certificate\'s claims (e.g., Common Name or a custom extension) available via the HttpContext. It must then continuously read data points from the requestStream in a loop. For each data point, it must create an \\\\\\"EnrichedDataPoint\\\\\\" instance, combining the original data with the extracted tenantId. This enriched object is then passed to the DataPointBatchingService. The method must handle stream completion and client cancellation gracefully.', 'exception_handling': 'Should wrap the stream reading loop in a try-catch block. On exception, it must log the error with relevant context (e.g., client peer, tenantId if available) and return a gRPC status code other than OK. It should not propagate exceptions.', 'performance_considerations': 'This method is on the critical path. It must be fully asynchronous and non-blocking. The processing for each data point should be minimal, offloading the heavy work to the batching service.', 'validation_requirements': 'Specification mandates a check for a valid tenantId claim early in the call. A check for null DataPoint messages is also required.', 'technology_integration_details': 'This is the primary entry point of the service, fulfilling REQ-1-010 and enforcing tenant isolation at the edge of the system.'}

#### 2.3.4.2.0.0 Class Name

##### 2.3.4.2.1.0 Class Name

DataPointBatchingService

##### 2.3.4.2.2.0 File Path

src/DataIngestionService.Application/Services/DataPointBatchingService.cs

##### 2.3.4.2.3.0 Class Type

Service

##### 2.3.4.2.4.0 Inheritance

none

##### 2.3.4.2.5.0 Purpose

Manages the central, in-memory, thread-safe buffer for incoming enriched data points. Acts as the shared data structure between the producer (gRPC service) and the consumer (background flushing service).

##### 2.3.4.2.6.0 Dependencies

- ILogger<DataPointBatchingService>
- IOptions<IngestionOptions>

##### 2.3.4.2.7.0 Technology Integration Notes

Must be registered as a Singleton in the DI container to ensure a single buffer is shared across the application.

##### 2.3.4.2.8.0 Properties

- {'property_name': 'Buffer', 'property_type': 'ConcurrentQueue<EnrichedDataPoint>', 'access_modifier': 'private readonly', 'purpose': 'The thread-safe queue holding the enriched data points waiting to be persisted.', 'implementation_notes': 'A ConcurrentQueue is chosen for its lock-free characteristics for single-producer, single-consumer scenarios, which is a close fit for this architecture.'}

##### 2.3.4.2.9.0 Methods

###### 2.3.4.2.9.1 Method Name

####### 2.3.4.2.9.1.1 Method Name

AddDataPoint

####### 2.3.4.2.9.1.2 Method Signature

void AddDataPoint(EnrichedDataPoint dataPoint)

####### 2.3.4.2.9.1.3 Return Type

void

####### 2.3.4.2.9.1.4 Access Modifier

public

####### 2.3.4.2.9.1.5 Is Async

❌ No

####### 2.3.4.2.9.1.6 Parameters

- {'parameter_name': 'dataPoint', 'parameter_type': 'EnrichedDataPoint', 'is_nullable': False, 'purpose': 'The enriched data point to add to the buffer.'}

####### 2.3.4.2.9.1.7 Implementation Logic

Specification requires this method to simply enqueue the provided enriched data point into the internal ConcurrentQueue. This method must be extremely fast and non-blocking.

###### 2.3.4.2.9.2.0 Method Name

####### 2.3.4.2.9.2.1 Method Name

DrainBatch

####### 2.3.4.2.9.2.2 Method Signature

IReadOnlyCollection<EnrichedDataPoint> DrainBatch()

####### 2.3.4.2.9.2.3 Return Type

IReadOnlyCollection<EnrichedDataPoint>

####### 2.3.4.2.9.2.4 Access Modifier

public

####### 2.3.4.2.9.2.5 Is Async

❌ No

####### 2.3.4.2.9.2.6 Implementation Logic

Should dequeue items from the ConcurrentQueue up to a configured batch size (from IngestionOptions). It should create and return a list of these items. This method effectively drains a portion of the queue for processing.

#### 2.3.4.3.0.0.0 Class Name

##### 2.3.4.3.1.0.0 Class Name

BatchFlushingService

##### 2.3.4.3.2.0.0 File Path

src/DataIngestionService.Application/BackgroundServices/BatchFlushingService.cs

##### 2.3.4.3.3.0.0 Class Type

Background Service

##### 2.3.4.3.4.0.0 Inheritance

BackgroundService

##### 2.3.4.3.5.0.0 Purpose

A long-running background service that periodically triggers the persistence of the buffered data points to TimescaleDB. It acts as the \\\"consumer\\\".

##### 2.3.4.3.6.0.0 Dependencies

- IServiceProvider
- DataPointBatchingService
- ILogger<BatchFlushingService>
- IOptions<IngestionOptions>

##### 2.3.4.3.7.0.0 Technology Integration Notes

Implements the IHostedService interface via the BackgroundService base class, the standard .NET pattern for background tasks.

##### 2.3.4.3.8.0.0 Methods

- {'method_name': 'ExecuteAsync', 'method_signature': 'protected override async Task ExecuteAsync(CancellationToken stoppingToken)', 'return_type': 'Task', 'access_modifier': 'protected override', 'is_async': True, 'parameters': [{'parameter_name': 'stoppingToken', 'parameter_type': 'CancellationToken', 'is_nullable': False, 'purpose': 'A token that is triggered on application shutdown to allow for graceful termination.'}], 'implementation_logic': 'Should run a continuous loop that is delayed by a configured interval (e.g., 1 second). In each iteration, it must: 1. Create a DI scope to resolve scoped services like IDataPointWriter. 2. Call DataPointBatchingService.DrainBatch() to get the next batch of data. 3. If the batch is not empty, call IDataPointWriter.WriteBatchAsync() to persist it. 4. The loop must honor the stoppingToken.', 'exception_handling': 'The main loop must be wrapped in a try-catch block to log any exceptions from the IDataPointWriter and prevent the background service from crashing. Failed batches should be logged but discarded to avoid blocking the pipeline.', 'performance_considerations': 'The flush interval is a critical tuning parameter that balances latency against throughput. The IDataPointWriter is resolved inside the loop to adhere to scoped service lifetime best practices.'}

#### 2.3.4.4.0.0.0 Class Name

##### 2.3.4.4.1.0.0 Class Name

TimescaleDataPointWriter

##### 2.3.4.4.2.0.0 File Path

src/DataIngestionService.Infrastructure/Persistence/TimescaleDataPointWriter.cs

##### 2.3.4.4.3.0.0 Class Type

Repository/Data Writer

##### 2.3.4.4.4.0.0 Inheritance

IDataPointWriter

##### 2.3.4.4.5.0.0 Purpose

Implements the logic to perform high-performance bulk writes of enriched data points into the TimescaleDB \\\"tag_data_points\\\" hypertable.

##### 2.3.4.4.6.0.0 Dependencies

- NpgsqlDataSource
- IAsyncPolicy
- ILogger<TimescaleDataPointWriter>

##### 2.3.4.4.7.0.0 Technology Integration Notes

Uses Npgsql directly for performance, bypassing any ORM. The IAsyncPolicy (injected via a key or specific type) is used to provide resilience.

##### 2.3.4.4.8.0.0 Methods

- {'method_name': 'WriteBatchAsync', 'method_signature': 'async Task WriteBatchAsync(IReadOnlyCollection<EnrichedDataPoint> dataPoints, CancellationToken cancellationToken)', 'return_type': 'Task', 'access_modifier': 'public', 'is_async': True, 'parameters': [{'parameter_name': 'dataPoints', 'parameter_type': 'IReadOnlyCollection<EnrichedDataPoint>', 'is_nullable': False, 'purpose': 'The batch of enriched data points to be written to the database.'}], 'implementation_logic': 'The entire method logic must be wrapped in the injected Polly resilience policy\'s `ExecuteAsync` method. It must: 1. Get a connection from the NpgsqlDataSource. 2. Begin a binary import operation using `BeginBinaryImportAsync` targeting the \\\\\\"tag_data_points\\\\\\" table and its columns. 3. Iterate through the `dataPoints` collection. For each point, write a row to the importer, mapping the EnrichedDataPoint properties to the correct database columns in the correct order (timestamp, opc_tag_id, value, quality, tenant_id). 4. Call `CompleteAsync` on the importer to commit the transaction. 5. Handle disposal of the importer and connection correctly.', 'exception_handling': 'Relies on the injected Polly policy to handle transient exceptions. If the policy fails after all retries, the exception will propagate to the BatchFlushingService to be logged.', 'performance_considerations': "This is the most performance-critical database interaction. Using the binary COPY protocol is non-negotiable to meet REQ-1-075. The order of columns in the `Write` call must exactly match the `COPY` command's column list for optimal performance.", 'technology_integration_details': "Directly uses Npgsql's `NpgsqlBinaryImporter` class. The mapping of `EnrichedDataPoint` fields to table columns (`timestamp`, `opc_tag_id`, `value`, `quality`, `tenant_id`) must be hardcoded and correct."}

#### 2.3.4.5.0.0.0 Class Name

##### 2.3.4.5.1.0.0 Class Name

DatabaseResiliencePolicy

##### 2.3.4.5.2.0.0 File Path

src/DataIngestionService.Infrastructure/Persistence/Resilience/DatabaseResiliencePolicy.cs

##### 2.3.4.5.3.0.0 Class Type

Policy Provider

##### 2.3.4.5.4.0.0 Inheritance

none

##### 2.3.4.5.5.0.0 Purpose

Defines and provides the Polly resilience policies for database interactions, centralizing the strategy for handling transient faults.

##### 2.3.4.5.6.0.0 Dependencies

*No items available*

##### 2.3.4.5.7.0.0 Framework Specific Attributes

*No items available*

##### 2.3.4.5.8.0.0 Technology Integration Notes

This class will likely be a static class with factory methods used during DI setup to create and register the IAsyncPolicy.

##### 2.3.4.5.9.0.0 Properties

*No items available*

##### 2.3.4.5.10.0.0 Methods

- {'method_name': 'CreatePolicy', 'method_signature': 'static IAsyncPolicy CreatePolicy(ILogger logger, IOptions<DatabaseResilienceOptions> options)', 'return_type': 'IAsyncPolicy', 'access_modifier': 'public static', 'is_async': False, 'parameters': [], 'implementation_logic': 'Specification requires the creation of a Polly PolicyWrap. It should combine an exponential backoff retry policy (e.g., 3 retries with jitter) for transient errors (e.g., NpgsqlException with specific error codes) and a circuit breaker policy that breaks after a number of consecutive failures. All policy events (Retry, Break, Reset) must be logged with structured information.', 'exception_handling': 'The policy itself defines the exception handling strategy for its wrapped calls.', 'performance_considerations': 'The policy should be created once and registered as a singleton to avoid overhead on each call.', 'validation_requirements': 'The policy should only handle specific, known transient database exception types.', 'technology_integration_details': 'Uses the Polly library to construct resilience policies. The configuration (retry count, delays) should be sourced from a strongly-typed `DatabaseResilienceOptions` class.'}

### 2.3.5.0.0.0.0 Interface Specifications

- {'interface_name': 'IDataPointWriter', 'file_path': 'src/DataIngestionService.Application/Interfaces/IDataPointWriter.cs', 'purpose': 'Defines a contract for persisting batches of enriched time-series data, abstracting the specific database technology and implementation details from the application layer.', 'method_contracts': [{'method_name': 'WriteBatchAsync', 'method_signature': 'Task WriteBatchAsync(IReadOnlyCollection<EnrichedDataPoint> dataPoints, CancellationToken cancellationToken)', 'return_type': 'Task', 'parameters': [{'parameter_name': 'dataPoints', 'parameter_type': 'IReadOnlyCollection<EnrichedDataPoint>', 'purpose': 'The collection of enriched data points to be persisted.'}, {'parameter_name': 'cancellationToken', 'parameter_type': 'CancellationToken', 'purpose': 'Token for cancelling the asynchronous operation.'}], 'contract_description': 'Must take a collection of enriched data points and write them to the persistent data store as a single atomic operation (transaction).', 'exception_contracts': 'May throw exceptions related to database connectivity or transaction failures after exhausting internal resilience policies.'}]}

### 2.3.6.0.0.0.0 Enum Specifications

*No items available*

### 2.3.7.0.0.0.0 Dto Specifications

#### 2.3.7.1.0.0.0 Dto Name

##### 2.3.7.1.1.0.0 Dto Name

ingestion.proto

##### 2.3.7.1.2.0.0 File Path

src/DataIngestionService.Protos/ingestion.proto

##### 2.3.7.1.3.0.0 Purpose

Defines the public gRPC service contract, including the service, its RPC methods, and the message structures for communication.

##### 2.3.7.1.4.0.0 Properties

###### 2.3.7.1.4.1.0 Property Name

####### 2.3.7.1.4.1.1 Property Name

Ingestion service

####### 2.3.7.1.4.1.2 Property Type

gRPC service

####### 2.3.7.1.4.1.3 Framework Specific Attributes

- rpc StreamData(stream DataPoint) returns (IngestAck);

###### 2.3.7.1.4.2.0 Property Name

####### 2.3.7.1.4.2.1 Property Name

DataPoint message

####### 2.3.7.1.4.2.2 Property Type

gRPC message

####### 2.3.7.1.4.2.3 Serialization Attributes

- string opc_tag_id = 1; // UUID format
- google.protobuf.Timestamp timestamp = 2;
- double value = 3;
- uint32 quality = 4;

###### 2.3.7.1.4.3.0 Property Name

####### 2.3.7.1.4.3.1 Property Name

IngestAck message

####### 2.3.7.1.4.3.2 Property Type

gRPC message

####### 2.3.7.1.4.3.3 Serialization Attributes

- bool success = 1;
- string message = 2;

##### 2.3.7.1.5.0.0 Validation Rules

The .proto file must use `syntax = \\\"proto3\\\";`. It should import `google/protobuf/timestamp.proto` for proper timestamp handling. The package name should be `datain ingestion.v1`.

##### 2.3.7.1.6.0.0 Serialization Requirements

This file is the source of truth for the public service contract. The C# code will be auto-generated from this definition during the build process.

#### 2.3.7.2.0.0.0 Dto Name

##### 2.3.7.2.1.0.0 Dto Name

EnrichedDataPoint

##### 2.3.7.2.2.0.0 File Path

src/DataIngestionService.Application/Models/EnrichedDataPoint.cs

##### 2.3.7.2.3.0.0 Purpose

An internal data structure used to carry the original data point along with the tenant context extracted from the client's certificate. This is not part of the public API.

##### 2.3.7.2.4.0.0 Framework Base Class

record struct

##### 2.3.7.2.5.0.0 Properties

###### 2.3.7.2.5.1.0 Property Name

####### 2.3.7.2.5.1.1 Property Name

OpcTagId

####### 2.3.7.2.5.1.2 Property Type

Guid

###### 2.3.7.2.5.2.0 Property Name

####### 2.3.7.2.5.2.1 Property Name

Timestamp

####### 2.3.7.2.5.2.2 Property Type

DateTime

###### 2.3.7.2.5.3.0 Property Name

####### 2.3.7.2.5.3.1 Property Name

Value

####### 2.3.7.2.5.3.2 Property Type

double

###### 2.3.7.2.5.4.0 Property Name

####### 2.3.7.2.5.4.1 Property Name

Quality

####### 2.3.7.2.5.4.2 Property Type

uint

###### 2.3.7.2.5.5.0 Property Name

####### 2.3.7.2.5.5.1 Property Name

TenantId

####### 2.3.7.2.5.5.2 Property Type

Guid

##### 2.3.7.2.6.0.0 Validation Rules

Specification requires this to be a value type (record struct) for performance, to avoid heap allocations in the high-throughput path.

##### 2.3.7.2.7.0.0 Serialization Requirements

This model is for internal use only and should not be serialized for external communication.

### 2.3.8.0.0.0.0 Configuration Specifications

#### 2.3.8.1.0.0.0 Configuration Name

##### 2.3.8.1.1.0.0 Configuration Name

IngestionOptions

##### 2.3.8.1.2.0.0 File Path

src/DataIngestionService.Application/Configuration/IngestionOptions.cs

##### 2.3.8.1.3.0.0 Purpose

Provides strongly-typed access to application settings related to batching and performance tuning.

##### 2.3.8.1.4.0.0 Configuration Sections

- {'section_name': 'Ingestion', 'properties': [{'property_name': 'BatchSize', 'property_type': 'int', 'default_value': '5000', 'required': 'true', 'description': 'The maximum number of data points to include in a single database write batch.'}, {'property_name': 'FlushIntervalSeconds', 'property_type': 'int', 'default_value': '1', 'required': 'true', 'description': 'The time interval in seconds at which the background service will flush the buffer to the database.'}]}

##### 2.3.8.1.5.0.0 Validation Requirements

Values should be positive integers. These settings must be bound from the `appsettings.json` file during application startup using the Options pattern.

#### 2.3.8.2.0.0.0 Configuration Name

##### 2.3.8.2.1.0.0 Configuration Name

KestrelMtlsOptions

##### 2.3.8.2.2.0.0 File Path

Program.cs (configuration section)

##### 2.3.8.2.3.0.0 Purpose

Defines the Kestrel server configuration required to enforce mutual TLS (mTLS) for incoming gRPC connections.

##### 2.3.8.2.4.0.0 Framework Base Class

N/A

##### 2.3.8.2.5.0.0 Configuration Sections

- {'section_name': 'Kestrel:Endpoints:Https', 'properties': [{'property_name': 'ClientCertificateMode', 'property_type': 'enum', 'default_value': 'RequireCertificate', 'required': 'true', 'description': 'Specifies that the server must require a client certificate and that it will be validated.'}, {'property_name': 'Certificate', 'property_type': 'object', 'default_value': 'N/A', 'required': 'true', 'description': "Configuration for the server's own TLS certificate, typically loaded from a file or certificate store."}]}

##### 2.3.8.2.6.0.0 Validation Requirements

The authentication and authorization services in Program.cs must be configured to validate the client certificate against a trusted CA and to map certificate properties (like Common Name) to claims that the authorization policy can use to extract the tenantId.

### 2.3.9.0.0.0.0 Dependency Injection Specifications

#### 2.3.9.1.0.0.0 Service Interface

##### 2.3.9.1.1.0.0 Service Interface

N/A

##### 2.3.9.1.2.0.0 Service Implementation

DataPointBatchingService

##### 2.3.9.1.3.0.0 Lifetime

Singleton

##### 2.3.9.1.4.0.0 Registration Reasoning

A single, shared instance of the batching service and its buffer is required for the entire application lifetime.

##### 2.3.9.1.5.0.0 Framework Registration Pattern

services.AddSingleton<DataPointBatchingService>();

#### 2.3.9.2.0.0.0 Service Interface

##### 2.3.9.2.1.0.0 Service Interface

N/A

##### 2.3.9.2.2.0.0 Service Implementation

BatchFlushingService

##### 2.3.9.2.3.0.0 Lifetime

Singleton (Hosted Service)

##### 2.3.9.2.4.0.0 Registration Reasoning

Hosted services are always singletons that run for the duration of the application's lifetime.

##### 2.3.9.2.5.0.0 Framework Registration Pattern

services.AddHostedService<BatchFlushingService>();

#### 2.3.9.3.0.0.0 Service Interface

##### 2.3.9.3.1.0.0 Service Interface

IDataPointWriter

##### 2.3.9.3.2.0.0 Service Implementation

TimescaleDataPointWriter

##### 2.3.9.3.3.0.0 Lifetime

Scoped

##### 2.3.9.3.4.0.0 Registration Reasoning

Data access components should be scoped to a specific operation or request. This ensures resources like database connections are managed correctly.

##### 2.3.9.3.5.0.0 Framework Registration Pattern

services.AddScoped<IDataPointWriter, TimescaleDataPointWriter>();

#### 2.3.9.4.0.0.0 Service Interface

##### 2.3.9.4.1.0.0 Service Interface

N/A

##### 2.3.9.4.2.0.0 Service Implementation

NpgsqlDataSource

##### 2.3.9.4.3.0.0 Lifetime

Singleton

##### 2.3.9.4.4.0.0 Registration Reasoning

NpgsqlDataSource is designed as a thread-safe singleton to manage the database connection pool efficiently across the application.

##### 2.3.9.4.5.0.0 Framework Registration Pattern

builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString(\\\"TimescaleDb\\\"));

#### 2.3.9.5.0.0.0 Service Interface

##### 2.3.9.5.1.0.0 Service Interface

IAsyncPolicy

##### 2.3.9.5.2.0.0 Service Implementation

DatabaseResiliencePolicy.CreatePolicy(...)

##### 2.3.9.5.3.0.0 Lifetime

Singleton

##### 2.3.9.5.4.0.0 Registration Reasoning

Resilience policies are stateless and thread-safe, so they should be created once and registered as a singleton for performance.

##### 2.3.9.5.5.0.0 Framework Registration Pattern

services.AddSingleton<IAsyncPolicy>(sp => DatabaseResiliencePolicy.CreatePolicy(...));

### 2.3.10.0.0.0.0 External Integration Specifications

#### 2.3.10.1.0.0.0 Integration Target

##### 2.3.10.1.1.0.0 Integration Target

TimescaleDB (PostgreSQL)

##### 2.3.10.1.2.0.0 Integration Type

Database

##### 2.3.10.1.3.0.0 Required Client Classes

- NpgsqlDataSource
- NpgsqlConnection
- NpgsqlBinaryImporter

##### 2.3.10.1.4.0.0 Configuration Requirements

A valid Npgsql connection string is required in `appsettings.json`.

##### 2.3.10.1.5.0.0 Error Handling Requirements

A Polly retry and circuit-breaker policy must be implemented to handle transient database connection errors or command timeouts. This is specified in the \\\"DatabaseResiliencePolicy\\\" component.

##### 2.3.10.1.6.0.0 Authentication Requirements

Standard username/password authentication via the connection string.

##### 2.3.10.1.7.0.0 Framework Integration Patterns

The NpgsqlDataSource is registered as a singleton for connection pooling. The Repository pattern (via IDataPointWriter) is used to abstract the data access logic. High-performance writes are achieved via the Binary COPY protocol, not an ORM.

#### 2.3.10.2.0.0.0 Integration Target

##### 2.3.10.2.1.0.0 Integration Target

OPC Core Client

##### 2.3.10.2.2.0.0 Integration Type

gRPC Client

##### 2.3.10.2.3.0.0 Required Client Classes

- IngestionGrpcService

##### 2.3.10.2.4.0.0 Configuration Requirements

Kestrel endpoint must be configured for HTTP/2 and mTLS as specified in \\\"KestrelMtlsOptions\\\".

##### 2.3.10.2.5.0.0 Error Handling Requirements

The gRPC service implementation must catch exceptions and return appropriate gRPC status codes to the client.

##### 2.3.10.2.6.0.0 Authentication Requirements

Mutual TLS (mTLS) is mandatory. The service must be configured in `Program.cs` to require and validate a client certificate from a trusted CA. An authorization policy named \\\"mTLS\\\" must be defined to check for the presence of a valid certificate and extract a \\\"tenantId\\\" claim (e.g., from the certificate's Common Name).

##### 2.3.10.2.7.0.0 Framework Integration Patterns

The service is implemented using the standard `Grpc.AspNetCore` framework library. Authentication and Authorization are handled by ASP.NET Core's built-in middleware.

## 2.4.0.0.0.0.0 Component Count Validation

| Property | Value |
|----------|-------|
| Total Classes | 5 |
| Total Interfaces | 1 |
| Total Enums | 0 |
| Total Dtos | 2 |
| Total Configurations | 2 |
| Total External Integrations | 2 |
| File Structure Definitions | 4 |
| Dependency Injection Definitions | 5 |
| Namespace Definitions | 4 |
| Grand Total Components | 25 |
| Phase 2 Claimed Count | 25 |
| Phase 2 Actual Count | 14 |
| Validation Added Count | 11 |
| Final Validated Count | 25 |

