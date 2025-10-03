# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todos los .csproj para que las dependencias estén disponibles
COPY Catalog/Catalog.csproj Catalog/
COPY Domain/Domain.csproj Domain/
COPY APP/APP.csproj APP/
COPY Infraestructure/Infraestructure.csproj Infraestructure/

# Restaurar
RUN dotnet restore Catalog/Catalog.csproj

# Copiar todo el código
COPY . .

# Compilar Catalog
WORKDIR /src/Catalog
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Catalog.dll"]
