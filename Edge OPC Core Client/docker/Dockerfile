# This is the production-ready, multi-stage Dockerfile.
# It creates a small, secure, and optimized runtime image.

# Stage 1: Build Environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project and solution files
COPY ["OpcCoreClient.sln", "."]
COPY ["src/OpcCoreClient/OpcCoreClient.csproj", "src/OpcCoreClient/"]

# Restore dependencies
RUN dotnet restore "src/OpcCoreClient/OpcCoreClient.csproj"

# Copy the rest of the source code
COPY . .

# Build and publish the application for release
WORKDIR "/src/src/OpcCoreClient"
RUN dotnet publish "OpcCoreClient.csproj" -c Release -o /app/publish --no-restore

# Stage 2: Final Runtime Environment
FROM mcr.microsoft.com/dotnet/runtime:8.0-jammy-chiseled AS final
WORKDIR /app

# Create a non-root user for security
RUN adduser --system --group --disabled-password --no-create-home appuser

# Copy published application from the build stage
COPY --from=build /app/publish .

# Copy the update script and make it executable
COPY ["docker/update.sh", "/usr/local/bin/update.sh"]
RUN chmod +x /usr/local/bin/update.sh

# Ensure the appuser owns the application files and buffer directory
RUN mkdir -p /app/buffer && chown -R appuser:appuser /app

# Switch to the non-root user
USER appuser

# Set the entrypoint for the container
ENTRYPOINT ["dotnet", "OpcCoreClient.dll"]