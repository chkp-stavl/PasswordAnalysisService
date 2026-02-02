# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything in the repo
COPY . .

# Move into API project directory
WORKDIR /src/PasswordAnalysisService.Api

# Restore & publish the API project explicitly
RUN dotnet restore PasswordAnalysisService.Api.csproj
RUN dotnet publish PasswordAnalysisService.Api.csproj -c Release -o /app/publish

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000
ENTRYPOINT ["dotnet", "PasswordAnalysisService.Api.dll"]
