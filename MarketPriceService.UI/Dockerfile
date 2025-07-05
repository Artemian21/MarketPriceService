FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["MarketPriceService.sln", "./"]
COPY ["MarketPriceService.UI/MarketPriceService.UI.csproj", "MarketPriceService.UI/"]
COPY ["MarketPriceService.BusinessLogic/MarketPriceService.BusinessLogic.csproj", "MarketPriceService.BusinessLogic/"]
COPY ["MarketPriceService.DataAccess/MarketPriceService.DataAccess.csproj", "MarketPriceService.DataAccess/"]

RUN dotnet restore "MarketPriceService.sln"
COPY . .
RUN dotnet build "MarketPriceService.sln" -c $BUILD_CONFIGURATION

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MarketPriceService.UI/MarketPriceService.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY entrypoint.sh .
RUN chmod +x ./entrypoint.sh

ENTRYPOINT ["./entrypoint.sh"]
