# Etapa base para rodar o aplicativo
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

# Etapa para build do projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DockerApiStudy.csproj", "."]
RUN dotnet restore "./DockerApiStudy.csproj"
COPY . .
RUN dotnet build "./DockerApiStudy.csproj" -c ${BUILD_CONFIGURATION} --no-restore -o /app/build

# Etapa para publicar o projeto
FROM build AS publish
RUN dotnet publish "./DockerApiStudy.csproj" -c ${BUILD_CONFIGURATION} --no-restore -o /app/publish /p:UseAppHost=false

# Etapa final para rodar o aplicativo
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DockerApiStudy.dll"]
