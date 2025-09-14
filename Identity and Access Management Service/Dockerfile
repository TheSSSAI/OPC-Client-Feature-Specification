# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers to leverage Docker layer caching
COPY ["src/IdentityAccess.Api/IdentityAccess.Api.csproj", "src/IdentityAccess.Api/"]
COPY ["src/IdentityAccess.Application/IdentityAccess.Application.csproj", "src/IdentityAccess.Application/"]
COPY ["src/IdentityAccess.Domain/IdentityAccess.Domain.csproj", "src/IdentityAccess.Domain/"]
COPY ["src/IdentityAccess.Infrastructure/IdentityAccess.Infrastructure.csproj", "src/IdentityAccess.Infrastructure/"]
COPY ["IdentityAccess.sln", "."]

RUN dotnet restore "IdentityAccess.sln"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/src/IdentityAccess.Api"

# Build and publish the application
RUN dotnet build "IdentityAccess.Api.csproj" -c Release -o /app/build
RUN dotnet publish "IdentityAccess.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Expose the port the app runs on
EXPOSE 8080

# Define environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Set the entry point for the container
ENTRYPOINT ["dotnet", "IdentityAccess.Api.dll"]