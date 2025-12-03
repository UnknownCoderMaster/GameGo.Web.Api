# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files (to'g'ri path - src/ yo'q!)
COPY GameGo.Api/GameGo.Api.csproj GameGo.Api/
COPY GameGo.Application/GameGo.Application.csproj GameGo.Application/
COPY GameGo.Domain/GameGo.Domain.csproj GameGo.Domain/
COPY GameGo.Infrastructure/GameGo.Infrastructure.csproj GameGo.Infrastructure/
COPY GameGo.Shared/GameGo.Shared.csproj GameGo.Shared/

# Restore
RUN dotnet restore GameGo.Api/GameGo.Api.csproj

# Copy everything
COPY . .

# Build
WORKDIR /src/GameGo.Api
RUN dotnet build GameGo.Api.csproj -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish GameGo.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080

# Environment
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Start
ENTRYPOINT ["dotnet", "GameGo.Api.dll"]