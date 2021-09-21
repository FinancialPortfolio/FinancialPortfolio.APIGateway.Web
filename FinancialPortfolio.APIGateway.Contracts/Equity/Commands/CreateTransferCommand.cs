using System;
using FinancialPortfolio.APIGateway.Contracts.Equity.Enums;
using FinancialPortfolio.CQRS.Models;
using FinancialPortfolio.Messaging;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Commands
{
    [Message("Equity", "Transfer")]
    public record CreateTransferCommand : ICommand
    {
        public decimal Amount { get; }
        
        public TransferType Type { get; }
        
        public DateTime? DateTime { get; }

        public CreateTransferCommand(decimal amount, TransferType type, DateTime? dateTime)
        {
            Amount = amount;
            Type = type;
            DateTime = dateTime;
        }
    }
}