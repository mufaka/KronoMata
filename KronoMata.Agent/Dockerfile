#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KronoMata.Agent/KronoMata.Agent.csproj", "KronoMata.Agent/"]
COPY ["KronoMata.Model/KronoMata.Model.csproj", "KronoMata.Model/"]
COPY ["KronoMata.Public/KronoMata.Public.csproj", "KronoMata.Public/"]
COPY ["KronoMata.Scheduling/KronoMata.Scheduling.csproj", "KronoMata.Scheduling/"]
RUN dotnet restore "KronoMata.Agent/KronoMata.Agent.csproj"
COPY . .
WORKDIR "/src/KronoMata.Agent"
RUN dotnet build "KronoMata.Agent.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KronoMata.Agent.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KronoMata.Agent.dll"]