using FinancialPortfolio.APIGateway.Contracts.Integrations.Models;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Services;

namespace FinancialPortfolio.APIGateway.Web.Factories
{
    public interface IIntegrationServiceFactory
    {
        IIntegrationService CreateIntegrationService(IntegrationSource source);
    }
}