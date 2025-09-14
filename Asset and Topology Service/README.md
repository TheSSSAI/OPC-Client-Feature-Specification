# Asset and Topology Service (REPO-SVC-AST)

## Overview

This microservice is the central authority for managing the physical and logical structure of the industrial environment for the OPC Platform. It is responsible for implementing the Asset Management module, allowing users to build an ISA-95 compatible hierarchical representation of their plant (e.g., Site > Area > Line > Machine).

This service manages:
- The creation and lifecycle of assets in a hierarchical structure.
- The creation and management of reusable Asset Templates.
- The mapping of raw OPC tags to specific assets and their properties.
- Asynchronous bulk import of asset hierarchies and tag configurations from files.

The contextualization of data provided by this service is fundamental for all higher-level features, including analysis, AI model assignment, and AR visualization.

## Technology Stack

- **Framework**: .NET 8, ASP.NET Core 8.0
- **Language**: C# 12
- **Data Persistence**: PostgreSQL 16
- **Caching**: Redis 7
- **Database Access**: Entity Framework Core 8.0
- **Containerization**: Docker

## Architecture

This service follows the **Clean Architecture** principles, separating concerns into four main projects:

- `AssetTopology.Domain`: Contains core domain entities, value objects, and business rules. Has no external dependencies.
- `AssetTopology.Application`: Contains application-level logic, use cases, service interfaces, and DTOs. Depends only on the Domain layer.
- `AssetTopology.Infrastructure`: Implements the interfaces defined in the Application layer. Contains all external concerns like database access (EF Core), caching (Redis), and file storage.
- `AssetTopology.Api`: Exposes the application's functionality via a RESTful API using ASP.NET Core Minimal APIs.

### Key Architectural Patterns

- **Repository Pattern**: Abstracts data persistence logic.
- **Unit of Work**: Manages transactions to ensure data consistency.
- **Cache-Aside Pattern**: Improves read performance for the asset hierarchy by using a distributed Redis cache.
- **Asynchronous Background Worker**: Handles long-running bulk import jobs without blocking the API.

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker and Docker Compose
- A text editor or IDE (e.g., VS Code, Visual Studio, Rider)

### Running Locally with Docker Compose

This is the recommended way to run the service and its dependencies for local development.

1.  **Configure Keycloak Realm**: Ensure you have a `realm-export.json` file in `./config/keycloak/` or Keycloak will start with a default configuration.
2.  **Build and Run**: From the root directory of the solution, run:
    ```bash
    docker-compose up --build
    ```
3.  **Accessing the API**: The service will be available at `http://localhost:8081`. The Swagger UI can be accessed at `http://localhost:8081/swagger`.

### Running with .NET CLI (requires manual setup of dependencies)

1.  **Start Dependencies**: You will need to have PostgreSQL and Redis instances running and accessible from your local machine.
2.  **Update Configuration**: Modify `src/AssetTopology.Api/appsettings.Development.json` with the correct connection strings for your local database and cache.
3.  **Run Migrations**: Navigate to the `src/AssetTopology.Infrastructure` directory and run EF Core migrations against your database.
    ```bash
    cd src/AssetTopology.Infrastructure
    dotnet ef database update --context AssetTopologyDbContext
    ```
4.  **Run the API**: Navigate to the `src/AssetTopology.Api` directory and run the application.
    ```bash
    cd ../AssetTopology.Api
    dotnet run
    ```

## API Endpoints

The API is versioned and follows RESTful principles. For a complete and interactive list of endpoints, please run the application and navigate to the Swagger UI at `/swagger`.

Key endpoints include:

- `GET /api/v1/assets`: Retrieves the full asset hierarchy for the tenant.
- `POST /api/v1/assets`: Creates a new asset.
- `POST /api/v1/assets/{assetId}/tags`: Maps an OPC tag to an asset.
- `GET /api/v1/asset-templates`: Lists all asset templates.
- `POST /api/v1/import/assets`: Initiates a bulk import of assets from a file.

## Testing

To run the unit tests for this service:

1.  Navigate to the root directory of the solution.
2.  Run the following command:
    ```bash
    dotnet test
    ```