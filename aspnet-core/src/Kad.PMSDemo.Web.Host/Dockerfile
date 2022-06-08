#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Kad.PMSDemo.Web.Host/Kad.PMSDemo.Web.Host.csproj", "src/Kad.PMSDemo.Web.Host/"]
COPY ["src/Kad.PMSDemo.Web.Core/Kad.PMSDemo.Web.Core.csproj", "src/Kad.PMSDemo.Web.Core/"]
COPY ["src/Kad.PMSDemo.Application/Kad.PMSDemo.Application.csproj", "src/Kad.PMSDemo.Application/"]
COPY ["src/Kad.PMSDemo.Application.Shared/Kad.PMSDemo.Application.Shared.csproj", "src/Kad.PMSDemo.Application.Shared/"]
COPY ["src/Kad.PMSDemo.Core.Shared/Kad.PMSDemo.Core.Shared.csproj", "src/Kad.PMSDemo.Core.Shared/"]
COPY ["src/Kad.PMSDemo.Core/Kad.PMSDemo.Core.csproj", "src/Kad.PMSDemo.Core/"]
COPY ["src/Kad.PMSDemo.EntityFrameworkCore/Kad.PMSDemo.EntityFrameworkCore.csproj", "src/Kad.PMSDemo.EntityFrameworkCore/"]
COPY ["src/Kad.PMSDemo.GraphQL/Kad.PMSDemo.GraphQL.csproj", "src/Kad.PMSDemo.GraphQL/"]
RUN dotnet restore "src/Kad.PMSDemo.Web.Host/Kad.PMSDemo.Web.Host.csproj"
COPY . .
WORKDIR "/src/src/Kad.PMSDemo.Web.Host"
RUN dotnet build "Kad.PMSDemo.Web.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kad.PMSDemo.Web.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kad.PMSDemo.Web.Host.dll"]
