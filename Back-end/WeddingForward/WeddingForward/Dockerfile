#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build

# Install NodeJs
RUN apt-get update && \
apt-get install -y wget && \
apt-get install -y gnupg2 && \
wget -qO- https://deb.nodesource.com/setup_12.x | bash - && \
apt-get install -y build-essential nodejs
# End Install

WORKDIR /src
COPY ["WeddingForward/WeddingForward.Api.csproj", "WeddingForward/"]
COPY ["WeddingForward.ApplicationServices/WeddingForward.ApplicationServices.csproj", "WeddingForward.ApplicationServices/"]
COPY ["WeddingForward.Data/WeddingForward.Data.csproj", "WeddingForward.Data/"]
RUN dotnet restore "WeddingForward/WeddingForward.Api.csproj"
COPY . .
WORKDIR "/src/WeddingForward"
RUN dotnet build "WeddingForward.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeddingForward.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeddingForward.Api.dll"]