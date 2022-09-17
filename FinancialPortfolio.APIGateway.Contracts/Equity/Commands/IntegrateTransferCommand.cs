using System;
using FinancialPortfolio.APIGateway.Contracts.Equity.Enums;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Commands
{
    public record IntegrateTransferCommand
    {
        public decimal Amount { get; }
        
        public TransferType Type { get; }
        
        public DateTime DateTime { get; }

        public IntegrateTransferCommand(decimal amount, TransferType type, DateTime dateTime)
        {
            Amount = amount;
            Type = type;
            DateTime = dateTime;
        }
    }
}