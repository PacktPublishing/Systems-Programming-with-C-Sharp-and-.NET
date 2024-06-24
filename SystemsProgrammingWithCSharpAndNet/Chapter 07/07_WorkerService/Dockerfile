#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["07_WorkerService/07_WorkerService.csproj", "07_WorkerService/"]
COPY ["../../Chapter 04/TheOneWithTheThreadTangles/ExtensionLibrary/ExtensionLibrary.csproj", "../../Chapter 04/TheOneWithTheThreadTangles/ExtensionLibrary/"]
RUN dotnet restore "./07_WorkerService/./07_WorkerService.csproj"
COPY . .
WORKDIR "/src/07_WorkerService"
RUN dotnet build "./07_WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./07_WorkerService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "07_WorkerService.dll"]