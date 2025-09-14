# This Dockerfile is optimized for local development and debugging with Visual Studio's "Fast Mode".
# For production, use the multi-stage Dockerfile in the 'docker' directory.

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["OpcCoreClient.sln", "."]
COPY ["src/OpcCoreClient/OpcCoreClient.csproj", "src/OpcCoreClient/"]
RUN dotnet restore "src/OpcCoreClient/OpcCoreClient.csproj"
COPY . .
WORKDIR "/src/src/OpcCoreClient"
RUN dotnet build "OpcCoreClient.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "OpcCoreClient.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpcCoreClient.dll"]