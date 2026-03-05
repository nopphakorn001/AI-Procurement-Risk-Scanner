FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ProcurementRisk.API/ProcurementRisk.API.csproj", "ProcurementRisk.API/"]
COPY ["ProcurementRisk.Application/ProcurementRisk.Application.csproj", "ProcurementRisk.Application/"]
COPY ["ProcurementRisk.Infrastructure/ProcurementRisk.Infrastructure.csproj", "ProcurementRisk.Infrastructure/"]
COPY ["ProcurementRisk.Domain/ProcurementRisk.Domain.csproj", "ProcurementRisk.Domain/"]

RUN dotnet restore "ProcurementRisk.API/ProcurementRisk.API.csproj"

COPY . .

RUN dotnet publish "ProcurementRisk.API/ProcurementRisk.API.csproj" \
    -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "ProcurementRisk.API.dll"]
