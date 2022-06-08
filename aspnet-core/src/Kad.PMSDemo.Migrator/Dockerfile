#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Kad.PMSDemo.Migrator/Kad.PMSDemo.Migrator.csproj", "src/Kad.PMSDemo.Migrator/"]
COPY ["src/Kad.PMSDemo.EntityFrameworkCore/Kad.PMSDemo.EntityFrameworkCore.csproj", "src/Kad.PMSDemo.EntityFrameworkCore/"]
COPY ["src/Kad.PMSDemo.Core/Kad.PMSDemo.Core.csproj", "src/Kad.PMSDemo.Core/"]
COPY ["src/Kad.PMSDemo.Core.Shared/Kad.PMSDemo.Core.Shared.csproj", "src/Kad.PMSDemo.Core.Shared/"]
RUN dotnet restore "src/Kad.PMSDemo.Migrator/Kad.PMSDemo.Migrator.csproj"
COPY . .
WORKDIR "/src/src/Kad.PMSDemo.Migrator"
RUN dotnet build "Kad.PMSDemo.Migrator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kad.PMSDemo.Migrator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kad.PMSDemo.Migrator.dll"]
