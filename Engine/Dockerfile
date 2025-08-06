# --- Stage 1: Build a aplicação ---
# Usa uma imagem do SDK do .NET para compilar o projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Define o diretório de trabalho
WORKDIR /app

# Copia o arquivo .csproj e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código do projeto
COPY . .

# Compila o projeto em modo de Release
RUN dotnet publish -c Release -o /app/publish

# --- Stage 2: Publicação e Execução ---
# Usa uma imagem de runtime do .NET sem o SDK, mas a mais leve possível
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final

# Define o diretório de trabalho
WORKDIR /app

# Copia os arquivos da etapa de build para o diretório de trabalho
COPY --from=build /app/publish .

# Define a porta que o container irá escutar (configurado no docker-compose)
EXPOSE 80

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "Engine.dll"]