# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo el csproj y restaurar
COPY CucharitaLeliQR/CucharitaLeliQR.csproj CucharitaLeliQR/
RUN dotnet restore CucharitaLeliQR/CucharitaLeliQR.csproj

# Copiar el resto del proyecto
COPY CucharitaLeliQR/. CucharitaLeliQR/
WORKDIR /src/CucharitaLeliQR

# Publicar
RUN dotnet publish CucharitaLeliQR.csproj -c Release -o /app/publish

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CucharitaLeliQR.dll"]