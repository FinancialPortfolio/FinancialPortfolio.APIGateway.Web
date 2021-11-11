using FinancialPortfolio.APIGateway.Contracts.Assets.Enums;
using FinancialPortfolio.CQRS.Commands;
using FinancialPortfolio.Messaging;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Commands
{
    [Message("Assets", "Asset")]
    public record CreateAssetCommand : ICommand
    {
        public string Symbol { get; }
        
        public string Name { get; }
        
        public AssetType Type { get; }

        public CreateAssetCommand(string symbol, string name, AssetType type)
        {
            Symbol = symbol;
            Name = name;
            Type = type;
        }
    }
}