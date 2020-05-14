#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ForDotNet.Auth/ForDotNet.Auth.csproj", "ForDotNet.Auth/"]
COPY ["ForDotNet.Common/ForDotNet.Common.csproj", "ForDotNet.Common/"]
RUN dotnet restore "ForDotNet.Auth/ForDotNet.Auth.csproj"
COPY . .
WORKDIR "/src/ForDotNet.Auth"
RUN dotnet build "ForDotNet.Auth.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ForDotNet.Auth.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ForDotNet.Auth.dll"]