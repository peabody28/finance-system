#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["payment.worker/payment.worker.csproj", "payment.worker/"]
RUN dotnet restore -s "http://nuget.peabody28.com/v3/index.json" -s "https://api.nuget.org/v3/index.json" "payment.worker/payment.worker.csproj"
COPY . .
WORKDIR "/src/payment.worker"
RUN dotnet build "payment.worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "payment.worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "payment.worker.dll"]