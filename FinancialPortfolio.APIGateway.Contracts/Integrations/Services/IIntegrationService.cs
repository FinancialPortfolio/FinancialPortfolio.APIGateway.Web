using FinancialPortfolio.APIGateway.Contracts.Integrations.Models;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Requests;

namespace FinancialPortfolio.APIGateway.Contracts.Integrations.Services
{
    public interface IIntegrationService
    {
        public IntegrationCommands Parse(IntegrateRequest request);
    }
}