using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Integrations.Responses
{
    public record IntegrationFileValidationResponse
    {
        public IEnumerable<InvalidOrderResponse> InvalidOrders { get; }

        public IntegrationFileValidationResponse(IEnumerable<InvalidOrderResponse> invalidOrders)
        {
            InvalidOrders = invalidOrders;
        }
    }
}