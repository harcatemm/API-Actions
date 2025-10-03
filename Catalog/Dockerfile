# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copia sln y csproj (ajusta rutas)
# COPY ["Solution.sln", "./"]
COPY ["Catalog.csproj", "./"]
RUN dotnet restore "Catalog.csproj"

COPY . .
WORKDIR /src
RUN dotnet publish -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Catalog.dll"]
