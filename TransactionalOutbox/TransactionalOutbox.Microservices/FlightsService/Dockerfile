﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TransactionalOutbox.Microservices/FlightService/FlightService.csproj", "TransactionalOutbox.Microservices/FlightService/"]
RUN dotnet restore "TransactionalOutbox.Microservices/FlightService/FlightService.csproj"
COPY . .
WORKDIR "/src/TransactionalOutbox.Microservices/FlightService"
RUN dotnet build "FlightService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FlightService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlightService.dll"]
