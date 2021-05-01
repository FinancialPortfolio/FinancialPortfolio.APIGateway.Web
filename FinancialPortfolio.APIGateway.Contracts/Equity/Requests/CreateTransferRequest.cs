using System;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Requests
{
    public class CreateTransferRequest
    {
        public decimal Amount { get; set; }
        
        public TransferType Type { get; set; }
        
        public DateTime? DateTime { get; set; }
    }
}