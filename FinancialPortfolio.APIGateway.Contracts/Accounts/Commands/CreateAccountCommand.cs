using System;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging;

namespace FinancialPortfolio.APIGateway.Contracts.Accounts.Commands
{
    [Message("Accounts", "Account")]
    public record CreateAccountCommand : ICommand
    {
        public string Name { get; }
        
        public string Description { get; }

        public Guid UserId { get; }

        public CreateAccountCommand(string name, string description, Guid userId)
        {
            Name = name;
            Description = description;
            UserId = userId;
        }
    }
}