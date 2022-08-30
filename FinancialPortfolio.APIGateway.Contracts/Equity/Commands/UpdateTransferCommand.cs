using System;
using FinancialPortfolio.APIGateway.Contracts.Equity.Enums;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Commands
{
    [Message("Equity", "Transfer")]
    public record UpdateTransferCommand : ICommand
    {
        public Guid Id { get; }
        
        public decimal Amount { get; }
        
        public TransferType Type { get; }
        
        public DateTime DateTime { get; }
        
        public Guid AccountId { get; }

        public UpdateTransferCommand(Guid id, decimal amount, TransferType type, DateTime dateTime, Guid accountId)
        {
            Id = id;
            Amount = amount;
            Type = type;
            DateTime = dateTime;
            AccountId = accountId;
        }
    }
}