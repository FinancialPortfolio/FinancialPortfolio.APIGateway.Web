using FinancialPortfolio.APIGateway.Contracts.Integrations.Enums;
using Microsoft.AspNetCore.Http;

namespace FinancialPortfolio.APIGateway.Contracts.Integrations.Requests
{
    public record IntegrateRequest
    {
        public IntegrationSource Source { get; set; }
        
        public IFormFile File { get; set; }
    }
}