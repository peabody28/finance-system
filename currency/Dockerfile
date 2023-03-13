#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["currency/currency.csproj", "currency/"]
RUN dotnet restore -s "http://nuget.peabody28.com/v3/index.json" -s "https://api.nuget.org/v3/index.json" "currency/currency.csproj"
COPY . .
WORKDIR "/src/currency"
RUN dotnet build "currency.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "currency.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "currency.dll"]