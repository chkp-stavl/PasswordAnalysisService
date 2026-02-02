# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy everything (solution + all projects)
COPY . .

# Move into API project
WORKDIR /src/PasswordAnalysisService.Api

# Restore & publish
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000
ENTRYPOINT ["dotnet", "PasswordAnalysisService.Api.dll"]
