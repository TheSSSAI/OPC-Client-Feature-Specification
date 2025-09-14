# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files and restore dependencies
# Copying .sln and .csproj files separately allows for caching the restore layer
COPY ["DataIngestionService.sln", "."]
COPY ["src/DataIngestionService.Api/DataIngestionService.Api.csproj", "src/DataIngestionService.Api/"]
COPY ["src/DataIngestionService.Application/DataIngestionService.Application.csproj", "src/DataIngestionService.Application/"]
COPY ["src/DataIngestionService.Infrastructure/DataIngestionService.Infrastructure.csproj", "src/DataIngestionService.Infrastructure/"]
COPY ["src/DataIngestionService.Protos/DataIngestionService.Protos.csproj", "src/DataIngestionService.Protos/"]
COPY ["nuget.config", "."]

RUN dotnet restore "DataIngestionService.sln"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/src/DataIngestionService.Api"

# Publish the application
RUN dotnet publish "DataIngestionService.Api.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Create the final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user and group for security
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Copy certificates into the container
# Certificates will be mounted from a volume in production (e.g., via Kubernetes secrets)
# This is for local development convenience.
COPY certs/ ./certs/
RUN chown -R appuser:appgroup ./certs && chmod -R 500 ./certs

# Set permissions for the non-root user
USER appuser

EXPOSE 8080
EXPOSE 8081

# Set the entry point for the container
ENTRYPOINT ["dotnet", "DataIngestionService.Api.dll"]