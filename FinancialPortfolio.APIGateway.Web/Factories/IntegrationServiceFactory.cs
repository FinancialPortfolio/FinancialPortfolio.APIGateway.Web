using System;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Models;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Services;
using AdmiralMarketsIntegrationService = FinancialPortfolio.APIGateway.Integrations.AdmiralMarkets.IntegrationService;
using InteractiveBrokersIntegrationService = FinancialPortfolio.APIGateway.Integrations.InteractiveBrokers.IntegrationService;

namespace FinancialPortfolio.APIGateway.Web.Factories
{
    public class IntegrationServiceFactory : IIntegrationServiceFactory
    {
        public IIntegrationService CreateIntegrationService(IntegrationSource source)
        {
            return source switch
            {
                IntegrationSource.AdmiralMarkets => new AdmiralMarketsIntegrationService(),
                IntegrationSource.InteractiveBrokers => new InteractiveBrokersIntegrationService(),
                _ => throw new InvalidOperationException($"Unsupported integration source: {source}")
            };
        }
    }
}