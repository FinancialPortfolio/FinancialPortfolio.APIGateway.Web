using System;
using FinancialPortfolio.Messaging;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Commands
{
    [Message("Equity", "Transfer")]
    public class CreateTransfer
    {
        public decimal Amount { get; private set; }
        
        public TransferType Type { get; private set; }
        
        public DateTime? DateTime { get; private set; }

        public CreateTransfer(decimal amount, TransferType type, DateTime? dateTime)
        {
            Amount = amount;
            Type = type;
            DateTime = dateTime;
        }
    }
}