using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Orders.Commands;

namespace FinancialPortfolio.APIGateway.Contracts.Integrations.Models
{
    public class IntegrationCommands
    {
        public IntegrateOrdersCommand IntegrateOrdersCommand { get; set; }
        
        public IntegrateTransfersCommand IntegrateTransfersCommand { get; set; }
    }
}