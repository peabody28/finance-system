#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["payment/payment.csproj", "payment/"]
RUN dotnet restore -s "http://nuget.peabody28.com/v3/index.json" -s "https://api.nuget.org/v3/index.json" "payment/payment.csproj"
COPY . .
WORKDIR "/src/payment"
RUN dotnet build "payment.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "payment.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "payment.dll"]