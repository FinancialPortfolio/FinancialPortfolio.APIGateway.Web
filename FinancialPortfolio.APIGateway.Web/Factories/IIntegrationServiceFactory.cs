using FinancialPortfolio.APIGateway.Contracts.Integrations.Enums;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Services;

namespace FinancialPortfolio.APIGateway.Web.Factories
{
    public interface IIntegrationServiceFactory
    {
        IIntegrationService CreateIntegrationService(IntegrationSource source);
    }
}