using System;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Accounts.Commands
{
    [Message("Accounts", "Account")]
    public record DeleteAccountCommand : ICommand
    {
        public Guid Id { get; }
        
        public DeleteAccountCommand(Guid id)
        {
            Id = id;
        }
    }
}