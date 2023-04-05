#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["payment.system.custompay.worker/payment.system.custompay.worker.csproj", "payment.system.custompay.worker/"]
RUN dotnet restore "payment.system.custompay.worker/payment.system.custompay.worker.csproj"
COPY . .
WORKDIR "/src/payment.system.custompay.worker"
RUN dotnet build "payment.system.custompay.worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "payment.system.custompay.worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "payment.system.custompay.worker.dll"]