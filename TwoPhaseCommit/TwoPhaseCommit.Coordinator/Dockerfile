FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TwoPhaseCommit.Coordinator/TwoPhaseCommit.Coordinator.csproj", "TwoPhaseCommit.Coordinator/"]
RUN dotnet restore "TwoPhaseCommit.Coordinator/TwoPhaseCommit.Coordinator.csproj"
COPY . .
WORKDIR "/src/TwoPhaseCommit.Coordinator"
RUN dotnet build "TwoPhaseCommit.Coordinator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TwoPhaseCommit.Coordinator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwoPhaseCommit.Coordinator.dll"]
