using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Requests
{
    public record FetchStockStatisticsRequest
    {
        public IEnumerable<string> Symbols { get; set; }
    }
}