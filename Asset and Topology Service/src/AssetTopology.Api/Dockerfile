# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files to restore dependencies
COPY ["AssetTopology.sln", "./"]
COPY ["src/AssetTopology.Api/AssetTopology.Api.csproj", "src/AssetTopology.Api/"]
COPY ["src/AssetTopology.Application/AssetTopology.Application.csproj", "src/AssetTopology.Application/"]
COPY ["src/AssetTopology.Domain/AssetTopology.Domain.csproj", "src/AssetTopology.Domain/"]
COPY ["src/AssetTopology.Infrastructure/AssetTopology.Infrastructure.csproj", "src/AssetTopology.Infrastructure/"]
COPY ["Directory.Build.props", "./"]
COPY ["nuget.config", "./"]

# Restore dependencies for all projects
RUN dotnet restore "AssetTopology.sln"

# Copy the entire source code
COPY . .
WORKDIR "/src/src/AssetTopology.Api"

# Build and publish the release version of the API project
RUN dotnet build "AssetTopology.Api.csproj" -c Release -o /app/build
RUN dotnet publish "AssetTopology.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user and group
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser
USER appuser

# Copy the published application from the build stage
COPY --from=build /app/publish .

# Expose the port the application will run on
EXPOSE 8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "System.Services.AssetTopology.Api.dll"]