# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /

# Copy csproj files
COPY ["/GameGo.Api/GameGo.Api.csproj", "GameGo.Api/"]
COPY ["/GameGo.Application/GameGo.Application.csproj", "GameGo.Application/"]
COPY ["/GameGo.Domain/GameGo.Domain.csproj", "GameGo.Domain/"]
COPY ["/GameGo.Infrastructure/GameGo.Infrastructure.csproj", "GameGo.Infrastructure/"]
COPY ["/GameGo.Shared/GameGo.Shared.csproj", "GameGo.Shared/"]

# Restore
RUN dotnet restore "GameGo.Api/GameGo.Api.csproj"

# Copy everything else
COPY / .

# Build
WORKDIR "/GameGo.Api"
RUN dotnet build "GameGo.Api.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "GameGo.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copy published files
COPY --from=publish /app/publish .

# Expose port
EXPOSE 8080

# Set environment
ENV ASPNETCORE_URLS=http://+:8080

# Start application
ENTRYPOINT ["dotnet", "GameGo.Api.dll"]