# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Backend/Evolutio.API/Evolutio.API.csproj", "src/Backend/Evolutio.API/"]
COPY ["src/Shared/Evolutio.Communication/Evolutio.Communication.csproj", "src/Shared/Evolutio.Communication/"]
COPY ["src/Shared/Evolutio.Exception/Evolutio.Exception.csproj", "src/Shared/Evolutio.Exception/"]
COPY ["src/Backend/Evolutio.Application/Evolutio.Application.csproj", "src/Backend/Evolutio.Application/"]
COPY ["src/Backend/Evolutio.Domain/Evolutio.Domain/Evolutio.Domain.csproj", "src/Backend/Evolutio.Domain/Evolutio.Domain/"]
COPY ["src/Backend/Evolutio.Infrastructure/Evolutio.Infrastructure.csproj", "src/Backend/Evolutio.Infrastructure/"]
RUN dotnet restore "./src/Backend/Evolutio.API/Evolutio.API.csproj"
COPY . .
WORKDIR "/src/src/Backend/Evolutio.API"
RUN dotnet build "./Evolutio.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Evolutio.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Evolutio.API.dll"]