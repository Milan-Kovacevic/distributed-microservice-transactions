FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChoreographySaga.Microservices/ChoreographySaga.StocksService/ChoreographySaga.StocksService.csproj", "ChoreographySaga.Microservices/ChoreographySaga.StocksService/"]
RUN dotnet restore "ChoreographySaga.Microservices/ChoreographySaga.StocksService/ChoreographySaga.StocksService.csproj"
COPY . .
WORKDIR "/src/ChoreographySaga.Microservices/ChoreographySaga.StocksService"
RUN dotnet build "ChoreographySaga.StocksService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChoreographySaga.StocksService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChoreographySaga.StocksService.dll"]
