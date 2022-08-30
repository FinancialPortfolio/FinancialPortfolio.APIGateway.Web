using System;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Commands
{
    [Message("Equity", "Transfer")]
    public record DeleteTransferCommand : ICommand
    {
        public Guid Id { get; }
        
        public Guid AccountId { get; }

        public DeleteTransferCommand(Guid id, Guid accountId)
        {
            Id = id;
            AccountId = accountId;
        }
    }
}