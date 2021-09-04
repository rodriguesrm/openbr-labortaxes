#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["OpenBr.LaborTaxes.Web.Api/OpenBr.LaborTaxes.Web.Api.csproj", "OpenBr.LaborTaxes.Web.Api/"]
COPY ["OpenBr.LaborTaxes.Business/OpenBr.LaborTaxes.Business.csproj", "OpenBr.LaborTaxes.Business/"]
COPY ["OpenBr.LaborTaxes.Contract/OpenBr.LaborTaxes.Contract.csproj", "OpenBr.LaborTaxes.Contract/"]
COPY [".nuget/nuget.config", ""]
RUN dotnet restore "OpenBr.LaborTaxes.Web.Api/OpenBr.LaborTaxes.Web.Api.csproj"
COPY . .
WORKDIR "/src/OpenBr.LaborTaxes.Web.Api"
RUN dotnet build "OpenBr.LaborTaxes.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenBr.LaborTaxes.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenBr.LaborTaxes.Web.Api.dll"]