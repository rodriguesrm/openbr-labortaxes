#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["OpenBr.Inss.Web.Api/OpenBr.Inss.Web.Api.csproj", "OpenBr.Inss.Web.Api/"]
COPY ["OpenBr.Inss.Business/OpenBr.Inss.Business.csproj", "OpenBr.Inss.Business/"]
RUN dotnet restore "OpenBr.Inss.Web.Api/OpenBr.Inss.Web.Api.csproj"
COPY . .
WORKDIR "/src/OpenBr.Inss.Web.Api"
RUN dotnet build "OpenBr.Inss.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OpenBr.Inss.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenBr.Inss.Web.Api.dll"]