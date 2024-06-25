FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChoreographySaga.OrderService/ChoreographySaga.OrderService.csproj", "ChoreographySaga.OrderService/"]
RUN dotnet restore "ChoreographySaga.OrderService/ChoreographySaga.OrderService.csproj"
COPY . .
WORKDIR "/src/ChoreographySaga.OrderService"
RUN dotnet build "ChoreographySaga.OrderService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChoreographySaga.OrderService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChoreographySaga.OrderService.dll"]
