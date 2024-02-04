FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5195

ENV ASPNETCORE_URLS=http://+:5195

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["src/SteamWorkshopStats.csproj", "src/"]
RUN dotnet restore "src/SteamWorkshopStats.csproj"
COPY . .
WORKDIR "/src/src"
RUN dotnet build "SteamWorkshopStats.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "SteamWorkshopStats.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SteamWorkshopStats.dll"]
