# Data Ingestion Service

This microservice is a highly specialized, high-throughput component designed for the critical purpose of ingesting massive volumes of real-time time-series data from distributed OPC Core Clients. It is a core component of the OPC-Twin Central Management Plane.

As specified in the system requirements, it exposes a gRPC endpoint with mutual TLS (mTLS) for secure, low-latency, and efficient binary communication. Its sole responsibility is to receive data streams, perform minimal validation and enrichment, and efficiently batch-write the data into the TimescaleDB time-series database.

## Key Architectural Principles

- **High Throughput**: Architected to meet demanding performance requirements of up to **10,000 values per second per tenant**.
- **Scalability**: Designed as a stateless service to be horizontally scalable via Kubernetes, supporting up to **10,000 concurrent client instances**.
- **Security**: All communication is secured end-to-end using gRPC over mutual TLS (mTLS), ensuring only authenticated and authorized clients can stream data.
- **Resilience**: Implements resilience patterns (Retry, Circuit Breaker via Polly) for database interactions to handle transient faults gracefully.

## Project Structure

This service follows the principles of **Clean Architecture** to ensure a separation of concerns, testability, and maintainability.

- `src/DataIngestionService.Api`: The entry point of the application. An ASP.NET Core project hosting the gRPC service and handling all web-related concerns.
- `src/DataIngestionService.Application`: Contains the core application logic, including data batching and background services. It has no knowledge of web or infrastructure details.
- `src/DataIngestionService.Infrastructure`: Implements data persistence logic, specifically the high-performance bulk-writer for TimescaleDB.
- `src/DataIngestionService.Protos`: Defines the gRPC service contract (`.proto` files). This is the public API of the service.
- `tests/DataIngestionService.Tests`: Contains unit and integration tests for the service.

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/products/docker-desktop/) and Docker Compose
- A valid set of mTLS certificates (`ca.crt`, `server.pfx`, `client.pfx`). A script to generate these for development can be found in the project's DevOps repository. Place them in a `certs/` directory at the solution root.

### Running Locally with Docker Compose

1.  **Clone the repository:**
    ```bash
    git clone <repository_url>
    cd DataIngestionService
    ```

2.  **Create the environment file:**
    Copy the example environment file and update it with your settings.
    ```bash
    cp .env.example .env
    ```
    You will need to set the `KESTREL_CERTIFICATE_PASSWORD` to the password you used when creating the `server.pfx` certificate. The default database password is `changeme`.

3.  **Run the service:**
    ```bash
    docker-compose up --build
    ```
    This command will build the service's Docker image and start both the `ingestion-service` container and a `timescale-db` container.

The service will be available at:
-   **gRPC (mTLS)**: `https://localhost:8081`
-   **HTTP (Health/Metrics)**: `http://localhost:8080`

## Configuration

The application is configured via `appsettings.json` and can be overridden by environment variables.

| Key                                  | Environment Variable                        | Description                                                                 |
| ------------------------------------ | ------------------------------------------- | --------------------------------------------------------------------------- |
| `ConnectionStrings:TimescaleDb`      | `ConnectionStrings__TimescaleDb`            | The connection string for the TimescaleDB instance.                         |
| `Kestrel:Endpoints:GrpcMtls:Url`     | `ASPNETCORE_URLS` (partially)               | The URL for the gRPC mTLS endpoint.                                         |
| `Kestrel:Certificates:Default:Path`  | N/A (set in code/Dockerfile)                | Path to the server's `.pfx` certificate inside the container.               |
| `Kestrel:Certificates:Default:Password`| `KESTREL_CERTIFICATE_PASSWORD`              | Password for the server's `.pfx` certificate.                             |
| `Ingestion:BatchSize`                | `Ingestion__BatchSize`                      | Max number of data points to write in a single database transaction.        |
| `Ingestion:FlushIntervalSeconds`     | `Ingestion__FlushIntervalSeconds`           | Interval in seconds for the background service to flush the data buffer.    |