#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ForDotNet.Gateway/ForDotNet.Gateway.csproj", "ForDotNet.Gateway/"]
COPY ["ForDotNet.Auth/ForDotNet.Auth.csproj", "ForDotNet.Auth/"]
COPY ["ForDotNet.Common/ForDotNet.Common.csproj", "ForDotNet.Common/"]
RUN dotnet restore "ForDotNet.Gateway/ForDotNet.Gateway.csproj"
COPY . .
WORKDIR "/src/ForDotNet.Gateway"
RUN dotnet build "ForDotNet.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ForDotNet.Gateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ForDotNet.Gateway.dll"]