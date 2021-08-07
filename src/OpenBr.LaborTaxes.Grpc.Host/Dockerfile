#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OpenBr.LaborTaxes.Grpc.Host/OpenBr.LaborTaxes.Grpc.Host.csproj", "OpenBr.LaborTaxes.Grpc.Host/"]
COPY ["OpenBr.LaborTaxes.Business/OpenBr.LaborTaxes.Business.csproj", "OpenBr.LaborTaxes.Business/"]
COPY ["OpenBr.LaborTaxes.Contract/OpenBr.LaborTaxes.Contract.csproj", "OpenBr.LaborTaxes.Contract/"]
RUN dotnet restore "OpenBr.LaborTaxes.Grpc.Host/OpenBr.LaborTaxes.Grpc.Host.csproj"
COPY . .
WORKDIR "/src/OpenBr.LaborTaxes.Grpc.Host"
RUN dotnet build "OpenBr.LaborTaxes.Grpc.Host.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenBr.LaborTaxes.Grpc.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenBr.LaborTaxes.Grpc.Host.dll"]