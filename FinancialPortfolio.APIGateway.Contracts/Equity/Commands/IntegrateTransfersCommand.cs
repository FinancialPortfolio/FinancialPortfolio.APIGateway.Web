using System;
using System.Collections.Generic;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Equity.Commands
{
    [Message("Equity", "Transfer")]
    public record IntegrateTransfersCommand : ICommand
    {
        public Guid AccountId { get; }
        
        public IEnumerable<IntegrateTransferCommand> Transfers { get; }

        public IntegrateTransfersCommand(Guid accountId, IEnumerable<IntegrateTransferCommand> transfers)
        {
            AccountId = accountId;
            Transfers = transfers;
        }
    }
}