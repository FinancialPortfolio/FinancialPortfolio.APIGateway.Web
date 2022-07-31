using System;
using FinancialPortfolio.APIGateway.Contracts.Equity.Enums;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Commands
{
    [Message("Equity", "Transfer")]
    public record CreateTransferCommand : ICommand
    {
        public decimal Amount { get; }
        
        public TransferType Type { get; }
        
        public DateTime? DateTime { get; }
        
        public Guid AccountId { get; }

        public CreateTransferCommand(decimal amount, TransferType type, DateTime? dateTime, Guid accountId)
        {
            Amount = amount;
            Type = type;
            DateTime = dateTime;
            AccountId = accountId;
        }
    }
}