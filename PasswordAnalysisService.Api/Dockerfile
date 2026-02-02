# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything
COPY . .

# Restore dependencies (explicit project path)
RUN dotnet restore PasswordAnalysisService.Api/PasswordAnalysisService.Api.csproj

# Build & publish
RUN dotnet publish PasswordAnalysisService.Api/PasswordAnalysisService.Api.csproj \
    -c Release \
    -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "PasswordAnalysisService.Api.dll"]
