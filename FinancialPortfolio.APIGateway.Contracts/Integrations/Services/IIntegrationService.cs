using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialPortfolio.APIGateway.Contracts.Equity.Commands;
using FinancialPortfolio.APIGateway.Contracts.Integrations.Requests;
using FinancialPortfolio.APIGateway.Contracts.Orders.Commands;

namespace FinancialPortfolio.APIGateway.Contracts.Integrations.Services
{
    public interface IIntegrationService
    {
        public Task<IEnumerable<IntegrateOrderCommand>> ParseOrdersAsync(IntegrateRequest request);
        
        public Task<IEnumerable<IntegrateTransferCommand>> ParseTransfersAsync(IntegrateRequest request);
    }
}