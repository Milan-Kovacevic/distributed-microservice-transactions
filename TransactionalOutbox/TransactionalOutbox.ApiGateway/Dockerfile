FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TransactionalOutbox.ApiGateway/TransactionalOutbox.ApiGateway.csproj", "TransactionalOutbox.ApiGateway/"]
RUN dotnet restore "TransactionalOutbox.ApiGateway/TransactionalOutbox.ApiGateway.csproj"
COPY . .
WORKDIR "/src/TransactionalOutbox.ApiGateway"
RUN dotnet build "TransactionalOutbox.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TransactionalOutbox.ApiGateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TransactionalOutbox.ApiGateway.dll"]
