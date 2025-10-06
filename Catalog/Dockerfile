# Catalog/Dockerfile — opción cacheada / optimizada
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo los csproj (ajusta nombres y rutas EXACTAS según tu repo)
COPY ["Solution.sln", "./"]
COPY ["Catalog/Catalog.csproj", "Catalog/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infraestructure/Infraestructure.csproj", "Infraestructure/"]
COPY ["APP/APP.csproj", "APP/"]               
# añade más COPY para otros proyectos si existen

# Restaurar (cacheable)
RUN dotnet restore "Catalog/Catalog.csproj"

# Copiar el resto del código
COPY . .

# Publicar
WORKDIR /src/Catalog
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Catalog.dll"]
