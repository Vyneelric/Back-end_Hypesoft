# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files and restore
COPY ["Hypersoft.API/Hypersoft.API.csproj", "Hypersoft.API/"]
COPY ["Hypersoft.Application/Hypersoft.Application.csproj", "Hypersoft.Application/"]
COPY ["Hypersoft.Domain/Hypersoft.Domain.csproj", "Hypersoft.Domain/"]
COPY ["Hypersoft.Infrastructure/Hypersoft.Infrastructure.csproj", "Hypersoft.Infrastructure/"]
RUN dotnet restore "Hypersoft.API/Hypersoft.API.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Hypersoft.API"
RUN dotnet build "Hypersoft.API.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "Hypersoft.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hypersoft.API.dll"]
