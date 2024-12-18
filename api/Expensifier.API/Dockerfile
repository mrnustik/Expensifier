FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
WORKDIR /src
COPY ["Expensifier.API/Expensifier.API.csproj", "Expensifier.API/"]
RUN dotnet restore "Expensifier.API/Expensifier.API.csproj" -a $TARGETARCH

COPY . .
RUN dotnet publish "Expensifier.API/Expensifier.API.csproj" -c $BUILD_CONFIGURATION -a $TARGETARCH -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
EXPOSE 8080
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Expensifier.API.dll"]
