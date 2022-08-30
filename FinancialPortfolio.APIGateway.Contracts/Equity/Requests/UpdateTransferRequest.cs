using System;
using FinancialPortfolio.APIGateway.Contracts.Equity.Enums;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Requests
{
    public record UpdateTransferRequest
    {
        public decimal Amount { get; set; }
        
        public TransferType Type { get; set; }
        
        public DateTime DateTime { get; set; }
    }
}