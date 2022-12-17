using FinancialPortfolio.APIGateway.Contracts.Assets.Requests;
using FluentValidation;

namespace FinancialPortfolio.APIGateway.Contracts.Assets.Validation
{
    public class FetchAssetStatisticsRequestValidation : AbstractValidator<FetchAssetStatisticsRequest>
    {
        public FetchAssetStatisticsRequestValidation()
        {
            RuleFor(x => x.Ids).NotNull();
        }
    }
}