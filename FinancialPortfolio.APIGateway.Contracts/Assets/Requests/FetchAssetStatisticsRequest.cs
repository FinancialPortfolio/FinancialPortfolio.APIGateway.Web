using System;
using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Requests
{
    public record FetchAssetStatisticsRequest
    {
        public List<Guid> Ids { get; set; }
    }
}