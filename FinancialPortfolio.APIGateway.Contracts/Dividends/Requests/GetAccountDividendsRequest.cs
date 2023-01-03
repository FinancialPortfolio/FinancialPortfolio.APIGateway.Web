using System;
using System.Collections.Generic;

namespace FinancialPortfolio.APIGateway.Contracts.Dividends.Requests
{
    public record GetAccountDividendsRequest
    {
        public List<Guid> AccountIds { get; set; }
        
        public DateTime StartDateTime { get; set; }
        
        public DateTime EndDateTime { get; set; }
    }
}