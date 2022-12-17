using System;
using System.Collections.Generic;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging.Attributes;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Commands
{
    [Message("Assets", "Asset")]
    public record FetchAssetStatisticsCommand : ICommand
    {
        public List<Guid> Ids { get; }

        public FetchAssetStatisticsCommand(List<Guid> ids)
        {
            Ids = ids;
        }
    }
}