# -------- BUILD STAGE --------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish

# -------- RUNTIME STAGE --------
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# IMPORTANT: Do NOT hardcode port
ENV ASPNETCORE_URLS=http://0.0.0.0:10000

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "GullyhiveBackend.dll"]
