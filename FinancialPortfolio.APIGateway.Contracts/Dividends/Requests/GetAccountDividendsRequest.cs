using System;

namespace FinancialPortfolio.APIGateway.Contracts.Dividends.Requests
{
    public record GetAccountDividendsRequest
    {
        public DateTime StartDateTime { get; set; }
        
        public DateTime EndDateTime { get; set; }
        
        public Guid? AssetId { get; set; }
    }
}