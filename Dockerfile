# === Build Stage ===
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY TravelList.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# === Runtime Stage ===
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# 建立資料庫存放目錄
RUN mkdir -p /data

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "TravelList.dll"]
