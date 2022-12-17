using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Requests
{
    public record FetchAssetStatisticsRequest
    {
        public IEnumerable<string> Symbols { get; set; }
    }
}