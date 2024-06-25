FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChoreographySaga.Microservices/ChoreographySaga.PaymentService/ChoreographySaga.PaymentService.csproj", "ChoreographySaga.Microservices/ChoreographySaga.PaymentService/"]
RUN dotnet restore "ChoreographySaga.Microservices/ChoreographySaga.PaymentService/ChoreographySaga.PaymentService.csproj"
COPY . .
WORKDIR "/src/ChoreographySaga.Microservices/ChoreographySaga.PaymentService"
RUN dotnet build "ChoreographySaga.PaymentService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChoreographySaga.PaymentService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChoreographySaga.PaymentService.dll"]
