FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FinancialPortfolio.APIGateway.Web/FinancialPortfolio.APIGateway.Web/FinancialPortfolio.APIGateway.Web.csproj", "FinancialPortfolio.APIGateway.Web/"]
COPY ["FinancialPortfolio.APIGateway.Web/FinancialPortfolio.APIGateway.Contracts/FinancialPortfolio.APIGateway.Contracts.csproj", "FinancialPortfolio.APIGateway.Contracts/"]
COPY ["NugetPackages", "LocalNugetPackages"]
RUN dotnet restore -s LocalNugetPackages -s https://api.nuget.org/v3/index.json "FinancialPortfolio.APIGateway.Web/FinancialPortfolio.APIGateway.Web.csproj"
COPY ./FinancialPortfolio.APIGateway.Web .
WORKDIR "/src/FinancialPortfolio.APIGateway.Web"
RUN dotnet build "FinancialPortfolio.APIGateway.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FinancialPortfolio.APIGateway.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialPortfolio.APIGateway.Web.dll"]
